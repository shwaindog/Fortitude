namespace FortitudeIO.Topics.Config.ConnectionConfig
{
    public interface ITopicConnectionConfig
    {
        TransportType TransportType { get; }
        int SendBufferSize { get; set; }
        int ReceiveBufferSize { get; set; }
        string InstanceName { get; set; }
    }
}
