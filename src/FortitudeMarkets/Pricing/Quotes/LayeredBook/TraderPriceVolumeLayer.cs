// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class TraderPriceVolumeLayer : PriceVolumeLayer, IMutableTraderPriceVolumeLayer
{
    public const string TraderCountOnlyName = "TraderCountOnly";

    public TraderPriceVolumeLayer() => TraderDetails = new List<IMutableTraderLayerInfo?>();

    public TraderPriceVolumeLayer(decimal price = 0m, decimal volume = 0m) : base(price, volume) =>
        TraderDetails = new List<IMutableTraderLayerInfo?>();

    public TraderPriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is ITraderPriceVolumeLayer trdToClone)
        {
            TraderDetails = new List<IMutableTraderLayerInfo?>(trdToClone.Count);
            foreach (var traderLayerInfo in trdToClone.Where(tli => tli != null))
                TraderDetails.Add(traderLayerInfo is TraderLayerInfo
                                      ? (IMutableTraderLayerInfo)traderLayerInfo.Clone()
                                      : new TraderLayerInfo(traderLayerInfo.TraderName, traderLayerInfo.TraderVolume));
        }
        else
        {
            TraderDetails = new List<IMutableTraderLayerInfo?>(0);
        }
    }

    protected       IList<IMutableTraderLayerInfo?> TraderDetails { get; }
    public override LayerType LayerType => LayerType.TraderPriceVolume;
    public override LayerFlags SupportsLayerFlags => LayerFlags.TraderName | LayerFlags.TraderCount | LayerFlags.TraderSize | base.SupportsLayerFlags;

    public IMutableTraderLayerInfo? this[int i]
    {
        get
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = TraderDetails.Count; j <= i; j++) TraderDetails.Add(new TraderLayerInfo());
            return TraderDetails[i];
        }
        set
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = TraderDetails.Count; j <= i; j++) TraderDetails.Add(j < i - 1 ? new TraderLayerInfo() : null);
            TraderDetails[i] = value;
        }
    }

    ITraderLayerInfo? ITraderPriceVolumeLayer.this[int i] => this[i];

    public int Count
    {
        get
        {
            for (var i = TraderDetails?.Count - 1 ?? 0; i >= 0; i--)
            {
                var traderLayerInfo = TraderDetails?[i];
                if (!traderLayerInfo?.IsEmpty ?? false) return i + 1;
            }

            return 0;
        }
    }

    public override bool IsEmpty
    {
        get => base.IsEmpty && TraderDetails.All(tli => tli?.IsEmpty ?? true);
        set
        {
            if (!value) return;
            foreach (var traderLayerInfo in TraderDetails)
                if (traderLayerInfo != null)
                    traderLayerInfo.IsEmpty = true;
            base.IsEmpty = true;
        }
    }

    public bool IsTraderCountOnly
    {
        get
        {
            return TraderDetails
                .All(mutableTraderLayerInfo => (mutableTraderLayerInfo?.IsEmpty ?? true)
                                            || mutableTraderLayerInfo.TraderName == TraderCountOnlyName);
        }
    }

    public void Add(string traderName, decimal volume)
    {
        var indexToUpdate = Count;
        AssertMaxTraderSizeNotExceeded(indexToUpdate);
        if (indexToUpdate == TraderDetails.Count)
        {
            TraderDetails.Add(new TraderLayerInfo(traderName, volume));
        }
        else
        {
            var entryToUpdate = TraderDetails[indexToUpdate];
            if (entryToUpdate != null)
            {
                entryToUpdate.TraderName   = traderName;
                entryToUpdate.TraderVolume = volume;
            }
        }
    }

    public bool RemoveAt(int index)
    {
        TraderDetails[index]?.StateReset();
        return true;
    }

    public void SetTradersCountOnly(int numberOfTraders)
    {
        AssertMaxTraderSizeNotExceeded(numberOfTraders);
        StateReset();
        for (var i = 0; i < numberOfTraders; i++)
        {
            var traderInfo = this[i];
            if (traderInfo != null)
            {
                traderInfo.TraderName   = TraderCountOnlyName;
                traderInfo.TraderVolume = 0m;
            }
        }
    }

    public override void StateReset()
    {
        foreach (var traderLayerInfo in TraderDetails) traderLayerInfo?.StateReset();
        base.StateReset();
    }

    public override IPriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IMutableTraderPriceVolumeLayer sourceTraderPriceVolumeLayer)
        {
            for (var i = 0; i < sourceTraderPriceVolumeLayer.Count; i++)
                if (i < TraderDetails.Count)
                {
                    if (TraderDetails[i] != null)
                        TraderDetails[i]?.CopyFrom(sourceTraderPriceVolumeLayer[i]!);
                    else if (sourceTraderPriceVolumeLayer[i] != null)
                        TraderDetails[i] = new TraderLayerInfo(sourceTraderPriceVolumeLayer[i]!);
                }
                else
                {
                    TraderDetails.Add(new TraderLayerInfo(sourceTraderPriceVolumeLayer[i]!));
                }

            for (var i = TraderDetails.Count - 1; i >= sourceTraderPriceVolumeLayer.Count; i--)
                TraderDetails[i]?.StateReset();
        }

        return this;
    }

    ITraderPriceVolumeLayer ICloneable<ITraderPriceVolumeLayer>.Clone() => (ITraderPriceVolumeLayer)Clone();

    ITraderPriceVolumeLayer ITraderPriceVolumeLayer.Clone() => (ITraderPriceVolumeLayer)Clone();

    IMutableTraderPriceVolumeLayer IMutableTraderPriceVolumeLayer.Clone() => (IMutableTraderPriceVolumeLayer)Clone();

    public override IPriceVolumeLayer Clone() => Recycler?.Borrow<TraderPriceVolumeLayer>().CopyFrom(this) ?? new TraderPriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ITraderPriceVolumeLayer otherTvl)) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);
        var traderDetailsSame = TraderDetails.Zip(otherTvl,
                                                  (ftd, std) => ftd != null && ftd.AreEquivalent(std, exactTypes))
                                             .All(same => same);
        return baseSame && traderDetailsSame;
    }

    IEnumerator<ITraderLayerInfo> IEnumerable<ITraderLayerInfo>.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableTraderLayerInfo> GetEnumerator() => TraderDetails.Take(Count).GetEnumerator()!;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private void AssertMaxTraderSizeNotExceeded(int proposedNewIndex)
    {
        if (proposedNewIndex > byte.MaxValue)
            throw new ArgumentOutOfRangeException($"Max Traders represented is {byte.MaxValue}");
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ITraderPriceVolumeLayer?)obj, true);

    public override int GetHashCode() => base.GetHashCode() ^ TraderDetails.GetHashCode();

    public override string ToString() =>
        $"TraderPriceVolumeLayer {{{nameof(Price)}: {Price:N5}, {nameof(Volume)}: " +
        $"{Volume:N2}, {nameof(TraderDetails)}:[ {string.Join(", ", TraderDetails)}]," +
        $" {nameof(Count)}: {Count} }}";
}
