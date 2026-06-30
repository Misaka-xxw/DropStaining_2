using Microsoft.EntityFrameworkCore;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public interface IMockLisAdapter
{
    Task<MockLisLookupResult> QueryAsync(string normalizedCode, CancellationToken cancellationToken = default);
}

public sealed class MockLisAdapter(StainerDbContext dbContext) : IMockLisAdapter
{
    public async Task<MockLisLookupResult> QueryAsync(string normalizedCode, CancellationToken cancellationToken = default)
    {
        var entries = await dbContext.MockLisEntries
            .AsNoTracking()
            .Where(x => x.IsEnabled && x.NormalizedCode == normalizedCode)
            .OrderBy(x => x.PrimaryAntibodyCode)
            .ToListAsync(cancellationToken);

        if (entries.Any(x => x.Scenario == MockLisScenario.Exception))
        {
            throw new MockLisException("mock_lis_exception", $"Mock LIS exception configured for {normalizedCode}.");
        }

        if (entries.Any(x => x.Scenario == MockLisScenario.Timeout))
        {
            return MockLisLookupResult.Timeout($"Mock LIS timeout configured for {normalizedCode}.");
        }

        if (entries.Any(x => x.Scenario == MockLisScenario.NoResult))
        {
            return MockLisLookupResult.NoResult();
        }

        var candidates = entries
            .Where(x => x.Scenario == MockLisScenario.Candidate && !string.IsNullOrWhiteSpace(x.PrimaryAntibodyCode))
            .Select(x => x.PrimaryAntibodyCode!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();

        return candidates.Count == 0
            ? MockLisLookupResult.NoResult()
            : MockLisLookupResult.Candidates(candidates);
    }
}

public sealed record MockLisLookupResult(
    string Status,
    IReadOnlyList<string> CandidatePrimaryAntibodyCodes,
    string? ErrorCode,
    string? ErrorMessage)
{
    public static MockLisLookupResult Candidates(IReadOnlyList<string> candidates) =>
        new(candidates.Count > 1 ? LisQueryStatus.MultipleCandidates : LisQueryStatus.SingleCandidate, candidates, null, null);

    public static MockLisLookupResult NoResult() =>
        new(LisQueryStatus.NoResult, [], "lis_not_found", "LIS lookup returned no primary antibody code.");

    public static MockLisLookupResult Timeout(string message) =>
        new(LisQueryStatus.TimedOut, [], "lis_timeout", message);
}

public sealed class MockLisException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
