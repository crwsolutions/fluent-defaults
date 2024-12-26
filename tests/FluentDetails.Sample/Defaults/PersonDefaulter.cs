using FluentDefaults;
using FluentDetails.Sample.Model;

namespace FluentDetails.Sample.Defaults;

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