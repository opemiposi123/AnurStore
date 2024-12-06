
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnurStore.IOC.ServiceCollections;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}
 