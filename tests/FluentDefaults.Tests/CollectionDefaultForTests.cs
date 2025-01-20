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
        var numberedAddresses = customer.Addresses2.ToArray();

        Assert.Equal("- unknown city -", customer.Addresses2.First().City);
        Assert.Equal("- unknown street -", customer.Addresses2.First().Street);
        Assert.Equal(1, numberedAddresses[0].Id);
        Assert.Equal(2, numberedAddresses[1].Id);
        Assert.Equal(3, numberedAddresses[2].Id);
        Assert.Equal("5 street", customer.Addresses1.First().Street);
        Assert.Equal("City in Far, far away", customer.Addresses1.First().City);
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
    int _number = 0;

    internal CollectionCustomerWithDefaultForDefaulter()
    {
        ForEach(x => x.Addresses2).DefaultFor(x => x.Id).Is(GetNumber);
        ForEach(x => x.Addresses2).DefaultFor(x => x.City, "- unknown city -");
        ForEach(x => x.Addresses2).DefaultFor(x => x.Street).Is(() => "- unknown street -");
        ForEach(x => x.Addresses1).DefaultFor(x => x.Street).Is((Customer x) => GetSome(x));
        ForEach(x => x.Addresses1).DefaultFor(x => x.City).Is((Address x) => GetSome(x));
    }

    private string GetSome(Customer x) => $"{x.Number5} street";
    private string GetSome(Address x) => $"City in {x.Region}";

    private int GetNumber() => ++_number;
}
