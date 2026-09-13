using GestionCommerciale.Api.DTOs.Clients;
using GestionCommerciale.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCommerciale.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // => "api/clients" (le nom de la classe sans "Controller")
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET /api/clients
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        // GET /api/clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound(new { message = $"Client avec l'id {id} introuvable." });

            return Ok(client);
        }

        // POST /api/clients
        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create(CreateClientDto dto)
        {
            var created = await _clientService.CreateAsync(dto);
            // 201 Created + l'URL pour récupérer la ressource créée (bonne pratique REST)
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateClientDto dto)
        {
            var success = await _clientService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = $"Client avec l'id {id} introuvable." });

            return NoContent();
        }

        // DELETE /api/clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _clientService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Client avec l'id {id} introuvable." });

            return NoContent();
        }
    }
}