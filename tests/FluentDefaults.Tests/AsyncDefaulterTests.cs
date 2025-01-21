using FluentDefaults.Tests.Model;
using System.Diagnostics;

namespace FluentDefaults.Tests;

public class AsyncDefaulterTests
{
    [Fact]
    public async Task DoWorkAsync_ShouldCompleteAfter500Milliseconds()
    {
        // Arrange
        var customer = new Customer();
        var defaulter = new AsyncCustomerDefaulter();
        var stopwatch = Stopwatch.StartNew();

        // Act
        await defaulter.ApplyAsync(customer);

        stopwatch.Stop();

        // Assert
        Assert.Equal(12, customer.Number1);
        Assert.Equal(42, customer.NullableNumber2);
        Assert.Equal(24, customer.FieldNumber3);
        Assert.InRange(stopwatch.ElapsedMilliseconds, 500, 600); // Allowing some margin for timing
    }
}

internal sealed class AsyncCustomerDefaulter : AbstractAsyncDefaulter<Customer>
{
    internal AsyncCustomerDefaulter()
    {
        DefaultFor(x => x.Number1).Is(12);
        DefaultFor(x => x.NullableNumber2)
            .IsAsync(async () =>
            {
                await Task.Delay(250); // Simulates work
                return 42;
            });
        DefaultFor(x => x.FieldNumber3)
            .IsAsync(async () =>
            {
                await Task.Delay(250); // Simulates work
                return 24;
            });
    }
}
