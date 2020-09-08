using Microsoft.AspNet.OData;
using NS.Models;
using System.Collections.Generic;
using System.Linq;

namespace NS.Controllers
{
    public class CustomersController
    {
        private static readonly List<Customer> _customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "Customer 1",
                Orders = new List<Order>
                {
                    new Order { Id = 1, Amount = 100M }
                }
            },
            new VipCustomer
            {
                Id = 2,
                Name = "Customer 2",
                LoyaltyCardNo = "9876543210",
                Orders = new List<Order>
                {
                    new Order { Id = 2, Amount = 230M },
                    new Order { Id = 3, Amount = 150M }
                }
            }
        };

        [EnableQuery]
        public IQueryable<Customer> Get()
        {
            return _customers.AsQueryable();
        }

        [EnableQuery]
        public SingleResult<Customer> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_customers.Where(d => d.Id.Equals(key)).AsQueryable());
        }
    }
}
