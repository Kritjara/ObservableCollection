namespace Kritjara.Collections.ObjectModel;

/// <summary>
/// 
/// </summary>
public static class ObservableFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static ObservableCollection<T> CreateObservableCollection<T>(ReadOnlySpan<T> items)
    {
        return ObservableCollection<T>.CreateObservableCollection(ref items);
    }
}