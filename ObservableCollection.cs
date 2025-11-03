using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;


/// <inheritdoc cref="IObservableCollection{T}"/>
[System.Runtime.CompilerServices.CollectionBuilder(typeof(ObservableFactory), nameof(ObservableFactory.CreateObservableCollection))]
public class ObservableCollection<T> : IList, IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged, IObservableCollection<T>, IReadOnlyObservableCollection<T>
{
    internal static ObservableCollection<T> CreateObservableCollection(ref ReadOnlySpan<T> items)
    {
        ObservableCollection<T> coll = new ObservableCollection<T>(items.Length);
        coll.Items.AddRange(items);
        return coll;
    }

    /// <summary>
    /// Список элементов.
    /// </summary>
    protected internal readonly List<T> Items;

    ///<summary>Инициализирует новый экземпляр класса <see cref="ObservableCollection{T}"/>.</summary>
    public ObservableCollection()
    {
        Items = [];
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="ObservableCollection{T}"/> с указанной начальной ёмкостью.</summary>
    ///<param name="capacity">Начальная ёмкость коллекции.</param>
    public ObservableCollection(int capacity)
    {
        Items = new List<T>(capacity);
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="ObservableCollection{T}"/>, содержащий скопированные элементы из указанной коллекции и с указанной начальной ёмкостью.</summary>
    ///<param name="items">Элементы для копирования</param>
    ///<param name="capacity">Начальная ёмкость коллекции.</param>
    ///<remarks>Указание <paramref name="capacity"/> при создании может оказаться лишним, если <paramref name="items"/> содержит больше элементов.</remarks>
    public ObservableCollection(IEnumerable<T> items, int capacity)
    {
        Items = new List<T>(capacity);
        Items.AddRange(items);
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="ObservableCollection{T}"/>, содержащий скопированные элементы из указанной коллекции.</summary>
    ///<param name="items">Элементы для копирования</param>
    public ObservableCollection(IEnumerable<T> items)
    {
        Items = [.. items];
    }

    ///<summary>Инициализирует новый экземпляр класса <see cref="ObservableCollection{T}"/>, содержащий скопированные элементы из указанной коллекции.</summary>
    public ObservableCollection(ReadOnlySpan<T> items)
    {
        Items = [.. items];
    }

    /// <summary>Получает или задает элемент элемент по указанному индексу.</summary>
    /// <param name="index">Индекс элемента.</param>
    /// <returns></returns>
    public virtual T this[int index]
    {
        get => Items[index];
        set
        {
            OnReplace(index, value);
        }
    }

    /// <inheritdoc/>
    public virtual void Add(T item)
    {
        int index = Items.Count;
        OnAdd(index, item);
    }


    /// <inheritdoc/>
    public virtual void AddRange(IEnumerable<T> collection)
    {
        int index = Items.Count;
        OnAddRange(index, collection);
    }

    /// <inheritdoc/>
    public virtual void Insert(int index, T item)
    {
        OnAdd(index, item);
    }


    /// <inheritdoc/>
    public virtual void InsertRange(int index, IEnumerable<T> collection)
    {
        OnAddRange(index, collection);
    }

    /// <inheritdoc/>
    public virtual void Move(int oldIndex, int newIndex)
    {
        OnMoveItem(oldIndex, newIndex);
    }

    /// <inheritdoc/>
    public virtual bool Remove(T item)
    {
        return OnRemove(item);
    }

    /// <inheritdoc/>
    public virtual void RemoveAt(int index)
    {
        OnRemoveAt(index);
    }

    /// <inheritdoc/>
    public virtual void RemoveRange(int index, int count)
    {
        OnRemoveRange(index, count);
    }

    /// <inheritdoc/>
    public virtual void Clear()
    {
        OnClearItems();
    }

    /// <summary>
    /// Удаляет все элементы коллекции и добавляет указанные элементы
    /// </summary>
    /// <param name="items"></param>
    public virtual void Reset(IEnumerable<T>? items)
    {
        OnReset(items);
    }

    /// <inheritdoc cref="List{T}.BinarySearch(T)"/>
    public int BinarySearch(T item) => Items.BinarySearch(item);

    /// <inheritdoc cref="List{T}.BinarySearch(T, IComparer{T})"/>
    public int BinarySearch(T item, IComparer<T>? comparer) => Items.BinarySearch(item, comparer);

    /// <inheritdoc/>
    public int IndexOf(T item) => Items.IndexOf(item);

    /// <inheritdoc/>
    public bool Contains(T item) => Items.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        Items.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc cref="List{T}.CopyTo(int, T[], int, int)"/>
    public void CopyTo(int index, T[] array, int arrayIndex, int count) => Items.CopyTo(index, array, arrayIndex, count);

    /// <inheritdoc cref="List{T}.Find(Predicate{T})"/>
    public T? Find(Predicate<T> match) => Items.Find(match);

    /// <inheritdoc cref="List{T}.FindIndex(Predicate{T})"/>
    public int FindIndex(Predicate<T> match) => Items.FindIndex(match);

    /// <inheritdoc cref="List{T}.FindIndex(int, Predicate{T})"/>
    public int FindIndex(int startIndex, Predicate<T> match) => Items.FindIndex(startIndex, match);

    /// <inheritdoc cref="List{T}.FindIndex(int, int, Predicate{T})"/>
    public int FindIndex(int startIndex, int count, Predicate<T> match) => Items.FindIndex(startIndex, count, match);

    /// <inheritdoc/>
    public int Count => Items.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();


    /// <summary>Заменяет элемент в коллекции>.</summary>
    /// <param name="index">Индекс элемента, который будет заменен.</param>
    /// <param name="item">Новый элемент.</param>
    protected virtual void OnReplace(int index, T item)
    {
        CheckReentrancy();
        T originalItem = this[index];
        Items[index] = item;

        OnIndexerPropertyChanged();
        OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
    }

    /// <summary>Добавляет элемент в коллекцию в указанную позицию.</summary>
    /// <param name="index">Индекс, по которому будет добавлен элемент.</param>
    /// <param name="item">Добавляемый элемент.</param>
    protected virtual void OnAdd(int index, T item)
    {
        CheckReentrancy();
        Items.Insert(index, item);

        OnCountPropertyChanged();
        OnIndexerPropertyChanged();
        OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
    }

    /// <summary>Добавялет диапазон элементов в указанную позицию.</summary>
    /// <param name="index">Индекс, по которому будет добавлен диапазон.</param>
    /// <param name="items">Элементы для добавления в коллекцию.</param>
    protected virtual void OnAddRange(int index, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        T[] newItems = [.. items];

        Items.InsertRange(index, newItems);

        for (int i = 0; i < newItems.Length; i++)
        {
            OnCollectionChanged(NotifyCollectionChangedAction.Add, newItems[i], index + i);
        }
    }

    /// <summary>Удаляет элемент из коллекции по указанному индексу.</summary>
    /// <param name="index">Индекс элемента, который требуется удалить</param>
    protected virtual void OnRemoveAt(int index)
    {
        CheckReentrancy();
        T removedItem = this[index];

        Items.RemoveAt(index);

        OnCountPropertyChanged();
        OnIndexerPropertyChanged();
        OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
    }

    /// <summary>Удаляет указанный элемент из коллекции.</summary>
    /// <param name="item">Элемент для удаления.</param>
    /// <returns></returns>
    protected virtual bool OnRemove(T item)
    {
        CheckReentrancy();

        int index = Items.IndexOf(item);
        T removedItem = this[index];

        if (index != -1)
        {
            Items.RemoveAt(index);

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Удаляет диапазон элементов из коллекции
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    protected virtual void OnRemoveRange(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            OnRemoveAt(index);
        }
    }

    /// <summary>Перемещает элемент c указанным индексу в новое место в коллекции.</summary>
    /// <param name="oldIndex"></param>
    /// <param name="newIndex"></param>
    protected virtual void OnMoveItem(int oldIndex, int newIndex)
    {
        CheckReentrancy();

        T removedItem = this[oldIndex];

        Items.RemoveAt(oldIndex);
        Items.Insert(newIndex, removedItem);

        OnIndexerPropertyChanged();
        OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
    }

    /// <summary>Удаляет все элементы из коллекции.</summary>
    protected virtual void OnClearItems()
    {
        CheckReentrancy();
        Items.Clear();
        OnCountPropertyChanged();
        OnIndexerPropertyChanged();
        OnCollectionReset();
    }

    /// <summary>Удаляет все элементы из коллекции и вставляет новые.</summary>
    protected virtual void OnReset(IEnumerable<T>? items)
    {
        CheckReentrancy();

        var newItems = items?.ToList() ?? [];

        // Если коллекция пустая и добавляем пустую - ничего не делаем
        if (Items.Count == 0 && newItems.Count == 0)
            return;

        Items.Clear();
        Items.AddRange(newItems);
        OnCountPropertyChanged();
        OnIndexerPropertyChanged();
        OnCollectionReset();
    }


    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Вызывает событие <see cref="PropertyChanged"/>.</summary>
    /// <param name="e"></param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

    /// <summary>
    /// Helper to raise PropertyChanged event with PropertyName == Count
    /// </summary>
    protected void OnCountPropertyChanged() => OnPropertyChanged(EventArgsCache.CountPropertyChanged);

    /// <summary>
    /// Helper to raise PropertyChanged event with PropertyName == Items[]
    /// </summary>
    protected void OnIndexerPropertyChanged() => OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);


    /// <inheritdoc/>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;



    /// <summary>
    /// Raise CollectionChanged event to any listeners.
    /// Properties/methods modifying this ObservableCollection will raise
    /// a collection changed event through this virtual method.
    /// </summary>
    /// <remarks>
    /// When overriding this method, either call its base implementation
    /// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
    /// </remarks>
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        NotifyCollectionChangedEventHandler? handler = CollectionChanged;
        if (handler != null)
        {
            // Not calling BlockReentrancy() here to avoid the SimpleMonitor allocation.
            _blockReentrancyCount++;
            try
            {
                handler(this, e);
            }
            finally
            {
                _blockReentrancyCount--;
            }
        }
    }

    private int _blockReentrancyCount;

    /// <summary> Check and assert for reentrant attempts to change this collection. </summary>
    /// <exception cref="InvalidOperationException"> raised when changing the collection
    /// while another collection change is still being notified to other listeners </exception>
    protected void CheckReentrancy()
    {
        if (_blockReentrancyCount > 0)
        {
            // we can allow changes if there's only one listener - the problem
            // only arises if reentrant changes make the original event args
            // invalid for later listeners.  This keeps existing code working
            // (e.g. Selector.SelectedItems).
            if (CollectionChanged?.GetInvocationList().Length > 1)
                throw new InvalidOperationException("ObservableCollectionReentrancyNotAllowed");
        }
    }


    /// <summary>
    /// Helper to raise CollectionChanged event to any listeners
    /// </summary>
    protected void OnCollectionChanged(NotifyCollectionChangedAction action, object? item, int index)
    {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
    }

    /// <summary>
    /// Helper to raise CollectionChanged event to any listeners
    /// </summary>
    protected void OnCollectionChanged(NotifyCollectionChangedAction action, object? item, int index, int oldIndex)
    {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
    }

    /// <summary>
    /// Helper to raise CollectionChanged event to any listeners
    /// </summary>
    protected void OnCollectionChanged(NotifyCollectionChangedAction action, object? oldItem, object? newItem, int index)
    {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
    }

    /// <summary>
    /// Helper to raise CollectionChanged event with action == Reset to any listeners
    /// </summary>
    protected void OnCollectionReset() => OnCollectionChanged(EventArgsCache.ResetCollectionChanged);

    /// <summary>
    /// Disallow reentrant attempts to change this collection. E.g. an event handler
    /// of the CollectionChanged event is not allowed to make changes to this collection.
    /// </summary>
    /// <remarks>
    /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
    /// <code>
    ///         using (BlockReentrancy())
    ///         {
    ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
    ///         }
    /// </code>
    /// </remarks>   
    protected IDisposable BlockReentrancy()
    {
        _blockReentrancyCount++;
        return EnsureMonitorInitialized();
    }

    private SimpleMonitor? _monitor;
    private SimpleMonitor EnsureMonitorInitialized() => _monitor ??= new SimpleMonitor(this);

    private sealed class SimpleMonitor : IDisposable
    {
        internal ObservableCollection<T> _collection;

        public SimpleMonitor(ObservableCollection<T> collection)
        {
            System.Diagnostics.Debug.Assert(collection != null);
            _collection = collection;
        }

        public void Dispose() => _collection._blockReentrancyCount--;
    }

    #region [ IList ]

    int IList.Add(object? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        try
        {
            Add((T)value);
        }
        catch (InvalidCastException ex)
        {
            ImplementsListHelper<T>.ThrowWrongValueTypeArgumentException(value, ex);
        }

        return Count - 1;
    }

    bool IList.Contains(object? item)
    {
        if (ImplementsListHelper<T>.IsCompatibleObject(item))
        {
            return Items.Contains((T)item);
        }
        return false;
    }

    int IList.IndexOf(object? item)
    {
        if (ImplementsListHelper<T>.IsCompatibleObject(item))
        {
            return Items.IndexOf((T)item);
        }
        return -1;
    }

    void IList.Insert(int index, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        try
        {
            OnAdd(index, (T)value);
        }
        catch (InvalidCastException ex)
        {
            ImplementsListHelper<T>.ThrowWrongValueTypeArgumentException(value, ex);
        }
    }

    void IList.Remove(object? value)
    {
        if (ImplementsListHelper<T>.IsCompatibleObject(value))
        {
            OnRemove((T)value);
        }
    }

    bool IList.IsFixedSize => ((IList)Items).IsFixedSize;

    object? IList.this[int index]
    {
        get => ((IList)Items)[index];
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            try
            {
                this[index] = (T)value;
            }
            catch (InvalidCastException ex)
            {
                ImplementsListHelper<T>.ThrowWrongValueTypeArgumentException(value, ex);
            }
        }
    }

    void ICollection.CopyTo(Array array, int index) => ((ICollection)Items).CopyTo(array, index);

    bool ICollection.IsSynchronized => ((ICollection)Items).IsSynchronized;

    object ICollection.SyncRoot => ((ICollection)Items).SyncRoot;

    #endregion
}
