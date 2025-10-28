// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.DataStructures.MemoryPools.Compression.Lzma;
using FortitudeCommon.DataStructures.MemoryPools.Compression.Lzma.Common;
using FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeCommon.DataStructures.Memory.Compression.Lzma;

[TestClassNotRequired]
public class TestLzmaHelper
{
    private const uint AdditionalSize           = 6 << 20;
    private const uint CompressedAdditionalSize = 1 << 10;
    private const uint MaxLzmaPropSize          = 10;
    private const int  SubBits                  = 8;

    private static uint GetLogSize(uint size)
    {
        for (var i = SubBits; i < 32; i++)
        for (uint j = 0; j < 1 << SubBits; j++)
            if (size <= ((uint)1 << i) + (j << (i - SubBits)))
                return (uint)(i << SubBits) + j;
        return 32 << SubBits;
    }

    private static ulong MyMultDiv64(ulong value, ulong elapsedTime)
    {
        ulong freq   = TimeSpan.TicksPerSecond;
        var   elTime = elapsedTime;
        while (freq > 1000000)
        {
            freq   >>= 1;
            elTime >>= 1;
        }
        if (elTime == 0)
            elTime = 1;
        return value * freq / elTime;
    }

    private static ulong GetCompressRating(uint dictionarySize, ulong elapsedTime, ulong size)
    {
        ulong t                 = GetLogSize(dictionarySize) - (18 << SubBits);
        var   numCommandsForOne = 1060 + ((t * t * 10) >> (2 * SubBits));
        var   numCommands       = (ulong)size * numCommandsForOne;
        return MyMultDiv64(numCommands, elapsedTime);
    }

    private static ulong GetDecompressRating(ulong elapsedTime, ulong outSize, ulong inSize)
    {
        var numCommands = inSize * 220 + outSize * 20;
        return MyMultDiv64(numCommands, elapsedTime);
    }

    private static ulong GetTotalRating(
        uint dictionarySize,
        ulong elapsedTimeEn, ulong sizeEn,
        ulong elapsedTimeDe,
        ulong inSizeDe, ulong outSizeDe) =>
        (GetCompressRating(dictionarySize, elapsedTimeEn, sizeEn) +
         GetDecompressRating(elapsedTimeDe, inSizeDe, outSizeDe)) / 2;

    private static void PrintValue(ulong v)
    {
        var s = v.ToString();
        for (var i = 0; i + s.Length < 6; i++)
            Console.Write(" ");
        Console.Write(s);
    }

    private static void PrintRating(ulong rating)
    {
        PrintValue(rating / 1000000);
        Console.Write(" MIPS");
    }

    private static void PrintResults(
        uint dictionarySize,
        ulong elapsedTime,
        ulong size,
        bool decompressMode, ulong secondSize)
    {
        var speed = MyMultDiv64(size, elapsedTime);
        PrintValue(speed / 1024);
        Console.Write(" KB/s  ");
        ulong rating;
        if (decompressMode)
            rating = GetDecompressRating(elapsedTime, size, secondSize);
        else
            rating = GetCompressRating(dictionarySize, elapsedTime, size);
        PrintRating(rating);
    }

    public static int LzmaBenchmark(int numIterations, uint dictionarySize)
    {
        if (numIterations <= 0)
            return 0;
        if (dictionarySize < 1 << 18)
        {
            Console.WriteLine("\nError: dictionary size for benchmark must be >= 19 (512 KB)");
            return 1;
        }
        Console.Write("\n       Compressing                Decompressing         UC Size     C Size      Ratio\n\n");

        var encoder = new LzmaEncoder();
        var decoder = new LzmaDecoder();

        var bufferSize           = dictionarySize + AdditionalSize;
        var compressedBufferSize = bufferSize / 2 + CompressedAdditionalSize;

        var progressInfo = new TestCodecProgress
        {
            ApprovedStart = dictionarySize
        };
        var encoderParams = new LzmaEncoderParams
        {
            DictionarySize = (int)dictionarySize, CodecProgress = progressInfo
        };

        var rg = new RandomBitBufferGenerator();

        var originalStream     = new byte[4096];
        var decompressedStream = new byte[4096];

        rg.Set(bufferSize);
        rg.Generate();
        var crc = new CRC();
        crc.Init();
        crc.Update(rg.Buffer, 0, rg.BufferSize);

        ulong totalBenchSize      = 0;
        ulong totalEncodeTime     = 0;
        ulong totalDecodeTime     = 0;
        ulong totalCompressedSize = 0;

        // var inStream         = new MemoryStream(rg.Buffer, 0, (int)rg.BufferSize);
        var inStream = MemoryUtils.CreateByteArrayMemoryStream(rg.BufferSize);
        inStream.Write(rg.Buffer, 0, (int)rg.BufferSize);
        inStream.Position = 0;
        // var compressedStream = new MemoryStream((int)kCompressedBufferSize);
        var compressedStream = MemoryUtils.CreateByteArrayMemoryStream(compressedBufferSize);
        var crcOutStream     = new CrcOutStream(rg.BufferSize);
        for (var i = 0; i < numIterations; i++)
        {
            progressInfo.Init();
            inStream.Seek(0, SeekOrigin.Begin);
            compressedStream.Seek(0, SeekOrigin.Begin);
            encoder.Compress(encoderParams, inStream, compressedStream);
            var totalUncompressedSize = (ulong)inStream.Position;
            var sp2                   = DateTime.UtcNow - progressInfo.Time;
            var encodeTime            = (ulong)sp2.Ticks;

            var compressedSize = compressedStream.Position;
            if (progressInfo.InSize == 0)
                throw new Exception("Internal ERROR 1282");

            ulong decodeTime = 0;
            for (var j = 0; j < 2; j++)
            {
                compressedStream.Seek(0, SeekOrigin.Begin);
                crcOutStream.Init();

                var startTime = DateTime.UtcNow;
                decoder.Decompress(compressedStream, crcOutStream);
                var sp = DateTime.UtcNow - startTime;
                decodeTime = (ulong)sp.Ticks;
                if (crcOutStream.GetDigest() != crc.GetDigest())
                    throw new Exception("CRC Error");
            }
            inStream.Seek(0, SeekOrigin.Begin);
            crcOutStream.Seek(0, SeekOrigin.Begin);

            var stillData = true;
            while (stillData)
            {
                var originalRead     = inStream.Read(originalStream, 0, originalStream.Length);
                var decompressedRead = crcOutStream.Read(decompressedStream, 0, originalRead);

                if (originalRead != decompressedRead)
                {
                    Console.Out.WriteLine($"ERROR expected same bytes to be read when inStream.Position {inStream.Position} and when crcOutStream.Position {crcOutStream.Position}!!!");
                    break;
                }
                for (var j = 0; j < originalRead; j++)
                    if (originalStream[j] != decompressedStream[j])
                    {
                        Console.Out.WriteLine($"ERROR uncompressed does not equal original at {j} when inStream.Position {inStream.Position}!!!");
                        originalRead = 0;
                        break;
                    }
                stillData = originalRead > 0;
            }

            var benchSize = bufferSize - (ulong)progressInfo.InSize;
            PrintResults(dictionarySize, encodeTime, benchSize, false, 0);
            Console.Write("     ");
            PrintResults(dictionarySize, decodeTime, bufferSize, true, (ulong)compressedSize);
            Console.Write("   ");
            Console.Write($"{totalUncompressedSize:0,000}");
            Console.Write("   ");
            Console.Write($"{compressedSize:0,000}");
            Console.Write("   ");
            Console.Write($"{(double)compressedSize * 100 / totalUncompressedSize:00.0}%");

            Console.WriteLine();

            totalBenchSize      += benchSize;
            totalEncodeTime     += encodeTime;
            totalDecodeTime     += decodeTime;
            totalCompressedSize += (ulong)compressedSize;
        }
        Console.WriteLine("--------------------------------------------------------------------------------------");
        PrintResults(dictionarySize, totalEncodeTime, totalBenchSize, false, 0);
        Console.Write("     ");
        PrintResults(dictionarySize, totalDecodeTime,
                     bufferSize * (ulong)numIterations, true, totalCompressedSize);
        Console.WriteLine("    Average");
        return 0;
    }


    private class TestCodecProgress : ICodecProgress
    {
        public long     ApprovedStart;
        public long     InSize;
        public DateTime Time;

        public void SetProgress(long inSize, long outSize)
        {
            if (inSize >= ApprovedStart && InSize == 0)
            {
                Time   = DateTime.UtcNow;
                InSize = inSize;
            }
        }

        public void Init()
        {
            InSize = 0;
        }
    }
}

internal class CrcOutStream : ByteArrayMemoryStream
{
    private CRC CRC = new();

    public CrcOutStream(long size) : base(MemoryUtils.CreateUnmanagedByteArray(size), true) { }

    public void Init()
    {
        CRC.Init();
    }

    public uint GetDigest() => CRC.GetDigest();

    public override void WriteByte(byte b)
    {
        CRC.UpdateByte(b);
        base.WriteByte(b);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        CRC.Update(buffer, (uint)offset, (uint)count);
        base.Write(buffer, offset, count);
    }
}

internal class ConsistentRandomUintGenerator
{
    private uint state1;
    private uint state2;

    public ConsistentRandomUintGenerator()
    {
        Init();
    }

    public void Init()
    {
        state1 = 362436069;
        state2 = 521288629;
    }

    public uint GetRnd() =>
        ((state1 = 36969 * (state1 & 0xffff) + (state1 >> 16)) << 16) ^
        (state2 = 18000 * (state2 & 0xffff) + (state2 >> 16));
}

internal class BitRangeRandomUintGenerator
{
    private int numBits;

    private ConsistentRandomUintGenerator RG = new();

    private uint value;

    public void Init()
    {
        value   = 0;
        numBits = 0;
    }

    public uint GetRnd(int numBits)
    {
        uint result;
        if (this.numBits > numBits)
        {
            result       =   value & (((uint)1 << numBits) - 1);
            value        >>= numBits;
            this.numBits -=  numBits;
            return result;
        }
        numBits      -=  this.numBits;
        result       =   value << numBits;
        value        =   RG.GetRnd();
        result       |=  value & (((uint)1 << numBits) - 1);
        value        >>= numBits;
        this.numBits =   32 - numBits;
        return result;
    }
}

internal class RandomBitBufferGenerator
{
    public byte[] Buffer = null!;

    public  uint                        BufferSize;
    private uint                        Pos;
    private uint                        Rep0;
    private BitRangeRandomUintGenerator RG = new();

    public RandomBitBufferGenerator() { }

    public void Set(uint bufferSize)
    {
        Buffer     = new byte[bufferSize];
        Pos        = 0;
        BufferSize = bufferSize;
    }

    private uint GetRndBit() => RG.GetRnd(1);

    private uint GetLogRandBits(int numBits)
    {
        var len = RG.GetRnd(numBits);
        return RG.GetRnd((int)len);
    }

    private uint GetOffset()
    {
        if (GetRndBit() == 0)
            return GetLogRandBits(4);
        return (GetLogRandBits(4) << 10) | RG.GetRnd(10);
    }

    private uint GetLen1() => RG.GetRnd(1 + (int)RG.GetRnd(2));
    private uint GetLen2() => RG.GetRnd(2 + (int)RG.GetRnd(2));

    public void Generate()
    {
        RG.Init();
        Rep0 = 1;
        while (Pos < BufferSize)
            if (GetRndBit() == 0 || Pos < 1)
            {
                Buffer[Pos++] = (byte)RG.GetRnd(8);
            }
            else
            {
                uint len;
                if (RG.GetRnd(3) == 0)
                {
                    len = 1 + GetLen1();
                }
                else
                {
                    do
                    {
                        Rep0 = GetOffset();
                    } while (Rep0 >= Pos);
                    Rep0++;
                    len = 2 + GetLen2();
                }
                for (uint i = 0; i < len && Pos < BufferSize; i++, Pos++)
                    Buffer[Pos] = Buffer[Pos - Rep0];
            }
    }
}
