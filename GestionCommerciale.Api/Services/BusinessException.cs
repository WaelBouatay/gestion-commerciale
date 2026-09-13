namespace GestionCommerciale.Api.Services
{
    // Exception utilisée pour toutes les erreurs de règles métier
    // (ex: stock insuffisant, client inexistant...)
    // Le Controller l'attrapera pour renvoyer un code HTTP 400 propre
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}