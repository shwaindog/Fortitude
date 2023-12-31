﻿#region

using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.Config;

public interface ITopicConfig
{
    string TopicName { get; }
    string Description { get; }
    IList<ITopicConnectionConfig> TopicConnectionConfigs { get; }
}
