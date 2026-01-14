using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace Moroshka.TypeAnalyzer.Benchmark;

[MemoryDiagnoser]
[RankColumn]
[SuppressMessage("Performance", "CA1822")]
public class Benchmark
{
	[Benchmark]
	public void TypeAnalyzer()
	{
		var typeAnalyzer = new TypeAnalyzer();
		var sampleType = typeof(SampleClass);
		var typeAnalysisResult = typeAnalyzer.Analyze(sampleType);
		_ = typeAnalysisResult.Constructors;
		_ = typeAnalysisResult.Fields;
		_ = typeAnalysisResult.Properties;
		_ = typeAnalysisResult.Methods;
		typeAnalysisResult = typeAnalyzer.Analyze(sampleType);
		_ = typeAnalysisResult.Constructors;
		_ = typeAnalysisResult.Fields;
		_ = typeAnalysisResult.Properties;
		_ = typeAnalysisResult.Methods;
	}

	[Benchmark]
	public void DirectReflection()
	{
		var sampleType = typeof(SampleClass);
		const BindingFlags bindingFlags = BindingFlags.Public |
										  BindingFlags.Instance |
										  BindingFlags.Static |
										  BindingFlags.DeclaredOnly;

		_ = sampleType.GetConstructors(bindingFlags).ToList();
		_ = sampleType.GetFields(bindingFlags).ToList();
		_ = sampleType.GetProperties(bindingFlags).ToList();
		_ = sampleType.GetMethods(bindingFlags).ToList();
		
		_ = sampleType.GetConstructors(bindingFlags).ToList();
		_ = sampleType.GetFields(bindingFlags).ToList();
		_ = sampleType.GetProperties(bindingFlags).ToList();
		_ = sampleType.GetMethods(bindingFlags).ToList();
	}


	#region Nested

	[SuppressMessage("ReSharper", "NotAccessedField.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	[SuppressMessage("Performance", "CA1822")]
	private sealed class SampleClass(int field)
	{
		public int Field = field;

		public string Property { get; set; }

		public void Method()
		{
		}
	}

	#endregion
}
