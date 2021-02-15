using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args) =>
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
                             .Run(args, CreateDefaultConfig());

        public static IConfig CreateDefaultConfig() =>
            DefaultConfig.Instance
                         .AddJob(Job.Default.WithRuntime(CoreRuntime.Core50))
                         .AddDiagnoser(MemoryDiagnoser.Default);
    }
}