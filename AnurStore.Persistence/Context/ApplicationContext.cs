using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AnurStore.Persistence.Context;

public class ApplicationContext : IdentityDbContext<User>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var adminRoleId = Guid.NewGuid().ToString();
        var adminUserId = Guid.NewGuid().ToString();
        var cashierRoleId = Guid.NewGuid().ToString();
        var cashierUserId = Guid.NewGuid().ToString();

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
        );

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = cashierRoleId,
                Name = "Cashier",
                NormalizedName = "CASHIER"
            }
        );

        var hasher = new PasswordHasher<User>();

        builder.Entity<User>().HasData(
            new User
            {
                Id = adminUserId,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                Address = "No 4 Unity Str Aboru",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty,
                Role = Role.Admin,
                FirstName = "Admin",
                LastName = "AnurStore",
                Gender = Gender.Female,
            }
        );

        builder.Entity<User>().HasData(
            new User
            {
                Id = cashierUserId,
                UserName = "Cashier",
                NormalizedUserName = "CASHIER",
                Email = "cashier@gmail.com",
                NormalizedEmail = "CASHIER@GMAIL.COM",
                Address = "No 5 Manchester Liberty Estate ",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Cashier@123"),
                SecurityStamp = string.Empty,
                Role = Role.Cahier,
                FirstName = "Cashier",
                LastName = "Ameerah",
                Gender = Gender.Female,
            }
        );

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            }
        );

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = cashierRoleId,
                UserId = cashierUserId
            }
        );
    }


    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountEntry> AccountEntries { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
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