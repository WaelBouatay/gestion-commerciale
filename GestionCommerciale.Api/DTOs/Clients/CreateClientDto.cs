using System.ComponentModel.DataAnnotations;


namespace GestionCommerciale.Api.DTOs.Clients
{
    public class CreateClientDto
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le prénom ou la raison sociale est obligatoire.")]
        [MaxLength(150)]
        public string PrenomOuRaisonSociale { get; set; } = string.Empty;
       
         [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string Email { get; set; } = string.Empty;
       
        [Required(ErrorMessage = "Le téléphone est obligatoire.")]
        [Phone(ErrorMessage = "Format de téléphone invalide.")]
        public string Telephone { get; set; } = string.Empty;
        
        public List<string> Adresses { get; set; } = new();
    }
}