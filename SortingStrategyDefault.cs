namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc cref="ISortingStrategy{T}"/>
public sealed class SortingStrategyDefault<T> : ISortingStrategy<T>
{
    /// <summary>Инициализирует новый экземпляр класса <see cref="SortingStrategy{T}"/></summary>
    /// <param name="comparer">Компаратор элементов</param>
    public SortingStrategyDefault(IComparer<T> comparer)
    {
        Comparer = comparer;
    }

    /// <inheritdoc cref="ISortingStrategy{T}.Changed"/>
    public event EventHandler? Changed;

    /// <inheritdoc cref="ISortingStrategy{T}.Comparer"/>
    public IComparer<T> Comparer { get; }

    /// <inheritdoc cref="ISortingStrategy{T}.IsSortAffected(string)"/>
    public bool IsSortAffected(string propertyName) => true;
      
}