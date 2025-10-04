using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kritjara.Collections.ObjectModel;

/// <summary>Представляет коллецию, которая реализует <see cref="INotifyCollectionChanged"/>.</summary>
/// <typeparam name="T">Тип элементов, содержащихся в коллекции.</typeparam>
[CollectionBuilder(typeof(ObservableFactory), nameof(ObservableFactory.CreateObservableCollection))]
public interface IObservableCollection<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyList<T>
{
    /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
    void AddRange(IEnumerable<T> collection);

    /// <inheritdoc cref="List{T}.InsertRange(int, IEnumerable{T})"/>
    void InsertRange(int index, IEnumerable<T> collection);

    /// <inheritdoc cref="System.Collections.ObjectModel.ObservableCollection{T}.Move(int, int)"/>
    public void Move(int oldIndex, int newIndex);

    /// <inheritdoc cref="List{T}.RemoveRange(int, int)"/>
    void RemoveRange(int index, int count);

    /// <summary>Создаёт пустую коллекцию.</summary>
    static IObservableCollection<T> Create() => new ObservableCollection<T>();

    /// <summary>Создаёт пустую коллекцию с указанной начальной ёмкостью.</summary>
    /// <param name="capacity">Начальная ёмкость списка.</param>
    static IObservableCollection<T> Create(int capacity) => new ObservableCollection<T>(capacity);

    /// <summary>Создаёт коллекцию, содержащую скопированные элементы из указанной коллекции.</summary>
    /// <param name="items">Коллекция, элементы которой будут скопированы.</param>
    static IObservableCollection<T> Create(IEnumerable<T> items) => new ObservableCollection<T>(items);

    /// <summary>Создаёт оболочку над стандартной наблюдаемой коллекцией в виде <see cref="IObservableCollection{T}"/>.</summary>
    /// <param name="source">Наблюдаемая коллекция.</param>
    static IObservableCollection<T> Create(System.Collections.ObjectModel.ObservableCollection<T> source) => new ObservableCollectionShell<T>(source);
}
