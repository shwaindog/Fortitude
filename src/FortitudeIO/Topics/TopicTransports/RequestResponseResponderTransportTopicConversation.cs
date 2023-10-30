using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Transports.NewSocketAPI.Conversations;

namespace FortitudeIO.Topics.TopicTransports
{
    public interface IRequestResponseResponderTransportTopicConversation : ITransportTopicConversation
    {
        IRequestResponseResponderConversation RequestResponseResponderConversation { get; }
    }

    public class RequestResponseResponderTransportTopicConversation : TransportTopicConversation, IRequestResponseResponderTransportTopicConversation
    {
        public RequestResponseResponderTransportTopicConversation(ITopicEndpointInfo endpoint,
            IRequestResponseResponderConversation requestResponseResponderConversation) : base(endpoint)
        {
            RequestResponseResponderConversation = requestResponseResponderConversation;
        }
        public RequestResponseResponderTransportTopicConversation(ITopicConnectionConfig connectionConfig,
            IRequestResponseResponderConversation requestResponseResponderConversation) : base(connectionConfig)
        {
            RequestResponseResponderConversation = requestResponseResponderConversation;
        }

        public IRequestResponseResponderConversation RequestResponseResponderConversation { get; }
    }
}
