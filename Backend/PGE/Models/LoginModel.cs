using System.ComponentModel.DataAnnotations;

namespace PGE.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nome obrigatorio")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Senha obrigatorio")]
        public string Password { get; set; }
    }
}
