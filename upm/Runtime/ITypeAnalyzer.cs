using System;

namespace Moroshka.TypeAnalyzer
{

/// <summary>
/// Interface for analyzing types. Provides a method to analyze a type with specified flags.
/// </summary>
public interface ITypeAnalyzer
{
	/// <summary>
	/// Analyzes the specified type based on the provided flags.
	/// </summary>
	/// <param name="type">The type to analyze.</param>
	/// <param name="options">Options indicating which members of the type to analyze.</param>
	/// <returns>A result object containing the analyzed members of the type.</returns>
	TypeAnalysisResult Analyze(Type type, TypeAnalysisOptions options = TypeAnalysisOptions.All);

	/// <summary>
	/// Clears the cache of previously analyzed types and their results.
	/// </summary>
	void ClearCache();
}

}