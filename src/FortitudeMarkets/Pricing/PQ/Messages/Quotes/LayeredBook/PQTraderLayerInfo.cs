// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[Flags]
public enum TraderLayerInfoFlags : byte
{
    None                    = 0x00
  , TraderNameUpdatedFlag   = 0x01
  , TraderVolumeUpdatedFlag = 0x02
}

public interface IPQTraderLayerInfo : IMutableTraderLayerInfo, ISupportsPQNameIdLookupGenerator
{
    bool HasUpdates            { get; set; }
    int  TraderNameId          { get; set; }
    bool IsTraderNameUpdated   { get; set; }
    bool IsTraderVolumeUpdated { get; set; }

    new IPQTraderLayerInfo Clone();
}

public class PQTraderLayerInfo : ReusableObject<ITraderLayerInfo>, IPQTraderLayerInfo
{
    private IPQNameIdLookupGenerator nameIdLookup = null!;

    private int traderNameId;

    protected TraderLayerInfoFlags UpdatedFlags;

    private decimal volume;

    public PQTraderLayerInfo() => NameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);

    public PQTraderLayerInfo
    (IPQNameIdLookupGenerator lookupDict, string? traderName = null,
        decimal traderVolume = 0m)
    {
        NameIdLookup = lookupDict;
        TraderName   = traderName;
        TraderVolume = traderVolume;
    }

    public PQTraderLayerInfo(ITraderLayerInfo toClone, IPQNameIdLookupGenerator pqNameIdLookupGenerator)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        TraderName   = toClone.TraderName;
        TraderVolume = toClone.TraderVolume;

        if (toClone is PQTraderLayerInfo pqTraderLayerInfo) UpdatedFlags = pqTraderLayerInfo.UpdatedFlags;
    }

    protected string PQTraderLayerInfoToStringMembers => $"{nameof(TraderName)}: {TraderName}, {nameof(TraderVolume)}: {TraderVolume:N2}";

    public int TraderNameId
    {
        get => traderNameId;
        set
        {
            if (value == traderNameId) return;
            IsTraderNameUpdated = true;
            traderNameId        = value;
        }
    }

    public string? TraderName
    {
        get => NameIdLookup[traderNameId];
        set
        {
            var convertedTraderId = NameIdLookup.GetOrAddId(value);
            if (convertedTraderId <= 0 && value != null)
                throw new Exception("Error attempted to set the Trader Name to something " +
                                    "not defined in the source name to Id table.");
            TraderNameId = convertedTraderId;
        }
    }

    public bool IsTraderNameUpdated
    {
        get => (UpdatedFlags & TraderLayerInfoFlags.TraderNameUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= TraderLayerInfoFlags.TraderNameUpdatedFlag;

            else if (IsTraderNameUpdated) UpdatedFlags ^= TraderLayerInfoFlags.TraderNameUpdatedFlag;
        }
    }

    public decimal TraderVolume
    {
        get => volume;
        set
        {
            if (volume == value) return;
            if (value == decimal.Zero)
            {
                volume = value;

                IsTraderVolumeUpdated = true;
                return;
            }
            // limitation of serialization is that only 6 significant digits can be sent efficiently so set the same value here.
            var abs                = Math.Abs(value);
            var beforeDecimalPoint = abs < 1 ? 0 : (int)(Math.Log10(decimal.ToDouble(abs)) + 1);
            var round              = Math.Max(0, 6 - beforeDecimalPoint);
            var checkValue         = decimal.Round(value, round);

            abs = Math.Abs(checkValue);

            beforeDecimalPoint = abs < 1 ? 0 : (int)(Math.Log10(decimal.ToDouble(abs)) + 1);
            if (beforeDecimalPoint > 6)
            {
                var scale        = beforeDecimalPoint - 6;
                var scalePower10 = 10 * scale;
                checkValue -= checkValue % scalePower10;
            }
            volume                = checkValue;
            IsTraderVolumeUpdated = true;
        }
    }

    public bool IsTraderVolumeUpdated
    {
        get => (UpdatedFlags & TraderLayerInfoFlags.TraderVolumeUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= TraderLayerInfoFlags.TraderVolumeUpdatedFlag;

            else if (IsTraderVolumeUpdated) UpdatedFlags ^= TraderLayerInfoFlags.TraderVolumeUpdatedFlag;
        }
    }

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (nameIdLookup == value) return;
            string? cacheTraderName               = null;
            if (traderNameId > 0) cacheTraderName = TraderName;
            nameIdLookup = value;
            if (traderNameId > 0) traderNameId = nameIdLookup.GetOrAddId(cacheTraderName);
        }
    }

    public bool HasUpdates
    {
        get => UpdatedFlags != TraderLayerInfoFlags.None;
        set => UpdatedFlags = value ? UpdatedFlags.AllFlags() : TraderLayerInfoFlags.None;
    }

    public bool IsEmpty
    {
        get => TraderName == null && TraderVolume == 0;
        set
        {
            if (!value) return;
            TraderName   = null;
            TraderVolume = 0;
        }
    }

    public override void StateReset()
    {
        TraderName   = null;
        TraderVolume = 0;
        UpdatedFlags = TraderLayerInfoFlags.None;
        base.StateReset();
    }

    public override ITraderLayerInfo CopyFrom
    (ITraderLayerInfo source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var pqTrdrLyrInfo = source as IPQTraderLayerInfo;
        if (source is ITraderLayerInfo traderLayerInfo && pqTrdrLyrInfo == null)
        {
            TraderName   = traderLayerInfo.TraderName;
            TraderVolume = traderLayerInfo.TraderVolume;
        }
        else if (pqTrdrLyrInfo != null)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            NameIdLookup.CopyFrom(pqTrdrLyrInfo.NameIdLookup);
            if (pqTrdrLyrInfo.IsTraderNameUpdated || isFullReplace) TraderName     = pqTrdrLyrInfo.TraderName;
            if (pqTrdrLyrInfo.IsTraderVolumeUpdated || isFullReplace) TraderVolume = pqTrdrLyrInfo.TraderVolume;

            if (isFullReplace) UpdatedFlags = (source as PQTraderLayerInfo)?.UpdatedFlags ?? UpdatedFlags;
        }

        return this;
    }

    object ICloneable.Clone() => Clone();

    IMutableTraderLayerInfo IMutableTraderLayerInfo.Clone() => (IMutableTraderLayerInfo)Clone();

    IPQTraderLayerInfo IPQTraderLayerInfo.Clone() => (IPQTraderLayerInfo)Clone();

    public override ITraderLayerInfo Clone() => Recycler?.Borrow<PQTraderLayerInfo>().CopyFrom(this) ?? new PQTraderLayerInfo(this, NameIdLookup);

    public virtual bool AreEquivalent(ITraderLayerInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var traderNameSame = TraderName == other.TraderName;
        var volumeSame     = TraderVolume == other.TraderVolume;

        var updatedSame = true;
        if (exactTypes)
        {
            var pqTraderLayerInfo = (PQTraderLayerInfo)other;
            updatedSame = UpdatedFlags == pqTraderLayerInfo.UpdatedFlags;
        }

        return traderNameSame && volumeSame && updatedSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ITraderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = TraderName?.GetHashCode() ?? 0 ^ TraderVolume.GetHashCode();
            return (hash * 397) ^ UpdatedFlags.GetHashCode();
        }
    }

    public override string ToString() => $"{GetType().Name}({PQTraderLayerInfoToStringMembers})";
}
