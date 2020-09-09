using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using NS.Models;
using System.Collections.Generic;
using System.Linq;

namespace NS.Controllers
{
    public class CustomersController : ODataController
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
            },
            new VipCustomer
            {
                Id = 3,
                Name = "Customer 3",
                LoyaltyCardNo = "123455",
                Orders = new List<Order>
                {
                    new Order { Id = 4, Amount = 20M },
                    new Order { Id = 5, Amount = 10M }
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

        // http://localhost:5000/odata/Customers/NS.Models.VipCustomer
        // {"@odata.context":"http://localhost:5000/odata/$metadata#Customers/NS.Models.VipCustomer","value":[{"Id":2,"Name":"Customer 2","LoyaltyCardNo":"9876543210"}]}
        [EnableQuery]
        public IActionResult GetFromVipCustomer()
        {
            return Ok(_customers.OfType<VipCustomer>());
        }

        // http://localhost:5000/odata/Customers(3)/NS.Models.VipCustomer
        // {"@odata.context":"http://localhost:5000/odata/$metadata#Customers/NS.Models.VipCustomer/$entity","Id":3,"Name":"Customer 3","LoyaltyCardNo":"123455"}

        // http://localhost:32522/odata/Customers/Model.VipCustomer(2) should work. It seems there's a bug in the ASP.NET Core version.
        // Would you please file an issue for this?
        [EnableQuery]
        public IActionResult GetFromVipCustomer(int key)
        {
            VipCustomer vipCustomer = _customers.OfType<VipCustomer>().FirstOrDefault(d => d.Id == key);
            if (vipCustomer == null)
            {
                return NotFound($"Cannot found the VipCustomer with id '{key}'");
            }

            return Ok(vipCustomer);
        }
    }
}
