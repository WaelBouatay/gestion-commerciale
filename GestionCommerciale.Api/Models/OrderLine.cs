namespace GestionCommerciale.Api.Models
{
    public class OrderLine
    {
        public int Id { get; set; }

        // Clé étrangère vers Order
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // Clé étrangère vers Product
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantite { get; set; }

        public decimal PrixUnitaire { get; set; }

        // Total de la ligne = Quantite * PrixUnitaire
        public decimal TotalLigne { get; set; }
    }
}