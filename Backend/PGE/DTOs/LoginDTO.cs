using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PGE.DTOs
{
    public class LoginDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public int PessoaId { get; set; }

        [Required]
        public string Nome { get; set; }

        [NotMapped]
        public string Password { get; set; }
        [JsonIgnore]
        public bool Admin { get; set; }
    }
}
