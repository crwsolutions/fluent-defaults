using FluentDefaults.Tests.Model;

namespace FluentDefaults.Tests;

public class SimpleDefaultForTests
{
    [Fact]
    public void Int_ShouldBeSetToDefault()
    {
        var customer = new Customer();
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(1, customer.Number1);
    }

    [Fact]
    public void NullableInt_ShouldBeSetToDefault()
    {
        var customer = new Customer();
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(2, customer.NullableNumber2);
    }

    [Fact]
    public void IntWithValue_ShouldNotBeSetToDefault()
    {
        var customer = new Customer
        {
            Number1 = 3
        };
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(3, customer.Number1);
    }

    [Fact]
    public void NullableIntWithValue_ShouldNotBeSetToDefault()
    {
        var customer = new Customer
        {
            NullableNumber2 = 4
        };
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(4, customer.NullableNumber2);
    }

    [Fact]
    public void NullableIntWith0_ShouldNotBeSetToDefault()
    {
        var customer = new Customer
        {
            NullableNumber2 = 0
        };
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(0, customer.NullableNumber2);
    }

    [Fact]
    public void FieldInt_ShouldBeSetToDefault()
    {
        var customer = new Customer();
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(3, customer.FieldNumber3);
    }
}

internal sealed class CustomerDefaulter : AbstractDefaulter<Customer>
{
    internal CustomerDefaulter()
    {
        DefaultFor(x => x.Number1).Is(1); // Default Number5 to 1
        DefaultFor(x => x.NullableNumber2).Is(2); // Default NullableNumber2 to null if not set
        DefaultFor(x => x.FieldNumber3).Is(3);
    }
}
