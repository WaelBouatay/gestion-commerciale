namespace GestionCommerciale.Api.DTOs.Orders
{
    public class CreateOrderDto
    {
        public int ClientId { get; set; }
        public List<CreateOrderLineDto> Lignes { get; set; } = new();

        public List<int> RemiseIds { get; set; } = new();
    }
}