using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ArchiveModel.Models.Database
{
    public partial class betsContext : DbContext
    {
        public betsContext()
        {
        }

        public betsContext(DbContextOptions<betsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Archive> Archive { get; set; }
        public virtual DbSet<FutureBets> FutureBets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;user=root;password=MySQL159*;database=bets");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Archive>(entity =>
            {
                entity.HasKey(e => e.IdArchive);

                entity.ToTable("archive", "bets");

                entity.Property(e => e.IdArchive)
                    .HasColumnName("idArchive")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AwayTeam)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.AwayTeamScore).HasColumnType("int(11)");

                entity.Property(e => e.HomeTeam)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.HomeTeamScore).HasColumnType("int(11)");

                entity.Property(e => e.League)
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FutureBets>(entity =>
            {
                entity.ToTable("future_bets", "bets");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AwayTeam)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.HomeTeam)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });
        }
    }
}
