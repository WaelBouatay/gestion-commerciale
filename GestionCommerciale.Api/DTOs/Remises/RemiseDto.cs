using GestionCommerciale.Api.Models;

namespace GestionCommerciale.Api.DTOs.Remises
{
    public class RemiseDto
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty;
        public TypeRemise Type { get; set; }
        public decimal Valeur { get; set; }
        public bool Active { get; set; }
    }
}