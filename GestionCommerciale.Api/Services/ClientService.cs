using GestionCommerciale.Api.Data;
using GestionCommerciale.Api.DTOs.Clients;
using GestionCommerciale.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionCommerciale.Api.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;

        // Injection de dépendances : .NET fournit automatiquement le AppDbContext
        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            return await _context.Clients
                .Select(c => MapToDto(c))
                .ToListAsync();
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            return client == null ? null : MapToDto(client);
        }

        public async Task<ClientDto> CreateAsync(CreateClientDto dto)
        {
            var client = new Client
            {
                Nom = dto.Nom,
                PrenomOuRaisonSociale = dto.PrenomOuRaisonSociale,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Adresses = dto.Adresses,
                DateCreation = DateTime.UtcNow
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return MapToDto(client);
        }

        public async Task<bool> UpdateAsync(int id, UpdateClientDto dto)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;

            client.Nom = dto.Nom;
            client.PrenomOuRaisonSociale = dto.PrenomOuRaisonSociale;
            client.Email = dto.Email;
            client.Telephone = dto.Telephone;
            client.Adresses = dto.Adresses;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        // Méthode privée pour convertir une entité Client en ClientDto
        private static ClientDto MapToDto(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                Nom = client.Nom,
                PrenomOuRaisonSociale = client.PrenomOuRaisonSociale,
                Email = client.Email,
                Telephone = client.Telephone,
                Adresses = client.Adresses,
                DateCreation = client.DateCreation
            };
        }
    }
}