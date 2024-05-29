// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeTests.FortitudeCommon.DataStructures.Memory.Compression.Lzma;

[TestClass]
public class LzmaEncoderTests
{
    // [TestMethod]
    public void CallLzmaBench()
    {
        var dictionary = 1 << 21;

        const int itereations = 2;

        TestLzmaHelper.LzmaBenchmark(itereations, (uint)dictionary);
    }
}
