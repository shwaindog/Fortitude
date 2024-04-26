#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;

public class RemoteMessageReceived<T> : RecyclableObject
{
    public T Message { get; set; } = default!;
    public MessageHeader? Header { get; set; }
    public IConversation? Conversation { get; set; }

    public override void StateReset()
    {
        Message = default!;
        Header = default!;
        Conversation = default!;
        base.StateReset();
    }
}

public class BroadcastReceiverListenContext<T> : ReceiverListenContext<T>
{
    private IMessageBus messageBus;
    private string publishAddress;
    private IRecycler? recycler;

    public BroadcastReceiverListenContext(string name, IMessageBus messageBus, string publishAddress) : base(name)
    {
        this.messageBus = messageBus;
        this.publishAddress = publishAddress;
    }

    public BroadcastReceiverListenContext(BroadcastReceiverListenContext<T> toClone) : base(toClone)
    {
        messageBus = toClone.messageBus;
        publishAddress = toClone.publishAddress;
        recycler = toClone.recycler;
    }
    public static IReceiverListenContext DynamicBuildTypedBroadcastReceiverListenContext(Type messageType, string name
        , IMessageBus messageBus, string publishAddress)
    {
        
        var typeInfo = typeof(BroadcastReceiverListenContext<>).MakeGenericType(messageType);
        var targetQueueReceiverListenContext = (IReceiverListenContext)Activator.CreateInstance(typeInfo, [name, messageBus, publishAddress])!;
        return targetQueueReceiverListenContext;
    }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public override void SendToReceiver(ConversationMessageNotification<T> conversationMessageNotification)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message = conversationMessageNotification.Message;
        payload.Header = conversationMessageNotification.Header;
        payload.Conversation = conversationMessageNotification.Conversation;

        messageBus.PublishAsync(Rule.NoKnownSender, publishAddress, payload, new DispatchOptions());
    }

    public override void SendToReceiver(T message)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message = message;

        messageBus.PublishAsync(Rule.NoKnownSender, publishAddress, payload, new DispatchOptions());
    }

    public override IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IReceiverListenContext)source, copyMergeFlags);

    public override IReceiverListenContext CopyFrom(IReceiverListenContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not BroadcastReceiverListenContext<T> broadcastReceiverListenContext) return this;
        messageBus = broadcastReceiverListenContext.messageBus;
        publishAddress = broadcastReceiverListenContext.publishAddress;
        recycler = broadcastReceiverListenContext.recycler;
        return this;
    }

    public override IReceiverListenContext<T> Clone() => new BroadcastReceiverListenContext<T>(this);
}

public class TargetedMessageQueueReceiverListenContext<T> : ReceiverListenContext<T>
{
    private IQueueContext queueContext;
    private string publishAddress;
    private IRecycler? recycler;

    public TargetedMessageQueueReceiverListenContext(string name, IQueueContext queueContext, string publishAddress) : base(name)
    {
        this.queueContext = queueContext;
        this.publishAddress = publishAddress;
    }

    public TargetedMessageQueueReceiverListenContext(TargetedMessageQueueReceiverListenContext<T> toClone) : base(toClone)
    {
        queueContext = toClone.queueContext;
        publishAddress = toClone.publishAddress;
        recycler = toClone.recycler;
    }

    public static IReceiverListenContext DynamicBuildTypedTargetedMessageQueueReceiverListenContext(Type messageType, string name
        , IQueueContext queueContext, string publishAddress)
    {
        
        var typeInfo = typeof(TargetedMessageQueueReceiverListenContext<>).MakeGenericType(messageType);
        var targetQueueReceiverListenContext = (IReceiverListenContext)Activator.CreateInstance(typeInfo, [name, queueContext, publishAddress])!;
        return targetQueueReceiverListenContext;
    }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public override void SendToReceiver(ConversationMessageNotification<T> conversationMessageNotification)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message = conversationMessageNotification.Message;
        payload.Header = conversationMessageNotification.Header;
        payload.Conversation = conversationMessageNotification.Conversation;

        queueContext.RegisteredOn.EnqueuePayloadBody(payload, Rule.NoKnownSender, publishAddress, MessageType.Publish, BusMessage.AppliesToAll);
    }

    public override void SendToReceiver(T message)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message = message;

        queueContext.RegisteredOn.EnqueuePayloadBody(payload, Rule.NoKnownSender, publishAddress, MessageType.Publish, BusMessage.AppliesToAll);
    }

    public override IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IReceiverListenContext)source, copyMergeFlags);

    public override IReceiverListenContext CopyFrom(IReceiverListenContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not TargetedMessageQueueReceiverListenContext<T> targetQueueReceiverListenContext) return this;
        queueContext = targetQueueReceiverListenContext.queueContext;
        publishAddress = targetQueueReceiverListenContext.publishAddress;
        recycler = targetQueueReceiverListenContext.recycler;
        return this;
    }

    public override IReceiverListenContext<T> Clone() => new TargetedMessageQueueReceiverListenContext<T>(this);
}

public class TargetedRuleReceiverListenContext<T> : ReceiverListenContext<T>
{
    private string publishAddress;
    private IRecycler? recycler;
    private IRule rule;

    public TargetedRuleReceiverListenContext(string name, IRule specificRule, string publishAddress) : base(name)
    {
        rule = specificRule;
        this.publishAddress = publishAddress;
    }

    public TargetedRuleReceiverListenContext(TargetedRuleReceiverListenContext<T> toClone) : base(toClone)
    {
        rule = toClone.rule;
        publishAddress = toClone.publishAddress;
        recycler = toClone.recycler;
    }

    public static IReceiverListenContext DynamicBuildTypedTargetedRuleReceiverListenContext(Type messageType, string name
        , IRule specificRule, string publishAddress)
    {
        
        var typeInfo = typeof(TargetedRuleReceiverListenContext<>).MakeGenericType(messageType);
        var targetQueueReceiverListenContext = (IReceiverListenContext)Activator.CreateInstance(typeInfo, [name, specificRule, publishAddress])!;
        return targetQueueReceiverListenContext;
    }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public override void SendToReceiver(ConversationMessageNotification<T> conversationMessageNotification)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message = conversationMessageNotification.Message;
        payload.Header = conversationMessageNotification.Header;
        payload.Conversation = conversationMessageNotification.Conversation;

        rule.Context.RegisteredOn.EnqueuePayloadBody(payload, Rule.NoKnownSender, publishAddress, MessageType.Publish, checkRule => checkRule == rule);
    }

    public override void SendToReceiver(T message)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message = message;

        rule.Context.RegisteredOn.EnqueuePayloadBody(payload, Rule.NoKnownSender, publishAddress, MessageType.Publish, checkRule => checkRule == rule);
    }

    public override IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IReceiverListenContext)source, copyMergeFlags);

    public override IReceiverListenContext CopyFrom(IReceiverListenContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not TargetedRuleReceiverListenContext<T> targetedRuleReceiverListenContext) return this;
        rule = targetedRuleReceiverListenContext.rule;
        publishAddress = targetedRuleReceiverListenContext.publishAddress;
        recycler = targetedRuleReceiverListenContext.recycler;
        return this;
    }

    public override IReceiverListenContext<T> Clone() => new TargetedRuleReceiverListenContext<T>(this);
}
