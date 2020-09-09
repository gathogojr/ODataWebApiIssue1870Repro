using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using NS.Models;
using System.Collections.Generic;
using System.Linq;

namespace NS.Controllers
{
    public class OrdersController : ODataController
    {
        private static readonly List<Order> _orders = new List<Order>
        {
            new Order
            {
                Id = 1,
                Amount = 100M,
                Customer = new Customer
                {
                    Id = 1, Name = "Customer 1"
                }
            },
            new Order
            {
                Id = 2,
                Amount = 230M,
                Customer = new VipCustomer
                {
                    Id = 2, Name = "Customer 2", LoyaltyCardNo = "9876543210"
                }
            },
            new Order
            {
                Id = 3,
                Amount = 150M,
                Customer = new VipCustomer
                {
                    Id = 2, Name = "Customer 2", LoyaltyCardNo = "9876543210"
                }
            }
        };

        [EnableQuery]
        public IQueryable<Order> Get()
        {
            return _orders.AsQueryable();
        }

        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(_orders.FirstOrDefault(d => d.Id == key));
        }
    }
}
