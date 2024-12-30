using FluentDefaults;
using FluentDetails.Sample.Model;
using FluentValidation;

namespace FluentDetails.Sample.Services;

public partial class PersonService
{
    [Dependency]
    private readonly IValidator<Person> _validator;

    [Dependency]
    private readonly IDefaulter<Person> _defaulter;

    [Dependency]
    private readonly IAsyncDefaulter<Person> _asyncDefaulter;

    public FluentValidation.Results.ValidationResult Validate(Person person)
    {
        _defaulter.Apply(person);
        return _validator.Validate(person);
    }

    public async Task<Person> DefaultAsync()
    {
        var person = new Person();
        await _asyncDefaulter.ApplyAsync(person);
        return person;
    }
}
