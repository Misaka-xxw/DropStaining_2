namespace Stainer.LegacyImporter;

public sealed class LegacyImportOptions
{
    public const string Usage = "Usage: dotnet run --project tools/Stainer.LegacyImporter -- --source-dir <dir> (--dry-run|--apply) [--database-url <connection>] [--report-path <path>]";

    public required string SourceDirectory { get; init; }
    public bool DryRun { get; init; }
    public bool Apply { get; init; }
    public string? DatabaseUrl { get; init; }
    public string? ReportPath { get; init; }

    public static ParseResult Parse(string[] args)
    {
        string? sourceDirectory = null;
        string? databaseUrl = null;
        string? reportPath = null;
        var dryRun = false;
        var apply = false;

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "--source-dir":
                    if (++i >= args.Length)
                    {
                        return ParseResult.Fail("--source-dir requires a value.");
                    }

                    sourceDirectory = args[i];
                    break;
                case "--database-url":
                    if (++i >= args.Length)
                    {
                        return ParseResult.Fail("--database-url requires a value.");
                    }

                    databaseUrl = args[i];
                    break;
                case "--report-path":
                    if (++i >= args.Length)
                    {
                        return ParseResult.Fail("--report-path requires a value.");
                    }

                    reportPath = args[i];
                    break;
                case "--dry-run":
                    dryRun = true;
                    break;
                case "--apply":
                    apply = true;
                    break;
                default:
                    return ParseResult.Fail($"Unknown argument: {arg}");
            }
        }

        if (string.IsNullOrWhiteSpace(sourceDirectory))
        {
            return ParseResult.Fail("--source-dir is required.");
        }

        if (dryRun == apply)
        {
            return ParseResult.Fail("Specify exactly one of --dry-run or --apply.");
        }

        return ParseResult.Success(new LegacyImportOptions
        {
            SourceDirectory = sourceDirectory,
            DryRun = dryRun,
            Apply = apply,
            DatabaseUrl = databaseUrl,
            ReportPath = reportPath
        });
    }
}
