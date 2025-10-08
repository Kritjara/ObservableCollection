using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;

/// <summary>Представляет коллецию только для чтения, которая реализует <see cref="INotifyCollectionChanged"/>.</summary>
/// <typeparam name="T">Тип элементов, содержащихся в коллекции.</typeparam>
public interface IReadOnlyObservableCollection<out T> : IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    /// <summary>Создаёт коллецию только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    static IReadOnlyObservableCollection<T> Create(IObservableCollection<T> source)
    {
        if (source is IReadOnlyObservableCollection<T> readonlycollection)
        {
            return readonlycollection;
        }
        return new ReadOnlyObservableCollection<T>(source);
    }

    /// <summary>Создаёт коллецию только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    static IReadOnlyObservableCollection<T> Create(System.Collections.ObjectModel.ReadOnlyObservableCollection<T> source) => new ReadOnlyObservableCollection<T>(source);

    /// <summary>Создаёт коллецию только для чтения.</summary>
    /// <param name="source">Основной источник элементов.</param>
    static IReadOnlyObservableCollection<T> Create(System.Collections.ObjectModel.ObservableCollection<T> source) => new ReadOnlyObservableCollection<T>(source);

}
