using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class Portion
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
