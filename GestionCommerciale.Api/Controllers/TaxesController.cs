using GestionCommerciale.Api.DTOs.Taxes;
using GestionCommerciale.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionCommerciale.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxesController : ControllerBase
    {
        private readonly ITaxeService _taxeService;

        public TaxesController(ITaxeService taxeService)
        {
            _taxeService = taxeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaxeDto>>> GetAll()
        {
            return Ok(await _taxeService.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<TaxeDto>> Create(CreateTaxeDto dto)
        {
            var created = await _taxeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTaxeDto dto)
        {
            var success = await _taxeService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _taxeService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}