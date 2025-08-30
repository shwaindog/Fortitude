// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;

public interface IBufferFlushingFormatWriter : IFormatWriter
{
    void FlushBufferToAppender(IBufferedFormatWriter toFlush);
}

public interface ICharBufferFlushingFormatWriter : IBufferFlushingFormatWriter
{
    void FlushBufferToAppender(ICharArrayFlushedBufferedFormatWriter toFlush);
}

public interface IByteBufferFlushingFormatWriter : IBufferFlushingFormatWriter
{
    void FlushBufferToAppender(IEncodedByteArrayBufferedFormatWriter toFlush);
}

public abstract class CharBufferFlushingFormatWriter<T> : FormatWriter<T>, ICharBufferFlushingFormatWriter where T : ICharBufferFlushingFormatWriter
{
    public void FlushBufferToAppender(IBufferedFormatWriter toFlush)
    {
        FlushBufferToAppender((ICharArrayFlushedBufferedFormatWriter)toFlush);
    }

    public abstract void FlushBufferToAppender(ICharArrayFlushedBufferedFormatWriter toFlush);

    protected override CharBufferFlushingFormatWriter<T> Initialize(IMutableFLogFormattingAppender owningAppender, string targetName
      , FormatWriterReceivedHandler<T> onWriteCompleteCallback)
    {
        base.Initialize(owningAppender, targetName, onWriteCompleteCallback);

        return this;
    }
}

public abstract class ByteBufferFlushingFormatWriter<T> : FormatWriter<T>, IByteBufferFlushingFormatWriter where T : IByteBufferFlushingFormatWriter
{
    public void FlushBufferToAppender(IBufferedFormatWriter toFlush)
    {
        FlushBufferToAppender((IEncodedByteArrayBufferedFormatWriter)toFlush);
    }

    public abstract void FlushBufferToAppender(IEncodedByteArrayBufferedFormatWriter toFlush);

    protected override ByteBufferFlushingFormatWriter<T> Initialize(IMutableFLogFormattingAppender owningAppender, string targetName
      , FormatWriterReceivedHandler<T> onWriteCompleteCallback)
    {
        base.Initialize(owningAppender, targetName, onWriteCompleteCallback);

        return this;
    }
}
