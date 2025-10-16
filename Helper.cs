using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace Kritjara.Collections.ObjectModel;

internal static class Helper
{
    public static string ErrorIsNotIListAndIsNotINotifyCollectionChanged<T>(Type type)
        => $"Невозможно создать read-only коллекцию: {type.FullName} не реализует требуемые интерфейсы: {typeof(IList<T>)} и {typeof(INotifyCollectionChanged)}.";

    internal static void ThrowIfValueSizeLessZero(int value, [CallerArgumentExpression(nameof(value))] string? argName = null)
    {
        if (value <= 0)
            throw new ArgumentException("Максимальный размер коллекции должен быть больше нуля", argName);
    }
}