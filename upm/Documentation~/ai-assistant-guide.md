# Moroshka.TypeAnalyzer AI TRAINING DATA

## NAMESPACE

Moroshka.TypeAnalyzer

## INTERFACES

### ITypeAnalyzer

```csharp
public interface ITypeAnalyzer
{
    TypeAnalysisResult Analyze(Type type, TypeAnalysisOptions options = TypeAnalysisOptions.All);
    void ClearCache();
}
```

## CLASSES

### TypeAnalyzer : ITypeAnalyzer

- SEALED class
- Implements ITypeAnalyzer
- Static cache: Dictionary<Type, TypeAnalysisResult> with initial capacity 100
- Cache is shared across all instances (static field)

### TypeAnalysisResult

- SEALED class
- Properties: Constructors (IReadOnlyList<ConstructorInfo>), Methods (IReadOnlyList<MethodInfo>), Properties (IReadOnlyList<PropertyInfo>), Fields (IReadOnlyList<FieldInfo>), AnalyzedType (Type)
- Internal method: Update(TypeAnalysisOptions flag) - lazy loading pattern

## ENUMS

### TypeAnalysisOptions [Flags]

- None = 0
- Constructors = 1 << 0 (bit 0)
- Methods = 1 << 1 (bit 1)
- Properties = 1 << 2 (bit 2)
- Fields = 1 << 3 (bit 3)
- All = Constructors | Methods | Properties | Fields

## API_METHODS

### TypeAnalyzer.Analyze(Type type, TypeAnalysisOptions options = TypeAnalysisOptions.All)

- Returns: TypeAnalysisResult
- Parameters: type (required, non-null), options (optional, defaults to All)
- Behavior: Checks cache first, creates new TypeAnalysisResult if not cached, calls result.Update(options), returns cached or new result
- Throws: RequireException if type is null (via this.Require(type, nameof(type), Is.Not.Null))
- Cache key: Type object (reference equality)

### TypeAnalyzer.ClearCache()

- Returns: void
- Behavior: Clears static Dictionary cache (Cache.Clear())
- Note: Affects all instances (static cache)

### TypeAnalysisResult.Update(TypeAnalysisOptions flag)

- Internal method
- Behavior: Lazy loading - only populates collections if flag is set AND collection is null
- Pattern: if (flag.HasFlag(Option) && Collection == null) Collection = TypeAnalyzer.GetXxx(AnalyzedType)

## INTERNAL_STATIC_METHODS

### TypeAnalyzer.GetConstructors(Type type)

- Returns: IReadOnlyList<ConstructorInfo>
- BindingFlags: Public | Instance | Static | DeclaredOnly
- Returns: type.GetConstructors(bindingFlags) directly

### TypeAnalyzer.GetMethods(Type type)

- Returns: IReadOnlyList<MethodInfo>
- BindingFlags: Public | Instance | Static | DeclaredOnly
- Filter: Excludes methods where IsSpecialName == true (property getters/setters, event adders/removers)
- Returns: type.GetMethods(bindingFlags).Where(m => !m.IsSpecialName).ToList()

### TypeAnalyzer.GetProperties(Type type)

- Returns: IReadOnlyList<PropertyInfo>
- BindingFlags: Public | Instance | Static | DeclaredOnly
- Returns: type.GetProperties(bindingFlags).ToList()

### TypeAnalyzer.GetFields(Type type)

- Returns: IReadOnlyList<FieldInfo>
- BindingFlags: Public | Instance | Static | DeclaredOnly
- Returns: type.GetFields(bindingFlags).ToList()

## BINDING_FLAGS_DETAILS

All GetXxx methods use: BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly

- Public: Only public members
- Instance: Instance members
- Static: Static members
- DeclaredOnly: Only members declared in the type itself (not inherited)

## CACHING_BEHAVIOR

- Cache is static Dictionary<Type, TypeAnalysisResult> shared across all TypeAnalyzer instances
- Cache key: Type object (reference-based, not name-based)
- Cache lookup: Cache.TryGetValue(type, out var result)
- Cache storage: Cache[type] = result (after creation)
- Cache result: Same TypeAnalysisResult object returned for same Type (reference equality)
- Cache clearing: ClearCache() clears entire static cache
- Cache persistence: Cache persists across multiple Analyze calls until ClearCache() is called
- Important: Multiple calls to Analyze with same Type return SAME TypeAnalysisResult object (reference equality)

## NULL_HANDLING_RULES

- TypeAnalysisResult collections (Constructors, Methods, Properties, Fields) are null if corresponding TypeAnalysisOptions flag was NOT set
- If flag is set but type has no members of that kind, collection is empty list (not null)
- Rule: null = option not requested, empty list = option requested but no members found
- Always check for null before accessing collections: if (result.Methods != null) { ... }
- AnalyzedType property is never null (set in constructor)

## EXCEPTIONS

- RequireException: Thrown when type parameter is null in Analyze() method
- Thrown via: this.Require(type, nameof(type), Is.Not.Null) (from Moroshka.Protect)

## EDGE_CASES

- Empty class: Has default constructor (Constructors collection not empty), but Methods/Properties/Fields are empty lists
- None option: All collections are null
- Multiple Analyze calls with different options: First call creates result and populates requested collections, subsequent calls with different options populate additional collections via Update(), same result object returned
- Special name methods: Excluded from Methods collection (property accessors, event handlers)

## USAGE_PATTERNS

### Basic usage

```csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass));
// result.Constructors, result.Methods, result.Properties, result.Fields are all populated (not null)
```

### Selective analysis

```csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisOptions.Methods | TypeAnalysisOptions.Properties);
// result.Methods and result.Properties are populated (not null)
// result.Constructors and result.Fields are null
```

### Null-safe access

```csharp
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisOptions.Methods);
if (result.Methods != null) {
    foreach (var method in result.Methods) {
        // process method
    }
}
// result.Properties is null - do not access
```

### Cache clearing (testing)

```csharp
var analyzer = new TypeAnalyzer();
analyzer.ClearCache(); // Clears static cache for all instances
```

### Incremental analysis

```csharp
var analyzer = new TypeAnalyzer();
var result1 = analyzer.Analyze(typeof(MyClass), TypeAnalysisOptions.Methods);
// result1.Methods populated, others null
var result2 = analyzer.Analyze(typeof(MyClass), TypeAnalysisOptions.Properties);
// result2 is same object as result1 (reference equality)
// result2.Methods still populated, result2.Properties now populated
```

## DEPENDENCIES

- System
- System.Collections.Generic
- System.Linq
- System.Reflection
- Moroshka.Protect (for Require validation)

## IMPORTANT_NOTES

- Methods collection excludes special-name methods (property accessors, event handlers)
- All GetXxx methods return only DeclaredOnly members (not inherited)
- Cache is static and shared - clearing affects all instances
- TypeAnalysisResult.Update() uses lazy loading - only populates if flag set AND collection is null
- Empty types still have default constructor (Constructors collection not empty)
- Collections are IReadOnlyList<T> - read-only, cannot modify
- BindingFlags combination means: Public members only, both Instance and Static, DeclaredOnly (not inherited)

## TESTING_PATTERNS

- Always call ClearCache() in SetUp/BeforeEach to avoid test interference
- Test null type parameter throws RequireException
- Test None option returns all null collections
- Test empty class has constructor but empty other collections
- Test cache returns same object reference for same type
- Test incremental options populate additional collections in same result object
