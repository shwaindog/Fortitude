namespace FortitudeCommon.Serdes.Binary;

public class ReadWriteBuffer : IBuffer
{
    public ReadWriteBuffer(byte[] buffer) => Buffer = buffer;

    public byte[] Buffer { get; }
    public int ReadCursor { get; set; }
    public int WriteCursor { get; set; }

    public bool AllRead => ReadCursor == WriteCursor;

    public int UnreadBytesRemaining => WriteCursor - ReadCursor;

    public void Reset()
    {
        ReadCursor = WriteCursor = 0;
    }

    public void MoveUnreadToBufferStart()
    {
        if (WriteCursor > 0 && ReadCursor > 0)
        {
            System.Buffer.BlockCopy(Buffer, ReadCursor, Buffer, 0,
                WriteCursor -= ReadCursor);
            ReadCursor = 0;
        }
    }

    public bool HasStorageForBytes(int bytes) => WriteCursor + bytes <= Buffer.Length;

    public int RemainingStorage => Buffer.Length - WriteCursor;

    public int Size => Buffer.Length;
}
