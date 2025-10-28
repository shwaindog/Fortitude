#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public interface IMessageBufferContext : IBufferContext
{
    new IMessageQueueBuffer EncodedBuffer    { get; set; }

    MessageHeader        MessageHeader    { get; set; }
    DateTime             DeserializerTime { get; set; }
}

public class MessageBufferContext : BufferContext, IMessageBufferContext
{
    public MessageBufferContext(IMessageQueueBuffer buffer) : base(buffer) { }

    public new IMessageQueueBuffer EncodedBuffer
    {
        get => (IMessageQueueBuffer)base.EncodedBuffer!;
        set => base.EncodedBuffer = value;
    }

    public MessageHeader MessageHeader { get; set; }

    public DateTime DeserializerTime { get; set; } = DateTime.MinValue;
}
