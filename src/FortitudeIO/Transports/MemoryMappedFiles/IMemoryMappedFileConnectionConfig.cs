#region

using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Transports.MemoryMappedFiles;

public interface IMemoryMappedFileConnectionConfig : ITopicConnectionConfig
{
    FileMode FileMode { get; set; }
    byte FileVersion { get; set; }
    string FilePath { get; set; }
}
