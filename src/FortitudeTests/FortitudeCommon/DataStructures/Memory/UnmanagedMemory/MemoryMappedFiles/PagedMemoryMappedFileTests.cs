// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeTests.FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;

[TestClass]
public class PagedMemoryMappedFileTests
{
    private static int                   newTestCount;
    private        FileInfo              memoryMappedFile        = null!;
    private        string                newMemoryMappedFilePath = null!;
    private        PagedMemoryMappedFile pagedMemoryMappedFile   = null!;

    [TestInitialize]
    public void Setup()
    {
        newMemoryMappedFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        memoryMappedFile        = new FileInfo(newMemoryMappedFilePath);
        if (memoryMappedFile.Exists) memoryMappedFile.Delete();
        pagedMemoryMappedFile = new PagedMemoryMappedFile(newMemoryMappedFilePath);
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
                Console.Out.WriteLine("Could not delete file {0}. Got {1}", existingMemMappedFile, ex);
                FLoggerFactory.WaitUntilDrained();
            }
    }

    [TestMethod]
    public unsafe void CreateNewChunkedMemoryMappedFile_GetNextChunkIsContiguous_AssertsUpperBoundaryContinuesToNewChunk()
    {
        using var onePageShiftableView = pagedMemoryMappedFile.CreateShiftableMemoryMappedFileView("onePageGrow");
        var       isAvailable          = onePageShiftableView.IsUpperViewAvailableForContiguousReadWrite;
        Assert.IsTrue(isAvailable);
        var ptr                = onePageShiftableView.StartAddress;
        var maxIntsInTwoChunks = onePageShiftableView.HalfViewSizeBytes / 2;
        for (var i = 0; i < maxIntsInTwoChunks; i++) StreamByteOps.ToBytes(ref ptr, i);
        ptr = onePageShiftableView.StartAddress;
        for (var i = 0; i < maxIntsInTwoChunks; i++)
        {
            var readValue = StreamByteOps.ToInt(ref ptr);
            Assert.AreEqual(i, readValue);
        }

        Assert.IsTrue(onePageShiftableView.ShiftFileUpByHalfOfViewSize(true));
        ptr = onePageShiftableView.StartAddress;
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

        Assert.IsTrue(onePageShiftableView.ShiftFileUpByHalfOfViewSize(false));
        Assert.IsFalse(onePageShiftableView.ShiftFileUpByHalfOfViewSize(false));
    }

    [TestMethod]
    public unsafe void CreateMemoryMappedFile_SetAValueMultipleChunksThenClosePagedMemoryMapFile_NewMemoryMappedFileFindValue()
    {
        const int writeCursorOffset    = 16_234;
        const int writeAndFindValue    = 89_234_123;
        using var onePageShiftableView = pagedMemoryMappedFile.CreateShiftableMemoryMappedFileView("onePageWrite", 1, 6, false);

        var isAvailable = onePageShiftableView.IsUpperViewAvailableForContiguousReadWrite;
        Assert.IsTrue(isAvailable);
        for (var i = 0; i < 12; i++)
        {
            if (i == 6)
            {
                var ptr = onePageShiftableView.StartAddress + writeCursorOffset;
                StreamByteOps.ToBytes(ref ptr, writeAndFindValue);
            }

            Assert.AreEqual(i, onePageShiftableView.LowerViewFileChunkNumber);
            Assert.AreEqual(i * onePageShiftableView.HalfViewSizeBytes, onePageShiftableView.LowerViewFileCursorOffset);

            onePageShiftableView.ShiftFileUpByHalfOfViewSize(true);
        }

        onePageShiftableView.Dispose();
        using var twoPageShiftableView = pagedMemoryMappedFile.CreateShiftableMemoryMappedFileView("twoPageRead", 2, 6);
        isAvailable = twoPageShiftableView.IsUpperViewAvailableForContiguousReadWrite;
        Assert.IsTrue(isAvailable);
        var assertedFoundValue = false;
        for (var i = 0; i < 6; i++)
        {
            if (i == 3)
            {
                var ptr        = twoPageShiftableView.StartAddress + writeCursorOffset;
                var checkValue = StreamByteOps.ToInt(ref ptr);
                Assert.AreEqual(writeAndFindValue, checkValue);
                assertedFoundValue = true;
            }

            Assert.AreEqual(i, twoPageShiftableView.LowerViewFileChunkNumber);
            Assert.AreEqual(i * twoPageShiftableView.HalfViewSizeBytes, twoPageShiftableView.LowerViewFileCursorOffset,
                            $"When i = {i} expected FileCursorOffset to be {i * twoPageShiftableView.HalfViewSizeBytes} " +
                            $"but was {twoPageShiftableView.LowerViewFileCursorOffset} when ChunkSizeBytes was {twoPageShiftableView.HalfViewSizeBytes}");

            twoPageShiftableView.ShiftFileUpByHalfOfViewSize(false);
        }

        twoPageShiftableView.EnsureLowerViewContainsFileCursorOffset(0);
        Assert.AreEqual(0, twoPageShiftableView.LowerViewFileChunkNumber);
        var expectedFileCursorWriteOffset = 6 * onePageShiftableView.HalfViewSizeBytes + writeCursorOffset;
        twoPageShiftableView.EnsureLowerViewContainsFileCursorOffset(expectedFileCursorWriteOffset);
        Assert.AreEqual(3, twoPageShiftableView.LowerViewFileChunkNumber);
        var chunk3Ptr          = twoPageShiftableView.StartAddress + writeCursorOffset;
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
