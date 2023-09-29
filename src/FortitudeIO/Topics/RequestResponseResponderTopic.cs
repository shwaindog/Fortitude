using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets;

namespace FortitudeIO.Topics
{
    public interface IRequestResponseResponderTopic : ITopic, IRequestResponseResponderConversation
    {
    }

    public class RequestResponseResponderTopic : Topic, IRequestResponseResponderTopic
    {
        public RequestResponseResponderTopic(string description) : base(description, ConversationType.RequestResponseRequester)
        {
        }

        private IRequestResponseResponderConversation sessionConnection;

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public event Action<ISocketConversation> OnNewClient;
        public event Action<ISocketConversation> OnClientRemoved;
        public void RegisterSerializer(uint messageId, IBinarySerializer serializer)
        {
            
        }

        public IReadOnlyDictionary<int, ISocketConversation> Clients { get; }
        public IConversationListener ConversationListener { get; }
        public void RemoveClient(ISocketConversation clientSocketSessionContext)
        {
            throw new NotImplementedException();
        }

        public void Broadcast(IVersionedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
