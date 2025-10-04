using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;

internal class ObservableCollectionShell<T> : IObservableCollection<T>, IList
{
    private readonly System.Collections.ObjectModel.ObservableCollection<T> Source;

    public ObservableCollectionShell(System.Collections.ObjectModel.ObservableCollection<T> source)
    {
        Source = source;
        source.CollectionChanged += Source_CollectionChanged;
        ((INotifyPropertyChanged)Source).PropertyChanged += Source_PropertyChanged;
    }

    private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }

    private void Source_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }


    public T this[int index]
    {
        get => Source[index];
        set => Source[index] = value;
    }

    public int Count => Source.Count;

    public void Add(T item) => Source.Add(item);

    public void AddRange(IEnumerable<T> items)
    {
        T[] _items = [.. items];
        foreach (var item in _items)
        {
            Add(item);
        }
    }

    public void Insert(int index, T item) => Source.Insert(index, item);

    public void InsertRange(int index, IEnumerable<T> items)
    {
        T[] _items = [.. items];
        for (int i = 0; i < _items.Length; i++)
        {
            Insert(index + i, _items[i]);
        }
    }

    public bool Remove(T item) => Source.Remove(item);

    public void RemoveAt(int index) => Source.RemoveAt(index);

    public bool Contains(T item) => Source.Contains(item);

    public void Clear() => Source.Clear();

    public int IndexOf(T item) => Source.IndexOf(item);

    public void Move(int from, int to) => Source.Move(from, to);

    public T? Find(Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(match);

        for (int i = 0; i < Count; i++)
        {
            if (match(Source[i])) return Source[i];
        }
        return default;
    }
    public int FindIndex(Predicate<T> match) => FindIndex(0, Source.Count, match);
    public int FindIndex(int startIndex, Predicate<T> match) => FindIndex(startIndex, Source.Count - startIndex, match);
    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, Source.Count);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, Source.Count - count, nameof(startIndex));
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
        ArgumentNullException.ThrowIfNull(match);

        int endIndex = startIndex + count;
        for (int i = startIndex; i < endIndex; i++)
        {
            if (match(Source[startIndex])) return i;
        }
        return -1;
    }

    public IEnumerator<T> GetEnumerator() => Source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Source.GetEnumerator();

    public void CopyTo(T[] array, int arrayIndex) => Source.CopyTo(array, arrayIndex);

    bool ICollection<T>.IsReadOnly => false;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void RemoveRange(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Source.RemoveAt(index);
        }
    }

    public void CopyTo(int index, T[] array, int arrayIndex, int count)
    {
        throw new NotImplementedException();
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
            return Source.Contains((T)item!);
        }
        return false;
    }

    int IList.IndexOf(object? item)
    {
        if (ImplementsListHelper<T>.IsCompatibleObject(item))
        {
            return Source.IndexOf((T)item);
        }
        return -1;
    }

    void IList.Insert(int index, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        try
        {
            Source.Insert(index, (T)value);
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
            Source.Remove((T)value);
        }
    }

    bool IList.IsFixedSize => ((IList)Source).IsFixedSize;
    bool IList.IsReadOnly => ((IList)Source).IsReadOnly;

    object? IList.this[int index]
    {
        get => ((IList)Source)[index];
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

    void ICollection.CopyTo(Array array, int index) => ((ICollection)Source).CopyTo(array, index);

    bool ICollection.IsSynchronized => ((ICollection)Source).IsSynchronized;

    object ICollection.SyncRoot => ((ICollection)Source).SyncRoot;

    #endregion
}
