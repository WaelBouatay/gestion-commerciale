using Microsoft.EntityFrameworkCore;
using GestionCommerciale.Api.Models;

namespace GestionCommerciale.Api.Data
{
    public class AppDbContext : DbContext
    {
        // Le constructeur reçoit les options (chaîne de connexion ...)
        // via l'injection de dépendances configurée dans Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Chaque DbSet correspond à une table
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Configuration de Client.Adresses (List<string> -> JSON) ---
              modelBuilder.Entity<Client>()
                .Property(c => c.Adresses)
                .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>()
        )
                .Metadata.SetValueComparer(
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                (a, b) => (a ?? new List<string>()).SequenceEqual(b ?? new List<string>()),
                v => v.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
                v => v.ToList()
        )
    );

            // --- Précision décimale pour les montants (évite les avertissements EF Core) ---
            modelBuilder.Entity<Product>()
                .Property(p => p.PrixUnitaireHT)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalHT)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalTTC)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderLine>()
                .Property(ol => ol.PrixUnitaire)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderLine>()
                .Property(ol => ol.TotalLigne)
                .HasPrecision(18, 2);

            // --- Relations ---

            // Client (1) -> (N) Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict); // empêche de supprimer un client qui a des commandes

            // Order (1) -> (N) OrderLine
            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // si on supprime une commande, ses lignes sont supprimées

            // Product (1) -> (N) OrderLine
            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Product)
                .WithMany()
                .HasForeignKey(ol => ol.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // empêche de supprimer un produit déjà commandé
        }
    }
}