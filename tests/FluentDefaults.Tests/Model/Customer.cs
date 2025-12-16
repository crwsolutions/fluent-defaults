namespace FluentDefaults.Tests.Model;
internal sealed class Customer
{
    public int Number1 { get; set; }
    public int? NullableNumber2 { get; set; }

    public int FieldNumber3 = default;

    public int Number4 { get; set; }

    public int Number5 { get; set; } = 5;

    public Address[] Addresses1 { get; set; } = [new Address()];
    public List<Address> Addresses2 { get; set; } = [new Address(), new Address(), new Address()];
    public Address[] Addresses3 { get; set; } = [new Address()];
    public Dictionary<string, Address> Addresses4 { get; set; } = new Dictionary<string, Address>
    {
        { "key1", new Address() },
        { "key2", new Address() },
        { "key3", new Address() }
    };

    public Dictionary<string, Address> Addresses5 { get; set; } = new Dictionary<string, Address>
    {
        { "key1", new Address() },
        { "key2", new Address() },
        { "key3", new Address() }
    };

    public Dictionary<string, Address> Addresses6 { get; set; } = new Dictionary<string, Address>
    {
        { "key1", new Address() },
        { "key2", new Address() },
        { "key3", new Address() }
    };

    public Dictionary<string, Address> Addresses7 { get; set; } = new Dictionary<string, Address>
    {
        { "key1", new Address() },
        { "key2", new Address() },
        { "key3", new Address() }
    };

    public Address Address1 { get; set; } = new Address();
    public Address Address2 { get; set; } = default!;
    public Address Address3 { get; set; } = default!;
    public Address Address4 { get; set; } = default!;
    public Address? OptionalAddress1 { get; set; }
    public Address? OptionalAddress2 { get; set; }
}
