using System;
using System.ComponentModel.DataAnnotations;

namespace MVCalendar.Controllers
{
    public class SignInViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}