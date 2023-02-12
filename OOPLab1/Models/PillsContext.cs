using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OOPLab1.Models;

public partial class PillsContext : DbContext
{
    public PillsContext()
    {
    }

    public PillsContext(DbContextOptions<PillsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ilness> Ilnesses { get; set; }

    public virtual DbSet<Pharmasy> Pharmasies { get; set; }

    public virtual DbSet<Pill> Pills { get; set; }

    public virtual DbSet<PillClass> PillClasses { get; set; }

    public virtual DbSet<PillsAndIlness> PillsAndIlnesses { get; set; }

    public virtual DbSet<PillsAndPharmasy> PillsAndPharmasies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= SCAT\\SQLEXPRESS; Database=pills; Trusted_Connection=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ilness>(entity =>
        {
            entity.ToTable("Ilness");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Pills).HasMaxLength(50);
        });

        modelBuilder.Entity<Pharmasy>(entity =>
        {
            entity.ToTable("Pharmasy");

            entity.Property(e => e.Adress)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Pill>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.ClassNavigation).WithMany(p => p.Pills)
                .HasForeignKey(d => d.Class)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pills_PillClass");
        });

        modelBuilder.Entity<PillClass>(entity =>
        {
            entity.ToTable("PillClass");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PillsAndIlness>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Illnes).WithMany(p => p.PillsAndIlnesses)
                .HasForeignKey(d => d.IllnesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PillsAndIlnesses_Ilness");

            entity.HasOne(d => d.Pill).WithMany(p => p.PillsAndIlnesses)
                .HasForeignKey(d => d.PillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PillsAndIlnesses_Pills");
        });

        modelBuilder.Entity<PillsAndPharmasy>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Pharmasy).WithMany(p => p.PillsAndPharmasies)
                .HasForeignKey(d => d.PharmasyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PillsAndPharmasies_Pharmasy");

            entity.HasOne(d => d.Pill).WithMany(p => p.PillsAndPharmasies)
                .HasForeignKey(d => d.PillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PillsAndPharmasies_Pills");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
