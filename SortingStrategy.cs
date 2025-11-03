using System.Collections.Specialized;

namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc cref="ISortingStrategy{T}"/>
public sealed class SortingStrategy<T> : ISortingStrategy<T>
{
    /// <summary>Инициализирует новый экземпляр класса <see cref="SortingStrategy{T}"/></summary>
    /// <param name="comparer">Компаратор элементов</param>
    /// <param name="propertyNames">Наименования свойст элемента, изменение которых вызовет переоценку индекса позиции элемента</param>
    public SortingStrategy(IComparer<T> comparer, params string[] propertyNames)
    {
        Comparer = comparer;
        PropertyNames = [.. propertyNames];
        PropertyNames.CollectionChanged += PropertyNames_CollectionChanged;
    }

    /// <inheritdoc cref="ISortingStrategy{T}.Changed"/>
    public event EventHandler? Changed;

    /// <inheritdoc cref="ISortingStrategy{T}.Comparer"/>
    public IComparer<T> Comparer { get; }

    /// <inheritdoc cref="ISortingStrategy{T}.IsSortAffected(string)"/>
    public bool IsSortAffected(string propertyName)
    {
        return PropertyNames.Count == 0 || PropertyNames.Contains(propertyName);
    }

    /// <summary>Коллекция, содержащая наименования свойст элемента, изменение которых вызовет переоценку индекса позиции элемента.</summary>
    public ObservableCollection<string> PropertyNames { get; }

    private void PropertyNames_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Move)
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
