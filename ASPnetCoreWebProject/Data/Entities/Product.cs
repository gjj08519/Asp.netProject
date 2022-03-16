using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaWebsite.Data.Entities
{
    public class Product
    {        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public ProductCategory Category { get; set; }

        [Required]
        public string ImageName { get; set; }

        public List<Portion> Portions { get; set; } = new List<Portion>();

        [NotMapped]
        public List<decimal> Prices { get; set; } = new List<decimal>();
    }

    public class DeserializedProduct
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string ImageName { get; set; }

        public List<string> Portions { get; set; } = new List<string>();

        public List<decimal> Prices { get; set; } = new List<decimal>();
    }
}
