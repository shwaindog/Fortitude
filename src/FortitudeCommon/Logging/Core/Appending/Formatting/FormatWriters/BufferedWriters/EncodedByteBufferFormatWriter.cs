// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;

public interface IEncodedByteArrayBufferedFormatWriter : IBufferedFormatWriter
{
    ByteArrayRange FlushRange();
}

public class EncodedByteBufferFormatWriter : RecyclableObject, IEncodedByteArrayBufferedFormatWriter
{
    private IBufferFlushingFormatWriter bufferFlusher = null!;

    private Encoder fileEncoder = null!;

    private FormatWriterReceivedHandler<EncodedByteBufferFormatWriter> onWriteCompleteCallback = null!;

    private IMutableEncodedByteBufferingAppender owningAppender = null!;

    public EncodedByteBufferFormatWriter Initialize(IMutableEncodedByteBufferingAppender owningEncodingAppender
      , IBufferFlushingFormatWriter bufferFlusherWriter, string targetName, int bufferNum,
        FormatWriterReceivedHandler<EncodedByteBufferFormatWriter> onCompleteCallback)
    {
        owningAppender = owningEncodingAppender;
        TargetName     = targetName;
        BufferNum      = bufferNum;
        Buffer         = (owningAppender.CharBufferSize * 3).SourceRecyclingByteArray();
        fileEncoder    = owningAppender.FileEncoder;
        bufferFlusher  = bufferFlusherWriter;

        onWriteCompleteCallback = onCompleteCallback;

        return this;
    }

    private RecyclingByteArray Buffer { get; set; } = null!;
    public IEncodedByteBufferingAppender OwningAppender => owningAppender;

    IFLogFormattingAppender IFormatWriter.OwningAppender => OwningAppender;

    public bool IsIOSynchronous => false;

    public bool InUse { get; set; }

    public string TargetName { get; private set; } = null!;

    public uint BufferedFormattedLogEntries { get; private set; }

    public bool NotifyStartEntryAppend(IFLogEntry forEntry) => Buffer.RemainingCapacity > forEntry.Message.Length * 4;

    public void Append(string toWrite)
    {
        BufferedChars += toWrite.Length;
        Buffer.Add(fileEncoder, toWrite);
    }

    public void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        var charCount = Math.Clamp(length, 0, toWrite.Length - fromIndex);
        BufferedChars += charCount;
        Buffer.Add(fileEncoder, toWrite, fromIndex, charCount);
    }

    public void Append(ICharSequence toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        var charCount = Math.Clamp(length, 0, toWrite.Length - fromIndex);
        BufferedChars += charCount;
        Buffer.Add(fileEncoder, toWrite, fromIndex, charCount);
    }

    public void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        var charCount = Math.Clamp(length, 0, toWrite.Length - fromIndex);
        BufferedChars += charCount;
        Buffer.Add(fileEncoder, toWrite, fromIndex, charCount);
    }

    public void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        var charCount = Math.Clamp(length, 0, toWrite.Length - fromIndex);
        BufferedChars += charCount;
        Buffer.Add(fileEncoder, toWrite, fromIndex, charCount);
    }

    public void NotifyEntryAppendComplete()
    {
        BufferedFormattedLogEntries++;
    }

    public int BufferNum { get; private set; }

    public int BufferCharCapacity => Buffer.Capacity / 3;

    public int BufferRemainingCharCapacity => BufferRemainingCharCapacity / 3;
    public int BufferedChars { get; private set; }

    public ByteArrayRange FlushRange() => Buffer.AsByteArrayRange;

    public void Flush()
    {
        bufferFlusher.FlushBufferToAppender(this);
    }

    public void Clear()
    {
        BufferedFormattedLogEntries = 0;
        Buffer.Clear();
        BufferedChars = 0;
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
