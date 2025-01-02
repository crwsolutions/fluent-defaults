# Dependency Injection

Defaulters can be used with any dependency injection library, such as `Microsoft.Extensions.DependencyInjection`. To inject a defaulter for a specific model, you should register the defaulter with the service provider as `IDefaulter<T>`, where `T` is the type of object being validated.

For example, imagine you have the following defaulter defined in your project:

```csharp
public class PersonDefaulter : AbstractDefaulter<Person>
{
    public PersonDefaulter()
    {
        DefaultFor(x => x.Discount).Is(20m);
    }
}
```

This defaulter can be registered as `IDefaulter<Person>` in your application's startup routine by calling into the .NET service provider. For example, in a Asp.Net core application the startup routine would look something like this:

```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IDefaulter<Person>, PersonDefaulter>();
    }
}
```

You can then inject the defaulter as you would with any other dependency:

```c#
public class PersonService
{
    private readonly IDefaulter<Person> _defaulter;

    public PersonService(IDefaulter<Person> defaulter)
    {
        _defaulter = defaulter;
    }

    public async Task DoSomething(Person person)
    {
        await _defaulter.Apply(person);
    }
}
```

## Scope

You can register the defaulter as `Scoped`, `Singleton` or `Transient`. If you aren't familiar with the difference between Singleton, Scoped and Transient [please review the Microsoft dependency injection documentation](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes)


```eval_rst
.. warning::
   If you register a defaulter as Singleton, you should ensure that you don't inject anything that's transient or request-scoped into the defaulter. We typically don't recommend registering defaulters as Singleton unless you are experienced with using Dependency Injection and know how to troubleshoot issues related to singleton-scoped objects having on non-singleton dependencies. Registering defaulters as Transient is the simplest and safest option.
```