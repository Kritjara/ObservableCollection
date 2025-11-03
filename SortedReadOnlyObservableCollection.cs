using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;

/// <summary>Представляет отсортированную и связанную с источником коллецию только для чтения, которая реализует <see cref="INotifyCollectionChanged"/>.</summary>
/// <typeparam name="T">Тип элементов, содержащихся в коллекции.</typeparam>
public class SortedReadOnlyObservableCollection<T> : ReadOnlyObservableCollection<T> where T : INotifyPropertyChanged
{
    #region [ ctors ]

    /// <summary>Создаёт новый экземпляр отсортированной коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    ///<param name="comparer">Компаратор для сортировки элементов</param>
    public SortedReadOnlyObservableCollection(IObservableCollection<T> source, IComparer<T> comparer) : base(new SortedObservableCollection<T>(source, comparer))
    {
        this.source = source;
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
    }

    /// <summary>Создаёт новый экземпляр отсортированной коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    ///<param name="sortingStrategy">Стратегия сортировки элементов</param>
    public SortedReadOnlyObservableCollection(IObservableCollection<T> source, ISortingStrategy<T> sortingStrategy) : base(new SortedObservableCollection<T>(source, sortingStrategy))
    {
        this.source = source;
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
    }

    /// <summary>Создаёт новый экземпляр отсортированной коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    ///<param name="comparer">Компаратор для сортировки элементов</param>
    public SortedReadOnlyObservableCollection(IReadOnlyObservableCollection<T> source, IComparer<T> comparer) : base(new SortedObservableCollection<T>(source, comparer))
    {
        this.source = source;
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
    }

    /// <summary>Создаёт новый экземпляр отсортированной коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    ///<param name="sortingStrategy">Стратегия сортировки элементов</param>
    public SortedReadOnlyObservableCollection(IReadOnlyObservableCollection<T> source, ISortingStrategy<T> sortingStrategy) : base(new SortedObservableCollection<T>(source, sortingStrategy))
    {
        this.source = source;
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
    }

    /// <summary>Создаёт новый экземпляр отсортированной коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    ///<param name="comparer">Компаратор для сортировки элементов</param>
    public SortedReadOnlyObservableCollection(System.Collections.ObjectModel.ObservableCollection<T> source, IComparer<T> comparer) : base(new SortedObservableCollection<T>(source, comparer))
    {
        this.source = source;
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
    }

    /// <summary>Создаёт новый экземпляр отсортированной коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    ///<param name="sortingStrategy">Стратегия сортировки элементов</param>
    public SortedReadOnlyObservableCollection(System.Collections.ObjectModel.ObservableCollection<T> source, ISortingStrategy<T> sortingStrategy) : base(new SortedObservableCollection<T>(source, sortingStrategy))
    {
        this.source = source;
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
    }

    /// <summary>Создаёт новый экземпляр отсортированной коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    ///<param name="comparer">Компаратор для сортировки элементов</param>
    public SortedReadOnlyObservableCollection(System.Collections.ObjectModel.ReadOnlyObservableCollection<T> source, IComparer<T> comparer) : base(new SortedObservableCollection<T>(source, comparer))
    {
        this.source = source;
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
    }

    /// <summary>Создаёт новый экземпляр отсортированной коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    ///<param name="sortingStrategy">Стратегия сортировки элементов</param>
    public SortedReadOnlyObservableCollection(System.Collections.ObjectModel.ReadOnlyObservableCollection<T> source, ISortingStrategy<T> sortingStrategy) : base(new SortedObservableCollection<T>(source, sortingStrategy))
    {
        this.source = source;
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
    }

    #endregion

    private readonly IEnumerable<T> source;

    private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                               
                    foreach (T item in e.NewItems!)
                    {
                        Items.Add(item);
                    }
                
                break;

            case NotifyCollectionChangedAction.Remove:

                    foreach (T item in e.OldItems!)
                    {
                        Items.Remove(item);
                    }                

                break;
            case NotifyCollectionChangedAction.Replace:

                Items.Remove((T)e.OldItems![0]!);
                Items.Add((T)e.NewItems![0]!);

                break;
            case NotifyCollectionChangedAction.Move:

                break;

            case NotifyCollectionChangedAction.Reset:

                ((SortedObservableCollection<T>)Items).Reset(source);

                break;
            default:
                break;
        }
      
    }


    /// <inheritdoc/>
    protected override void OnSourceCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnSourceCollectionChanged(e);
    }
}
