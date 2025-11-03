using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kritjara.Collections.ObjectModel;

/// <summary>
/// Слабый менеджер событий для PropertyChanged.
/// Позволяет подписываться на события INotifyPropertyChanged без сильных ссылок.
/// Не зависит от WPF.
/// </summary>
public static class PropertyChangedWeakEventManager
{
    // Таблица: источник -> список слабых ссылок на обработчики
    private static readonly ConditionalWeakTable<INotifyPropertyChanged, List<WeakReference<EventHandler<PropertyChangedEventArgs>>>> _handlers = [];

    /// <summary>
    /// Добавляет слабый обработчик события PropertyChanged для источника.
    /// </summary>
    /// <param name="source">Источник события (INotifyPropertyChanged).</param>
    /// <param name="handler">Обработчик события.</param>

    public static void AddHandler(INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(handler);

        // Получаем или создаем список handlers для источника
        List<WeakReference<EventHandler<PropertyChangedEventArgs>>> handlers = _handlers.GetOrCreateValue(source);
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
                handlers.Add(new WeakReference<EventHandler<PropertyChangedEventArgs>>(handler));
                // Подписываемся на событие, если это первая подписка
                if (handlers.Count == 1) // Или если это первая добавленная
                {
                    source.PropertyChanged += OnPropertyChanged;
                }
            }
        }
    }

    /// <summary>
    /// Удаляет слабый обработчик события PropertyChanged для источника.
    /// </summary>
    /// <param name="source">Источник события (INotifyPropertyChanged).</param>
    /// <param name="handler">Обработчик события.</param>
    public static void RemoveHandler(INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> handler)
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
                    source.PropertyChanged -= OnPropertyChanged;
                    // Можно удалить ключ из _handlers, но не обязательно (ConditionalWeakTable сам управляет)
                }
            }
        }
    }

    /// <summary>
    /// Обработчик события PropertyChanged источника.
    /// Пересылает событие всем живым слабым обработчикам.
    /// </summary>
    private static void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        INotifyPropertyChanged source = (INotifyPropertyChanged)sender!;

        if (!_handlers.TryGetValue(source, out var handlers))
            return;

        List<EventHandler<PropertyChangedEventArgs>> liveHandlers;
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