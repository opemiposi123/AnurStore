using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AnurStore.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountEntry> AccountEntries { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPurchase> ProductPurchases { get; set; }
        public DbSet<ProductPurchaseItem> ProductPurchaseItems { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }
        public DbSet<ProductSaleItem> ProductSaleItems { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }

}

