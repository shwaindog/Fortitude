// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

using FortitudeCommon.OSWrapper.Streams;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Coders;

internal class RangeEncoder
{
    public const uint TopValue = 1 << 24;
    private byte cache;
    private uint cacheSize;

    public ulong Low;
    public uint Range;

    private long StartPosition;

    private IStream byteStream = null!;

    private long streamOffset = 5;

    private long ByteStreamPos => byteStream.Position;

    public void SetStream(IStream stream)
    {
        byteStream = stream;
    }

    public void ReleaseStream()
    {
        byteStream = null!;
    }

    public void Init()
    {
        StartPosition = ByteStreamPos;

        Low = 0;
        Range = 0xFFFFFFFF;
        cacheSize = 1;
        cache = 0;
    }

    public void FlushData()
    {
        for (var i = 0; i < 5; i++)
            ShiftLow();
    }

    public void FlushStream()
    {
        byteStream.Flush();
    }

    public void CloseStream()
    {
        byteStream.Close();
    }

    public void Encode(uint start, uint size, uint total)
    {
        Low += start * (Range /= total);
        Range *= size;
        while (Range < TopValue)
        {
            Range <<= 8;
            ShiftLow();
        }
    }

    public void ShiftLow()
    {
        if ((uint)Low < 0xFF000000 || (uint)(Low >> 32) == 1)
        {
            var temp = cache;
            do
            {
                var writeByte = (byte)(temp + (Low >> 32));
                byteStream!.WriteByte(writeByte);
                temp = 0xFF;
            } while (--cacheSize != 0);

            cache = (byte)((uint)Low >> 24);
        }

        cacheSize++;
        Low = (uint)Low << 8;
    }

    public void EncodeDirectBits(uint v, int numTotalBits)
    {
        for (var i = numTotalBits - 1; i >= 0; i--)
        {
            Range >>= 1;
            if ((v >> i & 1) == 1)
                Low += Range;
            if (Range < TopValue)
            {
                Range <<= 8;
                ShiftLow();
            }
        }
    }

    public void EncodeBit(uint size0, int numTotalBits, uint symbol)
    {
        var newBound = (Range >> numTotalBits) * size0;
        if (symbol == 0)
        {
            Range = newBound;
        }
        else
        {
            Low += newBound;
            Range -= newBound;
        }

        while (Range < TopValue)
        {
            Range <<= 8;
            ShiftLow();
        }
    }

    public long GetProcessedSizeAdd() => cacheSize + streamOffset - StartPosition;
    // (long)Stream.GetProcessedSize();
}

internal class RangeDecoder
{
    public const uint TopValue = 1 << 24;
    public uint Code;

    public long readBytes;
    public uint Range;

    // public Buffer.InBuffer Stream = new Buffer.InBuffer(1 << 16);
    public  IStream byteStream = null!;

    public void Init(IStream stream)
    {
        // Stream.Init(stream);
        byteStream = stream;

        Code = 0;
        Range = 0xFFFFFFFF;
        for (var i = 0; i < 5; i++)
        {
            Code = Code << 8 | ReadByte();
        }
    }

    public byte ReadByte()
    {
        return (byte)byteStream.ReadByte();
    }

    public void ReleaseStream()
    {
        // Stream.ReleaseStream();
        byteStream = null!;
    }

    public void CloseStream()
    {
        byteStream.Close();
    }

    public void Normalize()
    {
        while (Range < TopValue)
        {
            Code = Code << 8 | ReadByte();
            Range <<= 8;
        }
    }

    public void Normalize2()
    {
        if (Range < TopValue)
        {
            Code = Code << 8 | ReadByte();
            Range <<= 8;
        }
    }

    public uint GetThreshold(uint total) => Code / (Range /= total);

    public void Decode(uint start, uint size, uint total)
    {
        Code -= start * Range;
        Range *= size;
        Normalize();
    }

    public uint DecodeDirectBits(int numTotalBits)
    {
        var range = Range;
        var code = Code;
        uint result = 0;
        for (var i = numTotalBits; i > 0; i--)
        {
            range >>= 1;
            /*
            result <<= 1;
            if (code >= range)
            {
                code -= range;
                result |= 1;
            }
            */
            var t = code - range >> 31;
            code -= range & t - 1;
            result = result << 1 | 1 - t;

            if (range < TopValue)
            {
                code = code << 8 | ReadByte();
                range <<= 8;
            }
        }

        Range = range;
        Code = code;
        return result;
    }

    public uint DecodeBit(uint size0, int numTotalBits)
    {
        var newBound = (Range >> numTotalBits) * size0;
        uint symbol;
        if (Code < newBound)
        {
            symbol = 0;
            Range = newBound;
        }
        else
        {
            symbol = 1;
            Code -= newBound;
            Range -= newBound;
        }

        Normalize();
        return symbol;
    }

    // ulong GetProcessedSize() {return Stream.GetProcessedSize(); }
}
