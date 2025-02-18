using FluentDefaults.Tests.Model;

namespace FluentDefaults.Tests;

public class IEnumeratorDefaultForTests
{
    [Fact]
    public void IEnumeratorWithDefaulter_ShouldGetThatDefault()
    {
        var customer = new EnumeratorCustomer();
        customer.Addresses = GetAddresses().ToList();
        var defaulter = new IEnumeratorCustomerWithDefaulterDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("Default Street", customer.Addresses.First().Street);
        Assert.Equal("Default City", customer.Addresses.First().City);
    }

    [Fact]
    public void IEnumeratorWithDefaulter_ApplyShouldThrowException_WhenCollectionHasDeferredExecution()
    {
        // Arrange
        var customer = new EnumeratorCustomer();
        customer.Addresses = GetAddresses(); // Returns an IEnumerable, not a list or array
        var defaulter = new IEnumeratorCustomerWithDefaulterDefaulter();

        // Act & Assert
        var exception = Assert.Throws<DeferredExecutionException>(() =>
        {
            defaulter.Apply(customer);
        });

        Assert.Equal("Deferred execution detected on member 'x.Addresses'. Please ensure the collection is materialized before processing.", exception.Message);
        Assert.Equal("x.Addresses", exception.MemberName);
    }

    [Fact]
    public void IEnumeratorWithCollectionRule_ShouldGetThatDefault()
    {
        var customer = new EnumeratorCustomer();
        customer.Addresses1 = GetAddresses().ToList();
        var defaulter = new IEnumeratorCustomerWithDefaulterDefaulter();

        defaulter.Apply(customer);

        Assert.Equal("Some Street", customer.Addresses1.First().Street);
    }

    [Fact]
    public void IEnumeratorWithCollectionRule_ApplyShouldThrowException_WhenCollectionHasDeferredExecution()
    {
        // Arrange
        var customer = new EnumeratorCustomer();
        customer.Addresses1 = GetAddresses(); // Returns an IEnumerable, not a list or array
        var defaulter = new IEnumeratorCustomerWithDefaulterDefaulter();

        // Act & Assert
        var exception = Assert.Throws<DeferredExecutionException>(() =>
        {
            defaulter.Apply(customer);
        });

        Assert.Equal("Deferred execution detected on member 'x.Addresses1 => x.Street'. Please ensure the collection is materialized before processing.", exception.Message);
        Assert.Equal("x.Addresses1 => x.Street", exception.MemberName);
    }

    internal IEnumerable<Address> GetAddresses()
    {
        yield return new Address();
    }
}

internal sealed class IEnumeratorAddressDefaulter : AbstractDefaulter<Address>
{
    internal IEnumeratorAddressDefaulter()
    {
        DefaultFor(x => x.Street).Is("Default Street");
        DefaultFor(x => x.City).Is("Default City");
    }
}

internal sealed class IEnumeratorCustomerWithDefaulterDefaulter : AbstractDefaulter<EnumeratorCustomer>
{
    internal IEnumeratorCustomerWithDefaulterDefaulter()
    {
        ForEach(x => x.Addresses).SetDefaulter(new IEnumeratorAddressDefaulter());
        ForEach(x => x.Addresses1).DefaultFor(x => x.Street).Is("Some Street");
    }
}


internal sealed class EnumeratorCustomer
{
    public IEnumerable<Address> Addresses { get; set; } = default!;
    public IEnumerable<Address> Addresses1 { get; set; } = default!;
}