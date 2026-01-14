using UnityEngine;

namespace Moroshka.TypeAnalyzer.Samples
{
	/// <summary>
	/// Example of using TypeAnalyzer for type analysis.
	/// Demonstrates gradual retrieval of type information: first partial, then full information.
	/// </summary>
	public sealed class TypeAnalyzerSample : MonoBehaviour
	{
		private ITypeAnalyzer _typeAnalyzer;

		private void Start()
		{
			_typeAnalyzer = new TypeAnalyzer();
			var type = typeof(ExampleClass);

			// First, we get only partial information - only methods
			Debug.Log("=== Partial analysis: methods only ===");
			var partialResult = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Methods);
			Debug.Log($"Type: {partialResult.AnalyzedType.Name}");
			Debug.Log($"Methods: {partialResult.Methods?.Count ?? 0}");
			Debug.Log($"Constructors: {(partialResult.Constructors == null ? "not analyzed" : "analyzed")}");
			Debug.Log($"Properties: {(partialResult.Properties == null ? "not analyzed" : "analyzed")}");
			Debug.Log($"Fields: {(partialResult.Fields == null ? "not analyzed" : "analyzed")}");

			if (partialResult.Methods != null)
			{
				foreach (var method in partialResult.Methods)
				{
					Debug.Log($"  - Method: {method.Name}");
				}
			}

			// Now we get full type information
			Debug.Log("\n=== Full analysis: all type members ===");
			var fullResult = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.All);
			Debug.Log($"Type: {fullResult.AnalyzedType.Name}");
			Debug.Log($"Constructors: {fullResult.Constructors?.Count ?? 0}");
			Debug.Log($"Methods: {fullResult.Methods?.Count ?? 0}");
			Debug.Log($"Properties: {fullResult.Properties?.Count ?? 0}");
			Debug.Log($"Fields: {fullResult.Fields?.Count ?? 0}");

			if (fullResult.Constructors != null)
			{
				foreach (var constructor in fullResult.Constructors)
				{
					Debug.Log($"  - Constructor: {constructor}");
				}
			}

			if (fullResult.Properties != null)
			{
				foreach (var property in fullResult.Properties)
				{
					Debug.Log($"  - Property: {property.Name} ({property.PropertyType.Name})");
				}
			}

			if (fullResult.Fields != null)
			{
				foreach (var field in fullResult.Fields)
				{
					Debug.Log($"  - Field: {field.Name} ({field.FieldType.Name})");
				}
			}
		}

		/// <summary>
		/// Example class for analysis.
		/// </summary>
		private sealed class ExampleClass
		{
			public int Value;

			public string Name { get; set; }

			public ExampleClass()
			{
			}

			public void DoSomething()
			{
			}
		}
	}
}
