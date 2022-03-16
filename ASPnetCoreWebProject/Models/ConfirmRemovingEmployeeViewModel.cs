using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PizzaWebsite.Data.Seeder.UserIdentityDataSeeder;

namespace PizzaWebsite.Models
{
    public class ConfirmRemovingEmployeeViewModel
    {
        public string UserId { get; set; }
        public Roles Role { get; set; }

        [Required]
        [Display(Name = "Employee Name:")]
        public string EmployeeName { get; set; }

        [Required]
        [Display(Name = "Reason to end their employment:")]
        public string Reason { get; set; }
    }
}
