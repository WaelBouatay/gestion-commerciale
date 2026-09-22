using System.ComponentModel.DataAnnotations;
using GestionCommerciale.Api.Models;

namespace GestionCommerciale.Api.DTOs.Taxes
{
    public class UpdateTaxeDto
    {
        [Required]
        public string Libelle { get; set; } = string.Empty;

        [Required]
        public TypeTaxe Type { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La valeur ne peut pas être négative.")]
        public decimal Valeur { get; set; }

        public bool Active { get; set; } = true;
    }
}