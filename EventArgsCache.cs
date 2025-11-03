using System.Collections.Specialized;
using System.ComponentModel;

namespace Kritjara.Collections.ObjectModel;

internal static class EventArgsCache
{
    internal static readonly PropertyChangedEventArgs CountPropertyChanged = new PropertyChangedEventArgs("Count");
    internal static readonly PropertyChangedEventArgs IndexerPropertyChanged = new PropertyChangedEventArgs("Item[]");
    internal static readonly PropertyChangedEventArgs SortingStrategyPropertyChanged = new PropertyChangedEventArgs("SortingStrategy");
    internal static readonly PropertyChangedEventArgs FilteringStrategyPropertyChanged = new PropertyChangedEventArgs("FilteringStrategy");
    internal static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
}