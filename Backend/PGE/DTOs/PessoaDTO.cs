using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PGE.Models;

namespace PGE.DTOs
{
    public class PessoaDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Nome { get; set; }

        [Required]
        [StringLength(11)]
        [MinLength(11, ErrorMessage = "Cpf deve conter no minimo 11 caracteres")]
        [Unicode(false)]
        public string Cpf { get; set; }

        [StringLength(20, ErrorMessage = "O codigo oab deve conter no maximo 20 caracteres")]
        [Unicode(false)]
        public string Oab { get; set; }
    }
}
