using System.ComponentModel.DataAnnotations;

namespace PGE.DTOs
{
    public class DistribuirDTO
    {
        [Key]
        public int Id { get; set; }

        public int ProcessoId { get; set; }

        public int ResponsavelAntigoId { get; set; }

        public int ResponsavelNovoId { get; set; }

        public DateOnly? Data { get; set; }
    }
}
