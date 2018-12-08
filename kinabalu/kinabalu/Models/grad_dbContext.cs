using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kinabalu.Models
{
    public partial class grad_dbContext : DbContext
    {
        public grad_dbContext(DbContextOptions<grad_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderProduct> OrderProduct { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Restock> Restock { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<Wishlist> Wishlist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address", "grad_db");

                entity.Property(e => e.AddressId)
                    .HasColumnName("address_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.House)
                    .IsRequired()
                    .HasColumnName("house")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasMaxLength(135)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category", "grad_db");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(135)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "grad_db");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasColumnName("email_address")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.AddressId });

                entity.ToTable("customer_address", "grad_db");

                entity.HasIndex(e => e.AddressId)
                    .HasName("FK_address_idx");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddressId)
                    .HasColumnName("address_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.CustomerAddress)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_address");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAddress)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customer");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order", "grad_db");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("FK_order_customer_idx");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ShipmentDate).HasColumnName("shipment_date");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId, e.Source });

                entity.ToTable("order_product", "grad_db");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_idx");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderProduct)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product", "grad_db");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("FK_product_category_idx");

                entity.HasIndex(e => e.SupplierId)
                    .HasName("FK_product_supplier_idx");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReorderLevel)
                    .HasColumnName("reorder_level")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("supplier_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightUnitOfMeasure)
                    .IsRequired()
                    .HasColumnName("weight_unit_of_measure")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_category");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_supplier");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId });

                entity.ToTable("rating", "grad_db");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_rating_idx");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Rating1)
                    .HasColumnName("rating")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Restock>(entity =>
            {
                entity.ToTable("restock", "grad_db");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_restock_idx");

                entity.Property(e => e.RestockId)
                    .HasColumnName("restock_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fulfilled)
                    .HasColumnName("fulfilled")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Restock)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId, e.Source });

                entity.ToTable("shopping_cart", "grad_db");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_shopping_cart_idx");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ProductQuantity)
                    .HasColumnName("product_quantity")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("supplier", "grad_db");

                entity.HasIndex(e => e.AddressId)
                    .HasName("FK_address_supplier_idx");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("supplier_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddressId)
                    .HasColumnName("address_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(135)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId, e.Source });

                entity.ToTable("wishlist", "grad_db");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("FK_customer_wishlist_idx");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_wishlist_idx");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasMaxLength(135)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
