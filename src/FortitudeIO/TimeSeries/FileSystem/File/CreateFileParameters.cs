namespace FortitudeIO.TimeSeries.FileSystem.File;

public interface IDirectoryFileNameResolver
{
    DirectoryInfo RootDirPath { get; }
    FileInfo FilePath(CreateFileParameters createFileParameters);
    string FileName(CreateFileParameters createFileParameters);
}

public struct CreateFileParameters
{
    public CreateFileParameters(IDirectoryFileNameResolver fileNameResolver, string instrumentName, string sourceName,
        TimeSeriesPeriod filePeriod, DateTime fileStartPeriod, TimeSeriesEntryType timeSeriesEntryType, uint internalIndexSize = 0,
        FileFlags initialFileFlags = FileFlags.None, int initialFileSizePages = 2, ushort maxStringSizeBytes = byte.MaxValue,
        string? category = null, ushort maxTypeStringSizeBytes = 512)
    {
        FileNameResolver = fileNameResolver;
        InstrumentName = instrumentName;
        SourceName = sourceName;
        FilePeriod = filePeriod;
        FileStartPeriod = fileStartPeriod;
        TimeSeriesEntryType = timeSeriesEntryType;
        InternalIndexSize = internalIndexSize;
        InitialFileFlags = initialFileFlags;
        InitialFileSizePages = initialFileSizePages;
        MaxStringSizeBytes = maxStringSizeBytes;
        Category = category;
        MaxTypeStringSizeBytes = maxTypeStringSizeBytes;
    }

    public IDirectoryFileNameResolver FileNameResolver { get; set; }
    public string InstrumentName { get; set; }
    public string? Category { get; set; }
    public string SourceName { get; set; }
    public string? OriginSourceText { get; set; }
    public string? ExternalIndexFileRelativePath { get; set; }
    public string? AnnotationFileRelativePath { get; set; }
    public TimeSeriesEntryType TimeSeriesEntryType { get; set; }
    public TimeSeriesPeriod FilePeriod { get; set; }
    public DateTime FileStartPeriod { get; set; }
    public uint InternalIndexSize { get; set; }
    public FileFlags InitialFileFlags { get; set; }
    public int InitialFileSizePages { get; set; }
    public ushort MaxStringSizeBytes { get; set; }
    public ushort MaxTypeStringSizeBytes { get; set; }
}
