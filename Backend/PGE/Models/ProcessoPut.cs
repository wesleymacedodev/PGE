

using System.Text.Json.Serialization;

namespace PGE.Models
{
    public class ProcessoPut
    {

        public int NumeroProcesso { get; set; }

        public int ParteId { get; set; }

        [JsonIgnore]
        public int ResponsavelId { get; set; }

        public string Tema { get; set; }

        public string Descricao { get; set; }

        public decimal? Valor { get; set; }
    }
}
