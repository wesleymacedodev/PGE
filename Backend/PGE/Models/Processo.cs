using System;
using System.Collections.Generic;

namespace PGE.Models;

public partial class Processo
{
    public int Id { get; set; }

    public int NumeroProcesso { get; set; }

    public int ParteId { get; set; }

    public int ResponsavelId { get; set; }

    public string Tema { get; set; }

    public string Descricao { get; set; }

    public decimal? Valor { get; set; }

    public virtual ICollection<Distribuir> Distribuir { get; set; } = new List<Distribuir>();

    public virtual ICollection<Documento> Documento { get; set; } = new List<Documento>();

    public virtual Pessoa Parte { get; set; }

    public virtual Pessoa Responsavel { get; set; }
}