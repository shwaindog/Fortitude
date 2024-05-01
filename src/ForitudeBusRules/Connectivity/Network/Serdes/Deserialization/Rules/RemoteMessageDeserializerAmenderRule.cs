#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;

public class RemoteMessageDeserializerAmenderRule : Rule
{
    protected readonly IMessageDeserializationRepository CapturedAnyRootDeserializationRepository;
    protected readonly IConverterRepository? ConverterRepository;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(RemoteMessageDeserializerAmenderRule));
    protected readonly string? RegistrationRepoName;
    private readonly ISocketSessionContext socketSessionContext;
    protected string ListeningOnAddress;
    public Action<IMessageDeserializer> MessageDeserializerAdded;

    public Action<IVersionedMessage, int, IMessageDeserializer> MessageReceived;
    protected IMessageDeserializationRepository? RegisterOnRepository;

    public RemoteMessageDeserializerAmenderRule(string ruleName, ISocketSessionContext relatedSocketSession
        , string listeningOnAddress, IConverterRepository? converterRepository = null, string? registrationRepoName = null) : base(ruleName)
    {
        socketSessionContext = relatedSocketSession;
        RegistrationRepoName = registrationRepoName;
        ListeningOnAddress = listeningOnAddress;
        ConverterRepository = converterRepository;
        CapturedAnyRootDeserializationRepository = relatedSocketSession.SocketReceiver?.Decoder?.MessageDeserializationRepository ??
                                                   relatedSocketSession.SerdesFactory.MessageDeserializationRepository(
                                                       relatedSocketSession.Name + "_" +
                                                       ruleName + "_RootDeserializerRepository");
        socketSessionContext.Connected += SetSocketSessionContextFindRegistrationRepository;
        SetSocketSessionContextFindRegistrationRepository();
        MessageReceived += MessageReceivedHandler;
        RegisterOnRepository!.MessageDeserializerRegistered += MessageDeserializerRegistered;
    }

    private void SetSocketSessionContextFindRegistrationRepository()
    {
        var rootReceiverRepo = socketSessionContext.SocketReceiver!.Decoder?.MessageDeserializationRepository!;
        var foundRepository = rootReceiverRepo.FindConnectedFallbackWithName(RegistrationRepoName);

        if (foundRepository != null && RegisterOnRepository == null) RegisterOnRepository = foundRepository;

        if (foundRepository == null && RegisterOnRepository != null)
        {
            logger.Info("Attaching existing registration repository with name {0} as deepest MessageDeserializationRepository for Topics under {1}",
                RegistrationRepoName ?? RegisterOnRepository.Name, ListeningOnAddress);
            rootReceiverRepo.AttachToEndOfConnectedFallbackRepos(RegisterOnRepository);
        }

        if (foundRepository == null && RegisterOnRepository == null)
        {
            logger.Warn(
                "Could not find registration repository with name {0}.  " +
                "Will set deepest MessageDeserializationRepository to a newly created Repository for Topics under {1}",
                RegistrationRepoName, ListeningOnAddress);
            RegisterOnRepository = socketSessionContext.SerdesFactory.MessageDeserializationRepository(
                FriendlyName + "_TopicDeserializationRepositoryAmendingRule_RegistrationRepository");
            rootReceiverRepo.AttachToEndOfConnectedFallbackRepos(RegisterOnRepository);
        }
    }

    protected virtual void MessageDeserializerRegistered(IMessageDeserializer newlyRegisteredMessageDeserializer) { }

    protected virtual void MessageReceivedHandler(IVersionedMessage deserializedMessage, int deserializedCount, IMessageDeserializer deserializer) { }

    protected virtual void RuleOverrideDeserializerResolverNoMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun) { }

    protected virtual void ResolveOrAttemptCreateMessageDeserializer(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        if (messageDeserializerResolveRun.MessageId != null) AttemptResolveDeserializerWithMessageId(messageDeserializerResolveRun);
        if (!messageDeserializerResolveRun.ContinueSearching) return;
        AttemptRuleSpecificOverrideNoKnownMessageId(messageDeserializerResolveRun);
        if (!messageDeserializerResolveRun.ContinueSearching) return;
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
            var checkExisting = RegisterOnRepository!.GetDeserializer(messageDeserializerResolveRun.MessageId!.Value);
            if (checkExisting is INotifyingMessageDeserializer existingNotifyingMessageDeserializer)
            {
                if (!existingNotifyingMessageDeserializer.IsRegistered(MessageReceivedHandler))
                    existingNotifyingMessageDeserializer.MessageDeserialized += MessageReceivedHandler;
                messageDeserializerResolveRun.MessageDeserializer = existingNotifyingMessageDeserializer;
                return;
            }

            messageDeserializerResolveRun.MessageDeserializer!.MessageDeserialized += MessageReceivedHandler;
            RegisterOnRepository!.RegisterDeserializer(messageDeserializerResolveRun.MessageId.Value
                , messageDeserializerResolveRun.MessageDeserializer!);
        }
    }

    protected virtual void AttemptResolveDeserializerWithMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        messageDeserializerResolveRun.MessageDeserializer = messageDeserializerResolveRun.RootMessageDeserializationRepository
            .GetDeserializer(messageDeserializerResolveRun.MessageId!.Value) as INotifyingMessageDeserializer;
        if (!messageDeserializerResolveRun.ContinueSearching)
        {
            if (!messageDeserializerResolveRun.HaveBothMessageDeserializerAndMessageId) return;
            var existingNotifyingMessageDeserializer = messageDeserializerResolveRun.MessageDeserializer!;
            if (!existingNotifyingMessageDeserializer.IsRegistered(MessageReceivedHandler))
                existingNotifyingMessageDeserializer.MessageDeserialized += MessageReceivedHandler;
            messageDeserializerResolveRun.MessageDeserializer = existingNotifyingMessageDeserializer;
            return;
        }

        messageDeserializerResolveRun.MessageDeserializer
            = RegisterOnRepository?.GetDeserializer(messageDeserializerResolveRun.MessageId!.Value) as INotifyingMessageDeserializer;
        if (!messageDeserializerResolveRun.ContinueSearching) return;

        FallbackResolveAttemptWithMessageId(messageDeserializerResolveRun);
        if (messageDeserializerResolveRun.HaveBothMessageDeserializerAndMessageId)
        {
            messageDeserializerResolveRun.MessageDeserializer!.MessageDeserialized += MessageReceivedHandler;
            RegisterOnRepository!.RegisterDeserializer(messageDeserializerResolveRun.MessageId!.Value
                , messageDeserializerResolveRun.MessageDeserializer!);
        }
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
        else if (ConverterRepository != null)
            foreach (var converter in ConverterRepository.GetConvertersWithToType(messageDeserializerResolveRun.PublishType))
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
        if (ConverterRepository != null)
            foreach (var converter in ConverterRepository.GetConvertersWithToType(messageDeserializerResolveRun.PublishType))
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
