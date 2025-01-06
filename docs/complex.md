---
layout: default
title: "Complex properties"
---

# Complex Properties

The `FluentDefaults` library also supports defining default values for complex properties and collections.

## DefaultFor().SetDefaulter()

The `DefaultFor().SetDefaulter()` method allows you to set a defaulter for a complex property within an object. This is useful when you need to apply default values to nested properties or objects within a parent object.

### Example

```csharp
using FluentDefaults;

internal sealed class ComplexAddressDefaulter : AbstractDefaulter<Address>
{
    internal ComplexAddressDefaulter()
    {
        DefaultFor(x => x.Street, "- unknown street -");
    }
}

internal sealed class ComplexCustomerDefaulter : AbstractDefaulter<Customer>
{
    internal ComplexCustomerDefaulter()
    {
        DefaultFor(x => x.Address).SetDefaulter(new ComplexAddressDefaulter());
    }
}

public class Customer
{
    public Address Address { get; set; } = new CollectionAddress();
}

public class CollectionAddress
{
    public string? Street { get; set; }
    public string? City { get; set; }
}
```

You can then apply the default values to an instance of the `Order` class:

```csharp
var customer = new Customer();
var defaulter = new ComplexCustomerDefaulter();

defaulter.Apply(customer);

Console.WriteLine(customer.Address.Street); // Output: '- unknown street -'
```