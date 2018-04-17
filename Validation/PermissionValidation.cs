using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Configuration;
using securityservice.Model;

namespace securityservice.Validation
{
    public class PermissionValidationAttribute : ValidationAttribute {

        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {
            Dictionary<string, string> permissions = new Dictionary<string, string> ();
            var _configuration = (IConfiguration) validationContext.GetService (typeof (IConfiguration));
            if (_configuration.GetSection ("PossiblePermissions") != null) {
                permissions = _configuration.GetSection ("PossiblePermissions").GetChildren ()
                    .Select (item => new KeyValuePair<string, string> (item.Key, item.Value))
                    .ToDictionary (x => x.Key, x => x.Value);

                UserGroup userGroup = (UserGroup) validationContext.ObjectInstance;

                foreach (var permission in userGroup.permissions) {
                    if (!permissions.Keys.Contains (permission))
                        return new ValidationResult ("Permission must be on of the available permissions");

                }

            }

            return ValidationResult.Success;
        }

    }
}