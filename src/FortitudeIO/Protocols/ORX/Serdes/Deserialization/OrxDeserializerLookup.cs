#region

using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public class OrxDeserializerLookup : IOrxDeserializerLookup
{
    private readonly IDictionary<Type, IDictionary<byte, IOrxDeserializer>> versionLookup =
        new Dictionary<Type, IDictionary<byte, IOrxDeserializer>>();

    public OrxDeserializerLookup(IRecycler orxRecyclingFactory) => Recycler = orxRecyclingFactory;

    public IOrxDeserializer? GetDeserializerForVersion(Type variableType, byte version)
    {
        if (versionLookup.TryGetValue(variableType, out var versionDict))
            if (versionDict.TryGetValue(version, out var orxDeserializer))
                return orxDeserializer;
        return null;
    }

    public IOrxDeserializer GetOrCreateDeserializerForVersion(Type variableType, byte version)
    {
        var alreadyExists = GetDeserializerForVersion(variableType, version);
        if (alreadyExists != null) return alreadyExists;
        var orxDeserializer = (IOrxDeserializer)Activator.CreateInstance(typeof(OrxByteDeserializer<>)
            .MakeGenericType(variableType), this, version)!;
        SetDeserializerForVersion(variableType, orxDeserializer,
            HighestUnsetVersionBelowVersionForType(variableType, version), version);
        return orxDeserializer;
    }

    public void SetDeserializerForVersion(Type variableType, IOrxDeserializer deserializer,
        byte fromVersion, byte toVersion)
    {
        for (var i = fromVersion; i <= toVersion; i++)
        {
            if (!versionLookup.TryGetValue(variableType, out var typeDictionary))
            {
                typeDictionary = new Dictionary<byte, IOrxDeserializer>();
                versionLookup.Add(variableType, typeDictionary);
            }

            if (!typeDictionary.ContainsKey(i)) typeDictionary.Add(i, deserializer);
        }
    }

    public IOrxDeserializer? GetDeserializerForVersion(uint messageId, byte version) => throw new NotImplementedException();

    public IRecycler Recycler { get; }

    private byte HighestUnsetVersionBelowVersionForType(Type variableType, byte upperLimit)
    {
        if (versionLookup.TryGetValue(variableType, out var versionDict))
            return versionDict.Keys.Any(k => k < upperLimit) ?
                (byte)(versionDict.Keys.Where(k => k < upperLimit).Max() + 1) :
                (byte)0;
        return 0;
    }
}
