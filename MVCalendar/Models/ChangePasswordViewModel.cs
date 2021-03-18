using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCalendar.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword")]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at max {1} characters long.", MinimumLength = 6)]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = " New Password")]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at max {1} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do no match.")]
        public string ConfirmPassword { get; set; }
    }
}
