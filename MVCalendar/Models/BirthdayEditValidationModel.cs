using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCalendar.Models
{
    public class BirthdayEditValidationModel:ValidationAttribute
    {
        protected override ValidationResult
                   IsValid(object value, ValidationContext validationContext)
        {
            var model = (EditUserViewModel)validationContext.ObjectInstance;
            DateTime BirthdayDate = Convert.ToDateTime(model.Birthday);
            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now);

            if (BirthdayDate > CurrentDate)
            {
                return new ValidationResult
                    ("The birthday date must be prior to the current date");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
