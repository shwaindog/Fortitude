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

public class TopicDeserializationRepositoryAmendingRule : Rule
{
    protected readonly IMessageDeserializationRepository CapturedAnyRootDeserializationRepository;
    private readonly IConverterRepository? converterRepository;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TopicDeserializationRepositoryAmendingRule));
    private readonly string? registrationRepoName;

    private readonly ISocketSessionContext socketSessionContext;
    protected ISubscription? ListenForPublishSubscriptions;
    protected ISubscription? ListenForRequestIdResponseRegistration;
    protected IMessageDeserializationRepository? RegisterOnRepository;

    public TopicDeserializationRepositoryAmendingRule(ISocketSessionContext socketSessionContext, string remotePublishRegistrationListenAddress
        , string registerRequestIdResponseListenAddress, IConverterRepository? converterRepository = null, string? registrationRepoName = null)
        : base(remotePublishRegistrationListenAddress)
    {
        this.socketSessionContext = socketSessionContext;
        this.converterRepository = converterRepository;
        this.registrationRepoName = registrationRepoName;
        CapturedAnyRootDeserializationRepository = SocketReceiver?.Decoder?.MessageDeserializationRepository ??
                                                   socketSessionContext.SerdesFactory.MessageDeserializationRepository(
                                                       socketSessionContext.Name +
                                                       "TopicDeserializationRepositoryAmendingRule_RootDeserializerRepository");
        this.socketSessionContext.Connected += RefreshRegistrationRepository;
        RefreshRegistrationRepository();
        RemotePublishRegistrationListenAddress = remotePublishRegistrationListenAddress.Replace("*", "");
        RegisterRequestIdResponseListenAddress = registerRequestIdResponseListenAddress.Replace("*", "");
    }

    protected string RemotePublishRegistrationListenAddress { get; }
    protected string RegisterRequestIdResponseListenAddress { get; }
    private ISocketReceiver? SocketReceiver => socketSessionContext.SocketReceiver;

    protected virtual MessageDeserializerResolveRun NewMessageDeserializerResolveRun =>
        Context.PooledRecycler.Borrow<MessageDeserializerResolveRun>();

    private void RefreshRegistrationRepository()
    {
        var rootReceiverRepo = SocketReceiver?.Decoder?.MessageDeserializationRepository!;
        var foundRepository = rootReceiverRepo.FindConnectedFallbackWithName(registrationRepoName);

        if (foundRepository != null && RegisterOnRepository == null)
        {
            logger.Info("Found registration repository with name {0}.  Will set as deepest MessageDeserializationRepository for Topics under {1}",
                registrationRepoName, RemotePublishRegistrationListenAddress);
            RegisterOnRepository = foundRepository;
        }

        if (foundRepository == null && RegisterOnRepository != null)
        {
            logger.Info("Attaching existing registration repository with name {0} as deepest MessageDeserializationRepository for Topics under {1}",
                registrationRepoName ?? RegisterOnRepository.Name, RemotePublishRegistrationListenAddress);
            rootReceiverRepo.AttachToEndOfConnectedFallbackRepos(RegisterOnRepository);
        }

        if (foundRepository == null && RegisterOnRepository == null)
        {
            logger.Warn(
                "Could not find registration repository with name {0}.  " +
                "Will set deepest MessageDeserializationRepository to a newly created Repository for Topics under {1}",
                registrationRepoName, RemotePublishRegistrationListenAddress);
            RegisterOnRepository = socketSessionContext.SerdesFactory.MessageDeserializationRepository(
                socketSessionContext.Name + "_TopicDeserializationRepositoryAmendingRule_RegistrationRepository");
            rootReceiverRepo.AttachToEndOfConnectedFallbackRepos(RegisterOnRepository);
        }
    }

    public override async ValueTask StartAsync()
    {
        logger.Info("TopicDeserializationRepositoryAmendingRule deployed on {0} and listening to {1} and {2}",
            Context.RegisteredOn.Name, RegisterRequestIdResponseListenAddress, RemotePublishRegistrationListenAddress);
        await LauncherRequestIdResponseListener();
        await LaunchTopicPublicationAmenderListener();
    }

    protected async ValueTask LaunchTopicPublicationAmenderListener()
    {
        ListenForPublishSubscriptions
            = await Context.MessageBus.RegisterRequestListenerAsync<RemoteMessageBusPublishRegistration,
                RemoteMessageBusPublishRegistrationResponse>(this,
                RemotePublishRegistrationListenAddress + "*", UpdatePublishRegistrationRequestReceived);
    }

    protected async ValueTask LauncherRequestIdResponseListener()
    {
        ListenForRequestIdResponseRegistration
            = await Context.MessageBus
                .RegisterRequestListenerAsync<RemoteRequestIdResponseRegistration,
                    RemoteRegistrationResponse>(this,
                    RegisterRequestIdResponseListenAddress + "*", RegisterRequestIdResponseSource);
    }

    protected virtual string ExtractSubscriptionPostfix(string fullMessageAddressDestination) =>
        fullMessageAddressDestination
            .Replace(RemotePublishRegistrationListenAddress, "")
            .Replace(RegisterRequestIdResponseListenAddress, "");

    protected virtual void RuleOverrideDeserializerResolverNoMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun) { }
    protected virtual string BuildNotifierNameFrom(string postfixSubscription, Type publishType) => postfixSubscription + "_" + publishType.Name;

    protected virtual string
        BuildReceiverContextNameFrom(string postfixSubscription, string publicationType, Type publishType, string publishAddress) =>
        postfixSubscription + "_" + publicationType + "_" + publishType.Name + "_" + publishAddress;

    protected virtual RemoteRegistrationResponse RegisterRequestIdResponseSource(
        IBusRespondingMessage<RemoteRequestIdResponseRegistration, RemoteRegistrationResponse> requestMessage)
    {
        var remoteRequestIdResponseRegistration = requestMessage.PayLoad.Body!;
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
                = typeof(TopicDeserializationRepositoryAmendingRule)
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

    protected virtual RemoteMessageBusPublishRegistrationResponse UpdatePublishRegistrationRequestReceived(
        IBusRespondingMessage<RemoteMessageBusPublishRegistration, RemoteMessageBusPublishRegistrationResponse> contextPublishRegistrationMsg)
    {
        var remoteMessageBusPublishRegistration = contextPublishRegistrationMsg.PayLoad.Body!;
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
            var newPassThroughNotifier = new PassThroughDeserializedNotifier<TM>(notifierName, DeserializeTypeFlags.DeserializeConversation)
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

        var resolvedConverter = remoteRequestIdResponseRegistration.Converter as IConverter<TM, TR> ?? converterRepository?.GetConverter<TM, TR>();
        if (resolvedConverter != null)
        {
            var convertingNotifier
                = new ConvertingDeserializedNotifier<TM, TR>(notifierName, resolvedConverter, DeserializeTypeFlags.DeserializeConversation)
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

    protected virtual IDeserializedNotifier? AddOrCreateDeserializedNotifier<TM, TR>(INotifyingMessageDeserializer<TM> messageDeserializer
        , string postfixSubscription,
        RemoteRequestIdResponseRegistration remoteRequestIdResponseRegistration) where TM : class, IVersionedMessage, new()
    {
        var notifierName = BuildNotifierNameFrom(postfixSubscription, typeof(TR));
        var checkExisting = messageDeserializer[notifierName];
        if (checkExisting is IDeserializedNotifier<TM, TR> deserializedNotifier) return deserializedNotifier;
        if (typeof(TM) == typeof(TR))
        {
            var newPassThroughNotifier = new PassThroughDeserializedNotifier<TM>(notifierName, DeserializeTypeFlags.DeserializeConversation)
            {
                RemoveOnZeroSubscribers = false
            };
            newPassThroughNotifier.RegisterMessageDeserializer(messageDeserializer);
            newPassThroughNotifier.AddRequestExpected(remoteRequestIdResponseRegistration.RequestId
                , remoteRequestIdResponseRegistration.ResponseSource);
            return newPassThroughNotifier as IDeserializedNotifier<TM, TR>;
        }

        var resolvedConverter = remoteRequestIdResponseRegistration.Converter as IConverter<TM, TR> ?? converterRepository?.GetConverter<TM, TR>();
        if (resolvedConverter != null)
        {
            var convertingNotifier
                = new ConvertingDeserializedNotifier<TM, TR>(notifierName, resolvedConverter, DeserializeTypeFlags.DeserializeConversation)
                {
                    RemoveOnZeroSubscribers = false
                };
            convertingNotifier.RegisterMessageDeserializer(messageDeserializer);
            convertingNotifier.AddRequestExpected(remoteRequestIdResponseRegistration.RequestId, remoteRequestIdResponseRegistration.ResponseSource);
            return convertingNotifier;
        }

        logger.Warn("Could not resolve Converter<{0}, {1}> for request {2} on with Deserializer {3} address {4}",
            typeof(TM).Name, typeof(TR).Name, remoteRequestIdResponseRegistration, messageDeserializer, postfixSubscription);

        return null;
    }

    protected virtual void ResolveOrAttemptCreateMessageDeserializer(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        if (messageDeserializerResolveRun.MessageId != null) AttemptResolveDeserializerWithMessageId(messageDeserializerResolveRun);
        if (!messageDeserializerResolveRun.ContinueSearching) return;
        AttemptRuleSpecificOverrideNoKnownMessageId(messageDeserializerResolveRun);
        if (messageDeserializerResolveRun is { ContinueSearching: false, RootMessageDeserializationRepositoryIsFactoryRepository: false }) return;
        AttemptResolveMessageDeserializerFromTypesAddPublishTypeAsDeserializedType(messageDeserializerResolveRun);
        if (!messageDeserializerResolveRun.ContinueSearching) return;
        AttemptAllRegisteredConvertersFindMessageDeserializer(messageDeserializerResolveRun);
    }

    protected virtual void AttemptRuleSpecificOverrideNoKnownMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        RuleOverrideDeserializerResolverNoMessageId(messageDeserializerResolveRun);
        if (messageDeserializerResolveRun.HaveBothMessageDeserializerAndMessageId)
        {
            var checkExisting
                = messageDeserializerResolveRun.RootMessageDeserializationRepository.GetDeserializer(messageDeserializerResolveRun.MessageId!.Value);
            if (checkExisting is INotifyingMessageDeserializer existingNotifyingMessageDeserializer)
            {
                messageDeserializerResolveRun.MessageDeserializer = existingNotifyingMessageDeserializer;
                return;
            }

            ;
            RegisterOnRepository!.RegisterDeserializer(messageDeserializerResolveRun.MessageId.Value
                , messageDeserializerResolveRun.MessageDeserializer!);
        }
    }

    protected virtual void AttemptResolveDeserializerWithMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        messageDeserializerResolveRun.MessageDeserializer = messageDeserializerResolveRun.RootMessageDeserializationRepository
            .GetDeserializer(messageDeserializerResolveRun.MessageId!.Value) as INotifyingMessageDeserializer;
        if (!messageDeserializerResolveRun.ContinueSearching) return;

        messageDeserializerResolveRun.MessageDeserializer
            = RegisterOnRepository?.GetDeserializer(messageDeserializerResolveRun.MessageId!.Value) as INotifyingMessageDeserializer;
        if (!messageDeserializerResolveRun.ContinueSearching) return;

        FallbackResolveAttemptWithMessageId(messageDeserializerResolveRun);
        if (messageDeserializerResolveRun.HaveBothMessageDeserializerAndMessageId)
            RegisterOnRepository!.RegisterDeserializer(messageDeserializerResolveRun.MessageId!.Value
                , messageDeserializerResolveRun.MessageDeserializer!);
    }

    protected virtual void FallbackResolveAttemptWithMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        if (!messageDeserializerResolveRun.RootMessageDeserializationRepositoryIsFactoryRepository) return;
        var factoryRepository = messageDeserializerResolveRun.RootMessageDeserializationFactoryRepository!;
        var msgId = messageDeserializerResolveRun.MessageId!.Value;
        var deserializedType = messageDeserializerResolveRun.RemoteNotificationRegistration.DeserializedType ??
                               messageDeserializerResolveRun.RemoteNotificationRegistration.Converter?.FromType;
        if (deserializedType != null)
            messageDeserializerResolveRun.MessageDeserializer = factoryRepository
                .SourceDeserializerFromMessageId(msgId, deserializedType) as INotifyingMessageDeserializer;
        else if (converterRepository != null)
            foreach (var converter in converterRepository.GetConvertersWithToType(messageDeserializerResolveRun.PublishType))
            {
                if (messageDeserializerResolveRun.RootMessageDeserializationFactoryRepository!
                        .SourceDeserializerFromMessageId(msgId, converter.FromType) is INotifyingMessageDeserializer
                    checkDeserializer)
                {
                    messageDeserializerResolveRun.MessageDeserializer = checkDeserializer;
                    break;
                }
            }
        else
            messageDeserializerResolveRun.MessageDeserializer = factoryRepository.SourceDeserializerFromMessageId(msgId,
                messageDeserializerResolveRun!.PublishType) as INotifyingMessageDeserializer;
    }

    protected virtual void AttemptResolveMessageDeserializerFromTypesAddPublishTypeAsDeserializedType(
        MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        if (!messageDeserializerResolveRun.RootMessageDeserializationRepositoryIsFactoryRepository) return;
        var tryDeserializedType
            = messageDeserializerResolveRun.DeserializedType ??
              messageDeserializerResolveRun.Converter?.FromType ?? messageDeserializerResolveRun.PublishType;
        messageDeserializerResolveRun.MessageId
            = messageDeserializerResolveRun.RootMessageDeserializationFactoryRepository!.ResolveExpectedMessageIdForMessageType(tryDeserializedType);
        if (messageDeserializerResolveRun.ContinueSearching) AttemptResolveDeserializerWithMessageId(messageDeserializerResolveRun);
    }

    protected virtual void AttemptAllRegisteredConvertersFindMessageDeserializer(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        if (!messageDeserializerResolveRun.RootMessageDeserializationRepositoryIsFactoryRepository) return;
        if (converterRepository != null)
            foreach (var converter in converterRepository.GetConvertersWithToType(messageDeserializerResolveRun.PublishType))
            {
                var expectedMessageId
                    = messageDeserializerResolveRun.RootMessageDeserializationFactoryRepository!.ResolveExpectedMessageIdForMessageType(
                        converter.FromType);
                if (expectedMessageId == null) continue;
                AttemptResolveDeserializerWithMessageId(messageDeserializerResolveRun);
                if (!messageDeserializerResolveRun.ContinueSearching) return;
            }
    }
}
