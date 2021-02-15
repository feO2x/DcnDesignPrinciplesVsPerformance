``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET Core SDK=5.0.103
  [Host]     : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  Job-QWRXDD : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Runtime=.NET Core 5.0  

```
|                 Method |     Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------- |---------:|----------:|----------:|-------:|------:|------:|----------:|
| ThrowAndCatchException | 3.540 μs | 0.0171 μs | 0.0160 μs | 0.0114 |     - |     - |     200 B |
