namespace FortitudeIO.Transports.NewSocketAPI.SessionConnection;

public interface ISocketSessionSender : ISessionSender, ISocketSession
{
    ISocketSessionConnection? Parent { get; set; }
}
