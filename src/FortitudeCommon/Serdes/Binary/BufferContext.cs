namespace FortitudeCommon.Serdes.Binary;

public interface IBufferContext : ISerdeContext
{
    IBuffer? EncodedBuffer { get; set; }
    int LastReadLength { get; set; }
    int LastWriteLength { get; set; }
}

public interface IMessageBufferContext : IBufferContext
{
    int MessageVersion { get; set; }
    int MessageSize { get; set; }
}

public class BufferContext : IBufferContext
{
    public BufferContext(IBuffer buffer) => EncodedBuffer = buffer;
    public MarshalType MarshalType => MarshalType.Binary;
    public IBuffer? EncodedBuffer { get; set; }
    public int LastWriteLength { get; set; } = -1;
    public int LastReadLength { get; set; } = -1;
    public ContextDirection Direction { get; set; } = ContextDirection.Both;
}

public class MessageBufferContext : BufferContext, IMessageBufferContext
{
    public MessageBufferContext(IBuffer buffer) : base(buffer) { }
    public int MessageVersion { get; set; }
    public int MessageSize { get; set; }
}
