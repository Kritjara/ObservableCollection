using System.Collections.Specialized;

namespace Kritjara.Collections.ObjectModel;

/// <summary>Расширения для коллекций с уведомлениями <see cref="INotifyCollectionChanged"/></summary>
public static class Extensions
{
    /// <summary>
    /// Возвращает обёртку над <see cref="IObservableCollection{T}"/> с сохранением уведомлений 
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Оригинальная коллекция с уведомлениями</param>
    /// <remarks>Этот метод может вернуть ссылку на <paramref name="source"/>, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
    /// <returns>Read-only версия коллекции</returns>
    public static IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this IObservableCollection<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (source is IReadOnlyObservableCollection<T> asReadOnly)
        {
            return asReadOnly;
        }
        return new ReadOnlyObservableCollection<T>(source);
    }

    /// <summary>
    /// Возвращает обёртку над <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> с сохранением уведомлений 
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Оригинальная коллекция с уведомлениями</param>
    /// <returns>Read-only версия коллекции</returns>
    public static IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this System.Collections.ObjectModel.ObservableCollection<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadOnlyObservableCollection<T>(source);
    }

    /// <summary>
    /// Возвращает обёртку над <see cref="System.Collections.ObjectModel.ReadOnlyObservableCollection{T}"/> с сохранением уведомлений 
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Оригинальная коллекция с уведомлениями</param>
    /// <returns></returns>
    public static IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this System.Collections.ObjectModel.ReadOnlyObservableCollection<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadOnlyObservableCollection<T>(source);
    }


    /// <summary>
    /// Возвращает <see cref="IReadOnlyObservableCollection{T}"/> с сохранением уведомлений 
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Оригинальная коллекция с уведомлениями</param>
    /// <remarks>Этот метод может вернуть ссылку на <paramref name="source"/>, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
    /// <returns>Read-only версия коллекции</returns>
    public static IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this INotifyCollectionChanged source)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (source is IReadOnlyObservableCollection<T> asReadOnly)
        {            
            return asReadOnly;
        }

        if (ReadOnlyObservableCollection<T>.TryCreate(source, out var result))
        {
            return result;
        }

        throw new InvalidOperationException(Helper.ErrorIsNotIListAndIsNotINotifyCollectionChanged<T>(source.GetType()));
    }

    /// <summary>
    /// Возвращает <see cref="IReadOnlyObservableCollection{T}"/> с сохранением уведомлений 
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Оригинальная коллекция с уведомлениями</param>
    /// <remarks>Этот метод может вернуть ссылку на <paramref name="source"/>, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
    /// <returns>Read-only версия коллекции</returns>
    public static IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (source is IReadOnlyObservableCollection<T> asReadOnly)
        {
            return asReadOnly;
        }

        if (source is IList<T> list)
        {
            if (ReadOnlyObservableCollection<T>.TryCreate(list, out var result))
            {
                return result;
            }
        }

        throw new InvalidOperationException(Helper.ErrorIsNotIListAndIsNotINotifyCollectionChanged<T>(source.GetType()));
    }

}

internal static class Helper
{
    public static string ErrorIsNotIListAndIsNotINotifyCollectionChanged<T>(Type type)
        => $"Невозможно создать read-only коллекцию: {type.FullName} не реализует требуемые интерфейсы: {typeof(IList<T>)} и {typeof(INotifyCollectionChanged)}.";

}