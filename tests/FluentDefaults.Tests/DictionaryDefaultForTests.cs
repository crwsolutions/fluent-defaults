using FluentDefaults.Tests.Model;

namespace FluentDefaults.Tests;

public class DictionaryDefaultForTests
{
    [Fact]
    public void DictionaryWithDefaulter_ShouldGetThatDefault()
    {
        var customer = new Customer();
        customer.Addresses4.First().Value.Specs = [new() { Spec = SpecsEnum.Garage }];
        var defaulter = new DictionaryCustomerWithDefaulterDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("Default Street", customer.Addresses4["key1"].Street);
        Assert.Equal("Default City", customer.Addresses4["key1"].City);
        Assert.Equal("Street 1", customer.Addresses5["key1"].Street);
        Assert.Equal("", customer.Addresses4["key1"].Specs!.First().Description);
    }

    [Fact]
    public void DictionaryWithDefaultFor_ShouldGetThatDefault()
    {
        var customer = new Customer();

        var defaulter = new DictionaryCustomerWithDefaultForDefaulter();

        defaulter.Apply(customer);
        var numberedAddresses = customer.Addresses5;

        Assert.Equal("- unknown city -", customer.Addresses5.First().Value.City);
        Assert.Equal("- unknown street -", customer.Addresses5.First().Value.Street);
        Assert.Equal(1, numberedAddresses["key1"].Id);
        Assert.Equal(2, numberedAddresses["key2"].Id);
        Assert.Equal(3, numberedAddresses["key3"].Id);
        Assert.Equal("5 street", customer.Addresses4.First().Value.Street);
    }
}

internal sealed class DictionaryAddressDefaulter : AbstractDefaulter<KeyValuePair<string, Address>>
{
    internal DictionaryAddressDefaulter()
    {
        DefaultFor(x => x.Value.Street).Is("Default Street");
        DefaultFor(x => x.Value.City).Is("Default City");
        ForEach(x => x.Value.Specs).SetDefaulter(new HouseSpecDefaulter());
    }
}

internal sealed class HouseSpecDefaulter : AbstractDefaulter<HouseSpec>
{
    internal HouseSpecDefaulter()
    {
        DefaultFor(x => x.Description).Is("");
    }
}

internal sealed class DictionaryAddressDefaulterWithParameter : AbstractDefaulter<KeyValuePair<string, Address>>
{
    internal DictionaryAddressDefaulterWithParameter(Customer customer)
    {
        DefaultFor(x => x.Value.Street).Is($"Street {customer.Number1}");
    }
}

internal sealed class DictionaryCustomerWithDefaulterDefaulter : AbstractDefaulter<Customer>
{
    internal DictionaryCustomerWithDefaulterDefaulter()
    {
        DefaultFor(x => x.Number1).Is(1);
        ForEach(x => x.Addresses4).SetDefaulter(new DictionaryAddressDefaulter());
        ForEach(x => x.Addresses5).SetDefaulter((x) => new DictionaryAddressDefaulterWithParameter(x));
    }
}

internal sealed class DictionaryCustomerWithDefaultForDefaulter : AbstractDefaulter<Customer>
{
    int _number = 0;

    internal DictionaryCustomerWithDefaultForDefaulter()
    {
        ForEach(x => x.Addresses5).DefaultFor(x => x.Value.City).Is("- unknown city -");
        ForEach(x => x.Addresses5).DefaultFor(x => x.Value.Street).Is(() => "- unknown street -");
        ForEach(x => x.Addresses5).DefaultFor(x => x.Value.Id).Is(GetNumber);
        ForEach(x => x.Addresses4).DefaultFor(x => x.Value.Street).Is(GetSome);
    }

    private string GetSome(Customer x) => $"{x.Number5} street";

    private int GetNumber() => ++_number;
}

