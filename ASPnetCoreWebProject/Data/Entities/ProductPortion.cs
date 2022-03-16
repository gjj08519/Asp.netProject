

using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaWebsite.Data.Entities
{
    public class ProductPortion
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int PortionId { get; set; }

        [ForeignKey("PortionId")]
        public Portion Portion { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
