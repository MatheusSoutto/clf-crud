using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ClfApi.Models;

#nullable disable

namespace ClfApi.Context
{
    public partial class ClfDbContext : DbContext
    {
        public ClfDbContext()
        {
        }

        public ClfDbContext(DbContextOptions<ClfDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clf> Clfs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=ClfDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");

            modelBuilder.Entity<Clf>(entity =>
            {
                entity.ToTable("Clf");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Client).HasMaxLength(200);

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Request)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.RequestDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.RfcIdentity).HasMaxLength(50);

                entity.Property(e => e.UserAgent).HasMaxLength(200);

                entity.Property(e => e.UserId).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
