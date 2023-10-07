namespace FortitudeCommon.Extensions;

public static class DiffNewUpdatedDeleted
{
    public static DiffResults<T> Diff<T, U>(this IEnumerable<T> originalSet,
        IEnumerable<T>? newSet,
        Func<T, U> uniqueId) where U : notnull
    {
        var originalSetDict = originalSet?.ToDictionary(uniqueId) ?? new Dictionary<U, T>();
        var newItems = new List<T>();
        var updatedItems = new List<T>();
        var checkNew = newSet ?? new List<T>();
        foreach (var checkItem in checkNew)
        {
            var checkItemKey = uniqueId(checkItem);
            if (!originalSetDict.ContainsKey(checkItemKey))
            {
                newItems.Add(checkItem);
                originalSetDict.Remove(checkItemKey);
            }
            else
            {
                var originalItem = originalSetDict[checkItemKey];
                originalSetDict.Remove(checkItemKey);
                if (!Equals(originalItem, checkItem)) updatedItems.Add(checkItem);
            }
        }

        var deletedItems = originalSetDict.Values.ToList();
        return new DiffResults<T>(newItems, updatedItems, deletedItems);
    }
}

public class DiffResults<T>
{
    public DiffResults(IEnumerable<T> newItems, IEnumerable<T> updatedItems, IEnumerable<T> deletedItems)
    {
        NewItems = newItems;
        UpdatedItems = updatedItems;
        DeletedItems = deletedItems;
    }


    public IEnumerable<T> NewItems { get; private set; }
    public IEnumerable<T> UpdatedItems { get; private set; }
    public IEnumerable<T> DeletedItems { get; private set; }
}
