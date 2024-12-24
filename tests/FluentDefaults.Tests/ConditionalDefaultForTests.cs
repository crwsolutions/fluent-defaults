namespace FluentDefaults.Tests;

public class ConditionalDefaultForTests
{
    [Fact]
    public void IntWithPassingCondition_ShouldBeSetToDefault()
    {
        var customer = new ConditionalCustomer();
        var defaulter = new ConditionalCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(0, customer.Number3);
    }

    [Fact]
    public void IntWithFailingCondition_ShouldNotBeSetToDefault()
    {
        var customer = new ConditionalCustomer();
        var defaulter = new ConditionalCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(4, customer.Number4);
    }
}
public class ConditionalCustomer
{
    public int Number1 { get; set; } = 5;
    public int Number3 { get; set; }
    public int Number4 { get; set; }
}

public class ConditionalCustomerDefaulter : AbstractDefaulter<ConditionalCustomer>
{
    public ConditionalCustomerDefaulter()
    {
        DefaultFor(x => x.Number3, 3).When(x => x.Number1 != 5);
        DefaultFor(x => x.Number4, () => 4).When(x => x.Number1 == 5);
    }
}

