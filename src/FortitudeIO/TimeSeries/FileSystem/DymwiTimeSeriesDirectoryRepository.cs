// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
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
                        repoPathBuilder.PriceSummaryFile(new DiscreetTimePeriod(TimeBoundaryPeriod.OneYear), new DiscreetTimePeriod(TimeBoundaryPeriod.OneYear))
                      , repoPathBuilder.IndicatorFile(new DiscreetTimePeriod(TimeBoundaryPeriod.OneYear), new DiscreetTimePeriod(TimeBoundaryPeriod.OneYear))
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
                            repoPathBuilder.PriceSummaryFile(new DiscreetTimePeriod(TimeBoundaryPeriod.OneWeek), new DiscreetTimePeriod(TimeBoundaryPeriod.OneMonth))
                          , repoPathBuilder.IndicatorFile(new DiscreetTimePeriod(TimeBoundaryPeriod.OneWeek), new DiscreetTimePeriod(TimeBoundaryPeriod.OneMonth))
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
                                repoPathBuilder.PriceSummaryFile(new DiscreetTimePeriod(TimeBoundaryPeriod.FourHours), new DiscreetTimePeriod(TimeBoundaryPeriod.FourHours))
                              , repoPathBuilder.IndicatorFile(new DiscreetTimePeriod(TimeBoundaryPeriod.FourHours), new DiscreetTimePeriod(TimeBoundaryPeriod.FourHours))
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
                                    repoPathBuilder.PriceSummaryFile(new DiscreetTimePeriod(TimeBoundaryPeriod.TenMinutes), new DiscreetTimePeriod(TimeBoundaryPeriod.OneHour))
                                  , repoPathBuilder.IndicatorFile(new DiscreetTimePeriod(TimeBoundaryPeriod.TenMinutes), new DiscreetTimePeriod(TimeBoundaryPeriod.OneHour))
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
                                    repoPathBuilder.PriceFile(new DiscreetTimePeriod(TimeBoundaryPeriod.Tick))
                                  , repoPathBuilder.PriceSummaryFile(new DiscreetTimePeriod(TimeBoundaryPeriod.FifteenSeconds)
                                                                   , new DiscreetTimePeriod(TimeBoundaryPeriod.FiveMinutes))
                                  , repoPathBuilder.IndicatorFile(new DiscreetTimePeriod(TimeBoundaryPeriod.FifteenSeconds),
                                                                  new DiscreetTimePeriod(TimeBoundaryPeriod.FiveMinutes))
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
