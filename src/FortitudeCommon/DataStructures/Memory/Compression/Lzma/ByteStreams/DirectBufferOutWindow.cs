// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.OSWrapper.Streams;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.ByteStreams;

public class DirectBufferOutWindow : IOutWindow
{
    private IByteArray buffer = null!;
    private long pos;
    private IStream? trainOnlyStream;
    private long windowSize = 0;

    public uint TrainSize { get; private set; }

    public void Create(uint windowSize)
    {
        if (windowSize > this.windowSize)
        {
            throw new ArgumentException("Expected output buffer to be bigger than dictionaryWindow Size");
        }
    }

    public DirectBufferOutWindow(IByteArray mappedBuffer)
    {
        // System.GC.Collect();
        buffer = mappedBuffer;
        windowSize = mappedBuffer.Length;

        pos = 0;
    }

    public void Init(IStream stream, bool solid)
    {
        ReleaseStream();
        trainOnlyStream = stream;
        if (!solid)
        {
            pos = 0;
            TrainSize = 0;
        }
    }

    public bool Train(IStream stream)
    {
        var len = stream.Length;
        var size = len < windowSize ? (uint)len : windowSize;
        TrainSize = (uint)size;
        return true;
    }

    int ReadByteRange(long pos, long curSize)
    {
        var numRead = 0;
        if (trainOnlyStream is IAcceptsByteArrayStream readIntoByteArray)
        {
            numRead = readIntoByteArray.Read(buffer, (int)pos, (int)curSize);
        }
        else
        {
            var cappedSize = Math.Min((int)curSize, trainOnlyStream!.Length - trainOnlyStream.Position);
            var readBytes = new byte[byte.MaxValue];
            var readSoFar = 0;
            for (; readSoFar < cappedSize;)
            {
                var amountToRead = Math.Min(cappedSize - readSoFar, byte.MaxValue);
                var bytesRead = trainOnlyStream.Read(readBytes, 0, (int)amountToRead);
                for (int j = 0; j < bytesRead; j++)
                {
                    buffer[readSoFar + j] = readBytes[j];
                }
                readSoFar += bytesRead;
            }
            numRead = readSoFar;
        }
        return numRead;
    }

    public void ReleaseStream()
    {
        buffer.Flush();
        trainOnlyStream = null!;
    }

    public void Flush()
    {
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
                throw new Exception("Buffer should never be smaller than the expected output");
        }
    }

    public void PutByte(byte b)
    {
        buffer[pos++] = b;
        if (pos >= windowSize)
            throw new Exception("Buffer should never be smaller than the expected output");
    }

    public byte GetByte(uint distance)
    {
        var pos = this.pos - distance - 1;
        if (pos >= windowSize)
            pos += windowSize;
        return buffer[pos];
    }
}
