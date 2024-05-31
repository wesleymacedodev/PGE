using System.ComponentModel.DataAnnotations;

namespace PGE.DTOs
{
    public class ProcessoDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NumeroProcesso { get; set; }

        [Required]
        public int ParteId { get; set; }

        public int ResponsavelId { get; set; }
        [Required]
        public string Tema { get; set; }
        
        public string Descricao { get; set; }
        [Required]
        public decimal? Valor { get; set; }
    }
}
