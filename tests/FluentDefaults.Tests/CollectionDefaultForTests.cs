using FluentDefaults.Tests.Model;

namespace FluentDefaults.Tests;

public class CollectionDefaultForTests
{
    [Fact]
    public void CollectionWithDefaulter_ShouldGetThatDefault()
    {
        var customer = new Customer();
        var defaulter = new CollectionCustomerWithDefaulterDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("Default Street", customer.Addresses1[0].Street);
        Assert.Equal("Default City", customer.Addresses1[0].City);
    }

    [Fact]
    public void CollectionWithDefaultFor_ShouldGetThatDefault()
    {
        var customer = new Customer();
        var defaulter = new CollectionCustomerWithDefaultForDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("- unknown city -", customer.Addresses2.First().City);
        Assert.Equal("- unknown street -", customer.Addresses2.First().Street);
    }
}

internal sealed class CollectionAddressDefaulter : AbstractDefaulter<Address>
{
    internal CollectionAddressDefaulter()
    {
        DefaultFor(x => x.Street).Is("Default Street");
        DefaultFor(x => x.City).Is("Default City");
    }
}

internal sealed class CollectionCustomerWithDefaulterDefaulter : AbstractDefaulter<Customer>
{
    internal CollectionCustomerWithDefaulterDefaulter()
    {
        ForEach(x => x.Addresses1).SetDefaulter(new CollectionAddressDefaulter());
        ForEach(x => x.Addresses2).SetDefaulter(new CollectionAddressDefaulter());
    }
}

internal sealed class CollectionCustomerWithDefaultForDefaulter : AbstractDefaulter<Customer>
{
    internal CollectionCustomerWithDefaultForDefaulter()
    {
        ForEach(x => x.Addresses2).DefaultFor(x => x.City, "- unknown city -");
        ForEach(x => x.Addresses2).DefaultFor(x => x.Street).Is(() => "- unknown street -");
    }
}
