using GestionCommerciale.Api.Models;

namespace GestionCommerciale.Api.DTOs.Taxes
{
    public class TaxeDto
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty;
        public TypeTaxe Type { get; set; }
        public decimal Valeur { get; set; }
        public bool Active { get; set; }
    }
}