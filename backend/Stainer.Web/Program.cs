using Stainer.Web.Infrastructure;
using Stainer.Web.Infrastructure.Data;
using Stainer.Web.Infrastructure.Health;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddStainerInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

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
