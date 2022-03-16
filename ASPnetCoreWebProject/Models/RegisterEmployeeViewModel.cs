using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PizzaWebsite.Data.Seeder.UserIdentityDataSeeder;

namespace PizzaWebsite.Models
{
    public class RegisterEmployeeViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Roles Role { get; set; }
    }
}
