namespace GestionCommerciale.Api.Models
{
    
    public enum TypeTaxe
    {
        Pourcentage,
        MontantFixe
    }

    public class Taxe
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty; 
        public TypeTaxe Type { get; set; }
        public decimal Valeur { get; set; } 
        public bool Active { get; set; } = true;
    }
}