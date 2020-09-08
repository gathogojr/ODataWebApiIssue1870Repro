using System.ComponentModel.DataAnnotations;

namespace NS.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Customer Customer { get; set; }
    }
}
