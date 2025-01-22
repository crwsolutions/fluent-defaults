using FluentDefaults.Tests.Model;

namespace FluentDefaults.Tests;

public class ComplexDefaultForTests
{
    [Fact]
    public void IntWithFailingCondition_ShouldNotBeSetToDefault()
    {
        var customer = new Customer();
        var defaulter = new ComplexCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("Default Street", customer.Address1.Street);
        Assert.Null(customer.Address2);
        Assert.Equal("Default Street", customer.Address3.Street);
        Assert.Equal("Default Street", customer.OptionalAddress1!.Street);
        Assert.Null(customer.OptionalAddress2);
    }
}
internal sealed class ComplexAddressDefaulter : AbstractDefaulter<Address>
{
    internal ComplexAddressDefaulter()
    {
        DefaultFor(x => x.Street).Is("Default Street");
        DefaultFor(x => x.City).Is("Default City");
    }
}

 internal sealed class ComplexAddressWithConstructorDefaulter : AbstractDefaulter<Address>
{
    private readonly Customer _customer;

    internal ComplexAddressWithConstructorDefaulter(Customer customer)
    {
        _customer = customer;

        DefaultFor(x => x.Street).Is("Default Street");
        DefaultFor(x => x.City).Is("Default City");
    }
}

internal sealed class ComplexCustomerDefaulter : AbstractDefaulter<Customer>
{
    internal ComplexCustomerDefaulter()
    {
        DefaultFor(x => x.Address1).SetDefaulter(new ComplexAddressDefaulter());
        DefaultFor(x => x.Address2).SetDefaulter(new ComplexAddressDefaulter());
        DefaultFor(x => x.Address3).Is(() => new Address());
        DefaultFor(x => x.Address3).SetDefaulter(new ComplexAddressDefaulter());
        DefaultFor(x => x.Address4).SetDefaulter((x) => new ComplexAddressWithConstructorDefaulter(x));
        DefaultFor(x => x.OptionalAddress1).Is(new Address());
        DefaultFor(x => x.OptionalAddress1!).SetDefaulter(new ComplexAddressDefaulter());
        DefaultFor(x => x.OptionalAddress2!).SetDefaulter(new ComplexAddressDefaulter());
    }
}
