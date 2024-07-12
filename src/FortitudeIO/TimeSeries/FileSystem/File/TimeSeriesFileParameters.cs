// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries.FileSystem.File;

public interface IDirectoryFileNameResolver
{
    FileInfo FilePath(TimeSeriesFileParameters timeSeriesFileParameters);
}

public struct TimeSeriesFileParameters
{
    public TimeSeriesFileParameters
    (FileInfo timeSeriesFileInfo, IInstrument instrument,
        TimeSeriesPeriod filePeriod, DateTime fileStartPeriod, uint internalIndexSize = 0,
        FileFlags initialFileFlags = FileFlags.None, int initialFileSize = ushort.MaxValue * 2, ushort maxStringSizeBytes = byte.MaxValue
      , ushort maxTypeStringSizeBytes = 512)
    {
        Instrument             = instrument;
        TimeSeriesFileInfo     = timeSeriesFileInfo;
        FilePeriod             = filePeriod;
        FileStartPeriod        = fileStartPeriod;
        InternalIndexSize      = internalIndexSize;
        InitialFileFlags       = initialFileFlags;
        InitialFileSize        = initialFileSize;
        MaxStringSizeBytes     = maxStringSizeBytes;
        MaxTypeStringSizeBytes = maxTypeStringSizeBytes;
    }

    public FileInfo TimeSeriesFileInfo { get; set; }

    public IInstrument Instrument { get; set; }

    public string? OriginSourceText              { get; set; }
    public string? ExternalIndexFileRelativePath { get; set; }
    public string? AnnotationFileRelativePath    { get; set; }

    public TimeSeriesPeriod FilePeriod      { get; set; }
    public DateTime         FileStartPeriod { get; set; }

    public uint      InternalIndexSize      { get; set; }
    public FileFlags InitialFileFlags       { get; set; }
    public int       InitialFileSize        { get; set; }
    public ushort    MaxStringSizeBytes     { get; set; }
    public ushort    MaxTypeStringSizeBytes { get; set; }
}

public static class CreateFileParametersExtensions
{
    public static TimeSeriesFileParameters SetFilePeriod(this TimeSeriesFileParameters input, TimeSeriesPeriod filePeriod)
    {
        var updated = input;
        updated.FilePeriod = filePeriod;
        return updated;
    }

    public static TimeSeriesFileParameters SetInternalIndexSize(this TimeSeriesFileParameters input, uint internalIndexSize)
    {
        var updated = input;
        updated.InternalIndexSize =  internalIndexSize;
        updated.InitialFileFlags  |= internalIndexSize > 0 ? FileFlags.HasInternalIndexInHeader : FileFlags.None;
        return updated;
    }

    public static TimeSeriesFileParameters SetFileFlags(this TimeSeriesFileParameters input, FileFlags toSet)
    {
        var updated = input;
        updated.InitialFileFlags |= toSet;
        return updated;
    }

    public static TimeSeriesFileParameters AssertTimeSeriesEntryType(this TimeSeriesFileParameters input, InstrumentType instrumentType)
    {
        if (input.Instrument.InstrumentType != instrumentType)
            throw new Exception($"Expected TimeSeriesFileParameters.TimeSeriesType to be of " +
                                $"type {instrumentType} but it was {input.Instrument.InstrumentType}");
        return input;
    }

    public static TimeSeriesFileParameters SetInitialFileSize(this TimeSeriesFileParameters input, int initialFileSize)
    {
        var updated = input;
        updated.InitialFileSize = initialFileSize;
        return updated;
    }
}
