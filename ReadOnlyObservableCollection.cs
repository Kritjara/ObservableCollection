using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc cref="IReadOnlyObservableCollection{T}"/>
public class ReadOnlyObservableCollection<T> : IReadOnlyObservableCollection<T>, IList<T>, IList
{
    /// <summary>Основной источник элементов</summary>
    internal readonly IList<T> Source;

    /// <summary>Создаёт новый экземпляр коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    public ReadOnlyObservableCollection(System.Collections.ObjectModel.ObservableCollection<T> source)
    {
        Source = source;
        ((INotifyCollectionChanged)Source).CollectionChanged += Source_CollectionChanged;
        ((INotifyPropertyChanged)Source).PropertyChanged += Source_PropertyChanged;
    }

    /// <summary>Создаёт новый экземпляр коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    public ReadOnlyObservableCollection(IObservableCollection<T> source)
    {
        Source = source;
        ((INotifyCollectionChanged)Source).CollectionChanged += Source_CollectionChanged;
        ((INotifyPropertyChanged)Source).PropertyChanged += Source_PropertyChanged;
    }


    /// <inheritdoc/>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }


    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    private void Source_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }


    /// <inheritdoc/>
    public T this[int index] => Source[index];

    /// <inheritdoc/>
    public int Count => Source.Count;

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => Source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Source.GetEnumerator();



    #region [ IList<T> ]

    int IList<T>.IndexOf(T item) => Source.IndexOf(item);

    void IList<T>.Insert(int index, T item) => ThrowNotSupportedException();

    void IList<T>.RemoveAt(int index) => ThrowNotSupportedException();

    T IList<T>.this[int index]
    {
        get => Source[index];
        set
        {
            ThrowNotSupportedException();
        }
    }

    void ICollection<T>.Add(T item) => ThrowNotSupportedException();

    void ICollection<T>.Clear() => ThrowNotSupportedException();

    bool ICollection<T>.Contains(T item) => Source.Contains(item);

    void ICollection<T>.CopyTo(T[] array, int arrayIndex) => Source.CopyTo(array, arrayIndex);

    bool ICollection<T>.Remove(T item)
    {
        ThrowNotSupportedException();
        return false;
    }

    int ICollection<T>.Count => Source.Count;
    bool ICollection<T>.IsReadOnly => true;

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Source.GetEnumerator(); 

    #endregion

    #region [ IList ]

    int IList.Add(object? value)
    {
        ThrowNotSupportedException();
        return -1;
    }

    void IList.Clear()
    {
        ThrowNotSupportedException();
    }

    bool IList.Contains(object? value)
    {
        if (value is null || !value.GetType().IsAssignableFrom(typeof(T)))
        {
            return false;
        }
        return Source.Contains((T)value);
    }

    int IList.IndexOf(object? value)
    {
        if (value is null || !value.GetType().IsAssignableFrom(typeof(T)))
        {
            return -1;
        }
        return Source.IndexOf((T)value);
    }

    void IList.Insert(int index, object? value)
    {
        ThrowNotSupportedException();
    }

    void IList.Remove(object? value)
    {
        ThrowNotSupportedException();
    }

    void IList.RemoveAt(int index)
    {
        ThrowNotSupportedException();
    }

    bool IList.IsFixedSize { get; } = false;
    bool IList.IsReadOnly { get; } = true;

    object? IList.this[int index]
    {
        get => Source[index];
        set
        {
            ThrowNotSupportedException();
        }
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");

        if (array.Rank != 1)
            throw new ArgumentException("Multidimensional arrays are not supported.");

        if (array.Length - index < Source.Count)
            throw new ArgumentException("The number of elements in the source collection is greater than the available space from index to the end of the destination array.");

        // Проверка типа массива
        if (array is T[] typedArray)
        {
            Source.CopyTo(typedArray, index);
        }
        else
        {
            // Для нетипизированных массивов копируем элементы по одному
            Type? elementType = array.GetType().GetElementType();
            if (elementType is null || !elementType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("Invalid array type.");
            }

            for (int i = 0; i < Source.Count; i++)
            {
                array.SetValue(Source[i], index + i);
            }
        }
    }

    bool ICollection.IsSynchronized { get; } = false;
    object ICollection.SyncRoot => this; 

    #endregion

    [DoesNotReturn]
    private static void ThrowNotSupportedException()
    {
        throw new NotSupportedException("Эта коллекция только для чтения.");
    }

}
