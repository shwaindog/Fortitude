#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Products;

public abstract class ProductOrder : IProductOrder
{
    private int refCount = 0;

    protected ProductOrder(IOrderId orderId, TimeInForce timeInForce, DateTime creationTime)
    {
        Message = new MutableString();
        Order = new Order(orderId, timeInForce, creationTime, this);
        IsComplete = false;
    }

    protected ProductOrder(IProductOrder toClone)
    {
        Message = toClone.Message;
        IsComplete = toClone.IsComplete;
        Order = toClone.Order;
    }

    protected ProductOrder(string message, IOrder order, bool isComplete)
        : this((MutableString)message, order, isComplete) { }

    protected ProductOrder(IMutableString message, IOrder order, bool isComplete)
    {
        Message = message;
        Order = order;
        IsComplete = isComplete;
    }

    public abstract ProductType ProductType { get; }
    public IMutableString? Message { get; set; }
    public IOrder? Order { get; set; }
    public bool IsComplete { get; set; }
    public abstract bool IsError { get; }
    public abstract void ApplyAmendment(IOrderAmend amendment);
    public abstract bool RequiresAmendment(IOrderAmend amendment);

    public virtual void CopyFrom(IProductOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (ProductType != source.ProductType)
            throw new ArgumentException("Attempting to copy different product types across");

        Message = source.Message;
        IsComplete = source.IsComplete;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IProductOrder)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => Interlocked.Decrement(ref refCount);

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount == 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }


    public abstract void RegisterExecution(IExecution execution);
    public abstract IProductOrder Clone();
}
