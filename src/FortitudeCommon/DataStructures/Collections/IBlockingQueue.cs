#region

using System.Diagnostics.CodeAnalysis;

#endregion

namespace FortitudeCommon.DataStructures.Collections;

public interface IBlockingQueue<T>
{
    void Add(T item);
    bool TryAdd(T item);
    T Take();
    bool TryTake([MaybeNullWhen(false)] out T item);
}
