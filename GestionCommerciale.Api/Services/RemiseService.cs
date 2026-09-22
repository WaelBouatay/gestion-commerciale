using GestionCommerciale.Api.Data;
using GestionCommerciale.Api.DTOs.Remises;
using GestionCommerciale.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionCommerciale.Api.Services
{
    public class RemiseService : IRemiseService
    {
        private readonly AppDbContext _context;

        public RemiseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RemiseDto>> GetAllAsync()
        {
            return await _context.Remises.Select(r => MapToDto(r)).ToListAsync();
        }

        public async Task<RemiseDto> CreateAsync(CreateRemiseDto dto)
        {
            var remise = new Remise
            {
                Libelle = dto.Libelle,
                Type = dto.Type,
                Valeur = dto.Valeur,
                Active = dto.Active
            };

            _context.Remises.Add(remise);
            await _context.SaveChangesAsync();
            return MapToDto(remise);
        }

        public async Task<bool> UpdateAsync(int id, UpdateRemiseDto dto)
        {
            var remise = await _context.Remises.FindAsync(id);
            if (remise == null) return false;

            remise.Libelle = dto.Libelle;
            remise.Type = dto.Type;
            remise.Valeur = dto.Valeur;
            remise.Active = dto.Active;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var remise = await _context.Remises.FindAsync(id);
            if (remise == null) return false;

            _context.Remises.Remove(remise);
            await _context.SaveChangesAsync();
            return true;
        }

        private static RemiseDto MapToDto(Remise remise)
        {
            return new RemiseDto
            {
                Id = remise.Id,
                Libelle = remise.Libelle,
                Type = remise.Type,
                Valeur = remise.Valeur,
                Active = remise.Active
            };
        }
    }
}