// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Compression.Lzma;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;

#endregion

namespace FortitudeTests.FortitudeCommon.DataStructures.Memory.Compression.Lzma;

[TestClass]
public class LzmaEncoderTests
{
    private RandomBitBufferGenerator bitGenerator = new();

    private LzmaDecoder decoder = null!;
    private LzmaEncoder encoder = null!;

    [TestInitialize]
    public void Setup()
    {
        encoder = new LzmaEncoder();
        decoder = new LzmaDecoder();
    }

    // [TestMethod]
    public void CallLongRunningLzmaBenchmark()
    {
        var dictionary = 1 << 21;

        const int itereations = 2;

        TestLzmaHelper.LzmaBenchmark(itereations, (uint)dictionary);
    }

    [TestMethod]
    public void UnmanagedMemorySmallInputStream_Compresses_DecompressedStreamIsTheSame()
    {
        const int bufferSize           = 128 * 1024;
        const int compressedBufferSize = bufferSize / 2;

        bitGenerator.Set(bufferSize);
        bitGenerator.Generate();

        var inStream = MemoryUtils.CreateByteArrayMemoryStream(bufferSize);
        inStream.Write(bitGenerator.Buffer, 0, bufferSize);
        inStream.Position = 0;

        var compressedStream = MemoryUtils.CreateByteArrayMemoryStream(compressedBufferSize);

        var encoderParams = new LzmaEncoderParams
        {
            DictionarySize = bufferSize
        };
        encoder.Compress(encoderParams, inStream, compressedStream);

        compressedStream.Seek(0, SeekOrigin.Begin);
        var crcOutStream = new CrcOutStream(bufferSize);
        decoder.Decompress(compressedStream, crcOutStream);

        inStream.Seek(0, SeekOrigin.Begin);
        crcOutStream.Seek(0, SeekOrigin.Begin);

        var stillData          = true;
        var originalStream     = new byte[4096];
        var decompressedStream = new byte[4096];
        while (stillData)
        {
            var originalRead     = inStream.Read(originalStream, 0, originalStream.Length);
            var decompressedRead = crcOutStream.Read(decompressedStream, 0, originalRead);

            if (originalRead != decompressedRead)
                Assert.Fail($"ERROR expected same bytes to be read when inStream.Position {inStream.Position} " +
                            $"and when crcOutStream.Position {crcOutStream.Position}!!!");
            for (var j = 0; j < originalRead; j++)
                if (originalStream[j] != decompressedStream[j])
                    Assert.Fail($"ERROR uncompressed does not equal original at {j} when inStream.Position {inStream.Position}!!!");
            stillData = originalRead > 0;
        }
    }


    [TestMethod]
    public unsafe void MemoryMappedFile_CompressSubSectionThenClear_DecompressedToSameLocationRestoresFile()
    {
        const int startCompressZero = 200;
        const int endCompressZero   = 2000;

        var compressAndZeroFileInfo = CopyTestTextFileWithName("ToCompressAndZero.json");
        var compareFileInfo         = CopyTestTextFileWithName("ToCompareWith.json");

        var fileSize = compressAndZeroFileInfo.Length;

        var compressAndZeroPagedMemoryMapFile = new PagedMemoryMappedFile(compressAndZeroFileInfo.FullName);
        var compareFileInfoMapFile            = new PagedMemoryMappedFile(compareFileInfo.FullName);

        var compressAndZeroFileView = compressAndZeroPagedMemoryMapFile.CreateMappedViewRegion("compressWholeFile", 0, 1, null);
        var compareFileView         = compareFileInfoMapFile.CreateMappedViewRegion("compareWholeFile", 0, 1, null);

        var compressZeroPtr = compressAndZeroFileView!.StartAddress;
        var comparePtr      = compareFileView!.StartAddress;
        for (var i = 0; i < fileSize; i++) Assert.AreEqual(*comparePtr++, *compressZeroPtr++);

        var rangeToCompressAndZero = compressAndZeroFileView.CreateUnmanagedByteArrayInThisRange(startCompressZero, endCompressZero);
        var rangeAsStream          = new ByteArrayMemoryStream(rangeToCompressAndZero, true);

        var compressedStream = MemoryUtils.CreateByteArrayMemoryStream(fileSize);
        compressedStream.CloseByteArrayOnDispose = false;

        var encoderParams = new LzmaEncoderParams
        {
            DictionarySize = endCompressZero
        };
        encoder.Compress(encoderParams, rangeAsStream, compressedStream);

        rangeToCompressAndZero.Clear();

        compressZeroPtr = compressAndZeroFileView.StartAddress;
        for (var i = startCompressZero; i < endCompressZero; i++) Assert.AreEqual(0, *(compressZeroPtr + i));

        compressedStream.Close();
        compressAndZeroFileView.Dispose();
        compressAndZeroPagedMemoryMapFile.Dispose();

        // Close zeroed out stream and reopen and decompress into same location and assert same values

        compressAndZeroPagedMemoryMapFile = new PagedMemoryMappedFile(compressAndZeroFileInfo.FullName);
        compressAndZeroFileView           = compressAndZeroPagedMemoryMapFile.CreateMappedViewRegion("compressWholeFile", 0, 1, null);

        compressZeroPtr = compressAndZeroFileView!.StartAddress;
        comparePtr      = compareFileView.StartAddress;
        for (var i = 0; i < startCompressZero; i++) Assert.AreEqual(*comparePtr++, *compressZeroPtr++);

        compressZeroPtr = compressAndZeroFileView.StartAddress;
        for (var i = startCompressZero; i < endCompressZero; i++) Assert.AreEqual(0, *(compressZeroPtr + i));

        var rangeToRestore = compressAndZeroFileView.CreateUnmanagedByteArrayInThisRange(startCompressZero, endCompressZero);
        rangeAsStream    = new ByteArrayMemoryStream(rangeToRestore, true);
        compressedStream = compressedStream.ReopenStream();
        decoder.Decompress(compressedStream, rangeAsStream);

        compressZeroPtr = compressAndZeroFileView.StartAddress;
        comparePtr      = compareFileView.StartAddress;
        for (var i = 0; i < fileSize; i++) Assert.AreEqual(*comparePtr++, *compressZeroPtr++);
    }

    public FileInfo CopyTestTextFileWithName(string fileNameOnly)
    {
        var wellKnownJsonFilePath = "FortitudeBusRules/Config/TestBusRulesConfigLoads.json";
        var copyToLocation        = "FortitudeCommon/DataStructures/Memory/Compression/Lzma";
        Directory.CreateDirectory(copyToLocation);
        var expectedDestFileName = copyToLocation + "/" + fileNameOnly;
        File.Delete(expectedDestFileName);
        var textFile = new FileInfo(wellKnownJsonFilePath);
        textFile.CopyTo(expectedDestFileName);
        return new FileInfo(expectedDestFileName);
    }
}
