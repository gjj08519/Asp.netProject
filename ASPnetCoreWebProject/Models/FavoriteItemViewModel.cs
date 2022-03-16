using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    public class FavoriteItemViewModel
    {
        public IEnumerable<FavoriteItem> FavoriteItems { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
