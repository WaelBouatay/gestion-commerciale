namespace GestionCommerciale.Api.Models
{
    public enum OrderStatus
    {
        Brouillon,
        Validee,
        Annulee
    }

    public class Order
    {
        public int Id { get; set; }

        public string NumeroCommande { get; set; } = string.Empty;

        // Clé étrangère vers Client
        public int ClientId { get; set; }

        // Propriété de navigation vers le Client associé
        public Client? Client { get; set; }

        public DateTime DateCommande { get; set; } = DateTime.UtcNow;

        public OrderStatus Statut { get; set; } = OrderStatus.Brouillon;

        public decimal TotalHT { get; set; }

        public decimal TotalTTC { get; set; }

        // Une commande contient plusieurs lignes
        public List<OrderLine> OrderLines { get; set; } = new();
    }
}