// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.Lz;

public class DirectBufferInWindow : IInWindow
{
    public IByteArray BufferBase { get; private set; }   // pointer to buffer with data

    public  uint BufferOffset { get; private set; }
    private uint keepSizeAfter;  // how many BYTEs must be kept buffer after _pos
    private uint keepSizeBefore; // how many BYTEs must be kept in buffer before _pos

    private long    pointerToLastSafePosition;
    public  uint    Pos { get; private set; } // offset (from _buffer) of curent byte
    private uint    posLimit;                 // offset (from _buffer) of first byte when new block reading must be done
    private ByteStream? ignored;
    private bool    streamEndWasReached;            // if (true) then _streamPos shows real end of stream
    public  uint    StreamPos { get; private set; } // offset (from _buffer) of first not read byte from Stream

    public DirectBufferInWindow(IByteArray directInput)
    {
        BufferBase = directInput;
    }


    public void MoveBlock()
    {
        var offset = (uint)BufferOffset + Pos - keepSizeBefore;
        // we need one additional byte, since MovePos moves on 1 byte.
        if (offset > 0)
            offset--;

        var numBytes = (uint)BufferOffset + StreamPos - offset;

        // check negative offset ????
        for (uint i = 0; i < numBytes; i++)
            BufferBase[i] = BufferBase[offset + i];
        BufferOffset -= offset;
    }

    public virtual void ReadBlock()
    {
    }


    private void Free()
    {
        BufferBase = null!;
    }

    public void Create(uint keepSizeBefore, uint keepSizeAfter, uint keepSizeReserv)
    {
        this.keepSizeBefore    = keepSizeBefore;
        this.keepSizeAfter = keepSizeAfter;
        var blockSize = keepSizeBefore + keepSizeAfter + keepSizeReserv;
        if (BufferBase == null)
        {
            throw new InvalidOperationException("Expected DirectBufferInWindow to be set on construction");
        }

        pointerToLastSafePosition = BufferBase.Length;
    }

    public void SetStream(ByteStream stream)
    {
        this.ignored = stream;
    }

    public void ReleaseStream()
    {
        ignored = null!;
    }

    public void Init()
    {
        BufferOffset = 0;
        Pos = 0;
        StreamPos = 0;
        streamEndWasReached = false;
    }

    public void MovePos()
    {
        Pos++;
        if (Pos > posLimit)
        {
            var pointerToPostion = BufferOffset + Pos;
            if (pointerToPostion > pointerToLastSafePosition)
                MoveBlock();
        }
    }

    public byte GetIndexByte(int index) => BufferBase[BufferOffset + Pos + index];

    // index + limit have not to exceed _keepSizeAfter;
    public uint GetMatchLen(int index, uint distance, uint limit)
    {
        if (streamEndWasReached)
            if (Pos + index + limit > StreamPos)
                limit = StreamPos - (uint)(Pos + index);
        distance++;
        // Byte *pby = _buffer + (size_t)_pos + index;
        var pby = BufferOffset + Pos + (uint)index;

        uint i;
        for (i = 0; i < limit && BufferBase[pby + i] == BufferBase[pby + i - distance]; i++) ;
        return i;
    }

    public uint GetNumAvailableBytes() => StreamPos - Pos;

    public void ReduceOffsets(int subValue)
    {
        BufferOffset += (uint)subValue;
        posLimit -= (uint)subValue;
        Pos -= (uint)subValue;
        StreamPos -= (uint)subValue;
    }
}
