namespace Stainer.Web.Application.Services;

using Microsoft.AspNetCore.Http;

public sealed class BusinessRuleException : Exception
{
    public BusinessRuleException(string code, string message, int statusCode = StatusCodes.Status400BadRequest)
        : base(message)
    {
        Code = code;
        StatusCode = statusCode;
    }

    public string Code { get; }
    public int StatusCode { get; }
}
