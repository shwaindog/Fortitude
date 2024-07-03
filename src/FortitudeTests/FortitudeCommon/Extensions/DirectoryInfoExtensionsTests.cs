// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeTests.FortitudeCommon.Extensions;

public class DirectoryInfoExtensionsTests
{
    private static int newTestCount;

    public static string GenerateUniqueDirectoryNameOffDateTime(string prefix)
    {
        var now = DateTime.Now;
        Interlocked.Increment(ref newTestCount);
        var nowString = now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
        return prefix + "_" + nowString + "_" + newTestCount;
    }

    public static DirectoryInfo UniqueNewTestDirectory(string namePrefix)
    {
        var testDirPath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueDirectoryNameOffDateTime(namePrefix));
        var dirInfo     = new DirectoryInfo(testDirPath);
        if (!dirInfo.Exists) dirInfo.Create();
        return dirInfo;
    }
}
