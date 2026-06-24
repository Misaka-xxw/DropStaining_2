namespace Stainer.LegacyImporter;

public sealed class ParseResult
{
    private ParseResult(bool isSuccess, LegacyImportOptions? options, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Options = options;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }
    public LegacyImportOptions? Options { get; }
    public string? ErrorMessage { get; }

    public static ParseResult Success(LegacyImportOptions options) => new(true, options, null);

    public static ParseResult Fail(string errorMessage) => new(false, null, errorMessage);
}
