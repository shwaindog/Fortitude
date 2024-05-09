#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
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

    public override string ToString() => $"{GetType().Name}[{typeof(T).Name}]({nameof(Body)}: {Body})";
}

public delegate bool RuleFilter(IRule appliesToRule);

public interface IBusMessage : IStoreState<IBusMessage>, ICanCarrySocketSenderPayload, ICanCarrySocketReceiverPayload
{
    MessageType Type { get; }
    IRule? Sender { get; }
    string? DestinationAddress { get; }
    DateTime? SentTime { get; }
    IPayload Payload { get; }
    IAsyncResponseSource Response { get; }
    IProcessorRegistry? ProcessorRegistry { get; }
    RuleFilter RuleFilter { get; }
    void IncrementCargoRefCounts();
    void DecrementCargoRefCounts();
    IBusMessage<TAsPayload> BorrowCopy<TAsPayload>(IQueueContext messageContext);
    IBusRespondingMessage<TAsPayload, TAsResponse> BorrowCopy<TAsPayload, TAsResponse>(IQueueContext messageContext);
}

public enum MessageType
{
    Unknown
    , Publish
    , RunActionPayload
    , TimerPayload
    , RequestResponse
    , LoadRule
    , UnloadRule
    , ListenerSubscribe
    , ListenerUnsubscribe
    , QueueParamsExecutionPayload
    , ValueTaskCallback
    , SendToRemote
    , AddWatchSocket
    , RemoveWatchSocket
    , AddListenSubscribeInterceptor
    , RemoveListenSubscribeInterceptor
}

public interface IBusMessage<out TPayload> : IBusMessage, IRecyclableObject
{
    new IPayload<TPayload> Payload { get; }
}

public interface IBusRespondingMessage<out TPayload, in TResponse> : IBusMessage<TPayload>
{
    new IResponseValueTaskSource<TResponse> Response { get; }
}

public class BusMessage : IBusMessage
{
    public static readonly Payload<object> ResetStatePayload = new();
    public static readonly NoMessageResponseSource NoOpCompletionSource = new();
    public static readonly RuleFilter AppliesToAll = _ => true;
    private static readonly Recycler Recycler = new();

    static BusMessage() => ResetStatePayload.AutoRecycleAtRefCountZero = false;

    public MessageType Type { get; set; }
    public IRule? Sender { get; set; }
    public string? DestinationAddress { get; set; }
    public DateTime? SentTime { get; set; }
    public IPayload Payload { get; set; } = ResetStatePayload;
    public IAsyncResponseSource Response { get; set; } = NoOpCompletionSource;
    public IProcessorRegistry? ProcessorRegistry { get; set; }

    public RuleFilter RuleFilter { get; set; } = AppliesToAll;

    public void IncrementCargoRefCounts()
    {
        Payload.IncrementRefCount();
        ProcessorRegistry?.IncrementRefCount();
        // Never increment or decrement Response it must survive the message
    }

    public void DecrementCargoRefCounts()
    {
        Payload.DecrementRefCount();
        ProcessorRegistry?.DecrementRefCount();
        // Never increment or decrement Response it must survive the message
    }

    public IBusRespondingMessage<TPayload, TResponse> BorrowCopy<TPayload, TResponse>(IQueueContext messageContext)
    {
        var castClone = messageContext.PooledRecycler.Borrow<BusMessage<TPayload, TResponse>>();
        castClone.CopyFrom(this);
        castClone.IncrementCargoRefCounts();
        return castClone;
    }

    public IBusMessage<TPayload> BorrowCopy<TPayload>(IQueueContext messageContext) => BorrowCopy<TPayload, object>(messageContext);

    public IBusMessage CopyFrom(IBusMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Type = source.Type;
        Sender = source.Sender;
        DestinationAddress = source.DestinationAddress;
        SentTime = source.SentTime;
        Payload = source.Payload;
        Response = source.Response;
        ProcessorRegistry = source.ProcessorRegistry;
        RuleFilter = source.RuleFilter;
        return this;
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IBusMessage)source, copyMergeFlags);
        return this;
    }

    public bool IsTaskCallbackItem => Type == MessageType.ValueTaskCallback;

    public void SetAsTaskCallbackItem(SendOrPostCallback callback, object? state)
    {
        var payLoad = Recycler.Borrow<Payload<TaskPayload>>();
        var taskPayload = Recycler.Borrow<TaskPayload>();
        taskPayload.Callback = callback;
        taskPayload.State = state;
        payLoad.SetBody = taskPayload;
        Type = MessageType.ValueTaskCallback;
        Payload = payLoad;
        Sender = Rule.NoKnownSender;
        DestinationAddress = "NotUsed";
        SentTime = DateTime.UtcNow;
        Response = NoOpCompletionSource;
        ProcessorRegistry = null;
        RuleFilter = AppliesToAll;
    }

    public void InvokeTaskCallback()
    {
        if (Payload.BodyObj(PayloadRequestType.QueueReceive) is TaskPayload taskPayload) taskPayload.Invoke();
    }

    public bool IsSocketSenderItem => Type == MessageType.SendToRemote;
    public ISocketSender? SocketSender => Payload.BodyObj(PayloadRequestType.QueueReceive) as ISocketSender;

    public void SetAsSocketSenderItem(ISocketSender socketSender)
    {
        var payLoad = Recycler.Borrow<Payload<ISocketSender>>();
        payLoad.SetBody = socketSender;
        Type = MessageType.SendToRemote;
        Payload = payLoad;
        Sender = Rule.NoKnownSender;
        DestinationAddress = "NotUsed";
        SentTime = DateTime.UtcNow;
        Response = NoOpCompletionSource;
        ProcessorRegistry = null;
        RuleFilter = AppliesToAll;
    }

    public bool IsSocketReceiverItem => Type == MessageType.AddWatchSocket || Type == MessageType.RemoveWatchSocket;
    public bool IsSocketAdd => Type == MessageType.AddWatchSocket;
    public ISocketReceiver? SocketReceiver => Payload.BodyObj(PayloadRequestType.QueueReceive) as ISocketReceiver;

    public void SetAsSocketReceiverItem(ISocketReceiver socketReceiver, bool isAdd)
    {
        var payLoad = Recycler.Borrow<Payload<ISocketReceiver>>();
        payLoad.SetBody = socketReceiver;
        Type = isAdd ? MessageType.AddWatchSocket : MessageType.RemoveWatchSocket;
        Payload = payLoad;
        Sender = Rule.NoKnownSender;
        DestinationAddress = "NotUsed";
        SentTime = DateTime.UtcNow;
        Response = NoOpCompletionSource;
        ProcessorRegistry = null;
        RuleFilter = AppliesToAll;
    }

    public override string ToString() =>
        $"{GetType().Name}{{{nameof(Type)}: {Type}, " +
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

    private int isInRecycler;
    private int refCount;

    static BusMessage() => ResetStateTypedPayload.AutoRecycleAtRefCountZero = false;

    public new IPayload<TPayload> Payload
    {
        get => (Payload<TPayload>)((BusMessage)this).Payload;
        set => ((BusMessage)this).Payload = value;
    }

    public new IResponseValueTaskSource<TResponse> Response
    {
        get
        {
            var underlyingResponse = base.Response;
            if (underlyingResponse is NoMessageResponseSource) return NoTypedOpCompletionSource;
            return (IResponseValueTaskSource<TResponse>)underlyingResponse;
        }
        set => base.Response = value;
    }

    public int RefCount => refCount;
    public bool AutoRecycleAtRefCountZero { get; set; }

    public bool IsInRecycler
    {
        get => isInRecycler != 0;
        set => isInRecycler = value ? 1 : 0;
    }

    public IRecycler? Recycler { get; set; }

    public virtual int DecrementRefCount()
    {
        DecrementCargoRefCounts();
        if (!IsInRecycler && Interlocked.Decrement(ref refCount) <= 0 && AutoRecycleAtRefCountZero) Recycle();
        return refCount;
    }

    public virtual int IncrementRefCount()
    {
        if (!IsInRecycler) IncrementCargoRefCounts();
        return Interlocked.Increment(ref refCount);
    }

    public bool Recycle()
    {
        if (!IsInRecycler)
            if (Interlocked.CompareExchange(ref isInRecycler, 1, 0) == 0)
                Recycler?.Recycle(this);

        return IsInRecycler;
    }

    public void StateReset()
    {
        RuleFilter = AppliesToAll;
        Type = MessageType.Unknown;
        Sender = null;
        DestinationAddress = null;
        SentTime = DateTimeConstants.UnixEpoch;
        base.Payload = ResetStateTypedPayload;
        base.Response = NoOpCompletionSource;
        refCount = 0;
    }

    public override string ToString() =>
        $"{GetType().Name}[{typeof(TPayload).Name}, {typeof(TResponse).Name}]({nameof(Type)}: {Type}, " +
        $"{nameof(Sender)}: {Sender}, " +
        $"{nameof(DestinationAddress)}: {DestinationAddress}, " +
        $"{nameof(SentTime)}: {SentTime}, " +
        $"{nameof(Payload)}: {Payload}, " +
        $"{nameof(Response)}: {Response}, " +
        $"{nameof(refCount)}: {refCount}, " +
        $"{nameof(IsInRecycler)}: {IsInRecycler})";
}
