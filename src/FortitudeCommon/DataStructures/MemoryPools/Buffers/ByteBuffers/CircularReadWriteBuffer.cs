// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using System.Runtime.InteropServices;

#endregion

namespace FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;

public unsafe class CircularReadWriteBuffer : IMessageQueueBuffer
{
    public const int LargeObjectHeapAllocationSize = 85_000;

    private GCHandle? handle;

    private long bufferShifted;
    private int  pinCount;
    private long readCursor = 0;
    private bool shouldUnpin = true;
    private long writeCursor = 0;

    private List<MessageBufferEntry> messageQueue = new();

    public CircularReadWriteBuffer(byte[] buffer)
    {
        Buffer = buffer;
        if (buffer.Length >= LargeObjectHeapAllocationSize) shouldUnpin = false;
    }

    public byte[] Buffer { get; private set; }

    public bool CanRead  => ReadCursor < WriteCursor;
    public bool CanSeek  => true;
    public bool CanWrite => BufferRelativeWriteCursor < Length;
    public long Position
    {
        get => WriteCursor = ReadCursor;
        set
        {
            if (value < bufferShifted || value > bufferShifted + Length)
            {
                WriteCursor = ReadCursor = bufferShifted = value;
                return;
            }
            WriteCursor = ReadCursor = value;
        }
    }

    public byte* ReadBuffer
    {
        get
        {
            if (pinCount < 0) pinCount                      = 0;
            if (handle is not { IsAllocated: true }) handle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
            if (shouldUnpin) pinCount++;
            return (byte*)handle.Value.AddrOfPinnedObject().ToPointer();
        }
    }

    public byte* WriteBuffer
    {
        get
        {
            if (pinCount < 0) pinCount                      = 0;
            if (handle is not { IsAllocated: true }) handle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
            if (shouldUnpin) pinCount++;

            return (byte*)handle.Value.AddrOfPinnedObject().ToPointer();
        }
    }

    public void Dispose()
    {
        if (shouldUnpin && --pinCount <= 0 && handle is { IsAllocated: true })
        {
            handle.Value.Free();
            handle = null;
        }
    }

    public void Close()
    {
        Dispose();
    }

    public void DestroyBuffer() { }

    public nint BufferRelativeReadCursor  => (nint)(ReadCursor - bufferShifted);
    public nint BufferRelativeWriteCursor => (nint)(WriteCursor - bufferShifted);

    public long ReadCursor
    {
        get => readCursor;
        set
        {
            LimitNextDeserialize = null;
            readCursor           = value;
        }
    }

    public long WriteCursor
    {
        get => writeCursor;
        set
        {
            LimitNextSerialize = null;
            writeCursor        = value;
        }
    }

    public bool AllRead => ReadCursor == WriteCursor;

    public long UnreadBytesRemaining => Math.Min(LimitNextDeserialize ?? WriteCursor - ReadCursor, WriteCursor - ReadCursor);

    public long? LimitNextSerialize { get; set; }

    public long? LimitNextDeserialize { get; set; }

    public void Flush() { }

    public int Read(byte[] buffer, int offset, int count)
    {
        var remainingBytes = BufferRelativeWriteCursor - BufferRelativeReadCursor;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        for (var i = offset; i < offset + cappedSize; i++) buffer[i] = Buffer[(int)Position++];
        return (int)cappedSize;
    }

    public int Read(Span<byte> bufferInto)
    {
        var remainingBytes = BufferRelativeWriteCursor - BufferRelativeReadCursor;
        var cappedSize     = Math.Min(bufferInto.Length, remainingBytes);

        for (var i = 0; i < cappedSize; i++) bufferInto[i] = Buffer[(int)Position++];
        return (int)cappedSize;
    }

    public int ReadByte()
    {
        var result = ReadCursor >= WriteCursor ? -1 : Buffer[BufferRelativeReadCursor];
        ReadCursor++;
        return result;
    }

    public long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                if (offset > Buffer.Length || offset < 0) throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = (int)offset;
                break;
            case SeekOrigin.End:
                if (offset > Buffer.Length || offset < 0) throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = Buffer.Length - (int)offset;
                break;
            default:
                var proposedCursor = Position + (int)offset;
                if (proposedCursor > Buffer.Length || proposedCursor < 0) throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = proposedCursor;
                break;
        }
        return Position;
    }

    public void SetLength(long newSize)
    {
        if (newSize == Length) return;
        var replacementArray = new byte[newSize];
        Array.Copy(Buffer, replacementArray, newSize);
        Buffer = replacementArray;
    }

    public void Write(byte[] buffer, int offset, int count)
    {
        var remainingBytes = Buffer.Length - BufferRelativeWriteCursor;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        for (var i = offset; i < offset + cappedSize; i++) Buffer[(int)WriteCursor++] = buffer[i];
    }

    public void Write(ReadOnlySpan<byte> bufferSource)
    {
        var remainingBytes = Buffer.Length - BufferRelativeWriteCursor;
        var cappedSize     = Math.Min(bufferSource.Length, remainingBytes);

        for (var i = 0; i < cappedSize; i++) Buffer[(int)WriteCursor++] = bufferSource[i];
    }

    public void WriteByte(byte value)
    {
        if (BufferRelativeWriteCursor >= Length) TryHandleRemainingWriteBufferRunningLow();
        if (BufferRelativeWriteCursor < Length) Buffer[(int)WriteCursor++] = value;
    }

    public void SetAllRead()
    {
        if (messageQueue.Any())
        {
            Debugger.Break();
        }
        bufferShifted = ReadCursor = WriteCursor;
    }

    public void TryHandleRemainingWriteBufferRunningLow()
    {
        if (BufferRelativeReadCursor > 0 && BufferRelativeWriteCursor > 0)
        {
            var moveBackBy = (int)BufferRelativeReadCursor;
            var length     = BufferRelativeWriteCursor - BufferRelativeReadCursor;
            System.Buffer.BlockCopy(Buffer, (int)BufferRelativeReadCursor, Buffer, 0,
                                    (int)length);
            for (var i = 0; i < messageQueue.Count; i++)
            {
                var messageBufferEntry = messageQueue[i];
                messageQueue[i] = new MessageBufferEntry(messageBufferEntry.MessageStartFromBufferOrigin - moveBackBy, messageBufferEntry.MessageSize);
            }
            bufferShifted = ReadCursor;
        }
    }

    public bool HasStorageForBytes(int bytes) => BufferRelativeWriteCursor + bytes <= Buffer.Length;

    public long RemainingStorage =>
        Math.Min(LimitNextSerialize ?? Buffer.Length - BufferRelativeWriteCursor, Buffer.Length - BufferRelativeWriteCursor);

    public long Length => Buffer.Length;

    public bool CanWriteSize(int size) => RemainingStorage > size;

    public bool EnforceCappedMessageSize { get; set; }

    public long MaximumMessageSize { get; set; }

    public bool HasAnotherMessage => messageQueue.Count > 0;

    public MessageBufferEntry PopNextMessage()
    {
        var next = messageQueue[0];
        messageQueue.RemoveAt(0);
        return next;
    }

    public void QueueMessage(MessageBufferEntry messageEntry)
    {
        messageQueue.Add(messageEntry);
    }

    ~CircularReadWriteBuffer()
    {
        if (handle is not { IsAllocated: true }) return;
        handle.Value.Free();
        handle = null;
    }
}
