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
        builder.Services.AddScoped<IValidator<Person>, PersonValidator>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapPost("/validate", (HttpContext httpContext, [FromBody] Person person, [FromServices] PersonService personService) =>
        {
            return personService.Validate(person);
        });

        app.Run();
    }
}
