namespace Kritjara.Collections.ObjectModel;

/// <summary>Коллекция для хранения последних использованных элементов.</summary>
/// <typeparam name="T"></typeparam>
public class RecentItemsCollection<T> : ReadOnlyObservableCollection<T>
{
    private new ObservableCollection<T> Items => (ObservableCollection<T>)base.Items;

    private static T[] CheckItemsAndSize(IEnumerable<T> items, int maxSize)
    {
        T[] result = [.. items];
        if (result.Length > maxSize)
        {
            throw new ArgumentException("Количество начальных элементов коллекции превышает указанную максимальную ёмкость коллекции", nameof(items));
        }
        return result;
    }

    /// <summary></summary>
    /// <param name="maxSize">Максимальный размер коллекции</param>
    /// <exception cref="ArgumentException"></exception>
    public RecentItemsCollection(int maxSize) : base(new ObservableCollection<T>(maxSize))
    {
        Helper.ThrowIfValueSizeLessZero(maxSize);
        this.maxSize = maxSize;
    }

    /// <summary>Инициализирует новый экземпляр класса <see cref="RecentItemsCollection{T}"/></summary>
    /// <param name="items">Начальные элементы коллекции</param>
    /// <param name="maxSize">Максимальный размер коллекции</param>
    /// <exception cref="ArgumentException"></exception>
    public RecentItemsCollection(IEnumerable<T> items, int maxSize) : base(new ObservableCollection<T>(CheckItemsAndSize(items, maxSize), maxSize))
    {
        Helper.ThrowIfValueSizeLessZero(maxSize);
        this.maxSize = maxSize;
    }

    private int maxSize;
    /// <summary>
    /// Максимальный размер коллекции
    /// </summary>
    public int MaxSize
    {
        get => maxSize;
        set
        {
            if (maxSize == value) return;

            Helper.ThrowIfValueSizeLessZero(value);

            int countToRemove = Items.Count - value;
            if (Items.Count > value)
            {
                for (int i = 0; i < countToRemove; i++)
                {
                    Items.RemoveAt(Items.Count - 1);
                }
            }

            maxSize = value;
        }
    }

    /// <summary>
    /// Добавляет элемент в начало списка
    /// </summary>
    /// <param name="item"></param>
    public void Touch(T item)
    {
        int index = Items.IndexOf(item);
        if (index != -1)
        {
            Items.Move(index, 0);
            return;
        }

        if (Items.Count < maxSize)
        {
            Items.Insert(0, item);
        }
        else
        {
            Items.RemoveAt(Items.Count - 1);
            Items.Insert(0, item);
        }
    }

    /// <summary>Удаляет указанный элемент из коллекции</summary>
    /// <param name="item">Элемент для удаления</param>
    public bool Remove(T item) => Items.Remove(item);

    /// <summary>Очищает коллекцию</summary>
    public void Clear() => Items.Clear();

}
