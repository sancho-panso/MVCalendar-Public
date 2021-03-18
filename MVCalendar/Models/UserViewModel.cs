using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCalendar.Models
{
    public class UserViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
        public string Lastname { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do no match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[+]{1}[0-9]{11}$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [BirthdayValidationModel(ErrorMessage = "The birthday date must be prior to the current date")]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }
    }
}

