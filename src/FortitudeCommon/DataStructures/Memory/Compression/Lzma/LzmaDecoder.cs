// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

#region

using FortitudeCommon.DataStructures.Memory.Compression.Lzma.ByteStreams;
using FortitudeCommon.DataStructures.Memory.Compression.Lzma.Coders;
using FortitudeCommon.OSWrapper.Streams;

#endregion

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma;

public interface ILzmaDecoder
{
    void Decompress(IStream inStream, IStream outStream, ICodecProgress? progress = null);
}

public class LzmaDecoder : ILzmaDecoder // ,System.IO.Stream
{
    private bool solid = false;

    private uint dictionarySize;
    private uint dictionarySizeCheck;

    private BitDecoder[] isMatchDecoders = new BitDecoder[LzmaCodecConstants.NumStates << LzmaCodecConstants.NumPosStatesBitsMax];
    private BitDecoder[] isRep0LongDecoders = new BitDecoder[LzmaCodecConstants.NumStates << LzmaCodecConstants.NumPosStatesBitsMax];
    private BitDecoder[] isRepDecoders = new BitDecoder[LzmaCodecConstants.NumStates];
    private BitDecoder[] isRepG0Decoders = new BitDecoder[LzmaCodecConstants.NumStates];
    private BitDecoder[] isRepG1Decoders = new BitDecoder[LzmaCodecConstants.NumStates];
    private BitDecoder[] isRepG2Decoders = new BitDecoder[LzmaCodecConstants.NumStates];

    private LenDecoder lenDecoder = new();

    private LiteralDecoder literalDecoder = new();

    private IOutWindow outWindow = null!;

    private BitTreeDecoder posAlignDecoder = new(LzmaCodecConstants.NumAlignBits);
    private BitDecoder[] posDecoders = new BitDecoder[LzmaCodecConstants.NumFullDistances - LzmaCodecConstants.EndPosModelIndex];

    private BitTreeDecoder[] posSlotDecoder = new BitTreeDecoder[LzmaCodecConstants.NumLenToPosStates];

    private uint posStateMask;
    private RangeDecoder rangeDecoder = new();
    private LenDecoder repLenDecoder = new();

    public LzmaDecoder()
    {
        dictionarySize = 0xFFFFFFFF;
        for (var i = 0; i < LzmaCodecConstants.NumLenToPosStates; i++)
            posSlotDecoder[i] = new BitTreeDecoder(LzmaCodecConstants.NumPosSlotBits);
    }

    public void Decompress(IStream inStream, IStream outStream, ICodecProgress? progress = null)
    {
        long outSize = Init(inStream, outStream);

        var state = new LzmaCodecConstants.State();
        state.Init();
        uint rep0 = 0, rep1 = 0, rep2 = 0, rep3 = 0;

        ulong nowPos64 = 0;
        var outSize64 = (ulong)outSize;
        if (nowPos64 < outSize64)
        {
            if (isMatchDecoders[state.Index << LzmaCodecConstants.NumPosStatesBitsMax].Decode(rangeDecoder) != 0)
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
            if (isMatchDecoders[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].Decode(rangeDecoder) == 0)
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
                        if (isRep0LongDecoders[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].Decode(rangeDecoder) == 0)
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

                    len = repLenDecoder.Decode(rangeDecoder, posState) + LzmaCodecConstants.MatchMinLen;
                    state.UpdateRep();
                }
                else
                {
                    rep3 = rep2;
                    rep2 = rep1;
                    rep1 = rep0;
                    len = LzmaCodecConstants.MatchMinLen + lenDecoder.Decode(rangeDecoder, posState);
                    state.UpdateMatch();
                    var posSlot = posSlotDecoder[LzmaCodecConstants.GetLenToPosState(len)].Decode(rangeDecoder);
                    if (posSlot >= LzmaCodecConstants.StartPosModelIndex)
                    {
                        var numDirectBits = (int)((posSlot >> 1) - 1);
                        rep0 = (2 | posSlot & 1) << numDirectBits;
                        if (posSlot < LzmaCodecConstants.EndPosModelIndex)
                        {
                            rep0 += BitTreeDecoder.ReverseDecode(posDecoders,
                                rep0 - posSlot - 1, rangeDecoder, numDirectBits);
                        }
                        else
                        {
                            rep0 += rangeDecoder.DecodeDirectBits(
                                numDirectBits - LzmaCodecConstants.NumAlignBits) << LzmaCodecConstants.NumAlignBits;
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

    private void SetDictionarySize(uint dictionarySize)
    {
        if (this.dictionarySize != dictionarySize || this.dictionarySize == 0xFFFFFFFF && dictionarySize < this.dictionarySize )
        {
            this.dictionarySize = dictionarySize;
            dictionarySizeCheck = Math.Max(this.dictionarySize, 1);
        }
        var blockSize = Math.Max(dictionarySizeCheck, 1 << 12);
        outWindow.Create(blockSize);
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
        if (pb > LzmaCodecConstants.NumPosStatesBitsMax)
            throw new InvalidParamException();
        var numPosStates = (uint)1 << pb;
        lenDecoder.Create(numPosStates);
        repLenDecoder.Create(numPosStates);
        posStateMask = numPosStates - 1;
    }

    private long Init(IStream inStream, IStream outStream)
    {
        outWindow = new OutWindow();
        outWindow.Init(outStream, solid);
        byte[] properties = new byte[5];

        int readBytes = inStream.Read(properties, 0, 5);
        if (readBytes != 5)
            throw new Exception("input .lzma is too short");
        SetDecoderProperties(properties);
        
        long outSize = 0;
        for (int k = 0; k < 8; k++)
        {
            int v = inStream.ReadByte();
            if (v < 0)
                throw new Exception("Can't Read 1");
            outSize |= (long)(byte)v << 8 * k;
        }
        
        rangeDecoder.Init(inStream);
        uint i;
        for (i = 0; i < LzmaCodecConstants.NumStates; i++)
        {
            for (uint j = 0; j <= posStateMask; j++)
            {
                var index = (i << LzmaCodecConstants.NumPosStatesBitsMax) + j;
                isMatchDecoders[index].Init();
                isRep0LongDecoders[index].Init();
            }

            isRepDecoders[i].Init();
            isRepG0Decoders[i].Init();
            isRepG1Decoders[i].Init();
            isRepG2Decoders[i].Init();
        }

        literalDecoder.Init();
        for (i = 0; i < LzmaCodecConstants.NumLenToPosStates; i++)
            posSlotDecoder[i].Init();
        // m_PosSpecDecoder.Init();
        for (i = 0; i < LzmaCodecConstants.NumFullDistances - LzmaCodecConstants.EndPosModelIndex; i++)
            posDecoders[i].Init();

        lenDecoder.Init();
        repLenDecoder.Init();
        posAlignDecoder.Init();

        return outSize;
    }

    private void SetDecoderProperties(byte[] properties)
    {
        if (properties.Length < 5)
            throw new InvalidParamException();
        var lc = properties[0] % 9;
        var remainder = properties[0] / 9;
        var lp = remainder % 5;
        var pb = remainder / 5;
        if (pb > LzmaCodecConstants.NumPosStatesBitsMax)
            throw new InvalidParamException();
        uint dictionarySize = 0;
        for (var i = 0; i < 4; i++)
            dictionarySize += (uint)properties[1 + i] << i * 8;
        SetDictionarySize(dictionarySize);
        SetLiteralProperties(lp, lc);
        SetPosBitsProperties(pb);
    }

    private class LenDecoder
    {
        private BitDecoder choice = new();
        private BitDecoder choice2 = new();
        private BitTreeDecoder highCoder = new(LzmaCodecConstants.NumHighLenBits);
        private BitTreeDecoder[] lowCoder = new BitTreeDecoder[LzmaCodecConstants.NumPosStatesMax];
        private BitTreeDecoder[] midCoder = new BitTreeDecoder[LzmaCodecConstants.NumPosStatesMax];
        private uint numPosStates = 0;

        public void Create(uint numPosStates)
        {
            for (var posState = this.numPosStates; posState < numPosStates; posState++)
            {
                lowCoder[posState] = new BitTreeDecoder(LzmaCodecConstants.NumLowLenBits);
                midCoder[posState] = new BitTreeDecoder(LzmaCodecConstants.NumMidLenBits);
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

        public uint Decode(RangeDecoder rangeDecoder, uint posState)
        {
            if (choice.Decode(rangeDecoder) == 0)
            {
                return lowCoder[posState].Decode(rangeDecoder);
            }
            else
            {
                var symbol = LzmaCodecConstants.NumLowLenSymbols;
                if (choice2.Decode(rangeDecoder) == 0)
                {
                    symbol += midCoder[posState].Decode(rangeDecoder);
                }
                else
                {
                    symbol += LzmaCodecConstants.NumMidLenSymbols;
                    symbol += highCoder.Decode(rangeDecoder);
                }

                return symbol;
            }
        }
    }

    private class LiteralDecoder
    {
        private Decoder2[]? coders;
        private int numPosBits;
        private int numPrevBits;
        private uint posMask;

        public void Create(int numPosBits, int numPrevBits)
        {
            if (coders != null && this.numPrevBits == numPrevBits &&
                this.numPosBits == numPosBits)
                return;
            this.numPosBits = numPosBits;
            posMask = ((uint)1 << numPosBits) - 1;
            this.numPrevBits = numPrevBits;
            var numStates = (uint)1 << this.numPrevBits + this.numPosBits;
            coders = new Decoder2[numStates];
            for (uint i = 0; i < numStates; i++)
                coders[i].Create();
        }

        public void Init()
        {
            var numStates = (uint)1 << numPrevBits + numPosBits;
            for (uint i = 0; i < numStates; i++)
                coders![i].Init();
        }

        private uint GetState(uint pos, byte prevByte) => ((pos & posMask) << numPrevBits) + (uint)(prevByte >> 8 - numPrevBits);

        public byte DecodeNormal(RangeDecoder rangeDecoder, uint pos
            , byte prevByte) =>
            coders![GetState(pos, prevByte)].DecodeNormal(rangeDecoder);

        public byte DecodeWithMatchByte(RangeDecoder rangeDecoder, uint pos
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

            public byte DecodeNormal(RangeDecoder rangeDecoder)
            {
                uint symbol = 1;
                do
                {
                    symbol = symbol << 1 | decoders[symbol].Decode(rangeDecoder);
                } while (symbol < 0x100);

                return (byte)symbol;
            }

            public byte DecodeWithMatchByte(RangeDecoder rangeDecoder
                , byte matchByte)
            {
                uint symbol = 1;
                do
                {
                    var matchBit = (uint)(matchByte >> 7) & 1;
                    matchByte <<= 1;
                    var bit = decoders[(1 + matchBit << 8) + symbol].Decode(rangeDecoder);
                    symbol = symbol << 1 | bit;
                    if (matchBit != bit)
                    {
                        while (symbol < 0x100)
                            symbol = symbol << 1 | decoders[symbol].Decode(rangeDecoder);
                        break;
                    }
                } while (symbol < 0x100);

                return (byte)symbol;
            }
        }
    };
}
