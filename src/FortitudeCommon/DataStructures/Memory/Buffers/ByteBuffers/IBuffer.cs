// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.OSWrapper.Streams;

#endregion

namespace FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;

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

public readonly record struct MessageBufferEntry(nint MessageStartFromBufferOrigin, int MessageSize, long BufferRelativeOffset = 0)
{
    public readonly nint BufferAdjustedMessageStart = (nint)(MessageStartFromBufferOrigin + BufferRelativeOffset);
}

public interface IMessageQueueBuffer : IBuffer
{
    bool EnforceCappedMessageSize { get; }

    long MaximumMessageSize { get; }

    bool HasAnotherMessage { get; }

    MessageBufferEntry PopNextMessage();

    void QueueMessage(MessageBufferEntry messageEntry);
}
