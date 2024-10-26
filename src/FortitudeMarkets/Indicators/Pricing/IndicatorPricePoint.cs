// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Indicators.Pricing;

public interface IIndicatorPricePoint : IPricePoint, IReusableObject<IIndicatorPricePoint>, IDoublyLinkedListNode<IIndicatorPricePoint>
{
    long IndicatorSourceTickerId { get; }

    new IIndicatorPricePoint? Previous { get; set; }
    new IIndicatorPricePoint? Next     { get; set; }

    new IIndicatorPricePoint Clone();
}

public readonly struct IndicatorPricePointValue // not inheriting from IIndicatorPricePoint to prevent accidental boxing unboxing
{
    private readonly PricePointValue backingPricePointValue;

    public IndicatorPricePointValue(long indicatorSourceTickerId, decimal singleValue, DateTime atTime)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        backingPricePointValue = new PricePointValue(singleValue, atTime);
    }

    public IndicatorPricePointValue(long indicatorSourceTickerId, IPricePoint pricePoint)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        backingPricePointValue = new PricePointValue(pricePoint);
    }

    public IndicatorPricePointValue(long indicatorSourceTickerId, PricePointValue pricePoint)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        backingPricePointValue = pricePoint;
    }

    public IndicatorPricePointValue(IIndicatorPricePoint toClone)
    {
        IndicatorSourceTickerId = toClone.IndicatorSourceTickerId;

        backingPricePointValue = new PricePointValue(toClone.SingleValue, toClone.AtTime);
    }

    public long IndicatorSourceTickerId { get; }

    public DateTime AtTime      => backingPricePointValue.AtTime;
    public decimal  SingleValue => backingPricePointValue.SingleValue;


    public static implicit operator PricePointValue(IndicatorPricePointValue indicatorPricePoint) => indicatorPricePoint.backingPricePointValue;
}

public static class IndicatorPricePointValueExtensions
{
    public static IndicatorPricePoint ToIndicatorPricePoint(this IndicatorPricePointValue indicatorPricePointValue, IRecycler? recycler = null) =>
        recycler?.Borrow<IndicatorPricePoint>().CopyFrom(indicatorPricePointValue) ?? new IndicatorPricePoint(indicatorPricePointValue);

    public static PricePoint ToPricePoint(this IndicatorPricePointValue pricePointValue, IRecycler? recycler = null) =>
        recycler?.Borrow<PricePoint>().CopyFrom(pricePointValue) ?? new PricePoint(pricePointValue);
}

public class IndicatorPricePoint : PricePoint, IIndicatorPricePoint
{
    public IndicatorPricePoint() { }

    public IndicatorPricePoint
        (long indicatorSourceTickerId, decimal singleValue, DateTime atTime) : base(singleValue, atTime) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorPricePoint
        (long indicatorSourceTickerId, IPricePoint pricePoint) : base(pricePoint) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorPricePoint
        (long indicatorSourceTickerId, PricePointValue pricePoint) : base(pricePoint) =>
        IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorPricePoint(IndicatorPricePointValue toClone) : base(toClone) => IndicatorSourceTickerId = toClone.IndicatorSourceTickerId;

    public IndicatorPricePoint(IIndicatorPricePoint toClone) : base(toClone) => IndicatorSourceTickerId = toClone.IndicatorSourceTickerId;

    public new IIndicatorPricePoint? Previous
    {
        get => base.Previous as IIndicatorPricePoint;
        set => base.Previous = value;
    }

    public new IIndicatorPricePoint? Next
    {
        get => base.Next as IIndicatorPricePoint;
        set => base.Next = value;
    }

    public long IndicatorSourceTickerId { get; set; }

    public IReusableObject<IIndicatorPricePoint> CopyFrom
        (IReusableObject<IIndicatorPricePoint> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IIndicatorPricePoint)source, copyMergeFlags);

    public IIndicatorPricePoint CopyFrom(IIndicatorPricePoint source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        IndicatorSourceTickerId = source.IndicatorSourceTickerId;

        return this;
    }

    public override bool AreEquivalent(IPricePoint? other, bool exactTypes = false)
    {
        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var idIsSame                                                    = !exactTypes;
        if (other is IIndicatorPricePoint indicatorPricePoint) idIsSame = IndicatorSourceTickerId == indicatorPricePoint.IndicatorSourceTickerId;

        var allAreSame = baseIsSame && idIsSame;

        return allAreSame;
    }

    public override IIndicatorPricePoint Clone() =>
        Recycler?.Borrow<IndicatorPricePoint>().CopyFrom((IIndicatorPricePoint)this)
     ?? new IndicatorPricePoint((IIndicatorPricePoint)this);

    public void Configure(long indicatorSourceTickerId, decimal singleValue, DateTime atTime)
    {
        AtTime      = atTime;
        SingleValue = singleValue;
    }

    public void Configure(IndicatorPricePointValue indicatorPricePoint)
    {
        AtTime      = indicatorPricePoint.AtTime;
        SingleValue = indicatorPricePoint.SingleValue;
    }

    public IndicatorPricePoint CopyFrom(IndicatorPricePointValue source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        IndicatorSourceTickerId = source.IndicatorSourceTickerId;

        return this;
    }

    public override IIndicatorPricePoint CopyFrom(IPricePoint source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IIndicatorPricePoint indicatorPricePoint) IndicatorSourceTickerId = indicatorPricePoint.IndicatorSourceTickerId;

        return this;
    }

    public override string ToString() =>
        $"{nameof(IndicatorPricePoint)}({nameof(AtTime)}: {AtTime}, {nameof(SingleValue)}: {SingleValue}, " +
        $"{nameof(IndicatorSourceTickerId)}: {IndicatorSourceTickerId})";


    public static implicit operator IndicatorPricePointValue(IndicatorPricePoint indicatorPricePoint) => new(indicatorPricePoint);
}
