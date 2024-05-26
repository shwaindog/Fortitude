// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks


using FortitudeCommon.DataStructures.Memory.Compression.Lzma.Common;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.Lz;

public class BinTree : InWindow, IMatchFinder
{
    private const uint Hash2Size = 1 << 10;
    private const uint Hash3Size = 1 << 16;
    private const uint BT2HashSize = 1 << 16;
    private const uint StartMaxLen = 1;
    private const uint Hash3Offset = Hash2Size;
    private const uint EmptyHashValue = 0;
    private const uint MaxValForNormalize = ((uint)1 << 31) - 1;

    private uint    cutValue = 0xFF;
    private uint    cyclicBufferPos;
    private uint    cyclicBufferSize = 0;
    private uint[]? hash;
    private uint    hashMask;
    private uint    hashSizeSum = 0;
    private uint    matchMaxLen;

    private uint[]? son;

    private bool hashArray = true;
    private uint fixHashSize = Hash2Size + Hash3Size;
    private uint minMatchCheck = 4;

    private uint numHashDirectBytes = 0;

    public new void SetStream(Stream stream)
    {
        base.SetStream(stream);
    }

    public new void ReleaseStream()
    {
        base.ReleaseStream();
    }

    public new void Init()
    {
        base.Init();
        for (uint i = 0; i < hashSizeSum; i++)
            hash![i] = EmptyHashValue;
        cyclicBufferPos = 0;
        ReduceOffsets(-1);
    }

    public new byte GetIndexByte(int index) => base.GetIndexByte(index);

    public new uint GetMatchLen(int index, uint distance, uint limit) => base.GetMatchLen(index, distance, limit);

    public new uint GetNumAvailableBytes() => base.GetNumAvailableBytes();

    public void Create(uint historySize, uint keepAddBufferBefore,
        uint matchMaxLen, uint keepAddBufferAfter)
    {
        if (historySize > MaxValForNormalize - 256)
            throw new Exception();
        cutValue = 16 + (matchMaxLen >> 1);

        var windowReservSize = (historySize + keepAddBufferBefore +
                                matchMaxLen + keepAddBufferAfter) / 2 + 256;

        base.Create(historySize + keepAddBufferBefore, matchMaxLen + keepAddBufferAfter, windowReservSize);

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
        if (Pos + matchMaxLen <= StreamPos)
        {
            lenLimit = matchMaxLen;
        }
        else
        {
            lenLimit = StreamPos - Pos;
            if (lenLimit < minMatchCheck)
            {
                MovePos();
                return 0;
            }
        }

        uint offset = 0;
        var matchMinPos = Pos > cyclicBufferSize ? Pos - cyclicBufferSize : 0;
        var cur = BufferOffset + Pos;
        var maxLen = StartMaxLen; // to avoid items for len < hashSize;
        uint hashValue, hash2Value = 0, hash3Value = 0;

        if (hashArray)
        {
            var temp = CRC.Table[BufferBase[cur]] ^ BufferBase[cur + 1];
            hash2Value = temp & (Hash2Size - 1);
            temp ^= (uint)BufferBase[cur + 2] << 8;
            hash3Value = temp & (Hash3Size - 1);
            hashValue = (temp ^ (CRC.Table[BufferBase[cur + 3]] << 5)) & hashMask;
        }
        else
        {
            hashValue = BufferBase[cur] ^ ((uint)BufferBase[cur + 1] << 8);
        }

        var curMatch = hash![fixHashSize + hashValue];
        if (hashArray)
        {
            var curMatch2 = hash[hash2Value];
            var curMatch3 = hash[Hash3Offset + hash3Value];
            hash[hash2Value] = Pos;
            hash[Hash3Offset + hash3Value] = Pos;
            if (curMatch2 > matchMinPos)
                if (BufferBase[BufferOffset + curMatch2] == BufferBase[cur])
                {
                    distances[offset++] = maxLen = 2;
                    distances[offset++] = Pos - curMatch2 - 1;
                }

            if (curMatch3 > matchMinPos)
                if (BufferBase[BufferOffset + curMatch3] == BufferBase[cur])
                {
                    if (curMatch3 == curMatch2)
                        offset -= 2;
                    distances[offset++] = maxLen = 3;
                    distances[offset++] = Pos - curMatch3 - 1;
                    curMatch2 = curMatch3;
                }

            if (offset != 0 && curMatch2 == curMatch)
            {
                offset -= 2;
                maxLen = StartMaxLen;
            }
        }

        hash[fixHashSize + hashValue] = Pos;

        var ptr0 = (cyclicBufferPos << 1) + 1;
        var ptr1 = cyclicBufferPos << 1;

        uint len0, len1;
        len0 = len1 = numHashDirectBytes;

        if (numHashDirectBytes != 0)
            if (curMatch > matchMinPos)
                if (BufferBase[BufferOffset + curMatch + numHashDirectBytes] !=
                    BufferBase[cur + numHashDirectBytes])
                {
                    distances[offset++] = maxLen = numHashDirectBytes;
                    distances[offset++] = Pos - curMatch - 1;
                }

        var count = cutValue;

        while (true)
        {
            if (curMatch <= matchMinPos || count-- == 0)
            {
                son![ptr0] = son[ptr1] = EmptyHashValue;
                break;
            }

            var delta = Pos - curMatch;
            var cyclicPos = (delta <= cyclicBufferPos ?
                cyclicBufferPos - delta :
                cyclicBufferPos - delta + cyclicBufferSize) << 1;

            var pby1 = BufferOffset + curMatch;
            var len = Math.Min(len0, len1);
            if (BufferBase[pby1 + len] == BufferBase[cur + len])
            {
                while (++len != lenLimit)
                    if (BufferBase[pby1 + len] != BufferBase[cur + len])
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

            if (BufferBase[pby1 + len] < BufferBase[cur + len])
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

    public void Skip(uint num)
    {
        do
        {
            uint lenLimit;
            if (Pos + matchMaxLen <= StreamPos)
            {
                lenLimit = matchMaxLen;
            }
            else
            {
                lenLimit = StreamPos - Pos;
                if (lenLimit < minMatchCheck)
                {
                    MovePos();
                    continue;
                }
            }

            var matchMinPos = Pos > cyclicBufferSize ? Pos - cyclicBufferSize : 0;
            var cur = BufferOffset + Pos;

            uint hashValue;

            if (hashArray)
            {
                var temp = CRC.Table[BufferBase[cur]] ^ BufferBase[cur + 1];
                var hash2Value = temp & (Hash2Size - 1);
                hash![hash2Value] = Pos;
                temp ^= (uint)BufferBase[cur + 2] << 8;
                var hash3Value = temp & (Hash3Size - 1);
                hash[Hash3Offset + hash3Value] = Pos;
                hashValue = (temp ^ (CRC.Table[BufferBase[cur + 3]] << 5)) & hashMask;
            }
            else
            {
                hashValue = BufferBase[cur] ^ ((uint)BufferBase[cur + 1] << 8);
            }

            var curMatch = hash![fixHashSize + hashValue];
            hash[fixHashSize + hashValue] = Pos;

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

                var delta = Pos - curMatch;
                var cyclicPos = (delta <= cyclicBufferPos ?
                    cyclicBufferPos - delta :
                    cyclicBufferPos - delta + cyclicBufferSize) << 1;

                var pby1 = BufferOffset + curMatch;
                var len = Math.Min(len0, len1);
                if (BufferBase[pby1 + len] == BufferBase[cur + len])
                {
                    while (++len != lenLimit)
                        if (BufferBase[pby1 + len] != BufferBase[cur + len])
                            break;
                    if (len == lenLimit)
                    {
                        son![ptr1] = son[cyclicPos];
                        son[ptr0] = son[cyclicPos + 1];
                        break;
                    }
                }

                if (BufferBase[pby1 + len] < BufferBase[cur + len])
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
        } while (--num != 0);
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

    public new void MovePos()
    {
        if (++cyclicBufferPos >= cyclicBufferSize)
            cyclicBufferPos = 0;
        base.MovePos();
        if (Pos == MaxValForNormalize)
            Normalize();
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
        var subValue = Pos - cyclicBufferSize;
        NormalizeLinks(son!, cyclicBufferSize * 2, subValue);
        NormalizeLinks(hash!, hashSizeSum, subValue);
        ReduceOffsets((int)subValue);
    }

    public void SetCutValue(uint cutValue)
    {
        this.cutValue = cutValue;
    }
}
