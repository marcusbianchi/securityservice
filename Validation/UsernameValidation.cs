using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Configuration;
using securityservice.Data;

namespace securityservice.Validation {
    public class UsernameValidation : ValidationAttribute {

        // private readonly ApplicationDbContext context;
        // public UsernameValidation (ApplicationDbContext _context) {
        //     context = _context;
        // }

        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {
            var _context = (ApplicationDbContext) validationContext.GetService (typeof (ApplicationDbContext));

            bool contactExists =  _context.Users.Any (user => user.username.Equals (value));

            if (contactExists)
                return new ValidationResult ("The username already exist");

            return ValidationResult.Success;
        }

    }
}