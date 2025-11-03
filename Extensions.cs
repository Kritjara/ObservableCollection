using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;

/// <summary>Расширения для коллекций с уведомлениями <see cref="INotifyCollectionChanged"/></summary>
public static class Extensions
{

    extension<T>(System.Collections.ObjectModel.ObservableCollection<T> source)
    {
        /// <summary>
        /// Возвращает обёртку над <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
        /// </summary>
        /// <returns>Read-only версия коллекции</returns>
        public IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection()
        {
            ArgumentNullException.ThrowIfNull(source);

            return new ReadOnlyObservableCollection<T>(source);
        }
    }

    extension<T>(IObservableCollection<T> source)
    {
        /// <summary>
        /// Возвращает обёртку над <see cref="IObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
        /// </summary>
        /// <remarks>Этот метод может вернуть ссылку на оригинал, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
        /// <returns>Read-only версия коллекции</returns>
        public IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection()
        {
            ArgumentNullException.ThrowIfNull(source);

            if (source is IReadOnlyObservableCollection<T> asReadOnly)
            {
                return asReadOnly;
            }
            return new ReadOnlyObservableCollection<T>(source);
        }
    }

    extension<T>(IObservableCollection<T> source) where T : INotifyPropertyChanged
    {
        /// <summary>
        /// Возвращает отфильтрованную <see cref="FilteredObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
        /// </summary>
        /// <param name="predicate">Условия для фильтра элементов</param>
        /// <returns>Отфильтрованная read-only версия коллекции</returns>
        public FilteredObservableCollection<T> AsFiltered(Predicate<T> predicate)
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
        public SortedReadOnlyObservableCollection<T> AsSorted(IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(comparer);

            return new SortedReadOnlyObservableCollection<T>(source, comparer);
        }
    }

    extension<T>(System.Collections.ObjectModel.ReadOnlyObservableCollection<T> source)
    {
        /// <summary>
        /// Возвращает обёртку над <see cref="System.Collections.ObjectModel.ReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection()
        {
            ArgumentNullException.ThrowIfNull(source);

            return new ReadOnlyObservableCollection<T>(source);
        }
    }

    extension<T>(IReadOnlyObservableCollection<T> source) where T : INotifyPropertyChanged
    {
        /// <summary>
        /// Возвращает отфильтрованную <see cref="FilteredObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
        /// </summary>
        /// <param name="predicate">Условия фильтрации элементов</param>
        /// <returns>Отфильтрованная read-only версия коллекции</returns>
        public FilteredObservableCollection<T> AsFiltered(Predicate<T> predicate)
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
        public FilteredObservableCollection<T> AsFiltered(IFilteringStrategy<T> filteringStrategy)
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
        public SortedReadOnlyObservableCollection<T> AsSorted(IComparer<T> comparer)
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
        public SortedReadOnlyObservableCollection<T> AsSorted(ISortingStrategy<T> sortingStrategy)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(sortingStrategy);

            return new SortedReadOnlyObservableCollection<T>(source, sortingStrategy);
        }
    }


    extension(INotifyCollectionChanged source)
    {
        /// <summary>
        /// Возвращает <see cref="IReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <remarks>Этот метод может вернуть ссылку на оригинал, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
        /// <returns>Read-only версия коллекции</returns>
        public IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>()
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
    }

    extension<T>(IEnumerable<T> source)
    {
        /// <summary>
        /// Возвращает <see cref="IReadOnlyObservableCollection{T}"/> с привязкой к источнику и сохранением уведомлений 
        /// </summary>
        /// <remarks>Этот метод может вернуть ссылку на оригинал, если тот уже является <see cref="IReadOnlyObservableCollection{T}"/></remarks>
        /// <returns>Read-only версия коллекции</returns>
        public IReadOnlyObservableCollection<T> AsReadOnlyObservableCollection()
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

}