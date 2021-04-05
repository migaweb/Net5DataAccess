using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace code.ef.Models
{
    public partial class udemyContext : DbContext
    {
        public udemyContext()
        {
        }

        public udemyContext(DbContextOptions<udemyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;Database=udemy;User Id=udemy;Port=5433");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.ToTable("customers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasColumnType("character varying(20)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(20)");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Customerid).HasColumnName("customerid");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(20)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("orders_customerid_fkey");
            });
        }
    }
}
