#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations;

public class ConversationRequester : SocketConversation, IStreamControls, IConversationRequester
{
    private readonly bool isAcceptedClientRequester;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(ConversationRequester));
    private string originalConnectionName;

    public ConversationRequester(ISocketSessionContext socketSessionContext,
        IStreamControls streamControls, bool isAcceptedClientRequester = false) : base(socketSessionContext, streamControls)
    {
        this.isAcceptedClientRequester = isAcceptedClientRequester;
        originalConnectionName = socketSessionContext.Name;
        SocketSessionContext.SocketFactoryResolver.SocketReceiverFactory.ConfigureNewSocketReceiver += AddRequesterSetName;
        SocketSessionContext.Started += SendConnectionNameToRemote;
    }

    public virtual void Send(IVersionedMessage versionedMessage)
    {
        versionedMessage.IncrementRefCount();
        SocketSessionContext.SocketSender!.Send(versionedMessage);
    }

    public virtual ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        SocketSessionContext.StreamControls!.StartAsync(timeoutTimeSpan, alternativeExecutionContext);

    public virtual ValueTask<bool>
        StartAsync(int timeoutMs, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        SocketSessionContext.StreamControls!.StartAsync(timeoutMs, alternativeExecutionContext);

    private void SendConnectionNameToRemote()
    {
        Send(new RequesterNameMessage(originalConnectionName));
    }

    private void AddRequesterSetName(ISocketReceiver socketReceiver)
    {
        socketReceiver.Decoder!.MessageDeserializationRepository.RegisterDeserializer<RequesterNameMessage>()!.AddDeserializedNotifier(
            new PassThroughDeserializedNotifier<RequesterNameMessage>($"{nameof(ConversationRequester)}.{nameof(AddRequesterSetName)}", (message
                , header, conversation) =>
            {
                var nameChange = isAcceptedClientRequester ?
                    "responder_" + message.RequesterConnectionName + "_" + originalConnectionName :
                    "requester_" + originalConnectionName + "_" + message.RequesterConnectionName;
                logger.Info("Conversation name was updated from {0} to {1}", conversation.Name, nameChange);
                conversation.Name = nameChange;
            }));
    }
}
