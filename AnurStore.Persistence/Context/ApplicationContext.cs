using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AnurStore.Persistence.Context;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Account> Accounts { get; set; } = default!;
    public DbSet<AccountEntry> AccountEntries { get; set; } = default!;
    public DbSet<Brand> Brands { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Inventory> Inventories { get; set; } = default!;
    public DbSet<Reciept> Reciepts { get; set; } = default!;
    public DbSet<RecieptItem> RecieptItems { get; set; } = default!; 
    public DbSet<Payment> Payments { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductPurchase> ProductPurchases { get; set; } = default!;        
    public DbSet<ProductPurchaseItem> ProductPurchaseItems { get; set; } = default!;
    public DbSet<ProductSale> ProductSales { get; set; } = default!;
    public DbSet<ProductSaleItem> ProductSaleItems { get; set; } = default!;
    public DbSet<ProductSize> ProductSizes { get; set; } = default!;
    public DbSet<ProductUnit> ProductUnits { get; set; } = default!;
    public DbSet<Report> Reports { get; set; } = default!;
    public DbSet<Supplier> Suppliers { get; set; } = default!;
    public DbSet<Transaction> Transactions { get; set; } = default!;        
}