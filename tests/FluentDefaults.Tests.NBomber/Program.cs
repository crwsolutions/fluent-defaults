using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace FluentDefaults.Tests.NBomber;

internal class Program
{
    static void Main(string[] args)
    {
        using var httpClient = new HttpClient();

        var scenario = Scenario.Create(
            "fetch_default_async",
            async context =>
            {
                var request = Http.CreateRequest("GET", "https://localhost:7256/default-async")
                    .WithHeader("Content-Type", "application/json");

                var response = await Http.Send(httpClient, request);

                return response.IsError ? Response.Fail() : Response.Ok();
            }
        );

        NBomberRunner.RegisterScenarios(scenario).Run();
    }
}
