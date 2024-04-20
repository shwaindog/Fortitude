#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.Serdes;

[Flags]
public enum MarshalType
{
    None = 0
    , Binary = 1
    , Object = 2
    , Cached = 4
    , Recycled = 8
}

public interface ISerdes<in TIn, out TOut>
{
    MarshalType MarshalType { get; }
    ISerializer<TIn>? Serializer { get; }
    IDeserializer<TOut>? Deserializer { get; }
}

public interface IConvertingSerdes<in TIn, out TOut> : IConverter<TIn, TOut>, ISerdes<TIn, TOut>
{
    ISerdeContext SerdeContext { get; }
}
