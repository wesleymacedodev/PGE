using System;
using System.Collections.Generic;

namespace PGE.Models;

public partial class Pessoa
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Cpf { get; set; }

    public string? Oab { get; set; }

    public virtual ICollection<Distribuir> DistribuirResponsavelAntigo { get; set; } = new List<Distribuir>();

    public virtual ICollection<Distribuir> DistribuirResponsavelNovo { get; set; } = new List<Distribuir>();

    public virtual Login Login { get; set; }

    public virtual ICollection<Processo> ProcessoParte { get; set; } = new List<Processo>();

    public virtual ICollection<Processo> ProcessoResponsavel { get; set; } = new List<Processo>();
}