using System;
using System.Collections.Generic;

namespace PGE.Models;

public partial class Documento
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Caminho { get; set; }

    public string Extensao { get; set; }

    public int ProcessoId { get; set; }

    public virtual Processo Processo { get; set; }
}