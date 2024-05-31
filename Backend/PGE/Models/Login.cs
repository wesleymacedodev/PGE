using System;
using System.Collections.Generic;

namespace PGE.Models;

public partial class Login
{
    public int Id { get; set; }

    public int PessoaId { get; set; }

    public string Nome { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }

    public bool Admin { get; set; }

    public virtual Pessoa Pessoa { get; set; }
}