// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Storage.TimeSeries.FileSystem.Config;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem;

public class RemoteLocalCachingRepository : TimeSeriesDirectoryRepositoryBase, ITimeSeriesRepository
{
    private readonly ITimeSeriesRepository localRepository;
    private readonly ITimeSeriesRepository remoteRepository;

    public RemoteLocalCachingRepository(IRemoteLocalCachingFileRepositoryConfig remoteLocalConfig, string repositoryName)
        : base(remoteLocalConfig.BuildRepositoryInfo(repositoryName))
    {
        var localRepositoryPathBuilder = remoteLocalConfig.RepositoryPathBuilder(remoteLocalConfig.LocalRepositoryLocationConfig);
        localRepository = DymwiTimeSeriesDirectoryRepository.OpenRepository(localRepositoryPathBuilder);
        ;
        var remoteRepositoryPathBuilder = remoteLocalConfig.RepositoryPathBuilder(remoteLocalConfig.RemoteRepositoryLocationConfig);
        remoteRepository = DymwiTimeSeriesDirectoryRepository.OpenRepository(remoteRepositoryPathBuilder);
    }

    public override IMap<IInstrument, InstrumentRepoFileSet> InstrumentFilesMap
    {
        get
        {
            var result = localRepository.InstrumentFilesMap.Clone();
            foreach (var keyValuePair in remoteRepository.InstrumentFilesMap)
            {
                var combined = keyValuePair.Value;

                if (result.TryGetValue(keyValuePair.Key, out var existingSet)) combined = existingSet! + keyValuePair.Value;
                result.AddOrUpdate(keyValuePair.Key, combined!);
            }
            return result;
        }
        protected set { }
    }

    public override IReaderSession<TEntry>? GetReaderSession<TEntry>(IInstrument instrument, UnboundedTimeRange? restrictedRange = null)
    {
        var instrumentFiles = localRepository.InstrumentFilesMap[instrument] + remoteRepository.InstrumentFilesMap[instrument];

        if (instrumentFiles == null) return null;

        if (restrictedRange != null) instrumentFiles = instrumentFiles.Where(irf => irf.FileIntersects(restrictedRange)).ToInstrumentRepoFileSet();
        if (instrumentFiles.Any(irf => irf.Proximity == RepositoryProximity.Remote))
        {
            var copiedLocal = new InstrumentRepoFileSet();
            foreach (var remoteFile in instrumentFiles.Where(irf => irf.Proximity == RepositoryProximity.Remote))
            {
                var remoteRepoFile   = remoteFile.TimeSeriesRepoFile;
                var repoRelativePath = remoteRepoFile.RootDirRelativePath;
                var localPath        = Path.Combine(localRepository.RepoRootDirectory.DirPath, repoRelativePath);
                if (!Directory.Exists(Path.GetDirectoryName(localPath))) Directory.CreateDirectory(Path.GetDirectoryName(localPath)!);
                var localFile = !System.IO.File.Exists(localPath)
                    ? remoteRepoFile.File.CopyTo(Path.Combine(localRepository.RepoRootDirectory.DirPath, repoRelativePath))
                    : new FileInfo(localPath);
                var localInstrumentFile = localRepository.RepoRootDirectory.GetInstrumentFileDetails(localFile);
                copiedLocal.Add(localInstrumentFile!);
            }
            instrumentFiles -= copiedLocal;
            instrumentFiles += copiedLocal;
        }
        return new RepositoryFilesReaderSession<TEntry>(instrumentFiles!);
    }


    public override IWriterSession<TEntry>? GetWriterSession<TEntry>(IInstrument instrument) =>
        // for now always write to local copy.  Move to remote manually or when archiving
        localRepository.GetWriterSession<TEntry>(instrument);
}
