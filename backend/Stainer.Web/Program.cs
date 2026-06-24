using Stainer.Web.Infrastructure;
using Stainer.Web.Infrastructure.Data;
using Stainer.Web.Infrastructure.Health;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddStainerInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

if (args.Contains("--seed-reference-data", StringComparer.OrdinalIgnoreCase))
{
    using var seedScope = app.Services.CreateScope();
    var dbContext = seedScope.ServiceProvider.GetRequiredService<StainerDbContext>();
    await DatabaseInitializer.InitializeAsync(dbContext);
    await dbContext.Database.MigrateAsync();
    var seeder = seedScope.ServiceProvider.GetRequiredService<ReferenceDataSeeder>();
    await seeder.SeedAsync();
    Console.WriteLine("Reference data seeded.");
    return;
}

app.MapGet("/health", () => Results.Ok(new { ok = true, app = "Stainer ASP.NET Core backend" }));
app.MapGet("/health/database", async (DatabaseHealthChecker checker, CancellationToken cancellationToken) =>
{
    var report = await checker.CheckAsync(cancellationToken);
    return Results.Ok(report);
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
    await DatabaseInitializer.InitializeAsync(dbContext);
}

app.Run();

public partial class Program;
