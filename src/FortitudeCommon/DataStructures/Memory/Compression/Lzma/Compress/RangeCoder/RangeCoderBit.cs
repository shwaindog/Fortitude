// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.RangeCoder;

internal struct BitEncoder
{
    public const int NumBitModelTotalBits = 11;
    public const uint BitModelTotal = 1 << NumBitModelTotalBits;
    private const int NumMoveBits = 5;
    private const int NumMoveReducingBits = 2;
    public const int NumBitPriceShiftBits = 6;

    private uint Prob;

    public void Init()
    {
        Prob = BitModelTotal >> 1;
    }

    public void UpdateModel(uint symbol)
    {
        if (symbol == 0)
            Prob += (BitModelTotal - Prob) >> NumMoveBits;
        else
            Prob -= Prob >> NumMoveBits;
    }

    public void Encode(RangeEncoder rangeEncoder, uint symbol)
    {
        // encoder.EncodeBit(Prob, kNumBitModelTotalBits, symbol);
        // UpdateModel(symbol);
        var newBound = (rangeEncoder.Range >> NumBitModelTotalBits) * Prob;
        if (symbol == 0)
        {
            rangeEncoder.Range = newBound;
            Prob += (BitModelTotal - Prob) >> NumMoveBits;
        }
        else
        {
            rangeEncoder.Low += newBound;
            rangeEncoder.Range -= newBound;
            Prob -= Prob >> NumMoveBits;
        }

        if (rangeEncoder.Range < RangeEncoder.TopValue)
        {
            rangeEncoder.Range <<= 8;
            rangeEncoder.ShiftLow();
        }
    }

    private static uint[] ProbPrices = new uint[BitModelTotal >> NumMoveReducingBits];

    static BitEncoder()
    {
        const int NumBits = NumBitModelTotalBits - NumMoveReducingBits;
        for (var i = NumBits - 1; i >= 0; i--)
        {
            var start = (uint)1 << (NumBits - i - 1);
            var end = (uint)1 << (NumBits - i);
            for (var j = start; j < end; j++)
                ProbPrices[j] = ((uint)i << NumBitPriceShiftBits) +
                                (((end - j) << NumBitPriceShiftBits) >> (NumBits - i - 1));
        }
    }

    public uint GetPrice(uint symbol) => ProbPrices[(((Prob - symbol) ^ -(int)symbol) & (BitModelTotal - 1)) >> NumMoveReducingBits];
    public uint GetPrice0() => ProbPrices[Prob >> NumMoveReducingBits];
    public uint GetPrice1() => ProbPrices[(BitModelTotal - Prob) >> NumMoveReducingBits];
}

internal struct BitDecoder
{
    public const int NumBitModelTotalBits = 11;
    public const uint BitModelTotal = 1 << NumBitModelTotalBits;
    private const int NumMoveBits = 5;

    private uint Prob;

    public void UpdateModel(int numMoveBits, uint symbol)
    {
        if (symbol == 0)
            Prob += (BitModelTotal - Prob) >> numMoveBits;
        else
            Prob -= Prob >> numMoveBits;
    }

    public void Init()
    {
        Prob = BitModelTotal >> 1;
    }

    public uint Decode(RangeDecoder rangeDecoder)
    {
        var newBound = (uint)(rangeDecoder.Range >> NumBitModelTotalBits) * (uint)Prob;
        if (rangeDecoder.Code < newBound)
        {
            rangeDecoder.Range = newBound;
            Prob += (BitModelTotal - Prob) >> NumMoveBits;
            if (rangeDecoder.Range < RangeDecoder.TopValue)
            {
                rangeDecoder.Code  =   (rangeDecoder.Code << 8) | (byte)rangeDecoder.ReadByte();
                rangeDecoder.Range <<= 8;
            }

            return 0;
        }
        else
        {
            rangeDecoder.Range -= newBound;
            rangeDecoder.Code -= newBound;
            Prob -= Prob >> NumMoveBits;
            if (rangeDecoder.Range < RangeDecoder.TopValue)
            {
                rangeDecoder.Code  =   (rangeDecoder.Code << 8) | (byte)rangeDecoder.ReadByte();
                rangeDecoder.Range <<= 8;
            }

            return 1;
        }
    }
}
