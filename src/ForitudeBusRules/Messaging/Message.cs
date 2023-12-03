#region

using Fortitude.EventProcessing.BusRules.MessageBus.Messages;
using Fortitude.EventProcessing.BusRules.MessageBus.Tasks;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.Messaging;

public interface IPayLoad
{
    Type BodyType { get; }

    object? BodyObj { get; }
}

public interface IPayLoad<T> : IPayLoad
{
    T? Body { get; }
}

public struct PayLoad<T> : IPayLoad<T>
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

    public override string ToString() => $"{GetType().Name}[{typeof(T).Name}]({nameof(Body)}: {Body})";
}

public interface IMessage : IStoreState<IMessage>
{
    MessageType Type { get; }
    IRule? Sender { get; }
    string? DestinationAddress { get; }
    DateTime? SentTime { get; }
    IPayLoad? PayLoad { get; }
    IMessageResponseSource Response { get; }
    IProcessorRegistry? ProcessorRegistry { get; }
    void IncrementCargoRefCounts();
    void DecrementCargoRefCounts();
    int IncrementRefCount();
    int DecrementRefCount();
    IMessage<TAsPayLoad> BorrowCopy<TAsPayLoad>(IEventContext messageContext);
    IRespondingMessage<TAsPayLoad, TAsResponse> BorrowCopy<TAsPayLoad, TAsResponse>(IEventContext messageContext);
}

public enum MessageType
{
    Publish
    , RunActionPayload
    , TimerPayload
    , RequestResponse
    , LoadRule
    , UnloadRule
    , ListenerSubscribe
    , ListenerUnsubscribe
    , TaskAction
}

public interface IMessage<TPayLoad> : IMessage, IRecyclableObject<IMessage>
{
    new PayLoad<TPayLoad> PayLoad { get; }
    new int IncrementRefCount();
    new int DecrementRefCount();
}

public interface IRespondingMessage<TPayLoad, TResponse> : IMessage<TPayLoad>
{
    new IResponseValueTaskSource<TResponse> Response { get; }
}

public class Message : IMessage
{
    public static readonly NoMessageResponseSource NoOpCompletionSource = new();

    public MessageType Type { get; set; }
    public IRule? Sender { get; set; }
    public string? DestinationAddress { get; set; }
    public DateTime? SentTime { get; set; }
    public IPayLoad? PayLoad { get; set; }
    public IMessageResponseSource Response { get; set; } = NoOpCompletionSource;

    public IProcessorRegistry? ProcessorRegistry { get; set; }

    public virtual int IncrementRefCount() => 0;

    public virtual int DecrementRefCount() => 0;

    public void CopyFrom(IMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Type = source.Type;
        Sender = source.Sender;
        DestinationAddress = source.DestinationAddress;
        SentTime = source.SentTime;
        PayLoad = source.PayLoad;
        Response = source.Response;
        ProcessorRegistry = source.ProcessorRegistry;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IMessage)source, copyMergeFlags);
    }

    public void IncrementCargoRefCounts()
    {
        if (PayLoad?.BodyObj is IRecyclableObject recyclePayLoad) recyclePayLoad.IncrementRefCount();
        if (ProcessorRegistry is IRecyclableObject recycleRegistry) recycleRegistry.IncrementRefCount();
        if (Response is IRecyclableObject recycleResponse) recycleResponse.IncrementRefCount();
    }

    public void DecrementCargoRefCounts()
    {
        if (PayLoad?.BodyObj is IRecyclableObject recyclePayLoad) recyclePayLoad.DecrementRefCount();
        if (ProcessorRegistry is IRecyclableObject recycleRegistry) recycleRegistry.DecrementRefCount();
        if (Response is IRecyclableObject recycleResponse) recycleResponse.DecrementRefCount();
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
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }

    public override int DecrementRefCount()
    {
        DecrementCargoRefCounts();
        if (Interlocked.Decrement(ref refCount) <= 0 && RecycleOnRefCountZero) Recycle();
        return refCount;
    }

    public override int IncrementRefCount()
    {
        if (!IsInRecycler) IncrementCargoRefCounts();
        return Interlocked.Increment(ref refCount);
    }

    public bool Recycle()
    {
        if (Recycler != null && !IsInRecycler && (refCount == 0 || !RecycleOnRefCountZero))
            Recycler!.Recycle(this);
        return IsInRecycler;
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
