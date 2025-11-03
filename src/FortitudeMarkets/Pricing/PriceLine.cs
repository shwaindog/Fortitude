// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing;

public interface IPriceLine
{
    PricePointValue FirstPoint  { get; }
    PricePointValue SecondPoint { get; }
}

public struct PriceLineValue // not inheriting from IPriceLine to prevent accidental boxing unboxing
{
    public PriceLineValue(decimal firstSingleValue, DateTime firstAtTime, decimal secondSingleValue, DateTime secondAtTime)
    {
        FirstPoint  = new PricePointValue(firstSingleValue, firstAtTime);
        SecondPoint = new PricePointValue(secondSingleValue, secondAtTime);
    }

    public PriceLineValue(IPricePoint firstPoint, IPricePoint secondPoint)
    {
        FirstPoint  = new PricePointValue(firstPoint);
        SecondPoint = new PricePointValue(secondPoint);
    }

    public PriceLineValue(PricePointValue firstPoint, PricePointValue secondPoint)
    {
        FirstPoint  = firstPoint;
        SecondPoint = secondPoint;
    }

    public PriceLineValue(IPriceLine toClone)
    {
        FirstPoint  = toClone.FirstPoint;
        SecondPoint = toClone.SecondPoint;
    }

    public PriceLineValue(PriceLineValue toClone)
    {
        FirstPoint  = toClone.FirstPoint;
        SecondPoint = toClone.SecondPoint;
    }

    public PricePointValue FirstPoint  { get; }
    public PricePointValue SecondPoint { get; }
}

public class PriceLine : ReusableObject<IPriceLine>, IPriceLine
{
    public PriceLine() { }

    public PriceLine(decimal firstSingleValue, DateTime firstAtTime, decimal secondSingleValue, DateTime secondAtTime)
    {
        FirstPoint  = new PricePointValue(firstSingleValue, firstAtTime);
        SecondPoint = new PricePointValue(secondSingleValue, secondAtTime);
    }

    public PriceLine(IPricePoint firstPoint, IPricePoint secondPoint)
    {
        FirstPoint  = new PricePointValue(firstPoint);
        SecondPoint = new PricePointValue(secondPoint);
    }

    public PriceLine(PricePointValue firstPoint, PricePointValue secondPoint)
    {
        FirstPoint  = firstPoint;
        SecondPoint = secondPoint;
    }

    public PriceLine(IPriceLine toClone)
    {
        FirstPoint  = toClone.FirstPoint;
        SecondPoint = toClone.SecondPoint;
    }

    public PriceLine(PriceLineValue toClone)
    {
        FirstPoint  = toClone.FirstPoint;
        SecondPoint = toClone.SecondPoint;
    }

    public PricePointValue FirstPoint  { get; set; }
    public PricePointValue SecondPoint { get; set; }

    public override IPriceLine CopyFrom(IPriceLine source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        FirstPoint  = source.FirstPoint;
        SecondPoint = source.SecondPoint;
        return this;
    }

    public override IPriceLine Clone() => Recycler?.Borrow<PriceLine>().CopyFrom(this) ?? new PriceLine((IPriceLine)this);


    public static implicit operator PriceLineValue(PriceLine priceLine) => new((IPriceLine)priceLine);
}
