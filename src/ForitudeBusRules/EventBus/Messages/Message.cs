#region

using Fortitude.EventProcessing.BusRules.EventBus.Tasks;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.EventBus.Messages;

public interface IPayLoad
{
    Type BodyType { get; }

    object? BodyObj { get; }
}

public interface IPayLoad<T> : IPayLoad
{
    T? Body { get; }
}

internal struct PayLoad<T> : IPayLoad
{
    private int refCount;
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
}

public enum MessageType
{
    Publish
    , RunActionPayload
    , RequestResponse
    , LoadRule
    , MessageListener
    , MessageUnsubscribe
}

public interface IMessage<TPayLoad, TResponse> : IMessage, IRecyclableObject<IMessage>
{
    new IPayLoad<TPayLoad> PayLoad { get; }
    new ReusableValueTaskSource<TResponse> Response { get; }
}

public class Message : IMessage
{
    public static ReusableValueTaskSource<object> NoOpCompletionSource = new();

    public MessageType Type { get; set; }
    public IRule? Sender { get; set; }
    public string? DestinationAddress { get; set; }
    public DateTime? SentTime { get; set; }
    public IPayLoad? PayLoad { get; set; }
    public IMessageResponseSource Response { get; set; } = NoOpCompletionSource;

    public IProcessorRegistry? ProcessorRegistry { get; set; }

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

    public IMessage<TPayLoad, TResponse> BorrowCopy<TPayLoad, TResponse>(IEventContext messageContext)
    {
        var castClone = messageContext.PooledRecycler.Borrow<Message<TPayLoad, TResponse>>();
        castClone.CopyFrom(this);
        return castClone;
    }

    public override string ToString() =>
        $"{GetType()}{{{nameof(Type)}: {Type}," +
        $"{nameof(Sender)}: {Sender}, " +
        $"{nameof(DestinationAddress)}: {DestinationAddress}, " +
        $"{nameof(SentTime)}: {SentTime}, " +
        $"{nameof(PayLoad)}: {PayLoad}, " +
        $"{nameof(Response)}: {Response}}}";
}

public class Message<TPayLoad, TResponse> : Message, IMessage<TPayLoad, TResponse>
{
    public new IPayLoad<TPayLoad> PayLoad
    {
        get => (IPayLoad<TPayLoad>)((Message)this).PayLoad!;
        set => ((Message)this).PayLoad = value;
    }

    public new ReusableValueTaskSource<TResponse> Response
    {
        get => (ReusableValueTaskSource<TResponse>)((Message)this).Response;
        set => ((Message)this).Response = value;
    }

    public int RefCount => 0;
    public bool RecycleOnRefCountZero { get; set; } = false;
    public bool AutoRecycledByProducer { get; set; } = true;
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => 0;

    public int IncrementRefCount() => 0;

    public bool Recycle()
    {
        if (!AutoRecycledByProducer && !IsInRecycler)
            Recycler!.Recycle(this);
        return IsInRecycler;
    }
}
