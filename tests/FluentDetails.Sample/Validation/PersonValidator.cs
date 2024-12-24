using FluentDetails.Sample.Model;
using FluentValidation;

namespace FluentDetails.Sample.Validation;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Name).Length(0, 10);
        RuleFor(x => x.Email).NotNull().EmailAddress();
        RuleFor(x => x.Discount).InclusiveBetween(18, 60);
    }
}