// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeTests.FortitudeCommon.Extensions;

public class FileInfoExtensionsTests
{
    private static int newTestCount;

    public static string GenerateUniqueFileNameOffDateTime(string prefix, string extension)
    {
        var now = DateTime.Now;
        Interlocked.Increment(ref newTestCount);
        var nowString = now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
        return prefix + "_" + nowString + "_" + newTestCount + "." + extension;
    }

    public static void DeleteTestFiles(FileInfo existing)
    {
        try
        {
            if (existing.Exists) existing.Delete();
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine("Could not delete file {0}. Got {1}", existing, ex);
            FLoggerFactory.WaitUntilDrained();
        }
    }

    public static void DeleteTestFiles(string filePrefix, string? extension = null, DirectoryInfo? optionalDirectoryInfo = null)
    {
        var dirInfo     = new DirectoryInfo(optionalDirectoryInfo?.FullName ?? Environment.CurrentDirectory);
        var filePattern = extension != null ? filePrefix + "*." + extension : filePrefix + "*";
        foreach (var existingTimeSeriesFile in dirInfo.GetFiles(filePattern))
            try
            {
                existingTimeSeriesFile.Delete();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Could not delete file {0}. Got {1}", existingTimeSeriesFile, ex);
                FLoggerFactory.WaitUntilDrained();
            }
    }

    public static FileInfo UniqueNewTestFileInfo(string namePrefix, string extensionNoDot = "bin", DirectoryInfo? optionalDirectoryInfo = null)
    {
        var testDirPath = Path.Combine(optionalDirectoryInfo?.FullName ?? Environment.CurrentDirectory
                                     , GenerateUniqueFileNameOffDateTime(namePrefix, extensionNoDot));
        var testFile = new FileInfo(testDirPath);
        if (testFile.Exists) testFile.Delete();
        return testFile;
    }
}
