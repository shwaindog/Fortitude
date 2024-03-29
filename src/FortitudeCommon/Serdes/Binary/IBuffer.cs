namespace FortitudeCommon.Serdes.Binary;

public interface IBuffer
{
    public byte[] Buffer { get; }

    int Size { get; }
    int ReadCursor { get; set; }
    bool AllRead { get; }
    int UnreadBytesRemaining { get; }
    int WriteCursor { get; set; }
    int RemainingStorage { get; }
    void Reset();
    void MoveUnreadToBufferStart();
    bool HasStorageForBytes(int bytes);
}
