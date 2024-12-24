namespace FluentDefaults.Tests;

public class FunctionDefaultForTests
{
    [Fact]
    public void IntWithDefaultFunction_ShouldBeSetToDefault()
    {
        var customer = new FunctionCustomer();
        var defaulter = new FunctionCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(4, customer.Number4);
    }
}

public class FunctionCustomer
{
    public int Number4 { get; set; }
}

public class FunctionCustomerDefaulter : AbstractDefaulter<FunctionCustomer>
{
    public FunctionCustomerDefaulter()
    {
        DefaultFor(x => x.Number4, () => 4); 
    }
}
