// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public class MarketAggregate : ReusableObject<IMarketAggregate>, IMutableMarketAggregate
{
    public MarketAggregate()
    {
    }

    public MarketAggregate(MarketDataSource dataSource, decimal volume, decimal vwap, DateTime? updateTime = null)
    {
        DataSource = dataSource;
        UpdateTime = updateTime ?? DateTime.MinValue;
        Volume     = volume;
        Vwap       = vwap;
    }

    public MarketAggregate(IMarketAggregate toClone)
    {
        DataSource = toClone.DataSource;
        UpdateTime = toClone.UpdateTime;
        Volume     = toClone.Volume;
        Vwap       = toClone.Vwap;
    }

    public MarketDataSource DataSource { get; set; }

    public DateTime UpdateTime { get; set; }

    public decimal Volume { get; set; }
    public decimal Vwap { get; set; }

    public bool IsEmpty
    {
        get => DataSource == MarketDataSource.None && UpdateTime == DateTime.MinValue && Volume == 0m && Vwap == 0m;
        set
        {
            if (!value) return;
            DataSource = MarketDataSource.None;
            UpdateTime = DateTime.MinValue;
            Volume     = 0m;
            Vwap       = 0m;
        }
    }
    IMutableMarketAggregate ITrackableReset<IMutableMarketAggregate>.ResetWithTracking() => ResetWithTracking();

    public MarketAggregate ResetWithTracking()
    {
        IsEmpty = true;
        return this;
    }

    public override void StateReset()
    {
        IsEmpty = true;
        base.StateReset();
    }

    IMutableMarketAggregate ICloneable<IMutableMarketAggregate>.Clone() => Clone();

    IMutableMarketAggregate IMutableMarketAggregate.Clone() => Clone();

    public override MarketAggregate Clone() => 
        Recycler?.Borrow<MarketAggregate>().CopyFrom(this) ?? new MarketAggregate(this);

    public override MarketAggregate CopyFrom(IMarketAggregate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        DataSource = source.DataSource;
        UpdateTime = source.UpdateTime;
        Volume     = source.Volume;
        Vwap       = source.Vwap;
        return this;
    }

    public bool AreEquivalent(IMarketAggregate? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var dataSourceSame = DataSource == other.DataSource;
        var updateTimeSame = true;
        if (dataSourceSame && DataSource != MarketDataSource.Published)
        {
            updateTimeSame = UpdateTime == other.UpdateTime;
        }
        var volumeSame     = Volume == other.Volume;
        var vwapSame       = Vwap == other.Vwap;

        var allSame = dataSourceSame && updateTimeSame && volumeSame && vwapSame;

        return allSame;
    }

    public bool AreEquivalent(IMutableMarketAggregate? other, bool exactTypes = false) => AreEquivalent((IMarketAggregate?)other, exactTypes);
    
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMarketAggregate, true);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(DataSource);
        hashCode.Add(UpdateTime);
        hashCode.Add(Volume);
        hashCode.Add(Vwap);

        return hashCode.ToHashCode();
    }


    protected string OpenInterestToStringMembers =>$"{nameof(DataSource)}: {DataSource}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(Volume)}: {Volume:N2}, {nameof(Vwap)}: {Vwap:N5}";

    public override string ToString() => $"{nameof(MarketAggregate)}{{{OpenInterestToStringMembers}}}";
}
