namespace Kritjara.Collections.ObjectModel;

/// <summary>
/// Определяет стратегию сортировки для коллекции, включая компаратор элементов, уведомления об изменениях условий сортировки и проверку влияния изменений свойств на порядок.
/// </summary>
/// <typeparam name="T">Тип сортируемых элементов.</typeparam>
public interface ISortingStrategy<T>
{
    /// <summary>Происходит при изменении условий сравнения элементов.</summary>
    event EventHandler Changed;

    /// <summary>Компаратор элементов</summary>
    IComparer<T> Comparer { get; }

    /// <summary>Проверяет указанное наименование свойства объекта на то, требуется ли переоценка индекса позиции элемента внутри коллекции после изменения значения этого свойства</summary>
    /// <param name="propertyName">Наименование свойства объекта, значение которого изменилось.</param>
    /// <returns><see langword="true"/> - если нужна переоценка индекса, в противнос случае - <see langword="false"/></returns>
    public bool IsSortAffected(string propertyName);
}
