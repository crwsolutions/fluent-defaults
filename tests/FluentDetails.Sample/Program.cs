using FluentDefaults;
using FluentDetails.Sample.Defaults;
using FluentDetails.Sample.Model;
using FluentDetails.Sample.Services;
using FluentDetails.Sample.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FluentDetails.Sample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddScoped<PersonService>();
        builder.Services.AddScoped<IDefaulter<Person>, PersonDefaulter>();
        builder.Services.AddScoped<IAsyncDefaulter<Person>, PersonAsyncDefaulter>();
        builder.Services.AddScoped<IValidator<Person>, PersonValidator>();

        // Register the HTTP client
        builder.Services.AddHttpClient("DefaultsClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7256");
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("/default-async", async (HttpContext httpContext, [FromServices] PersonService personService) =>
        {
            return await personService.DefaultAsync();
        });

        app.MapPost("/validate", (HttpContext httpContext, [FromBody] Person person, [FromServices] PersonService personService) =>
        {
            return personService.Validate(person);
        });

        app.MapGet("/default-discount", (HttpContext httpContext) =>
        {
            return 15;
        });

        app.Run();
    }
}
