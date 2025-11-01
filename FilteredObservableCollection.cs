using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Markup;

namespace Kritjara.Collections.ObjectModel;

/// <inheritdoc cref="IReadOnlyObservableCollection{T}"/>
/// <summary>
/// Представляет фильтрованную коллекцию только для чтения, которая реализует <see cref="INotifyCollectionChanged"/> и <see cref="INotifyPropertyChanged"/>.
/// Коллекция автоматически обновляется при изменении элементов источника или их свойств, применяя заданный предикат фильтрации.
/// </summary>
public class FilteredObservableCollection<T> : ReadOnlyObservableCollection<T> where T : INotifyPropertyChanged
{
    /// <summary>
    /// Предикат для фильтрации элементов.
    /// </summary>
    private Predicate<T> filter;

    /// <summary>
    /// Видимая коллекция, содержащая только элементы, прошедшие фильтр.
    /// </summary>
    private readonly ObservableCollection<T> visible;

    /// <summary>
    /// Источник элементов для фильтрации.
    /// </summary>
    private readonly IObservableCollection<T> source;

    private readonly ObservableCollection<T> interim;

    /// <inheritdoc cref="filter"/>
    public Predicate<T> Filter
    {
        get => filter;
        set
        {
            if (ReferenceEquals(filter, value)) return;

            filter = value;
            visible.Reset(source.Where(item => filter(item)));
            OnPropertyChanged(EventArgsCache.FilterPropertyChanged);
        }
    }


    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FilteredObservableCollection{T}"/> с указанным источником и предикатом фильтрации.
    /// </summary>
    /// <param name="source">Источник элементов для фильтрации.</param>
    /// <param name="filter">Предикат для фильтрации элементов.</param>
    public FilteredObservableCollection(IObservableCollection<T> source, Predicate<T> filter) : base(new ObservableCollection<T>(source.Where(item => filter(item))))
    {
        this.source = source;
        this.filter = filter;
        interim = [.. source];
        visible = (ObservableCollection<T>)Items; // Items из base - это visible

        source.CollectionChanged += Source_CollectionChanged;
        interim.CollectionChanged += Interim_CollectionChanged;
        foreach (var item in interim)
        {
            item.PropertyChanged += OnItemPropertyChanged;
        }
    }

    /// <summary>
    /// Обрабатывает изменения в источнике.
    /// </summary>
    private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:

                T addedItem = (T)e.NewItems![0]!;
                addedItem.PropertyChanged += OnItemPropertyChanged;
                interim.Insert(e.NewStartingIndex, addedItem);

                break;

            case NotifyCollectionChangedAction.Remove:

                T removedItem = (T)e.OldItems![0]!;
                removedItem.PropertyChanged -= OnItemPropertyChanged;
                interim.RemoveAt(e.OldStartingIndex);

                break;

            case NotifyCollectionChangedAction.Replace:

                T oldItem = (T)e.OldItems![0]!;
                oldItem.PropertyChanged -= OnItemPropertyChanged;
                T newItem = (T)e.NewItems![0]!;
                newItem.PropertyChanged += OnItemPropertyChanged;
                interim[e.NewStartingIndex] = newItem;

                break;

            case NotifyCollectionChangedAction.Move:

                interim.Move(e.OldStartingIndex, e.NewStartingIndex);

                break;

            case NotifyCollectionChangedAction.Reset:

                foreach (var item in interim)
                {
                    item.PropertyChanged -= OnItemPropertyChanged;
                }

                interim.Reset(source);

                foreach (var item in interim)
                {
                    item.PropertyChanged += OnItemPropertyChanged;
                }

                break;

            default:
                break;
        }


    }

    /// <summary>
    /// Обрабатывает изменения в источнике.
    /// </summary>
    private void Interim_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        void ItemAdded(T item)
        {
            if (filter(item))
            {
                int insertIndex = FindInsertPosition(item);
                visible.Insert(insertIndex, item);
            }
        }

        void ItemRemoved(T item)
        {
            if (filter(item))
            {
                visible.Remove(item);
            }
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:

                ItemAdded((T)e.NewItems![0]!);

                break;

            case NotifyCollectionChangedAction.Remove:

                ItemRemoved((T)e.OldItems![0]!);

                break;

            case NotifyCollectionChangedAction.Replace:

                ItemRemoved((T)e.OldItems![0]!);
                ItemAdded((T)e.NewItems![0]!);

                break;

            case NotifyCollectionChangedAction.Move:
                break;

            case NotifyCollectionChangedAction.Reset:

                visible.Reset(source.Where(item => filter(item)));

                break;

            default:
                break;
        }
    }



    /// <summary>
    /// Обрабатывает изменения свойств элементов.
    /// </summary>
    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        T item = (T)sender!;

        bool currentlyVisible = visible.Contains(item);
        bool shouldBeVisible = filter(item);

        if (currentlyVisible && !shouldBeVisible)
        {
            // Удаляем из visible
            visible.Remove(item);
        }
        else if (!currentlyVisible && shouldBeVisible)
        {
            // Добавляем в visible в порядке добавления (или можно сортировать, но по умолчанию - порядок источника)
            int insertIndex = FindInsertPosition(item);
            visible.Insert(insertIndex, item);
        }
        // Если элемент уже правильно отфильтрован, ничего не делаем
    }

    /// <summary>
    /// Находит позицию для вставки элемента в видимую коллекцию (в порядке добавления из источника).
    /// </summary>
    private int FindInsertPosition(T item)
    {
        // Для простоты вставляем в конец, сохраняя порядок добавления
        // в будущем можно улучшить
        return visible.Count;
    }

  
    /// <summary>
    /// Удаляет подписку на событие <see cref="INotifyCollectionChanged"/> коллекции источника, а также - <see cref="INotifyPropertyChanged"/> всех элементов источника 
    /// </summary>
    protected override void OnDispose()
    {
        base.OnDispose();
        interim.CollectionChanged -= Interim_CollectionChanged;
        source.CollectionChanged -= Source_CollectionChanged;
        foreach (var item in interim)
        {
            item.PropertyChanged -= OnItemPropertyChanged;
        }
    }
}
