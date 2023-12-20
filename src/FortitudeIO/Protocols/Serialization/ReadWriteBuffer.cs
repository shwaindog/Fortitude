namespace FortitudeIO.Protocols.Serialization;

public class ReadWriteBuffer
{
    public ReadWriteBuffer(byte[] buffer) => Buffer = buffer;

    public byte[] Buffer { get; }
    public int ReadCursor { get; set; }
    public int WrittenCursor { get; set; }

    public bool AllRead => ReadCursor == WrittenCursor;

    public int UnreadBytesRemaining() => WrittenCursor - ReadCursor;

    public void Reset()
    {
        ReadCursor = WrittenCursor = 0;
    }

    public void MoveUnreadToBufferStart()
    {
        if (WrittenCursor > 0 && ReadCursor > 0)
        {
            System.Buffer.BlockCopy(Buffer, ReadCursor, Buffer, 0,
                WrittenCursor = WrittenCursor - ReadCursor);
            ReadCursor = 0;
        }
    }

    public bool HasStorageForBytes(int bytes) => WrittenCursor - 1 + bytes < Buffer.Length;

    public int RemainingStorage() => Buffer.Length - WrittenCursor;

    public int Size() => Buffer.Length;
}
