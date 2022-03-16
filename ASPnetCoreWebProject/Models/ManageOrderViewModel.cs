using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    public class ManageOrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public Dictionary<Order, decimal> TotalForeachOrder { get; set; } = new Dictionary<Order, decimal>();
        //  public List<CartItem> Items { get; set; }
        public Dictionary<Order, List<CartItem>> OrderDetails { get; set; } = new Dictionary<Order, List<CartItem>>();
        public decimal Total { get; set; }
    }
}
