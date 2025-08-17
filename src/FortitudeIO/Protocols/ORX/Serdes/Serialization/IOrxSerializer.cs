#region

using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Serialization;

public interface IOrxSerializer
{
    int Serialize(object message, IBuffer buffer, int headerOffset);
    unsafe int Serialize(object message, byte* ptr, byte* endPtr);
}
