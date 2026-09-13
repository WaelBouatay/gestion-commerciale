using GestionCommerciale.Api.DTOs.Orders;

namespace GestionCommerciale.Api.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task<bool> UpdateAsync(int id, UpdateOrderDto dto);
        Task<bool> DeleteAsync(int id);
        Task<OrderDto> ValidateAsync(int id);
    }
}