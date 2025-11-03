// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

namespace FortitudeCommon.DataStructures.MemoryPools.Compression.Lzma.Common;

internal class CRC
{
    public static readonly uint[] Table;

    private uint value = 0xFFFFFFFF;

    static CRC()
    {
        Table = new uint[256];
        const uint Poly = 0xEDB88320;
        for (uint i = 0; i < 256; i++)
        {
            var r = i;
            for (var j = 0; j < 8; j++)
                if ((r & 1) != 0)
                    r = (r >> 1) ^ Poly;
                else
                    r >>= 1;
            Table[i] = r;
        }
    }

    public void Init()
    {
        value = 0xFFFFFFFF;
    }

    public void UpdateByte(byte b)
    {
        value = Table[(byte)value ^ b] ^ (value >> 8);
    }

    public void Update(byte[] data, uint offset, uint size)
    {
        for (uint i = 0; i < size; i++)
            value = Table[(byte)value ^ data[offset + i]] ^ (value >> 8);
    }

    public uint GetDigest() => value ^ 0xFFFFFFFF;

    private static uint CalculateDigest(byte[] data, uint offset, uint size)
    {
        var crc = new CRC();
        // crc.Init();
        crc.Update(data, offset, size);
        return crc.GetDigest();
    }

    private static bool VerifyDigest(uint digest, byte[] data, uint offset, uint size) => CalculateDigest(data, offset, size) == digest;
}
