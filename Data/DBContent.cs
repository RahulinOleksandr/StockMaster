using Microsoft.EntityFrameworkCore;
using Stock_Master.Models;

namespace Stock_Master.Data
{
    public class DBContent : DbContext
    {
        public DBContent(DbContextOptions<DBContent> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<Stock> Stocks => Set<Stock>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { warehouseId = 1, warehouseName = "Головний склад", address = "Суми, вул. Промислова 1" },
                new Warehouse { warehouseId = 2, warehouseName = "Склад №2", address = "Суми, вул. Індустріальна 5" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { productId = 1, productName = "Ноутбук", category = "Електроніка", price = 25000 },
                new Product { productId = 2, productName = "Монітор", category = "Електроніка", price = 8000 },
                new Product { productId = 3, productName = "Клавіатура", category = "Периферія", price = 1200 }
            );

            modelBuilder.Entity<Stock>().HasData(
                new Stock { stockId = 1, ProductId = 1, WarehouseId = 1, quantity = 15 },
                new Stock { stockId = 2, ProductId = 2, WarehouseId = 1, quantity = 30 },
                new Stock { stockId = 3, ProductId = 1, WarehouseId = 2, quantity = 5 }, // the same laptop, different configuration
                new Stock { stockId = 4, ProductId = 3, WarehouseId = 2, quantity = 50 }
            );
        }
    }
}
