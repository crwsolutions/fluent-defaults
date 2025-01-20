using FluentDefaults.Tests.Model;

namespace FluentDefaults.Tests;

public class FunctionWithInstanceDefaultForTests
{
    [Fact]
    public void IntWithDefaultFunctionWithInstance_ShouldBeSetToDefault()
    {
        var customer = new Customer
        {
            Number1 = 1
        };
        var defaulter = new FunctionWithInstanceCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(1, customer.Number4);
        Assert.Equal(1, customer.FieldNumber3);
        Assert.Equal(2, customer.NullableNumber2);
    }
}

internal sealed class FunctionWithInstanceCustomerDefaulter : AbstractDefaulter<Customer>
{
    internal FunctionWithInstanceCustomerDefaulter()
    {
        DefaultFor(x => x.Number4).Is(x => x.Number1);
        DefaultFor(x => x.FieldNumber3, (x) => x.Number1);
        DefaultFor(x => x.NullableNumber2).Is(x => Calculate(x.Number1));
    }

    private int? Calculate(int number1) => number1 + 1;
}
