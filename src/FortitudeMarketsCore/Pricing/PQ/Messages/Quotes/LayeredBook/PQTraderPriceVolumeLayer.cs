// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQTraderPriceVolumeLayer : IMutableTraderPriceVolumeLayer, IPQPriceVolumeLayer,
    IEnumerable<IPQTraderLayerInfo>, IPQSupportsStringUpdates<IPriceVolumeLayer>, IHasNameIdLookup
{
    new IPQTraderLayerInfo? this[int index] { get; set; }
    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQTraderPriceVolumeLayer       Clone();
    new IEnumerator<IPQTraderLayerInfo> GetEnumerator();
}

public class PQTraderPriceVolumeLayer : PQPriceVolumeLayer, IPQTraderPriceVolumeLayer
{
    private const int  PositionShift                = 24;
    private const int  IsTraderCountFlag            = 0x0080_0000;
    private const uint TraderInfoIdentifierMask     = 0x007F_FFFF;
    private const int  TraderInfoMaxSignifcanDigits = 6;

    public const string TraderCountOnlyName = TraderPriceVolumeLayer.TraderCountOnlyName;

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQTraderPriceVolumeLayer));

    protected readonly IList<IPQTraderLayerInfo?> TraderDetails = new List<IPQTraderLayerInfo?>();
    private            IPQNameIdLookupGenerator   nameIdLookup  = null!;

    public PQTraderPriceVolumeLayer(IPQNameIdLookupGenerator initialDictionary) => NameIdLookup = initialDictionary;

    public PQTraderPriceVolumeLayer(IPQNameIdLookupGenerator traderIdToNameLookup, decimal price = 0m, decimal volume = 0m)
        : base(price, volume) =>
        NameIdLookup = traderIdToNameLookup;

    public PQTraderPriceVolumeLayer(IPriceVolumeLayer toClone, IPQNameIdLookupGenerator ipqNameIdLookupGenerator) : base(toClone)
    {
        NameIdLookup = ipqNameIdLookupGenerator;
        if (toClone is ITraderPriceVolumeLayer traderPvl)
            foreach (var traderLayerInfo in traderPvl)
                TraderDetails.Add(new PQTraderLayerInfo(traderLayerInfo, ipqNameIdLookupGenerator));
    }

    protected string PQTraderPriceVolumeLayerToStringMembers =>
        $"{PQPriceVolumeLayerToStringMembers}, {nameof(TraderDetails)}: [{string.Join(", ", TraderDetails)}]";

    public override LayerType  LayerType          => LayerType.TraderPriceVolume;
    public override LayerFlags SupportsLayerFlags => LayerFlags.TraderName | LayerFlags.TraderCount | LayerFlags.TraderSize | base.SupportsLayerFlags;

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
            for (var j = TraderDetails.Count; j <= i; j++) TraderDetails.Add(new PQTraderLayerInfo(NameIdLookup));
            return TraderDetails[i];
        }
        set
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = TraderDetails.Count; j <= i; j++)
                TraderDetails.Add(j < i - 1 ? new PQTraderLayerInfo(NameIdLookup) : null);
            TraderDetails[i] = value;
        }
    }

    public override bool IsEmpty
    {
        get => base.IsEmpty && TraderDetails.All(tli => tli?.IsEmpty ?? true);
        set
        {
            if (!value) return;
            foreach (var pqTraderLayerInfo in TraderDetails)
                if (pqTraderLayerInfo != null)
                    pqTraderLayerInfo.IsEmpty = true;
            base.IsEmpty = true;
        }
    }

    public override bool HasUpdates
    {
        get
        {
            return base.HasUpdates || NameIdLookup.HasUpdates ||
                   TraderDetails.Any(tli => tli?.HasUpdates ?? false);
        }
        set
        {
            base.HasUpdates = value;
            foreach (var pqTraderLayerInfo in TraderDetails.Where(pqtli => pqtli != null))
                pqTraderLayerInfo!.HasUpdates = value;
            NameIdLookup.HasUpdates = value;
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
                     mutableTraderLayerInfo?.TraderName != TraderCountOnlyName) ||
                    (mutableTraderLayerInfo?.TraderVolume ?? 0) > 0)
                    return false;
            }

            return TraderDetails.Any(tli => tli?.TraderName == TraderCountOnlyName);
        }
    }

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public virtual IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (nameIdLookup == value) return;
            nameIdLookup = value;
            foreach (var pqTraderLayerInfo in TraderDetails.OfType<IPQTraderLayerInfo>()) pqTraderLayerInfo.NameIdLookup = nameIdLookup;
        }
    }

    public bool RemoveAt(int index)
    {
        TraderDetails[index]?.StateReset();
        return true;
    }

    public void Add(string traderName, decimal volume)
    {
        var indexToUpdate = Count;
        AssertMaxTraderSizeNotExceeded(indexToUpdate);
        UpsertTraderName(indexToUpdate, NameIdLookup.GetId(traderName));
        UpsertTraderVolume(indexToUpdate, volume);
    }

    public void SetTradersCountOnly(int numberOfTraders)
    {
        AssertMaxTraderSizeNotExceeded(numberOfTraders);
        foreach (var pqTraderLayerInfo in TraderDetails) pqTraderLayerInfo?.StateReset();
        var traderCountId = NameIdLookup.GetId(TraderCountOnlyName);
        for (var i = 0; i < numberOfTraders; i++) UpsertTraderName(i, traderCountId);
    }

    public override void StateReset()
    {
        base.StateReset();
        foreach (var pqTraderLayerInfo in TraderDetails) pqTraderLayerInfo?.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;

        if (IsTraderCountOnly)
        {
            if (!updatedOnly || TraderDetails.Any(tli => tli?.HasUpdates ?? false))
                yield return new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset, IsTraderCountFlag | Count);
        }
        else
        {
            var numberOfTraderInfos = Math.Min(byte.MaxValue + 1, TraderDetails.Count);
            for (byte i = 0; i < numberOfTraderInfos; i++)
            {
                var tli = TraderDetails[i];
                if (tli != null && (tli.HasUpdates || !updatedOnly))
                {
                    var traderPosOffset = (uint)i << PositionShift;

                    if (!updatedOnly || tli.IsTraderNameUpdated)
                    {
                        var tliTraderNameId      = (uint)tli.TraderNameId;
                        var traderIndexAndNameId = traderPosOffset | tliTraderNameId;
                        yield return new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset, traderIndexAndNameId);
                    }

                    if (!updatedOnly || tli.IsTraderVolumeUpdated)
                    {
                        var value            = PQScaling.AutoScale(tli.TraderVolume, TraderInfoMaxSignifcanDigits, out var flag);
                        var volumePlusOffset = traderPosOffset | value;
                        yield return new PQFieldUpdate(PQFieldKeys.LayerTraderVolumeOffset, volumePlusOffset, flag);
                    }
                }
                if (i == byte.MaxValue) yield break;
            }
        }
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id is >= PQFieldKeys.LayerTraderIdOffset and < PQFieldKeys.LayerTraderIdOffset +
                                                                         PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            var index           = pqFieldUpdate.Value >> PositionShift;
            var traderCountOrId = (int)(pqFieldUpdate.Value & TraderInfoIdentifierMask);
            var isTraderCount   = (pqFieldUpdate.Value & IsTraderCountFlag) == IsTraderCountFlag;

            if (isTraderCount)
                SetTradersCountOnly(traderCountOrId);
            else
                UpsertTraderName((int)index, traderCountOrId);
            return 0;
        }

        if (pqFieldUpdate.Id is >= PQFieldKeys.LayerTraderVolumeOffset and < PQFieldKeys.LayerTraderVolumeOffset +
                                                                             PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            var index = pqFieldUpdate.Value >> PositionShift;
            UpsertTraderVolume((int)index, PQScaling.Unscale(pqFieldUpdate.Value &
                                                             TraderInfoIdentifierMask, pqFieldUpdate.Flag));
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        if (NameIdLookup is IPQNameIdLookupGenerator pqNameIdLookupGenerator)
            foreach (var stringUpdate in pqNameIdLookupGenerator.GetStringUpdates(snapShotTime,
                                                                                  messageFlags))
                yield return stringUpdate;
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFieldKeys.LayerNameDictionaryUpsertCommand) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var sourceTraderLayers = 0;

        var isFullReplace = copyMergeFlags.HasFullReplace();
        var pqtpvl        = source as PQTraderPriceVolumeLayer;
        var tpvl          = source as ITraderPriceVolumeLayer;
        if (tpvl != null && pqtpvl == null)
        {
            var i = 0;
            foreach (var traderLayerInfo in tpvl)
            {
                UpsertTraderName(i, NameIdLookup.GetId(traderLayerInfo.TraderName));
                UpsertTraderVolume(i++, traderLayerInfo.TraderVolume);
            }
            sourceTraderLayers = i;
        }
        else if (pqtpvl != null)
        {
            NameIdLookup.CopyFrom(pqtpvl.NameIdLookup);
            sourceTraderLayers = pqtpvl.Count;
            for (var i = 0; i < sourceTraderLayers; i++)
            {
                var otherTrdrLayerInfo = pqtpvl.TraderDetails[i];
                if (otherTrdrLayerInfo != null && (otherTrdrLayerInfo.HasUpdates || isFullReplace))
                {
                    if (otherTrdrLayerInfo.IsTraderNameUpdated || isFullReplace)
                        UpsertTraderName(i, NameIdLookup.GetId(otherTrdrLayerInfo.TraderName));
                    if (otherTrdrLayerInfo.IsTraderVolumeUpdated || isFullReplace)
                        UpsertTraderVolume(i, otherTrdrLayerInfo.TraderVolume);
                }
            }
        }
        if (tpvl != null)
            for (var i = sourceTraderLayers; i < TraderDetails.Count; i++)
                if (TraderDetails[i] is { } makeEmpty)
                    makeEmpty.IsEmpty = true;

        if (pqtpvl != null && isFullReplace) SetFlagsSame(source);

        return this;
    }

    IPQTraderPriceVolumeLayer IPQTraderPriceVolumeLayer.Clone() => (IPQTraderPriceVolumeLayer)Clone();

    ITraderPriceVolumeLayer ICloneable<ITraderPriceVolumeLayer>.Clone() => (ITraderPriceVolumeLayer)Clone();

    ITraderPriceVolumeLayer ITraderPriceVolumeLayer.Clone() => (ITraderPriceVolumeLayer)Clone();

    IMutableTraderPriceVolumeLayer IMutableTraderPriceVolumeLayer.Clone() => (IMutableTraderPriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQTraderPriceVolumeLayer(this, NameIdLookup);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ITraderPriceVolumeLayer traderPvLayer)) return false;
        var baseSame  = base.AreEquivalent(other, exactTypes);
        var countSame = Count == traderPvLayer.Count;
        var traderDetailsSame = TraderDetails.Zip(traderPvLayer, (ftd, std) =>
                                                      ftd != null && ftd.AreEquivalent(std, exactTypes)).All(same => same);

        var allAreSame = baseSame && countSame && traderDetailsSame;
        return allAreSame;
    }

    public IEnumerator<IPQTraderLayerInfo> GetEnumerator() => TraderDetails.Take(Count).GetEnumerator()!;

    IEnumerator<IMutableTraderLayerInfo> IMutableTraderPriceVolumeLayer.GetEnumerator() => GetEnumerator();

    IEnumerator<IPQTraderLayerInfo> IEnumerable<IPQTraderLayerInfo>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableTraderLayerInfo> IEnumerable<IMutableTraderLayerInfo>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ITraderLayerInfo> IEnumerable<ITraderLayerInfo>.GetEnumerator() => GetEnumerator();

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

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
            throw new ArgumentOutOfRangeException($"Max Traders represented is {byte.MaxValue}. Got {proposedNewIndex}");
    }

    private void UpsertTraderName(int index, int traderNameId)
    {
        for (var i = TraderDetails.Count; i <= index; i++) TraderDetails.Add(new PQTraderLayerInfo(NameIdLookup));
        var traderLayer = TraderDetails[index]!;

        traderLayer.NameIdLookup = NameIdLookup;
        traderLayer.TraderNameId = traderNameId;
    }

    private void UpsertTraderVolume(int index, decimal volume)
    {
        for (var i = TraderDetails.Count; i <= index; i++) TraderDetails.Add(new PQTraderLayerInfo(NameIdLookup));
        TraderDetails[index]!.TraderVolume = volume;
    }
}
