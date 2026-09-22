using System.ComponentModel.DataAnnotations;
using GestionCommerciale.Api.Models;

namespace GestionCommerciale.Api.DTOs.Remises
{
    public class UpdateRemiseDto
    {
        [Required]
        public string Libelle { get; set; } = string.Empty;

        [Required]
        public TypeRemise Type { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La valeur ne peut pas être négative.")]
        public decimal Valeur { get; set; }

        public bool Active { get; set; } = true;
    }
}