namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc cref="IFilteringStrategy{T}"/>
public sealed class FilteringStrategyDefault<T> : IFilteringStrategy<T>
{  
    /// <summary>Инициализирует новый экземпляр класса <see cref="FilteringStrategyDefault{T}"/></summary>
    /// <param name="predicate"></param>
    public FilteringStrategyDefault(Predicate<T> predicate)
    {
        Predicate = predicate;
    }

    /// <inheritdoc cref="IFilteringStrategy{T}.Changed"/>
    public event EventHandler? Changed;

    /// <inheritdoc cref="IFilteringStrategy{T}.Predicate"/>
    public Predicate<T> Predicate { get; }

    /// <inheritdoc cref="IFilteringStrategy{T}.IsFilterAffected(string)"/>
    public bool IsFilterAffected(string propertyName) => true;
}
