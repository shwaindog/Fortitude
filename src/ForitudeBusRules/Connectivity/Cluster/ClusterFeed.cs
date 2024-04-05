#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeBusRules.Connectivity.Cluster;

public interface IClusterFeed
{
    ConversationState FeedState { get; }
}

internal class ClusterFeed { }
