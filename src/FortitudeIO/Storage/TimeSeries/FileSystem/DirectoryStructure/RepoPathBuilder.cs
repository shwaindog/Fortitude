// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.Storage.TimeSeries.FileSystem.Config;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Storage.TimeSeries;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;

public interface IRepoPathBuilder
{
    DirectoryInfo RepositoryRootDirectory { get; }

    bool    CreateIfNotExists       { get; }
    string? RepositoryName          { get; }
    string  TimeSeriesFileExtension { get; }

    RepositoryProximity Proximity { get; }

    IFileRepositoryLocationConfig FileRepositoryLocationConfig { get; }

    IRepositoryRootDirectory CreateRepositoryRootDirectory();

    IPathFile IndicatorStateFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null);
    IPathFile IndicatorSignalFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null);
    IPathFile IndicatorFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null);
    IPathFile CandleFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null);
    IPathFile PriceFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null);

    IPathFile CreateTimeSeriesFile
        (string fileExtension, InstrumentType instrumentType, DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null);
}

public class RepoPathBuilder : IRepoPathBuilder
{
    private readonly ISingleRepositoryBuilderConfig repoConfig;

    public RepoPathBuilder(ISingleRepositoryBuilderConfig repoConfig) => this.repoConfig = repoConfig;

    public bool    CreateIfNotExists => repoConfig.CreateIfNotExists;
    public string? RepositoryName    => repoConfig.FileRepositoryLocationConfig.RepositoryName;

    public DirectoryInfo       RepositoryRootDirectory => new(repoConfig.FileRepositoryLocationConfig.RootDirectoryPath);
    public RepositoryProximity Proximity               => repoConfig.FileRepositoryLocationConfig.Proximity;

    public IFileRepositoryLocationConfig FileRepositoryLocationConfig => repoConfig.FileRepositoryLocationConfig;

    public string TimeSeriesFileExtension => repoConfig.FileRepositoryLocationConfig.TimeSeriesFileExtension;

    public virtual IRepositoryRootDirectory CreateRepositoryRootDirectory() => new RepositoryRootDirectory(RepositoryName, RepositoryRootDirectory);

    public virtual IPathFile IndicatorStateFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null) =>
        CreateTimeSeriesFile("_IndicatorState." + TimeSeriesFileExtension, InstrumentType.AlgoState, from, to);

    public virtual IPathFile IndicatorSignalFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null) =>
        CreateTimeSeriesFile("_IndicatorSignal." + TimeSeriesFileExtension, InstrumentType.AlgoSignal, from, to);

    public virtual IPathFile IndicatorFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null) =>
        CreateTimeSeriesFile("_Indicator." + TimeSeriesFileExtension, InstrumentType.Indicator, from, to);

    public virtual IPathFile CandleFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null) =>
        CreateTimeSeriesFile("_Summary." + TimeSeriesFileExtension, InstrumentType.Candle, from, to);

    public virtual IPathFile PriceFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null) =>
        CreateTimeSeriesFile(TimeSeriesFileExtension, InstrumentType.Price, from, to);

    public virtual IPathFile CreateTimeSeriesFile
    (string fileExtension, InstrumentType instrumentType, DiscreetTimePeriod? from = null
      , DiscreetTimePeriod? to = null) =>
        new PathFile(fileExtension, new InstrumentEntryRangeMatch(instrumentType, from, to));
}
