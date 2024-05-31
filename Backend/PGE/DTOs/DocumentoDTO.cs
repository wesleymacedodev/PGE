using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PGE.DTOs
{
    public class DocumentoDTO
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Caminho { get; set; }

        public string Extensao { get; set; }

        public int ProcessoId { get; set; }
    }
}
