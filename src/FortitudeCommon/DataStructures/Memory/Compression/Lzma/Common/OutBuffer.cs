// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Common;

public class OutBuffer
{
    private byte[]  buffer;
    private uint    bufferSize;
    private uint    pos;
    private ulong   processedSize;
    private Stream? stream;

    public OutBuffer(uint bufferSize)
    {
        buffer        = new byte[bufferSize];
        this.bufferSize = bufferSize;
    }

    public void SetStream(Stream stream)
    {
        this.stream = stream;
    }

    public void FlushStream()
    {
        stream?.Flush();
    }

    public void CloseStream()
    {
        stream?.Close();
    }

    public void ReleaseStream()
    {
        stream = null;
    }

    public void Init()
    {
        processedSize = 0;
        pos = 0;
    }

    public void WriteByte(byte b)
    {
        buffer[pos++] = b;
        if (pos >= bufferSize)
            FlushData();
    }

    public void FlushData()
    {
        if (pos == 0)
            return;
        stream!.Write(buffer, 0, (int)pos);
        pos = 0;
    }

    public ulong GetProcessedSize() => processedSize + pos;
}
