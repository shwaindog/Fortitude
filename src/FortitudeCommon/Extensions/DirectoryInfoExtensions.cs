// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.Extensions;

public static class DirectoryInfoExtensions
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(DirectoryInfoExtensions));

    public static IEnumerable<FileInfo> RecursiveFindFiles(this DirectoryInfo searchDir, string filePattern = "*.*")
    {
        foreach (var match in searchDir.GetFiles(filePattern)) yield return match;
        foreach (var subDir in searchDir.GetDirectories())
        foreach (var subDirMatches in RecursiveFindFiles(subDir, filePattern))
            yield return subDirMatches;
    }

    public static bool RecursiveDelete(this DirectoryInfo deleteDir)
    {
        try
        {
            foreach (var match in deleteDir.GetFiles())
            {
                match.Delete();
                if (match.Exists) return false;
            }
            foreach (var subDir in deleteDir.GetDirectories())
                if (!RecursiveDelete(subDir))
                    return false;
            deleteDir.Delete();
            return true;
        }
        catch (Exception ex)
        {
            Logger.Warn("Could not delete file or directory {0}.  Got {1}", deleteDir.FullName, ex);
            return false;
        }
    }
}
