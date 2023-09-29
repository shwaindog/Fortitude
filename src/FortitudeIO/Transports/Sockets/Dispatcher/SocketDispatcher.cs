using System;

namespace FortitudeIO.Transports.Sockets.Dispatcher
{
    public interface ISocketDispatcher : ISocketDispatcherCommon
    {
        ISocketDispatcherListener Listener { get; set; }
        ISocketDispatcherSender Sender { get; set; }
    }
    public class SocketDispatcher : ISocketDispatcher
    {
        private ISocketDispatcherListener listener;
        private ISocketDispatcherSender sender;

        public SocketDispatcher(ISocketDispatcherListener listener, ISocketDispatcherSender sender)
        {
            this.listener = listener;
            this.sender = sender;
        }

        public void Start()
        {
            listener.Start();
            sender.Start();
        }

        public void Stop()
        {
            listener.Stop();
            sender.Stop();
        }

        public int UsageCount => Math.Max(listener.UsageCount, sender.UsageCount);

        public string DispatcherDescription
        {
            get { return listener.DispatcherDescription; }
            set
            {
                listener.DispatcherDescription = value;
                sender.DispatcherDescription = value;
            }
        }

        public ISocketDispatcherListener Listener
        {
            get { return listener; }
            set { listener = value; }
        }

        public ISocketDispatcherSender Sender
        {
            get { return sender; }
            set { sender = value; }
        }
    }
}
