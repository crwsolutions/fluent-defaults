using FluentDefaults.Tests.Model;

namespace FluentDefaults.Tests;

public class ConditionalDefaultForTests
{
    [Fact]
    public void IntWithPassingCondition_ShouldBeSetToDefault()
    {
        var customer = new Customer();
        var defaulter = new ConditionalCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(0, customer.FieldNumber3);
    }

    [Fact]
    public void IntWithFailingCondition_ShouldNotBeSetToDefault()
    {
        var customer = new Customer();
        var defaulter = new ConditionalCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(4, customer.Number4);
    }
}

internal sealed class ConditionalCustomerDefaulter : AbstractDefaulter<Customer>
{
    internal  ConditionalCustomerDefaulter()
    {
        DefaultFor(x => x.FieldNumber3).Is(3).When(x => x.Number5 != 5);
        DefaultFor(x => x.Number4).Is(() => 4).When(x => x.Number5 == 5);
    }
}

