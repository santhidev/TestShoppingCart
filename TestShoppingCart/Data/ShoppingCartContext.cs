using System.Collections.Generic;
using TestShoppingCart.Models;
using Microsoft.EntityFrameworkCore;

namespace TestShoppingCart.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products", schema: "public");
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductId).HasColumnName("product_id");  // ชื่อคอลัมน์ตรงกับ PostgreSQL
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("stocks", schema: "public");
                entity.HasKey(e => e.StockId);
                entity.Property(e => e.StockId).HasColumnName("stock_id");  // ชื่อคอลัมน์ตรงกับ PostgreSQL
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
            });

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Stock)
                .WithOne(s => s.Product)
                .HasForeignKey<Stock>(s => s.ProductId);
        }
    }
}
