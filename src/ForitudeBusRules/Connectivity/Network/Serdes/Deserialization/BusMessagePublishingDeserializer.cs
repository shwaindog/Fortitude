// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines.Execution;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types.Mutable;
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
        Header  = default!;

        Conversation = default!;
        base.StateReset();
    }
}

public class BroadcastReceiverListenContext<T> : ReceiverListenContext<T>
{
    private IMessageBus messageBus;
    private string      publishAddress;
    private IRecycler?  recycler;

    public BroadcastReceiverListenContext(string name, string publishAddress, IMessageBus? messageBus = null) : base(name)
    {
        var checkSameContext = QueueContext.CurrentThreadQueueContext;
        this.messageBus     = checkSameContext?.MessageBus ?? messageBus!;
        this.publishAddress = publishAddress;
    }

    public BroadcastReceiverListenContext(BroadcastReceiverListenContext<T> toClone) : base(toClone)
    {
        messageBus = toClone.messageBus;
        recycler   = toClone.recycler;

        publishAddress = toClone.publishAddress;
    }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public static IReceiverListenContext DynamicBuildTypedBroadcastReceiverListenContext
    (Type messageType, string name
      , IMessageBus messageBus, string publishAddress)
    {
        var typeInfo                         = typeof(BroadcastReceiverListenContext<>).MakeGenericType(messageType);
        var targetQueueReceiverListenContext = (IReceiverListenContext)Activator.CreateInstance(typeInfo, [name, messageBus, publishAddress])!;
        return targetQueueReceiverListenContext;
    }

    public override void SendToReceiver(ConversationMessageNotification<T> conversationMessageNotification)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message      = conversationMessageNotification.Message;
        payload.Header       = conversationMessageNotification.Header;
        payload.Conversation = conversationMessageNotification.Conversation;

        messageBus.PublishAsync(Rule.NoKnownSender, publishAddress, payload, new DispatchOptions());
    }

    public override void SendToReceiver(T message)
    {
        messageBus.PublishAsync(Rule.NoKnownSender, publishAddress, message, new DispatchOptions());
    }

    public override ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IReceiverListenContext)source, copyMergeFlags);

    public override IReceiverListenContext CopyFrom(IReceiverListenContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not BroadcastReceiverListenContext<T> broadcastReceiverListenContext) return this;
        messageBus     = broadcastReceiverListenContext.messageBus;
        publishAddress = broadcastReceiverListenContext.publishAddress;
        recycler       = broadcastReceiverListenContext.recycler;
        return this;
    }

    public override IReceiverListenContext<T> Clone() => new BroadcastReceiverListenContext<T>(this);
}

public class TargetedMessageQueueReceiverListenContext<T> : ReceiverListenContext<T>
{
    private string        publishAddress;
    private IQueueContext queueContext;
    private IRecycler?    recycler;

    public TargetedMessageQueueReceiverListenContext(string name, IQueueContext queueContext, string publishAddress) : base(name)
    {
        this.queueContext   = queueContext;
        this.publishAddress = publishAddress;
    }

    public TargetedMessageQueueReceiverListenContext(TargetedMessageQueueReceiverListenContext<T> toClone) : base(toClone)
    {
        queueContext   = toClone.queueContext;
        publishAddress = toClone.publishAddress;
        recycler       = toClone.recycler;
    }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public static IReceiverListenContext DynamicBuildTypedTargetedMessageQueueReceiverListenContext
    (Type messageType, string name
      , IQueueContext queueContext, string publishAddress)
    {
        var typeInfo                         = typeof(TargetedMessageQueueReceiverListenContext<>).MakeGenericType(messageType);
        var targetQueueReceiverListenContext = (IReceiverListenContext)Activator.CreateInstance(typeInfo, [name, queueContext, publishAddress])!;
        return targetQueueReceiverListenContext;
    }

    public override void SendToReceiver(ConversationMessageNotification<T> conversationMessageNotification)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message      = conversationMessageNotification.Message;
        payload.Header       = conversationMessageNotification.Header;
        payload.Conversation = conversationMessageNotification.Conversation;

        queueContext.RegisteredOn.EnqueuePayloadBody(payload, Rule.NoKnownSender, MessageType.Publish, publishAddress, BusMessage.AppliesToAll);
    }

    public override void SendToReceiver(T message)
    {
        queueContext.RegisteredOn.EnqueuePayloadBody(message, Rule.NoKnownSender, MessageType.Publish, publishAddress, BusMessage.AppliesToAll);
    }

    public override ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IReceiverListenContext)source, copyMergeFlags);

    public override IReceiverListenContext CopyFrom(IReceiverListenContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not TargetedMessageQueueReceiverListenContext<T> targetQueueReceiverListenContext) return this;
        queueContext   = targetQueueReceiverListenContext.queueContext;
        publishAddress = targetQueueReceiverListenContext.publishAddress;
        recycler       = targetQueueReceiverListenContext.recycler;
        return this;
    }

    public override IReceiverListenContext<T> Clone() => new TargetedMessageQueueReceiverListenContext<T>(this);
}

public class TargetedRuleReceiverListenContext<T> : ReceiverListenContext<T>
{
    private string     publishAddress;
    private IRecycler? recycler;
    private IRule      rule;

    public TargetedRuleReceiverListenContext(string name, IRule specificRule, string publishAddress) : base(name)
    {
        rule = specificRule;

        this.publishAddress = publishAddress;
    }

    public TargetedRuleReceiverListenContext(TargetedRuleReceiverListenContext<T> toClone) : base(toClone)
    {
        rule     = toClone.rule;
        recycler = toClone.recycler;

        publishAddress = toClone.publishAddress;
    }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public static IReceiverListenContext DynamicBuildTypedTargetedRuleReceiverListenContext
    (Type messageType, string name
      , IRule specificRule, string publishAddress)
    {
        var typeInfo                         = typeof(TargetedRuleReceiverListenContext<>).MakeGenericType(messageType);
        var targetQueueReceiverListenContext = (IReceiverListenContext)Activator.CreateInstance(typeInfo, [name, specificRule, publishAddress])!;
        return targetQueueReceiverListenContext;
    }

    public override void SendToReceiver(ConversationMessageNotification<T> conversationMessageNotification)
    {
        var payload = Recycler.Borrow<RemoteMessageReceived<T>>();
        payload.Message = conversationMessageNotification.Message;
        payload.Header  = conversationMessageNotification.Header;

        payload.Conversation = conversationMessageNotification.Conversation;

        rule.Context.RegisteredOn.EnqueuePayloadBody(payload, Rule.NoKnownSender, MessageType.Publish, publishAddress
                                                   , checkRule => checkRule == rule);
    }

    public override void SendToReceiver(T message)
    {
        rule.Context.RegisteredOn.EnqueuePayloadBody(message, Rule.NoKnownSender, MessageType.Publish, publishAddress
                                                   , checkRule => checkRule == rule);
    }

    public override ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IReceiverListenContext)source, copyMergeFlags);

    public override IReceiverListenContext CopyFrom(IReceiverListenContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not TargetedRuleReceiverListenContext<T> targetedRuleReceiverListenContext) return this;
        rule           = targetedRuleReceiverListenContext.rule;
        publishAddress = targetedRuleReceiverListenContext.publishAddress;
        recycler       = targetedRuleReceiverListenContext.recycler;
        return this;
    }

    public override IReceiverListenContext<T> Clone() => new TargetedRuleReceiverListenContext<T>(this);
}

public class InvokeRuleCallbackListenContext<T> : ReceiverListenContext<T>
{
    private Func<T, ValueTask>? asyncCallback;

    private IRule      calleeRule;
    private Action<T>? messageCallback;
    private IRecycler? recycler;

    public InvokeRuleCallbackListenContext(string name, IRule calleeRule, Action<T> callback) : base(name)
    {
        recycler        = QueueContext.CurrentThreadQueueContext?.PooledRecycler;
        this.calleeRule = calleeRule;
        messageCallback = SingleParamActionWrapper<T>.WrapAndAttach(callback);
    }

    public InvokeRuleCallbackListenContext(string name, IRule calleeRule, Func<T, ValueTask> callback) : base(name)
    {
        var checkSameContext = QueueContext.CurrentThreadQueueContext;
        recycler        = checkSameContext?.PooledRecycler;
        this.calleeRule = calleeRule;
        asyncCallback   = callback;
    }

    public InvokeRuleCallbackListenContext(InvokeRuleCallbackListenContext<T> toClone) : base(toClone)
    {
        recycler      = toClone.recycler;
        calleeRule    = toClone.calleeRule;
        asyncCallback = toClone.asyncCallback;

        messageCallback = toClone.messageCallback;
    }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public static IReceiverListenContext DynamicBuildTypedTargetedRuleReceiverListenContext
    (Type messageType, string name
      , Action<T> callback)
    {
        var typeInfo = typeof(InvokeRuleCallbackListenContext<>).MakeGenericType(messageType);

        var invokeCallbackActionListenContext = (IReceiverListenContext)Activator.CreateInstance(typeInfo, [name, callback])!;
        return invokeCallbackActionListenContext;
    }

    public static IReceiverListenContext DynamicBuildTypedTargetedRuleReceiverListenContext
    (Type messageType, string name
      , IRule calleeRule, Func<T, ValueTask> callback)
    {
        var typeInfo                          = typeof(InvokeRuleCallbackListenContext<>).MakeGenericType(messageType);
        var invokeCallbackActionListenContext = (IReceiverListenContext)Activator.CreateInstance(typeInfo, [name, calleeRule, callback])!;
        return invokeCallbackActionListenContext;
    }

    public override void SendToReceiver(ConversationMessageNotification<T> conversationMessageNotification)
    {
        var message = conversationMessageNotification.Message;
        messageCallback?.Invoke(message);
        if (asyncCallback != null)
        {
            var oneParamAsyncActionCallback = recycler?.Borrow<OneParamAsyncActionPayload<T>>() ?? new OneParamAsyncActionPayload<T>();
            oneParamAsyncActionCallback.Configure(asyncCallback, message);
            calleeRule.Context.MessageBus.Send(calleeRule, oneParamAsyncActionCallback, MessageType.InvokeablePayload
                                             , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: calleeRule));
        }
    }

    public override void SendToReceiver(T message)
    {
        messageCallback?.Invoke(message);
        if (asyncCallback != null)
        {
            var oneParamAsyncActionCallback = recycler?.Borrow<OneParamAsyncActionPayload<T>>() ?? new OneParamAsyncActionPayload<T>();
            oneParamAsyncActionCallback.Configure(asyncCallback, message);
            calleeRule.Context.MessageBus.Send(calleeRule, oneParamAsyncActionCallback, MessageType.InvokeablePayload
                                             , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: calleeRule));
        }
    }

    public override ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IReceiverListenContext)source, copyMergeFlags);

    public override IReceiverListenContext CopyFrom(IReceiverListenContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not InvokeRuleCallbackListenContext<T> invokeCallbackActionListenContext) return this;
        recycler        = invokeCallbackActionListenContext.recycler;
        messageCallback = invokeCallbackActionListenContext.messageCallback;
        return this;
    }

    public override IReceiverListenContext<T> Clone() => new InvokeRuleCallbackListenContext<T>(this);
}
