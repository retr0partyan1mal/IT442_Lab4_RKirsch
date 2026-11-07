using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MMABooksEFClasses.MarisModels
{
    public partial class MMABooksContext : DbContext
    {
        public MMABooksContext()
        {
        }

        public MMABooksContext(DbContextOptions<MMABooksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<InvoiceLineItem> InvoiceLineItem { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<State> State { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = ConfigDB.GetMySqlConnectionString();
            if (!optionsBuilder.IsConfigured)
            {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));
                optionsBuilder.UseMySql(connectionString, serverVersion)
                              .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.InvalidIncludePathError));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID)
                    .HasName("PRIMARY");

                entity.ToTable("customers");

                entity.HasIndex(e => e.StateCode)
                    .HasName("FK_Customers_States");

                entity.Property(e => e.CustomerID).HasColumnName("CustomerID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StateCode).HasColumnName("State")
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.StateCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customers_States");
            });

            modelBuilder.Entity<InvoiceLineItem>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceID, e.ProductCode })
                    .HasName("PRIMARY");

                entity.ToTable("invoicelineitems");

                entity.HasIndex(e => e.ProductCode)
                    .HasName("FK_InvoiceLineItems_Products");

                entity.Property(e => e.InvoiceID).HasColumnName("InvoiceID");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ItemTotal).HasColumnType("decimal(10,4)");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,4)");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceLineItem)
                    .HasForeignKey(d => d.InvoiceID)
                    .HasConstraintName("FK_InvoiceLineItems_Invoices");

                entity.HasOne(d => d.ProductCodeNavigation)
                    .WithMany(p => p.InvoiceLineItem)
                    .HasForeignKey(d => d.ProductCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceLineItems_Products");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceID)
                    .HasName("PRIMARY");

                entity.ToTable("invoices");

                entity.HasIndex(e => e.CustomerID)
                    .HasName("FK_Invoices_Customers");

                entity.Property(e => e.InvoiceID).HasColumnName("InvoiceID");

                entity.Property(e => e.CustomerID).HasColumnName("CustomerID");

                entity.Property(e => e.InvoiceTotal).HasColumnType("decimal(10,4)");

                entity.Property(e => e.ProductTotal).HasColumnType("decimal(10,4)");

                entity.Property(e => e.SalesTax).HasColumnType("decimal(10,4)");

                entity.Property(e => e.Shipping).HasColumnType("decimal(10,4)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK_Invoices_Customers");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductCode)
                    .HasName("PRIMARY");

                entity.ToTable("products");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,4)");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.StateCode)
                    .HasName("PRIMARY");

                entity.ToTable("states");

                entity.Property(e => e.StateCode)
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
