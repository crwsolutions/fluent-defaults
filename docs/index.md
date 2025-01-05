---
layout: default
title: "Home"
---

# FluentDefaults Documentation

Welcome to the documentation for the `FluentDefaults` library. This library provides a fluent interface for defining and applying default values to properties or fields of an object.

Example:
```csharp
public class PersonDefaulter : AbstractDefaulter<Person>
{
    public PersonDefaulter()
    {
        DefaultFor(x => x.Id).Is(() => Guid.NewGuid());
        DefaultFor(x => x.IsVip).Is(false);
        DefaultFor(x => x.Discount).Is(20m).When(x => x.IsVip == true);
        DefaultFor(x => x.Discount).Is(10m).When(x => x.IsVip == false);
    }
}
```

## Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [Creating your first defaulter](#creating-your-first-defaulter)

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

```csharp
var customer = new Customer();
var defaulter = new CustomerDefaulter();

defaulter.Apply(customer);

Console.WriteLine(customer.Number1); // Output: 1 
Console.WriteLine(customer.Number2); // Output: 2 
Console.WriteLine(customer.Number3); // Output: 3
```
