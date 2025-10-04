using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;

internal class ReadOnlyObservableCollectionShell<T> : IReadOnlyObservableCollection<T>
{
    internal readonly System.Collections.ObjectModel.ReadOnlyObservableCollection<T> Source;

    public ReadOnlyObservableCollectionShell(System.Collections.ObjectModel.ReadOnlyObservableCollection<T> source)
    {
        Source = source;
        ((INotifyCollectionChanged)Source).CollectionChanged += Source_CollectionChanged;
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

    public T this[int index] => Source[index];

    public int Count => Source.Count;

    public bool Contains(T item) => Source.Contains(item);

    public int IndexOf(T item) => Source.IndexOf(item);

    public IEnumerator<T> GetEnumerator() => Source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Source.GetEnumerator();

    public void CopyTo(T[] array, int arrayIndex) => Source.CopyTo(array, arrayIndex);

 

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

}
