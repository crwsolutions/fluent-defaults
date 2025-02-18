---
layout: default
title: "Chaining Methods"
---

# Chaining Method Extensions

The `ApplyDefaulter` and `ApplyDefaulterAsync` extension methods allow you to apply defaulters to an object. These methods support chaining, enabling you to perform multiple actions on an object in a fluid manner.

## Example Usage
Both Apply and ApplyAsync support method chaining, making it easy to perform multiple actions on an object in a single statement. Here is an example:

```csharp
// Chaining synchronous and asynchronous defaulters along with other operations
await dto.Validate()
          .ApplyDefaults(defaulter)
          .MapToDomainModel()
          .ProcessAsync();
```
