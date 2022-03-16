using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int PortionId { get; set; }

        public Portion Portion { get; set; }

        [NotMapped]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }        
    }
}
