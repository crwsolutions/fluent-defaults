using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace FluentDefaults.Tests.Benchmark;

internal class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<SingletonBenchmark>();

        // Check for any benchmark results that have exceptions
        foreach (var report in summary.Reports)
        {
            if (report.ExecuteResults.Any(result => result.IsSuccess == false))
            {
                Console.WriteLine($"Benchmark {report.BenchmarkCase.Descriptor.WorkloadMethod.Name} threw an exception.");
            }
        }
    }
}
