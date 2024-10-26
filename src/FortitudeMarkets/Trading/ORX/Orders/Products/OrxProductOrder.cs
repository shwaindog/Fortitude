#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Products;

public abstract class OrxProductOrder : ReusableObject<IProductOrder>, IProductOrder
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


    public abstract void RegisterExecution(IExecution execution);

    public abstract override IProductOrder Clone();

    public override IProductOrder CopyFrom(IProductOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (ProductType != source.ProductType)
            throw new ArgumentException("Attempting to copy different product types across");

        Message = source.Message.SyncOrRecycle(Message);
        IsComplete = source.IsComplete;
        return this;
    }
}
