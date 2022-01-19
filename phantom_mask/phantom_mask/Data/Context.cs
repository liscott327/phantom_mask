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

        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Mask> Mask { get; set; }
        public virtual DbSet<Pharmacy> Pharmacy { get; set; }
        public virtual DbSet<PurchaseHistory> PurchaseHistory { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Server=localhost;Database=Pharmacy;User ID=sa;Password=taipay.mssql.5517;Trusted_Connection=True;Integrated Security=False;");

                var builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json", optional: false);

                var configuration = builder.Build();

                connectionString = configuration.GetConnectionString("DatabaseConnection").ToString();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasOne(d => d.Mask)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.MaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_mask1");

                entity.HasOne(d => d.Pharmacy)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.PharmacyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_pharmacy");
            });

            modelBuilder.Entity<PurchaseHistory>(entity =>
            {
                entity.HasOne(d => d.Mask)
                    .WithMany(p => p.PurchaseHistory)
                    .HasForeignKey(d => d.MaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_purchaseHistory_mask");

                entity.HasOne(d => d.Pharmacy)
                    .WithMany(p => p.PurchaseHistory)
                    .HasForeignKey(d => d.PharmacyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_purchaseHistory_pharmacy");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PurchaseHistory)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_purchaseHistory_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
