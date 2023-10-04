namespace FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;

public interface IOrxRecyclingDisassemblerLookup
{
    IOrxRecyclingDisassembler? GetOrCreateRecyclingDisassembler(Type variableType);
    IOrxRecyclingDisassembler? GetRecyclingDisassembler(Type variableType);
    void SetRecyclingDisassembler(Type variableType, IOrxRecyclingDisassembler deserializer);
}
