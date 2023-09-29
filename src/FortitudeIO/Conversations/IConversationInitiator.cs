using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeIO.Conversations
{
    public interface IConversationInitiator
    {
        void Start();
        void Stop();
    }
}
