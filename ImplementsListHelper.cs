using System.Diagnostics.CodeAnalysis;

namespace Kritjara.Collections.ObjectModel;

internal static class ImplementsListHelper<T>
{
    public static void ThrowWrongValueTypeArgumentException(object value, Exception innerException)
    {
        throw new ArgumentException($"The value '{value.GetType().FullName}' is not of type '{typeof(T).FullName}' and cannot be used in this generic collection.", nameof(value), innerException);
    }

    public static bool IsCompatibleObject([NotNullWhen(true)] object? value)
    {
        // Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
        // Note that default(T) is not equal to null for value types except when T is Nullable<U>.
        return (value is T) || (value == null && default(T) == null);
    }
}