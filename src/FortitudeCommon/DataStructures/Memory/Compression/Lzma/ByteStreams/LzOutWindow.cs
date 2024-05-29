// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory;
using FortitudeCommon.OSWrapper.Streams;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.ByteStreams;

public class OutWindow : IOutWindow
{
    private byte[] buffer = null!;
    private uint pos;
    private IStream? stream;
    private uint streamPos;
    private uint windowSize = 0;
    public uint TrainSize { get; private set; }

    public void Create(uint windowSize)
    {
        if (this.windowSize > 0 && this.windowSize != windowSize) return;
        // System.GC.Collect();
        buffer = new byte[windowSize];
        this.windowSize = windowSize;
        pos = 0;
        streamPos = 0;
    }

    public void Init(IStream stream, bool solid)
    {
        ReleaseStream();
        this.stream = stream;
        if (!solid)
        {
            streamPos = 0;
            pos = 0;
            TrainSize = 0;
        }
    }

    public bool Train(IStream stream)
    {
        var len = stream.Length;
        var size = len < windowSize ? (uint)len : windowSize;
        TrainSize = size;
        return true;
    }

    public bool Train(IByteArray stream)
    {
        var len = stream.Length;
        var size = len < windowSize ? (uint)len : windowSize;
        TrainSize = size;
        return true;
    }

    public void ReleaseStream()
    {
        Flush();
        stream = null!;
    }

    public void Flush()
    {
        var size = pos - streamPos;
        if (size == 0)
            return;
        stream!.Write(buffer, (int)streamPos, (int)size);
        if (pos >= windowSize)
            pos = 0;
        streamPos = pos;
    }

    public void CopyBlock(uint distance, uint len)
    {
        var pos = this.pos - distance - 1;
        if (pos >= windowSize)
            pos += windowSize;
        for (; len > 0; len--)
        {
            if (pos >= windowSize)
                pos = 0;
            buffer[this.pos++] = buffer[pos++];
            if (this.pos >= windowSize)
                Flush();
        }
    }

    public void PutByte(byte b)
    {
        buffer[pos++] = b;
        if (pos >= windowSize)
            Flush();
    }

    public byte GetByte(uint distance)
    {
        var pos = this.pos - distance - 1;
        if (pos >= windowSize)
            pos += windowSize;
        return buffer[pos];
    }
}
