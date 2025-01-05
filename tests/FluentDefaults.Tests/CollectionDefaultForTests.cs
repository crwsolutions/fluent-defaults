namespace FluentDefaults.Tests;

public class CollectionDefaultForTests
{
    [Fact]
    public void CollectionWithDefaulter_ShouldGetThatDefault()
    {
        var customer = new CollectionCustomer();
        var defaulter = new CollectionCustomerWithDefaulterDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("Default Street", customer.Addresses1[0].Street);
        Assert.Equal("Default City", customer.Addresses1[0].City);
    }

    [Fact]
    public void CollectionWithDefaultFor_ShouldGetThatDefault()
    {
        var customer = new CollectionCustomer();
        var defaulter = new CollectionCustomerWithDefaultForDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("- unknown city -", customer.Addresses2.First().City);
        Assert.Equal("- unknown street -", customer.Addresses2.First().Street);
    }
}

public class CollectionAddressDefaulter : AbstractDefaulter<CollectionAddress>
{
    public CollectionAddressDefaulter()
    {
        DefaultFor(x => x.Street).Is("Default Street");
        DefaultFor(x => x.City).Is("Default City");
    }
}

public class CollectionCustomerWithDefaulterDefaulter : AbstractDefaulter<CollectionCustomer>
{
    public CollectionCustomerWithDefaulterDefaulter()
    {
        ForEach(x => x.Addresses1).SetDefaulter(new CollectionAddressDefaulter());
        ForEach(x => x.Addresses2).SetDefaulter(new CollectionAddressDefaulter());
    }
}

public class CollectionCustomerWithDefaultForDefaulter : AbstractDefaulter<CollectionCustomer>
{
    public CollectionCustomerWithDefaultForDefaulter()
    {
        ForEach(x => x.Addresses2).DefaultFor(x => x.City, "- unknown city -");
        ForEach(x => x.Addresses2).DefaultFor(x => x.Street).Is("- unknown street -");
    }
}

public class CollectionCustomer
{
    public CollectionAddress[] Addresses1 { get; set; } = [new CollectionAddress()];
    public List<CollectionAddress> Addresses2 { get; set; } = [new CollectionAddress()];
}

public class CollectionAddress
{
    public string? Street { get; set; }
    public string? City { get; set; }
}
