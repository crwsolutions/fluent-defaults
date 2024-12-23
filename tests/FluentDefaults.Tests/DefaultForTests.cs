namespace FluentDefaults.Tests;

public class DefaultForTests
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

        Assert.Equal(2, customer.Number2);
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
            Number2 = 4
        };
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(4, customer.Number2);
    }

    [Fact]
    public void NullableIntWith0_ShouldNotBeSetToDefault()
    {
        var customer = new Customer
        {
            Number2 = 0
        };
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(0, customer.Number2);
    }

    [Fact]
    public void FieldInt_ShouldBeSetToDefault()
    {
        var customer = new Customer();
        var defaulter = new CustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal(3, customer.Number3);
    }
}

public class Customer
{
    public int Number1 { get; set; }
    public int? Number2 { get; set; }

    public int Number3;
}

public class CustomerDefaulter : AbstractDefaulter<Customer>
{
    public CustomerDefaulter()
    {
        DefaultFor(x => x.Number1, 1); // Default Number1 to 1
        DefaultFor(x => x.Number2, 2); // Default Number2 to null if not set
        DefaultFor(x => x.Number3, 3); // Default Number2 to null if not set
    }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}

public class InvoiceLine
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
}

public class Invoice
{
    public Address Address { get; set; }
    public List<InvoiceLine> Lines { get; set; }
    public decimal Total;
}

