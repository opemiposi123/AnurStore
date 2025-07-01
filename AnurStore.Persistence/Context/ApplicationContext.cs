using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace AnurStore.Persistence.Context;

public class ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<User>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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
    public DbSet<PasswordReset> PasswordResets { get; set; } = default!;



    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }


    private void ApplyAuditInfo()
    {
        string userName = "System";

        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext != null && httpContext.User?.Identity?.IsAuthenticated == true)
        {
            userName = httpContext.User.Identity.Name ?? "System";
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = DateTime.Now;
                entry.Entity.CreatedBy = userName;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedOn = DateTime.Now;
                entry.Entity.LastModifiedBy = userName;
            }

            if (entry.State == EntityState.Deleted && entry.Entity is ISoftDelete deletable)
            {
                entry.State = EntityState.Modified;
                deletable.IsDeleted = true;
            }
        }
    }
}