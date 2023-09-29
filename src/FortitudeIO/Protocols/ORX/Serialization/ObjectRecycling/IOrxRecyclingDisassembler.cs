using FortitudeCommon.DataStructures.Memory;

namespace FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling
{
    public interface IOrxRecyclingDisassembler
    {
        void ReturnReferencePropertiesToPool(object toBeRecycled, IRecycler recyclator);
    }
}