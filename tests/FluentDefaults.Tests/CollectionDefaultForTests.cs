namespace FluentDefaults.Tests;

public class CollectionDefaultForTests
{
    [Fact]
    public void IntWithFailingCondition_ShouldNotBeSetToDefault()
    {
        var customer = new CollectionCustomer();
        var defaulter = new CollectionCustomerDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("Default Street", customer.Addresses1[0].Street);
        Assert.Equal("Default Street", customer.Addresses2.First().Street);
    }
}
public class CollectionAddressDefaulter : AbstractDefaulter<CollectionAddress>
{
    public CollectionAddressDefaulter()
    {
        DefaultFor(x => x.Street, "Default Street");
        DefaultFor(x => x.City, "Default City");
    }
}

public class CollectionCustomerDefaulter : AbstractDefaulter<CollectionCustomer>
{
    public CollectionCustomerDefaulter()
    {
        DefaultForEach(x => x.Addresses1).SetDefaulter(new CollectionAddressDefaulter());
        DefaultForEach(x => x.Addresses2).SetDefaulter(new CollectionAddressDefaulter());
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
