// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Stainer.LegacyImporter;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Data;

var optionsResult = LegacyImportOptions.Parse(args);
if (!optionsResult.IsSuccess)
{
    Console.Error.WriteLine(optionsResult.ErrorMessage);
    Console.Error.WriteLine(LegacyImportOptions.Usage);
    return 2;
}

var options = optionsResult.Options!;
var connectionString = options.DatabaseUrl ?? Environment.GetEnvironmentVariable("STAINER_DATABASE_URL") ?? "Data Source=data/stainer.db";

DatabaseInitializer.EnsureDatabaseDirectory(connectionString);
var dbOptions = new DbContextOptionsBuilder<StainerDbContext>()
    .UseSqlite(connectionString)
    .AddInterceptors(new SqlitePragmaConnectionInterceptor())
    .Options;

await using var dbContext = new StainerDbContext(dbOptions);
var importer = new LegacyJsonImporter(dbContext, new ReagentBarcodeParser());
var report = await importer.ImportAsync(options);

Console.WriteLine($"Legacy import {(options.Apply ? "apply" : "dry-run")} completed: {report.Result}");
Console.WriteLine($"Report: {report.ReportPath}");
Console.WriteLine($"Issues: {report.Issues.Count}");
return report.Result == LegacyImportResult.Failed ? 1 : 0;
