using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models
{
    /*
    * Course: 		Web Programming 3
    * Assessment: 	Assignment 3
    * Created by: 	HIEU DAO LE DUC
    * Date: 		04 NOVEMBER 2021
    * Class Name: 	ContactViewModel.cs
    * Description:  Represents a view model with Contact information and reCAPTCHA response.
    */
    public class ContactViewModel
    {
        private static List<string> _topics = new List<string>()
        {
            "My order",
            "Feedback",
            "Product questions",
            "Customer service and feedback",
            "Technical questions, specifications, geometry, sizing and historical information",
            "Warranty",
            "Registration",
            "Catalogue requests",
            "Owner's manuals",
            "Media enquiries",
            "Sponsorship and donations"
        };

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

        /// <summary>
        /// Retrieves a list of pre-defined topics.
        /// </summary>
        public static List<string> Topics
        {
            get { return _topics; }
        }

        [Required]
        public string Topic { get; set; }

        [Required]
        public string Message { get; set; }

        public string ReCaptchaResponse { get; set; }
    }
}
