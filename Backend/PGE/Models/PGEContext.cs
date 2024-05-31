using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PGE.Models;

public partial class PGEContext : DbContext
{
    public PGEContext(DbContextOptions<PGEContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Distribuir> Distribuir { get; set; }

    public virtual DbSet<Documento> Documento { get; set; }

    public virtual DbSet<Login> Login { get; set; }

    public virtual DbSet<Pessoa> Pessoa { get; set; }

    public virtual DbSet<Processo> Processo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Distribuir>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("data");
            entity.Property(e => e.ProcessoId).HasColumnName("processo_id");
            entity.Property(e => e.ResponsavelAntigoId).HasColumnName("responsavel_antigo_id");
            entity.Property(e => e.ResponsavelNovoId).HasColumnName("responsavel_novo_id");

            entity.HasOne(d => d.Processo).WithMany(p => p.Distribuir)
                .HasForeignKey(d => d.ProcessoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Distribuir_Processo");

            entity.HasOne(d => d.ResponsavelAntigo).WithMany(p => p.DistribuirResponsavelAntigo)
                .HasForeignKey(d => d.ResponsavelAntigoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Distribuir_Antigo_Pessoa");

            entity.HasOne(d => d.ResponsavelNovo).WithMany(p => p.DistribuirResponsavelNovo)
                .HasForeignKey(d => d.ResponsavelNovoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Distribuir_Nova_Pessoa");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Caminho)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("caminho");
            entity.Property(e => e.Extensao)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("extensao");
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.ProcessoId).HasColumnName("processo_id");

            entity.HasOne(d => d.Processo).WithMany(p => p.Documento)
                .HasForeignKey(d => d.ProcessoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Documento_Processo");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasIndex(e => e.Nome, "UQ_Login_Nome").IsUnique();

            entity.HasIndex(e => e.PessoaId, "UQ_Login_Pessoa").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Admin)
                .HasDefaultValue(false)
                .HasColumnName("admin");
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasColumnName("password_salt");
            entity.Property(e => e.PessoaId).HasColumnName("pessoa_id");

            entity.HasOne(d => d.Pessoa).WithOne(p => p.Login)
                .HasForeignKey<Login>(d => d.PessoaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Login_Pessoa");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasIndex(e => e.Cpf, "UQ_Pessoa_CPF").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("cpf");
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.Oab)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("oab");
        });

        modelBuilder.Entity<Processo>(entity =>
        {
            entity.HasIndex(e => e.NumeroProcesso, "UQ_Processo_Numero_Processo").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descricao");
            entity.Property(e => e.NumeroProcesso).HasColumnName("numero_processo");
            entity.Property(e => e.ParteId).HasColumnName("parte_id");
            entity.Property(e => e.ResponsavelId).HasColumnName("responsavel_id");
            entity.Property(e => e.Tema)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tema");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("valor");

            entity.HasOne(d => d.Parte).WithMany(p => p.ProcessoParte)
                .HasForeignKey(d => d.ParteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Processo_Parte_Pessoa");

            entity.HasOne(d => d.Responsavel).WithMany(p => p.ProcessoResponsavel)
                .HasForeignKey(d => d.ResponsavelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Processo_Responsavel_Pessoa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}