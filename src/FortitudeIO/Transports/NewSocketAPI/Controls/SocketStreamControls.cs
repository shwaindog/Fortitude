using FortitudeCommon.Monitoring.Logging;

namespace FortitudeIO.Transports.NewSocketAPI.Sockets
{
    public interface IStreamControls
    {
        void Connect();
        void Disconnect();
        void StartMessaging();
        void StopMessaging();
    }

    public abstract class SocketStreamControls : IStreamControls
    {
        private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(SocketStreamControls));
        protected ISocketSessionContext SocketSocketSessionContext;

        public abstract void Connect();


        public virtual void StartMessaging()
        {
            SocketSocketSessionContext.SocketDispatcher.Start();
        }

        public virtual void StopMessaging()
        {
            SocketSocketSessionContext.SocketDispatcher.Stop();
        }

        public virtual void Disconnect()
        {
            StopMessaging();
            logger.Info("Server {0} @{0} stopped", SocketSocketSessionContext.ConversationDescription, 
                SocketSocketSessionContext.SocketConnection.ConnectedPort);
            SocketSocketSessionContext.OnDisconnected();
        }
    }
}
