using PizzaWebsite.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models
{
    public class CheckoutViewModel
    {
        [Required]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "First name cannot contain numbers.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "Last name cannot contain numbers.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@".*@.*\.\w{2,}", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class DeliveryCheckoutViewModel : CheckoutViewModel
    {
        // Assisted by https://stackoverflow.com/questions/15774555/efficient-regex-for-canadian-postal-code-function
        [RegularExpression(@"^[ABCEGHJ-NPRSTVXY]\d[ABCEGHJ-NPRSTV-Z][ -]?\d[ABCEGHJ-NPRSTV-Z]\d$", ErrorMessage = "Please enter a valid Canadian Postal Code.")]
        [DataType(DataType.PostalCode)]
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Delivery Area")]
        public string DeliveryArea { get; set; }

        [Required]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }

        /// <summary>
        /// Retrieves a list of pre-defined delivery areas.
        /// </summary>
        public static List<string> DeliveryAreas
        {
            get { return UserData.DeliveryAreas; }
        }
    }
}
