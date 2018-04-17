using System.ComponentModel.DataAnnotations;

namespace securityservice.Model
{
    public class User {
        public int userId { get; set; }

        [Required]
        [MaxLength (20)]
        public string username { get; set; }

        [Required]
        [MaxLength (50)]
        public string name { get; set; }

        [Required]
        [MaxLength (255)]
        public string password { get; set; }

        [Required]
        [MaxLength (50)]
        public string email { get; set; }
        public bool? enabled { get; set; }
        public UserGroup userGroup { get; set; }

    }
}