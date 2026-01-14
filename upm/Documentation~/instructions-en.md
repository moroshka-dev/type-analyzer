# Usage Instructions

`TypeAnalyzer` is a tool for analyzing type structure.
It provides the ability to extract information about constructors, methods, properties, and fields of a given type.
This tool is useful when working with reflection, when you need to dynamically explore types at runtime.

## How to Use

`TypeAnalyzer` is used for:

- Type inspection: Getting information about the structure of a class or struct.
- Data filtering: Selectively extracting only those type elements that are needed (e.g., only methods or only properties).
- Result caching: Performance optimization through caching of analysis results.

### Type Analysis

To analyze a type, use the `Analyze` method of the `ITypeAnalyzer` interface. The method takes two parameters:

- `type`: The type to be analyzed.
- `options`: Options that determine which type elements should be extracted (constructors, methods, properties, fields). By default, `TypeAnalysisOptions.All` is used.

``` csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisOptions.Methods | TypeAnalysisOptions.Properties);
```

In this example, only methods and properties of the `MyClass` type will be extracted.

### Using Options

The `TypeAnalysisOptions` allow flexible configuration of analysis:

- `None`: Does not extract any type elements.
- `Constructors`: Extracts type constructors.
- `Methods`: Extracts type methods.
- `Properties`: Extracts type properties.
- `Fields`: Extracts type fields.
- `All`: Extracts all type elements.

If you need only specific elements, combine options using the | operator:

``` csharp
var options = TypeAnalysisOptions.Constructors | TypeAnalysisOptions.Fields;
var result = analyzer.Analyze(typeof(MyClass), options);
```

### Working with Results

The analysis result is returned as a `TypeAnalysisResult` object, which contains collections:

- `Constructors`: List of constructors.
- `Methods`: List of methods.
- `Properties`: List of properties.
- `Fields`: List of fields.

These collections can be used for further processing. For example:

``` csharp
if (result.Methods != null)
{
    foreach (var method in result.Methods)
    {
        Console.WriteLine($"Method: {method.Name}");
    }
}
```

**Important:** Collections can be `null` if the corresponding option was not specified during analysis. Always check collections for `null` before using them.

### Caching

`TypeAnalyzer` automatically caches analysis results for each type.
This avoids re-analyzing the same type, which improves performance.

If you need to clear the cache (e.g., during testing), use the `ClearCache()` method:

``` csharp
analyzer.ClearCache();
```

## Recommendations

- Performance optimization
  - Use a minimal set of options to extract only the data you actually need.
  - If you work with the same type multiple times, rely on caching to avoid unnecessary computations.
- Handling empty types
  - If the analyzed type does not contain elements (e.g., an empty class), the corresponding collections in `TypeAnalysisResult` will be empty (but not `null` if the corresponding option was specified). Make sure your code correctly handles such cases.
- Handling collections
  - Collections in `TypeAnalysisResult` can be `null` if the corresponding option was not specified in the analysis options. Always check collections for `null` before using them.
- Input validation
  - Before calling the `Analyze` method, make sure the passed type is not `null`. Otherwise, a `RequireException` will be thrown.
- Testing
  - Clear the cache before each test to avoid the influence of previous runs.
  - Check both positive scenarios (type with various elements) and negative ones (empty type, `None` option).

## Usage Example

### Full Type Analysis

``` csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisOptions.All);

Console.WriteLine("Constructors:");
foreach (var ctor in result.Constructors)
{
    Console.WriteLine(ctor.Name);
}

Console.WriteLine("Methods:");
foreach (var method in result.Methods)
{
    Console.WriteLine(method.Name);
}
```

### Selective Analysis

``` csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisOptions.Properties);

Console.WriteLine("Properties:");
foreach (var property in result.Properties)
{
    Console.WriteLine(property.Name);
}
```
