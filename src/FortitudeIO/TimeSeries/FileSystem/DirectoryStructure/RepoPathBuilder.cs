// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface IRepoPathBuilder
{
    DirectoryInfo RepositoryRootDirectory { get; }
    bool          CreateIfNotExists       { get; }
    string        RepositoryName          { get; }

    string TimeSeriesFileExtension { get; }

    RepositoryProximity Proximity { get; }

    IRepositoryRootDirectory CreateRepositoryRootDirectory();

    IPathFile AlgoStateFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile AlgoSignalFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile IndicatorFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile PriceSummaryFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile PriceFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    IPathFile CreateTimeSeriesFile(string fileExtension, InstrumentType instrumentType, TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
}

public struct RepositoryStructureInfo
{
    public RepositoryStructureInfo(string repoName, DirectoryInfo repositoryRootDirectory, RepositoryProximity proximity, bool createIfNotExists
      , string timeSeriesFileExtension = ".tsf")
    {
        RepoName                = repoName;
        RepositoryRoot          = repositoryRootDirectory;
        Proximity               = proximity;
        CreateIfNotExists       = createIfNotExists;
        TimeSeriesFileExtension = timeSeriesFileExtension;
    }

    public RepositoryStructureInfo(string repoName, string repositoryRootDirectoryPath, RepositoryProximity proximity, bool createIfNotExists
      , string timeSeriesFileExtension = ".tsf")
    {
        RepoName                = repoName;
        RepositoryRoot          = new DirectoryInfo(repositoryRootDirectoryPath);
        Proximity               = proximity;
        CreateIfNotExists       = createIfNotExists;
        TimeSeriesFileExtension = timeSeriesFileExtension;
    }

    public string        RepoName;
    public DirectoryInfo RepositoryRoot;

    public RepositoryProximity Proximity;

    public bool   CreateIfNotExists;
    public string TimeSeriesFileExtension = ".tsf";
}

public class RepoPathBuilder : IRepoPathBuilder
{
    private readonly RepositoryStructureInfo repositoryStructureInfo;

    public RepoPathBuilder(RepositoryStructureInfo repositoryStructureInfo) => this.repositoryStructureInfo = repositoryStructureInfo;

    public bool   CreateIfNotExists => repositoryStructureInfo.CreateIfNotExists;
    public string RepositoryName    => repositoryStructureInfo.RepoName;

    public DirectoryInfo       RepositoryRootDirectory => repositoryStructureInfo.RepositoryRoot;
    public RepositoryProximity Proximity               => repositoryStructureInfo.Proximity;

    public string TimeSeriesFileExtension => repositoryStructureInfo.TimeSeriesFileExtension;

    public virtual IRepositoryRootDirectory CreateRepositoryRootDirectory() => new RepositoryRootDirectory(RepositoryName, RepositoryRootDirectory);

    public virtual IPathFile AlgoStateFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile("_AlgoState" + TimeSeriesFileExtension, InstrumentType.AlgoState, from, to);

    public virtual IPathFile AlgoSignalFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile("_AlgoSignal" + TimeSeriesFileExtension, InstrumentType.AlgoSignal, from, to);

    public virtual IPathFile IndicatorFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile("_Indicator" + TimeSeriesFileExtension, InstrumentType.Indicator, from, to);

    public virtual IPathFile PriceSummaryFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile("_Summary" + TimeSeriesFileExtension, InstrumentType.PriceSummaryPeriod, from, to);

    public virtual IPathFile PriceFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile(TimeSeriesFileExtension, InstrumentType.Price, from, to);

    public virtual IPathFile CreateTimeSeriesFile(string fileExtension, InstrumentType instrumentType, TimeSeriesPeriod? from = null
      , TimeSeriesPeriod? to = null) =>
        new PathFile(fileExtension, new InstrumentMatch(instrumentType, from, to));
}
