﻿using FluentDefaults.Tests.Model;

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

internal sealed class ComplexCustomerDefaulter : AbstractDefaulter<Customer>
{
    internal ComplexCustomerDefaulter()
    {
        DefaultFor(x => x.Address1).SetDefaulter(new ComplexAddressDefaulter());
        DefaultFor(x => x.Address2).SetDefaulter(new ComplexAddressDefaulter());
        DefaultFor(x => x.Address3).Is(() => new Address());
        DefaultFor(x => x.Address3).SetDefaulter(new ComplexAddressDefaulter());
    }
}
