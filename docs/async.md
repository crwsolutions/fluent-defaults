---
layout: default
title: "Async Defaulters"
---

## Async Defaulters

The `FluentDefaults` library supports asynchronous default value definitions using the `AbstractAsyncDefaulter<T>` class.

### Example

```csharp
using FluentDefaults; 
using System.Net.Http; 
using System.Threading.Tasks;

public class Person 
{ 
    public Guid? Id { get; set; }
    public bool? IsVip { get; set; }
    public decimal? Discount { get; set; }
}

public class PersonAsyncDefaulter : AbstractAsyncDefaulter<Person>
{ 
    private readonly HttpClient _httpClient;

    public PersonAsyncDefaulter(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("DefaultsClient");

        DefaultFor(x => x.Id).Is(() => Guid.NewGuid());
        DefaultFor(x => x.IsVip).Is(false);
        DefaultFor(x => x.Discount).IsAsync(GetDefaultDiscount);
    }

    private async Task<decimal?> GetDefaultDiscount()
    {
        var response = await _httpClient.GetAsync("/default-discount");
        response.EnsureSuccessStatusCode();
        var n = await response.Content.ReadAsStringAsync();
        return decimal.Parse(n);
    }
}
```

You can then apply the default values to an instance of the `Person` class asynchronously:

```csharp
var person = new Person();
var defaulter = new PersonAsyncDefaulter(httpClientFactory);

await defaulter.ApplyAsync(person);

Console.WriteLine(person.Id); // Output: A new Guid value 
Console.WriteLine(person.IsVip); // Output: false 
Console.WriteLine(person.Discount); // Output: The value fetched from the /default-discount endpoint
``` 