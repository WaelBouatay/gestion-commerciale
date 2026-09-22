namespace GestionCommerciale.Api.Models
{
    
    public class OrderRemise
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int RemiseId { get; set; }
        public Remise? Remise { get; set; }
    }
}