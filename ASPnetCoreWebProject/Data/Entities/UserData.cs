using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class UserData
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public byte[] ProfilePicture { get; set; }
        
        public string DeliveryAddress { get; set; }

        public string DeliveryArea { get; set; }

        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        public List<Order> OrdersHistory;
        public List<FavoriteItem> FavoriteItem;

        private static List<string> _deliveryAreas = new List<string>()
        {
            "Sainte-Anne-De-Bellevue",
            "Baie-D'Urfé",
            "Senneville",
            "Kirkland",
            "Dollard-Des-Ormeaux",
            "Beaconsfield",
            "Pierrefonds and Roxboro",
            "L'Île-Bizard–Sainte-Geneviève",
            "Pointe-Claire",
            "Dorval"
        };

        /// <summary>
        /// Retrieves a list of pre-defined delivery areas.
        /// </summary>
        public static List<string> DeliveryAreas
        {
            get { return _deliveryAreas; }
        }
    }
}
