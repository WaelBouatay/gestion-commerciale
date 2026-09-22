using GestionCommerciale.Api.Data;
using GestionCommerciale.Api.DTOs.Taxes;
using GestionCommerciale.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionCommerciale.Api.Services
{
    public class TaxeService : ITaxeService
    {
        private readonly AppDbContext _context;

        public TaxeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaxeDto>> GetAllAsync()
        {
            return await _context.Taxes.Select(t => MapToDto(t)).ToListAsync();
        }

        public async Task<TaxeDto> CreateAsync(CreateTaxeDto dto)
        {
            var taxe = new Taxe
            {
                Libelle = dto.Libelle,
                Type = dto.Type,
                Valeur = dto.Valeur,
                Active = dto.Active
            };

            _context.Taxes.Add(taxe);
            await _context.SaveChangesAsync();
            return MapToDto(taxe);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaxeDto dto)
        {
            var taxe = await _context.Taxes.FindAsync(id);
            if (taxe == null) return false;

            taxe.Libelle = dto.Libelle;
            taxe.Type = dto.Type;
            taxe.Valeur = dto.Valeur;
            taxe.Active = dto.Active;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var taxe = await _context.Taxes.FindAsync(id);
            if (taxe == null) return false;

            _context.Taxes.Remove(taxe);
            await _context.SaveChangesAsync();
            return true;
        }

        private static TaxeDto MapToDto(Taxe taxe)
        {
            return new TaxeDto
            {
                Id = taxe.Id,
                Libelle = taxe.Libelle,
                Type = taxe.Type,
                Valeur = taxe.Valeur,
                Active = taxe.Active
            };
        }
    }
}