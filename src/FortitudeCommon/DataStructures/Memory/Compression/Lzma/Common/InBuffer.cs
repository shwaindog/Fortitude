// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Common;

public class InBuffer
{
    private byte[]  buffer;
    private uint    bufferSize;
    private uint    limit;
    private uint    pos;
    private ulong   processedSize;
    private Stream? stream;
    private bool    streamWasExhausted;

    public InBuffer(uint bufferSize)
    {
        buffer        = new byte[bufferSize];
        this.bufferSize = bufferSize;
    }

    public void Init(Stream stream)
    {
        this.stream = stream;
        processedSize = 0;
        limit = 0;
        pos = 0;
        streamWasExhausted = false;
    }

    public bool ReadBlock()
    {
        if (streamWasExhausted)
            return false;
        processedSize += pos;
        var aNumProcessedBytes = stream!.Read(buffer, 0, (int)bufferSize);
        pos = 0;
        limit = (uint)aNumProcessedBytes;
        streamWasExhausted = aNumProcessedBytes == 0;
        return !streamWasExhausted;
    }


    public void ReleaseStream()
    {
        // m_Stream.Close(); 
        stream = null;
    }

    public bool ReadByte(byte b) // check it
    {
        if (pos >= limit)
            if (!ReadBlock())
                return false;
        b = buffer[pos++];
        return true;
    }

    public byte ReadByte()
    {
        // return (byte)m_Stream.ReadByte();
        if (pos >= limit)
            if (!ReadBlock())
                return 0xFF;
        return buffer[pos++];
    }

    public ulong GetProcessedSize() => processedSize + pos;
}
