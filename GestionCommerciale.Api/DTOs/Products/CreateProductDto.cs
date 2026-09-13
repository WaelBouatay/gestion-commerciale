using System.ComponentModel.DataAnnotations;

namespace GestionCommerciale.Api.DTOs.Products
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "La référence est obligatoire.")]
        public string Reference { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [MaxLength(150)]
        public string Nom { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à zéro.")]
        public decimal PrixUnitaireHT { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif.")]
        public int QuantiteEnStock { get; set; }
    }
}