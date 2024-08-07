// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

#region

using FortitudeCommon.DataStructures.Memory.Compression.Lzma.ByteStreams;
using FortitudeCommon.DataStructures.Memory.Compression.Lzma.Coders;
using FortitudeCommon.OSWrapper.Streams;

#endregion

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma;

public interface ILzmaEncoder
{
    void Compress(LzmaEncoderParams encoderParams, IStream inStream, IStream outStream);
}

public class LzmaEncoder : ILzmaEncoder
{
    private const uint IfinityPrice = 0xFFFFFFF;

    private const int DefaultDictionaryLogSize = 22;
    private const uint NumFastBytesDefault = 0x20;

    private const uint NumLenSpecSymbols = LzmaCodecConstants.NumLowLenSymbols + LzmaCodecConstants.NumMidLenSymbols;

    private const uint NumOpts = 1 << 12;

    private const int PropSize = 5;

    private static byte[] fastPos = new byte[1 << 11];

    private uint additionalOffset;
    private uint alignPriceCount;
    private uint[] alignPrices = new uint[LzmaCodecConstants.AlignTableSize];

    private uint dictionarySize = 1 << DefaultDictionaryLogSize;
    private uint dictionarySizePrev = 0xFFFFFFFF;
    private uint[] distancesPrices = new uint[LzmaCodecConstants.NumFullDistances << LzmaCodecConstants.NumLenToPosStatesBits];

    private uint distTableSize = DefaultDictionaryLogSize * 2;
    private bool finished;

    private BitEncoder[] isMatch = new BitEncoder[LzmaCodecConstants.NumStates << LzmaCodecConstants.NumPosStatesBitsMax];
    private BitEncoder[] isRep = new BitEncoder[LzmaCodecConstants.NumStates];
    private BitEncoder[] isRep0Long = new BitEncoder[LzmaCodecConstants.NumStates << LzmaCodecConstants.NumPosStatesBitsMax];
    private BitEncoder[] isRepG0 = new BitEncoder[LzmaCodecConstants.NumStates];
    private BitEncoder[] isRepG1 = new BitEncoder[LzmaCodecConstants.NumStates];
    private BitEncoder[] isRepG2 = new BitEncoder[LzmaCodecConstants.NumStates];

    private LenPriceTableEncoder lenEncoder = new();

    private LiteralEncoder literalEncoder = new();
    private uint longestMatchLength;

    private bool longestMatchWasFound;

    private uint[] matchDistances = new uint[LzmaCodecConstants.MatchMaxLen * 2 + 2];
    private IMatchFinder matchFinder = null!;

    private MatchFinderType matchFinderType = MatchFinderType.BT4;
    private uint matchPriceCount;

    private bool needReleaseMFStream;
    private uint numDistancePairs;

    private uint numFastBytes = NumFastBytesDefault;
    private uint numFastBytesPrev = 0xFFFFFFFF;
    private int numLiteralContextBits = 3;
    private int numLiteralPosStateBits = 0;
    private Optimal[] optimum = new Optimal[NumOpts];
    private uint optimumCurrentIndex;

    private uint optimumEndIndex;
    private BitTreeEncoder posAlignEncoder = new(LzmaCodecConstants.NumAlignBits);

    private BitEncoder[] posEncoders = new BitEncoder[LzmaCodecConstants.NumFullDistances - LzmaCodecConstants.EndPosModelIndex];

    private BitTreeEncoder[] posSlotEncoder = new BitTreeEncoder[LzmaCodecConstants.NumLenToPosStates];

    private uint[] posSlotPrices = new uint[1 << LzmaCodecConstants.NumPosSlotBits + LzmaCodecConstants.NumLenToPosStatesBits];

    private int posStateBits = 2;
    private uint posStateMask = 4 - 1;
    private byte previousByte;
    private RangeEncoder rangeEncoder = new();
    private uint[] repDistances = new uint[LzmaCodecConstants.NumRepDistances];
    private LenPriceTableEncoder repMatchLenEncoder = new();

    private LzmaCodecConstants.State state = new();

    private uint trainSize = 0;
    private bool writeEndMark = false;

    private long nowPos64;
    private byte[] properties = new byte[PropSize];
    private uint[] repLens = new uint[LzmaCodecConstants.NumRepDistances];

    private uint[] reps = new uint[LzmaCodecConstants.NumRepDistances];

    private uint[] tempPrices = new uint[LzmaCodecConstants.NumFullDistances];

    static LzmaEncoder()
    {
        const byte kFastSlots = 22;
        var c = 2;
        fastPos[0] = 0;
        fastPos[1] = 1;
        for (byte slotFast = 2; slotFast < kFastSlots; slotFast++)
        {
            var k = (uint)1 << (slotFast >> 1) - 1;
            for (uint j = 0; j < k; j++, c++)
                fastPos[c] = slotFast;
        }
    }

    public LzmaEncoder()
    {
        for (int i = 0; i < NumOpts; i++)
            optimum[i] = new Optimal();
        for (int i = 0; i < LzmaCodecConstants.NumLenToPosStates; i++)
            posSlotEncoder[i] = new BitTreeEncoder(LzmaCodecConstants.NumPosSlotBits);
    }

    public void Compress(LzmaEncoderParams encoderParams, IStream inStream, IStream outStream)
    {
        needReleaseMFStream = false;

        CoderPropID[] propIDs =
        {
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker
        };
        object[] properties =
        {
            encoderParams.DictionarySize,
            encoderParams.PosStateBits,
            encoderParams.LitContextBits,
            encoderParams.LitPosBits,
            encoderParams.Algorithm,
            encoderParams.NumFastBytes,
            encoderParams.MatchFinder,
            encoderParams.HasEOS
        };
        SetCoderProperties(propIDs, properties);
        WriteCoderProperties(outStream);
        long fileSize;
        if (encoderParams.HasEOS)
            fileSize = -1;
        else
            fileSize = inStream.Length;
        for (int i = 0; i < 8; i++)
        {
            var writeByte = (byte)(fileSize >> 8 * i);
            outStream.WriteByte(writeByte);
        }

        try
        {
            SetStreams(inStream, outStream);
            
            matchFinder.SetStream(inStream);
            matchFinder.Init();
            if (encoderParams.TrainStream != null)
            {
                var trainByteStream = encoderParams.TrainStream;
                var trainStreamSize = trainByteStream.Length;

                IInWindow trainWindow;
                uint      trainLength;
                trainWindow = new InWindow();
                trainWindow.SetStream(trainByteStream);
                trainLength = (uint)trainByteStream.Length;
                matchFinder.Process(trainLength, trainWindow);
            }
            while (true)
            {
                long processedInSize;
                long processedOutSize;
                bool finished;
                CodeOneBlock(out processedInSize, out processedOutSize, out finished);
                if (finished)
                    return;
                if (encoderParams.CodecProgress != null) encoderParams.CodecProgress.SetProgress(processedInSize, processedOutSize);
            }
        }
        finally
        {
            ReleaseStreams();
        }
    }

    private void SetCoderProperties(CoderPropID[] propIDs, object[] properties)
    {
        for (uint i = 0; i < properties.Length; i++)
        {
            var prop = properties[i];
            switch (propIDs[i])
            {
                case CoderPropID.NumFastBytes:
                    {
                        if (!(prop is int))
                            throw new InvalidParamException();
                        var numFastBytes = (int)prop;
                        if (numFastBytes < 5 || numFastBytes > LzmaCodecConstants.MatchMaxLen)
                            throw new InvalidParamException();
                        this.numFastBytes = (uint)numFastBytes;
                        break;
                    }
                case CoderPropID.Algorithm:
                    {
                        /*
                        if (!(prop is Int32))
                            throw new InvalidParamException();
                        Int32 maximize = (Int32)prop;
                        _fastMode = (maximize == 0);
                        _maxMode = (maximize >= 2);
                        */
                        break;
                    }
                case CoderPropID.MatchFinder:
                    {
                        if (!(prop is string or MatchFinderType))
                            throw new InvalidParamException();
                        var matchFinderIndexPrev = matchFinderType;
                        var m = FindMatchFinder(prop.ToString()!);
                        if (m < 0)
                            throw new InvalidParamException();
                        if (m != null && matchFinderIndexPrev != matchFinderType)
                        {
                            matchFinderType    = m.Value;
                            dictionarySizePrev = 0xFFFFFFFF;
                            matchFinder        = null!;
                        }

                        break;
                    }
                case CoderPropID.DictionarySize:
                    {
                        const int DicLogSizeMaxCompress = 30;
                        if (!(prop is int))
                            throw new InvalidParamException();
                        ;
                        var dictionarySize = (int)prop;
                        if (dictionarySize < (uint)(1 << LzmaCodecConstants.DicLogSizeMin) ||
                            dictionarySize > (uint)(1 << DicLogSizeMaxCompress))
                            throw new InvalidParamException();
                        this.dictionarySize = (uint)dictionarySize;
                        int dicLogSize;
                        for (dicLogSize = 0; dicLogSize < (uint)DicLogSizeMaxCompress; dicLogSize++)
                            if (dictionarySize <= (uint)1 << dicLogSize)
                                break;
                        distTableSize = (uint)dicLogSize * 2;
                        break;
                    }
                case CoderPropID.PosStateBits:
                    {
                        if (!(prop is int))
                            throw new InvalidParamException();
                        var v = (int)prop;
                        if (v < 0 || v > (uint)LzmaCodecConstants.NumPosStatesBitsEncodingMax)
                            throw new InvalidParamException();
                        posStateBits = v;
                        posStateMask = ((uint)1 << posStateBits) - 1;
                        break;
                    }
                case CoderPropID.LitPosBits:
                    {
                        if (!(prop is int))
                            throw new InvalidParamException();
                        var v = (int)prop;
                        if (v < 0 || v > LzmaCodecConstants.NumLitPosStatesBitsEncodingMax)
                            throw new InvalidParamException();
                        numLiteralPosStateBits = v;
                        break;
                    }
                case CoderPropID.LitContextBits:
                    {
                        if (!(prop is int))
                            throw new InvalidParamException();
                        var v = (int)prop;
                        if (v < 0 || v > LzmaCodecConstants.NumLitContextBitsMax)
                            throw new InvalidParamException();
                        ;
                        numLiteralContextBits = v;
                        break;
                    }
                case CoderPropID.EndMarker:
                    {
                        if (!(prop is bool))
                            throw new InvalidParamException();
                        SetWriteEndMarkerMode((bool)prop);
                        break;
                    }
                default:
                    throw new InvalidParamException();
            }
        }
    }

    private void WriteCoderProperties(IStream outStream)
    {
        properties[0] = (byte)((posStateBits * 5 + numLiteralPosStateBits) * 9 + numLiteralContextBits);
        for (var i = 0; i < 4; i++)
            properties[1 + i] = (byte)(dictionarySize >> 8 * i & 0xFF);
        outStream.Write(properties, 0, PropSize);
    }

    private static uint GetPosSlot(uint pos)
    {
        if (pos < 1 << 11)
            return fastPos[pos];
        if (pos < 1 << 21)
            return (uint)(fastPos[pos >> 10] + 20);
        return (uint)(fastPos[pos >> 20] + 40);
    }

    private static uint GetPosSlot2(uint pos)
    {
        if (pos < 1 << 17)
            return (uint)(fastPos[pos >> 6] + 12);
        if (pos < 1 << 27)
            return (uint)(fastPos[pos >> 16] + 32);
        return (uint)(fastPos[pos >> 26] + 52);
    }

    private void BaseInit()
    {
        state.Init();
        previousByte = 0;
        for (uint i = 0; i < LzmaCodecConstants.NumRepDistances; i++)
            repDistances[i] = 0;
    }

    private void Create(IStream inputStream)
    {
        if (matchFinder == null)
        {
            var inputWindow = new InWindow();
            var bt           = new BinTree(inputWindow);
            var numHashBytes = 4;
            if (matchFinderType == MatchFinderType.BT2)
                numHashBytes = 2;
            bt.SetType(numHashBytes);
            matchFinder = bt;
        }

        literalEncoder.Create(numLiteralPosStateBits, numLiteralContextBits);

        if (dictionarySize == dictionarySizePrev && numFastBytesPrev == numFastBytes)
            return;
        matchFinder.Create(dictionarySize, NumOpts, numFastBytes, LzmaCodecConstants.MatchMaxLen + 1);
        dictionarySizePrev = dictionarySize;
        numFastBytesPrev = numFastBytes;
    }

    private void SetWriteEndMarkerMode(bool writeEndMarker)
    {
        writeEndMark = writeEndMarker;
    }

    private void Init()
    {
        BaseInit();
        rangeEncoder.Init();

        uint i;
        for (i = 0; i < LzmaCodecConstants.NumStates; i++)
        {
            for (uint j = 0; j <= posStateMask; j++)
            {
                var complexState = (i << LzmaCodecConstants.NumPosStatesBitsMax) + j;
                isMatch[complexState].Init();
                isRep0Long[complexState].Init();
            }

            isRep[i].Init();
            isRepG0[i].Init();
            isRepG1[i].Init();
            isRepG2[i].Init();
        }

        literalEncoder.Init();
        for (i = 0; i < LzmaCodecConstants.NumLenToPosStates; i++)
            posSlotEncoder[i].Init();
        for (i = 0; i < LzmaCodecConstants.NumFullDistances - LzmaCodecConstants.EndPosModelIndex; i++)
            posEncoders[i].Init();

        lenEncoder.Init((uint)1 << posStateBits);
        repMatchLenEncoder.Init((uint)1 << posStateBits);

        posAlignEncoder.Init();

        longestMatchWasFound = false;
        optimumEndIndex = 0;
        optimumCurrentIndex = 0;
        additionalOffset = 0;
    }

    private void ReadMatchDistances(out uint lenRes, out uint numDistancePairs)
    {
        lenRes = 0;
        numDistancePairs = matchFinder.GetMatches(matchDistances);
        if (numDistancePairs > 0)
        {
            lenRes = matchDistances[numDistancePairs - 2];
            if (lenRes == numFastBytes)
                lenRes += matchFinder.GetMatchLen((int)lenRes - 1, matchDistances[numDistancePairs - 1],
                    LzmaCodecConstants.MatchMaxLen - lenRes);
        }

        additionalOffset++;
    }


    private void MovePos(uint num)
    {
        if (num > 0)
        {
            matchFinder.Process(num);
            additionalOffset += num;
        }
    }

    private uint GetRepLen1Price(LzmaCodecConstants.State state, uint posState) =>
        isRepG0[state.Index].GetPrice0() +
        isRep0Long[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].GetPrice0();

    private uint GetPureRepPrice(uint repIndex, LzmaCodecConstants.State state, uint posState)
    {
        uint price;
        if (repIndex == 0)
        {
            price = isRepG0[state.Index].GetPrice0();
            price += isRep0Long[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].GetPrice1();
        }
        else
        {
            price = isRepG0[state.Index].GetPrice1();
            if (repIndex == 1)
            {
                price += isRepG1[state.Index].GetPrice0();
            }
            else
            {
                price += isRepG1[state.Index].GetPrice1();
                price += isRepG2[state.Index].GetPrice(repIndex - 2);
            }
        }

        return price;
    }

    private uint GetRepPrice(uint repIndex, uint len, LzmaCodecConstants.State state, uint posState)
    {
        var price = repMatchLenEncoder.GetPrice(len - LzmaCodecConstants.MatchMinLen, posState);
        return price + GetPureRepPrice(repIndex, state, posState);
    }

    private uint GetPosLenPrice(uint pos, uint len, uint posState)
    {
        uint price;
        var lenToPosState = LzmaCodecConstants.GetLenToPosState(len);
        if (pos < LzmaCodecConstants.NumFullDistances)
            price = distancesPrices[lenToPosState * LzmaCodecConstants.NumFullDistances + pos];
        else
            price = posSlotPrices[(lenToPosState << LzmaCodecConstants.NumPosSlotBits) + GetPosSlot2(pos)] +
                    alignPrices[pos & LzmaCodecConstants.AlignMask];
        return price + lenEncoder.GetPrice(len - LzmaCodecConstants.MatchMinLen, posState);
    }

    private uint Backward(out uint backRes, uint cur)
    {
        optimumEndIndex = cur;
        var posMem = optimum[cur].PosPrev;
        var backMem = optimum[cur].BackPrev;
        do
        {
            if (optimum[cur].Prev1IsChar)
            {
                optimum[posMem].MakeAsChar();
                optimum[posMem].PosPrev = posMem - 1;
                if (optimum[cur].Prev2)
                {
                    optimum[posMem - 1].Prev1IsChar = false;
                    optimum[posMem - 1].PosPrev = optimum[cur].PosPrev2;
                    optimum[posMem - 1].BackPrev = optimum[cur].BackPrev2;
                }
            }

            var posPrev = posMem;
            var backCur = backMem;

            backMem = optimum[posPrev].BackPrev;
            posMem = optimum[posPrev].PosPrev;

            optimum[posPrev].BackPrev = backCur;
            optimum[posPrev].PosPrev = cur;
            cur = posPrev;
        } while (cur > 0);

        backRes = optimum[0].BackPrev;
        optimumCurrentIndex = optimum[0].PosPrev;
        return optimumCurrentIndex;
    }


    private uint GetOptimum(uint position, out uint backRes)
    {
        if (optimumEndIndex != optimumCurrentIndex)
        {
            var lenRes = optimum[optimumCurrentIndex].PosPrev - optimumCurrentIndex;
            backRes = optimum[optimumCurrentIndex].BackPrev;
            optimumCurrentIndex = optimum[optimumCurrentIndex].PosPrev;
            return lenRes;
        }

        optimumCurrentIndex = optimumEndIndex = 0;

        uint lenMain, numDistancePairs;
        if (!longestMatchWasFound)
        {
            ReadMatchDistances(out lenMain, out numDistancePairs);
        }
        else
        {
            lenMain = longestMatchLength;
            numDistancePairs = this.numDistancePairs;
            longestMatchWasFound = false;
        }

        var numAvailableBytes = matchFinder.GetNumAvailableBytes() + 1;
        if (numAvailableBytes < 2)
        {
            backRes = 0xFFFFFFFF;
            return 1;
        }

        if (numAvailableBytes > LzmaCodecConstants.MatchMaxLen)
            numAvailableBytes = LzmaCodecConstants.MatchMaxLen;

        uint repMaxIndex = 0;
        uint i;
        for (i = 0; i < LzmaCodecConstants.NumRepDistances; i++)
        {
            reps[i] = repDistances[i];
            repLens[i] = matchFinder.GetMatchLen(0 - 1, reps[i], LzmaCodecConstants.MatchMaxLen);
            if (repLens[i] > repLens[repMaxIndex])
                repMaxIndex = i;
        }

        if (repLens[repMaxIndex] >= numFastBytes)
        {
            backRes = repMaxIndex;
            var lenRes = repLens[repMaxIndex];
            MovePos(lenRes - 1);
            return lenRes;
        }

        if (lenMain >= numFastBytes)
        {
            backRes = matchDistances[numDistancePairs - 1] + LzmaCodecConstants.NumRepDistances;
            MovePos(lenMain - 1);
            return lenMain;
        }

        var currentByte = matchFinder.GetIndexByte(0 - 1);
        var matchByte = matchFinder.GetIndexByte((int)(0 - repDistances[0] - 1 - 1));

        if (lenMain < 2 && currentByte != matchByte && repLens[repMaxIndex] < 2)
        {
            backRes = 0xFFFFFFFF;
            return 1;
        }

        optimum[0].State = state;

        var posState = position & posStateMask;

        optimum[1].Price = isMatch[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].GetPrice0() +
                            literalEncoder.GetSubCoder(position, previousByte).GetPrice(!state.IsCharState(), matchByte, currentByte);
        optimum[1].MakeAsChar();

        var matchPrice = isMatch[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].GetPrice1();
        var repMatchPrice = matchPrice + isRep[state.Index].GetPrice1();

        if (matchByte == currentByte)
        {
            var shortRepPrice = repMatchPrice + GetRepLen1Price(state, posState);
            if (shortRepPrice < optimum[1].Price)
            {
                optimum[1].Price = shortRepPrice;
                optimum[1].MakeAsShortRep();
            }
        }

        var lenEnd = lenMain >= repLens[repMaxIndex] ? lenMain : repLens[repMaxIndex];

        if (lenEnd < 2)
        {
            backRes = optimum[1].BackPrev;
            return 1;
        }

        optimum[1].PosPrev = 0;

        optimum[0].Backs0 = reps[0];
        optimum[0].Backs1 = reps[1];
        optimum[0].Backs2 = reps[2];
        optimum[0].Backs3 = reps[3];

        var len = lenEnd;
        do
        {
            optimum[len--].Price = IfinityPrice;
        } while (len >= 2);

        for (i = 0; i < LzmaCodecConstants.NumRepDistances; i++)
        {
            var repLen = repLens[i];
            if (repLen < 2)
                continue;
            var price = repMatchPrice + GetPureRepPrice(i, state, posState);
            do
            {
                var curAndLenPrice = price + repMatchLenEncoder.GetPrice(repLen - 2, posState);
                var optimum = this.optimum[repLen];
                if (curAndLenPrice < optimum.Price)
                {
                    optimum.Price = curAndLenPrice;
                    optimum.PosPrev = 0;
                    optimum.BackPrev = i;
                    optimum.Prev1IsChar = false;
                }
            } while (--repLen >= 2);
        }

        var normalMatchPrice = matchPrice + isRep[state.Index].GetPrice0();

        len = repLens[0] >= 2 ? repLens[0] + 1 : 2;
        if (len <= lenMain)
        {
            uint offs = 0;
            while (len > matchDistances[offs])
                offs += 2;
            for (; ; len++)
            {
                var distance = matchDistances[offs + 1];
                var curAndLenPrice = normalMatchPrice + GetPosLenPrice(distance, len, posState);
                var optimum = this.optimum[len];
                if (curAndLenPrice < optimum.Price)
                {
                    optimum.Price = curAndLenPrice;
                    optimum.PosPrev = 0;
                    optimum.BackPrev = distance + LzmaCodecConstants.NumRepDistances;
                    optimum.Prev1IsChar = false;
                }

                if (len == matchDistances[offs])
                {
                    offs += 2;
                    if (offs == numDistancePairs)
                        break;
                }
            }
        }

        uint cur = 0;

        while (true)
        {
            cur++;
            if (cur == lenEnd)
                return Backward(out backRes, cur);
            uint newLen;
            ReadMatchDistances(out newLen, out numDistancePairs);
            if (newLen >= numFastBytes)
            {
                this.numDistancePairs = numDistancePairs;
                longestMatchLength = newLen;
                longestMatchWasFound = true;
                return Backward(out backRes, cur);
            }

            position++;
            var posPrev = optimum[cur].PosPrev;
            LzmaCodecConstants.State state;
            if (optimum[cur].Prev1IsChar)
            {
                posPrev--;
                if (optimum[cur].Prev2)
                {
                    state = optimum[optimum[cur].PosPrev2].State;
                    if (optimum[cur].BackPrev2 < LzmaCodecConstants.NumRepDistances)
                        state.UpdateRep();
                    else
                        state.UpdateMatch();
                }
                else
                {
                    state = optimum[posPrev].State;
                }

                state.UpdateChar();
            }
            else
            {
                state = optimum[posPrev].State;
            }

            if (posPrev == cur - 1)
            {
                if (optimum[cur].IsShortRep())
                    state.UpdateShortRep();
                else
                    state.UpdateChar();
            }
            else
            {
                uint pos;
                if (optimum[cur].Prev1IsChar && optimum[cur].Prev2)
                {
                    posPrev = optimum[cur].PosPrev2;
                    pos = optimum[cur].BackPrev2;
                    state.UpdateRep();
                }
                else
                {
                    pos = optimum[cur].BackPrev;
                    if (pos < LzmaCodecConstants.NumRepDistances)
                        state.UpdateRep();
                    else
                        state.UpdateMatch();
                }

                var opt = optimum[posPrev];
                if (pos < LzmaCodecConstants.NumRepDistances)
                {
                    if (pos == 0)
                    {
                        reps[0] = opt.Backs0;
                        reps[1] = opt.Backs1;
                        reps[2] = opt.Backs2;
                        reps[3] = opt.Backs3;
                    }
                    else if (pos == 1)
                    {
                        reps[0] = opt.Backs1;
                        reps[1] = opt.Backs0;
                        reps[2] = opt.Backs2;
                        reps[3] = opt.Backs3;
                    }
                    else if (pos == 2)
                    {
                        reps[0] = opt.Backs2;
                        reps[1] = opt.Backs0;
                        reps[2] = opt.Backs1;
                        reps[3] = opt.Backs3;
                    }
                    else
                    {
                        reps[0] = opt.Backs3;
                        reps[1] = opt.Backs0;
                        reps[2] = opt.Backs1;
                        reps[3] = opt.Backs2;
                    }
                }
                else
                {
                    reps[0] = pos - LzmaCodecConstants.NumRepDistances;
                    reps[1] = opt.Backs0;
                    reps[2] = opt.Backs1;
                    reps[3] = opt.Backs2;
                }
            }

            optimum[cur].State = state;
            optimum[cur].Backs0 = reps[0];
            optimum[cur].Backs1 = reps[1];
            optimum[cur].Backs2 = reps[2];
            optimum[cur].Backs3 = reps[3];
            var curPrice = optimum[cur].Price;

            currentByte = matchFinder.GetIndexByte(0 - 1);
            matchByte = matchFinder.GetIndexByte((int)(0 - reps[0] - 1 - 1));

            posState = position & posStateMask;

            var curAnd1Price = curPrice +
                               isMatch[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].GetPrice0() +
                               literalEncoder.GetSubCoder(position, matchFinder.GetIndexByte(0 - 2))
                                   .GetPrice(!state.IsCharState(), matchByte, currentByte);

            var nextOptimum = optimum[cur + 1];

            var nextIsChar = false;
            if (curAnd1Price < nextOptimum.Price)
            {
                nextOptimum.Price = curAnd1Price;
                nextOptimum.PosPrev = cur;
                nextOptimum.MakeAsChar();
                nextIsChar = true;
            }

            matchPrice = curPrice + isMatch[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].GetPrice1();
            repMatchPrice = matchPrice + isRep[state.Index].GetPrice1();

            if (matchByte == currentByte &&
                !(nextOptimum.PosPrev < cur && nextOptimum.BackPrev == 0))
            {
                var shortRepPrice = repMatchPrice + GetRepLen1Price(state, posState);
                if (shortRepPrice <= nextOptimum.Price)
                {
                    nextOptimum.Price = shortRepPrice;
                    nextOptimum.PosPrev = cur;
                    nextOptimum.MakeAsShortRep();
                    nextIsChar = true;
                }
            }

            var numAvailableBytesFull = matchFinder.GetNumAvailableBytes() + 1;
            numAvailableBytesFull = Math.Min(NumOpts - 1 - cur, numAvailableBytesFull);
            numAvailableBytes = numAvailableBytesFull;

            if (numAvailableBytes < 2)
                continue;
            if (numAvailableBytes > numFastBytes)
                numAvailableBytes = numFastBytes;
            if (!nextIsChar && matchByte != currentByte)
            {
                // try Literal + rep0
                var t = Math.Min(numAvailableBytesFull - 1, numFastBytes);
                var lenTest2 = matchFinder.GetMatchLen(0, reps[0], t);
                if (lenTest2 >= 2)
                {
                    var state2 = state;
                    state2.UpdateChar();
                    var posStateNext = position + 1 & posStateMask;
                    var nextRepMatchPrice = curAnd1Price +
                                            isMatch[(state2.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posStateNext].GetPrice1() +
                                            isRep[state2.Index].GetPrice1();
                    {
                        var offset = cur + 1 + lenTest2;
                        while (lenEnd < offset)
                            this.optimum[++lenEnd].Price = IfinityPrice;
                        var curAndLenPrice = nextRepMatchPrice + GetRepPrice(
                            0, lenTest2, state2, posStateNext);
                        var optimum = this.optimum[offset];
                        if (curAndLenPrice < optimum.Price)
                        {
                            optimum.Price = curAndLenPrice;
                            optimum.PosPrev = cur + 1;
                            optimum.BackPrev = 0;
                            optimum.Prev1IsChar = true;
                            optimum.Prev2 = false;
                        }
                    }
                }
            }

            uint startLen = 2; // speed optimization 

            for (uint repIndex = 0; repIndex < LzmaCodecConstants.NumRepDistances; repIndex++)
            {
                var lenTest = matchFinder.GetMatchLen(0 - 1, reps[repIndex], numAvailableBytes);
                if (lenTest < 2)
                    continue;
                var lenTestTemp = lenTest;
                do
                {
                    while (lenEnd < cur + lenTest)
                        this.optimum[++lenEnd].Price = IfinityPrice;
                    var curAndLenPrice = repMatchPrice + GetRepPrice(repIndex, lenTest, state, posState);
                    var optimum = this.optimum[cur + lenTest];
                    if (curAndLenPrice < optimum.Price)
                    {
                        optimum.Price = curAndLenPrice;
                        optimum.PosPrev = cur;
                        optimum.BackPrev = repIndex;
                        optimum.Prev1IsChar = false;
                    }
                } while (--lenTest >= 2);

                lenTest = lenTestTemp;

                if (repIndex == 0)
                    startLen = lenTest + 1;

                // if (_maxMode)
                if (lenTest < numAvailableBytesFull)
                {
                    var t = Math.Min(numAvailableBytesFull - 1 - lenTest, numFastBytes);
                    var lenTest2 = matchFinder.GetMatchLen((int)lenTest, reps[repIndex], t);
                    if (lenTest2 >= 2)
                    {
                        var state2 = state;
                        state2.UpdateRep();
                        var posStateNext = position + lenTest & posStateMask;
                        var curAndLenCharPrice =
                            repMatchPrice + GetRepPrice(repIndex, lenTest, state, posState) +
                            isMatch[(state2.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posStateNext].GetPrice0() +
                            literalEncoder.GetSubCoder(position + lenTest,
                                matchFinder.GetIndexByte((int)lenTest - 1 - 1)).GetPrice(true,
                                matchFinder.GetIndexByte((int)lenTest - 1 - (int)(reps[repIndex] + 1)),
                                matchFinder.GetIndexByte((int)lenTest - 1));
                        state2.UpdateChar();
                        posStateNext = position + lenTest + 1 & posStateMask;
                        var nextMatchPrice = curAndLenCharPrice + isMatch[(state2.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posStateNext].GetPrice1();
                        var nextRepMatchPrice = nextMatchPrice + isRep[state2.Index].GetPrice1();

                        // for(; lenTest2 >= 2; lenTest2--)
                        {
                            var offset = lenTest + 1 + lenTest2;
                            while (lenEnd < cur + offset)
                                this.optimum[++lenEnd].Price = IfinityPrice;
                            var curAndLenPrice = nextRepMatchPrice + GetRepPrice(0, lenTest2, state2, posStateNext);
                            var optimum = this.optimum[cur + offset];
                            if (curAndLenPrice < optimum.Price)
                            {
                                optimum.Price = curAndLenPrice;
                                optimum.PosPrev = cur + lenTest + 1;
                                optimum.BackPrev = 0;
                                optimum.Prev1IsChar = true;
                                optimum.Prev2 = true;
                                optimum.PosPrev2 = cur;
                                optimum.BackPrev2 = repIndex;
                            }
                        }
                    }
                }
            }

            if (newLen > numAvailableBytes)
            {
                newLen = numAvailableBytes;
                for (numDistancePairs = 0; newLen > matchDistances[numDistancePairs]; numDistancePairs += 2) ;
                matchDistances[numDistancePairs] = newLen;
                numDistancePairs += 2;
            }

            if (newLen >= startLen)
            {
                normalMatchPrice = matchPrice + isRep[state.Index].GetPrice0();
                while (lenEnd < cur + newLen)
                    optimum[++lenEnd].Price = IfinityPrice;

                uint offs = 0;
                while (startLen > matchDistances[offs])
                    offs += 2;

                for (var lenTest = startLen; ; lenTest++)
                {
                    var curBack = matchDistances[offs + 1];
                    var curAndLenPrice = normalMatchPrice + GetPosLenPrice(curBack, lenTest, posState);
                    var optimum = this.optimum[cur + lenTest];
                    if (curAndLenPrice < optimum.Price)
                    {
                        optimum.Price = curAndLenPrice;
                        optimum.PosPrev = cur;
                        optimum.BackPrev = curBack + LzmaCodecConstants.NumRepDistances;
                        optimum.Prev1IsChar = false;
                    }

                    if (lenTest == matchDistances[offs])
                    {
                        if (lenTest < numAvailableBytesFull)
                        {
                            var t = Math.Min(numAvailableBytesFull - 1 - lenTest, numFastBytes);
                            var lenTest2 = matchFinder.GetMatchLen((int)lenTest, curBack, t);
                            if (lenTest2 >= 2)
                            {
                                var state2 = state;
                                state2.UpdateMatch();
                                var posStateNext = position + lenTest & posStateMask;
                                var curAndLenCharPrice = curAndLenPrice +
                                                         isMatch[(state2.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posStateNext].GetPrice0() +
                                                         literalEncoder.GetSubCoder(position + lenTest,
                                                             matchFinder.GetIndexByte((int)lenTest - 1 - 1)).GetPrice(true,
                                                             matchFinder.GetIndexByte((int)lenTest - (int)(curBack + 1) - 1),
                                                             matchFinder.GetIndexByte((int)lenTest - 1));
                                state2.UpdateChar();
                                posStateNext = position + lenTest + 1 & posStateMask;
                                var nextMatchPrice = curAndLenCharPrice +
                                                     isMatch[(state2.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posStateNext].GetPrice1();
                                var nextRepMatchPrice = nextMatchPrice + isRep[state2.Index].GetPrice1();

                                var offset = lenTest + 1 + lenTest2;
                                while (lenEnd < cur + offset)
                                    this.optimum[++lenEnd].Price = IfinityPrice;
                                curAndLenPrice = nextRepMatchPrice + GetRepPrice(0, lenTest2, state2, posStateNext);
                                optimum = this.optimum[cur + offset];
                                if (curAndLenPrice < optimum.Price)
                                {
                                    optimum.Price = curAndLenPrice;
                                    optimum.PosPrev = cur + lenTest + 1;
                                    optimum.BackPrev = 0;
                                    optimum.Prev1IsChar = true;
                                    optimum.Prev2 = true;
                                    optimum.PosPrev2 = cur;
                                    optimum.BackPrev2 = curBack + LzmaCodecConstants.NumRepDistances;
                                }
                            }
                        }

                        offs += 2;
                        if (offs == numDistancePairs)
                            break;
                    }
                }
            }
        }
    }

    private bool ChangePair(uint smallDist, uint bigDist)
    {
        const int kDif = 7;
        return smallDist < (uint)1 << 32 - kDif && bigDist >= smallDist << kDif;
    }

    private void WriteEndMarker(uint posState)
    {
        if (!writeEndMark)
            return;

        isMatch[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].Encode(rangeEncoder, 1);
        isRep[state.Index].Encode(rangeEncoder, 0);
        state.UpdateMatch();
        var len = LzmaCodecConstants.MatchMinLen;
        lenEncoder.Encode(rangeEncoder, len - LzmaCodecConstants.MatchMinLen, posState);
        uint posSlot = (1 << LzmaCodecConstants.NumPosSlotBits) - 1;
        var lenToPosState = LzmaCodecConstants.GetLenToPosState(len);
        posSlotEncoder[lenToPosState].Encode(rangeEncoder, posSlot);
        var footerBits = 30;
        var posReduced = ((uint)1 << footerBits) - 1;
        rangeEncoder.EncodeDirectBits(posReduced >> LzmaCodecConstants.NumAlignBits, footerBits - LzmaCodecConstants.NumAlignBits);
        posAlignEncoder.ReverseEncode(rangeEncoder, posReduced & LzmaCodecConstants.AlignMask);
    }

    private void Flush(uint nowPos)
    {
        ReleaseMFStream();
        WriteEndMarker(nowPos & posStateMask);
        rangeEncoder.FlushData();
        rangeEncoder.FlushStream();
    }

    public void CodeOneBlock(out long inSize, out long outSize, out bool finished)
    {
        inSize = 0;
        outSize = 0;
        finished = true;

        if (this.finished)
            return;
        this.finished = true;


        var progressPosValuePrev = nowPos64;
        if (nowPos64 == 0)
        {
            if (matchFinder.GetNumAvailableBytes() == 0)
            {
                Flush((uint)nowPos64);
                return;
            }

            uint len, numDistancePairs; // it's not used
            ReadMatchDistances(out len, out numDistancePairs);
            var posState = (uint)nowPos64 & posStateMask;
            isMatch[(state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState].Encode(rangeEncoder, 0);
            state.UpdateChar();
            var curByte = matchFinder.GetIndexByte((int)(0 - additionalOffset));
            literalEncoder.GetSubCoder((uint)nowPos64, previousByte).Encode(rangeEncoder, curByte);
            previousByte = curByte;
            additionalOffset--;
            nowPos64++;
        }

        if (matchFinder.GetNumAvailableBytes() == 0)
        {
            Flush((uint)nowPos64);
            return;
        }

        while (true)
        {
            uint pos;
            var len = GetOptimum((uint)nowPos64, out pos);

            var posState = (uint)nowPos64 & posStateMask;
            var complexState = (state.Index << LzmaCodecConstants.NumPosStatesBitsMax) + posState;
            if (len == 1 && pos == 0xFFFFFFFF)
            {
                isMatch[complexState].Encode(rangeEncoder, 0);
                var curByte = matchFinder.GetIndexByte((int)(0 - additionalOffset));
                var subCoder = literalEncoder.GetSubCoder((uint)nowPos64, previousByte);
                if (!state.IsCharState())
                {
                    var matchByte = matchFinder.GetIndexByte((int)(0 - repDistances[0] - 1 - additionalOffset));
                    subCoder.EncodeMatched(rangeEncoder, matchByte, curByte);
                }
                else
                {
                    subCoder.Encode(rangeEncoder, curByte);
                }

                previousByte = curByte;
                state.UpdateChar();
            }
            else
            {
                isMatch[complexState].Encode(rangeEncoder, 1);
                if (pos < LzmaCodecConstants.NumRepDistances)
                {
                    isRep[state.Index].Encode(rangeEncoder, 1);
                    if (pos == 0)
                    {
                        isRepG0[state.Index].Encode(rangeEncoder, 0);
                        if (len == 1)
                            isRep0Long[complexState].Encode(rangeEncoder, 0);
                        else
                            isRep0Long[complexState].Encode(rangeEncoder, 1);
                    }
                    else
                    {
                        isRepG0[state.Index].Encode(rangeEncoder, 1);
                        if (pos == 1)
                        {
                            isRepG1[state.Index].Encode(rangeEncoder, 0);
                        }
                        else
                        {
                            isRepG1[state.Index].Encode(rangeEncoder, 1);
                            isRepG2[state.Index].Encode(rangeEncoder, pos - 2);
                        }
                    }

                    if (len == 1)
                    {
                        state.UpdateShortRep();
                    }
                    else
                    {
                        repMatchLenEncoder.Encode(rangeEncoder, len - LzmaCodecConstants.MatchMinLen, posState);
                        state.UpdateRep();
                    }

                    var distance = repDistances[pos];
                    if (pos != 0)
                    {
                        for (var i = pos; i >= 1; i--)
                            repDistances[i] = repDistances[i - 1];
                        repDistances[0] = distance;
                    }
                }
                else
                {
                    isRep[state.Index].Encode(rangeEncoder, 0);
                    state.UpdateMatch();
                    lenEncoder.Encode(rangeEncoder, len - LzmaCodecConstants.MatchMinLen, posState);
                    pos -= LzmaCodecConstants.NumRepDistances;
                    var posSlot = GetPosSlot(pos);
                    var lenToPosState = LzmaCodecConstants.GetLenToPosState(len);
                    posSlotEncoder[lenToPosState].Encode(rangeEncoder, posSlot);

                    if (posSlot >= LzmaCodecConstants.StartPosModelIndex)
                    {
                        var footerBits = (int)((posSlot >> 1) - 1);
                        var baseVal = (2 | posSlot & 1) << footerBits;
                        var posReduced = pos - baseVal;

                        if (posSlot < LzmaCodecConstants.EndPosModelIndex)
                        {
                            BitTreeEncoder.ReverseEncode(posEncoders,
                                baseVal - posSlot - 1, rangeEncoder, footerBits, posReduced);
                        }
                        else
                        {
                            rangeEncoder.EncodeDirectBits(posReduced >> LzmaCodecConstants.NumAlignBits, footerBits - LzmaCodecConstants.NumAlignBits);
                            posAlignEncoder.ReverseEncode(rangeEncoder, posReduced & LzmaCodecConstants.AlignMask);
                            alignPriceCount++;
                        }
                    }

                    var distance = pos;
                    for (var i = LzmaCodecConstants.NumRepDistances - 1; i >= 1; i--)
                        repDistances[i] = repDistances[i - 1];
                    repDistances[0] = distance;
                    matchPriceCount++;
                }

                previousByte = matchFinder.GetIndexByte((int)(len - 1 - additionalOffset));
            }

            additionalOffset -= len;
            nowPos64 += len;
            if (additionalOffset == 0)
            {
                // if (!_fastMode)
                if (matchPriceCount >= 1 << 7)
                    FillDistancesPrices();
                if (alignPriceCount >= LzmaCodecConstants.AlignTableSize)
                    FillAlignPrices();
                inSize = nowPos64;
                outSize = rangeEncoder.GetProcessedSizeAdd();
                if (matchFinder.GetNumAvailableBytes() == 0)
                {
                    Flush((uint)nowPos64);
                    return;
                }

                if (nowPos64 - progressPosValuePrev >= 1 << 12)
                {
                    this.finished = false;
                    finished = false;
                    return;
                }
            }
        }
    }

    private void ReleaseMFStream()
    {
        if (matchFinder != null && needReleaseMFStream)
        {
            matchFinder.ReleaseStream();
            needReleaseMFStream = false;
        }
    }

    private void SetOutStream(IStream outStream)
    {
        rangeEncoder.SetStream(outStream);
    }

    private void ReleaseOutStream()
    {
        rangeEncoder.ReleaseStream();
    }

    private void ReleaseStreams()
    {
        ReleaseMFStream();
        ReleaseOutStream();
    }

    private void SetStreams(IStream inStream, IStream outStream)
    {
        finished = false;
        Create(inStream);
        SetOutStream(outStream);
        Init();

        // if (!_fastMode)
        {
            FillDistancesPrices();
            FillAlignPrices();
        }

        lenEncoder.SetTableSize(numFastBytes + 1 - LzmaCodecConstants.MatchMinLen);
        lenEncoder.UpdateTables((uint)1 << posStateBits);
        repMatchLenEncoder.SetTableSize(numFastBytes + 1 - LzmaCodecConstants.MatchMinLen);
        repMatchLenEncoder.UpdateTables((uint)1 << posStateBits);

        nowPos64 = 0;
    }

    private void FillDistancesPrices()
    {
        for (var i = LzmaCodecConstants.StartPosModelIndex; i < LzmaCodecConstants.NumFullDistances; i++)
        {
            var posSlot = GetPosSlot(i);
            var footerBits = (int)((posSlot >> 1) - 1);
            var baseVal = (2 | posSlot & 1) << footerBits;
            tempPrices[i] = BitTreeEncoder.ReverseGetPrice(posEncoders,
                baseVal - posSlot - 1, footerBits, i - baseVal);
        }

        for (uint lenToPosState = 0; lenToPosState < LzmaCodecConstants.NumLenToPosStates; lenToPosState++)
        {
            uint posSlot;
            var encoder = posSlotEncoder[lenToPosState];

            var st = lenToPosState << LzmaCodecConstants.NumPosSlotBits;
            for (posSlot = 0; posSlot < distTableSize; posSlot++)
                posSlotPrices[st + posSlot] = encoder.GetPrice(posSlot);
            for (posSlot = LzmaCodecConstants.EndPosModelIndex; posSlot < distTableSize; posSlot++)
                posSlotPrices[st + posSlot] += (posSlot >> 1) - 1 - LzmaCodecConstants.NumAlignBits << BitEncoder.NumBitPriceShiftBits;

            var st2 = lenToPosState * LzmaCodecConstants.NumFullDistances;
            uint i;
            for (i = 0; i < LzmaCodecConstants.StartPosModelIndex; i++)
                distancesPrices[st2 + i] = posSlotPrices[st + i];
            for (; i < LzmaCodecConstants.NumFullDistances; i++)
                distancesPrices[st2 + i] = posSlotPrices[st + GetPosSlot(i)] + tempPrices[i];
        }

        matchPriceCount = 0;
    }

    private void FillAlignPrices()
    {
        for (uint i = 0; i < LzmaCodecConstants.AlignTableSize; i++)
            alignPrices[i] = posAlignEncoder.ReverseGetPrice(i);
        alignPriceCount = 0;
    }

    private static MatchFinderType? FindMatchFinder(string s)
    {
        foreach (var finderType in Enum.GetValues<MatchFinderType>())
        {
            if (s == finderType.ToString())
                return finderType;
        }
        return null;
    }

    public void SetTrainSize(uint trainSize)
    {
        this.trainSize = trainSize;
    }


    private class LiteralEncoder
    {
        private Encoder2[]? coders;
        private int numPosBits;
        private int numPrevBits;
        private uint posMask;

        public void Create(int numPosBits, int numPrevBits)
        {
            if (coders != null && this.numPrevBits == numPrevBits && this.numPosBits == numPosBits)
                return;
            this.numPosBits = numPosBits;
            posMask = ((uint)1 << numPosBits) - 1;
            this.numPrevBits = numPrevBits;
            var numStates = (uint)1 << this.numPrevBits + this.numPosBits;
            coders = new Encoder2[numStates];
            for (uint i = 0; i < numStates; i++)
                coders[i].Create();
        }

        public void Init()
        {
            var numStates = (uint)1 << numPrevBits + numPosBits;
            for (uint i = 0; i < numStates; i++)
                coders![i].Init();
        }

        public Encoder2 GetSubCoder(uint pos, byte prevByte) =>
            coders![((pos & posMask) << numPrevBits) + (uint)(prevByte >> 8 - numPrevBits)];

        public struct Encoder2
        {
            private BitEncoder[] encoders;

            public void Create()
            {
                encoders = new BitEncoder[0x300];
            }

            public void Init()
            {
                for (var i = 0; i < 0x300; i++) encoders[i].Init();
            }

            public void Encode(RangeEncoder rangeEncoder, byte symbol)
            {
                uint context = 1;
                for (var i = 7; i >= 0; i--)
                {
                    var bit = (uint)(symbol >> i & 1);
                    encoders[context].Encode(rangeEncoder, bit);
                    context = context << 1 | bit;
                }
            }

            public void EncodeMatched(RangeEncoder rangeEncoder, byte matchByte
                , byte symbol)
            {
                uint context = 1;
                var same = true;
                for (var i = 7; i >= 0; i--)
                {
                    var bit = (uint)(symbol >> i & 1);
                    var state = context;
                    if (same)
                    {
                        var matchBit = (uint)(matchByte >> i & 1);
                        state += 1 + matchBit << 8;
                        same = matchBit == bit;
                    }

                    encoders[state].Encode(rangeEncoder, bit);
                    context = context << 1 | bit;
                }
            }

            public uint GetPrice(bool matchMode, byte matchByte, byte symbol)
            {
                uint price = 0;
                uint context = 1;
                var i = 7;
                if (matchMode)
                    for (; i >= 0; i--)
                    {
                        var matchBit = (uint)(matchByte >> i) & 1;
                        var bit = (uint)(symbol >> i) & 1;
                        price += encoders[(1 + matchBit << 8) + context].GetPrice(bit);
                        context = context << 1 | bit;
                        if (matchBit != bit)
                        {
                            i--;
                            break;
                        }
                    }

                for (; i >= 0; i--)
                {
                    var bit = (uint)(symbol >> i) & 1;
                    price += encoders[context].GetPrice(bit);
                    context = context << 1 | bit;
                }

                return price;
            }
        }
    }

    private class LenEncoder
    {
        private BitEncoder choice = new();
        private BitEncoder choice2 = new();
        private BitTreeEncoder highCoder = new(LzmaCodecConstants.NumHighLenBits);
        private BitTreeEncoder[] lowCoder = new BitTreeEncoder[LzmaCodecConstants.NumPosStatesEncodingMax];
        private BitTreeEncoder[] midCoder = new BitTreeEncoder[LzmaCodecConstants.NumPosStatesEncodingMax];

        public LenEncoder()
        {
            for (uint posState = 0; posState < LzmaCodecConstants.NumPosStatesEncodingMax; posState++)
            {
                lowCoder[posState] = new BitTreeEncoder(LzmaCodecConstants.NumLowLenBits);
                midCoder[posState] = new BitTreeEncoder(LzmaCodecConstants.NumMidLenBits);
            }
        }

        public void Init(uint numPosStates)
        {
            choice.Init();
            choice2.Init();
            for (uint posState = 0; posState < numPosStates; posState++)
            {
                lowCoder[posState].Init();
                midCoder[posState].Init();
            }

            highCoder.Init();
        }

        public void Encode(RangeEncoder rangeEncoder, uint symbol
            , uint posState)
        {
            if (symbol < LzmaCodecConstants.NumLowLenSymbols)
            {
                choice.Encode(rangeEncoder, 0);
                lowCoder[posState].Encode(rangeEncoder, symbol);
            }
            else
            {
                symbol -= LzmaCodecConstants.NumLowLenSymbols;
                choice.Encode(rangeEncoder, 1);
                if (symbol < LzmaCodecConstants.NumMidLenSymbols)
                {
                    choice2.Encode(rangeEncoder, 0);
                    midCoder[posState].Encode(rangeEncoder, symbol);
                }
                else
                {
                    choice2.Encode(rangeEncoder, 1);
                    highCoder.Encode(rangeEncoder, symbol - LzmaCodecConstants.NumMidLenSymbols);
                }
            }
        }

        public void SetPrices(uint posState, uint numSymbols, uint[] prices, uint st)
        {
            var a0 = choice.GetPrice0();
            var a1 = choice.GetPrice1();
            var b0 = a1 + choice2.GetPrice0();
            var b1 = a1 + choice2.GetPrice1();
            uint i = 0;
            for (i = 0; i < LzmaCodecConstants.NumLowLenSymbols; i++)
            {
                if (i >= numSymbols)
                    return;
                prices[st + i] = a0 + lowCoder[posState].GetPrice(i);
            }

            for (; i < LzmaCodecConstants.NumLowLenSymbols + LzmaCodecConstants.NumMidLenSymbols; i++)
            {
                if (i >= numSymbols)
                    return;
                prices[st + i] = b0 + midCoder[posState].GetPrice(i - LzmaCodecConstants.NumLowLenSymbols);
            }

            for (; i < numSymbols; i++)
                prices[st + i] = b1 + highCoder.GetPrice(i - LzmaCodecConstants.NumLowLenSymbols - LzmaCodecConstants.NumMidLenSymbols);
        }
    };

    private class LenPriceTableEncoder : LenEncoder
    {
        private uint[] counters = new uint[LzmaCodecConstants.NumPosStatesEncodingMax];
        private uint[] prices = new uint[LzmaCodecConstants.NumLenSymbols << LzmaCodecConstants.NumPosStatesBitsEncodingMax];
        private uint tableSize;

        public void SetTableSize(uint tableSize)
        {
            this.tableSize = tableSize;
        }

        public uint GetPrice(uint symbol, uint posState) => prices[posState * LzmaCodecConstants.NumLenSymbols + symbol];

        private void UpdateTable(uint posState)
        {
            SetPrices(posState, tableSize, prices, posState * LzmaCodecConstants.NumLenSymbols);
            counters[posState] = tableSize;
        }

        public void UpdateTables(uint numPosStates)
        {
            for (uint posState = 0; posState < numPosStates; posState++)
                UpdateTable(posState);
        }

        public new void Encode(RangeEncoder rangeEncoder, uint symbol
            , uint posState)
        {
            base.Encode(rangeEncoder, symbol, posState);
            if (--counters[posState] == 0)
                UpdateTable(posState);
        }
    }

    private class Optimal
    {
        public uint BackPrev;
        public uint BackPrev2;

        public uint Backs0;
        public uint Backs1;
        public uint Backs2;
        public uint Backs3;
        public uint PosPrev;

        public uint PosPrev2;

        public bool Prev1IsChar;
        public bool Prev2;

        public uint Price;
        public LzmaCodecConstants.State State;

        public void MakeAsChar()
        {
            BackPrev = 0xFFFFFFFF;
            Prev1IsChar = false;
        }

        public void MakeAsShortRep()
        {
            BackPrev = 0;
            Prev1IsChar = false;
        }

        public bool IsShortRep() => BackPrev == 0;
    }
}
