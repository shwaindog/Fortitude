// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface IRepoStructureBuilder
{
    DirectoryInfo RepositoryRootDirectory { get; }
    bool          CreateIfNotExists       { get; }
    string        RepositoryName          { get; }

    RepositoryProximity Proximity { get; }

    ITimeSeriesFileStructure CreateAlgoStateTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    ITimeSeriesFileStructure CreateAlgoSignalTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    ITimeSeriesFileStructure CreateIndicatorTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    ITimeSeriesFileStructure CreatePriceSummaryTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    ITimeSeriesFileStructure CreatePriceTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
    ITimeSeriesFileStructure CreateTimeSeriesFile(InstrumentType instrumentType, TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null);
}

public struct RepositoryStructureInfo
{
    public RepositoryStructureInfo(string repoName, DirectoryInfo repositoryRootDirectory, RepositoryProximity proximity, bool createIfNotExists
      , string timeSeriesFileExtension = "tsf")
    {
        RepoName                = repoName;
        RepositoryRoot          = repositoryRootDirectory;
        Proximity               = proximity;
        CreateIfNotExists       = createIfNotExists;
        TimeSeriesFileExtension = timeSeriesFileExtension;
    }

    public RepositoryStructureInfo(string repoName, string repositoryRootDirectoryPath, RepositoryProximity proximity, bool createIfNotExists
      , string timeSeriesFileExtension = "tsf")
    {
        RepoName                = repoName;
        RepositoryRoot          = new DirectoryInfo(repositoryRootDirectoryPath);
        Proximity               = proximity;
        CreateIfNotExists       = createIfNotExists;
        TimeSeriesFileExtension = timeSeriesFileExtension;
    }

    public string              RepoName;
    public DirectoryInfo       RepositoryRoot;
    public RepositoryProximity Proximity;
    public bool                CreateIfNotExists;
    public string              TimeSeriesFileExtension = "tsf";
}

public class RepoStructureBuilder : IRepoStructureBuilder
{
    private readonly RepositoryStructureInfo repositoryStructureInfo;

    public RepoStructureBuilder(RepositoryStructureInfo repositoryStructureInfo) => this.repositoryStructureInfo = repositoryStructureInfo;

    public bool   CreateIfNotExists => repositoryStructureInfo.CreateIfNotExists;
    public string RepositoryName    => repositoryStructureInfo.RepoName;

    public DirectoryInfo       RepositoryRootDirectory => repositoryStructureInfo.RepositoryRoot;
    public RepositoryProximity Proximity               => repositoryStructureInfo.Proximity;

    public virtual ITimeSeriesFileStructure CreateAlgoStateTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile(InstrumentType.AlgoState, from, to);

    public virtual ITimeSeriesFileStructure CreateAlgoSignalTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile(InstrumentType.AlgoSignal, from, to);

    public virtual ITimeSeriesFileStructure CreateIndicatorTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile(InstrumentType.Indicator, from, to);

    public virtual ITimeSeriesFileStructure CreatePriceSummaryTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile(InstrumentType.PriceSummaryPeriod, from, to);

    public virtual ITimeSeriesFileStructure CreatePriceTimeSeriesFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null) =>
        CreateTimeSeriesFile(InstrumentType.Price, from, to);

    public virtual ITimeSeriesFileStructure CreateTimeSeriesFile(InstrumentType instrumentType, TimeSeriesPeriod? from = null
      , TimeSeriesPeriod? to = null) =>
        new TimeSeriesFileStructure(repositoryStructureInfo.TimeSeriesFileExtension, new InstrumentMatch(instrumentType, from, to));
}
