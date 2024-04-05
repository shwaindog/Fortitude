namespace FortitudeBusRules.Connectivity.Cluster.Messages;

public static class InterClusterMessages
{
    public const uint MessageIdBaseNumber = 100_000;
}

public enum InterClusterMessageIds : uint
{
    RemoteTopicSubscribeRequest = InterClusterMessages.MessageIdBaseNumber + 1
    , RemoteTopicSubscribeResponse = InterClusterMessages.MessageIdBaseNumber + 2
}
