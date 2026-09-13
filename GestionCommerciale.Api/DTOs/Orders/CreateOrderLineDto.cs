namespace GestionCommerciale.Api.DTOs.Orders
{
    public class CreateOrderLineDto
    {
        public int ProductId { get; set; }
        public int Quantite { get; set; }
    }
}