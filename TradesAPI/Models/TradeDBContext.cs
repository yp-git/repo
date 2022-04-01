using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TradesAPI.Models
{
    public partial class TradeDBContext : DbContext
    {
        public TradeDBContext()
        {
        }

        public TradeDBContext(DbContextOptions<TradeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TradeDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.HasIndex(e => e.SecurityCode, "nciSecurityCode");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Action)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TradeId).HasColumnName("TradeID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
