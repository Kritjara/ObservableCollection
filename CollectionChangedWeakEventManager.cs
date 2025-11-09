using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace Kritjara.Collections.ObjectModel;

/// <summary>
/// Слабый менеджер событий для CollectionChanged.
/// Позволяет подписываться на события CollectionChanged объектов типа INotifyCollectionChanged без сильных ссылок.
/// </summary>
public static class CollectionChangedWeakEventManager
{
    // Таблица: источник -> список слабых ссылок на обработчики
    private static readonly ConditionalWeakTable<INotifyCollectionChanged, List<WeakReference<NotifyCollectionChangedEventHandler>>> _handlers = [];

    /// <summary>
    /// Добавляет слабый обработчик события CollectionChanged для источника.
    /// </summary>
    /// <param name="source">Источник события (INotifyCollectionChanged).</param>
    /// <param name="handler">Обработчик события.</param>

    public static void AddHandler(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(handler);

        // Получаем или создаем список handlers для источника
        List<WeakReference<NotifyCollectionChangedEventHandler>> handlers = _handlers.GetOrCreateValue(source);
        lock (handlers)
        {
            // Проверяем, есть ли уже этот handler (чтобы избежать дубликатов)
            bool alreadyExists = false;
            foreach (var wr in handlers)
            {
                if (wr.TryGetTarget(out var existingHandler) && existingHandler == handler)
                {
                    alreadyExists = true;
                    break;
                }
            }

            // Если дубликата нет, добавляем новую слабую ссылку
            if (!alreadyExists)
            {
                handlers.Add(new WeakReference<NotifyCollectionChangedEventHandler>(handler));
                // Подписываемся на событие, если это первая подписка
                if (handlers.Count == 1) // Или если это первая добавленная
                {
                    source.CollectionChanged += OnCollectionChanged;
                }
            }
        }
    }


    /// <summary>
    /// Удаляет слабый обработчик события CollectionChanged для источника.
    /// </summary>
    /// <param name="source">Источник события (INotifyCollectionChanged).</param>
    /// <param name="handler">Обработчик события.</param>
    public static void RemoveHandler(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(handler);

        // Получаем список handlers для источника (если он есть)
        if (_handlers.TryGetValue(source, out var handlers))
        {
            lock (handlers)
            {
                handlers.RemoveAll(wr =>
                {
                    if (wr.TryGetTarget(out var existingHandler))
                    {
                        return existingHandler == handler;
                    }
                    else
                    {
                        return true;
                    }
                });

                // Если список пуст, отписываемся от события
                if (handlers.Count == 0)
                {
                    source.CollectionChanged -= OnCollectionChanged;
                    // Можно удалить ключ из _handlers, но не обязательно (ConditionalWeakTable сам управляет)
                }
            }
        }
    }

    /// <summary>
    /// Обработчик события CollectionChanged источника.
    /// Пересылает событие всем живым слабым обработчикам.
    /// </summary>
    private static void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        INotifyCollectionChanged source = (INotifyCollectionChanged)sender!;

        if (!_handlers.TryGetValue(source, out var handlers))
            return;

        List<NotifyCollectionChangedEventHandler> liveHandlers;
        lock (handlers)
        {
            handlers.RemoveAll(wr => !wr.TryGetTarget(out _));

            liveHandlers = [];
            foreach (var wr in handlers)
            {
                if (wr.TryGetTarget(out var handler))
                {
                    liveHandlers.Add(handler);
                }
            }
        }

        foreach (var handler in liveHandlers)
        {
            handler(source, e);
        }
    }
}