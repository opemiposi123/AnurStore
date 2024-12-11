using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using AnurStore.Application.Validators.Brand;
using AnurStore.Application.Validators.Category;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using AnurStore.Persistence.Repositories;
using AspNetCoreHero.ToastNotification;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Database
var connectionString = builder.Configuration.GetConnectionString("AnurStore");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));

//Repositories
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//Services
builder.Services.AddScoped<IUserAuthService,UserAuthService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

//Validators
builder.Services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryValidator>();
builder.Services.AddScoped<IValidator<CreateBrandRequest>, CreateBrandValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryValidator>();
builder.Services.AddScoped<IValidator<UpdateBrandRequest>, UpdateBrandValidator>();

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

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
}
);
builder.Services.AddHttpContextAccessor();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserAuth}/{action=Login}/{id?}");

app.Run();
