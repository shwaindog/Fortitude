#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public interface IOrxDeserializerLookup
{
    IRecycler Recycler { get; }
    IOrxDeserializer GetOrCreateDeserializerForVersion(Type variableType, byte version);
    IOrxDeserializer? GetDeserializerForVersion(Type variableType, byte version);
    IOrxDeserializer? GetDeserializerForVersion(uint messageId, byte version);

    void SetDeserializerForVersion(Type variableType, IOrxDeserializer deserializer, byte fromVersion,
        byte toVersion);
}
