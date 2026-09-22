namespace GestionCommerciale.Api.Models
{
    public enum TypeRemise
    {
        Pourcentage,
        MontantFixe
    }

    public class Remise
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty; // ex: "Remise fidélité", "Promo rentrée"
        public TypeRemise Type { get; set; }
        public decimal Valeur { get; set; }
        public bool Active { get; set; } = true;
    }
}