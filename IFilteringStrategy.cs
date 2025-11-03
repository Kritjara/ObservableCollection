namespace Kritjara.Collections.ObjectModel;

/// <summary>
/// Определяет стратегию фильтрации для коллекции, включая предикат элементов, 
/// уведомления об изменениях условий фильтрации и проверку влияния изменений свойств на фильтр.
/// </summary>
/// <typeparam name="T">Тип элементов в коллекции.</typeparam>
public interface IFilteringStrategy<T>
{
    /// <summary>Происходит при изменении условий фильтра.</summary>
    event EventHandler Changed;

    /// <summary>Делегат метода фильтрации элементов</summary>
    Predicate<T> Predicate { get; }

    /// <summary>Проверяет требуется ли переоценка фильтра элемента после изменения указанного его свойства</summary>
    public bool IsFilterAffected(string propertyName);
}
