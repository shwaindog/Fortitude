namespace FortitudeCommon.Serdes.Binary;

public unsafe interface IBuffer : IDisposable
{
    byte* ReadBuffer { get; }
    byte* WriteBuffer { get; }
    nint BufferRelativeReadCursor { get; }
    nint BufferRelativeWriteCursor { get; }
    long Size { get; }
    long ReadCursor { get; set; }
    bool AllRead { get; }
    long UnreadBytesRemaining { get; }
    long WriteCursor { get; set; }
    long RemainingStorage { get; }
    void SetAllRead();
    void TryHandleRemainingWriteBufferRunningLow();
    bool HasStorageForBytes(int bytes);
}
