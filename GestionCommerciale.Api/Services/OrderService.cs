using GestionCommerciale.Api.Data;
using GestionCommerciale.Api.DTOs.Orders;
using GestionCommerciale.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionCommerciale.Api.Services
{
    public class OrderService : IOrderService
    {
        private const decimal TAUX_TVA = 0.19m; // 19%, règle de gestion section 4

        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .ToListAsync();

            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? null : MapToDto(order);
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            // Règle : "Il ne doit pas être possible de créer une commande sans client"
            var client = await _context.Clients.FindAsync(dto.ClientId);
            if (client == null)
                throw new BusinessException("Le client spécifié n'existe pas.");

            if (dto.Lignes.Count == 0)
                throw new BusinessException("Une commande doit contenir au moins une ligne.");

            var order = new Order
            {
                ClientId = dto.ClientId,
                NumeroCommande = await GenerateNumeroCommandeAsync(),
                DateCommande = DateTime.UtcNow,
                Statut = OrderStatus.Brouillon
            };

            // On construit chaque ligne en vérifiant les règles métier
            foreach (var ligneDto in dto.Lignes)
            {
                var orderLine = await BuildOrderLineAsync(ligneDto);
                order.OrderLines.Add(orderLine);
            }

            // Calcul des totaux (Total HT = somme des lignes, Total TTC = HT + TVA)
            CalculateTotals(order);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // On recharge avec les relations pour renvoyer un DTO complet
            return (await GetByIdAsync(order.Id))!;
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return false;

            // On ne modifie pas une commande déjà validée ou annulée
            if (order.Statut != OrderStatus.Brouillon)
                throw new BusinessException("Seule une commande en Brouillon peut être modifiée.");

            var client = await _context.Clients.FindAsync(dto.ClientId);
            if (client == null)
                throw new BusinessException("Le client spécifié n'existe pas.");

            if (dto.Lignes.Count == 0)
                throw new BusinessException("Une commande doit contenir au moins une ligne.");

            order.ClientId = dto.ClientId;

            // Stratégie simple : on supprime les anciennes lignes et on recrée les nouvelles
            _context.OrderLines.RemoveRange(order.OrderLines);
            order.OrderLines.Clear();

            foreach (var ligneDto in dto.Lignes)
            {
                var orderLine = await BuildOrderLineAsync(ligneDto);
                order.OrderLines.Add(orderLine);
            }

            CalculateTotals(order);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrderDto> ValidateAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new BusinessException("Commande introuvable.");

            if (order.Statut != OrderStatus.Brouillon)
                throw new BusinessException("Seule une commande en Brouillon peut être validée.");

            // Règle : "Le stock produit doit être mis à jour lorsqu'une commande est validée"
            foreach (var ligne in order.OrderLines)
            {
                var product = ligne.Product!;

                // On revérifie le stock au moment de la validation (il a pu changer depuis la création)
                if (ligne.Quantite > product.QuantiteEnStock)
                    throw new BusinessException(
                        $"Stock insuffisant pour le produit '{product.Nom}'. Disponible : {product.QuantiteEnStock}, demandé : {ligne.Quantite}.");

                product.QuantiteEnStock -= ligne.Quantite;
            }

            order.Statut = OrderStatus.Validee;

            await _context.SaveChangesAsync();

            return (await GetByIdAsync(order.Id))!;
        }

        // ---------- Méthodes privées utilitaires ----------

        private async Task<OrderLine> BuildOrderLineAsync(CreateOrderLineDto ligneDto)
        {
            // Règle : "quantité inférieure ou égale à zéro" interdite
            if (ligneDto.Quantite <= 0)
                throw new BusinessException("La quantité doit être supérieure à zéro.");

            var product = await _context.Products.FindAsync(ligneDto.ProductId);
            if (product == null)
                throw new BusinessException($"Le produit avec l'id {ligneDto.ProductId} n'existe pas.");

            // Règle : "pas possible de commander une quantité supérieure au stock disponible"
            if (ligneDto.Quantite > product.QuantiteEnStock)
                throw new BusinessException(
                    $"Stock insuffisant pour le produit '{product.Nom}'. Disponible : {product.QuantiteEnStock}, demandé : {ligneDto.Quantite}.");

            // On fige le prix actuel du produit sur la ligne (voir explication étape 2)
            return new OrderLine
            {
                ProductId = product.Id,
                Quantite = ligneDto.Quantite,
                PrixUnitaire = product.PrixUnitaireHT,
                TotalLigne = ligneDto.Quantite * product.PrixUnitaireHT
            };
        }

        private static void CalculateTotals(Order order)
        {
            order.TotalHT = order.OrderLines.Sum(l => l.TotalLigne);
            order.TotalTTC = Math.Round(order.TotalHT * (1 + TAUX_TVA), 2);
        }

        private async Task<string> GenerateNumeroCommandeAsync()
        {
            // Génère un numéro simple du type CMD-2026-00001
            var year = DateTime.UtcNow.Year;
            var count = await _context.Orders.CountAsync(o => o.DateCommande.Year == year);
            return $"CMD-{year}-{(count + 1):D5}";
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                NumeroCommande = order.NumeroCommande,
                ClientId = order.ClientId,
                ClientNom = order.Client != null
                    ? $"{order.Client.Nom} {order.Client.PrenomOuRaisonSociale}"
                    : string.Empty,
                DateCommande = order.DateCommande,
                Statut = order.Statut,
                TotalHT = order.TotalHT,
                TotalTTC = order.TotalTTC,
                Lignes = order.OrderLines.Select(l => new OrderLineDto
                {
                    Id = l.Id,
                    ProductId = l.ProductId,
                    ProductNom = l.Product?.Nom ?? string.Empty,
                    Quantite = l.Quantite,
                    PrixUnitaire = l.PrixUnitaire,
                    TotalLigne = l.TotalLigne
                }).ToList()
            };
        }
    }
}