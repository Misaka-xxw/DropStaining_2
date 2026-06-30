using Microsoft.AspNetCore.Http;

namespace Stainer.Web.Application.Services;

public sealed class HospitalBarcodeNormalizer
{
    public string Normalize(string? rawCode)
    {
        var normalized = (rawCode ?? string.Empty).Trim(' ', '\r', '\n', '\t');
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException("hospital_code_required", "Hospital barcode is required.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }
}
