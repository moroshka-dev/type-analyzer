using BenchmarkDotNet.Running;
using Moroshka.TypeAnalyzer.Benchmark;

BenchmarkRunner.Run<Benchmark>();

/*
| Method           | Mean      | Error    | StdDev    | Rank | Gen0   | Allocated |
|----------------- |----------:|---------:|----------:|-----:|-------:|----------:|
| TypeAnalyzer     |  22.81 ns | 0.292 ns |  0.273 ns |    1 | 0.0014 |      24 B |
| DirectReflection | 432.85 ns | 8.617 ns | 10.898 ns |    2 | 0.0629 |    1056 B |
*/