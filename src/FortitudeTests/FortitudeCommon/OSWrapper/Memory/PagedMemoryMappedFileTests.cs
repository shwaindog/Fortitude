#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeTests.FortitudeCommon.OSWrapper.Memory;

[TestClass]
public class PagedMemoryMappedFileTests
{
    private static int newTestCount;
    private FileInfo memoryMappedFile = null!;
    private string newMemoryMappedFilePath = null!;
    private PagedMemoryMappedFile pagedMemoryMappedFile = null!;

    [TestInitialize]
    public void Setup()
    {
        newMemoryMappedFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        memoryMappedFile = new FileInfo(newMemoryMappedFilePath);
        if (memoryMappedFile.Exists) memoryMappedFile.Delete();
        pagedMemoryMappedFile = new PagedMemoryMappedFile(newMemoryMappedFilePath, 1);
    }

    [TestCleanup]
    public void Cleanup()
    {
        pagedMemoryMappedFile.Dispose();
        var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var existingMemMappedFile in dirInfo.GetFiles("PagedMemoryMappedFileTests_*"))
            try
            {
                existingMemMappedFile.Delete();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Could not delete file {0}", existingMemMappedFile);
                FLoggerFactory.WaitUntilDrained();
            }
    }

    [TestMethod]
    public unsafe void CreateNewChunkedMemoryMappedFile_GetNextChunkIsContiguous_AssertsUpperBoundaryContinuesToNewChunk()
    {
        using var twoChunkView = pagedMemoryMappedFile.CreateTwoContiguousPagedFileChunks();
        var isAvailable = twoChunkView.IsUpperChunkAvailableForContiguousReadWrite;
        Assert.IsTrue(isAvailable);
        var ptr = twoChunkView.LowerChunkAddress;
        var maxIntsInTwoChunks = twoChunkView.ChunkSizeBytes / 2;
        for (var i = 0; i < maxIntsInTwoChunks; i++) StreamByteOps.ToBytes(ref ptr, i);
        ptr = twoChunkView.LowerChunkAddress;
        for (var i = 0; i < maxIntsInTwoChunks; i++)
        {
            var readValue = StreamByteOps.ToInt(ref ptr);
            Assert.AreEqual(i, readValue);
        }

        Assert.IsTrue(twoChunkView.NextChunk(true));
        ptr = twoChunkView.LowerChunkAddress;
        for (var i = maxIntsInTwoChunks / 2; i < maxIntsInTwoChunks; i++)
        {
            var readValue = StreamByteOps.ToInt(ref ptr);
            Assert.AreEqual(i, readValue);
        }

        for (var i = maxIntsInTwoChunks / 2; i < maxIntsInTwoChunks; i++)
        {
            var readValue = StreamByteOps.ToInt(ref ptr);
            Assert.AreEqual(0, readValue);
        }

        Assert.IsTrue(twoChunkView.NextChunk(false));
        Assert.IsFalse(twoChunkView.NextChunk(false));
    }

    [TestMethod]
    public unsafe void CreateMemoryMappedFile_SetAValueMultipleChunksThenClosePagedMemoryMapFile_NewMemoryMappedFileFindValue()
    {
        const int writeCursorOffset = 16_234;
        const int writeAndFindValue = 89_234_123;
        using var twoChunkView = pagedMemoryMappedFile.CreateTwoContiguousPagedFileChunks(true);

        var isAvailable = twoChunkView.IsUpperChunkAvailableForContiguousReadWrite;
        Assert.IsTrue(isAvailable);
        for (var i = 0; i < 12; i++)
        {
            if (i == 6)
            {
                var ptr = twoChunkView.LowerChunkAddress + writeCursorOffset;
                StreamByteOps.ToBytes(ref ptr, writeAndFindValue);
            }

            Assert.AreEqual(i, twoChunkView.LowerChunkNumber);
            Assert.AreEqual(i * twoChunkView.ChunkSizeBytes, twoChunkView.LowerChunkFileCursorOffset);

            twoChunkView.NextChunk(true);
        }

        twoChunkView.Dispose();
        pagedMemoryMappedFile.Dispose();
        pagedMemoryMappedFile = new PagedMemoryMappedFile(newMemoryMappedFilePath, 2);
        using var twoChunkViewTwoPagesPerChunk = pagedMemoryMappedFile.CreateTwoContiguousPagedFileChunks(true);
        isAvailable = twoChunkViewTwoPagesPerChunk.IsUpperChunkAvailableForContiguousReadWrite;
        Assert.IsTrue(isAvailable);
        var assertedFoundValue = false;
        for (var i = 0; i < 6; i++)
        {
            if (i == 3)
            {
                var ptr = twoChunkViewTwoPagesPerChunk.LowerChunkAddress + writeCursorOffset;
                var checkValue = StreamByteOps.ToInt(ref ptr);
                Assert.AreEqual(writeAndFindValue, checkValue);
                assertedFoundValue = true;
            }

            Assert.AreEqual(i, twoChunkViewTwoPagesPerChunk.LowerChunkNumber);
            Assert.AreEqual(i * twoChunkViewTwoPagesPerChunk.ChunkSizeBytes, twoChunkViewTwoPagesPerChunk.LowerChunkFileCursorOffset,
                $"When i = {i} expected FileCursorOffset to be {i * twoChunkViewTwoPagesPerChunk.ChunkSizeBytes} " +
                $"but was {twoChunkViewTwoPagesPerChunk.LowerChunkFileCursorOffset} when ChunkSizeBytes was {twoChunkViewTwoPagesPerChunk.ChunkSizeBytes}");

            twoChunkViewTwoPagesPerChunk.NextChunk(false);
        }

        twoChunkViewTwoPagesPerChunk.EnsureLowerChunkContainsFileCursorOffset(0);
        Assert.AreEqual(0, twoChunkViewTwoPagesPerChunk.LowerChunkNumber);
        var expectedFileCursorWriteOffset = 6 * twoChunkView.ChunkSizeBytes + writeCursorOffset;
        twoChunkViewTwoPagesPerChunk.EnsureLowerChunkContainsFileCursorOffset(expectedFileCursorWriteOffset);
        Assert.AreEqual(3, twoChunkViewTwoPagesPerChunk.LowerChunkNumber);
        var chunk3Ptr = twoChunkViewTwoPagesPerChunk.LowerChunkAddress + writeCursorOffset;
        var checkExpectedValue = StreamByteOps.ToInt(ref chunk3Ptr);
        Assert.AreEqual(writeAndFindValue, checkExpectedValue);

        Assert.IsTrue(assertedFoundValue);
    }

    private string GenerateUniqueFileNameOffDateTime()
    {
        var now = DateTime.Now;
        Interlocked.Increment(ref newTestCount);
        var nowString = now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
        return "PagedMemoryMappedFileTests_" + nowString + "_" + newTestCount + ".bin";
    }
}
