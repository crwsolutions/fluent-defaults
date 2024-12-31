# FluentDefaults
A library for .NET that uses a fluent interface for setting default values, similar to how FluentValidation handles validations. It promotes cleaner code, better separation of concerns, and integrates well with existing .NET frameworks.

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