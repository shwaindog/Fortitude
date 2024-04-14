namespace FortitudeCommon.Serdes.Binary;

public interface IBufferContext : ISerdeContext
{
    IBuffer? EncodedBuffer { get; set; }
    int LastReadLength { get; set; }
    int LastWriteLength { get; set; }
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
