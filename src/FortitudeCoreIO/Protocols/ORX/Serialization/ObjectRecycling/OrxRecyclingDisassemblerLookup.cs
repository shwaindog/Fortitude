namespace FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;

public class OrxRecyclingDisassemblerLookup : IOrxRecyclingDisassemblerLookup
{
    private readonly IDictionary<Type, IOrxRecyclingDisassembler> typeLookup =
        new Dictionary<Type, IOrxRecyclingDisassembler>();

    public IOrxRecyclingDisassembler GetOrCreateRecyclingDisassembler(Type variableType)
    {
        var alreadyExists = GetRecyclingDisassembler(variableType);
        if (alreadyExists != null) return alreadyExists;
        var orxDeserializer = (IOrxRecyclingDisassembler)Activator.CreateInstance(typeof(OrxRecyclingDisassembler<>)
            .MakeGenericType(variableType), this)!;
        SetRecyclingDisassembler(variableType, orxDeserializer);
        return orxDeserializer;
    }

    public IOrxRecyclingDisassembler? GetRecyclingDisassembler(Type variableType)
    {
        if (typeLookup.TryGetValue(variableType, out var disassembler)) return disassembler;
        return null;
    }

    public void SetRecyclingDisassembler(Type variableType, IOrxRecyclingDisassembler deserializer)
    {
        if (!typeLookup.ContainsKey(variableType)) typeLookup.Add(variableType, deserializer);
    }
}
