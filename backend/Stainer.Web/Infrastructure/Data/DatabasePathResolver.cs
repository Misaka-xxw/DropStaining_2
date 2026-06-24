namespace Stainer.Web.Infrastructure.Data;

public static class DatabasePathResolver
{
    public const string EnvironmentVariableName = "STAINER_DATABASE_URL";

    public static string ResolveConnectionString(IConfiguration configuration, IHostEnvironment environment)
    {
        var environmentValue = Environment.GetEnvironmentVariable(EnvironmentVariableName);
        if (!string.IsNullOrWhiteSpace(environmentValue))
        {
            return NormalizeConnectionString(environmentValue, environment.ContentRootPath);
        }

        var configuredValue =
            configuration.GetConnectionString("StainerDatabase")
            ?? configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>()?.ConnectionString;

        if (!string.IsNullOrWhiteSpace(configuredValue))
        {
            return NormalizeConnectionString(configuredValue, environment.ContentRootPath);
        }

        return BuildDefaultConnectionString(environment.ContentRootPath);
    }

    public static string BuildDefaultConnectionString(string contentRootPath)
    {
        var databasePath = Path.GetFullPath(Path.Combine(contentRootPath, "..", "..", "data", "stainer.db"));
        return $"Data Source={databasePath}";
    }

    public static string NormalizeConnectionString(string value, string contentRootPath)
    {
        if (value.StartsWith("sqlite:///", StringComparison.OrdinalIgnoreCase))
        {
            var path = value["sqlite:///".Length..].Replace('/', Path.DirectorySeparatorChar);
            return $"Data Source={Path.GetFullPath(path)}";
        }

        if (value.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
        {
            return value;
        }

        var rootedPath = Path.IsPathRooted(value)
            ? value
            : Path.Combine(contentRootPath, value);

        return $"Data Source={Path.GetFullPath(rootedPath)}";
    }

    public static string GetDatabasePath(string connectionString)
    {
        var builder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connectionString);
        return Path.GetFullPath(builder.DataSource);
    }
}
