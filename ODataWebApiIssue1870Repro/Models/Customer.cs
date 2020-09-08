using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NS.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
    }
}
