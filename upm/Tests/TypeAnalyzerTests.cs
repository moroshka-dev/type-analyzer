using System.Diagnostics.CodeAnalysis;
using Moroshka.Protect;
using NUnit.Framework;
using Is = NUnit.Framework.Is;

namespace Moroshka.TypeAnalyzer.Tests
{

[TestFixture]
internal sealed class TypeAnalyzerTests
{
	private ITypeAnalyzer _typeAnalyzer;

	[SetUp]
	public void SetUp()
	{
		_typeAnalyzer = new TypeAnalyzer();
		_typeAnalyzer.ClearCache();
	}

	[Test]
	public void Analyze_WithConstructorsFlag_ReturnsOnlyConstructors()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Constructors);

		// Assert
		Assert.That(result.Constructors, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Methods, Is.Null);
		Assert.That(result.Properties, Is.Null);
		Assert.That(result.Fields, Is.Null);
	}

	[Test]
	public void Analyze_WithMethodsFlag_ReturnsOnlyMethods()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Methods);

		// Assert
		Assert.That(result.Constructors, Is.Null);
		Assert.That(result.Methods, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Properties, Is.Null);
		Assert.That(result.Fields, Is.Null);
	}

	[Test]
	public void Analyze_WithPropertiesFlag_ReturnsOnlyProperties()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Properties);

		// Assert
		Assert.That(result.Constructors, Is.Null);
		Assert.That(result.Methods, Is.Null);
		Assert.That(result.Properties, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Fields, Is.Null);
	}

	[Test]
	public void Analyze_WithFieldsFlag_ReturnsOnlyFields()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Fields);

		// Assert
		Assert.That(result.Constructors, Is.Null);
		Assert.That(result.Methods, Is.Null);
		Assert.That(result.Properties, Is.Null);
		Assert.That(result.Fields, Is.Not.Null.And.Not.Empty);
	}

	[Test]
	public void Analyze_WithNoneFlag_ReturnsNoMembers()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.None);

		// Assert
		Assert.That(result.Constructors, Is.Null);
		Assert.That(result.Methods, Is.Null);
		Assert.That(result.Properties, Is.Null);
		Assert.That(result.Fields, Is.Null);
	}

	[Test]
	public void Analyze_WithAllFlags_ReturnsAllMembers()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type);

		// Assert
		Assert.That(result.Constructors, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Methods, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Properties, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Fields, Is.Not.Null.And.Not.Empty);
	}

	[Test]
	public void Analyze_CachesResults()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result1 = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Constructors);
		var result2 = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Constructors);

		// Assert
		Assert.That(result1, Is.SameAs(result2));
	}

	[Test]
	public void Analyze_EmptyType_ReturnsEmptyCollections()
	{
		// Arrange
		var type = typeof(EmptyClass);

		// Act
		var result = _typeAnalyzer.Analyze(type);

		// Assert
		Assert.That(result.Constructors, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Methods, Is.Not.Null.And.Empty);
		Assert.That(result.Properties, Is.Not.Null.And.Empty);
		Assert.That(result.Fields, Is.Not.Null.And.Empty);
	}

	[Test]
	public void Analyze_NullType_ThrowsException()
	{
		// Act & Assert
		Assert.Throws<RequireException>(() => _typeAnalyzer.Analyze(null));
	}

	[Test]
	public void Analyze_ReturnsResultWithCorrectAnalyzedType()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type);

		// Assert
		Assert.That(result.AnalyzedType, Is.EqualTo(type));
	}

	[Test]
	public void Analyze_WithCombinedFlags_ReturnsOnlyRequestedMembers()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Constructors | TypeAnalysisOptions.Methods);

		// Assert
		Assert.That(result.Constructors, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Methods, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Properties, Is.Null);
		Assert.That(result.Fields, Is.Null);
	}

	[Test]
	public void Analyze_WithAnotherCombinedFlags_ReturnsOnlyRequestedMembers()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Properties | TypeAnalysisOptions.Fields);

		// Assert
		Assert.That(result.Constructors, Is.Null);
		Assert.That(result.Methods, Is.Null);
		Assert.That(result.Properties, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Fields, Is.Not.Null.And.Not.Empty);
	}

	[Test]
	public void Analyze_GraduallyFillsCollections_DoesNotOverwriteExisting()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result1 = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Constructors);
		var constructors1 = result1.Constructors;
		var result2 = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Constructors | TypeAnalysisOptions.Methods);

		// Assert
		Assert.That(result1, Is.SameAs(result2));
		Assert.That(result2.Constructors, Is.SameAs(constructors1));
		Assert.That(result2.Methods, Is.Not.Null.And.Not.Empty);
		Assert.That(result2.Properties, Is.Null);
		Assert.That(result2.Fields, Is.Null);
	}

	[Test]
	public void Analyze_ExcludesSpecialNameMethods()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Methods);

		// Assert
		Assert.That(result.Methods, Is.Not.Null);
		foreach (var method in result.Methods)
		{
			Assert.That(method.IsSpecialName, Is.False, $"Method {method.Name} should not be a special-name method");
		}
	}

	[Test]
	public void ClearCache_RemovesAllCachedResults()
	{
		// Arrange
		var type = typeof(SampleClass);
		var result1 = _typeAnalyzer.Analyze(type);

		// Act
		_typeAnalyzer.ClearCache();
		var result2 = _typeAnalyzer.Analyze(type);

		// Assert
		Assert.That(result1, Is.Not.SameAs(result2));
	}

	[Test]
	public void Analyze_WithDifferentOptions_ReturnsSameCachedResult()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result1 = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Constructors);
		var result2 = _typeAnalyzer.Analyze(type, TypeAnalysisOptions.Methods);

		// Assert
		Assert.That(result1, Is.SameAs(result2));
	}

	#region Nested

	[SuppressMessage("ReSharper", "NotAccessedField.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	[SuppressMessage("Performance", "CA1822")]
	private sealed class SampleClass
	{
		public int Field;

		public string Property { get; set; }

		public SampleClass(int field)
		{
			Field = field;
		}

		public void Method()
		{
		}
	}

	private sealed class EmptyClass
	{
	}

	#endregion
}

}
