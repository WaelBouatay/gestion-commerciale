using GestionCommerciale.Api.DTOs.Remises;

namespace GestionCommerciale.Api.Services
{
    public interface IRemiseService
    {
        Task<List<RemiseDto>> GetAllAsync();
        Task<RemiseDto> CreateAsync(CreateRemiseDto dto);
        Task<bool> UpdateAsync(int id, UpdateRemiseDto dto);
        Task<bool> DeleteAsync(int id);
    }
}