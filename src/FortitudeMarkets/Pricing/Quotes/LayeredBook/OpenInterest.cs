// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class OpenInterest : ReusableObject<IOpenInterest>, IMutableOpenInterest
{
    public OpenInterest()
    {
    }

    public OpenInterest(MarketDataSource dataSource, decimal volume, decimal vwap, DateTime? updateTime = null)
    {
        DataSource = dataSource;
        UpdateTime = updateTime ?? DateTime.MinValue;
        Volume     = volume;
        Vwap       = vwap;
    }

    public OpenInterest(IOpenInterest toClone)
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

    public override void StateReset()
    {
        IsEmpty = true;
        base.StateReset();
    }

    IMutableOpenInterest ICloneable<IMutableOpenInterest>.Clone() => Clone();

    IMutableOpenInterest IMutableOpenInterest.Clone() => Clone();

    public override OpenInterest Clone() => 
        Recycler?.Borrow<OpenInterest>().CopyFrom(this) ?? new OpenInterest(this);

    public override OpenInterest CopyFrom(IOpenInterest source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        DataSource = source.DataSource;
        UpdateTime = source.UpdateTime;
        Volume     = source.Volume;
        Vwap       = source.Vwap;
        return this;
    }

    public bool AreEquivalent(IOpenInterest? other, bool exactTypes = false)
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

    public bool AreEquivalent(IMutableOpenInterest? other, bool exactTypes = false) => AreEquivalent((IOpenInterest?)other, exactTypes);
    
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOpenInterest, true);

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

    public override string ToString() => $"{nameof(OpenInterest)}{{{OpenInterestToStringMembers}}}";
}
