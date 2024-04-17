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
using NLog;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;

public class TopicDeserializationRepositoryAmendingRule : Rule
{
    private IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TopicDeserializationRepositoryAmendingRule));

    private readonly ISocketSessionContext socketSessionContext;
    private readonly IConverterRepository? converterRepository;
    private readonly string? registrationRepoName;
    private readonly IMessageDeserializationRepository capturedAnyRootDeserializationRepository;
    private IMessageDeserializationRepository? registerOnRepository;
    private ISubscription? listenForPublishSubscriptions;
    private ISubscription? listenForRequestIdResponseRegistration;

    public TopicDeserializationRepositoryAmendingRule(ISocketSessionContext socketSessionContext, string remotePublishRegistrationListenAddress
        , string registerRequestIdResponseListenAddress, IConverterRepository? converterRepository = null, string? registrationRepoName = null) : base(remotePublishRegistrationListenAddress)
    {
        this.socketSessionContext = socketSessionContext;
        this.converterRepository = converterRepository;
        this.registrationRepoName = registrationRepoName;
        capturedAnyRootDeserializationRepository = SocketReceiver?.Decoder?.MessageDeserializationRepository ??
                                                   socketSessionContext.SerdesFactory.MessageDeserializationRepository(
                                                       socketSessionContext.Name +
                                                       "TopicDeserializationRepositoryAmendingRule_RootDeserializerRepository");
        this.socketSessionContext.Connected += RefreshRegistrationRepository;
        RefreshRegistrationRepository();
        RemotePublishRegistrationListenAddress = remotePublishRegistrationListenAddress.Replace("*", "");
        RegisterRequestIdResponseListenAddress = registerRequestIdResponseListenAddress.Replace("*", "");
    }

    private void RefreshRegistrationRepository()
    {
        var rootReceiverRepo = SocketReceiver?.Decoder?.MessageDeserializationRepository!;
        var foundRepository = rootReceiverRepo.FindConnectedFallbackWithName(registrationRepoName);

        if (foundRepository != null && registerOnRepository == null)
        {
            logger.Info("Found registration repository with name {0}.  Will set as deepest MessageDeserializationRepository for Topics under {1}",
                registrationRepoName, RemotePublishRegistrationListenAddress);
            registerOnRepository = foundRepository;
        }

        if (foundRepository == null && registerOnRepository != null)
        {
            logger.Info("Attaching existing registration repository with name {0} as deepest MessageDeserializationRepository for Topics under {1}",
                registrationRepoName ?? registerOnRepository.Name, RemotePublishRegistrationListenAddress);
            rootReceiverRepo.AttachToEndOfConnectedFallbackRepos(registerOnRepository);
        }

        if (foundRepository == null && registerOnRepository == null)
        {
            logger.Warn(
                "Could not find registration repository with name {0}.  Will set deepest MessageDeserializationRepository to a newly created Repository for Topics under {1}"
                ,
                registrationRepoName, RemotePublishRegistrationListenAddress);
            registerOnRepository = socketSessionContext.SerdesFactory.MessageDeserializationRepository(
                socketSessionContext.Name + "_TopicDeserializationRepositoryAmendingRule_RegistrationRepository");
            rootReceiverRepo.AttachToEndOfConnectedFallbackRepos(registerOnRepository);
        }
    }

    private string RemotePublishRegistrationListenAddress { get; }
    private string RegisterRequestIdResponseListenAddress { get; }
    private ISocketReceiver? SocketReceiver => socketSessionContext.SocketReceiver;

    public override async ValueTask StartAsync()
    {
        logger.Info("TopicDeserializationRepositoryAmendingRule deployed on {0} and listening to {1} and {2}", 
            Context.RegisteredOn.Name, RegisterRequestIdResponseListenAddress, RemotePublishRegistrationListenAddress);
        listenForRequestIdResponseRegistration
            = await Context.MessageBus
                .RegisterRequestListenerAsync<RemoteRequestIdResponseRegistration, RemoteRegistrationResponse>(this
                    , RegisterRequestIdResponseListenAddress + "*", RegisterRequestIdResponseSource);
        listenForPublishSubscriptions
            = await Context.MessageBus.RegisterRequestListenerAsync<RemoteMessageBusPublishRegistration, RemoteMessageBusPublishRegistrationResponse>(this, RemotePublishRegistrationListenAddress + "*"
                , UpdateRequest);
    }

    private RemoteRegistrationResponse RegisterRequestIdResponseSource(
        IBusRespondingMessage<RemoteRequestIdResponseRegistration, RemoteRegistrationResponse> requestMessage)
    {
        var remoteRequestIdResponseRegistration = requestMessage.PayLoad.Body!;
        var addressPostfix = ExtractSubscriptionPostfix(requestMessage.DestinationAddress!);
        remoteRequestIdResponseRegistration.DeserializedType ??= remoteRequestIdResponseRegistration.ResponseSource.ResponseType;

        var messageDeserializer = ResolveOrAttemptCreateMessageDeserializer(addressPostfix, remoteRequestIdResponseRegistration
            , remoteRequestIdResponseRegistration.ResponseSource.ResponseType);
        if (messageDeserializer == null)
        {
            var response = Context.PooledRecycler.Borrow<RemoteRegistrationResponse>();
            response.Succeeded = false;
            response.FailureReason
                = $"Could not find or resolve deserializer for {remoteRequestIdResponseRegistration} as a INotifyingMessageDeserializer";
            return response;
        }

        MethodInfo unTypedMethodInfo
            = typeof(TopicDeserializationRepositoryAmendingRule).GetMethod(nameof(AddOrCreateDeserializedNotifierReturnRegistrationResponse), BindingFlags.NonPublic | BindingFlags.Instance)!;
        MethodInfo genericMethodInfo = unTypedMethodInfo.MakeGenericMethod(messageDeserializer.MessageType, remoteRequestIdResponseRegistration.ResponseSource.ResponseType)!;

        var responseRegisterRequestId = (RemoteRegistrationResponse)genericMethodInfo!.Invoke(this, [messageDeserializer, addressPostfix, remoteRequestIdResponseRegistration])!;

        return responseRegisterRequestId;
    }

    private RemoteMessageBusPublishRegistrationResponse UpdateRequest(IBusRespondingMessage<RemoteMessageBusPublishRegistration, RemoteMessageBusPublishRegistrationResponse> contextPublishRegistrationMsg)
    {
        var remoteMessageBusPublishRegistration = contextPublishRegistrationMsg.PayLoad.Body!;
        var addressPostfix = ExtractSubscriptionPostfix(contextPublishRegistrationMsg.DestinationAddress!);
        try
        {
            var messageDeserializer = ResolveOrAttemptCreateMessageDeserializer(addressPostfix, remoteMessageBusPublishRegistration
                , remoteMessageBusPublishRegistration.PublishType);
            if (messageDeserializer == null)
            {
                logger.Warn("Could not resolve MessageDeserializer for request {0} on address {1}", remoteMessageBusPublishRegistration
                    , contextPublishRegistrationMsg.DestinationAddress);
                var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
                responseMisMatchTypes.Succeeded = false;
                responseMisMatchTypes.FailureReason = $"Could not resolve MessageDeserializer for request {remoteMessageBusPublishRegistration}" +
                                                      $" on address {contextPublishRegistrationMsg.DestinationAddress}";
                return responseMisMatchTypes;
            }

            MethodInfo unTypedMethodInfo = GetType().GetMethod(nameof(AddOrCreateDeserializedNotifier))!;
            MethodInfo genericMethodInfo = unTypedMethodInfo.MakeGenericMethod(
                messageDeserializer.MessageType
                , remoteMessageBusPublishRegistration.PublishType)!;

            var sourcedDeserializedNotifier
                = (IDeserializedNotifier)genericMethodInfo!.Invoke(this, [messageDeserializer, addressPostfix, remoteMessageBusPublishRegistration])!;
            if (sourcedDeserializedNotifier == null)
            {
                logger.Warn("Could not resolve DeserializerNotifier for request {0} on address {1}", remoteMessageBusPublishRegistration
                    , contextPublishRegistrationMsg.DestinationAddress);
                var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
                responseMisMatchTypes.Succeeded = false;
                responseMisMatchTypes.FailureReason
                    = $"Could not resolve DeserializerNotifier for request {remoteMessageBusPublishRegistration} on address {contextPublishRegistrationMsg.DestinationAddress}";
                return responseMisMatchTypes;
            }

            if (remoteMessageBusPublishRegistration.Rule == null && remoteMessageBusPublishRegistration.QueueContext == null)
            {
                return CheckUpdateGlobalMessageBusPublishReceiveContext(remoteMessageBusPublishRegistration, addressPostfix
                    , sourcedDeserializedNotifier);
            }

            if (remoteMessageBusPublishRegistration.Rule == null && remoteMessageBusPublishRegistration.QueueContext != null)
            {
                return CheckUpdateTargetMessageQueuePublishReceiveContext(remoteMessageBusPublishRegistration, addressPostfix
                    , sourcedDeserializedNotifier);
            }

            return CheckUpdateTargetRulePublishReceiveContext(remoteMessageBusPublishRegistration, addressPostfix
                , sourcedDeserializedNotifier);
        }
        catch (Exception ex)
        {
            logger.Warn("Caught exception attempting to change publication subscription on topic {0}. With request {1}.  Got {2}", 
                contextPublishRegistrationMsg.DestinationAddress, remoteMessageBusPublishRegistration, ex);
            var responseMisMatchTypes = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
            responseMisMatchTypes.Succeeded = false;
            responseMisMatchTypes.FailureReason = $"Caught exception attempting to change publication subscription on topic {contextPublishRegistrationMsg.DestinationAddress}. " +
                                                  $"With request {remoteMessageBusPublishRegistration}.  Got {ex}";
            return responseMisMatchTypes;
        }
    }

    private RemoteMessageBusPublishRegistrationResponse CheckUpdateGlobalMessageBusPublishReceiveContext(RemoteMessageBusPublishRegistration regRequest
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
                logger.Warn("Unexpected DeserializedNotifier type {0} does not match existing receiver type {1}", sourcedDeserializedNotifier.NotifyingType, existingReceiverContext.ExpectedType);
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
                logger.Info("Removed new global bus broadcast of Message type {0} to {1}", sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            }

            return responseUpdateExisting;
        }
        
        var responseAddNew = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
        if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
        {
            var broadcastReceiverListenContext = BroadcastReceiverListenContext<object>
                .DynamicBuildTypedBroadcastReceiverListenContext(sourcedDeserializedNotifier.NotifyingType, receiverContextName, Context.MessageBus, regRequest.PublishAddress);

            sourcedDeserializedNotifier[receiverContextName] = broadcastReceiverListenContext;
            logger.Info("Added new global bus broadcast of Message type {0} to {1}", sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
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
            responseAddNew.FailureReason = $"Requested a removal of subscription {addressPostfix} for a global bus broadcast subscription that does not exist." +
                $"  There is a mismatch between subscribing and unsubscribing!";

        }
        return responseAddNew;
    }

    private RemoteMessageBusPublishRegistrationResponse CheckUpdateTargetMessageQueuePublishReceiveContext(RemoteMessageBusPublishRegistration regRequest, 
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
                logger.Warn("Unexpected DeserializedNotifier type {0} does not match existing receiver type {1}", sourcedDeserializedNotifier.NotifyingType, existingReceiverContext.ExpectedType);
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
                logger.Info("Removed target MessageQueue {0} broadcast of Message type {1} to {2}",regRequest.QueueContext.RegisteredOn.Name, sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            }
            
            return responseUpdateExisting;
        }
        
        var responseAddNew = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
        if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
        {
            var targetQueueReceiverListenContext = TargetedMessageQueueReceiverListenContext<object>
                .DynamicBuildTypedTargetedMessageQueueReceiverListenContext(sourcedDeserializedNotifier.NotifyingType, receiverContextName, regRequest.QueueContext, regRequest.PublishAddress);

            sourcedDeserializedNotifier[receiverContextName] = targetQueueReceiverListenContext;
            logger.Info("Added new target MessageQueue to QueueName {0} broadcast of Message type {1} to {2}", regRequest.QueueContext.RegisteredOn.Name, sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
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
            responseAddNew.FailureReason = $"Requested a removal of subscription {addressPostfix} for a MessageQueue to QueueName {regRequest.QueueContext.RegisteredOn.Name} subscription that does not exist." +
                                           $"  There is a mismatch between subscribing and unsubscribing!";
        }
        return responseAddNew;
    }

    private RemoteMessageBusPublishRegistrationResponse CheckUpdateTargetRulePublishReceiveContext(RemoteMessageBusPublishRegistration regRequest, string requestDestinationAddress, IDeserializedNotifier sourcedDeserializedNotifier)
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
                logger.Warn("Unexpected DeserializedNotifier type {0} does not match existing receiver type {1}", sourcedDeserializedNotifier.NotifyingType, existingReceiverContext.ExpectedType);
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
                logger.Info("Removed Target Rule {0} broadcast of Message type {0} to {1}", sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
            }
            
            return responseUpdateExisting;
        }
        
        var responseAddNew = Context.PooledRecycler.Borrow<RemoteMessageBusPublishRegistrationResponse>();
        if (regRequest.AddRemoveRegistration == AddRemoveCommand.Add)
        {
            var targetQueueReceiverListenContext = TargetedRuleReceiverListenContext<object>
                .DynamicBuildTypedTargetedRuleReceiverListenContext(sourcedDeserializedNotifier.NotifyingType, receiverContextName, regRequest.Rule, regRequest.PublishAddress);

            sourcedDeserializedNotifier[receiverContextName] = targetQueueReceiverListenContext;
            logger.Info("Added new target Rule Name {0} broadcast of Message type {1} to {2}", regRequest.Rule.FriendlyName, sourcedDeserializedNotifier.NotifyingType, regRequest.PublishAddress);
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
            responseAddNew.FailureReason = $"Requested a removal of subscription {addressPostfix} for a Rule Name {regRequest.Rule.FriendlyName} subscription that does not exist.  " +
                                           "There is a mismatch between subscribing and unsubscribing!";
        }
        return responseAddNew;
    }

    protected virtual RemoteRegistrationResponse AddOrCreateDeserializedNotifierReturnRegistrationResponse<TM, TR>(INotifyingMessageDeserializer<TM> messageDeserializer, string postfixSubscription, 
        RemoteRequestIdResponseRegistration remoteRequestIdResponseRegistration) where TM : class, IVersionedMessage, new()
    {
        var notifierName = BuildNotifierNameFrom(postfixSubscription, typeof(TR));
        var checkExisting = messageDeserializer[notifierName];
        if (checkExisting is IDeserializedNotifier<TM, TR> deserializedNotifier)
        {
            deserializedNotifier.AddRequestExpected(remoteRequestIdResponseRegistration.RequestId, remoteRequestIdResponseRegistration.ResponseSource);
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
            newPassThroughNotifier.AddRequestExpected(remoteRequestIdResponseRegistration.RequestId, remoteRequestIdResponseRegistration.ResponseSource);
            var response = Context.PooledRecycler.Borrow<RemoteRegistrationResponse>();
            response.Succeeded = true;
            return response;
        }
        var resolvedConverter = remoteRequestIdResponseRegistration.Converter as IConverter<TM, TR> ?? converterRepository?.GetConverter<TM, TR>();
        if (resolvedConverter != null)
        {
            var convertingNotifier = new ConvertingDeserializedNotifier<TM, TR>(notifierName, resolvedConverter, DeserializeTypeFlags.DeserializeConversation)
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
            = $"Could not resolve a converter for INotifyingMessageDeserializer<{typeof(TM).Name} and ConvertingDeserializedNotifier<{typeof(TM).Name}, {typeof(TR).Name}>";
        return failedResponse;
    }

    protected virtual IDeserializedNotifier? AddOrCreateDeserializedNotifier<TM, TR>(INotifyingMessageDeserializer<TM> messageDeserializer, string postfixSubscription, 
        RemoteRequestIdResponseRegistration remoteRequestIdResponseRegistration) where TM : class, IVersionedMessage, new()
    {
        var notifierName = BuildNotifierNameFrom(postfixSubscription, typeof(TR));
        var checkExisting = messageDeserializer[notifierName];
        if (checkExisting is IDeserializedNotifier<TM, TR> deserializedNotifier)
        {
            return deserializedNotifier;
        }
        if (typeof(TM) == typeof(TR))
        {
            var newPassThroughNotifier = new PassThroughDeserializedNotifier<TM>(notifierName, DeserializeTypeFlags.DeserializeConversation)
                {
                    RemoveOnZeroSubscribers = false
                };
            newPassThroughNotifier.RegisterMessageDeserializer(messageDeserializer);
            newPassThroughNotifier.AddRequestExpected(remoteRequestIdResponseRegistration.RequestId, remoteRequestIdResponseRegistration.ResponseSource);
            return newPassThroughNotifier as IDeserializedNotifier<TM, TR>;
        }
        var resolvedConverter = remoteRequestIdResponseRegistration.Converter as IConverter<TM, TR> ?? converterRepository?.GetConverter<TM, TR>();
        if (resolvedConverter != null)
        {
            var convertingNotifier = new ConvertingDeserializedNotifier<TM, TR>(notifierName, resolvedConverter, DeserializeTypeFlags.DeserializeConversation)
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
    

    protected virtual string BuildNotifierNameFrom(string postfixSubscription, Type publishType)
    {
        return postfixSubscription + "_" + publishType.Name;
    }
    protected virtual string BuildReceiverContextNameFrom(string postfixSubscription, string publicationType, Type expectedType, string publishAddress)
    {
        return postfixSubscription + "_" + publicationType + "_" + expectedType.Name + "_" + publishAddress;
    }


    protected virtual INotifyingMessageDeserializer? ResolveOrAttemptCreateMessageDeserializer(string subscriptionPostfix
        , RemoteNotificationRegistration remoteNotificationRegistration, Type? publishType = null)
    {
        var decoderRootDeserializerRepo = SocketReceiver?.Decoder?.MessageDeserializationRepository ?? capturedAnyRootDeserializationRepository!;
        var msgId = remoteNotificationRegistration.MessageId;
        if (msgId != null)
            return AttemptResolveDeserializerWithMessageId(subscriptionPostfix, remoteNotificationRegistration, msgId.Value, publishType);
        if (decoderRootDeserializerRepo is not IMessageDeserializerFactoryRepository factoryRepository) return null;
        var deserializedType = remoteNotificationRegistration.DeserializedType ?? remoteNotificationRegistration.Converter?.FromType;
        if (deserializedType != null)
        {
            var expectedMessageId = factoryRepository.ResolveExpectedMessageIdForMessageType(deserializedType);
            if (expectedMessageId != null)
                return AttemptResolveDeserializerWithMessageId(subscriptionPostfix, remoteNotificationRegistration, expectedMessageId.Value
                    , publishType);
        }
        else if (converterRepository != null && publishType != null)
        {
            foreach (var converter in converterRepository.GetConvertersWithToType(publishType))
            {
                var expectedMessageId = factoryRepository.ResolveExpectedMessageIdForMessageType(converter.FromType);
                if (expectedMessageId == null) continue;
                var attemptGetDeserializer = AttemptResolveDeserializerWithMessageId(subscriptionPostfix, remoteNotificationRegistration
                    , expectedMessageId.Value, publishType);
                if (attemptGetDeserializer == null) continue;
                return attemptGetDeserializer;
            }
        }

        return null;
    }

    protected virtual INotifyingMessageDeserializer? AttemptResolveDeserializerWithMessageId(string subscriptionPostfix
        , RemoteNotificationRegistration remoteNotificationRegistration, uint msgId, Type? publishType = null)
    {
        INotifyingMessageDeserializer? registerDeserializer = null;
        var deserializerRepo = SocketReceiver?.Decoder?.MessageDeserializationRepository;
        var existingDeserializer = deserializerRepo.GetDeserializer(msgId);
        if (existingDeserializer is INotifyingMessageDeserializer deserializer) return deserializer;
        existingDeserializer = registerOnRepository?.GetDeserializer(msgId);
        if (existingDeserializer is INotifyingMessageDeserializer registrationDeserializer) return registrationDeserializer;
        registerDeserializer = RuleSpecificDeserializerResolver(subscriptionPostfix, remoteNotificationRegistration, msgId, publishType)
                               ?? FallbackResolveAttemptWithMessageId(remoteNotificationRegistration, msgId, publishType);

        if (registerDeserializer != null)
        {

            registerOnRepository!.RegisterDeserializer(msgId, registerDeserializer);
            return registerDeserializer;
        }

        return null;
    }

    protected virtual INotifyingMessageDeserializer? FallbackResolveAttemptWithMessageId(RemoteNotificationRegistration remoteNotificationRegistration
        , uint msgId, Type? publishType = null)
    {
        var decoderRootdeserializerRepo = SocketReceiver?.Decoder?.MessageDeserializationRepository ?? capturedAnyRootDeserializationRepository!;
        INotifyingMessageDeserializer? returnDeserializer = null;
        if (decoderRootdeserializerRepo is IMessageDeserializerFactoryRepository factoryRepository)
        {
            var deserializedType = remoteNotificationRegistration.DeserializedType ?? remoteNotificationRegistration.Converter?.FromType;
            if (deserializedType != null)
                returnDeserializer = factoryRepository.SourceDeserializerFromMessageId(msgId, deserializedType) as INotifyingMessageDeserializer;
            else if (converterRepository != null && publishType != null)
                foreach (var converter in converterRepository.GetConvertersWithToType(publishType))
                {
                    if (factoryRepository.SourceDeserializerFromMessageId(msgId, converter.FromType) is INotifyingMessageDeserializer checkDeserializer)
                    {
                        returnDeserializer = checkDeserializer;
                        break;
                    }
                }
        }

        return returnDeserializer;
    }

    protected virtual string ExtractSubscriptionPostfix(string fullMessageAddressDestination) =>
        fullMessageAddressDestination
            .Replace(RemotePublishRegistrationListenAddress, "")
            .Replace(RegisterRequestIdResponseListenAddress, "");

    protected virtual INotifyingMessageDeserializer? RuleSpecificDeserializerResolver(RemoteMessageBusPublishRegistration remoteMessageBusPublishRegistration) => null;

    protected virtual INotifyingMessageDeserializer? RuleSpecificDeserializerResolver(string subscriptionPostfix
        , RemoteNotificationRegistration remoteNotificationRegistration, uint? msgId = null, Type? publishType = null) =>
        null;
}
