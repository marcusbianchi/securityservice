using System.ComponentModel.DataAnnotations;
using securityservice.Validation;

namespace securityservice.Model
{
    public class User {
        public int userId { get; set; }

        [Required]
        [MaxLength (20)]
        [UsernameValidation]
        public string username { get; set; }

        [Required]
        [MaxLength (50)]
        public string name { get; set; }

        [Required]
        [MaxLength (280)]
        public string password { get; set; }

        [Required]
        [MaxLength (50)]
        public string email { get; set; }
        public bool? enabled { get; set; }
        public UserGroup userGroup { get; set; }

    }
}