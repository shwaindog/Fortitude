using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeIO.Transports;

namespace FortitudeIO.Conversations
{
    public interface IConversationState
    {
        ConversationType ConversationType { get; }
        event Action<string, int> Error;
        event Action Started;
        event Action Stopped;
        ConversationState ConversationState { get; }
        string ConversationDescription { get; }
    }
}
