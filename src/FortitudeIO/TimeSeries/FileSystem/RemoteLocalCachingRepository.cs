// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.Session;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public class RemoteLocalRepositoryParams : IRepositoryBuilder
{
    public RemoteLocalRepositoryParams(IRepositoryBuilder localRepositoryBuilder, IRepositoryBuilder remoteRepositoryBuilder)
    {
        LocalRepositoryBuilder  = localRepositoryBuilder;
        RemoteRepositoryBuilder = remoteRepositoryBuilder;
    }

    public IRepositoryBuilder LocalRepositoryBuilder  { get; set; }
    public IRepositoryBuilder RemoteRepositoryBuilder { get; set; }

    public ITimeSeriesRepository BuildRepository() =>
        new RemoteLocalCachingRepository(LocalRepositoryBuilder.BuildRepository(), RemoteRepositoryBuilder.BuildRepository());
}

public class RemoteLocalCachingRepository : ITimeSeriesRepository
{
    private readonly ITimeSeriesRepository localRepository;
    private readonly ITimeSeriesRepository remoteRepository;

    public RemoteLocalCachingRepository(ITimeSeriesRepository localRepository, ITimeSeriesRepository remoteRepository)
    {
        this.localRepository  = localRepository;
        this.remoteRepository = remoteRepository;
    }

    public RepositoryProximity Proximity => RepositoryProximity.Both;

    public void CloseAllFilesAndSessions()
    {
        localRepository.CloseAllFilesAndSessions();
    }

    public IMap<IInstrument, InstrumentRepoFileSet> InstrumentFilesMap
    {
        get
        {
            var result = localRepository.InstrumentFilesMap.Clone();
            foreach (var keyValuePair in remoteRepository.InstrumentFilesMap)
            {
                var combined = keyValuePair.Value;

                if (result.TryGetValue(keyValuePair.Key, out var existingSet)) combined = existingSet! + keyValuePair.Value;
                result.AddOrUpdate(keyValuePair.Key, combined);
            }
            return result;
        }
    }

    public IRepositoryRootDirectory RepoRootDirectory => localRepository.RepoRootDirectory;

    public IReaderSession<TEntry>? GetReaderSession<TEntry>
        (IInstrument instrument, TimeRange? restrictedRange = null) where TEntry : ITimeSeriesEntry<TEntry>
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


    public IWriterSession<TEntry>? GetWriterSession<TEntry>
        (IInstrument instrument) where TEntry : ITimeSeriesEntry<TEntry> =>
        // for now always write to local copy.  Move to remote manually or when archiving
        localRepository.GetWriterSession<TEntry>(instrument);
}
