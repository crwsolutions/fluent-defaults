using BenchmarkDotNet.Attributes;

namespace FluentDefaults.Tests.Benchmark;

//| Method    | Mean       | Error    | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
//|---------- |-----------:|---------:|---------:|------:|-------:|----------:|------------:|
//| Transient | 1,822.7 ns | 21.89 ns | 20.48 ns |  1.00 | 0.2975 |    2496 B |        1.00 |
//| Singleton |   468.0 ns |  1.91 ns |  1.69 ns |  0.26 | 0.0429 |     360 B |        0.14 |

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
