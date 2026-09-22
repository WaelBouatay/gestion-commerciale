using GestionCommerciale.Api.DTOs.Taxes;

namespace GestionCommerciale.Api.Services
{
    public interface ITaxeService
    {
        Task<List<TaxeDto>> GetAllAsync();
        Task<TaxeDto> CreateAsync(CreateTaxeDto dto);
        Task<bool> UpdateAsync(int id, UpdateTaxeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}