using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Repositories;
using Stainer.Web.Infrastructure.Data;
using Stainer.Web.Infrastructure.Health;
using Stainer.Web.Infrastructure.Repositories;

namespace Stainer.Web.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStainerInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = DatabasePathResolver.ResolveConnectionString(configuration, environment);
        DatabaseInitializer.EnsureDatabaseDirectory(connectionString);

        services.AddSingleton(new DatabaseHealthChecker(connectionString));
        services.AddDbContext<StainerDbContext>(options =>
            options.UseSqlite(connectionString)
                .AddInterceptors(new SqlitePragmaConnectionInterceptor()));
        services.AddScoped<ReferenceDataSeeder>();
        services.AddScoped<IReferenceDataRepository, EfReferenceDataRepository>();

        return services;
    }
}
