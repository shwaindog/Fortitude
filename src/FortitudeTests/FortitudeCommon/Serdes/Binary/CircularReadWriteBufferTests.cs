#region

using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeTests.FortitudeCommon.Serdes.Binary;

[TestClass]
public class CircularReadWriteBufferTests
{
    [TestMethod]
    public void ByteArray_New_SetsBufferAndDefaultsValues()
    {
        var byteArray = new byte[20];

        var readWriteBuffer = new CircularReadWriteBuffer(byteArray);

        Assert.AreSame(readWriteBuffer.Buffer, byteArray);
        Assert.AreEqual(0, readWriteBuffer.ReadCursor);
        Assert.AreEqual(0, readWriteBuffer.BufferRelativeReadCursor);
        Assert.AreEqual(0, readWriteBuffer.WriteCursor);
        Assert.AreEqual(0, readWriteBuffer.BufferRelativeWriteCursor);
        Assert.AreEqual(0, readWriteBuffer.UnreadBytesRemaining);
        Assert.IsTrue(readWriteBuffer.AllRead);
        Assert.AreEqual(20, readWriteBuffer.Size);
        Assert.AreEqual(20, readWriteBuffer.RemainingStorage);
        Assert.IsTrue(readWriteBuffer.HasStorageForBytes(20));
        Assert.IsFalse(readWriteBuffer.HasStorageForBytes(21));
    }

    [TestMethod]
    public void WrittenBuffer_RemainingStorage_ReturnsExpectedValue_ResetWrittenReadToZero()
    {
        var readWriteBuffer = new CircularReadWriteBuffer(new byte[20]) { WriteCursor = 15, ReadCursor = 5 };

        Assert.AreEqual(15, readWriteBuffer.WriteCursor);
        Assert.AreEqual(15, readWriteBuffer.BufferRelativeWriteCursor);
        Assert.AreEqual(5, readWriteBuffer.ReadCursor);
        Assert.AreEqual(5, readWriteBuffer.BufferRelativeReadCursor);
        Assert.AreEqual(5, readWriteBuffer.RemainingStorage);
        Assert.IsTrue(readWriteBuffer.HasStorageForBytes(5));
        Assert.IsFalse(readWriteBuffer.HasStorageForBytes(6));
        Assert.IsFalse(readWriteBuffer.AllRead);
        Assert.AreEqual(10, readWriteBuffer.UnreadBytesRemaining);

        readWriteBuffer.SetAllRead();
        Assert.AreEqual(15, readWriteBuffer.WriteCursor);
        Assert.AreEqual(0, readWriteBuffer.BufferRelativeWriteCursor);
        Assert.AreEqual(15, readWriteBuffer.ReadCursor);
        Assert.AreEqual(0, readWriteBuffer.BufferRelativeReadCursor);
        Assert.IsTrue(readWriteBuffer.AllRead);
        Assert.AreEqual(20, readWriteBuffer.RemainingStorage);
        Assert.IsTrue(readWriteBuffer.HasStorageForBytes(20));
        Assert.IsFalse(readWriteBuffer.HasStorageForBytes(21));
    }

    [TestMethod]
    public void WrittenBuffer_MoveUnreadToBufferStart_ResetWrittenReadToZero()
    {
        var readWriteBuffer = new CircularReadWriteBuffer(new byte[25]) { WriteCursor = 15, ReadCursor = 5 };

        for (byte i = 5; i < 25; i++) readWriteBuffer.Buffer[i] = i;

        readWriteBuffer.TryHandleRemainingWriteBufferRunningLow();

        Assert.AreEqual(5, readWriteBuffer.ReadCursor);
        Assert.AreEqual(0, readWriteBuffer.BufferRelativeReadCursor);
        Assert.AreEqual(15, readWriteBuffer.WriteCursor);
        Assert.AreEqual(10, readWriteBuffer.BufferRelativeWriteCursor);
        for (byte i = 0; i < 10; i++) Assert.AreEqual(i + 5, readWriteBuffer.Buffer[i]);

        readWriteBuffer.TryHandleRemainingWriteBufferRunningLow();
        Assert.AreEqual(5, readWriteBuffer.ReadCursor);
        Assert.AreEqual(0, readWriteBuffer.BufferRelativeReadCursor);
        Assert.AreEqual(15, readWriteBuffer.WriteCursor);
        Assert.AreEqual(10, readWriteBuffer.BufferRelativeWriteCursor);
        for (byte i = 0; i < 10; i++) Assert.AreEqual(i + 5, readWriteBuffer.Buffer[i]);

        readWriteBuffer.ReadCursor = readWriteBuffer.WriteCursor;
        readWriteBuffer.TryHandleRemainingWriteBufferRunningLow();
        Assert.AreEqual(15, readWriteBuffer.ReadCursor);
        Assert.AreEqual(0, readWriteBuffer.BufferRelativeReadCursor);
        Assert.AreEqual(15, readWriteBuffer.WriteCursor);
        Assert.AreEqual(0, readWriteBuffer.BufferRelativeWriteCursor);
        for (byte i = 0; i < 10; i++) Assert.AreEqual(i + 5, readWriteBuffer.Buffer[i]);
    }
}
