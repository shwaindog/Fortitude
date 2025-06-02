// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.OSWrapper.Streams;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public unsafe interface IBuffer : IStream
{
    byte* ReadBuffer                { get; }
    byte* WriteBuffer               { get; }
    nint  BufferRelativeReadCursor  { get; }
    nint  BufferRelativeWriteCursor { get; }
    long  ReadCursor                { get; set; }
    bool  AllRead                   { get; }
    long  UnreadBytesRemaining      { get; }
    long  WriteCursor               { get; set; }
    long? LimitNextSerialize        { get; set; }
    long? LimitNextDeserialize      { get; set; }
    long  RemainingStorage          { get; }
    void  SetAllRead();
    void  TryHandleRemainingWriteBufferRunningLow();
    bool  HasStorageForBytes(int bytes);
    void  DestroyBuffer();
}

public readonly struct MessageBufferEntry(nint messageStartAt, int messageSize, long bufferRelativeOffset = 0)
{
    public readonly nint MessageStartFromBufferOrigin = messageStartAt;

    public readonly int MessageSize = messageSize;

    public readonly long BufferRelativeOffset = bufferRelativeOffset;

    public nint BufferAdjustedMessageStart => (nint)(messageStartAt + bufferRelativeOffset);
}

public interface IMessageQueueBuffer : IBuffer
{
    bool EnforceCappedMessageSize { get; }

    long MaximumMessageSize { get; }

    bool HasAnotherMessage { get; }

    MessageBufferEntry PopNextMessage();

    void QueueMessage(MessageBufferEntry messageEntry);
}
