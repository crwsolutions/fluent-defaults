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
        Assert.Equal("Default Street", customer.Addresses2.First().Street);
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
