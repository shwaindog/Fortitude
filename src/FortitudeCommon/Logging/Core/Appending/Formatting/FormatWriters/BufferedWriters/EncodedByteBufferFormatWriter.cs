using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;

public interface IEncodedByteArrayBufferedFormatWriter : IBufferedFormatWriter
{
    ByteArrayRange FlushRange();
}

public class EncodedByteBufferFormatWriter : RecyclableObject, IEncodedByteArrayBufferedFormatWriter
{
    private IMutableEncodedByteBufferingAppender owningAppender = null!;

    private FormatWriterReceivedHandler<EncodedByteBufferFormatWriter> onWriteCompleteCallback = null!;

    private int encodedCharCount;
    private Encoder fileEncoder = null!;

    private IBufferFlushingFormatWriter bufferFlusher = null!;
    private RecyclingByteArray Buffer { get; set; } = null!; 

    public EncodedByteBufferFormatWriter Initialize(IMutableEncodedByteBufferingAppender owningEncodingAppender
      , IBufferFlushingFormatWriter bufferFlusherWriter, string targetName, int bufferNum,
        FormatWriterReceivedHandler<EncodedByteBufferFormatWriter> onCompleteCallback)
    {
        TargetName     = targetName;
        BufferNum      = bufferNum;
        Buffer         = (owningAppender.CharBufferSize * 3).SourceRecyclingByteArray();
        fileEncoder    = owningAppender.FileEncoder;
        owningAppender = owningEncodingAppender;
        bufferFlusher  = bufferFlusherWriter;
        
        onWriteCompleteCallback = onCompleteCallback;

        return this;
    }

    IFLogFormattingAppender IFormatWriter.OwningAppender => OwningAppender;
    public IEncodedByteBufferingAppender OwningAppender => owningAppender;

    public bool IsIOSynchronous => false;

    public bool InUse { get; set; }

    public string TargetName { get; private set; } = null!;

    public bool NotifyStartEntryAppend(IFLogEntry forEntry)
    {
        return Buffer.RemainingCapacity > forEntry.Message.Length * 4;
    }

    public void Append(string toWrite)
    {
        encodedCharCount += toWrite.Length;
        Buffer.Add(fileEncoder, toWrite);
    }

    public void Append(StringBuilder toWrite, int fromIndex = 0, int length = Int32.MaxValue)
    {
        var charCount = Math.Clamp(length, 0, toWrite.Length - fromIndex);
        encodedCharCount += charCount;
        Buffer.Add(fileEncoder, toWrite, fromIndex, charCount);
    }

    public void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = Int32.MaxValue)
    {
        var charCount = Math.Clamp(length, 0, toWrite.Length - fromIndex);
        encodedCharCount += charCount;
        Buffer.Add(fileEncoder, toWrite, fromIndex, charCount);
    }

    public void Append(char[] toWrite, int fromIndex = 0, int length = Int32.MaxValue)
    {
        var charCount = Math.Clamp(length, 0, toWrite.Length - fromIndex);
        encodedCharCount += charCount;
        Buffer.Add(fileEncoder, toWrite, fromIndex, charCount);
    }

    public void NotifyEntryAppendComplete() { }

    public int BufferNum { get; private set; }

    public int BufferCharCapacity => Buffer.Capacity / 3;

    public int BufferRemainingCharCapacity => BufferRemainingCharCapacity / 3;
    public int BufferedChars => encodedCharCount;

    public ByteArrayRange FlushRange() => Buffer.AsByteArrayRange;

    public void Flush()
    {
        bufferFlusher.FlushBufferToAppender(this);
    }

    public void Clear()
    {
        Buffer.Clear();
        encodedCharCount = 0;
    }

    public void Dispose()
    {
        if (InUse)
        {
            InUse = false;
            onWriteCompleteCallback(this);
        }
    }
}
