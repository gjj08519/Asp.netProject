using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }

    }
}
