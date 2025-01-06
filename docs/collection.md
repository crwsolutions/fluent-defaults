---
layout: default
title: "Collections"
---

# Async Defaulters

The `FluentDefaults` library supports default values for collections using the `ForEach` method to apply the same rule to multiple items in a collection.

## ForEach().DefaultFor().Is()

The `ForEach` method is used to iterate over each item in a specified collection property of an object. It allows you to apply rules or actions to each item within the collection. The `DefaultFor` method is used to specify a property of the items within the collection that you want to set a default value for. The `Is` method is used to assign the default value to the specified property. It is called after `DefaultFor` to set the value that should be used if the property is not already set. You can pass a fixed value or a function (eg `() => Guid.NewGuid()`)

### Example with fixed value

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

## ForEach().SetDefaulter()

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