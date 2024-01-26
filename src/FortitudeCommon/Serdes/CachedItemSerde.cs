#region

#endregion

namespace FortitudeCommon.Serdes;

public class CachedItemSerde<T> : IDeserializer<T>, ISerializer<T>
{
    public MarshalType MarshalType => MarshalType.Object | MarshalType.Cached;

    public T? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to support reading");
        if ((readContext.MarshalType & MarshalType.Object) > 0)
            throw new ArgumentException("Expected to receive an readContext of with MarshalType of Object");
        if (readContext is ICachedItemContext<T> existingItemContext) return existingItemContext.LastItem;
        throw new ArgumentException("Expected readContext to be a ICachedItemContext");
    }

    public void Serialize(T obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is ICachedItemContext<T> existingItemContext) existingItemContext.LastItem = obj;
        else throw new ArgumentException("Expected readContext to be a ICachedItemContext");
    }
}
