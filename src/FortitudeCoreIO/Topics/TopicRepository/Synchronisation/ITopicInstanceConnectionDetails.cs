﻿#region

using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Topics.TopicRepository;

public interface ITopicInstanceConnectionDetails
{
    string? TopicName { get; set; }
    string? Hostname { get; set; }
    string? InstanceName { get; set; }
    ConversationType ConversationType { get; set; }
    List<ITopicEndpointInfo>? Connections { get; set; }
}