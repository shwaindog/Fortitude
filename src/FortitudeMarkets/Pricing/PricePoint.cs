// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing;

public interface IPricePoint : IReusableObject<IPricePoint>, IDoublyLinkedListNode<IPricePoint>, IInterfacesComparable<IPricePoint>
{
    DateTime AtTime      { get; }
    decimal  SingleValue { get; }
}

public struct PricePointValue // not inheriting from IPricePoint to prevent accidental boxing unboxing
{
    public PricePointValue(decimal singleValue, DateTime atTime)
    {
        AtTime      = atTime;
        SingleValue = singleValue;
    }

    public PricePointValue(IPricePoint toClone)
    {
        AtTime      = toClone.AtTime;
        SingleValue = toClone.SingleValue;
    }

    public DateTime AtTime      { get; }
    public decimal  SingleValue { get; }
}

public static class PricePointValueExtensions
{
    public static PricePoint ToPricePoint(this PricePointValue pricePointValue, IRecycler? recycler = null) =>
        recycler?.Borrow<PricePoint>().CopyFrom(pricePointValue) ?? new PricePoint(pricePointValue);
}

public class PricePoint : ReusableObject<IPricePoint>, IPricePoint
{
    public PricePoint() { }

    public PricePoint(decimal singleValue, DateTime atTime)
    {
        AtTime      = atTime;
        SingleValue = singleValue;
    }

    public PricePoint(IPricePoint toClone)
    {
        AtTime      = toClone.AtTime;
        SingleValue = toClone.SingleValue;
    }

    public PricePoint(PricePointValue toClone)
    {
        AtTime      = toClone.AtTime;
        SingleValue = toClone.SingleValue;
    }

    public IPricePoint? Previous { get; set; }
    public IPricePoint? Next     { get; set; }

    public DateTime AtTime      { get; set; }
    public decimal  SingleValue { get; set; }

    public override IPricePoint CopyFrom(IPricePoint source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SingleValue = source.SingleValue;
        AtTime      = source.AtTime;
        return this;
    }

    public override IPricePoint Clone() => Recycler?.Borrow<PricePoint>().CopyFrom(this) ?? new PricePoint((IPricePoint)this);

    public virtual bool AreEquivalent(IPricePoint? other, bool exactTypes = false)
    {
        var singleValueSame = SingleValue == other?.SingleValue;
        var atTimeSame      = AtTime == other?.AtTime;

        var allAreSame = singleValueSame && atTimeSame;
        return allAreSame;
    }

    public void Configure(decimal singleValue, DateTime atTime)
    {
        AtTime      = atTime;
        SingleValue = singleValue;
    }

    public void Configure(PricePointValue pricePoint)
    {
        AtTime      = pricePoint.AtTime;
        SingleValue = pricePoint.SingleValue;
    }

    public PricePoint CopyFrom(PricePointValue source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SingleValue = source.SingleValue;
        AtTime      = source.AtTime;
        return this;
    }

    protected bool Equals(PricePoint other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PricePoint)obj);
    }

    public override int GetHashCode() => HashCode.Combine(AtTime, SingleValue);

    public override string ToString() => $"{nameof(PricePoint)}({nameof(AtTime)}: {AtTime}, {nameof(SingleValue)}: {SingleValue})";


    public static implicit operator PricePointValue(PricePoint pricePoint) => new(pricePoint);
}
