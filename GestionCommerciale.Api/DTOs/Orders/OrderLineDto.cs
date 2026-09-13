namespace GestionCommerciale.Api.DTOs.Orders
{
    public class OrderLineDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductNom { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal TotalLigne { get; set; }
    }
}