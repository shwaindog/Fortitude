// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Types.Mutable;
using static FortitudeBusRules.BusMessaging.Messages.MarshallerType;
using static FortitudeBusRules.BusMessaging.Messages.PayloadRequestType;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages;

public enum PayloadRequestType
{
    Dispatch       // perform marshalling before queued to any queues
  , QueueSend      // perform marshalling on each queue before it is sent
  , QueueReceive   // perform marshalling when being read anyway where on the queue during processing
  , CalleeRetrieve // perform marshalling when read by any rule.
}

// If type supports Freezable the Frozen version is sent
public enum MarshallerType
{
    DefaultForType        // will look at the sender payload instance and pick the most appropriate marshaller for the type
  , PassThrough           // this should not create a Marshaller so the original sender instance is passed around
  , Freezable             // if the type supports IFreezable then send the Frozen item. If frozen supports RefCount then it is also ref counted
  , RefCountIncrementer   // increment recyclable.RefCount at the marshall point
  , NonDecrementingCloner // will not decrement even if is recyclable
  , CloneRecyclable       // clone and decrement at end of each queue or dispatch
  , BorrowReusable        // each will uses context recycler to create and copy details on each queue
  , NoQueueDecrement      // disable queue processing incrementing and decrementing
  , SerdeCaller           // will marshall via a Serde passed marshaller.
}

public struct PayloadMarshalOptions
{
    public PayloadMarshalOptions
    (PayloadRequestType marshalOn = QueueSend, MarshallerType marshallerType = DefaultForType,
        ISerdesMarshaller? serdesMarshaller = null)
    {
        MarshalOn      = marshalOn;
        MarshallerType = marshallerType;
    }

    public PayloadRequestType MarshalOn { get; } = QueueSend;

    public MarshallerType MarshallerType { get; } = DefaultForType;

    public ISerdesMarshaller? SerdesMarshaller { get; }
}

public static class PayloadMarshallerExtensions
{
    public static IPayloadMarshaller<T>? ResolvePayloadMarshaller<T>
    (this PayloadMarshalOptions payloadMarshalOptions, T senderInstance
      , IRecycler recycler) =>
        TypePayloadMarshallerExtensions<T>.ResolvePayloadMarshaller(payloadMarshalOptions, senderInstance, recycler);

    // ReSharper disable once ClassNeverInstantiated.Local
    private class TypePayloadMarshallerExtensions<T>
    {
        private static readonly Type GenericIReusableOfT = typeof(IReusableObject<>).MakeGenericType(typeof(T));

        public static IPayloadMarshaller<T>? ResolvePayloadMarshaller
        (PayloadMarshalOptions payloadMarshalOptions, T senderInstance
          , IRecycler recycler)
        {
            var typeofT = typeof(T);
            if (typeofT.IsValueType || typeofT.IsInterface || payloadMarshalOptions.MarshallerType == PassThrough ||
                (typeofT.GetInterfaces().All(t => t != GenericIReusableOfT) &&
                 senderInstance is not (IRecyclableObject or ICloneable)))
                return null;

            var customMarshaller = recycler.Borrow<PayloadMarshaller<T>>();
            return customMarshaller.Configure(payloadMarshalOptions, senderInstance, recycler);
            ;
        }
    }
}

public interface IPayloadMarshaller : IRecyclableObject
{
    MarshallerType MarshallerType { get; }

    IPayloadMarshaller Configure(PayloadMarshalOptions payloadMarshalOpts, object senderInstance, IRecycler? senderRecycler = null);
    IPayloadMarshaller QueueMarshaller(IMessageQueue messageQueue);

    object GetMarshalled(object input, PayloadRequestType payloadRequestType);

    void PayloadRefCountDecrement(object? payload);
    void PayloadRefCountIncrement(object? payload);
}

public interface IPayloadMarshaller<T> : IReusableObject<IPayloadMarshaller<T>>, IPayloadMarshaller
{
    IPayloadMarshaller<T> Configure(PayloadMarshalOptions payloadMarshalOptions, T senderInstance, IRecycler? senderRecycler = null);

    T GetMarshalled(T input, PayloadRequestType payloadRequestType);

    new IPayloadMarshaller<T> QueueMarshaller(IMessageQueue messageQueue);

    void PayloadRefCountDecrement(T? payload);
    void PayloadRefCountIncrement(T? payload);
}

public class PayloadMarshaller<T> : ReusableObject<IPayloadMarshaller<T>>, IPayloadMarshaller<T>
{
    private static readonly Type GenericIReusableOfT = typeof(IReusableObject<>).MakeGenericType(typeof(T));

    private T? dispatchMarshalledInstance;

    private PayloadMarshaller<T>? parentMarshaller;
    private PayloadMarshalOptions payloadMarshalOptions;

    private T?   queueMarshalledInstance;
    private bool queuePayloadHasBeenReplaced;

    public PayloadMarshaller()
    {
        payloadMarshalOptions      = new PayloadMarshalOptions();
        dispatchMarshalledInstance = default;
    }

    public PayloadMarshaller(PayloadMarshalOptions payloadMarshalOptions, T senderInstance, IRecycler? senderRecycler = null)
    {
        Configure(payloadMarshalOptions, senderInstance, senderRecycler);
    }

    public PayloadMarshaller(PayloadMarshaller<T> toClone)
    {
        payloadMarshalOptions      = toClone.payloadMarshalOptions;
        SenderRecycler             = toClone.SenderRecycler;
        ReceiverRecycler           = toClone.ReceiverRecycler;
        dispatchMarshalledInstance = toClone.dispatchMarshalledInstance;
        queueMarshalledInstance    = toClone.queueMarshalledInstance;
    }

    public IRecycler  SenderRecycler   { get; set; } = null!;

    public IRecycler? ReceiverRecycler { get; set; }

    public bool CloneMarshallerOnEachQueue =>
        payloadMarshalOptions.MarshalOn is QueueReceive or CalleeRetrieve &&
        MarshallerType is BorrowReusable or NonDecrementingCloner or SerdeCaller;

    public MarshallerType MarshallerType => payloadMarshalOptions.MarshallerType;


    public IPayloadMarshaller Configure(PayloadMarshalOptions payloadMarshalOpts, object senderInstance, IRecycler? senderRecycler = null) =>
        Configure(payloadMarshalOpts, (T)senderInstance, senderRecycler)!;

    IPayloadMarshaller IPayloadMarshaller.QueueMarshaller(IMessageQueue messageQueue) => QueueMarshaller(messageQueue);


    public object GetMarshalled(object input, PayloadRequestType payloadRequestType) => GetMarshalled((T)input, payloadRequestType)!;

    public void PayloadRefCountIncrement(object? payload)
    {
        if (payload != null) PayloadRefCountIncrement((T)payload);
    }

    public void PayloadRefCountDecrement(object? payload)
    {
        if (payload != null) PayloadRefCountDecrement((T)payload);
    }

    public IPayloadMarshaller<T> Configure(PayloadMarshalOptions payloadMarshalOpts, T senderInstance, IRecycler? senderRecycler = null)
    {
        payloadMarshalOptions = payloadMarshalOpts;
        ValidateMarshalOptions(payloadMarshalOpts);
        SenderRecycler = senderRecycler ?? Recycler ?? SingletonRecycler.Instance;
        if (MarshallerType == DefaultForType)
        {
            if (senderInstance is IFreezable)
                payloadMarshalOptions = new PayloadMarshalOptions(payloadMarshalOpts.MarshalOn, Freezable);
            else if (typeof(T).GetInterfaces().Any(t => t == GenericIReusableOfT))
                payloadMarshalOptions = new PayloadMarshalOptions(payloadMarshalOpts.MarshalOn, BorrowReusable);
            else if (senderInstance is IRecyclableObject and ICloneable)
                payloadMarshalOptions = new PayloadMarshalOptions(payloadMarshalOpts.MarshalOn, CloneRecyclable);
            else if (senderInstance is IRecyclableObject)
                payloadMarshalOptions = new PayloadMarshalOptions(payloadMarshalOpts.MarshalOn, RefCountIncrementer);
            else if (senderInstance is ICloneable)
                payloadMarshalOptions = new PayloadMarshalOptions(payloadMarshalOpts.MarshalOn, NonDecrementingCloner);
            else
                payloadMarshalOptions = new PayloadMarshalOptions(payloadMarshalOpts.MarshalOn, PassThrough);
        }

        return this;
    }

    public override IPayloadMarshaller<T> CopyFrom(IPayloadMarshaller<T> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is PayloadMarshaller<T> copyMarshaller)
        {
            payloadMarshalOptions      = copyMarshaller.payloadMarshalOptions;
            SenderRecycler             = copyMarshaller.SenderRecycler;
            ReceiverRecycler           = copyMarshaller.ReceiverRecycler;
            dispatchMarshalledInstance = copyMarshaller.dispatchMarshalledInstance;
        }

        return this;
    }

    public override IPayloadMarshaller<T> Clone() => Recycler?.Borrow<PayloadMarshaller<T>>().CopyFrom(this) ?? new PayloadMarshaller<T>();

    public IPayloadMarshaller<T> QueueMarshaller(IMessageQueue messageQueue)
    {
        IncrementRefCount();
        if (CloneMarshallerOnEachQueue)
        {
            var payLoadMarshaller = (PayloadMarshaller<T>)messageQueue.Context.PooledRecycler.Borrow<PayloadMarshaller<T>>().CopyFrom(this);
            payLoadMarshaller.ReceiverRecycler = messageQueue.Context.PooledRecycler;
            payLoadMarshaller.parentMarshaller = this;
            return payLoadMarshaller;
        }

        return this;
    }

    public T GetMarshalled(T input, PayloadRequestType payloadRequestType)
    {
        if (MarshallerType == PassThrough || input == null) return input;
        return payloadRequestType switch
               {
                   Dispatch     => DispatchSelector(input)
                 , QueueSend    => QueueSendSelector(input)
                 , QueueReceive => QueueReceiveSelector(input)
                 , _            => CalleeRetrieveSelector(input)
               };
    }

    public void PayloadRefCountIncrement(T? payload)
    {
        if ((payloadMarshalOptions.MarshalOn == Dispatch && MarshallerType is CloneRecyclable or BorrowReusable or NonDecrementingCloner)
         || MarshallerType is NoQueueDecrement
         || payload is not IRecyclableObject recyclableObject)
            return;

        recyclableObject.IncrementRefCount();
    }

    public void PayloadRefCountDecrement(T? payload)
    {
        if ((payloadMarshalOptions.MarshalOn == Dispatch && MarshallerType is CloneRecyclable or BorrowReusable or NonDecrementingCloner)
         || MarshallerType is NoQueueDecrement
         || payload is not IRecyclableObject recyclableObject)
            return;

        recyclableObject.DecrementRefCount();
    }

    public override int DecrementRefCount()
    {
        if (RefCount == 1) DispatchComplete();
        parentMarshaller?.DecrementRefCount();
        return base.DecrementRefCount();
    }

    public void DispatchComplete()
    {
        if (parentMarshaller == null)
            if (payloadMarshalOptions.MarshalOn == Dispatch && dispatchMarshalledInstance is IRecyclableObject dispatchPayloadInstanceRecyclable
                                                            && MarshallerType is CloneRecyclable or BorrowReusable)
                dispatchPayloadInstanceRecyclable.DecrementRefCount();

        if (queuePayloadHasBeenReplaced && queueMarshalledInstance is IRecyclableObject queuePayloadInstanceRecyclable &&
            payloadMarshalOptions is
                { MarshalOn: CalleeRetrieve or QueueReceive, MarshallerType: CloneRecyclable or BorrowReusable or NonDecrementingCloner })
            queuePayloadInstanceRecyclable.DecrementRefCount();
    }

    public void ValidateMarshalOptions(PayloadMarshalOptions payloadMarshalOpts)
    {
        if (payloadMarshalOpts.MarshallerType == SerdeCaller && payloadMarshalOpts.SerdesMarshaller == null)
            throw new ArgumentException("You can not set MarshallerType = SerdeCaller without setting a SerdesMarshaller");
        if (payloadMarshalOpts.MarshallerType == BorrowReusable && typeof(T).GetInterfaces().All(t => t != GenericIReusableOfT))
            throw new ArgumentException("You can not set MarshallerType = BorrowReusable when Payload is not of type IResusable<T>");
        if (payloadMarshalOpts.MarshallerType is NonDecrementingCloner or CloneRecyclable &&
            typeof(T).GetInterfaces().All(t => t != typeof(ICloneable)))
            throw new ArgumentException("You can not set MarshallerType = Cloner or CloneRecycler when Payload type is not of type ICloneable");
    }

    private T DispatchSelector(T input)
    {
        if (payloadMarshalOptions.MarshalOn != Dispatch) return dispatchMarshalledInstance = input;
        return dispatchMarshalledInstance = RunMarshaller(dispatchMarshalledInstance ?? input);
    }

    private T QueueSendSelector(T input)
    {
        if (payloadMarshalOptions.MarshalOn != QueueSend) return queueMarshalledInstance = dispatchMarshalledInstance ?? input;
        return queueMarshalledInstance = RunMarshaller(input);
    }

    private T QueueReceiveSelector(T input)
    {
        if (payloadMarshalOptions.MarshalOn != QueueReceive) return input;
        var calleeResult = RunMarshaller(input);
        queuePayloadHasBeenReplaced |= !Equals(input, calleeResult);
        return calleeResult;
    }

    private T CalleeRetrieveSelector(T input)
    {
        var calleeResult = RunMarshaller(input);
        queuePayloadHasBeenReplaced |= !Equals(input, calleeResult);
        return calleeResult;
    }

    private T RunMarshaller(T input)
    {
        var result = input;
        switch (MarshallerType)
        {
            case RefCountIncrementer:
                if (input is IRecyclableObject recyclableObject) recyclableObject.IncrementRefCount();
                break;
            case Freezable:
                if (input is IFreezable freezableT)
                {
                    result = (T)freezableT.Freeze;
                    if (result is IRecyclableObject recyclableFrozen) recyclableFrozen.IncrementRefCount();
                }
                break;
            case NonDecrementingCloner:
                if (input is ICloneable cloneable) result = (T)cloneable.Clone();
                break;
            case CloneRecyclable:
                if (input is ICloneable cloneableRecycler)
                {
                    var clone = (T)cloneableRecycler.Clone();
                    result = clone;
                }

                break;
            case BorrowReusable:
                if (input is IReusableObject)
                {
                    var borrowed = (ITransferState)(ReceiverRecycler ?? SenderRecycler).Borrow(typeof(T));
                    borrowed.CopyFrom((ITransferState)input, CopyMergeFlags.FullReplace);
                    result = (T)borrowed;
                }

                break;
            case SerdeCaller:
                if (payloadMarshalOptions.SerdesMarshaller is ISerdesMarshaller<T> typedSerdesMarshaller)
                    result = typedSerdesMarshaller.Marshall(input);
                break;
            case PassThrough:      break;
            case NoQueueDecrement: break;
        }

        return result;
    }
}
