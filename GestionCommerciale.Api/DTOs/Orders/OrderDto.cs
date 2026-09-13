using GestionCommerciale.Api.Models;

namespace GestionCommerciale.Api.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string NumeroCommande { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string ClientNom { get; set; } = string.Empty;
        public DateTime DateCommande { get; set; }
        public OrderStatus Statut { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TotalTTC { get; set; }
        public List<OrderLineDto> Lignes { get; set; } = new();
    }
}