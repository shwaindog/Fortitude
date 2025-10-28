// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.Serdes;

public class ReusableObjectItemSerde<T> : IDeserializer<T>, ISerializer<T> where T : class, IReusableObject, new()
{
    public MarshalType MarshalType => MarshalType.Object | MarshalType.Recycled;

    public T? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0) throw new ArgumentException("Expected readContext to support reading");
        if ((readContext.MarshalType & MarshalType.Object) > 0)
            throw new ArgumentException("Expected to receive an readContext of with MarshalType of Object");
        if ((readContext.MarshalType & MarshalType.Recycled) > 0)
            throw new ArgumentException("Expected to receive an readContext of with MarshalType of Recycled");
        if (readContext is IPooledRecyclableContext<T> existingItemContext)
            return existingItemContext.Recycler?.Borrow<T>()
                                      .CopyFrom(existingItemContext.LastItem!, CopyMergeFlags.Default) as T ??
                   existingItemContext.LastItem?.Clone() as T;
        throw new ArgumentException("Expected readContext to be a IPooledRecyclableContext");
    }

    public void Serialize(T obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0) throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IPooledRecyclableContext<T> existingItemContext)
            existingItemContext.LastItem = obj;
        else
            throw new ArgumentException("Expected readContext to be a IPooledRecyclableContext");
    }
}
