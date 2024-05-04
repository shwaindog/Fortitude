#region

using System.Reflection;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;

public class RemoteRequestResponseRegistrationRule : RemoteMessageDeserializerAmenderRule
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(RemoteRequestResponseRegistrationRule));

    private readonly ISocketSessionContext socketSessionContext;
    protected ISubscription? ListenForRequestIdResponseRegistration;

    public RemoteRequestResponseRegistrationRule(string ruleName, ISocketSessionContext socketSessionContext
        , string listeningOnAddress, IConverterRepository? converterRepository = null, string? registrationRepoName = null)
        : base(ruleName, socketSessionContext, listeningOnAddress.Replace("*", ""), converterRepository, registrationRepoName) =>
        this.socketSessionContext = socketSessionContext;

    private ISocketReceiver? SocketReceiver => socketSessionContext.SocketReceiver;

    protected virtual MessageDeserializerResolveRun NewMessageDeserializerResolveRun =>
        Context.PooledRecycler.Borrow<MessageDeserializerResolveRun>();

    public override async ValueTask StartAsync()
    {
        logger.Info("RemoteRequestResponseRegistrationRule deployed on {0} and listening to {1}",
            Context.RegisteredOn.Name, ListeningOnAddress);
        await LauncherRequestIdResponseListener();
    }

    protected virtual async ValueTask LauncherRequestIdResponseListener()
    {
        await this.RegisterRequestListenerAsync<RemoteRequestIdResponseRegistration, RemoteRegistrationResponse>(
            ListeningOnAddress + "*", RegisterRequestIdResponseSource);
    }

    protected virtual string ExtractSubscriptionPostfix(string fullMessageAddressDestination) =>
        fullMessageAddressDestination
            .Replace(ListeningOnAddress, "");

    protected virtual string BuildNotifierNameFrom(string postfixSubscription, Type publishType) => postfixSubscription + "_" + publishType.Name;

    protected virtual RemoteRegistrationResponse RegisterRequestIdResponseSource(
        IBusRespondingMessage<RemoteRequestIdResponseRegistration, RemoteRegistrationResponse> requestMessage)
    {
        var remoteRequestIdResponseRegistration = requestMessage.Payload.Body();
        var resolverRun = NewMessageDeserializerResolveRun;
        try
        {
            var addressPostfix = ExtractSubscriptionPostfix(requestMessage.DestinationAddress!);
            remoteRequestIdResponseRegistration.DeserializedType ??= remoteRequestIdResponseRegistration.ResponseSource.ResponseType;
            resolverRun.SubscribeFullAddress = requestMessage.DestinationAddress!;
            resolverRun.SubscribePostFix = ExtractSubscriptionPostfix(requestMessage.DestinationAddress!);
            resolverRun.RemoteNotificationRegistration = remoteRequestIdResponseRegistration;
            resolverRun.PublishType = remoteRequestIdResponseRegistration.ResponseSource.ResponseType;
            resolverRun.MessageId = remoteRequestIdResponseRegistration.MessageId;
            resolverRun.RootMessageDeserializationRepository =
                SocketReceiver?.Decoder?.MessageDeserializationRepository ?? CapturedAnyRootDeserializationRepository;

            ResolveOrAttemptCreateMessageDeserializer(resolverRun);
            if (!resolverRun.HaveBothMessageDeserializerAndMessageId)
            {
                var response = Context.PooledRecycler.Borrow<RemoteRegistrationResponse>();
                response.Succeeded = false;
                response.FailureReason = resolverRun.FailureMessage ??
                                         $"Could not find or resolve deserializer for {remoteRequestIdResponseRegistration} as a INotifyingMessageDeserializer";
                return response;
            }

            var messageDeserializer = resolverRun.MessageDeserializer!;

            var unTypedMethodInfo
                = typeof(RemoteRequestResponseRegistrationRule)
                    .GetMethod(nameof(AddOrCreateDeserializedNotifierReturnRegistrationResponse),
                        BindingFlags.NonPublic | BindingFlags.Instance)!;
            var genericMethodInfo = unTypedMethodInfo.MakeGenericMethod(messageDeserializer.MessageType,
                remoteRequestIdResponseRegistration.ResponseSource.ResponseType);

            var responseRegisterRequestId = (RemoteRegistrationResponse)genericMethodInfo.Invoke(
                this, new object[] { messageDeserializer, addressPostfix, remoteRequestIdResponseRegistration })!;

            return responseRegisterRequestId;
        }
        catch (Exception ex)
        {
            logger.Warn("Caught exception attempting to register a requestId response source on topic {0}. With request {1}.  Got {2}",
                requestMessage.DestinationAddress, remoteRequestIdResponseRegistration, ex);
            var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteRegistrationResponse>();
            responseMisMatchTypes.Succeeded = false;
            responseMisMatchTypes.FailureReason = $"Caught exception attempting to change publication subscription on " +
                                                  $"topic {requestMessage.DestinationAddress}. " +
                                                  $"With request {remoteRequestIdResponseRegistration}.  Got {ex}";
            return responseMisMatchTypes;
        }
        finally
        {
            resolverRun.DecrementRefCount();
        }
    }

    protected virtual RemoteRegistrationResponse AddOrCreateDeserializedNotifierReturnRegistrationResponse<TM, TR>(
        INotifyingMessageDeserializer<TM> messageDeserializer, string postfixSubscription,
        RemoteRequestIdResponseRegistration remoteRequestIdResponseRegistration) where TM : class, IVersionedMessage, new()
    {
        var notifierName = BuildNotifierNameFrom(postfixSubscription, typeof(TR));
        var checkExisting = messageDeserializer[notifierName];
        if (checkExisting is IDeserializedNotifier<TM, TR> deserializedNotifier)
        {
            deserializedNotifier.AddRequestExpected(remoteRequestIdResponseRegistration.RequestId
                , remoteRequestIdResponseRegistration.ResponseSource);
            var response = Context.PooledRecycler.Borrow<RemoteRegistrationResponse>();
            response.Succeeded = true;
            return response;
        }

        if (typeof(TM) == typeof(TR))
        {
            var newPassThroughNotifier = new PassThroughDeserializedNotifier<TM>(notifierName, DeserializeNotifyTypeFlags.MessageAndConversation)
            {
                RemoveOnZeroSubscribers = false
            };
            newPassThroughNotifier.RegisterMessageDeserializer(messageDeserializer);
            newPassThroughNotifier.AddRequestExpected(remoteRequestIdResponseRegistration.RequestId
                , remoteRequestIdResponseRegistration.ResponseSource);
            var response = Context.PooledRecycler.Borrow<RemoteRegistrationResponse>();
            response.Succeeded = true;
            return response;
        }

        var resolvedConverter = remoteRequestIdResponseRegistration.Converter as IConverter<TM, TR> ?? ConverterRepository?.GetConverter<TM, TR>();
        if (resolvedConverter != null)
        {
            var convertingNotifier
                = new ConvertingDeserializedNotifier<TM, TR>(notifierName, resolvedConverter, DeserializeNotifyTypeFlags.MessageAndConversation)
                {
                    RemoveOnZeroSubscribers = false
                };
            convertingNotifier.RegisterMessageDeserializer(messageDeserializer);
            convertingNotifier.AddRequestExpected(remoteRequestIdResponseRegistration.RequestId, remoteRequestIdResponseRegistration.ResponseSource);
            var response = Context.PooledRecycler.Borrow<RemoteRegistrationResponse>();
            response.Succeeded = true;
            return response;
        }

        var failedResponse = Context.PooledRecycler.Borrow<RemoteRegistrationResponse>();
        failedResponse.Succeeded = false;
        failedResponse.FailureReason
            = $"Could not resolve a converter for INotifyingMessageDeserializer<{typeof(TM).Name} and " +
              $"ConvertingDeserializedNotifier<{typeof(TM).Name}, {typeof(TR).Name}>";
        return failedResponse;
    }
}
