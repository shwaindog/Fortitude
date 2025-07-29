using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;


public interface IBufferedFormatWriter : IFormatWriter
{
    int BufferNum { get; }

    int BufferCharCapacity { get; }

    int BufferRemainingCapacity { get; }

    int Buffered { get; }

    CharArrayRange FlushRange();

    void EnsureCapacity(int changedCapacity);
}

internal class BufferedFormatWriter : FormatWriter<IBufferedFormatWriter>, IBufferedFormatWriter
{
    private RecyclingCharArray Buffer { get; set; }

    public BufferedFormatWriter(IMutableFLogBufferingFormatAppender owningAppender, int bufferNum,
        FormatWriterReceivedHandler<IBufferedFormatWriter> onWriteCompleteCallback) 
        : base(owningAppender, onWriteCompleteCallback)
    {
        BufferNum = bufferNum;
        Buffer    = owningAppender.CharBufferSize.SourceRecyclingCharArray();
    }

    public int BufferNum { get; }

    public override bool IsIOSynchronous => false;

    public int BufferCharCapacity => Buffer.Capacity;

    public int Buffered           => Buffer.Count;

    public int BufferRemainingCapacity => Buffer.RemainingCapacity;
    
    public override void Append(string toWrite)
    {
        Buffer.Add(toWrite);
    }

    public override void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer.Add(toWrite, fromIndex, length);
    }

    public override void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer.Add(toWrite, fromIndex, length);
    }

    public override void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer.Add(toWrite, fromIndex, length);
    }

    public override void Dispose()
    {
        InUse = false;
        OnWriteCompleteCallback(this);
    }

    public void EnsureCapacity(int changedCapacity)
    {
        Buffer = Buffer.EnsureCapacity(changedCapacity);
    }

    public CharArrayRange FlushRange() => Buffer.AsCharArrayRange;
}