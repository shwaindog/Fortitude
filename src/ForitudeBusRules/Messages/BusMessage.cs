// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeBusRules.Messages;

public interface IPayload : IRecyclableObject
{
    Type BodyType { get; }

    object? BodyObj(PayloadRequestType requestType);
}

public interface IPayload<out T> : IPayload
{
    T Body(PayloadRequestType requestType = PayloadRequestType.CalleeRetrieve);
}

public class Payload<T> : ReusableObject<Payload<T>>, IPayload<T>
{
    private T body;

    public Payload() => body = default!;

    public Payload(T body) => this.body = body;

    public Payload(Payload<T> toClone)
    {
        body = toClone.body;

        PayloadMarshaller = toClone.PayloadMarshaller;
    }

    public IPayloadMarshaller<T>? PayloadMarshaller { get; set; }

    public T SetBody
    {
        set => body = value;
    }

    public Type BodyType { get; } = typeof(T);

    public T Body(PayloadRequestType requestType = PayloadRequestType.CalleeRetrieve)
    {
        if (PayloadMarshaller == null) return body;
        return body = PayloadMarshaller.GetMarshalled(body, requestType);
    }

    public object? BodyObj(PayloadRequestType requestType) => Body(requestType);

    public override int DecrementRefCount()
    {
        if (PayloadMarshaller != null)
        {
            PayloadMarshaller.PayloadRefCountDecrement(body);
            PayloadMarshaller.DecrementRefCount();
        }
        else if (body is IRecyclableObject recyclableBody)
        {
            recyclableBody.DecrementRefCount();
        }

        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (PayloadMarshaller != null)
        {
            PayloadMarshaller.IncrementRefCount();
            PayloadMarshaller.PayloadRefCountIncrement(body);
        }
        else if (body is IRecyclableObject recyclableBody)
        {
            recyclableBody.IncrementRefCount();
        }

        return base.IncrementRefCount();
    }

    public override void StateReset()
    {
        body = default!;

        PayloadMarshaller = null;
    }

    public override Payload<T> CopyFrom(Payload<T> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        body = source.body;

        PayloadMarshaller = source.PayloadMarshaller;
        return this;
    }

    public override Payload<T> Clone() => Recycler?.Borrow<Payload<T>>().CopyFrom(this) ?? new Payload<T>(this);

    public override string ToString() => $"{nameof(Payload<T>)}[{typeof(T).Name}]({nameof(Body)}: {Body})";
}

public delegate bool RuleFilter(IRule appliesToRule);

public interface IBusMessage : IRecyclableObject, ITransferState<IBusMessage>, ICanCarrySocketSenderPayload, ICanCarrySocketReceiverPayload
{
    MessageType Type       { get; }
    IRule?      Sender     { get; }
    DateTime?   SentTime   { get; }
    IPayload    Payload    { get; }
    RuleFilter  RuleFilter { get; }

    string? DestinationAddress { get; }

    IAsyncResponseSource? Response          { get; }
    IProcessorRegistry?   ProcessorRegistry { get; }

    void IncrementCargoRefCounts();
    void DecrementCargoRefCounts();

    IBusMessage BorrowCopy(IQueueContext messageContext);

    IBusMessage<TAsPayload> BorrowCopy<TAsPayload>(IQueueContext messageContext);

    IBusRespondingMessage<TAsPayload, TAsResponse> BorrowCopy<TAsPayload, TAsResponse>(IQueueContext messageContext);
}

public readonly struct BusMessageValue
{
    public BusMessageValue(IBusMessage toClone)
    {
        Type     = toClone.Type;
        Sender   = toClone.Sender;
        SentTime = toClone.SentTime;
        Payload  = toClone.Payload;
        Response = toClone.Response;

        RuleFilter = toClone.RuleFilter;

        DestinationAddress = toClone.DestinationAddress;
    }

    public MessageType Type       { get; }
    public IRule?      Sender     { get; }
    public DateTime?   SentTime   { get; }
    public IPayload    Payload    { get; }
    public RuleFilter  RuleFilter { get; }

    public string? DestinationAddress { get; }

    public IAsyncResponseSource? Response          { get; }
    public IProcessorRegistry?   ProcessorRegistry => Response as IProcessorRegistry;
}

public static class BusMessageValueExtensions
{
    public static BusMessageValue ToBusMessageValue(this IBusMessage toClone) => new(toClone);

    public static void IncrementCargoRefCounts(this BusMessageValue busMessageValue)
    {
        busMessageValue.Payload.IncrementRefCount();
        busMessageValue.Response?.IncrementRefCount();
        // Never increment or decrement Response it must survive the message and is decremented on get result or a optional timer
    }

    public static void DecrementCargoRefCounts(this BusMessageValue busMessageValue)
    {
        busMessageValue.Payload.DecrementRefCount();
        busMessageValue.Response?.DecrementRefCount();
        // Never increment or decrement Response it must survive the message and is decremented on get result or a optional timer
    }

    public static IBusRespondingMessage<TPayload, TResponse> BorrowCopy<TPayload, TResponse>
        (this BusMessageValue busMessageValue, IQueueContext messageContext)
    {
        var castClone = messageContext.PooledRecycler.Borrow<BusMessage<TPayload, TResponse>>();
        castClone.CopyFrom(busMessageValue);
        castClone.IncrementCargoRefCounts();
        return castClone;
    }

    public static IBusMessage<TPayload> BorrowCopy<TPayload>(this BusMessageValue busMessageValue, IQueueContext messageContext) =>
        busMessageValue.BorrowCopy<TPayload, IDispatchResult>(messageContext);

    public static IBusMessage BorrowCopy(this BusMessageValue busMessageValue, IQueueContext messageContext)
    {
        var clone = messageContext.PooledRecycler.Borrow<BusMessage>();
        clone.CopyFrom(busMessageValue);
        clone.IncrementCargoRefCounts();
        return clone;
    }
}

public enum MessageType
{
    Unknown
  , Publish
  , RunActionPayload
  , RunAsyncActionPayload
  , TimerPayload
  , RequestResponse
  , LoadRule
  , UnloadRule
  , ListenerSubscribe
  , ListenerUnsubscribe
  , InvokeablePayload
  , ValueTaskCallback
  , SendToRemote
  , AddWatchSocket
  , RemoveWatchSocket
  , AddListenSubscribeInterceptor
  , RemoveListenSubscribeInterceptor
}

public interface IBusMessage<out TPayload> : IBusMessage
{
    new IPayload<TPayload> Payload { get; }
}

public interface IBusRespondingMessage<out TPayload, in TResponse> : IBusMessage<TPayload>
{
    new IReusableAsyncResponse<TResponse> Response { get; }
}

public class BusMessage : RecyclableObject, IBusMessage
{
    public static readonly Payload<object> ResetStatePayload = new()
    {
        AutoRecycleAtRefCountZero = false
    };

    public static readonly RuleFilter AppliesToAll = _ => true;

    static BusMessage() => ResetStatePayload.AutoRecycleAtRefCountZero = false;

    public MessageType Type   { get; set; }
    public IRule?      Sender { get; set; }

    public string? DestinationAddress { get; set; }

    public DateTime? SentTime { get; set; }
    public IPayload  Payload  { get; set; } = ResetStatePayload;

    public IAsyncResponseSource? Response { get; set; }

    public IProcessorRegistry? ProcessorRegistry
    {
        get => Response as IProcessorRegistry;
        set => Response = value;
    }

    public RuleFilter RuleFilter { get; set; } = AppliesToAll;

    public void IncrementCargoRefCounts()
    {
        Payload.IncrementRefCount();
        Response?.IncrementRefCount();
        // Never increment or decrement Response it must survive the message and is decremented on get result or a optional timer
    }

    public void DecrementCargoRefCounts()
    {
        Payload.DecrementRefCount();
        Response?.DecrementRefCount();
        // Never increment or decrement Response it must survive the message and is decremented on get result or a optional timer
    }

    public void TaskPostProcessingCleanup()
    {
        DecrementCargoRefCounts();
    }

    public IBusRespondingMessage<TPayload, TResponse> BorrowCopy<TPayload, TResponse>(IQueueContext messageContext)
    {
        var castClone = messageContext.PooledRecycler.Borrow<BusMessage<TPayload, TResponse>>();
        castClone.CopyFrom(this);
        castClone.IncrementCargoRefCounts();
        return castClone;
    }

    public IBusMessage<TPayload> BorrowCopy<TPayload>(IQueueContext messageContext) => BorrowCopy<TPayload, IDispatchResult>(messageContext);

    public IBusMessage BorrowCopy(IQueueContext messageContext)
    {
        var clone = messageContext.PooledRecycler.Borrow<BusMessage>();
        clone.CopyFrom(this);
        clone.IncrementCargoRefCounts();
        return clone;
    }

    public IBusMessage CopyFrom(IBusMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Type     = source.Type;
        Sender   = source.Sender;
        SentTime = source.SentTime;
        Payload  = source.Payload;
        Response = source.Response;

        RuleFilter = source.RuleFilter;

        DestinationAddress = source.DestinationAddress;
        return this;
    }

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IBusMessage)source, copyMergeFlags);
        return this;
    }

    public bool IsTaskCallbackItem => Type == MessageType.ValueTaskCallback;

    public void SetAsTaskCallbackItem(SendOrPostCallback callback, object? state)
    {
        var payLoad     = Recycler?.Borrow<Payload<TaskPayload>>() ?? new Payload<TaskPayload>();
        var taskPayload = Recycler?.Borrow<TaskPayload>() ?? new TaskPayload();
        taskPayload.Callback = callback;
        taskPayload.State    = state;
        payLoad.SetBody      = taskPayload;

        Type       = MessageType.ValueTaskCallback;
        Payload    = payLoad;
        Sender     = Rule.NoKnownSender;
        SentTime   = DateTime.UtcNow;
        Response   = null;
        RuleFilter = AppliesToAll;

        DestinationAddress = "NotUsed";
    }

    public void InvokeTaskCallback()
    {
        if (Payload.BodyObj(PayloadRequestType.QueueReceive) is TaskPayload taskPayload) taskPayload.Invoke();
    }

    public bool IsSocketSenderItem => Type == MessageType.SendToRemote;

    public ISocketSender? SocketSender => Payload.BodyObj(PayloadRequestType.QueueReceive) as ISocketSender;

    public void SetAsSocketSenderItem(ISocketSender socketSender)
    {
        var payLoad = Recycler?.Borrow<Payload<ISocketSender>>() ?? new Payload<ISocketSender>();
        payLoad.SetBody = socketSender;

        Type       = MessageType.SendToRemote;
        Payload    = payLoad;
        Sender     = Rule.NoKnownSender;
        SentTime   = DateTime.UtcNow;
        Response   = null;
        RuleFilter = AppliesToAll;

        DestinationAddress = "NotUsed";
        ProcessorRegistry  = null;
    }

    public bool IsSocketReceiverItem => Type == MessageType.AddWatchSocket || Type == MessageType.RemoveWatchSocket;
    public bool IsSocketAdd          => Type == MessageType.AddWatchSocket;

    public ISocketReceiver? SocketReceiver => Payload.BodyObj(PayloadRequestType.QueueReceive) as ISocketReceiver;

    public override void StateReset()
    {
        DecrementCargoRefCounts();
        RuleFilter = AppliesToAll;
        Type       = MessageType.Unknown;
        Sender     = null;
        SentTime   = DateTimeConstants.UnixEpoch;

        Payload  = ResetStatePayload;
        Response = null;

        DestinationAddress = null;
        base.StateReset();
    }

    public void SetAsSocketReceiverItem(ISocketReceiver socketReceiver, bool isAdd)
    {
        var payLoad = Recycler?.Borrow<Payload<ISocketReceiver>>() ?? new Payload<ISocketReceiver>();
        payLoad.SetBody = socketReceiver;

        Type       = isAdd ? MessageType.AddWatchSocket : MessageType.RemoveWatchSocket;
        Payload    = payLoad;
        Sender     = Rule.NoKnownSender;
        SentTime   = DateTime.UtcNow;
        Response   = null;
        RuleFilter = AppliesToAll;

        DestinationAddress = "NotUsed";
        ProcessorRegistry  = null;
    }

    public IBusMessage CopyFrom(BusMessageValue source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Type     = source.Type;
        Sender   = source.Sender;
        SentTime = source.SentTime;
        Payload  = source.Payload;
        Response = source.Response;

        RuleFilter = source.RuleFilter;

        DestinationAddress = source.DestinationAddress;
        return this;
    }

    public override string ToString() =>
        $"{nameof(BusMessage)}{{{nameof(Type)}: {Type}, " +
        $"{nameof(Sender)}: {Sender}, " +
        $"{nameof(DestinationAddress)}: {DestinationAddress}, " +
        $"{nameof(SentTime)}: {SentTime}, " +
        $"{nameof(Payload)}: {Payload}, " +
        $"{nameof(Response)}: {Response}}}";
}

public class BusMessage<TPayload, TResponse> : BusMessage, IBusRespondingMessage<TPayload, TResponse>
{
    public static readonly Payload<TPayload> ResetStateTypedPayload = new();

    public static readonly NoMessageResponseSource<TResponse> NoTypedOpCompletionSource = new();

    static BusMessage() => ResetStateTypedPayload.AutoRecycleAtRefCountZero = false;

    public new IPayload<TPayload> Payload
    {
        get => (Payload<TPayload>)((BusMessage)this).Payload;
        set => ((BusMessage)this).Payload = value;
    }

    public new IReusableAsyncResponse<TResponse> Response
    {
        get
        {
            var underlyingResponse = base.Response!;
            if (underlyingResponse is NoMessageResponseSource) return NoTypedOpCompletionSource;
            return (IReusableAsyncResponse<TResponse>)underlyingResponse;
        }
        set => base.Response = value;
    }

    public override void StateReset()
    {
        base.StateReset();
        base.Payload = ResetStateTypedPayload;
    }

    public override string ToString() =>
        $"{nameof(BusMessage<TPayload, TResponse>)}[{typeof(TPayload).Name}, {typeof(TResponse).Name}]({nameof(Type)}: {Type}, " +
        $"{nameof(Sender)}: {Sender}, " +
        $"{nameof(DestinationAddress)}: {DestinationAddress}, " +
        $"{nameof(SentTime)}: {SentTime}, " +
        $"{nameof(Payload)}: {Payload}, " +
        $"{nameof(Response)}: {Response}, " +
        $"{nameof(refCount)}: {refCount}, " +
        $"{nameof(IsInRecycler)}: {IsInRecycler})";
}
