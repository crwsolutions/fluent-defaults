using FluentDefaults;
using FluentDetails.Sample.Model;

namespace FluentDetails.Sample.Defaults;

public class PersonDefaulter : AbstractDefaulter<Person>
{
    public PersonDefaulter()
    {
        DefaultFor(x => x.Id, () => Guid.NewGuid());
        DefaultFor(x => x.IsVip, false);
        DefaultFor(x => x.Discount, 20m).When(x => x.IsVip == true);
        DefaultFor(x => x.Discount, 10m).When(x => x.IsVip == false);
    }
}