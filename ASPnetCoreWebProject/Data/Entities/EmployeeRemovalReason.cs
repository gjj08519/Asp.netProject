using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class EmployeeRemovalReason
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employee Name:")]
        public string EmployeeName { get; set; }

        [Required]
        [Display(Name = "Reason to end their employment:")]
        public string Reason { get; set; }
    }
}
