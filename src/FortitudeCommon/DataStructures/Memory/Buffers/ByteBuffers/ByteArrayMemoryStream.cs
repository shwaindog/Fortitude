// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.OSWrapper.Streams;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface IAcceptsByteArrayStream : IStream
{
    int  Read(IByteArray buffer, int offset, int count);
    void Write(IByteArray buffer, int offset, int count);
}

public class ByteArrayMemoryStream : Stream, IAcceptsByteArrayStream
{
    private readonly IByteArray byteArray;

    private readonly bool writable;

    public ByteArrayMemoryStream(IByteArray byteArray, bool writable, bool closeByteArrayOnDispose = true)
    {
        this.byteArray          = byteArray;
        this.writable           = writable;
        CloseByteArrayOnDispose = closeByteArrayOnDispose;
    }

    public bool CloseByteArrayOnDispose { get; set; }

    public override bool CanRead  => Position < byteArray.Length;
    public override bool CanSeek  => true;
    public override bool CanWrite => writable;
    public override long Length   => byteArray.Length;
    public override long Position { get; set; }

    public int Read(IByteArray buffer, int offset, int count)
    {
        var remainingBytes = byteArray.Length - Position;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        for (var i = offset; i < offset + cappedSize; i++) buffer[i] = byteArray[(int)Position++];
        return (int)cappedSize;
    }

    public void Write(IByteArray buffer, int offset, int count)
    {
        if (!writable) return;
        var remainingBytes = byteArray.Length - Position;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        for (var i = offset; i < offset + cappedSize; i++) byteArray[(int)Position++] = buffer[i];
    }

    public override void Flush()
    {
        byteArray.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var remainingBytes = byteArray.Length - Position;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        for (var i = offset; i < offset + cappedSize; i++) buffer[i] = byteArray[(int)Position++];
        return (int)cappedSize;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                if (offset > byteArray.Length || offset < 0)
                    throw new Exception("Attempted to seek beyond the end of the unmanaged buffer");
                Position = (int)offset;
                break;
            case SeekOrigin.End:
                if (offset > byteArray.Length || offset < 0)
                    throw new Exception("Attempted to seek beyond the end of the unmanaged buffer");
                Position = byteArray.Length - (int)offset;
                break;
            default:
                var proposedCursor = Position + (int)offset;
                if (proposedCursor > byteArray.Length || proposedCursor < 0)
                    throw new Exception("Attempted to seek beyond the end of the unmanaged buffer");
                Position = proposedCursor;
                break;
        }
        return Position;
    }

    public override void SetLength(long value)
    {
        byteArray.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!writable) return;
        var remainingBytes = byteArray.Length - Position;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        for (var i = offset; i < offset + cappedSize; i++) byteArray[(int)Position++] = buffer[i];
    }

    public override int ReadByte() => byteArray.Length - Position <= 0 ? -1 : byteArray[Position++];

    public override void WriteByte(byte value)
    {
        if (!writable || byteArray.Length - Position <= 0) return;
        byteArray[Position++] = value;
    }

    public override void Close()
    {
        if (CloseByteArrayOnDispose) byteArray.Dispose();
        base.Close();
    }

    public ByteArrayMemoryStream ReopenStream() => new(byteArray, writable, CloseByteArrayOnDispose);

    protected override void Dispose(bool disposing)
    {
        if (CloseByteArrayOnDispose && disposing) byteArray.Dispose();
        base.Dispose(disposing);
    }
}
