using BenchmarkDotNet.Attributes;

namespace FluentDefaults.Tests.Benchmark;

//| Method    | Mean       | Error    | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
//|---------- |-----------:|---------:|---------:|------:|-------:|----------:|------------:|
//| Transient | 2,390.0 ns | 37.93 ns | 59.05 ns |  1.00 | 0.7973 |    2512 B |        1.00 |
//| Singleton |   530.2 ns |  4.87 ns |  4.32 ns |  0.22 | 0.0839 |     264 B |        0.11 |

[MemoryDiagnoser(true)]
public class SingletonBenchmark
{
    [Benchmark(Baseline = true)]
    public bool Transient()
    {
        var customer = new Customer();
        var defaulter = new CustomerDefaulter();
        defaulter.Apply(customer);

        if (
            customer.Number1 != 1 ||
            customer.Number2 != 2 ||
            customer.Number3 != 3
            )
        {
            throw new Exception("Not correct");
        }

        return true;
    }

    private static readonly CustomerDefaulter _defaulter = new();

    [Benchmark]
    public bool Singleton()
    {
        var customer = new Customer();
        _defaulter.Apply(customer);

        if (
            customer.Number1 != 1 ||
            customer.Number2 != 2 ||
            customer.Number3 != 3
            )
        {
            throw new Exception("Not correct");
        }

        return true;
    }
}

public sealed class Customer
{
    public int Number1 { get; set; }
    public int? Number2 { get; set; }

    public int Number3;
}

public sealed class CustomerDefaulter : AbstractDefaulter<Customer>
{
    public CustomerDefaulter()
    {
        DefaultFor(x => x.Number1).Is(1); // Default Number1 to 1
        DefaultFor(x => x.Number2).Is(2); // Default Number2 to null if not set
        DefaultFor(x => x.Number3).Is(3);
    }
}
