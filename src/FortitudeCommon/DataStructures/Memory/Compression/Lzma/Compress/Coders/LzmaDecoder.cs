// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

#region

using FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.Lz;
using FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.RangeCoder;

#endregion

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.Coders;

public class Decoder : ICoder, ISetDecoderProperties // ,System.IO.Stream
{
    private bool solid = false;

    private uint dictionarySize;
    private uint dictionarySizeCheck;

    private BitDecoder[] isMatchDecoders = new BitDecoder[Base.NumStates << Base.NumPosStatesBitsMax];
    private BitDecoder[] isRep0LongDecoders = new BitDecoder[Base.NumStates << Base.NumPosStatesBitsMax];
    private BitDecoder[] isRepDecoders = new BitDecoder[Base.NumStates];
    private BitDecoder[] isRepG0Decoders = new BitDecoder[Base.NumStates];
    private BitDecoder[] isRepG1Decoders = new BitDecoder[Base.NumStates];
    private BitDecoder[] isRepG2Decoders = new BitDecoder[Base.NumStates];

    private LenDecoder lenDecoder = new();

    private LiteralDecoder literalDecoder = new();

    private OutWindow outWindow = new();

    private BitTreeDecoder posAlignDecoder = new(Base.NumAlignBits);
    private BitDecoder[] posDecoders = new BitDecoder[Base.NumFullDistances - Base.EndPosModelIndex];

    private BitTreeDecoder[] posSlotDecoder = new BitTreeDecoder[Base.NumLenToPosStates];

    private uint posStateMask;
    private FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.RangeCoder.RangeDecoder rangeDecoder = new();
    private LenDecoder repLenDecoder = new();

    public Decoder()
    {
        dictionarySize = 0xFFFFFFFF;
        for (var i = 0; i < Base.NumLenToPosStates; i++)
            posSlotDecoder[i] = new BitTreeDecoder(Base.NumPosSlotBits);
    }

    private void SetDictionarySize(uint dictionarySize)
    {
        if (this.dictionarySize != dictionarySize)
        {
            this.dictionarySize = dictionarySize;
            dictionarySizeCheck = Math.Max(this.dictionarySize, 1);
            var blockSize = Math.Max(dictionarySizeCheck, 1 << 12);
            outWindow.Create(blockSize);
        }
    }

    private void SetLiteralProperties(int lp, int lc)
    {
        if (lp > 8)
            throw new InvalidParamException();
        if (lc > 8)
            throw new InvalidParamException();
        literalDecoder.Create(lp, lc);
    }

    private void SetPosBitsProperties(int pb)
    {
        if (pb > Base.NumPosStatesBitsMax)
            throw new InvalidParamException();
        var numPosStates = (uint)1 << pb;
        lenDecoder.Create(numPosStates);
        repLenDecoder.Create(numPosStates);
        posStateMask = numPosStates - 1;
    }

    private void Init(Stream inStream, Stream outStream)
    {
        rangeDecoder.Init(inStream);
        outWindow.Init(outStream, solid);

        uint i;
        for (i = 0; i < Base.NumStates; i++)
        {
            for (uint j = 0; j <= posStateMask; j++)
            {
                var index = (i << Base.NumPosStatesBitsMax) + j;
                isMatchDecoders[index].Init();
                isRep0LongDecoders[index].Init();
            }

            isRepDecoders[i].Init();
            isRepG0Decoders[i].Init();
            isRepG1Decoders[i].Init();
            isRepG2Decoders[i].Init();
        }

        literalDecoder.Init();
        for (i = 0; i < Base.NumLenToPosStates; i++)
            posSlotDecoder[i].Init();
        // m_PosSpecDecoder.Init();
        for (i = 0; i < Base.NumFullDistances - Base.EndPosModelIndex; i++)
            posDecoders[i].Init();

        lenDecoder.Init();
        repLenDecoder.Init();
        posAlignDecoder.Init();
    }

    public void Code(Stream inStream, Stream outStream,
        long inSize, long outSize, ICodeProgress progress)
    {
        Init(inStream, outStream);

        var state = new Base.State();
        state.Init();
        uint rep0 = 0, rep1 = 0, rep2 = 0, rep3 = 0;

        ulong nowPos64 = 0;
        var outSize64 = (ulong)outSize;
        if (nowPos64 < outSize64)
        {
            if (isMatchDecoders[state.Index << Base.NumPosStatesBitsMax].Decode(rangeDecoder) != 0)
                throw new DataErrorException();
            state.UpdateChar();
            var b = literalDecoder.DecodeNormal(rangeDecoder, 0, 0);
            outWindow.PutByte(b);
            nowPos64++;
        }

        while (nowPos64 < outSize64)
            // UInt64 next = Math.Min(nowPos64 + (1 << 18), outSize64);
            // while(nowPos64 < next)
        {
            var posState = (uint)nowPos64 & posStateMask;
            if (isMatchDecoders[(state.Index << Base.NumPosStatesBitsMax) + posState].Decode(rangeDecoder) == 0)
            {
                byte b;
                var prevByte = outWindow.GetByte(0);
                if (!state.IsCharState())
                    b = literalDecoder.DecodeWithMatchByte(rangeDecoder,
                        (uint)nowPos64, prevByte, outWindow.GetByte(rep0));
                else
                    b = literalDecoder.DecodeNormal(rangeDecoder, (uint)nowPos64, prevByte);
                outWindow.PutByte(b);
                state.UpdateChar();
                nowPos64++;
            }
            else
            {
                uint len;
                if (isRepDecoders[state.Index].Decode(rangeDecoder) == 1)
                {
                    if (isRepG0Decoders[state.Index].Decode(rangeDecoder) == 0)
                    {
                        if (isRep0LongDecoders[(state.Index << Base.NumPosStatesBitsMax) + posState].Decode(rangeDecoder) == 0)
                        {
                            state.UpdateShortRep();
                            outWindow.PutByte(outWindow.GetByte(rep0));
                            nowPos64++;
                            continue;
                        }
                    }
                    else
                    {
                        uint distance;
                        if (isRepG1Decoders[state.Index].Decode(rangeDecoder) == 0)
                        {
                            distance = rep1;
                        }
                        else
                        {
                            if (isRepG2Decoders[state.Index].Decode(rangeDecoder) == 0)
                            {
                                distance = rep2;
                            }
                            else
                            {
                                distance = rep3;
                                rep3 = rep2;
                            }

                            rep2 = rep1;
                        }

                        rep1 = rep0;
                        rep0 = distance;
                    }

                    len = repLenDecoder.Decode(rangeDecoder, posState) + Base.MatchMinLen;
                    state.UpdateRep();
                }
                else
                {
                    rep3 = rep2;
                    rep2 = rep1;
                    rep1 = rep0;
                    len = Base.MatchMinLen + lenDecoder.Decode(rangeDecoder, posState);
                    state.UpdateMatch();
                    var posSlot = posSlotDecoder[Base.GetLenToPosState(len)].Decode(rangeDecoder);
                    if (posSlot >= Base.StartPosModelIndex)
                    {
                        var numDirectBits = (int)((posSlot >> 1) - 1);
                        rep0 = (2 | (posSlot & 1)) << numDirectBits;
                        if (posSlot < Base.EndPosModelIndex)
                        {
                            rep0 += BitTreeDecoder.ReverseDecode(posDecoders,
                                rep0 - posSlot - 1, rangeDecoder, numDirectBits);
                        }
                        else
                        {
                            rep0 += rangeDecoder.DecodeDirectBits(
                                numDirectBits - Base.NumAlignBits) << Base.NumAlignBits;
                            rep0 += posAlignDecoder.ReverseDecode(rangeDecoder);
                        }
                    }
                    else
                    {
                        rep0 = posSlot;
                    }
                }

                if (rep0 >= outWindow.TrainSize + nowPos64 || rep0 >= dictionarySizeCheck)
                {
                    if (rep0 == 0xFFFFFFFF)
                        break;
                    throw new DataErrorException();
                }

                outWindow.CopyBlock(rep0, len);
                nowPos64 += len;
            }
        }

        outWindow.Flush();
        outWindow.ReleaseStream();
        rangeDecoder.ReleaseStream();
    }

    public void SetDecoderProperties(byte[] properties)
    {
        if (properties.Length < 5)
            throw new InvalidParamException();
        var lc = properties[0] % 9;
        var remainder = properties[0] / 9;
        var lp = remainder % 5;
        var pb = remainder / 5;
        if (pb > Base.NumPosStatesBitsMax)
            throw new InvalidParamException();
        uint dictionarySize = 0;
        for (var i = 0; i < 4; i++)
            dictionarySize += (uint)properties[1 + i] << (i * 8);
        SetDictionarySize(dictionarySize);
        SetLiteralProperties(lp, lc);
        SetPosBitsProperties(pb);
    }

    public bool Train(Stream stream)
    {
        solid = true;
        return outWindow.Train(stream);
    }

    private class LenDecoder
    {
        private BitDecoder choice = new();
        private BitDecoder choice2 = new();
        private BitTreeDecoder highCoder = new(Base.NumHighLenBits);
        private BitTreeDecoder[] lowCoder = new BitTreeDecoder[Base.NumPosStatesMax];
        private BitTreeDecoder[] midCoder = new BitTreeDecoder[Base.NumPosStatesMax];
        private uint numPosStates = 0;

        public void Create(uint numPosStates)
        {
            for (var posState = this.numPosStates; posState < numPosStates; posState++)
            {
                lowCoder[posState] = new BitTreeDecoder(Base.NumLowLenBits);
                midCoder[posState] = new BitTreeDecoder(Base.NumMidLenBits);
            }

            this.numPosStates = numPosStates;
        }

        public void Init()
        {
            choice.Init();
            for (uint posState = 0; posState < numPosStates; posState++)
            {
                lowCoder[posState].Init();
                midCoder[posState].Init();
            }

            choice2.Init();
            highCoder.Init();
        }

        public uint Decode(FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.RangeCoder.RangeDecoder rangeDecoder, uint posState)
        {
            if (choice.Decode(rangeDecoder) == 0)
            {
                return lowCoder[posState].Decode(rangeDecoder);
            }
            else
            {
                var symbol = Base.NumLowLenSymbols;
                if (choice2.Decode(rangeDecoder) == 0)
                {
                    symbol += midCoder[posState].Decode(rangeDecoder);
                }
                else
                {
                    symbol += Base.NumMidLenSymbols;
                    symbol += highCoder.Decode(rangeDecoder);
                }

                return symbol;
            }
        }
    }

    private class LiteralDecoder
    {
        private Decoder2[]? coders;
        private int         numPosBits;
        private int         numPrevBits;
        private uint        posMask;

        public void Create(int numPosBits, int numPrevBits)
        {
            if (coders != null && this.numPrevBits == numPrevBits &&
                this.numPosBits == numPosBits)
                return;
            this.numPosBits  = numPosBits;
            posMask        = ((uint)1 << numPosBits) - 1;
            this.numPrevBits = numPrevBits;
            var numStates = (uint)1 << (this.numPrevBits + this.numPosBits);
            coders = new Decoder2[numStates];
            for (uint i = 0; i < numStates; i++)
                coders[i].Create();
        }

        public void Init()
        {
            var numStates = (uint)1 << (numPrevBits + numPosBits);
            for (uint i = 0; i < numStates; i++)
                coders![i].Init();
        }

        private uint GetState(uint pos, byte prevByte) => ((pos & posMask) << numPrevBits) + (uint)(prevByte >> (8 - numPrevBits));

        public byte DecodeNormal(FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.RangeCoder.RangeDecoder rangeDecoder, uint pos
            , byte prevByte) =>
            coders![GetState(pos, prevByte)].DecodeNormal(rangeDecoder);

        public byte DecodeWithMatchByte(FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.RangeCoder.RangeDecoder rangeDecoder, uint pos
            , byte prevByte, byte matchByte) =>
            coders![GetState(pos, prevByte)].DecodeWithMatchByte(rangeDecoder, matchByte);

        private struct Decoder2
        {
            private BitDecoder[] decoders;

            public void Create()
            {
                decoders = new BitDecoder[0x300];
            }

            public void Init()
            {
                for (var i = 0; i < 0x300; i++) decoders[i].Init();
            }

            public byte DecodeNormal(FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.RangeCoder.RangeDecoder rangeDecoder)
            {
                uint symbol = 1;
                do
                {
                    symbol = (symbol << 1) | decoders[symbol].Decode(rangeDecoder);
                } while (symbol < 0x100);

                return (byte)symbol;
            }

            public byte DecodeWithMatchByte(FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.RangeCoder.RangeDecoder rangeDecoder
                , byte matchByte)
            {
                uint symbol = 1;
                do
                {
                    var matchBit = (uint)(matchByte >> 7) & 1;
                    matchByte <<= 1;
                    var bit = decoders[((1 + matchBit) << 8) + symbol].Decode(rangeDecoder);
                    symbol = (symbol << 1) | bit;
                    if (matchBit != bit)
                    {
                        while (symbol < 0x100)
                            symbol = (symbol << 1) | decoders[symbol].Decode(rangeDecoder);
                        break;
                    }
                } while (symbol < 0x100);

                return (byte)symbol;
            }
        }
    };

    /*
    public override bool CanRead { get { return true; }}
    public override bool CanWrite { get { return true; }}
    public override bool CanSeek { get { return true; }}
    public override long Length { get { return 0; }}
    public override long Position
    {
        get { return 0;	}
        set { }
    }
    public override void Flush() { }
    public override int Read(byte[] buffer, int offset, int count)
    {
        return 0;
    }
    public override void Write(byte[] buffer, int offset, int count)
    {
    }
    public override long Seek(long offset, System.IO.SeekOrigin origin)
    {
        return 0;
    }
    public override void SetLength(long value) {}
    */
}
