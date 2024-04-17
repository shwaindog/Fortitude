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

public interface IPayLoad : IRecyclableObject
{
    Type BodyType { get; }

    object? BodyObj { get; }
}

public interface IPayLoad<T> : IPayLoad
{
    T? Body { get; }
}

public class PayLoad<T> : RecyclableObject, IPayLoad<T>
{
    public PayLoad() { }
    public PayLoad(T? body) => Body = body;

    public PayLoad(PayLoad<T> toClone)
    {
        BodyType = toClone.BodyType;
        Body = toClone.Body;
    }

    public Type BodyType { get; } = typeof(T);
    public T? Body { get; set; }
    public object? BodyObj => Body;

    public override int DecrementRefCount()
    {
        if (Body is IRecyclableObject recyclableBody) recyclableBody.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public override int IncrementRefCount()
    {
        if (Body is IRecyclableObject recyclableBody) recyclableBody.IncrementRefCount();
        return base.IncrementRefCount();
    }

    public override void StateReset()
    {
        Body = default;
    }

    public override string ToString() => $"{GetType().Name}[{typeof(T).Name}]({nameof(Body)}: {Body})";
}

public delegate bool RuleFilter(IRule appliesToRule);

public interface IBusMessage : IStoreState<IBusMessage>, ICanCarrySocketSenderPayload, ICanCarrySocketReceiverPayload
{
    MessageType Type { get; }
    IRule? Sender { get; }
    string? DestinationAddress { get; }
    DateTime? SentTime { get; }
    IPayLoad? PayLoad { get; }
    IAsyncResponseSource Response { get; }
    IProcessorRegistry? ProcessorRegistry { get; }
    RuleFilter RuleFilter { get; }
    void IncrementCargoRefCounts();
    void DecrementCargoRefCounts();
    IBusMessage<TAsPayLoad> BorrowCopy<TAsPayLoad>(IQueueContext messageContext);
    IBusRespondingMessage<TAsPayLoad, TAsResponse> BorrowCopy<TAsPayLoad, TAsResponse>(IQueueContext messageContext);
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
    , SendToRemote
    , AddWatchSocket
    , RemoveWatchSocket
    , AddListenSubscribeInterceptor
    , RemoveListenSubscribeInterceptor
}

public interface IBusMessage<TPayLoad> : IBusMessage, IRecyclableObject
{
    new PayLoad<TPayLoad> PayLoad { get; }
}

public interface IBusRespondingMessage<TPayLoad, TResponse> : IBusMessage<TPayLoad>
{
    new IResponseValueTaskSource<TResponse> Response { get; }
}

public class BusMessage : IBusMessage
{
    public static readonly NoMessageResponseSource NoOpCompletionSource = new();
    public static readonly RuleFilter AppliesToAll = _ => true;
    private static readonly Recycler Recycler = new();

    public MessageType Type { get; set; }
    public IRule? Sender { get; set; }
    public string? DestinationAddress { get; set; }
    public DateTime? SentTime { get; set; }
    public IPayLoad? PayLoad { get; set; }
    public IAsyncResponseSource Response { get; set; } = NoOpCompletionSource;
    public IProcessorRegistry? ProcessorRegistry { get; set; }

    public RuleFilter RuleFilter { get; set; } = AppliesToAll;

    public void IncrementCargoRefCounts()
    {
        PayLoad?.IncrementRefCount();
        ProcessorRegistry?.IncrementRefCount();
        Response?.IncrementRefCount();
    }

    public void DecrementCargoRefCounts()
    {
        PayLoad?.DecrementRefCount();
        ProcessorRegistry?.DecrementRefCount();
        Response?.DecrementRefCount();
    }

    public IBusRespondingMessage<TPayLoad, TResponse> BorrowCopy<TPayLoad, TResponse>(IQueueContext messageContext)
    {
        var castClone = messageContext.PooledRecycler.Borrow<BusMessage<TPayLoad, TResponse>>();
        castClone.CopyFrom(this);
        castClone.IncrementCargoRefCounts();
        return castClone;
    }

    public IBusMessage<TPayLoad> BorrowCopy<TPayLoad>(IQueueContext messageContext) => BorrowCopy<TPayLoad, object>(messageContext);

    public IBusMessage CopyFrom(IBusMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Type = source.Type;
        Sender = source.Sender;
        DestinationAddress = source.DestinationAddress;
        SentTime = source.SentTime;
        PayLoad = source.PayLoad;
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

    public bool IsTaskCallbackItem => Type == MessageType.QueueParamsExecutionPayload;

    public void SetAsTaskCallbackItem(SendOrPostCallback callback, object? state)
    {
        var payLoad = Recycler.Borrow<PayLoad<TaskPayload>>();
        var taskPayLoad = Recycler.Borrow<TaskPayload>();
        taskPayLoad.Callback = callback;
        taskPayLoad.State = state;
        payLoad.Body = taskPayLoad;
        Type = MessageType.QueueParamsExecutionPayload;
        PayLoad = payLoad;
        Sender = Rule.NoKnownSender;
        DestinationAddress = "NotUsed";
        SentTime = DateTime.UtcNow;
        Response = NoOpCompletionSource;
        ProcessorRegistry = null;
        RuleFilter = AppliesToAll;
    }

    public void InvokeTaskCallback()
    {
        if (PayLoad!.BodyObj is TaskPayload taskPayload) taskPayload.Invoke();
    }

    public bool IsSocketSenderItem => Type == MessageType.SendToRemote;
    public ISocketSender? SocketSender => PayLoad!.BodyObj as ISocketSender;

    public void SetAsSocketSenderItem(ISocketSender socketSender)
    {
        var payLoad = Recycler.Borrow<PayLoad<ISocketSender>>();
        payLoad.Body = socketSender;
        Type = MessageType.SendToRemote;
        PayLoad = payLoad;
        Sender = Rule.NoKnownSender;
        DestinationAddress = "NotUsed";
        SentTime = DateTime.UtcNow;
        Response = NoOpCompletionSource;
        ProcessorRegistry = null;
        RuleFilter = AppliesToAll;
    }

    public bool IsSocketReceiverItem => Type == MessageType.AddWatchSocket || Type == MessageType.RemoveWatchSocket;
    public bool IsSocketAdd => Type == MessageType.AddWatchSocket;
    public ISocketReceiver? SocketReceiver => PayLoad!.BodyObj as ISocketReceiver;

    public void SetAsSocketReceiverItem(ISocketReceiver socketReceiver, bool isAdd)
    {
        var payLoad = Recycler.Borrow<PayLoad<ISocketReceiver>>();
        payLoad.Body = socketReceiver;
        Type = isAdd ? MessageType.AddWatchSocket : MessageType.RemoveWatchSocket;
        PayLoad = payLoad;
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
        $"{nameof(PayLoad)}: {PayLoad}, " +
        $"{nameof(Response)}: {Response}}}";
}

public class BusMessage<TPayLoad, TResponse> : BusMessage, IBusRespondingMessage<TPayLoad, TResponse>
{
    public static readonly NoMessageResponseSource<TResponse> NoTypedOpCompletionSource = new();

    private int isInRecycler;
    private int refCount;

    public new PayLoad<TPayLoad> PayLoad
    {
        get => (PayLoad<TPayLoad>)((BusMessage)this).PayLoad!;
        set => ((BusMessage)this).PayLoad = value;
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
        base.PayLoad = null;
        base.Response = NoOpCompletionSource;
        refCount = 0;
    }

    public override string ToString() =>
        $"{GetType().Name}[{typeof(TPayLoad).Name}, {typeof(TResponse).Name}]({nameof(Type)}: {Type}, " +
        $"{nameof(Sender)}: {Sender}, " +
        $"{nameof(DestinationAddress)}: {DestinationAddress}, " +
        $"{nameof(SentTime)}: {SentTime}, " +
        $"{nameof(PayLoad)}: {PayLoad}, " +
        $"{nameof(Response)}: {Response}, " +
        $"{nameof(refCount)}: {refCount}, " +
        $"{nameof(IsInRecycler)}: {IsInRecycler})";
}
