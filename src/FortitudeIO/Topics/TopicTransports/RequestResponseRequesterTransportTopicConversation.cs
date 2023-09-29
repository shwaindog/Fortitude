using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Transports.NewSocketAPI.Conversations;

namespace FortitudeIO.Topics.TopicTransports
{
    public interface IRequestResponseRequesterTransportTopicConversation : ITransportTopicConversation
    {
        IRequestResponseRequesterConversation RequestResponseRequesterConversation { get; }
    }

    public class RequestResponseRequesterTransportTopicConversation : TransportTopicConversation, IRequestResponseRequesterTransportTopicConversation
    {
        public RequestResponseRequesterTransportTopicConversation(ITopicEndpointInfo endpoint, 
            IRequestResponseRequesterConversation requestResponseRequesterConversation) : base(endpoint)
        {
            RequestResponseRequesterConversation = requestResponseRequesterConversation;
        }

        public RequestResponseRequesterTransportTopicConversation(ITopicConnectionConfig connectionConfig, 
            IRequestResponseRequesterConversation requestResponseRequesterConversation) : base(connectionConfig)
        {
            RequestResponseRequesterConversation = requestResponseRequesterConversation;
        }

        public IRequestResponseRequesterConversation RequestResponseRequesterConversation { get; }
    }
}
