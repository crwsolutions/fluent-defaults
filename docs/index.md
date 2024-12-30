# FluentDefaults Documentation

Welcome to the documentation for the `FluentDefaults` library. This library provides a fluent interface for defining and applying default values to properties or fields of an object.

## Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [Creating your first defaulter](#creating-your-first-defaulter)
- [Complex Properties](#complex-properties)
- [Async Defaulters](#async-defaulters)

## Overview

The `FluentDefaults` library allows you to define default values for properties or fields of an object using a fluent interface. It supports both synchronous and asynchronous default value definitions.

## Installation

To install the `FluentDefaults` library, you can use the NuGet package manager:

```bash
dotnet add package FluentDefaults
```

## Creating your first defaulter

To create your first defaulter, you need to define a class that inherits from `AbstractDefaulter<T>` and specify the default values for the properties of your target class.

### Example

```csharp
using FluentDefaults;

public class Customer 
{ 
    public int Number1 { get; set; }
    public int? Number2 { get; set; }
    public int Number3; 
}

public class CustomerDefaulter : AbstractDefaulter<Customer>
{ 
   public CustomerDefaulter()
   { 
       DefaultFor(x => x.Number1, 1); // Default Number1 to 1 
       DefaultFor(x => x.Number2, 2); // Default Number2 to 2 if not set 
       DefaultFor(x => x.Number3, 3); // Default Number3 to 3 
   }
}
```

You can then apply the default values to an instance of the `Customer` class:

var customer = new Customer();
var defaulter = new CustomerDefaulter();

defaulter.Apply(customer);

Console.WriteLine(customer.Number1); // Output: 1 
Console.WriteLine(customer.Number2); // Output: 2 
Console.WriteLine(customer.Number3); // Output: 3

## Complex Properties

The `FluentDefaults` library also supports defining default values for complex properties and collections.

### Example

```csharp
using FluentDefaults;

public class Order 
{ 
    public List Items { get; set; }
}

public class Item 
{ 
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class OrderDefaulter : AbstractDefaulter<Order>
{ 
    public OrderDefaulter()
    { 
        DefaultForEach(x => x.Items).DefaultFor(x => x.Price, 9.99m);
    }
}
```

You can then apply the default values to an instance of the `Order` class:

```csharp
var order = new Order 
{ 
    Items = new List { 
        new Item { Name = "Item1" },
        new Item { Name = "Item2", Price = 19.99m }
    }
};

var defaulter = new OrderDefaulter();
defaulter.Apply(order);

Console.WriteLine(order.Items[0].Price); // Output: 9.99 
Console.WriteLine(order.Items[1].Price); // Output: 19.99
```


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