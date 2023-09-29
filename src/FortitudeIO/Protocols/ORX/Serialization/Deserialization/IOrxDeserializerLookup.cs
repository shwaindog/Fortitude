using System;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization
{
    public interface IOrxDeserializerLookup
    {
        IOrxDeserializer GetOrCreateDeserializerForVersion(Type variableType, byte version);
        IOrxDeserializer GetDeserializerForVersion(Type variableType, byte version);

        void SetDeserializerForVersion(Type variableType, IOrxDeserializer deserializer, byte fromVersion, 
            byte toVersion);

        IRecycler OrxRecyclingFactory { get; }
    }
}
