#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Products;

public abstract class OrxProductOrder : IProductOrder
{
    protected OrxProductOrder() { }

    protected OrxProductOrder(IProductOrder toClone)
    {
        IsComplete = toClone.IsComplete;
        Message = toClone.Message != null ? new MutableString(toClone.Message) : null;
    }

    [OrxOptionalField(2)] public MutableString? Message { get; set; }

    public abstract ProductType ProductType { get; }

    [OrxMandatoryField(1)] public bool IsComplete { get; set; }

    IMutableString? IProductOrder.Message
    {
        get => Message;
        set => Message = value as MutableString;
    }

    public abstract bool IsError { get; }
    public IOrder? Order { get; set; }
    public abstract void ApplyAmendment(IOrderAmend amendment);
    public abstract bool RequiresAmendment(IOrderAmend amendment);

    public virtual void CopyFrom(IProductOrder source, IRecycler recycler)
    {
        if (ProductType != source.ProductType)
            throw new ArgumentException("Attempting to copy different product types across");

        Message = source.Message != null ? recycler.Borrow<MutableString>().Clear().Append(source.Message) : null;
        IsComplete = source.IsComplete;
    }

    public abstract void RegisterExecution(IExecution execution);

    public abstract IProductOrder Clone();

    public abstract OrxProductOrder GetPooledInstance(IRecycler recycler);
}
