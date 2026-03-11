using System.Reflection;
using api.Models;

namespace api.Tests.Models;

public class ModelsReflectionTests
{
    private static readonly Assembly ModelsAssembly = typeof(Country).Assembly;
    private const string ModelsNamespace = "api.Models";

    public static IEnumerable<Type> GetModelTypes()
    {
        return ModelsAssembly
            .GetTypes()
            .Where(t => t.IsClass && t.IsPublic && t.Namespace == ModelsNamespace);
    }

    [Fact]
    public void All_Model_Types_Are_Discoverable()
    {
        var types = GetModelTypes().ToList();
        Assert.NotEmpty(types);
    }

    [Theory]
    [MemberData(nameof(ModelTypes))]
    public void Model_Can_Be_Constructed(Type modelType)
    {
        var instance = CreateInstanceOf(modelType);
        Assert.NotNull(instance);
    }

    public static IEnumerable<object[]> ModelTypes()
    {
        foreach (var t in GetModelTypes())
        {
            yield return new object[] { t };
        }
    }

    [Theory]
    [MemberData(nameof(ModelTypes))]
    public void Can_Set_And_Get_All_Writable_Properties(Type modelType)
    {
        var instance = CreateInstanceOf(modelType) ?? throw new InvalidOperationException($"Cannot create {modelType.Name}");

        foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanWrite) continue; // skip read-only

            var valueToSet = CreateSampleValue(prop.PropertyType);

            // Some navigation properties may be proxied/lazy-loaded; we only set when compatible
            if (valueToSet != null && prop.PropertyType.IsInstanceOfType(valueToSet) || valueToSet == null)
            {
                prop.SetValue(instance, valueToSet);

                var readValue = prop.GetValue(instance);

                if (prop.PropertyType.IsValueType || IsNullableValueType(prop.PropertyType))
                {
                    Assert.Equal(valueToSet, readValue);
                }
                else
                {
                    // For reference types we set a non-null when possible, otherwise allow null
                    if (valueToSet != null)
                    {
                        Assert.Same(valueToSet, readValue);
                    }
                }
            }
        }
    }

    private static bool IsNullableValueType(Type t)
    {
        return Nullable.GetUnderlyingType(t) != null;
    }

    private static object? CreateInstanceOf(Type t)
    {
        // Try parameterless first
        var defaultCtor = t.GetConstructor(Type.EmptyTypes);
        if (defaultCtor != null)
        {
            return Activator.CreateInstance(t);
        }

        // Try the simplest public constructor with generated args
        var ctors = t.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(c => c.GetParameters().Length)
            .ToList();
        foreach (var ctor in ctors)
        {
            var ps = ctor.GetParameters();
            var args = new object?[ps.Length];
            var ok = true;
            for (int i = 0; i < ps.Length; i++)
            {
                args[i] = CreateSampleValue(ps[i].ParameterType);
                // If cannot create a required value type arg, skip this ctor
                if (args[i] == null && (ps[i].ParameterType.IsValueType && Nullable.GetUnderlyingType(ps[i].ParameterType) == null))
                {
                    ok = false; break;
                }
            }
            if (!ok) continue;
            try
            {
                return ctor.Invoke(args);
            }
            catch
            {
                // try next
            }
        }

        return null;
    }

    private static object? CreateSampleValue(Type t)
    {
        // Handle nullable types
        var underlying = Nullable.GetUnderlyingType(t);
        if (underlying != null)
        {
            return CreateSampleValue(underlying);
        }

        if (t == typeof(int)) return 123;
        if (t == typeof(float)) return 123.45f;
        if (t == typeof(double)) return 123.45d;
        if (t == typeof(decimal)) return 123.45m;
        if (t == typeof(bool)) return true;
        if (t == typeof(string)) return "sample";
        if (t == typeof(DateTime)) return new DateTime(2000, 1, 1);

        // Arrays
        if (t.IsArray)
        {
            var elemType = t.GetElementType()!;
            if (elemType == typeof(string)) return new[] { "A", "B" };
            if (elemType == typeof(int)) return new[] { 1, 2 };
            // Create empty array for other types
            return Array.CreateInstance(elemType, 0);
        }

        // ICollection<T> or IEnumerable<T>
        if (t.IsGenericType)
        {
            var genDef = t.GetGenericTypeDefinition();
            if (genDef == typeof(ICollection<>) || genDef == typeof(IEnumerable<>) || genDef == typeof(IList<>))
            {
                var elemType = t.GetGenericArguments()[0];
                var listType = typeof(List<>).MakeGenericType(elemType);
                return Activator.CreateInstance(listType);
            }
        }

        // For complex reference types from models, try to create a new instance
        if (!t.IsValueType)
        {
            try
            {
                return Activator.CreateInstance(t);
            }
            catch
            {
                return null; // If no parameterless ctor, leave null
            }
        }

        // Fallback default for structs
        return Activator.CreateInstance(t);
    }
}