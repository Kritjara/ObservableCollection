using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc/>
/// <summary>
/// <summary>Представляет отсортированную коллецию, которая реализует <see cref="INotifyCollectionChanged"/>, которая отслеживает изменения элементов по событию <see cref="INotifyPropertyChanged"/> и перемещает их в соответствующую позицию.</summary>
/// </summary>
public class SortedObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
{
    /// <summary>
    /// Компаратор элементов
    /// </summary>
    private ISortingStrategy<T> sortingStrategy;

    /// <inheritdoc cref="sortingStrategy"/>
    public ISortingStrategy<T> SortingStrategy
    {
        get => sortingStrategy;
        set
        {
            if (ReferenceEquals(sortingStrategy, value)) return;

            sortingStrategy.Changed -= SortingStrategy_Changed;

            sortingStrategy = value;
            sortingStrategy.Changed += SortingStrategy_Changed;
            Refresh();
            OnPropertyChanged(EventArgsCache.SortingStrategyPropertyChanged);
        }
    }

    private void SortingStrategy_Changed(object? sender, EventArgs e)
    {
        Refresh();
    }


    #region [ ctors ]

    ///<summary>Инициализирует новый экземпляр класса <see cref="SortedObservableCollection{T}"/>.</summary>
    ///<param name="comparer">Компаратор для сортировки элементов</param>
    public SortedObservableCollection(IComparer<T> comparer) : base()
    {
        sortingStrategy = new SortingStrategyDefault<T>(comparer);
        sortingStrategy.Changed += SortingStrategy_Changed;
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="SortedObservableCollection{T}"/>.</summary>
    ///<param name="sortingStrategy">Стратегия сортировки элементов</param>
    public SortedObservableCollection(ISortingStrategy<T> sortingStrategy) : base()
    {
        this.sortingStrategy = sortingStrategy;
        sortingStrategy.Changed += SortingStrategy_Changed;
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="SortedObservableCollection{T}"/> с указанной начальной ёмкостью.</summary>
    ///<param name="capacity">Начальная ёмкость коллекции.</param>
    ///<param name="comparer">Компаратор для сортировки элементов</param>
    public SortedObservableCollection(int capacity, IComparer<T> comparer) : base(capacity)
    {
        sortingStrategy = new SortingStrategyDefault<T>(comparer);
        sortingStrategy.Changed += SortingStrategy_Changed;
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="SortedObservableCollection{T}"/> с указанной начальной ёмкостью.</summary>
    ///<param name="capacity">Начальная ёмкость коллекции.</param>
    ///<param name="sortingStrategy">Стратегия сортировки элементов</param>
    public SortedObservableCollection(int capacity, ISortingStrategy<T> sortingStrategy) : base(capacity)
    {
        this.sortingStrategy = sortingStrategy;
        sortingStrategy.Changed += SortingStrategy_Changed;
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="SortedObservableCollection{T}"/>, содержащий скопированные элементы из указанной коллекции и с указанной начальной ёмкостью.</summary>
    ///<param name="items">Элементы для копирования</param>
    ///<param name="capacity">Начальная ёмкость коллекции.</param>
    ///<param name="comparer">Компаратор для сортировки элементов</param>
    ///<remarks>Указание <paramref name="capacity"/> при создании может оказаться лишним, если <paramref name="items"/> содержит больше элементов.</remarks>
    public SortedObservableCollection(IEnumerable<T> items, int capacity, IComparer<T> comparer) : base(items.Order(comparer), capacity)
    {
        sortingStrategy = new SortingStrategyDefault<T>(comparer);
        sortingStrategy.Changed += SortingStrategy_Changed;
        foreach (var item in Items)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChanged);
        }
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="SortedObservableCollection{T}"/>, содержащий скопированные элементы из указанной коллекции и с указанной начальной ёмкостью.</summary>
    ///<param name="items">Элементы для копирования</param>
    ///<param name="capacity">Начальная ёмкость коллекции.</param>
    ///<param name="sortingStrategy">Стратегия сортировки элементов</param>
    ///<remarks>Указание <paramref name="capacity"/> при создании может оказаться лишним, если <paramref name="items"/> содержит больше элементов.</remarks>
    public SortedObservableCollection(IEnumerable<T> items, int capacity, ISortingStrategy<T> sortingStrategy) : base(items.Order(sortingStrategy.Comparer), capacity)
    {
        this.sortingStrategy = sortingStrategy;
        sortingStrategy.Changed += SortingStrategy_Changed;
        foreach (var item in Items)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChanged);
        }
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="SortedObservableCollection{T}"/>, содержащий скопированные элементы из указанной коллекции.</summary>
    ///<param name="items">Элементы для копирования</param>
    ///<param name="comparer">Компаратор для сортировки элементов</param>
    public SortedObservableCollection(IEnumerable<T> items, IComparer<T> comparer) : base(items.Order(comparer))
    {
        sortingStrategy = new SortingStrategyDefault<T>(comparer);
        sortingStrategy.Changed += SortingStrategy_Changed;
        foreach (var item in Items)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChanged);
        }
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="SortedObservableCollection{T}"/>, содержащий скопированные элементы из указанной коллекции.</summary>
    ///<param name="items">Элементы для копирования</param>
    ///<param name="sortingStrategy">Стратегия сортировки элементов</param>
    public SortedObservableCollection(IEnumerable<T> items, ISortingStrategy<T> sortingStrategy) : base(items.Order(sortingStrategy.Comparer))
    {
        this.sortingStrategy = sortingStrategy;
        sortingStrategy.Changed += SortingStrategy_Changed;
        foreach (var item in Items)
        {
            PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChanged);
        }
    }

    #endregion

    /// <summary>Добавялет элемент в отсортированный список.</summary>
    /// <param name="item">Элемент для добавления.</param>
    public override void Add(T item)
    {
        OnAdd(FindInsertPosition(item), item);
    }

    /// <summary>Добавялет диапазон элементов в отсортированный список.</summary>
    /// <param name="collection">Диапазон элементов для добавления.</param>
    public override void AddRange(IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            Add(item);
        }
    }

    /// <summary>Унаследованный метод, который не поддерживается в реализации этого класса, т.к. индекс расположения элемента определяется автоматически. Используйте метод <see cref="Add"/>.</summary>
    public override void Insert(int index, T item)
    {
        ThrowCanNotBeInserted();
    }

    /// <summary>Унаследованный метод, который не поддерживается в реализации этого класса, т.к. индекс расположения элемента определяется автоматически.  Используйте метод <see cref="AddRange"/>.</summary>
    public override void InsertRange(int index, IEnumerable<T> collection)
    {
        ThrowCanNotBeInserted();
    }

    static void ThrowCanNotBeInserted()
    {
        throw new InvalidOperationException("Нельзя указать индекс для вставки элементов в отсортированном списке. Используйте метод Add или AddRange.");
    }


    /// <summary>Унаследованный метод, который не поддерживается в реализации этого класса, т.к. индексы элементов определяются автоматически.</summary>
    public override void Move(int oldIndex, int newIndex)
    {
        throw new InvalidOperationException("Нельзя перемещать элементы в отсортированном списке.");
    }

    /// <inheritdoc/>
    protected override void OnAdd(int index, T item)
    {
        PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChanged);
        base.OnAdd(index, item);
    }

    /// <inheritdoc/>
    protected override void OnRemoveAt(int index)
    {
        T removedItem = this[index];
        PropertyChangedWeakEventManager.RemoveHandler(removedItem, OnItemPropertyChanged);
        base.OnRemoveAt(index);
    }

    /// <inheritdoc/>
    protected override bool OnRemove(T item)
    {
        PropertyChangedWeakEventManager.RemoveHandler(item, OnItemPropertyChanged);
        return base.OnRemove(item);
    }

    /// <inheritdoc/>
    protected override void OnReplace(int index, T item)
    {
        throw new InvalidOperationException("Нельзя заменять элементы по индексу в отсортированном списке. Удалите старый элемент и добавьте новый.");
    }

    /// <inheritdoc/>
    protected override void OnClearItems()
    {
        foreach (var item in Items)
        {
            PropertyChangedWeakEventManager.RemoveHandler(item, OnItemPropertyChanged);
        }
        base.OnClearItems();
    }

    /// <inheritdoc/>
    protected override void OnReset(IEnumerable<T>? items)
    {
        foreach (var item in Items)
        {
            PropertyChangedWeakEventManager.RemoveHandler(item, OnItemPropertyChanged);
        }

        if (items is null)
        {
            base.OnClearItems();
        }
        else
        {
            IEnumerable<T> sortedItems = items.Order(sortingStrategy.Comparer);
            base.Reset(sortedItems);
            foreach (var item in sortedItems)
            {
                PropertyChangedWeakEventManager.AddHandler(item, OnItemPropertyChanged);
            }
        }
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        T item = (T)sender!;

        int currentIndex = Items.IndexOf(item);
        if (currentIndex == -1) return;

        int leftNeighbor = currentIndex > 0 ? currentIndex - 1 : -1;
        int rightNeighbor = currentIndex < Items.Count - 1 ? currentIndex + 1 : -1;

        int start;
        int end;

        // Определяем, нужно ли перемещать и в какую часть
        if (leftNeighbor != -1 && sortingStrategy.Comparer.Compare(item, Items[leftNeighbor]) < 0)
        {
            // Элемент меньше левого соседа — перемещаем в левую часть
            start = 0;
            end = leftNeighbor;
            int newIndex = FindInsertPosition(item, start, end);
            OnMoveItem(currentIndex, newIndex);
        }
        else if (rightNeighbor != -1 && sortingStrategy.Comparer.Compare(item, Items[rightNeighbor]) > 0)
        {
            // Элемент больше правого соседа — перемещаем в правую часть
            start = rightNeighbor;
            end = Items.Count - 1;

            // -1 потому что элемент находится левее нового индекса и перед добавлением в новую позицию он сначала будет удален
            int newIndex = FindInsertPosition(item, start, end) - 1;

            OnMoveItem(currentIndex, newIndex);
        }
        else
        {
            // Иначе — элемент уже на месте (между соседями), ничего не делаем           
        }
    }

    private int FindInsertPosition(T item)
    {
        return FindInsertPosition(item, 0, Items.Count - 1);
    }

    private int FindInsertPosition(T item, int start, int end)
    {
        while (start <= end)
        {
            int mid = start + (end - start) / 2;
            int comparison = sortingStrategy.Comparer.Compare(item, Items[mid]);

            if (comparison == 0)
            {
                // Элемент равен — вставляем после (для стабильности дубликатов)
                return mid + 1;
            }
            else if (comparison < 0)
            {
                // Ищем в левой половине
                end = mid - 1;
            }
            else
            {
                // Ищем в правой половине
                start = mid + 1;
            }
        }

        // Если не нашли, возвращаем start как позицию вставки
        return start;
    }


    /// <summary>Вызывает принудительную пересортировку коллекции.</summary>
    public void Refresh()
    {
        Items.Sort(sortingStrategy.Comparer);
        OnCollectionReset();
    }
}
