namespace FortitudeIO.TimeSeries.FileSystem.File;

[Flags]
public enum FileFlags : ushort
{
    None
    , WriterOpened = 1
    , Corrupt = 2
    , WriterUpdatingHistorical = 4
    , WriterAppending = 8
    , ClosedForReading = 16
    , HasExternalIndexFile = 32
    , HasInternalIndexInHeader = 64
    , HasExternalAnnotationFile = 128
    , HasOriginalSourceText = 256
}

public static class FileFlagExtensions
{
    public static bool HasOriginalSourceTextFlag(this FileFlags flag) => (flag & FileFlags.HasOriginalSourceText) > 0;
    public static bool HasExternalIndexFileFlag(this FileFlags flag) => (flag & FileFlags.HasExternalIndexFile) > 0;
    public static bool HasInternalIndexInHeaderFlag(this FileFlags flag) => (flag & FileFlags.HasInternalIndexInHeader) > 0;
    public static bool HasExternalAnnotationFileFlag(this FileFlags flag) => (flag & FileFlags.HasExternalAnnotationFile) > 0;
}
