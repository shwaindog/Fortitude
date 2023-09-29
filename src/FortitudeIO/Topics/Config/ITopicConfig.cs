using System.Collections.Generic;
using FortitudeIO.Topics.Config.ConnectionConfig;

namespace FortitudeIO.Topics.Config
{
    public interface ITopicConfig
    {
        string TopicName { get; }
        string Description { get; }
        IList<ITopicConnectionConfig> TopicConnectionConfigs { get; }
    }
}
