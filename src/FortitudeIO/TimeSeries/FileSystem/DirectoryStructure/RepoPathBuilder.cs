// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.Config;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface IRepoPathBuilder
{
    DirectoryInfo RepositoryRootDirectory { get; }
    bool          CreateIfNotExists       { get; }
    string?       RepositoryName          { get; }
    string        TimeSeriesFileExtension { get; }

    RepositoryProximity Proximity { get; }

    IFileRepositoryLocationConfig FileRepositoryLocationConfig { get; }

    IRepositoryRootDirectory CreateRepositoryRootDirectory();

    IPathFile IndicatorStateFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile IndicatorSignalFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile IndicatorFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile PriceSummaryFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile PriceFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile CreateTimeSeriesFile(string fileExtension, InstrumentType instrumentType, TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
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

    public virtual IPathFile IndicatorStateFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile("_AlgoState" + TimeSeriesFileExtension, InstrumentType.AlgoState, from, to);

    public virtual IPathFile IndicatorSignalFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile("_AlgoSignal" + TimeSeriesFileExtension, InstrumentType.AlgoSignal, from, to);

    public virtual IPathFile IndicatorFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile("_Indicator" + TimeSeriesFileExtension, InstrumentType.Indicator, from, to);

    public virtual IPathFile PriceSummaryFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile("_Summary" + TimeSeriesFileExtension, InstrumentType.PriceSummaryPeriod, from, to);

    public virtual IPathFile PriceFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile(TimeSeriesFileExtension, InstrumentType.Price, from, to);

    public virtual IPathFile CreateTimeSeriesFile
    (string fileExtension, InstrumentType instrumentType, TimeSeriesPeriod? from = null
      , TimeSeriesPeriod? to = null) =>
        new PathFile(fileExtension, new InstrumentEntryRangeMatch(instrumentType, from, to));
}
