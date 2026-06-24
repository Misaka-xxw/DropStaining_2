using Microsoft.EntityFrameworkCore;
using Stainer.Web.Infrastructure.Data;
using Stainer.Web.Infrastructure.Health;

namespace Stainer.Web.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStainerInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = DatabasePathResolver.ResolveConnectionString(configuration, environment);
        DatabaseInitializer.EnsureDatabaseDirectory(connectionString);

        services.AddSingleton(new DatabaseHealthChecker(connectionString));
        services.AddDbContext<StainerDbContext>(options => options.UseSqlite(connectionString));

        return services;
    }
}
