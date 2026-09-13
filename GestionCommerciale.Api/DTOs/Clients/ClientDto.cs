namespace GestionCommerciale.Api.DTOs.Clients
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string PrenomOuRaisonSociale { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public List<string> Adresses { get; set; } = new();
        public DateTime DateCreation { get; set; }
    }
}