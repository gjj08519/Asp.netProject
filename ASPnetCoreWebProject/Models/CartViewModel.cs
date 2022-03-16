using PizzaWebsite.Data.Entities;
using System.Collections.Generic;

namespace PizzaWebsite.Models
{
    public class CartViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
