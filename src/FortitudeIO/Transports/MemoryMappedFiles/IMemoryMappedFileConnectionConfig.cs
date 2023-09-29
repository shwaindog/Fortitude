using System.IO;

namespace FortitudeIO.Topics.Config.ConnectionConfig
{
    public interface IMemoryMappedFileConnectionConfig : ITopicConnectionConfig
    {
        FileMode FileMode { get; set; }
        byte FileVersion { get; set; }
        string FilePath { get; set; }
    }
}
