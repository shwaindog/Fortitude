#region

using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeTests.FortitudeCommon.Serdes.Binary;

[TestClass]
public class ReadWriteBufferTests
{
    [TestMethod]
    public void ByteArray_New_SetsBufferAndDefaultsValues()
    {
        var byteArray = new byte[20];

        var readWriteBuffer = new ReadWriteBuffer(byteArray);

        Assert.AreSame(readWriteBuffer.Buffer, byteArray);
        Assert.AreEqual(0, readWriteBuffer.ReadCursor);
        Assert.AreEqual(0, readWriteBuffer.WrittenCursor);
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
        var readWriteBuffer = new ReadWriteBuffer(new byte[20]) { WrittenCursor = 15, ReadCursor = 5 };

        Assert.AreEqual(15, readWriteBuffer.WrittenCursor);
        Assert.AreEqual(5, readWriteBuffer.ReadCursor);
        Assert.AreEqual(5, readWriteBuffer.RemainingStorage);
        Assert.IsTrue(readWriteBuffer.HasStorageForBytes(5));
        Assert.IsFalse(readWriteBuffer.HasStorageForBytes(6));
        Assert.IsFalse(readWriteBuffer.AllRead);
        Assert.AreEqual(10, readWriteBuffer.UnreadBytesRemaining);

        readWriteBuffer.Reset();
        Assert.AreEqual(0, readWriteBuffer.WrittenCursor);
        Assert.AreEqual(0, readWriteBuffer.ReadCursor);
        Assert.IsTrue(readWriteBuffer.AllRead);
        Assert.AreEqual(20, readWriteBuffer.RemainingStorage);
        Assert.IsTrue(readWriteBuffer.HasStorageForBytes(20));
        Assert.IsFalse(readWriteBuffer.HasStorageForBytes(21));
    }

    [TestMethod]
    public void WrittenBuffer_MoveUnreadToBufferStart_ResetWrittenReadToZero()
    {
        var readWriteBuffer = new ReadWriteBuffer(new byte[20]) { WrittenCursor = 15, ReadCursor = 5 };

        for (byte i = 5; i < 15; i++) readWriteBuffer.Buffer[i] = i;

        readWriteBuffer.MoveUnreadToBufferStart();

        Assert.AreEqual(0, readWriteBuffer.ReadCursor);
        Assert.AreEqual(10, readWriteBuffer.WrittenCursor);
        for (byte i = 0; i < 10; i++) Assert.AreEqual(i + 5, readWriteBuffer.Buffer[i]);

        readWriteBuffer.MoveUnreadToBufferStart();
        Assert.AreEqual(0, readWriteBuffer.ReadCursor);
        Assert.AreEqual(10, readWriteBuffer.WrittenCursor);
        for (byte i = 0; i < 10; i++) Assert.AreEqual(i + 5, readWriteBuffer.Buffer[i]);

        readWriteBuffer.WrittenCursor = 0;
        readWriteBuffer.MoveUnreadToBufferStart();
        Assert.AreEqual(0, readWriteBuffer.ReadCursor);
        Assert.AreEqual(0, readWriteBuffer.WrittenCursor);
        for (byte i = 0; i < 10; i++) Assert.AreEqual(i + 5, readWriteBuffer.Buffer[i]);
    }
}
