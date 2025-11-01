using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc cref="IReadOnlyObservableCollection{T}"/>
public class ReadOnlyObservableCollection<T> : System.Collections.ObjectModel.ReadOnlyCollection<T>, IReadOnlyObservableCollection<T>, IDisposable
{

    /// <summary>Создаёт новый экземпляр коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    public ReadOnlyObservableCollection(System.Collections.ObjectModel.ObservableCollection<T> source) : base(source)
    {
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
        ((INotifyPropertyChanged)source).PropertyChanged += Source_PropertyChanged;
    }

    /// <summary>Создаёт новый экземпляр коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    public ReadOnlyObservableCollection(IObservableCollection<T> source) : base(source)
    {
        source.CollectionChanged += Source_CollectionChanged;
        source.PropertyChanged += Source_PropertyChanged;
    }

    /// <summary>Создаёт новый экземпляр коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    public ReadOnlyObservableCollection(System.Collections.ObjectModel.ReadOnlyObservableCollection<T> source) : base(source)
    {
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
        ((INotifyPropertyChanged)source).PropertyChanged += Source_PropertyChanged;
    }

    /// <summary>Создаёт новый экземпляр коллеции только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    private ReadOnlyObservableCollection(IList<T> source) : base(source)
    {
        ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
        ((INotifyPropertyChanged)source).PropertyChanged += Source_PropertyChanged;
    }

    /// <summary>Пытается создать коллекцию только для чтения из объекта типа <see cref="INotifyCollectionChanged"/></summary>
    /// <param name="source">Коллекция с уведомлениями</param>
    /// <param name="result">Результат успешной операции в виде <see cref="ReadOnlyObservableCollection{T}"/></param>
    /// <remarks>Создание будет успешным, если <paramref name="source"/> реализует <see cref="IList{T}"/></remarks>
    public static bool TryCreate(INotifyCollectionChanged source, [NotNullWhen(true)] out ReadOnlyObservableCollection<T>? result)
    {
        if (source is ReadOnlyObservableCollection<T> asReadOnly)
        {
            result = asReadOnly;
            return true;
        }

        if (source is IList<T> asList)
        {
            result = new ReadOnlyObservableCollection<T>(asList);
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>Пытается создать коллекцию только для чтения из объекта типа <see cref="IList{T}"/></summary>
    /// <param name="source">Коллекция с уведомлениями</param>
    /// <param name="result">Результат успешной операции в виде <see cref="ReadOnlyObservableCollection{T}"/></param>
    /// <remarks>Создание будет успешным, если <paramref name="source"/> реализует <see cref="INotifyCollectionChanged"/></remarks>
    public static bool TryCreate(IList<T> source, [NotNullWhen(true)] out ReadOnlyObservableCollection<T>? result)
    {
        if (source is ReadOnlyObservableCollection<T> asReadOnly)
        {
            result = asReadOnly;
            return true;
        }

        if (source is INotifyCollectionChanged)
        {
            result = new ReadOnlyObservableCollection<T>(source);
            return true;
        }

        result = null;
        return false;
    }

    /// <inheritdoc/>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }



    private void Source_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e);
    }

   
    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;
    
    /// <summary>
    /// Вызывает событие <see cref="PropertyChanged"/>
    /// </summary>
    /// <param name="e">Аргументы события</param>
    protected void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

    private bool isDisposed = false;

    /// <inheritdoc cref="OnDispose"/>
    public void Dispose()
    {
        if (isDisposed) return;
        isDisposed = true;
        OnDispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>Удаляет подписку на событие измения коллекции источника</summary>
    protected virtual void OnDispose()
    {
        ((INotifyCollectionChanged)Items).CollectionChanged -= Source_CollectionChanged;
        ((INotifyPropertyChanged)Items).PropertyChanged -= Source_PropertyChanged;
    }

}
