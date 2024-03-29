﻿#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public interface IPQTraderPriceVolumeLayer : IMutableTraderPriceVolumeLayer, IPQPriceVolumeLayer,
    IEnumerable<IPQTraderLayerInfo>, IPQSupportsStringUpdates<IPriceVolumeLayer>
{
    new IPQTraderLayerInfo? this[int index] { get; set; }
    IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
    new IPQTraderPriceVolumeLayer Clone();
    new IEnumerator<IPQTraderLayerInfo> GetEnumerator();
}

public class PQTraderPriceVolumeLayer : PQPriceVolumeLayer, IPQTraderPriceVolumeLayer
{
    private const int PositionShift = 24;
    private const int IsTraderCountFlag = 0x00800000;
    private const uint TraderInfoIdentifierMask = 0x007FFFFF;
    private const int TraderInfoMaxSignifcanDigits = 6;
    public const string TraderCountTraderName = TraderPriceVolumeLayer.TraderCountTraderName;
    protected readonly IList<IPQTraderLayerInfo?> TraderDetails = new List<IPQTraderLayerInfo?>();

    public PQTraderPriceVolumeLayer(IPQNameIdLookupGenerator initialDictionary) =>
        TraderNameIdLookup = initialDictionary ??
                             new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                 PQFieldFlags.TraderNameIdLookupSubDictionaryKey);

    public PQTraderPriceVolumeLayer(decimal price = 0m, decimal volume = 0m,
        IPQNameIdLookupGenerator? traderIdToNameLookup = null)
        : base(price, volume) =>
        TraderNameIdLookup = traderIdToNameLookup ??
                             new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                 PQFieldFlags.TraderNameIdLookupSubDictionaryKey);

    public PQTraderPriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is IPQTraderPriceVolumeLayer pqTraderToClone)
        {
            TraderNameIdLookup = pqTraderToClone.TraderNameIdLookup.Clone();
            TraderDetails = new List<IPQTraderLayerInfo?>();
            foreach (var pqTraderLayerInfoToClone in pqTraderToClone)
                TraderDetails.Add(pqTraderLayerInfoToClone.Clone());
        }
        else if (toClone is ITraderPriceVolumeLayer traderPvl)
        {
            foreach (var traderLayerInfo in traderPvl) TraderDetails.Add(new PQTraderLayerInfo(traderLayerInfo));
        }

        if (TraderNameIdLookup == null)
            TraderNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
    }

    protected string PQTraderPriceVolumeLayerToStringMembers =>
        $"{PQPriceVolumeLayerToStringMembers}, {nameof(TraderDetails)}: [{string.Join(", ", TraderDetails)}]";

    IMutableTraderLayerInfo? IMutableTraderPriceVolumeLayer.this[int i]
    {
        get => this[i];
        set => this[i] = value as IPQTraderLayerInfo;
    }

    ITraderLayerInfo? ITraderPriceVolumeLayer.this[int i] => this[i];

    public IPQTraderLayerInfo? this[int i]
    {
        get
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = TraderDetails.Count; j <= i; j++) TraderDetails.Add(new PQTraderLayerInfo(TraderNameIdLookup));
            return TraderDetails[i];
        }
        set
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = TraderDetails.Count; j <= i; j++)
                TraderDetails.Add(j < i - 1 ? new PQTraderLayerInfo(TraderNameIdLookup) : null);
            TraderDetails[i] = value;
        }
    }

    public override bool IsEmpty => base.IsEmpty && TraderDetails.All(tli => tli?.IsEmpty ?? true);

    public override bool HasUpdates
    {
        get
        {
            return base.HasUpdates || TraderNameIdLookup.HasUpdates ||
                   TraderDetails.Any(tli => tli?.HasUpdates ?? false);
        }
        set
        {
            base.HasUpdates = value;
            foreach (var pqTraderLayerInfo in TraderDetails.Where(pqtli => pqtli != null))
                pqTraderLayerInfo!.HasUpdates = value;
            TraderNameIdLookup.HasUpdates = value;
        }
    }

    public int Count
    {
        get
        {
            for (var i = TraderDetails?.Count - 1 ?? -1; i >= 0; i--)
            {
                var traderLayerInfo = TraderDetails?[i];
                if (!(traderLayerInfo?.IsEmpty ?? true)) return i + 1;
            }

            return 0;
        }
    }

    public bool IsTraderCountOnly
    {
        get
        {
            for (var i = TraderDetails.Count - 1; i >= 0; i--)
            {
                var mutableTraderLayerInfo = TraderDetails[i];
                if ((mutableTraderLayerInfo?.TraderName != null &&
                     mutableTraderLayerInfo?.TraderName != TraderCountTraderName) ||
                    (mutableTraderLayerInfo?.TraderVolume ?? 0) > 0)
                    return false;
            }

            return true;
        }
    }

    public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }

    public bool RemoveAt(int index)
    {
        TraderDetails[index]?.StateReset();
        return true;
    }

    public void Add(string traderName, decimal volume)
    {
        var indexToUpdate = Count;
        AssertMaxTraderSizeNotExceeded(indexToUpdate);
        UpsertTraderName(indexToUpdate, TraderNameIdLookup.GetId(traderName));
        UpsertTraderVolume(indexToUpdate, volume);
    }

    public void SetTradersCountOnly(int numberOfTraders)
    {
        AssertMaxTraderSizeNotExceeded(numberOfTraders);
        foreach (var pqTraderLayerInfo in TraderDetails) pqTraderLayerInfo?.StateReset();
        var traderCountId = TraderNameIdLookup.GetId(TraderCountTraderName);
        for (var i = 0; i < numberOfTraders; i++) UpsertTraderName(i, traderCountId);
    }

    public override void StateReset()
    {
        base.StateReset();
        foreach (var pqTraderLayerInfo in TraderDetails) pqTraderLayerInfo?.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
                     quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (IsTraderCountOnly)
        {
            if (!updatedOnly || TraderDetails.Any(tli => tli?.HasUpdates ?? false))
                yield return new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset,
                    IsTraderCountFlag | Count);
        }
        else
        {
            var numberOfTraderInfos = TraderDetails.Count;
            for (var i = 0; i < numberOfTraderInfos; i++)
            {
                var tli = TraderDetails[i];
                var traderPosOffset = (uint)i << PositionShift;
                if (tli != null && (!updatedOnly || tli.IsTraderNameUpdated))
                {
                    var traderId = traderPosOffset | (uint)tli.TraderNameId;
                    yield return new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset, traderId);
                }

                if (tli != null && (!updatedOnly || tli.IsTraderVolumeUpdated))
                {
                    var value = PQScaling.AutoScale(tli.TraderVolume, TraderInfoMaxSignifcanDigits,
                        out var flag);
                    yield return new PQFieldUpdate(PQFieldKeys.LayerTraderVolumeOffset,
                        traderPosOffset | value, flag);
                }
            }
        }
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id >= PQFieldKeys.LayerTraderIdOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LayerTraderIdOffset +
            PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            var index = pqFieldUpdate.Value >> PositionShift;
            var traderCountOrId = (int)(pqFieldUpdate.Value & TraderInfoIdentifierMask);
            var isTraderCount = (pqFieldUpdate.Value & IsTraderCountFlag) == IsTraderCountFlag;

            if (isTraderCount)
                SetTradersCountOnly(traderCountOrId);
            else
                UpsertTraderName((int)index, traderCountOrId);
            return 0;
        }

        if (pqFieldUpdate.Id >= PQFieldKeys.LayerTraderVolumeOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LayerTraderVolumeOffset +
            PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            var index = pqFieldUpdate.Value >> PositionShift;
            UpsertTraderVolume((int)index, PQScaling.Unscale(pqFieldUpdate.Value &
                                                             TraderInfoIdentifierMask, pqFieldUpdate.Flag));
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime,
        UpdateStyle updatedStyle)
    {
        if (TraderNameIdLookup is IPQNameIdLookupGenerator pqNameIdLookupGenerator)
            foreach (var stringUpdate in pqNameIdLookupGenerator.GetStringUpdates(snapShotTime,
                         updatedStyle))
                yield return stringUpdate;
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (updates.Field.Id != PQFieldKeys.LayerNameDictionaryUpsertCommand) return false;
        if (TraderNameIdLookup != null) return TraderNameIdLookup.UpdateFieldString(updates);
        return false;
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source,
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var pqtpvl = source as PQTraderPriceVolumeLayer;
        if (source is ITraderPriceVolumeLayer tpvl && pqtpvl == null)
        {
            var i = 0;
            foreach (var traderLayerInfo in tpvl)
            {
                UpsertTraderName(i, TraderNameIdLookup.GetId(traderLayerInfo.TraderName));
                UpsertTraderVolume(i++, traderLayerInfo.TraderVolume);
            }

            while (i < TraderDetails.Count)
            {
                UpsertTraderName(i, 0);
                UpsertTraderVolume(i++, 0);
            }
        }

        if (pqtpvl != null)
        {
            TraderNameIdLookup.CopyFrom(pqtpvl.TraderNameIdLookup);
            for (var i = 0; i < pqtpvl.TraderDetails.Count; i++)
            {
                var otherTrdrLayerInfo = pqtpvl.TraderDetails[i];
                if (otherTrdrLayerInfo?.HasUpdates ?? false)
                {
                    if (otherTrdrLayerInfo.IsTraderNameUpdated)
                        UpsertTraderName(i, TraderNameIdLookup.GetId(otherTrdrLayerInfo.TraderName));
                    if (otherTrdrLayerInfo.IsTraderVolumeUpdated)
                        UpsertTraderVolume(i, otherTrdrLayerInfo.TraderVolume);
                    if (TraderDetails[i] is { } currTraderDets) currTraderDets.TraderNameIdLookup = TraderNameIdLookup;
                }
            }

            for (var i = pqtpvl.TraderDetails.Count; i < TraderDetails.Count; i++)
            {
                UpsertTraderName(i, 0);
                UpsertTraderVolume(i, 0);
                if (TraderDetails[i] is { } currTraderDets)
                {
                    currTraderDets.TraderNameIdLookup = TraderNameIdLookup;
                    currTraderDets.HasUpdates = false;
                }
            }
        }

        return this;
    }

    public override void EnsureRelatedItemsAreConfigured(ISourceTickerQuoteInfo? referenceInstance)
    {
        if (referenceInstance is IPQSourceTickerQuoteInfo pqSrcTkrQtInfo)
            if (ReferenceEquals(pqSrcTkrQtInfo.TraderNameIdLookup, TraderNameIdLookup))
                TraderNameIdLookup = pqSrcTkrQtInfo.TraderNameIdLookup.Clone();
        if (TraderNameIdLookup == null)
            TraderNameIdLookup = new PQNameIdLookupGenerator(
                PQFieldKeys.LayerNameDictionaryUpsertCommand, 2);
    }

    public override void EnsureRelatedItemsAreConfigured(IPQPriceVolumeLayer? referenceInstance)
    {
        if (referenceInstance is IPQTraderPriceVolumeLayer pqTrdrPvLayer)
        {
            TraderNameIdLookup = pqTrdrPvLayer.TraderNameIdLookup;
            foreach (var pqTraderLayerInfo in TraderDetails)
                if (pqTraderLayerInfo != null)
                    pqTraderLayerInfo.TraderNameIdLookup = pqTrdrPvLayer.TraderNameIdLookup;
        }
    }

    IPQTraderPriceVolumeLayer IPQTraderPriceVolumeLayer.Clone() => (IPQTraderPriceVolumeLayer)Clone();

    ITraderPriceVolumeLayer ICloneable<ITraderPriceVolumeLayer>.Clone() => (ITraderPriceVolumeLayer)Clone();

    ITraderPriceVolumeLayer ITraderPriceVolumeLayer.Clone() => (ITraderPriceVolumeLayer)Clone();

    IMutableTraderPriceVolumeLayer IMutableTraderPriceVolumeLayer.Clone() => (IMutableTraderPriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQTraderPriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ITraderPriceVolumeLayer traderPvLayer)) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);
        var countSame = Count == traderPvLayer.Count;
        var traderDetailsSame = TraderDetails.Zip(traderPvLayer, (ftd, std) =>
            ftd != null && ftd.AreEquivalent(std, exactTypes)).All(same => same);

        return baseSame && countSame && traderDetailsSame;
    }

    public IEnumerator<IPQTraderLayerInfo> GetEnumerator() => TraderDetails.Take(Count).GetEnumerator()!;

    IEnumerator<IMutableTraderLayerInfo> IMutableTraderPriceVolumeLayer.GetEnumerator() => GetEnumerator();

    IEnumerator<IPQTraderLayerInfo> IEnumerable<IPQTraderLayerInfo>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableTraderLayerInfo> IEnumerable<IMutableTraderLayerInfo>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ITraderLayerInfo> IEnumerable<ITraderLayerInfo>.GetEnumerator() => GetEnumerator();

    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (TraderDetails != null ? TraderDetails.GetHashCode() : 0);
        }
    }

    public override string ToString() => $"{GetType().Name}({PQTraderPriceVolumeLayerToStringMembers})";

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private void AssertMaxTraderSizeNotExceeded(int proposedNewIndex)
    {
        if (proposedNewIndex > byte.MaxValue)
            throw new ArgumentOutOfRangeException($"Max Traders represented is {byte.MaxValue}");
    }

    private void UpsertTraderName(int index, int traderNameId)
    {
        for (var i = TraderDetails.Count; i <= index; i++) TraderDetails.Add(new PQTraderLayerInfo(TraderNameIdLookup));
        var traderLayer = TraderDetails[index]!;
        traderLayer.TraderNameIdLookup = TraderNameIdLookup;
        TraderDetails[index]!.TraderNameId = traderNameId;
    }

    private void UpsertTraderVolume(int index, decimal volume)
    {
        for (var i = TraderDetails.Count; i <= index; i++) TraderDetails.Add(new PQTraderLayerInfo(TraderNameIdLookup));
        TraderDetails[index]!.TraderVolume = volume;
    }
}
