namespace FortitudeCommon.Serdes.Binary;

public unsafe interface IBuffer : IDisposable
{
    byte* ReadBuffer { get; }
    byte* WriteBuffer { get; }
    nint BufferRelativeReadCursor { get; }
    nint BufferRelativeWriteCursor { get; }
    int Size { get; }
    nint ReadCursor { get; set; }
    bool AllRead { get; }
    nint UnreadBytesRemaining { get; }
    nint WriteCursor { get; set; }
    nint RemainingStorage { get; }
    void SetAllRead();
    bool CanWrite(int size);
    void TryHandleRemainingWriteBufferRunningLow();
    bool HasStorageForBytes(int bytes);
}
