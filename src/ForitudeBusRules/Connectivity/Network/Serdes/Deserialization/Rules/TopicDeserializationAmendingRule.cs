#region

using System.Reflection;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;

public class RemoteMessageBusTopicPublicationAmendingRule : RemoteMessageDeserializerAmenderRule
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(RemoteMessageBusTopicPublicationAmendingRule));

    private readonly ISocketSessionContext socketSessionContext;
    protected DeserializeNotifyTypeFlags DefaultNotifyTypeFlags = DeserializeNotifyTypeFlags.MessageAndConversation;
    protected ISubscription? ListenForPublishSubscriptions;

    public RemoteMessageBusTopicPublicationAmendingRule(string ruleName, ISocketSessionContext socketSessionContext
        , string remotePublishRegistrationListenAddress, IConverterRepository? converterRepository = null, string? registrationRepoName = null)
        : base(ruleName, socketSessionContext, remotePublishRegistrationListenAddress.Replace("*", ""), converterRepository, registrationRepoName) =>
        this.socketSessionContext = socketSessionContext;

    private ISocketReceiver? SocketReceiver => socketSessionContext.SocketReceiver;

    protected virtual MessageDeserializerResolveRun NewMessageDeserializerResolveRun =>
        Context.PooledRecycler.Borrow<MessageDeserializerResolveRun>();

    public override async ValueTask StartAsync()
    {
        logger.Info("TopicDeserializationRepositoryAmendingRule deployed on {0} and listening to {1}",
            Context.RegisteredOn.Name, ListeningOnAddress);
        await LaunchTopicPublicationAmenderListener();
    }

    protected virtual async ValueTask LaunchTopicPublicationAmenderListener()
    {
        ListenForPublishSubscriptions
            = await Context.MessageBus.RegisterRequestListenerAsync<RemoteMessageBusPublishRegistration,
                RemoteMessageBusPublishRegistrationResponse>(this,
                ListeningOnAddress + "*", UpdatePublishRegistrationRequestReceived);
    }

    public override async ValueTask StopAsync()
    {
        if (ListenForPublishSubscriptions != null) await ListenForPublishSubscriptions.UnsubscribeAsync();
    }

    protected virtual string ExtractSubscriptionPostfix(string fullMessageAddressDestination) =>
        fullMessageAddressDestination.Replace(ListeningOnAddress, "");

    protected virtual string BuildNotifierNameFrom(string postfixSubscription, Type publishType) => postfixSubscription + "_" + publishType.Name;

    protected virtual string
        BuildReceiverContextNameFrom(string postfixSubscription, string publicationType, Type publishType, string publishAddress) =>
        postfixSubscription + "_" + publicationType + "_" + publishType.Name + "_" + publishAddress;

    protected virtual RemoteMessageBusPublishRegistrationResponse UpdatePublishRegistrationRequestReceived(
        IBusRespondingMessage<RemoteMessageBusPublishRegistration, RemoteMessageBusPublishRegistrationResponse> contextPublishRegistrationMsg)
    {
        var remoteMessageBusPublishRegistration = contextPublishRegistrationMsg.Payload.Body()!;
        var addressPostfix = ExtractSubscriptionPostfix(contextPublishRegistrationMsg.DestinationAddress!);
        var resolverRun = NewMessageDeserializerResolveRun;
        try
        {
            resolverRun.SubscribeFullAddress = contextPublishRegistrationMsg.DestinationAddress!;
            resolverRun.SubscribePostFix = ExtractSubscriptionPostfix(contextPublishRegistrationMsg.DestinationAddress!);
            resolverRun.RemoteNotificationRegistration = remoteMessageBusPublishRegistration;
            resolverRun.PublishType = remoteMessageBusPublishRegistration.PublishType;
            resolverRun.MessageId = remoteMessageBusPublishRegistration.MessageId;
            resolverRun.RootMessageDeserializationRepository =
                SocketReceiver?.Decoder?.MessageDeserializationRepository ?? CapturedAnyRootDeserializationRepository;
            ResolveOrAttemptCreateMessageDeserializer(resolverRun);
            if (!resolverRun.HaveBothMessageDeserializerAndMessageId)
            {
                logger.Warn("Could not resolve MessageDeserializer for request {0} on address {1}", remoteMessageBusPublishRegistration
                    , contextPublishRegistrationMsg.DestinationAddress);
                var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
                responseMisMatchTypes.Succeeded = false;
                responseMisMatchTypes.FailureReason = resolverRun.FailureMessage ??
                                                      $"Could not resolve MessageDeserializer for request {remoteMessageBusPublishRegistration}" +
                                                      $" on address {contextPublishRegistrationMsg.DestinationAddress}";
                return responseMisMatchTypes;
            }

            var messageDeserializer = resolverRun.MessageDeserializer!;

            var unTypedMethodInfo = GetType().GetMethod(nameof(AddOrCreateDeserializedNotifier), BindingFlags.NonPublic | BindingFlags.Instance)!;
            var genericMethodInfo
                = unTypedMethodInfo.MakeGenericMethod(messageDeserializer.MessageType, remoteMessageBusPublishRegistration.PublishType);

            var sourcedDeserializedNotifier = (IDeserializedNotifier?)genericMethodInfo.Invoke(this,
                new object[] { messageDeserializer, addressPostfix, remoteMessageBusPublishRegistration });
            if (sourcedDeserializedNotifier == null)
            {
                logger.Warn("Could not resolve DeserializerNotifier for request {0} on address {1}", remoteMessageBusPublishRegistration
                    , contextPublishRegistrationMsg.DestinationAddress);
                var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
                responseMisMatchTypes.Succeeded = false;
                responseMisMatchTypes.FailureReason
                    = $"Could not resolve DeserializerNotifier for request {remoteMessageBusPublishRegistration} " +
                      $"on address {contextPublishRegistrationMsg.DestinationAddress}";
                return responseMisMatchTypes;
            }

            if (remoteMessageBusPublishRegistration.Rule == null && remoteMessageBusPublishRegistration.QueueContext == null)
                return CheckUpdateGlobalMessageBusPublishReceiveContext(remoteMessageBusPublishRegistration, addressPostfix
                    , sourcedDeserializedNotifier);

            if (remoteMessageBusPublishRegistration.Rule == null && remoteMessageBusPublishRegistration.QueueContext != null)
                return CheckUpdateTargetMessageQueuePublishReceiveContext(remoteMessageBusPublishRegistration, addressPostfix
                    , sourcedDeserializedNotifier);

            return CheckUpdateTargetRulePublishReceiveContext(remoteMessageBusPublishRegistration, addressPostfix
                , sourcedDeserializedNotifier);
        }
        catch (Exception ex)
        {
            logger.Warn("Caught exception attempting to change publication subscription on topic {0}. With request {1}.  Got {2}",
                contextPublishRegistrationMsg.DestinationAddress, remoteMessageBusPublishRegistration, ex);
            var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
            responseMisMatchTypes.Succeeded = false;
            responseMisMatchTypes.FailureReason = $"Caught exception attempting to change publication subscription on " +
                                                  $"topic {contextPublishRegistrationMsg.DestinationAddress}. " +
                                                  $"With request {remoteMessageBusPublishRegistration}.  Got {ex}";
            return responseMisMatchTypes;
        }
        finally
        {
            resolverRun.DecrementRefCount();
        }
    }

    protected virtual RemoteMessageBusPublishRegistrationResponse CheckUpdateGlobalMessageBusPublishReceiveContext(
        RemoteMessageBusPublishRegistration regRequest
        , string requestDestinationAddress, IDeserializedNotifier sourcedDeserializedNotifier)
    {
        var addressPostfix = ExtractSubscriptionPostfix(requestDestinationAddress);
        var receiverContextName
            = BuildReceiverContextNameFrom(addressPostfix, "GlobalBusPublication", regRequest.PublishType, regRequest.PublishAddress);
        var existingReceiverContext = sourcedDeserializedNotifier[receiverContextName];
        if (existingReceiverContext != null)
        {
            if (existingReceiverContext.ExpectedType != sourcedDeserializedNotifier.NotifyingType)
            {
                logger.Warn("Unexpected DeserializedNotifier type {0} does not match existing receiver type {1}",
                    sourcedDeserializedNotifier.NotifyingType, existingReceiverContext.ExpectedType);
                var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
                responseMisMatchTypes.Succeeded = false;
                responseMisMatchTypes.FailureReason = $"Unexpected DeserializedNotifier type {sourcedDeserializedNotifier.NotifyingType} " +
                                                      $"does not match existing receiver type {existingReceiverContext.ExpectedType}";
                return responseMisMatchTypes;
            }

            var responseUpdateExisting = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
            responseUpdateExisting.Succeeded = true;
            if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
            {
                existingReceiverContext.IncrementUsage();
                responseUpdateExisting.UnsubscribeAddress = requestDestinationAddress;
                var unsubscribeCommand = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
                unsubscribeCommand.PublishType = regRequest.PublishType;
                unsubscribeCommand.AddRemoveRegistration = AddRemoveCommand.Remove;
                unsubscribeCommand.PublishAddress = regRequest.PublishAddress;
                responseUpdateExisting.UnsubscribeRequest = unsubscribeCommand;
            }
            else
            {
                existingReceiverContext.DecrementUsage();
            }

            if (existingReceiverContext.UsageCount <= 0)
            {
                sourcedDeserializedNotifier[receiverContextName] = null;
                logger.Info("Removed new global bus broadcast of Message type {0} to {1}",
                    sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            }

            return responseUpdateExisting;
        }

        var responseAddNew = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
        if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
        {
            var broadcastReceiverListenContext = BroadcastReceiverListenContext<object>
                .DynamicBuildTypedBroadcastReceiverListenContext(sourcedDeserializedNotifier.NotifyingType,
                    receiverContextName, Context.MessageBus, regRequest.PublishAddress);

            sourcedDeserializedNotifier[receiverContextName] = broadcastReceiverListenContext;
            logger.Info("Added new global bus broadcast of Message type {0} to {1}",
                sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            responseAddNew.Succeeded = true;
            var unsubscribeCommand = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
            unsubscribeCommand.PublishType = regRequest.PublishType;
            unsubscribeCommand.AddRemoveRegistration = AddRemoveCommand.Remove;
            unsubscribeCommand.PublishAddress = regRequest.PublishAddress;
            responseAddNew.UnsubscribeRequest = unsubscribeCommand;
        }
        else
        {
            logger.Warn("Requested a removal of subscription {0} for a global bus broadcast subscription that does not exist.  " +
                        "There is a mismatch between subscribing and unsubscribing!", addressPostfix);
            responseAddNew.Succeeded = false;
            responseAddNew.FailureReason
                = $"Requested a removal of subscription {addressPostfix} for a global bus broadcast subscription that does not exist." +
                  $"  There is a mismatch between subscribing and unsubscribing!";
        }

        return responseAddNew;
    }

    protected virtual RemoteMessageBusPublishRegistrationResponse CheckUpdateTargetMessageQueuePublishReceiveContext(
        RemoteMessageBusPublishRegistration regRequest,
        string requestDestinationAddress, IDeserializedNotifier sourcedDeserializedNotifier)
    {
        var addressPostfix = ExtractSubscriptionPostfix(requestDestinationAddress);
        var receiverContextName
            = BuildReceiverContextNameFrom(addressPostfix, "TargetMessageQueue_" + regRequest.QueueContext!.RegisteredOn.Name, regRequest.PublishType,
                regRequest.PublishAddress);
        var existingReceiverContext = sourcedDeserializedNotifier[receiverContextName];
        if (existingReceiverContext != null)
        {
            if (existingReceiverContext.ExpectedType != sourcedDeserializedNotifier.NotifyingType)
            {
                logger.Warn("Unexpected DeserializedNotifier type {0} does not match existing receiver type {1}",
                    sourcedDeserializedNotifier.NotifyingType, existingReceiverContext.ExpectedType);
                var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
                responseMisMatchTypes.Succeeded = false;
                responseMisMatchTypes.FailureReason = $"Unexpected DeserializedNotifier type {sourcedDeserializedNotifier.NotifyingType} " +
                                                      $"does not match existing receiver type {existingReceiverContext.ExpectedType}";
                return responseMisMatchTypes;
            }

            var responseUpdateExisting = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
            responseUpdateExisting.Succeeded = true;
            if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
            {
                existingReceiverContext.IncrementUsage();
                responseUpdateExisting.UnsubscribeAddress = requestDestinationAddress;
                var unsubscribeCommand = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
                unsubscribeCommand.PublishType = regRequest.PublishType;
                unsubscribeCommand.AddRemoveRegistration = AddRemoveCommand.Remove;
                unsubscribeCommand.PublishAddress = regRequest.PublishAddress;
                unsubscribeCommand.QueueContext = regRequest.QueueContext;
                responseUpdateExisting.UnsubscribeRequest = unsubscribeCommand;
            }
            else
            {
                existingReceiverContext.DecrementUsage();
            }

            if (existingReceiverContext.UsageCount <= 0)
            {
                sourcedDeserializedNotifier[receiverContextName] = null;
                logger.Info("Removed target MessageQueue {0} broadcast of Message type {1} to {2}",
                    regRequest.QueueContext.RegisteredOn.Name, sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            }

            return responseUpdateExisting;
        }

        var responseAddNew = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
        if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
        {
            var targetQueueReceiverListenContext = TargetedMessageQueueReceiverListenContext<object>
                .DynamicBuildTypedTargetedMessageQueueReceiverListenContext(sourcedDeserializedNotifier.NotifyingType,
                    receiverContextName, regRequest.QueueContext, regRequest.PublishAddress);

            sourcedDeserializedNotifier[receiverContextName] = targetQueueReceiverListenContext;
            logger.Info("Added new target MessageQueue to QueueName {0} broadcast of Message type {1} to {2}",
                regRequest.QueueContext.RegisteredOn.Name, sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            responseAddNew.Succeeded = true;
            var unsubscribeCommand = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
            unsubscribeCommand.PublishType = regRequest.PublishType;
            unsubscribeCommand.AddRemoveRegistration = AddRemoveCommand.Remove;
            unsubscribeCommand.PublishAddress = regRequest.PublishAddress;
            unsubscribeCommand.QueueContext = regRequest.QueueContext;
            responseAddNew.UnsubscribeRequest = unsubscribeCommand;
        }
        else
        {
            logger.Warn("Requested a removal of subscription {0} for a MessageQueue to QueueName {1} subscription that does not exist.  " +
                        "There is a mismatch between subscribing and unsubscribing!", addressPostfix, regRequest.QueueContext.RegisteredOn.Name);

            responseAddNew.Succeeded = false;
            responseAddNew.FailureReason = $"Requested a removal of subscription {addressPostfix} for a MessageQueue to " +
                                           $"QueueName {regRequest.QueueContext.RegisteredOn.Name} subscription that does not exist." +
                                           $"  There is a mismatch between subscribing and unsubscribing!";
        }

        return responseAddNew;
    }

    protected virtual RemoteMessageBusPublishRegistrationResponse CheckUpdateTargetRulePublishReceiveContext(
        RemoteMessageBusPublishRegistration regRequest, string requestDestinationAddress, IDeserializedNotifier sourcedDeserializedNotifier)
    {
        var addressPostfix = ExtractSubscriptionPostfix(requestDestinationAddress);
        var receiverContextName
            = BuildReceiverContextNameFrom(addressPostfix, "TargetRule_" + regRequest.Rule!.FriendlyName, regRequest.PublishType,
                regRequest.PublishAddress);
        var existingReceiverContext = sourcedDeserializedNotifier[receiverContextName];
        if (existingReceiverContext != null)
        {
            if (existingReceiverContext.ExpectedType != sourcedDeserializedNotifier.NotifyingType)
            {
                logger.Warn("Unexpected DeserializedNotifier type {0} does not match existing receiver type {1}",
                    sourcedDeserializedNotifier.NotifyingType, existingReceiverContext.ExpectedType);
                var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
                responseMisMatchTypes.Succeeded = false;
                responseMisMatchTypes.FailureReason = $"Unexpected DeserializedNotifier type {sourcedDeserializedNotifier.NotifyingType} " +
                                                      $"does not match existing receiver type {existingReceiverContext.ExpectedType}";
                return responseMisMatchTypes;
            }

            var responseUpdateExisting = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
            responseUpdateExisting.Succeeded = true;
            if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
            {
                existingReceiverContext.IncrementUsage();
                responseUpdateExisting.UnsubscribeAddress = requestDestinationAddress;
                var unsubscribeCommand = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
                unsubscribeCommand.PublishType = regRequest.PublishType;
                unsubscribeCommand.AddRemoveRegistration = AddRemoveCommand.Remove;
                unsubscribeCommand.PublishAddress = regRequest.PublishAddress;
                unsubscribeCommand.QueueContext = regRequest.QueueContext;
                responseUpdateExisting.UnsubscribeRequest = unsubscribeCommand;
            }
            else
            {
                existingReceiverContext.DecrementUsage();
            }

            if (existingReceiverContext.UsageCount <= 0)
            {
                sourcedDeserializedNotifier[receiverContextName] = null;
                logger.Info("Removed Target Rule {0} broadcast of Message type {0} to {1}",
                    sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            }

            return responseUpdateExisting;
        }

        var responseAddNew = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
        if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
        {
            var targetQueueReceiverListenContext = TargetedRuleReceiverListenContext<object>
                .DynamicBuildTypedTargetedRuleReceiverListenContext(sourcedDeserializedNotifier.NotifyingType,
                    receiverContextName, regRequest.Rule, regRequest.PublishAddress);

            sourcedDeserializedNotifier[receiverContextName] = targetQueueReceiverListenContext;
            logger.Info("Added new target Rule Name {0} broadcast of Message type {1} to {2}", regRequest.Rule.FriendlyName,
                sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            responseAddNew.Succeeded = true;
            var unsubscribeCommand = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistration>();
            unsubscribeCommand.PublishType = regRequest.PublishType;
            unsubscribeCommand.AddRemoveRegistration = AddRemoveCommand.Remove;
            unsubscribeCommand.PublishAddress = regRequest.PublishAddress;
            unsubscribeCommand.QueueContext = regRequest.QueueContext;
            responseAddNew.UnsubscribeRequest = unsubscribeCommand;
        }
        else
        {
            logger.Warn("Requested a removal of subscription {0} for a Rule Name {1} subscription that does not exist.  " +
                        "There is a mismatch between subscribing and unsubscribing!", addressPostfix, regRequest.Rule.FriendlyName);

            responseAddNew.Succeeded = false;
            responseAddNew.FailureReason = $"Requested a removal of subscription {addressPostfix} for a " +
                                           $"Rule Name {regRequest.Rule.FriendlyName} subscription that does not exist.  " +
                                           "There is a mismatch between subscribing and unsubscribing!";
        }

        return responseAddNew;
    }

    protected virtual IDeserializedNotifier? AddOrCreateDeserializedNotifier<TM, TR>(INotifyingMessageDeserializer<TM> messageDeserializer
        , string postfixSubscription,
        RemoteMessageBusPublishRegistration remoteRequestIdResponseRegistration) where TM : class, IVersionedMessage, new()
    {
        var notifierName = BuildNotifierNameFrom(postfixSubscription, typeof(TR));
        var checkExisting = messageDeserializer[notifierName];
        if (checkExisting is IDeserializedNotifier<TM, TR> deserializedNotifier) return deserializedNotifier;
        if (typeof(TM) == typeof(TR))
        {
            var newPassThroughNotifier = new PassThroughDeserializedNotifier<TM>(notifierName, DefaultNotifyTypeFlags)
            {
                RemoveOnZeroSubscribers = false
            };
            messageDeserializer[newPassThroughNotifier.Name] = newPassThroughNotifier;
            return newPassThroughNotifier as IDeserializedNotifier<TM, TR>;
        }

        var resolvedConverter = remoteRequestIdResponseRegistration.Converter as IConverter<TM, TR> ?? ConverterRepository?.GetConverter<TM, TR>();
        if (resolvedConverter != null)
        {
            var convertingNotifier
                = new ConvertingDeserializedNotifier<TM, TR>(notifierName, resolvedConverter, DefaultNotifyTypeFlags)
                {
                    RemoveOnZeroSubscribers = false
                };
            messageDeserializer[convertingNotifier.Name] = convertingNotifier;
            return convertingNotifier;
        }

        logger.Warn("Could not resolve Converter<{0}, {1}> for request {2} on with Deserializer {3} address {4}",
            typeof(TM).Name, typeof(TR).Name, remoteRequestIdResponseRegistration, messageDeserializer, postfixSubscription);

        return null;
    }
}
