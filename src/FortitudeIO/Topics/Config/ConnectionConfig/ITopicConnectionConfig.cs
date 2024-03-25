namespace FortitudeIO.Topics.Config.ConnectionConfig;

public interface ITopicConnectionConfig
{
    TransportType TransportType { get; }
    string TopicName { get; set; }
    string? TopicDescription { get; set; }
}
