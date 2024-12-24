namespace FluentDefaults.Tests;

public class ComplexDefaultForTests
{
    [Fact]
    public void IntWithFailingCondition_ShouldNotBeSetToDefault()
    {
        var customer = new ComplexCustomer();
        var defaulter = new ComplexCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("Default Street", customer.Address1.Street);
        Assert.Null(customer.Address2);
        Assert.Equal("Default Street", customer.Address3.Street);
    }
}
public class Address4Defaulter : AbstractDefaulter<Address4>
{
    public Address4Defaulter()
    {
        DefaultFor(x => x.Street, "Default Street");
        DefaultFor(x => x.City, "Default City");
    }
}

public class ComplexCustomerDefaulter : AbstractDefaulter<ComplexCustomer>
{
    public ComplexCustomerDefaulter()
    {
        DefaultFor(x => x.Address1).SetDefaulter(new Address4Defaulter());
        DefaultFor(x => x.Address2).SetDefaulter(new Address4Defaulter());
        DefaultFor(x => x.Address3, () => new Address4());
        DefaultFor(x => x.Address3).SetDefaulter(new Address4Defaulter());
    }
}

public class ComplexCustomer
{
    public Address4? Address1 { get; set; } = new Address4();
    public Address4? Address2 { get; set; }
    public Address4? Address3 { get; set; }
}

public class Address4
{
    public string? Street { get; set; }
    public string? City { get; set; }
}

