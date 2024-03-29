﻿#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class ConversationRequester : SocketConversation, IInitiateControls, IConversationRequester
{
    public ConversationRequester(ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls) : base(socketSessionContext, initiateControls) { }

    public virtual void Send(IVersionedMessage versionedMessage)
    {
        SocketSessionContext.SocketSender!.Send(versionedMessage);
    }

    public void StartAsync() => ((IInitiateControls?)SocketSessionContext.StreamControls)?.StartAsync();
}
