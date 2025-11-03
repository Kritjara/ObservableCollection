using System.Collections.Specialized;
using System.ComponentModel;

#pragma warning disable CS1573 // Параметр не имеет соответствующий тег параметра в комментарии XML (в отличие от остальных параметров)

namespace Kritjara.Collections.ObjectModel;

/// <summary>Расширения для коллекций с уведомлениями <see cref="INotifyCollectionChanged"/></summary>
public static class Extensions
{


    /// <summary>
    /// Возвращает обёртку над <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <returns>Read-only версия коллекции</returns>
    public static IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this System.Collections.ObjectModel.ObservableCollection<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadOnlyObservableCollection<T>(source);
    }



    /// <summary>
    /// Возвращает обёртку над <see cref="IObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <remarks>Этот метод может вернуть ссылку на оригинал, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
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
    /// Возвращает отфильтрованную <see cref="FilteredObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <param name="predicate">Условия для фильтра элементов</param>
    /// <returns>Отфильтрованная read-only версия коллекции</returns>
    public static FilteredObservableCollection<T> AsFiltered<T>(this IObservableCollection<T> source, Predicate<T> predicate) where T : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return new FilteredObservableCollection<T>(source, predicate);
    }

    /// <summary>
    /// Возвращает отсортированную <see cref="SortedReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <param name="comparer">Компаратор для упорядочивания элементов</param>
    /// <returns>Отфильтрованная read-only версия коллекции</returns>
    public static SortedReadOnlyObservableCollection<T> AsSorted<T>(this IObservableCollection<T> source, IComparer<T> comparer) where T : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(comparer);

        return new SortedReadOnlyObservableCollection<T>(source, comparer);
    }



    /// <summary>
    /// Возвращает обёртку над <see cref="System.Collections.ObjectModel.ReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <returns></returns>
    public static IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this System.Collections.ObjectModel.ReadOnlyObservableCollection<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadOnlyObservableCollection<T>(source);
    }



    /// <summary>
    /// Возвращает отфильтрованную <see cref="FilteredObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <param name="predicate">Условия фильтрации элементов</param>
    /// <returns>Отфильтрованная read-only версия коллекции</returns>
    public static FilteredObservableCollection<T> AsFiltered<T>(this IReadOnlyObservableCollection<T> source, Predicate<T> predicate) where T : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return new FilteredObservableCollection<T>(source, predicate);
    }

    /// <summary>
    /// Возвращает отфильтрованную <see cref="FilteredObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <param name="filteringStrategy">Стратегия фильтрации элементов.</param>
    /// <returns>Отфильтрованная read-only версия коллекции</returns>
    public static FilteredObservableCollection<T> AsFiltered<T>(this IReadOnlyObservableCollection<T> source, IFilteringStrategy<T> filteringStrategy) where T : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(filteringStrategy);

        return new FilteredObservableCollection<T>(source, filteringStrategy);
    }

    /// <summary>
    /// Возвращает отсортированную <see cref="SortedReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <param name="comparer">Компаратор для сортировки элементов</param>
    /// <returns>Отфильтрованная read-only версия коллекции</returns>
    public static SortedReadOnlyObservableCollection<T> AsSorted<T>(this IReadOnlyObservableCollection<T> source, IComparer<T> comparer) where T : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(comparer);

        return new SortedReadOnlyObservableCollection<T>(source, comparer);
    }

    /// <summary>
    /// Возвращает отсортированную <see cref="SortedReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <param name="sortingStrategy">Стратегия сортировки элементов</param>
    /// <returns>Отфильтрованная read-only версия коллекции</returns>
    public static SortedReadOnlyObservableCollection<T> AsSorted<T>(this IReadOnlyObservableCollection<T> source, ISortingStrategy<T> sortingStrategy) where T : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sortingStrategy);

        return new SortedReadOnlyObservableCollection<T>(source, sortingStrategy);
    }




    /// <summary>
    /// Возвращает <see cref="IReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <remarks>Этот метод может вернуть ссылку на оригинал, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
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
    /// Возвращает <see cref="IReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
    /// </summary>
    /// <remarks>Этот метод может вернуть ссылку на оригинал, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
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