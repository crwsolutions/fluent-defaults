---
layout: default
title: "Collections"
---

## Async Defaulters

The `FluentDefaults` library supports default values for collections using the `ForEach` method.

### Example

```csharp
internal sealed class CollectionCustomerWithDefaultForDefaulter : AbstractDefaulter<CollectionCustomer>
{
    internal CollectionCustomerWithDefaultForDefaulter()
    {
        ForEach(x => x.Addresses).DefaultFor(x => x.Street).Is("- unknown street -"); //Or:
        ForEach(x => x.Addresses).DefaultFor(x => x.City, "- unknown city -");
    }
}

public class CollectionCustomer
{
    public CollectionAddress[] Addresses { get; set; } = [new CollectionAddress()];
}

public class CollectionAddress
{
    public string? Street { get; set; }
    public string? City { get; set; }
}
```

## SetDefaulter()

It is also possible to set a defaulter for each item in a collection using the `SetDefaulter` method.

### Example

```csharp
using FluentDefaults;

public class CollectionAddressDefaulter : AbstractDefaulter<CollectionAddress>
{
    public CollectionAddressDefaulter()
    {
        DefaultFor(x => x.Street).Is("- unknown street -");
    }
}

public class CollectionCustomerDefaulter : AbstractDefaulter<CollectionCustomer>
{
    public CollectionCustomerDefaulter()
    {
        ForEach(x => x.Addresses).SetDefaulter(new CollectionAddressDefaulter());
    }
}
```

You can then apply the default values to an instance of the `CollectionCustomer` class:

```csharp
var customer = new CollectionCustomer();
var defaulter = new CollectionCustomerDefaulter();

defaulter.Apply(customer);

Console.WriteLine(customer.Addresses1[0].Street); // Output: '- unknown street -'
```