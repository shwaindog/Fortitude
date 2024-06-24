// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using static FortitudeIO.TimeSeries.FileSystem.DirectoryStructure.RepositoryPathName;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

/// <summary>
///     Decade Year Month Week Instrument Time Series File Repository organises time series files by Decade Year, Month,
///     Month Week Number MarketType_MarketRegion and Instrument Name
/// </summary>
public class DymwiTimeSeriesDirectoryRepository : TimeSeriesDirectoryRepository
{
    public static readonly string[]  DymwiRequiredInstrumentKeys = ["SourceName", "MarketType", "MarketProductType", "MarketRegion"];
    public static readonly string[]  DymwiOptionalInstrumentKeys = ["Category"];

    protected DymwiTimeSeriesDirectoryRepository(RepositoryInfo repositoryInfo) : base(repositoryInfo) { }

    public static DymwiTimeSeriesDirectoryRepository OpenRepository(IRepoPathBuilder repoPathBuilder)
    {
        if (!repoPathBuilder.RepositoryRootDirectory.Exists)
        {
            if (repoPathBuilder.CreateIfNotExists)
                repoPathBuilder.RepositoryRootDirectory.Create();
            else
                throw new Exception($"Directory at {repoPathBuilder.RepositoryRootDirectory.FullName} does not exist and will not be created");
        }
        var repoRootDir = repoPathBuilder.CreateRepositoryRootDirectory();
        repoRootDir.Children = new List<IPathPart>
        {
            new PathDirectory(new PathName(Constant, "Summaries"))
            {
                new PathDirectory(new PathName(MarketType),
                                  new PathName(MarketRegion))
                {
                    new PathDirectory(new PathName(InstrumentName))
                    {
                        repoPathBuilder.PriceSummaryFile(TimeSeriesPeriod.OneYear, TimeSeriesPeriod.OneYear)
                      , repoPathBuilder.IndicatorFile(TimeSeriesPeriod.OneYear, TimeSeriesPeriod.OneYear)
                    }
                }
            }
          , new PathDirectory(new PathName(Decade))
            {
                new PathDirectory(new PathName(Constant, "Summaries"))
                {
                    new PathDirectory(new PathName(MarketType),
                                      new PathName(MarketRegion))
                    {
                        new PathDirectory(new PathName(InstrumentName))
                        {
                            repoPathBuilder.PriceSummaryFile(TimeSeriesPeriod.OneWeek, TimeSeriesPeriod.OneMonth)
                          , repoPathBuilder.IndicatorFile(TimeSeriesPeriod.OneWeek, TimeSeriesPeriod.OneMonth)
                        }
                    }
                }
              , new PathDirectory(new PathName(Year))
                {
                    new PathDirectory(new PathName(Constant, "Summaries"))
                    {
                        new PathDirectory(new PathName(MarketType),
                                          new PathName(MarketRegion))
                        {
                            new PathDirectory(new PathName(InstrumentName))
                            {
                                repoPathBuilder.PriceSummaryFile(TimeSeriesPeriod.FourHours, TimeSeriesPeriod.FourHours)
                              , repoPathBuilder.IndicatorFile(TimeSeriesPeriod.FourHours, TimeSeriesPeriod.FourHours)
                            }
                        }
                    }
                  , new PathDirectory(new PathName(Month))
                    {
                        new PathDirectory(new PathName(Constant, "Summaries"))
                        {
                            new PathDirectory(new PathName(MarketType),
                                              new PathName(MarketRegion))
                            {
                                new PathDirectory(new PathName(InstrumentName))
                                {
                                    repoPathBuilder.PriceSummaryFile(TimeSeriesPeriod.TenMinutes, TimeSeriesPeriod.OneHour)
                                  , repoPathBuilder.IndicatorFile(TimeSeriesPeriod.TenMinutes, TimeSeriesPeriod.OneHour)
                                }
                            }
                        }
                      , new PathDirectory(new PathName(Constant, "AlgoState"))
                        {
                            repoPathBuilder.IndicatorStateFile()
                        }
                      , new PathDirectory(new PathName(WeekOfMonth))
                        {
                            new PathDirectory(new PathName(MarketType),
                                              new PathName(MarketRegion))
                            {
                                new PathDirectory(new PathName(InstrumentName))
                                {
                                    repoPathBuilder.PriceFile(TimeSeriesPeriod.Tick)
                                  , repoPathBuilder.PriceSummaryFile(TimeSeriesPeriod.FifteenSeconds
                                                                   , TimeSeriesPeriod.FiveMinutes)
                                  , repoPathBuilder.IndicatorFile(TimeSeriesPeriod.FifteenSeconds,
                                                                  TimeSeriesPeriod.FiveMinutes)
                                }
                            }
                          , new PathDirectory(new PathName(Constant, "AlgoSignals"))
                            {
                                repoPathBuilder.IndicatorSignalFile()
                            }
                        }
                    }
                }
            }
        };

        var repositoryInfo = new RepositoryInfo(repoRootDir, repoPathBuilder.FileRepositoryLocationConfig, DymwiRequiredInstrumentKeys, DymwiOptionalInstrumentKeys);
        return new DymwiTimeSeriesDirectoryRepository(repositoryInfo);
    }
}
