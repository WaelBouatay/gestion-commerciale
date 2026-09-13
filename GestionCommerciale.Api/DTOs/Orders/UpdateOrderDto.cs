namespace GestionCommerciale.Api.DTOs.Orders
{
    public class UpdateOrderDto
    {
        public int ClientId { get; set; }
        public List<CreateOrderLineDto> Lignes { get; set; } = new();
    }
}