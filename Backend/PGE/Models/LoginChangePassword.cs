using System.ComponentModel.DataAnnotations;

namespace PGE.Models
{
    public class LoginChangePassword
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
