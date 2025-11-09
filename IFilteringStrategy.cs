namespace Kritjara.Collections.ObjectModel;

/// <summary>
/// Определяет стратегию фильтрации элементов для коллекции <see cref="FilteredObservableCollection{T}"/>.
/// </summary>
/// <typeparam name="T">Тип элементов в коллекции.</typeparam>
public interface IFilteringStrategy<T>
{
    /// <summary>Происходит при изменении условий фильтра.</summary>
    event EventHandler Changed;

    /// <summary>Делегат метода фильтрации элементов</summary>
    Predicate<T> Predicate { get; }

    /// <summary>Проверяет требуется ли переоценка видимости элемента после изменения указанного его свойства</summary>
    /// <param name="propertyName">Наименовение свойства, значение которого изменилось</param>
    public bool IsFilterAffected(string propertyName);
}
