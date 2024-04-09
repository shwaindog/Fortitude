#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

[Flags]
public enum TraderLayerInfoFlags : byte
{
    None = 0x00
    , TraderNameUpdatedFlag = 0x01
    , TraderVolumeUpdatedFlag = 0x02
}

public interface IPQTraderLayerInfo : IMutableTraderLayerInfo
{
    bool HasUpdates { get; set; }
    int TraderNameId { get; set; }
    bool IsTraderNameUpdated { get; set; }
    bool IsTraderVolumeUpdated { get; set; }
    IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
    new IPQTraderLayerInfo Clone();
}

public class PQTraderLayerInfo : ReusableObject<ITraderLayerInfo>, IPQTraderLayerInfo
{
    private int traderNameId;

    protected TraderLayerInfoFlags UpdatedFlags;
    private decimal volume;

    public PQTraderLayerInfo() => TraderNameIdLookup = null!;

    public PQTraderLayerInfo(IPQNameIdLookupGenerator? lookupDict, string? traderName = null,
        decimal traderVolume = 0m)
    {
        TraderNameIdLookup = lookupDict ?? new PQNameIdLookupGenerator(
            PQFieldKeys.LayerNameDictionaryUpsertCommand,
            PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
        TraderName = traderName;
        TraderVolume = traderVolume;
    }

    public PQTraderLayerInfo(ITraderLayerInfo toClone)
    {
        var pqTraderLayerInfo = toClone as PQTraderLayerInfo;
        if (pqTraderLayerInfo != null)
            TraderNameIdLookup = pqTraderLayerInfo.TraderNameIdLookup;
        else
            TraderNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
        TraderName = toClone.TraderName;
        TraderVolume = toClone.TraderVolume;

        if (pqTraderLayerInfo != null) UpdatedFlags = pqTraderLayerInfo.UpdatedFlags;
    }

    protected string PQTraderLayerInfoToStringMembers => $"{nameof(TraderName)}: {TraderName}, {nameof(TraderVolume)}: {TraderVolume:N2}";

    public int TraderNameId
    {
        get => traderNameId;
        set
        {
            if (value == traderNameId) return;
            IsTraderNameUpdated = true;
            traderNameId = value;
        }
    }

    public string? TraderName
    {
        get => TraderNameIdLookup[traderNameId];
        set
        {
            var convertedTraderId = TraderNameIdLookup.GetOrAddId(value);
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
            IsTraderVolumeUpdated = true;
            volume = value;
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

    public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }

    public bool HasUpdates
    {
        get => UpdatedFlags != TraderLayerInfoFlags.None;
        set => UpdatedFlags = value ? UpdatedFlags.AllFlags() : TraderLayerInfoFlags.None;
    }

    public bool IsEmpty => TraderName == null && TraderVolume == 0;

    public override void StateReset()
    {
        TraderName = null;
        TraderVolume = 0;
        base.StateReset();
    }

    public override ITraderLayerInfo CopyFrom(ITraderLayerInfo source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var pqTrdrLyrInfo = source as IPQTraderLayerInfo;
        if (source is ITraderLayerInfo traderLayerInfo && pqTrdrLyrInfo == null)
        {
            TraderName = traderLayerInfo.TraderName;
            TraderVolume = traderLayerInfo.TraderVolume;
        }
        else if (pqTrdrLyrInfo != null)
        {
            TraderNameIdLookup.CopyFrom(pqTrdrLyrInfo.TraderNameIdLookup);
            if (pqTrdrLyrInfo.IsTraderNameUpdated) TraderName = pqTrdrLyrInfo.TraderName;
            if (pqTrdrLyrInfo.IsTraderVolumeUpdated) TraderName = pqTrdrLyrInfo.TraderName;
            UpdatedFlags = (source as PQTraderLayerInfo)?.UpdatedFlags ?? UpdatedFlags;
        }

        return this;
    }

    object ICloneable.Clone() => Clone();

    IMutableTraderLayerInfo IMutableTraderLayerInfo.Clone() => (IMutableTraderLayerInfo)Clone();

    IPQTraderLayerInfo IPQTraderLayerInfo.Clone() => (IPQTraderLayerInfo)Clone();

    public override ITraderLayerInfo Clone() => Recycler?.Borrow<PQTraderLayerInfo>().CopyFrom(this) ?? new PQTraderLayerInfo(this);

    public virtual bool AreEquivalent(ITraderLayerInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var traderNameSame = TraderName == other.TraderName;
        var volumeSame = TraderVolume == other.TraderVolume;

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
