using AnurStore.Application.Abstractions.Repositories; 
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using AnurStore.Application.Validators.Brand;
using AnurStore.Application.Validators.Category;
using AnurStore.Application.Validators.Product;
using AnurStore.Application.Validators.ProductUnit;
using AnurStore.Application.Validators.Supplier;
using AnurStore.Application.Validators.User;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using AnurStore.Persistence.Context.Seeder;
using AnurStore.Persistence.Repositories;
using AspNetCoreHero.ToastNotification;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();

//Database
var connectionString = 
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer       (builder.Configuration.GetConnectionString("AnurStore")));

//Repositories
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IProductUnitRepository, ProductUnitRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IProductSaleRepository, ProductSaleRepository>();

//Services
builder.Services.AddScoped<IUserAuthService,UserAuthService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductUnitService, ProductUnitService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IProductSaleService, ProductSaleService>();

//Validators
builder.Services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryValidator>();
builder.Services.AddScoped<IValidator<CreateBrandRequest>, CreateBrandValidator>();
builder.Services.AddScoped<IValidator<CreateSupplierRequest>, CreateSupplierValidator>();
builder.Services.AddScoped<IValidator<CreateProductUnitRequest>, CreateProductUnitValidator>();
builder.Services.AddScoped<IValidator<CreateProductRequest>, CreateProductValidator>();
builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();


builder.Services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryValidator>();
builder.Services.AddScoped<IValidator<UpdateBrandRequest>, UpdateBrandValidator>();
builder.Services.AddScoped<IValidator<UpdateSupplierRequest>, UpdateSupplierValidator>();
builder.Services.AddScoped<IValidator<UpdateProductUnitRequest>, UpdateProductUnitValidator>();
builder.Services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductValidator>();
builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidator>();

//Identity
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = true;
    opt.User.RequireUniqueEmail = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    opt.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationContext>()
.AddDefaultTokenProviders();

//Notyf
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
}
);
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

await app.SeedToDatabaseAsync();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserAuth}/{action=Login}/{id?}");

app.Run();
