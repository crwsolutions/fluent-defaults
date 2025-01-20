namespace FluentDefaults.Tests.Model;
internal sealed class Customer
{
    public int Number1 { get; set; }
    public int? NullableNumber2 { get; set; }

    public int FieldNumber3;

    public int Number4 { get; set; }

    public int Number5 { get; set; } = 5;

    public Address[] Addresses1 { get; set; } = [new Address()];
    public List<Address> Addresses2 { get; set; } = [new Address(), new Address(), new Address()];

    public Address Address1 { get; set; } = new Address();
    public Address Address2 { get; set; } = default!;
    public Address Address3 { get; set; } = default!;
}
