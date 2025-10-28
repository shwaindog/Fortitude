// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;

public interface ICharArrayFlushedBufferedFormatWriter : IBufferedFormatWriter
{
    CharArrayRange FlushRange();
}

public class CharArrayBufferedFormatWriter : FormatWriter<IBufferedFormatWriter>, ICharArrayFlushedBufferedFormatWriter
{
    private IBufferFlushingFormatWriter bufferFlusher = null!;

    public CharArrayBufferedFormatWriter Initialize(IMutableFLogBufferingFormatAppender owningAppender, IBufferFlushingFormatWriter bufferFlusher
      , string targetName, int bufferNum,
        FormatWriterReceivedHandler<IBufferedFormatWriter> onWriteCompleteCallback)
    {
        base.Initialize(owningAppender, targetName, onWriteCompleteCallback);

        IsIOSynchronous = false;

        this.bufferFlusher = bufferFlusher;
        BufferNum          = bufferNum;
        if (BufferCharCapacity < owningAppender.CharBufferSize)
        {
            Buffer?.DecrementRefCount();
            Buffer = owningAppender.CharBufferSize.SourceRecyclingCharArray();
        }

        return this;
    }

    private RecyclingCharArray? Buffer { get; set; }

    public int BufferNum { get; private set; }

    public int BufferCharCapacity => Buffer?.Capacity ?? 0;

    public int BufferedChars => Buffer?.Count ?? 0;

    public int BufferRemainingCharCapacity => Buffer?.RemainingCapacity ?? 0;

    public uint BufferedFormattedLogEntries { get; private set; }

    public override void Append(string toWrite)
    {
        Buffer!.Add(toWrite);
    }

    public override void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public override void Append(ICharSequence toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public override void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public override void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public override void NotifyEntryAppendComplete()
    {
        BufferedFormattedLogEntries++;
    }

    public override void Dispose()
    {
        if (InUse)
        {
            InUse = false;
            OnWriteCompleteCallback(this);
        }
    }

    public CharArrayRange FlushRange() => Buffer!.AsCharArrayRange;

    public void Flush()
    {
        bufferFlusher.FlushBufferToAppender(this);
    }

    public void Clear()
    {
        BufferedFormattedLogEntries = 0;
        Buffer?.Clear();
    }

    public override void StateReset()
    {
        bufferFlusher = null!;
        BufferNum     = 0;

        base.StateReset();
    }
}
