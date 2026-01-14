using System;
using System.Collections.Generic;
using System.Reflection;

namespace Moroshka.TypeAnalyzer
{

/// <summary>
/// Represents the result of a type analysis. Contains information about the analyzed members of a type.
/// </summary>
public sealed class TypeAnalysisResult
{
	/// <summary>
	/// Gets the list of constructors of the analyzed type.
	/// </summary>
	public IReadOnlyList<ConstructorInfo> Constructors { get; private set; }

	/// <summary>
	/// Gets the list of methods of the analyzed type.
	/// </summary>
	public IReadOnlyList<MethodInfo> Methods { get; private set; }

	/// <summary>
	/// Gets the list of properties of the analyzed type.
	/// </summary>
	public IReadOnlyList<PropertyInfo> Properties { get; private set; }

	/// <summary>
	/// Gets the list of fields of the analyzed type.
	/// </summary>
	public IReadOnlyList<FieldInfo> Fields { get; private set; }

	/// <summary>
	/// Gets the type that was analyzed.
	/// </summary>
	public Type AnalyzedType { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="TypeAnalysisResult"/> class.
	/// </summary>
	/// <param name="type">The type to analyze.</param>
	public TypeAnalysisResult(Type type)
	{
		AnalyzedType = type;
	}

	/// <summary>
	/// Updates the analysis result based on the specified flags.
	/// </summary>
	/// <param name="flag">Flags indicating which members to update.</param>
	internal void Update(TypeAnalysisOptions flag)
	{
		if (flag.HasFlag(TypeAnalysisOptions.Constructors) && Constructors == null)
			Constructors = TypeAnalyzer.GetConstructors(AnalyzedType);
		if (flag.HasFlag(TypeAnalysisOptions.Methods) && Methods == null)
			Methods = TypeAnalyzer.GetMethods(AnalyzedType);
		if (flag.HasFlag(TypeAnalysisOptions.Properties) && Properties == null)
			Properties = TypeAnalyzer.GetProperties(AnalyzedType);
		if (flag.HasFlag(TypeAnalysisOptions.Fields) && Fields == null)
			Fields = TypeAnalyzer.GetFields(AnalyzedType);
	}
}

}