using GestionCommerciale.Api.Data;
using GestionCommerciale.Api.DTOs.Products;
using GestionCommerciale.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionCommerciale.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await _context.Products
                .Select(p => MapToDto(p))
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Reference = dto.Reference,
                Nom = dto.Nom,
                Description = dto.Description,
                PrixUnitaireHT = dto.PrixUnitaireHT,
                QuantiteEnStock = dto.QuantiteEnStock,
                DateCreation = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Reference = dto.Reference;
            product.Nom = dto.Nom;
            product.Description = dto.Description;
            product.PrixUnitaireHT = dto.PrixUnitaireHT;
            product.QuantiteEnStock = dto.QuantiteEnStock;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Reference = product.Reference,
                Nom = product.Nom,
                Description = product.Description,
                PrixUnitaireHT = product.PrixUnitaireHT,
                QuantiteEnStock = product.QuantiteEnStock,
                DateCreation = product.DateCreation
            };
        }
    }
}