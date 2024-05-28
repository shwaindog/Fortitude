// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory;
using Google.Protobuf.WellKnownTypes;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.Lz;

public class InWindow : IInWindow
{
    public uint       BlockSize;                                // Size of Allocated memory block
    public IByteArray BufferBase { get; private set; } = null!; // pointer to buffer with data

    public  uint BufferOffset { get; private set; }
    private uint keepSizeAfter;  // how many BYTEs must be kept buffer after _pos
    private uint keepSizeBefore; // how many BYTEs must be kept in buffer before _pos

    private uint        pointerToLastSafePosition;
    public  uint        Pos { get; private set; } // offset (from _buffer) of curent byte
    private uint        posLimit;                 // offset (from _buffer) of first byte when new block reading must be done
    private ByteStream? stream;
    private bool        streamEndWasReached;            // if (true) then _streamPos shows real end of stream
    public  uint        StreamPos { get; private set; } // offset (from _buffer) of first not read byte from Stream
    private long        streamOffset;

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
        if (streamEndWasReached)
            return;
        while (true)
        {
            var size = (int)(0 - BufferOffset + BlockSize - StreamPos);
            if (size == 0)
                return;
            var numReadBytes = ReadByteRange((int)(BufferOffset + StreamPos), size);
            if (numReadBytes == 0)
            {
                posLimit = StreamPos;
                var pointerToPostion = BufferOffset + posLimit;
                if (pointerToPostion > pointerToLastSafePosition)
                    posLimit = (uint)(pointerToLastSafePosition - BufferOffset);

                streamEndWasReached = true;
                return;
            }

            StreamPos += (uint)numReadBytes;
            if (StreamPos >= Pos + keepSizeAfter)
                posLimit = StreamPos - keepSizeAfter;
        }
    }

    int ReadByteRange(int offset, int size)
    {
        var numRead = 0;
        if (stream!.Value.Stream is IAcceptsByteArrayStream readIntoByteArray)
        {
            numRead = readIntoByteArray.Read(BufferBase, offset, size);
        }
        else if(stream.Value.Stream != null) 
        {
            var cappedSize = Math.Min(size, stream!.Value.Stream.Length - stream!.Value.Stream.Position);
            var readBytes  = new byte[byte.MaxValue];
            var readSoFar  = 0;
            for (; readSoFar < cappedSize; )
            {
                var amountToRead = Math.Min(cappedSize - readSoFar, byte.MaxValue);
                var bytesRead    = stream!.Value.Stream.Read(readBytes, 0, (int)amountToRead);
                for (int j = 0; j < bytesRead; j++)
                {
                    BufferBase[offset + readSoFar + j] = readBytes[j];
                }
                readSoFar += bytesRead;
            }
            numRead = readSoFar;
        }
        else
        {
            var byteArray  = stream!.Value.ByteArray!;
            var cappedSize = Math.Min(BufferBase.Length - offset, Math.Min(size, byteArray.Length - streamOffset));
            for (int i = 0; i < cappedSize; i++)
            {
                BufferBase[offset + i] = byteArray[streamOffset++];
            }
            numRead = (int)cappedSize;
        }
        return numRead;
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
        if (BufferBase == null || this.BlockSize != blockSize)
        {
            Free();
            this.BlockSize = blockSize;
            BufferBase = new ObjectByteArrayWrapper(new byte[this.BlockSize]);
        }

        pointerToLastSafePosition = this.BlockSize - keepSizeAfter;
    }

    public void SetStream(ByteStream stream)
    {
        this.stream = stream;
    }

    public void ReleaseStream()
    {
        stream = null!;
    }

    public void Init()
    {
        BufferOffset = 0;
        Pos = 0;
        StreamPos = 0;
        streamEndWasReached = false;
        ReadBlock();
    }

    public void MovePos()
    {
        Pos++;
        if (Pos > posLimit)
        {
            var pointerToPostion = BufferOffset + Pos;
            if (pointerToPostion > pointerToLastSafePosition)
                MoveBlock();
            ReadBlock();
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
