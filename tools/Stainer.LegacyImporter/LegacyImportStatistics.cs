namespace Stainer.LegacyImporter;

public sealed class LegacyImportStatistics
{
    public Dictionary<string, int> Scanned { get; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> Imported { get; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> Skipped { get; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> Failed { get; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> Issues { get; } = new(StringComparer.OrdinalIgnoreCase);

    public void IncrementScanned(string key) => Increment(Scanned, key);
    public void IncrementImported(string key) => Increment(Imported, key);
    public void IncrementSkipped(string key) => Increment(Skipped, key);
    public void IncrementFailed(string key) => Increment(Failed, key);

    public void Increment(string category, string key)
    {
        var target = category switch
        {
            "scanned" => Scanned,
            "imported" => Imported,
            "skipped" => Skipped,
            "failed" => Failed,
            "issues" => Issues,
            _ => Issues
        };
        Increment(target, key);
    }

    private static void Increment(IDictionary<string, int> dictionary, string key)
    {
        dictionary[key] = dictionary.TryGetValue(key, out var current) ? current + 1 : 1;
    }
}
