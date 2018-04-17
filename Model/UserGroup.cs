using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using securityservice.Validation;

namespace securityservice.Model
{
    public class UserGroup {
        public int userGroupId { get; set; }

        [Required]
        [MaxLength (50)]
        public string name { get; set; }

        [MaxLength (100)]
        public string description { get; set; }
        public ICollection<User> users { get; set; }
        public bool? enabled { get; set; }

        [PermissionValidation]
        [Column ("permissions", TypeName = "string[]")]
        private string[] _permissions;
        public string[] permissions {
            get {
                if (this._permissions == null)
                    return new string[0];
                else
                    return this._permissions;
            }
            set { this._permissions = value; }
        }
    }
}