using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kinabalu.Models
{
    public partial class grad_dbContext : DbContext
    {
        public grad_dbContext()
        {
        }

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
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Wishlist> Wishlist { get; set; }
        public virtual DbSet<BelowMinimumStockView> BelowMinimumStockView { get; set; }
        public virtual DbSet<CustomerView> CustomerView { get; set; }
        public virtual DbSet<ProductsView> ProductsView { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(e => e.AddressId)
                    .HasColumnName("address_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.House)
                    .IsRequired()
                    .HasColumnName("house")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasColumnType("varchar(135)");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(135)");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasColumnName("email_address")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.AddressId });

                entity.ToTable("customer_address");

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
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(135)");

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
                entity.ToTable("order");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("FK_order_customer_idx");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerSource)
                    .IsRequired()
                    .HasColumnName("customer_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnName("delivery_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.ShipmentDate)
                    .HasColumnName("shipment_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId, e.ProductSource });

                entity.ToTable("order_product");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_idx");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductSource)
                    .HasColumnName("product_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

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
                entity.ToTable("product");

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
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(135)");

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
                    .HasColumnType("varchar(135)");

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
                entity.HasKey(e => new { e.CustomerId, e.CustomerSource, e.ProductId, e.ProductSource });

                entity.ToTable("rating");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_rating_idx");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerSource)
                    .HasColumnName("customer_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductSource)
                    .HasColumnName("product_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Rating1)
                    .HasColumnName("rating")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Restock>(entity =>
            {
                entity.ToTable("restock");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_restock_idx");

                entity.Property(e => e.RestockId)
                    .HasColumnName("restock_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fulfilled)
                    .HasColumnName("fulfilled")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Role1)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasColumnType("varchar(135)");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.CustomerSource, e.ProductId, e.ProductSource });

                entity.ToTable("shopping_cart");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_shopping_cart_idx");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerSource)
                    .HasColumnName("customer_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductSource)
                    .HasColumnName("product_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.ProductQuantity)
                    .HasColumnName("product_quantity")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("supplier");

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
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(135)");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_address_supplier_idx");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("FK_customer_idx");

                entity.HasIndex(e => e.RoleId)
                    .HasName("FK_role_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerSource)
                    .IsRequired()
                    .HasColumnName("customer_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.CustomerSource, e.ProductId, e.ProductSource });

                entity.ToTable("wishlist");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("FK_customer_wishlist_idx");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_product_wishlist_idx");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerSource)
                    .HasColumnName("customer_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductSource)
                    .HasColumnName("product_source")
                    .HasColumnType("varchar(135)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<BelowMinimumStockView>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("below_minimum_stock_view");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity");

                entity.Property(e => e.ReorderLevel)
                    .HasColumnName("reorder_level");

                entity.Property(e => e.Difference)
                    .HasColumnName("difference");
            });

            modelBuilder.Entity<CustomerView>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.ToTable("customer_view");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name");

                entity.Property(e => e.EmailAddress)
                    .HasColumnName("email_address");

                entity.Property(e => e.Address)
                    .HasColumnName("address");
            });

            modelBuilder.Entity<ProductsView>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("product_view");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name");

                entity.Property(e => e.Description)
                    .HasColumnName("description");

                entity.Property(e => e.Category)
                    .HasColumnName("category");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity");

                entity.Property(e => e.Source)
                    .HasColumnName("source");

                entity.Property(e => e.AverageRating)
                    .HasColumnName("average_rating");

                entity.Property(e => e.AverageReceivedRating)
                    .HasColumnName("average_received_rating");
            });
        }
    }
}
