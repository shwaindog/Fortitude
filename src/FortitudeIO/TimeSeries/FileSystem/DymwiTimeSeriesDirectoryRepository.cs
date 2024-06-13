// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using static FortitudeIO.TimeSeries.FileSystem.DirectoryStructure.TimeSeriesPathNameComponent;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

/// <summary>
///     Decade Year Month Week Instrument Time Series File Repository organises time series files by Decade Year, Month,
///     Month Week Number and Instrument Name
/// </summary>
public class DymwiTimeSeriesDirectoryRepository : TimeSeriesDirectoryRepository
{
    protected DymwiTimeSeriesDirectoryRepository(RepositoryInfo repositoryInfo) : base(repositoryInfo) { }


    public static DymwiTimeSeriesDirectoryRepository OpenRepository(IRepoStructureBuilder repoStructureBuilder)
    {
        if (!repoStructureBuilder.RepositoryRootDirectory.Exists)
        {
            if (repoStructureBuilder.CreateIfNotExists)
                repoStructureBuilder.RepositoryRootDirectory.Create();
            else
                throw new Exception($"Directory at {repoStructureBuilder.RepositoryRootDirectory.FullName} does not exist and will not be created");
        }
        var repoRootDir = new RepositoryRootDirectoryStructure(repoStructureBuilder.RepositoryName, repoStructureBuilder.RepositoryRootDirectory)
        {
            repoStructureBuilder.CreatePriceSummaryTimeSeriesFile(TimeSeriesPeriod.OneYear, TimeSeriesPeriod.OneYear)
          , repoStructureBuilder.CreateIndicatorTimeSeriesFile(TimeSeriesPeriod.OneYear, TimeSeriesPeriod.OneYear)
          , new TimeSeriesDirectoryStructure(new TimeSeriesPathNameFormat(Decade))
            {
                repoStructureBuilder.CreatePriceSummaryTimeSeriesFile(TimeSeriesPeriod.OneWeek, TimeSeriesPeriod.OneMonth)
              , repoStructureBuilder.CreateIndicatorTimeSeriesFile(TimeSeriesPeriod.OneWeek, TimeSeriesPeriod.OneMonth)
              , new TimeSeriesDirectoryStructure(new TimeSeriesPathNameFormat(Year))
                {
                    repoStructureBuilder.CreatePriceSummaryTimeSeriesFile(TimeSeriesPeriod.FourHours, TimeSeriesPeriod.FourHours)
                  , repoStructureBuilder.CreateIndicatorTimeSeriesFile(TimeSeriesPeriod.FourHours, TimeSeriesPeriod.FourHours)
                  , new TimeSeriesDirectoryStructure(new TimeSeriesPathNameFormat(Month))
                    {
                        repoStructureBuilder.CreatePriceSummaryTimeSeriesFile(TimeSeriesPeriod.TenMinutes, TimeSeriesPeriod.OneHour)
                      , repoStructureBuilder.CreateIndicatorTimeSeriesFile(TimeSeriesPeriod.TenMinutes, TimeSeriesPeriod.OneHour)
                      , repoStructureBuilder.CreateAlgoStateTimeSeriesFile()
                      , new TimeSeriesDirectoryStructure(new TimeSeriesPathNameFormat(WeekOfMonth))
                        {
                            new TimeSeriesDirectoryStructure(new TimeSeriesPathNameFormat(InstrumentName))
                            {
                                repoStructureBuilder.CreatePriceTimeSeriesFile(TimeSeriesPeriod.Tick)
                              , repoStructureBuilder.CreatePriceSummaryTimeSeriesFile(TimeSeriesPeriod.FifteenSeconds, TimeSeriesPeriod.FiveMinutes)
                              , repoStructureBuilder.CreateIndicatorTimeSeriesFile(TimeSeriesPeriod.FifteenSeconds, TimeSeriesPeriod.FiveMinutes)
                              , repoStructureBuilder.CreateAlgoSignalTimeSeriesFile()
                            }
                        }
                    }
                }
            }
        };

        var repositoryInfo = new RepositoryInfo(repoRootDir, repoStructureBuilder.Proximity);
        return new DymwiTimeSeriesDirectoryRepository(repositoryInfo);
    }
}
