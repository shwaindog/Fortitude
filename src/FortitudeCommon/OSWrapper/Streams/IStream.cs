// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.OSWrapper.Streams;

public interface IStream : IDisposable
{
    bool CanRead  { get; }
    bool CanSeek  { get; }
    bool CanWrite { get; }
    long Length   { get; }
    long Position { get; set; }
    void Flush();

    int Read(byte[] buffer, int offset, int count);
    int Read(Span<byte> buffer);

    int ReadByte();

    long Seek(long offset, SeekOrigin origin);

    void SetLength(long value);

    void Write(byte[] buffer, int offset, int count);
    void Write(ReadOnlySpan<byte> buffer);

    void WriteByte(byte value);

    void Close();
}

public static class StreamExtensions
{
    public static IStream AsIStream(this Stream stream) => new StreamWrapper(stream);
}
