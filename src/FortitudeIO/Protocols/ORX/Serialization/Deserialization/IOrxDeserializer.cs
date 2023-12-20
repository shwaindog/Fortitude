#region

using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public interface IOrxDeserializer : IBinaryDeserializer
{
    unsafe object Deserialize(byte* ptr, int length, byte messageVersion);
}
