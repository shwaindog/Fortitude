#region

using FortitudeBusRules.MessageBus.Messages;
using FortitudeBusRules.MessageBus.Tasks;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.Messaging;

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

public interface IMessage : IStoreState<IMessage>
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
    IMessage<TAsPayLoad> BorrowCopy<TAsPayLoad>(IEventContext messageContext);
    IRespondingMessage<TAsPayLoad, TAsResponse> BorrowCopy<TAsPayLoad, TAsResponse>(IEventContext messageContext);
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
    , TaskAction
}

public interface IMessage<TPayLoad> : IMessage, IRecyclableObject
{
    new PayLoad<TPayLoad> PayLoad { get; }
}

public interface IRespondingMessage<TPayLoad, TResponse> : IMessage<TPayLoad>
{
    new IResponseValueTaskSource<TResponse> Response { get; }
}

public class Message : IMessage
{
    public static readonly NoMessageResponseSource NoOpCompletionSource = new();
    public static readonly RuleFilter AppliesToAll = _ => true;

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

    public IRespondingMessage<TPayLoad, TResponse> BorrowCopy<TPayLoad, TResponse>(IEventContext messageContext)
    {
        var castClone = messageContext.PooledRecycler.Borrow<Message<TPayLoad, TResponse>>();
        castClone.CopyFrom(this);
        castClone.IncrementCargoRefCounts();
        return castClone;
    }

    public IMessage<TPayLoad> BorrowCopy<TPayLoad>(IEventContext messageContext) =>
        BorrowCopy<TPayLoad, object>(messageContext);

    public IMessage CopyFrom(IMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
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
        CopyFrom((IMessage)source, copyMergeFlags);
        return this;
    }

    public override string ToString() =>
        $"{GetType().Name}{{{nameof(Type)}: {Type}, " +
        $"{nameof(Sender)}: {Sender}, " +
        $"{nameof(DestinationAddress)}: {DestinationAddress}, " +
        $"{nameof(SentTime)}: {SentTime}, " +
        $"{nameof(PayLoad)}: {PayLoad}, " +
        $"{nameof(Response)}: {Response}}}";
}

public class Message<TPayLoad, TResponse> : Message, IRespondingMessage<TPayLoad, TResponse>
{
    private int isInRecycler;
    private int refCount;

    public new PayLoad<TPayLoad> PayLoad
    {
        get => (PayLoad<TPayLoad>)((Message)this).PayLoad!;
        set => ((Message)this).PayLoad = value;
    }

    public new IResponseValueTaskSource<TResponse> Response
    {
        get => (IResponseValueTaskSource<TResponse>)base.Response;
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
