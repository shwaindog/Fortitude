using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Conversations;

namespace FortitudeIO.Topics
{
    public interface IRequestResponseRequesterTopic : ITopic, IRequestResponseRequesterConversation
    {
    }

    public class RequestResponseRequesterTopic : Topic, IRequestResponseRequesterTopic
    {
        public RequestResponseRequesterTopic(string description) : base(description, ConversationType.RequestResponseRequester)
        {
        }

        private IRequestResponseRequesterConversation sessionConnection;

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public IConversationListener ConversationListener { get; }
        public IConversationPublisher ConversationPublisher { get; }
    }
}
