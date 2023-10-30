#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public interface IOrxDeserializerLookup
{
    IRecycler OrxRecyclingFactory { get; }
    IOrxDeserializer GetOrCreateDeserializerForVersion(Type variableType, byte version);
    IOrxDeserializer? GetDeserializerForVersion(Type variableType, byte version);

    void SetDeserializerForVersion(Type variableType, IOrxDeserializer deserializer, byte fromVersion,
        byte toVersion);
}
