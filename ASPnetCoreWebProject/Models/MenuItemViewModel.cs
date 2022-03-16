using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    public class MenuItemViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int ChosenProductId { get; set; }
        public string ChosenProductPortion { get; set; }
        public int ChosenProductQuantity { get; set; }
    }
}
