using FluentDefaults.Tests.Model;

namespace FluentDefaults.Tests;

public class FunctionDefaultForTests
{
    [Fact]
    public void IntWithDefaultFunction_ShouldBeSetToDefault()
    {
        var customer = new Customer();
        var defaulter = new FunctionCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(4, customer.Number4);
    }
}

internal sealed class FunctionCustomerDefaulter : AbstractDefaulter<Customer>
{
    internal FunctionCustomerDefaulter()
    {
        DefaultFor(x => x.Number4).Is(() => 4);
    }
}
