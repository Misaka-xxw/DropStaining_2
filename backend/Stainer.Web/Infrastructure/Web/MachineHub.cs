using System.Net;
using Microsoft.AspNetCore.SignalR;
using Stainer.Web.Application.Services;

namespace Stainer.Web.Infrastructure.Web;

public sealed class MachineHub(UserSessionService sessionService) : Hub
{
    public const string Route = "/hubs/machine";
    public const string AuthorizedGroup = "machine.authorized";
    public const string AdminGroup = "machine.admin";

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext is null || !IsLocalRequest(httpContext))
        {
            Context.Abort();
            return;
        }

        var user = await sessionService.GetCurrentUserAsync(httpContext, Context.ConnectionAborted);
        if (user is null)
        {
            Context.Abort();
            return;
        }

        Context.Items["userId"] = user.UserId;
        Context.Items["activeRole"] = user.ActiveRole;
        await Groups.AddToGroupAsync(Context.ConnectionId, AuthorizedGroup, Context.ConnectionAborted);
        if (user.HasRole("admin"))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, AdminGroup, Context.ConnectionAborted);
        }

        await base.OnConnectedAsync();
    }

    private static bool IsLocalRequest(HttpContext httpContext)
    {
        var remoteIp = httpContext.Connection.RemoteIpAddress;
        var localIp = httpContext.Connection.LocalIpAddress;
        return remoteIp is null
            || IPAddress.IsLoopback(remoteIp)
            || (localIp is not null && remoteIp.Equals(localIp));
    }
}
