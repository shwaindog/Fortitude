using System;
using FortitudeIO.Conversations;

namespace FortitudeIO.Transports
{
    public interface ISocketConversation : IConversationState
    {
        IConversationListener ConversationListener { get; }
        IConversationPublisher ConversationPublisher { get; }
        event Action Connected;
        event Action Disconnected;
    }
}