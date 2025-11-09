using System.Collections.Specialized;

namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc cref="IFilteringStrategy{T}"/>
public sealed class FilteringStrategy<T> : IFilteringStrategy<T>
{
    /// <summary>Инициализирует новый экземпляр класса <see cref="IFilteringStrategy{T}"/></summary>
    /// <param name="predicate">Делегат метода фильтрации элементов</param>
    /// <param name="propertyNames">Наименования свойств объекта, изменение которых будет вызывать переоценку видимости объекта внутри коллекции</param>
    public FilteringStrategy(Predicate<T> predicate, params string[] propertyNames)
    {
        Predicate = predicate;
        PropertyNames = [.. propertyNames];
        PropertyNames.CollectionChanged += PropertyNames_CollectionChanged;
    }

    /// <inheritdoc cref="IFilteringStrategy{T}.Changed"/>
    public event EventHandler? Changed;

    /// <inheritdoc cref="IFilteringStrategy{T}.Predicate"/>
    public Predicate<T> Predicate { get; }

    /// <inheritdoc cref="IFilteringStrategy{T}.IsFilterAffected(string)"/>
    public bool IsFilterAffected(string propertyName)
    {        
        return PropertyNames.Count == 0 || PropertyNames.Contains(propertyName);
    }

    /// <summary>Коллекция, содержащая наименования свойств объекта, изменение которых вызывает переоценку видимости объекта внутри коллекции.</summary>
    public ObservableCollection<string> PropertyNames { get; }

    private void PropertyNames_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Move)
        {
            Changed?.Invoke(this, e);
        }
    }
}
