using System;
using System.Collections.Generic;

namespace PizzaWebsite.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public Boolean CheckedOut { get; set; }


        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
