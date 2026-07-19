using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Stainer.Tests;

public sealed class FormalPageAccessIntegrationTests
{
    [Theory]
    [InlineData("Development", true)]
    [InlineData("Testing", true)]
    [InlineData("Staging", false)]
    [InlineData("Production", false)]
    public async Task Legacy_runtime_compatibility_routes_are_limited_to_development_and_testing(
        string environment,
        bool expectedMapped)
    {
        await using var factory = CreateFactory(environment);
        using var client = factory.CreateClient();

        var state = await client.GetAsync("/api/state");
        Assert.Equal(expectedMapped ? HttpStatusCode.OK : HttpStatusCode.NotFound, state.StatusCode);

        foreach (var action in new[] { "start", "pause", "resume", "stop" })
        {
            var response = await client.PostAsync($"/api/run/{action}", null);
            Assert.Equal(expectedMapped ? HttpStatusCode.Unauthorized : HttpStatusCode.NotFound, response.StatusCode);
        }

        // /control-console 现在在所有环境都直接返回数字孪生页（不再受 legacy 运行时兼容开关控制，也无旧版 iframe 外壳）。
        var controlConsole = await client.GetStringAsync("/control-console");
        Assert.Contains("api/twin/snapshot", controlConsole, StringComparison.Ordinal);
        Assert.DoesNotContain("controlConsoleFrame", controlConsole, StringComparison.Ordinal);

        var login = await client.PostAsJsonAsync("/api/login", new { username = "operator", password = "123456", role = "operator" });
        var loginBody = await login.Content.ReadFromJsonAsync<Stainer.Web.Application.ReadModels.LoginResponse>();
        Assert.Equal("/control-console", loginBody?.Redirect);
    }

    [Fact]
    public async Task Management_and_engineering_reads_enforce_roles()
    {
        await using var factory = CreateFactory();
        using var anonymous = factory.CreateClient();
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync("/api/users")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync("/api/roles")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync("/api/engineering/coordinate-profiles")).StatusCode);

        using var operatorClient = factory.CreateClient();
        await LoginAsync(operatorClient, "operator", "operator");
        Assert.Equal(HttpStatusCode.Forbidden, (await operatorClient.GetAsync("/api/users")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await operatorClient.GetAsync("/api/engineering/liquid-classes")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await operatorClient.GetAsync("/api/engineering/diagnostics/command-log")).StatusCode);

        using var adminClient = factory.CreateClient();
        await LoginAsync(adminClient, "admin", "admin");
        Assert.Equal(HttpStatusCode.OK, (await adminClient.GetAsync("/api/users")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await adminClient.GetAsync("/api/roles")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await adminClient.GetAsync("/api/engineering/coordinate-profiles")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await adminClient.GetAsync("/api/engineering/liquid-classes")).StatusCode);
    }

    [Fact]
    public async Task Legacy_mock_write_contracts_remain_unmapped()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();

        // 旧页面专属脚本/模板已随旧页面删除。这里只保留后端契约的回归守卫：
        // 这些旧版 Mock 写契约从未映射（或仅在 Development 映射），Testing 环境下应保持 404。
        // 注：/api/dab 受并行 DAB 改造影响，不在此断言，避免与无关改动耦合。
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/api/logs")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.PostAsJsonAsync("/api/engineer/command", new { module = "serial", action = "connect" })).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.PostAsJsonAsync("/api/slides/configure", new { })).StatusCode);
    }

    private static WebApplicationFactory<Program> CreateFactory(string environment = "Testing")
    {
        var databasePath = Path.Combine(TestPaths.TempRoot, "stainer-formal-page-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment(environment);
            builder.UseSetting("ConnectionStrings:StainerDatabase", $"Data Source={databasePath}");
            builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:StainerDatabase"] = $"Data Source={databasePath}"
            }));
        });
    }

    private static async Task LoginAsync(HttpClient client, string username, string role)
    {
        var response = await client.PostAsJsonAsync("/api/login", new { username, password = "123456", role });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
