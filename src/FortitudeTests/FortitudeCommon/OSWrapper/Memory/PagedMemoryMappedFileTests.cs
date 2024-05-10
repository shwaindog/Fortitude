#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeTests.FortitudeCommon.OSWrapper.Memory;

[TestClass]
public class PagedMemoryMappedFileTests
{
    private FileInfo memoryMappedFile = null!;
    private PagedMemoryMappedFile pagedMemoryMappedFile = null!;

    [TestInitialize]
    public void Setup()
    {
        var newMemoryMappedFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        memoryMappedFile = new FileInfo(newMemoryMappedFilePath);
        if (memoryMappedFile.Exists) memoryMappedFile.Delete();
        pagedMemoryMappedFile = new PagedMemoryMappedFile(newMemoryMappedFilePath, 1);
    }

    [TestCleanup]
    public void Cleanup()
    {
        pagedMemoryMappedFile.Dispose();
        var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var existingMemMappedFile in dirInfo.GetFiles("PagedMemoryMappedFileTests_*")) existingMemMappedFile.Delete();
    }


    [TestMethod]
    public unsafe void CreateNewChunkedMemoryMappedFile_GetNextChunkIsContiguous_AssertsUpperBoundaryContinuesToNewChunk()
    {
        var twoChunkView = pagedMemoryMappedFile.CreateTwoContiguousPagedFileChunks();
        var isAvailable = twoChunkView.IsUpperChunkAvailableForContiguousReadWrite;
        Assert.IsTrue(isAvailable);
        var ptr = twoChunkView.ChunkAddress;
        var maxIntsInTwoChunks = twoChunkView.ChunkSizeBytes / 2;
        for (var i = 0; i < maxIntsInTwoChunks; i++) StreamByteOps.ToBytes(ref ptr, i);
        ptr = twoChunkView.ChunkAddress;
        for (var i = 0; i < maxIntsInTwoChunks; i++)
        {
            var readValue = StreamByteOps.ToInt(ref ptr);
            Assert.AreEqual(i, readValue);
        }

        Assert.IsTrue(twoChunkView.NextChunk(true));
        ptr = twoChunkView.ChunkAddress;
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

        twoChunkView.Dispose();
    }

    private string GenerateUniqueFileNameOffDateTime()
    {
        var now = DateTime.Now;
        var nowString = now.ToString("yyyy-MM-dd_HH-mm-dd_fff");
        return "PagedMemoryMappedFileTests_" + nowString + ".bin";
    }
}
