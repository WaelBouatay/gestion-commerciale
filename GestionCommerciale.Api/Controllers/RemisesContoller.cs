using GestionCommerciale.Api.DTOs.Remises;
using GestionCommerciale.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCommerciale.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemisesController : ControllerBase
    {
        private readonly IRemiseService _remiseService;

        public RemisesController(IRemiseService remiseService)
        {
            _remiseService = remiseService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RemiseDto>>> GetAll()
        {
            return Ok(await _remiseService.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<RemiseDto>> Create(CreateRemiseDto dto)
        {
            var created = await _remiseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRemiseDto dto)
        {
            var success = await _remiseService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _remiseService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}