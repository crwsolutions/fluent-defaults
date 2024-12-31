using System.Diagnostics;

namespace FluentDefaults.Tests;

public class AsyncDefaulterTests
{
    [Fact]
    public async Task DoWorkAsync_ShouldCompleteAfter500Milliseconds()
    {
        // Arrange
        var customer = new AsyncCustomer();
        var defaulter = new AsyncCustomerDefaulter();
        var stopwatch = Stopwatch.StartNew();

        // Act
        await defaulter.ApplyAsync(customer);

        stopwatch.Stop();

        // Assert
        Assert.Equal(12, customer.Number1);
        Assert.Equal(42, customer.Number2);
        Assert.Equal(24, customer.Number3);
        Assert.InRange(stopwatch.ElapsedMilliseconds, 500, 600); // Allowing some margin for timing
    }
}

public class AsyncCustomerDefaulter : AbstractAsyncDefaulter<AsyncCustomer>
{
    public AsyncCustomerDefaulter()
    {
        DefaultFor(x => x.Number1, 12);
        DefaultFor(x => x.Number2)
            .IsAsync(async () =>
            {
                await Task.Delay(250); // Simulates work
                return 42;
            });
        DefaultFor(x => x.Number3)
            .IsAsync(async () =>
            {
                await Task.Delay(250); // Simulates work
                return 24;
            });
    }
}

public class AsyncCustomer
{
    public int Number1 { get; set; }
    public int Number2 { get; set; }
    public int Number3;
}
