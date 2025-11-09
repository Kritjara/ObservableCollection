using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc cref="IReadOnlyObservableCollection{T}"/>
/// <summary>
/// Представляет фильтрованную коллекцию только для чтения, которая реализует <see cref="INotifyCollectionChanged"/> и <see cref="INotifyPropertyChanged"/>.
/// Коллекция автоматически обновляется при изменении элементов источника или их свойств, применяя заданный предикат фильтрации.
/// </summary>
public class FilteredObservableCollection<T> : ReadOnlyObservableCollection<T> where T : INotifyPropertyChanged
{
    /// <summary>
    /// Предикат для фильтрации элементов.
    /// </summary>
    private IFilteringStrategy<T> filteringStrategy;

    /// <summary>
    /// Видимая коллекция, содержащая только элементы, прошедшие фильтр.
    /// </summary>
    private readonly ObservableCollection<T> visible;

    /// <summary>
    /// Источник элементов для фильтрации.
    /// </summary>
    private readonly IEnumerable<T> source;

    /// <summary>
    /// Промежуточная коллекция - копия истоника. Позволяет эффективно управлять подписками на событие <see cref="INotifyPropertyChanged"/> элементов.
    /// </summary>
    private readonly ObservableCollection<T> interim;

    private readonly PropertyChangedEventHandler OnItemPropertyChangedEventHandler;
    private readonly NotifyCollectionChangedEventHandler OnSourceCollectionChangedEventHandler;

    #region [ ctors ]

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FilteredObservableCollection{T}"/> с указанным источником и предикатом фильтрации элементов.
    /// </summary>
    /// <param name="source">Источник элементов для фильтрации.</param>
    /// <param name="filter">Предикат для фильтрации элементов.</param>
    public FilteredObservableCollection(ObservableCollection<T> source, Predicate<T> filter) : base(new ObservableCollection<T>(source.Where(item => filter(item))))
    {
        OnItemPropertyChangedEventHandler = OnItemPropertyChanged;
        this.source = source;
        filteringStrategy = new FilteringStrategyDefault<T>(filter);
        interim = [.. source];
        visible = (ObservableCollection<T>)Items; // Items из base - это visible

        OnSourceCollectionChangedEventHandler = Source_CollectionChanged;
        CollectionChangedWeakEventManager.AddHandler(source, OnSourceCollectionChangedEventHandler);
        interim.CollectionChanged += Interim_CollectionChanged;
        foreach (var item in interim)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChangedEventHandler);
        }
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FilteredObservableCollection{T}"/> с указанным источником и предикатом фильтрации элементов.
    /// </summary>
    /// <param name="source">Источник элементов для фильтрации.</param>
    /// <param name="filter">Предикат для фильтрации элементов.</param>
    public FilteredObservableCollection(IObservableCollection<T> source, Predicate<T> filter) : base(new ObservableCollection<T>(source.Where(item => filter(item))))
    {
        OnItemPropertyChangedEventHandler = OnItemPropertyChanged;
        this.source = source;
        filteringStrategy = new FilteringStrategyDefault<T>(filter);
        interim = [.. source];
        visible = (ObservableCollection<T>)Items; // Items из base - это visible

        OnSourceCollectionChangedEventHandler = Source_CollectionChanged;
        CollectionChangedWeakEventManager.AddHandler(source, OnSourceCollectionChangedEventHandler);
        interim.CollectionChanged += Interim_CollectionChanged;
        foreach (var item in interim)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChangedEventHandler);
        }
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FilteredObservableCollection{T}"/> с указанным источником и предикатом фильтрации элементов.
    /// </summary>
    /// <param name="source">Источник элементов для фильтрации.</param>
    /// <param name="filter">Предикат для фильтрации элементов.</param>
    public FilteredObservableCollection(IReadOnlyObservableCollection<T> source, Predicate<T> filter) : base(new ObservableCollection<T>(source.Where(item => filter(item))))
    {
        OnItemPropertyChangedEventHandler = OnItemPropertyChanged;
        this.source = source;
        filteringStrategy = new FilteringStrategyDefault<T>(filter);
        interim = [.. source];
        visible = (ObservableCollection<T>)Items; // Items из base - это visible

        OnSourceCollectionChangedEventHandler = Source_CollectionChanged;
        CollectionChangedWeakEventManager.AddHandler(source, OnSourceCollectionChangedEventHandler);
        interim.CollectionChanged += Interim_CollectionChanged;
        foreach (var item in interim)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChangedEventHandler);
        }
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FilteredObservableCollection{T}"/> с указанным источником и условиями фильтрации элементов.
    /// </summary>
    /// <param name="source">Источник элементов для фильтрации.</param>
    /// <param name="filteringStrategy">Стратегия фильтрации элементов.</param>
    public FilteredObservableCollection(ObservableCollection<T> source, IFilteringStrategy<T> filteringStrategy) : base(new ObservableCollection<T>(source.Where(item => filteringStrategy.Predicate(item))))
    {
        OnItemPropertyChangedEventHandler = OnItemPropertyChanged;
        this.source = source;
        this.filteringStrategy = filteringStrategy;
        filteringStrategy.Changed += FilterContitions_Changed;

        interim = [.. source];
        visible = (ObservableCollection<T>)Items; // Items из base - это visible

        OnSourceCollectionChangedEventHandler = Source_CollectionChanged;
        CollectionChangedWeakEventManager.AddHandler(source, OnSourceCollectionChangedEventHandler);
        interim.CollectionChanged += Interim_CollectionChanged;
        foreach (var item in interim)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChangedEventHandler);
        }
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FilteredObservableCollection{T}"/> с указанным источником и условиями фильтрации элементов.
    /// </summary>
    /// <param name="source">Источник элементов для фильтрации.</param>
    /// <param name="filteringStrategy">Стратегия фильтрации элементов.</param>
    public FilteredObservableCollection(IObservableCollection<T> source, IFilteringStrategy<T> filteringStrategy) : base(new ObservableCollection<T>(source.Where(item => filteringStrategy.Predicate(item))))
    {
        OnItemPropertyChangedEventHandler = OnItemPropertyChanged;
        this.source = source;
        this.filteringStrategy = filteringStrategy;
        filteringStrategy.Changed += FilterContitions_Changed;

        interim = [.. source];
        visible = (ObservableCollection<T>)Items; // Items из base - это visible

        OnSourceCollectionChangedEventHandler = Source_CollectionChanged;
        CollectionChangedWeakEventManager.AddHandler(source, OnSourceCollectionChangedEventHandler);
        interim.CollectionChanged += Interim_CollectionChanged;
        foreach (var item in interim)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChangedEventHandler);
        }
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FilteredObservableCollection{T}"/> с указанным источником и условиями фильтрации элементов.
    /// </summary>
    /// <param name="source">Источник элементов для фильтрации.</param>
    /// <param name="filteringStrategy">Стратегия фильтрации элементов.</param>
    public FilteredObservableCollection(IReadOnlyObservableCollection<T> source, IFilteringStrategy<T> filteringStrategy) : base(new ObservableCollection<T>(source.Where(item => filteringStrategy.Predicate(item))))
    {
        OnItemPropertyChangedEventHandler = OnItemPropertyChanged;
        this.source = source;
        this.filteringStrategy = filteringStrategy;
        filteringStrategy.Changed += FilterContitions_Changed;

        interim = [.. source];
        visible = (ObservableCollection<T>)Items; // Items из base - это visible

        OnSourceCollectionChangedEventHandler = Source_CollectionChanged;
        CollectionChangedWeakEventManager.AddHandler(source, OnSourceCollectionChangedEventHandler);
        interim.CollectionChanged += Interim_CollectionChanged;
        foreach (var item in interim)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChangedEventHandler);
        }
    }

    #endregion

    /// <inheritdoc cref="filteringStrategy"/>
    public IFilteringStrategy<T> FilteringStrategy
    {
        get => filteringStrategy;
        set
        {
            if (ReferenceEquals(filteringStrategy, value)) return;

            filteringStrategy.Changed -= FilterContitions_Changed;

            filteringStrategy = value;
            filteringStrategy.Changed += FilterContitions_Changed;
            Refresh();
            OnPropertyChanged(EventArgsCache.FilteringStrategyPropertyChanged);
        }
    }

    private void FilterContitions_Changed(object? sender, EventArgs e)
    {
        Refresh();
    }

    /// <summary>Вызывает пересчет элементов коллекции - заново проверяет каждый элемент истоника на соответствие условиям фильтра.</summary>
    public void Refresh()
    {
        visible.Reset(source.Where(item => filteringStrategy.Predicate(item)));
    }


    /// <summary>
    /// Обрабатывает изменения в источнике.
    /// </summary>
    private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:

                T addedItem = (T)e.NewItems![0]!;
                PropertyChangedWeakEventManager.AddHandler(addedItem, OnItemPropertyChangedEventHandler);
                interim.Insert(e.NewStartingIndex, addedItem);

                break;

            case NotifyCollectionChangedAction.Remove:

                T removedItem = (T)e.OldItems![0]!;
                PropertyChangedWeakEventManager.RemoveHandler(removedItem, OnItemPropertyChangedEventHandler);
                interim.RemoveAt(e.OldStartingIndex);

                break;

            case NotifyCollectionChangedAction.Replace:

                T oldItem = (T)e.OldItems![0]!;
                PropertyChangedWeakEventManager.RemoveHandler(oldItem, OnItemPropertyChangedEventHandler);
                T newItem = (T)e.NewItems![0]!;
                PropertyChangedWeakEventManager.AddHandler(newItem, OnItemPropertyChangedEventHandler);
                interim[e.NewStartingIndex] = newItem;

                break;

            case NotifyCollectionChangedAction.Move:

                interim.Move(e.OldStartingIndex, e.NewStartingIndex);

                break;

            case NotifyCollectionChangedAction.Reset:

                foreach (var item in interim)
                {
                    PropertyChangedWeakEventManager.RemoveHandler(item, OnItemPropertyChangedEventHandler);
                }

                interim.Reset(source);

                foreach (var item in interim)
                {
                    PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChangedEventHandler);
                }

                break;

            default:
                break;
        }


    }

    /// <summary>
    /// Обрабатывает изменения в источнике.
    /// </summary>
    private void Interim_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        void ItemAdded(T item)
        {
            if (filteringStrategy.Predicate(item))
            {
                int insertIndex = FindInsertPosition(item);
                visible.Insert(insertIndex, item);
            }
        }

        void ItemRemoved(T item)
        {
            if (filteringStrategy.Predicate(item))
            {
                visible.Remove(item);
            }
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:

                ItemAdded((T)e.NewItems![0]!);

                break;

            case NotifyCollectionChangedAction.Remove:

                ItemRemoved((T)e.OldItems![0]!);

                break;

            case NotifyCollectionChangedAction.Replace:

                ItemRemoved((T)e.OldItems![0]!);
                ItemAdded((T)e.NewItems![0]!);

                break;

            case NotifyCollectionChangedAction.Move:
                break;

            case NotifyCollectionChangedAction.Reset:

                visible.Reset(source.Where(item => filteringStrategy.Predicate(item)));

                break;

            default:
                break;
        }
    }


    /// <summary>
    /// Обрабатывает изменения свойств элементов.
    /// </summary>
    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!filteringStrategy.IsFilterAffected(e.PropertyName ?? string.Empty)) return;

        T item = (T)sender!;

        bool currentlyVisible = visible.Contains(item);
        bool shouldBeVisible = filteringStrategy.Predicate(item);

        if (currentlyVisible && !shouldBeVisible)
        {
            // Удаляем из visible
            visible.Remove(item);
        }
        else if (!currentlyVisible && shouldBeVisible)
        {
            // Добавляем в visible в порядке добавления (или можно сортировать, но по умолчанию - порядок источника)
            int insertIndex = FindInsertPosition(item);
            visible.Insert(insertIndex, item);
        }
        // Если элемент уже правильно отфильтрован, ничего не делаем
    }

    /// <summary>
    /// Находит позицию для вставки элемента в видимую коллекцию. Сейчас - всегда в конец списка. Будет улучшено в будущем.
    /// </summary>
    private int FindInsertPosition(T item)
    {
        // Для простоты вставляем в конец, сохраняя порядок добавления
        // в будущем можно улучшить
        return visible.Count;
    }
}