using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using phantom_mask.Models.pharmacy;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Data
{
    public partial class Context : DbContext
    {
        private string connectionString;
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<inventory> inventory { get; set; }
        public virtual DbSet<mask> mask { get; set; }
        public virtual DbSet<pharmacy> pharmacy { get; set; }
        public virtual DbSet<purchaseHistory> purchaseHistory { get; set; }
        public virtual DbSet<user> user { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //資料庫來源
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json", optional: false);

                var configuration = builder.Build();

                connectionString = configuration.GetConnectionString("pharmacyConnContext").ToString();

                optionsBuilder.UseSqlServer(connectionString);

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<inventory>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.Property(e => e.pharmacyId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.mask)
                    .WithMany(p => p.inventory)
                    .HasForeignKey(d => d.maskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_mask1");

                entity.HasOne(d => d.pharmacy)
                    .WithMany(p => p.inventory)
                    .HasForeignKey(d => d.pharmacyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_pharmacy");
            });

            modelBuilder.Entity<purchaseHistory>(entity =>
            {
                entity.HasOne(d => d.mask)
                    .WithMany(p => p.purchaseHistory)
                    .HasForeignKey(d => d.maskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_purchaseHistory_mask");

                entity.HasOne(d => d.pharmacy)
                    .WithMany(p => p.purchaseHistory)
                    .HasForeignKey(d => d.pharmacyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_purchaseHistory_pharmacy");

                entity.HasOne(d => d.user)
                    .WithMany(p => p.purchaseHistory)
                    .HasForeignKey(d => d.userId)
                    .HasConstraintName("FK_purchaseHistory_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
