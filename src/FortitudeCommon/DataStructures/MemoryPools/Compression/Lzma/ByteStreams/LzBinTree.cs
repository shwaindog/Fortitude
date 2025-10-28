// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks


using FortitudeCommon.DataStructures.MemoryPools.Compression.Lzma.Common;
using FortitudeCommon.OSWrapper.Streams;

namespace FortitudeCommon.DataStructures.MemoryPools.Compression.Lzma.ByteStreams;

public class BinTree : IMatchFinder
{
    private readonly IInWindow sourceWindow;

    private const uint Hash2Size = 1 << 10;
    private const uint Hash3Size = 1 << 16;
    private const uint BT2HashSize = 1 << 16;
    private const uint StartMaxLen = 1;
    private const uint Hash3Offset = Hash2Size;
    private const uint EmptyHashValue = 0;
    private const uint MaxValForNormalize = ((uint)1 << 31) - 1;

    private uint cutValue = 0xFF;
    private uint cyclicBufferPos;
    private uint cyclicBufferSize = 0;
    private uint[]? hash;
    private uint hashMask;
    private uint hashSizeSum = 0;
    private uint matchMaxLen;

    private uint[]? son;

    private bool hashArray = true;
    private uint fixHashSize = Hash2Size + Hash3Size;
    private uint minMatchCheck = 4;

    private uint numHashDirectBytes = 0;

    public BinTree(IInWindow? sourceWindow = null)
    {
        this.sourceWindow = sourceWindow ?? new InWindow();
    }

    public void SetStream(IStream stream)
    {
        sourceWindow.SetStream(stream);
    }

    public void ReleaseStream()
    {
        sourceWindow.ReleaseStream();
    }

    public void Init()
    {
        sourceWindow.Init();
        for (uint i = 0; i < hashSizeSum; i++)
            hash![i] = EmptyHashValue;
        cyclicBufferPos = 0;
        sourceWindow.ReduceOffsets(-1);
    }

    public byte GetIndexByte(int index) => sourceWindow.GetIndexByte(index);

    public uint GetMatchLen(int index, uint distance, uint limit) => sourceWindow.GetMatchLen(index, distance, limit);

    public uint GetNumAvailableBytes() => sourceWindow.GetNumAvailableBytes();

    public void Create(uint historySize, uint keepAddBufferBefore,
        uint matchMaxLen, uint keepAddBufferAfter)
    {
        if (historySize > MaxValForNormalize - 256)
            throw new Exception();
        cutValue = 16 + (matchMaxLen >> 1);

        var windowReservSize = (historySize + keepAddBufferBefore +
                                matchMaxLen + keepAddBufferAfter) / 2 + 256;

        sourceWindow.Create(historySize + keepAddBufferBefore, matchMaxLen + keepAddBufferAfter, windowReservSize);

        this.matchMaxLen = matchMaxLen;

        var cyclicBufferSize = historySize + 1;
        if (this.cyclicBufferSize != cyclicBufferSize)
            son = new uint[(this.cyclicBufferSize = cyclicBufferSize) * 2];

        var hs = BT2HashSize;

        if (hashArray)
        {
            hs = historySize - 1;
            hs |= hs >> 1;
            hs |= hs >> 2;
            hs |= hs >> 4;
            hs |= hs >> 8;
            hs >>= 1;
            hs |= 0xFFFF;
            if (hs > 1 << 24)
                hs >>= 1;
            hashMask = hs;
            hs++;
            hs += fixHashSize;
        }

        if (hs != hashSizeSum)
            hash = new uint[hashSizeSum = hs];
    }

    public uint GetMatches(uint[] distances)
    {
        uint lenLimit;
        if (sourceWindow.Pos + matchMaxLen <= sourceWindow.StreamPos)
        {
            lenLimit = matchMaxLen;
        }
        else
        {
            lenLimit = sourceWindow.StreamPos - sourceWindow.Pos;
            if (lenLimit < minMatchCheck)
            {
                MovePos();
                return 0;
            }
        }

        uint offset = 0;
        var matchMinPos = sourceWindow.Pos > cyclicBufferSize ? sourceWindow.Pos - cyclicBufferSize : 0;
        var cur = sourceWindow.BufferOffset + sourceWindow.Pos;
        var maxLen = StartMaxLen; // to avoid items for len < hashSize;
        uint hashValue, hash2Value = 0, hash3Value = 0;

        if (hashArray)
        {
            var temp = CRC.Table[sourceWindow.BufferBase[cur]] ^ sourceWindow.BufferBase[cur + 1];
            hash2Value = temp & Hash2Size - 1;
            temp ^= (uint)sourceWindow.BufferBase[cur + 2] << 8;
            hash3Value = temp & Hash3Size - 1;
            hashValue = (temp ^ CRC.Table[sourceWindow.BufferBase[cur + 3]] << 5) & hashMask;
        }
        else
        {
            hashValue = sourceWindow.BufferBase[cur] ^ (uint)sourceWindow.BufferBase[cur + 1] << 8;
        }

        var curMatch = hash![fixHashSize + hashValue];
        if (hashArray)
        {
            var curMatch2 = hash[hash2Value];
            var curMatch3 = hash[Hash3Offset + hash3Value];
            hash[hash2Value] = sourceWindow.Pos;
            hash[Hash3Offset + hash3Value] = sourceWindow.Pos;
            if (curMatch2 > matchMinPos)
                if (sourceWindow.BufferBase[sourceWindow.BufferOffset + curMatch2] == sourceWindow.BufferBase[cur])
                {
                    distances[offset++] = maxLen = 2;
                    distances[offset++] = sourceWindow.Pos - curMatch2 - 1;
                }

            if (curMatch3 > matchMinPos)
                if (sourceWindow.BufferBase[sourceWindow.BufferOffset + curMatch3] == sourceWindow.BufferBase[cur])
                {
                    if (curMatch3 == curMatch2)
                        offset -= 2;
                    distances[offset++] = maxLen = 3;
                    distances[offset++] = sourceWindow.Pos - curMatch3 - 1;
                    curMatch2 = curMatch3;
                }

            if (offset != 0 && curMatch2 == curMatch)
            {
                offset -= 2;
                maxLen = StartMaxLen;
            }
        }

        hash[fixHashSize + hashValue] = sourceWindow.Pos;

        var ptr0 = (cyclicBufferPos << 1) + 1;
        var ptr1 = cyclicBufferPos << 1;

        uint len0, len1;
        len0 = len1 = numHashDirectBytes;

        if (numHashDirectBytes != 0)
            if (curMatch > matchMinPos)
                if (sourceWindow.BufferBase[sourceWindow.BufferOffset + curMatch + numHashDirectBytes] !=
                    sourceWindow.BufferBase[cur + numHashDirectBytes])
                {
                    distances[offset++] = maxLen = numHashDirectBytes;
                    distances[offset++] = sourceWindow.Pos - curMatch - 1;
                }

        var count = cutValue;

        while (true)
        {
            if (curMatch <= matchMinPos || count-- == 0)
            {
                son![ptr0] = son[ptr1] = EmptyHashValue;
                break;
            }

            var delta = sourceWindow.Pos - curMatch;
            var cyclicPos = (delta <= cyclicBufferPos ?
                cyclicBufferPos - delta :
                cyclicBufferPos - delta + cyclicBufferSize) << 1;

            var pby1 = sourceWindow.BufferOffset + curMatch;
            var len = Math.Min(len0, len1);
            if (sourceWindow.BufferBase[pby1 + len] == sourceWindow.BufferBase[cur + len])
            {
                while (++len != lenLimit)
                    if (sourceWindow.BufferBase[pby1 + len] != sourceWindow.BufferBase[cur + len])
                        break;
                if (maxLen < len)
                {
                    distances[offset++] = maxLen = len;
                    distances[offset++] = delta - 1;
                    if (len == lenLimit)
                    {
                        son![ptr1] = son[cyclicPos];
                        son[ptr0] = son[cyclicPos + 1];
                        break;
                    }
                }
            }

            if (sourceWindow.BufferBase[pby1 + len] < sourceWindow.BufferBase[cur + len])
            {
                son![ptr1] = curMatch;
                ptr1 = cyclicPos + 1;
                curMatch = son[ptr1];
                len1 = len;
            }
            else
            {
                son![ptr0] = curMatch;
                ptr0 = cyclicPos;
                curMatch = son[ptr0];
                len0 = len;
            }
        }

        MovePos();
        return offset;
    }

    public void Process(uint num, IInWindow? contextInput = null)
    {
        contextInput ??= sourceWindow;
        do
        {
            uint lenLimit;
            if (contextInput.Pos + matchMaxLen <= contextInput.StreamPos)
            {
                lenLimit = matchMaxLen;
            }
            else
            {
                lenLimit = contextInput.StreamPos - contextInput.Pos;
                if (lenLimit < minMatchCheck)
                {
                    MovePos(contextInput);
                    continue;
                }
            }

            var matchMinPos = contextInput.Pos > cyclicBufferSize ? contextInput.Pos - cyclicBufferSize : 0;
            var cur = contextInput.BufferOffset + contextInput.Pos;

            uint hashValue;

            if (hashArray)
            {
                var temp = CRC.Table[contextInput.BufferBase[cur]] ^ contextInput.BufferBase[cur + 1];
                var hash2Value = temp & Hash2Size - 1;
                hash![hash2Value] = contextInput.Pos;

                temp ^= (uint)contextInput.BufferBase[cur + 2] << 8;

                var hash3Value = temp & Hash3Size - 1;
                hash[Hash3Offset + hash3Value] = contextInput.Pos;

                hashValue = (temp ^ CRC.Table[contextInput.BufferBase[cur + 3]] << 5) & hashMask;
            }
            else
            {
                hashValue = contextInput.BufferBase[cur] ^ (uint)contextInput.BufferBase[cur + 1] << 8;
            }

            var curMatch = hash![fixHashSize + hashValue];
            hash[fixHashSize + hashValue] = contextInput.Pos;

            var ptr0 = (cyclicBufferPos << 1) + 1;
            var ptr1 = cyclicBufferPos << 1;

            uint len0, len1;
            len0 = len1 = numHashDirectBytes;

            var count = cutValue;
            while (true)
            {
                if (curMatch <= matchMinPos || count-- == 0)
                {
                    son![ptr0] = son[ptr1] = EmptyHashValue;
                    break;
                }

                var delta = contextInput.Pos - curMatch;
                var cyclicPos = (delta <= cyclicBufferPos ?
                    cyclicBufferPos - delta :
                    cyclicBufferPos - delta + cyclicBufferSize) << 1;

                var pby1 = contextInput.BufferOffset + curMatch;
                var len = Math.Min(len0, len1);
                if (contextInput.BufferBase[pby1 + len] == contextInput.BufferBase[cur + len])
                {
                    while (++len != lenLimit)
                        if (contextInput.BufferBase[pby1 + len] != contextInput.BufferBase[cur + len])
                            break;
                    if (len == lenLimit)
                    {
                        son![ptr1] = son[cyclicPos];
                        son[ptr0] = son[cyclicPos + 1];
                        break;
                    }
                }

                if (contextInput.BufferBase[pby1 + len] < contextInput.BufferBase[cur + len])
                {
                    son![ptr1] = curMatch;
                    ptr1 = cyclicPos + 1;
                    curMatch = son[ptr1];
                    len1 = len;
                }
                else
                {
                    son![ptr0] = curMatch;
                    ptr0 = cyclicPos;
                    curMatch = son[ptr0];
                    len0 = len;
                }
            }

            MovePos(contextInput);
        } while (--num > 0);
    }

    public void SetType(int numHashBytes)
    {
        hashArray = numHashBytes > 2;
        if (hashArray)
        {
            numHashDirectBytes = 0;
            minMatchCheck = 4;
            fixHashSize = Hash2Size + Hash3Size;
        }
        else
        {
            numHashDirectBytes = 2;
            minMatchCheck = 2 + 1;
            fixHashSize = 0;
        }
    }

    public void MovePos()
    {
        MovePos(sourceWindow);
    }

    private void MovePos(IInWindow input)
    {
        if (++cyclicBufferPos >= cyclicBufferSize)
            cyclicBufferPos = 0;
        input.MovePos();
        if (input.Pos == MaxValForNormalize)
            Normalize(input);
    }

    private void NormalizeLinks(uint[] items, uint numItems, uint subValue)
    {
        for (uint i = 0; i < numItems; i++)
        {
            var value = items[i];
            if (value <= subValue)
                value = EmptyHashValue;
            else
                value -= subValue;
            items[i] = value;
        }
    }

    private void Normalize()
    {
        Normalize(sourceWindow);
    }

    private void Normalize(IInWindow input)
    {
        var subValue = input.Pos - cyclicBufferSize;
        NormalizeLinks(son!, cyclicBufferSize * 2, subValue);
        NormalizeLinks(hash!, hashSizeSum, subValue);
        input.ReduceOffsets((int)subValue);
    }

    public void SetCutValue(uint cutValue)
    {
        this.cutValue = cutValue;
    }
}
