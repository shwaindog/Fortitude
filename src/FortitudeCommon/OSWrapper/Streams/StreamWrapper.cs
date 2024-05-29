// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.OSWrapper.Streams;

public class StreamWrapper : IStream
{
    private readonly Stream backingStream;

    public StreamWrapper(Stream backingStream) => this.backingStream = backingStream;

    public void Flush()
    {
        backingStream.Flush();
    }

    public int Read(byte[] buffer, int offset, int count) => backingStream.Read(buffer, offset, count);

    public int ReadByte() => backingStream.ReadByte();

    public long Seek(long offset, SeekOrigin origin) => backingStream.Seek(offset, origin);

    public void SetLength(long value)
    {
        backingStream.SetLength(value);
    }

    public void Write(byte[] buffer, int offset, int count)
    {
        backingStream.Write(buffer, offset, count);
    }

    public void WriteByte(byte value)
    {
        backingStream.WriteByte(value);
    }

    public bool CanRead  => backingStream.CanRead;
    public bool CanSeek  => backingStream.CanSeek;
    public bool CanWrite => backingStream.CanWrite;
    public long Length   => backingStream.Length;

    public long Position
    {
        get => backingStream.Position;
        set => backingStream.Position = value;
    }

    public void Dispose()
    {
        backingStream.Dispose();
    }

    public void Close()
    {
        backingStream.Close();
    }
}
