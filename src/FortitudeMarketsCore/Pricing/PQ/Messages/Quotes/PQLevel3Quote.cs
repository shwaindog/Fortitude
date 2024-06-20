// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

public interface IPQLevel3Quote : IPQLevel2Quote, IMutableLevel3Quote
{
    new IPQRecentlyTraded? RecentlyTraded                { get; set; }
    bool                   IsValueDateUpdated            { get; set; }
    bool                   IsBatchIdUpdated              { get; set; }
    bool                   IsSourceQuoteReferenceUpdated { get; set; }
    new IPQLevel3Quote     Clone();
}

public class PQLevel3Quote : PQLevel2Quote, IPQLevel3Quote
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQLevel3Quote));

    private uint               batchId;
    private IPQRecentlyTraded? recentlyTraded;
    private uint               sourceQuoteRef;
    private DateTime           valueDate = DateTimeConstants.UnixEpoch;

    public PQLevel3Quote() => recentlyTraded = new PQRecentlyTraded();

    public PQLevel3Quote(ISourceTickerQuoteInfo uniqueSourceTickerIdentifier)
        : base(uniqueSourceTickerIdentifier)
    {
        if (PQSourceTickerQuoteInfo!.LastTradedFlags != LastTradedFlags.None)
            recentlyTraded = new PQRecentlyTraded(PQSourceTickerQuoteInfo);
    }

    public PQLevel3Quote(ILevel0Quote toClone) : base(toClone)
    {
        if (toClone is IPQLevel3Quote ipql3QToClone)
        {
            BatchId              = ipql3QToClone.BatchId;
            SourceQuoteReference = ipql3QToClone.SourceQuoteReference;
            ValueDate            = ipql3QToClone.ValueDate;
            recentlyTraded       = ipql3QToClone?.RecentlyTraded?.Clone();
        }

        if (toClone is PQLevel3Quote pql3QToClone)
        {
            IsValueDateUpdated            = pql3QToClone.IsValueDateUpdated;
            IsBatchIdUpdated              = pql3QToClone.IsBatchIdUpdated;
            IsSourceQuoteReferenceUpdated = pql3QToClone.IsSourceQuoteReferenceUpdated;
            IsValueDateUpdated            = pql3QToClone.IsSourceQuoteReferenceUpdated;

            UpdatedFlags = pql3QToClone.UpdatedFlags;
        }
    }

    protected string Level3ToStringMembers =>
        $"{Level2ToStringMembers}, {nameof(BatchId)}: {BatchId}, {nameof(IsBatchIdUpdated)}: {IsBatchIdUpdated}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference}, " +
        $"{nameof(IsSourceQuoteReferenceUpdated)}: {IsSourceQuoteReferenceUpdated}, {nameof(ValueDate)}: {ValueDate}, " +
        $"{nameof(IsValueDateUpdated)}: {IsValueDateUpdated}, {nameof(RecentlyTraded)}: {RecentlyTraded}";

    public override QuoteLevel QuoteLevel => QuoteLevel.Level3;

    public uint BatchId
    {
        get => batchId;
        set
        {
            if (batchId == value) return;
            IsBatchIdUpdated = true;
            batchId          = value;
        }
    }

    public bool IsBatchIdUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag;

            else if (IsBatchIdUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag;
        }
    }

    public uint SourceQuoteReference
    {
        get => sourceQuoteRef;
        set
        {
            if (value == sourceQuoteRef) return;
            IsSourceQuoteReferenceUpdated = true;
            sourceQuoteRef                = value;
        }
    }

    public bool IsSourceQuoteReferenceUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag;

            else if (IsSourceQuoteReferenceUpdated)
                UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag;
        }
    }

    public DateTime ValueDate
    {
        get => valueDate;
        set
        {
            if (valueDate == value) return;
            IsValueDateUpdated = true;
            valueDate          = value;
        }
    }

    public bool IsValueDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ValueDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValueDateUpdatedFlag;

            else if (IsValueDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValueDateUpdatedFlag;
        }
    }

    public IPQRecentlyTraded? RecentlyTraded
    {
        get => recentlyTraded;
        set => recentlyTraded = value as PQRecentlyTraded;
    }

    IMutableRecentlyTraded? IMutableLevel3Quote.RecentlyTraded
    {
        get => RecentlyTraded;
        set => RecentlyTraded = value as IPQRecentlyTraded;
    }

    IRecentlyTraded? ILevel3Quote.RecentlyTraded => RecentlyTraded;

    public override bool HasUpdates
    {
        get =>
            base.HasUpdates || (recentlyTraded?.HasUpdates ?? false) || IsBatchIdUpdated ||
            IsSourceQuoteReferenceUpdated || IsValueDateUpdated;
        set
        {
            base.HasUpdates = IsBatchIdUpdated                    = IsSourceQuoteReferenceUpdated = IsValueDateUpdated = value;
            if (recentlyTraded != null) recentlyTraded.HasUpdates = value;
        }
    }

    public override void ResetFields()
    {
        recentlyTraded?.StateReset();

        BatchId   = SourceQuoteReference = 0;
        ValueDate = DateTimeConstants.UnixEpoch;
        base.ResetFields();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        quotePublicationPrecisionSetting = quotePublicationPrecisionSetting ?? PQSourceTickerQuoteInfo;

        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime,
                                                               messageFlags, quotePublicationPrecisionSetting))
            yield return updatedField;
        if (recentlyTraded != null)
            foreach (var recentlyTradedFields in recentlyTraded.GetDeltaUpdateFields(snapShotTime,
                                                                                     messageFlags, quotePublicationPrecisionSetting))
                yield return recentlyTradedFields;
        if (!updatedOnly || IsBatchIdUpdated) yield return new PQFieldUpdate(PQFieldKeys.BatchId, BatchId);

        if (!updatedOnly || IsSourceQuoteReferenceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.SourceQuoteReference, SourceQuoteReference);
        if (!updatedOnly || IsValueDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.ValueDate, valueDate.GetHoursFromUnixEpoch());
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQFieldKeys.LastTraderDictionaryUpsertCommand) return (int)pqFieldUpdate.Value;
        if (pqFieldUpdate.Id >= PQFieldKeys.LastTradedRangeStart
         && pqFieldUpdate.Id <= PQFieldKeys.LastTradedRangeEnd)
            return recentlyTraded!.UpdateField(pqFieldUpdate);
        if (pqFieldUpdate.Id == PQFieldKeys.BatchId)
        {
            BatchId = pqFieldUpdate.Value;
            return 0;
        }

        if (pqFieldUpdate.Id == PQFieldKeys.SourceQuoteReference)
        {
            SourceQuoteReference = pqFieldUpdate.Value;
            return 0;
        }

        if (pqFieldUpdate.Id == PQFieldKeys.ValueDate)
        {
            PQFieldConverters.UpdateHoursFromUnixEpoch(ref valueDate, pqFieldUpdate.Value);
            IsValueDateUpdated = true;
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, messageFlags))
            yield return pqFieldStringUpdate;
        if (recentlyTraded != null)
            foreach (var stringUpdate in recentlyTraded.GetStringUpdates(snapShotTime, messageFlags))
                yield return stringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        var found = base.UpdateFieldString(stringUpdate);
        if (found) return true;
        if (stringUpdate.Field.Id == PQFieldKeys.LastTraderDictionaryUpsertCommand)
        {
            if (recentlyTraded == null) recentlyTraded = new PQRecentlyTraded();
            return recentlyTraded.UpdateFieldString(stringUpdate);
        }

        return false;
    }

    public override ILevel0Quote CopyFrom
    (ILevel0Quote source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var l3Q   = source as ILevel3Quote;
        var pql3Q = source as PQLevel3Quote;
        if (pql3Q == null && l3Q != null)
        {
            // Only copy if changed
            if (recentlyTraded == null && l3Q.RecentlyTraded != null)
                recentlyTraded = new PQRecentlyTraded(l3Q.RecentlyTraded);
            else if (l3Q.RecentlyTraded != null)
                recentlyTraded?.CopyFrom(l3Q.RecentlyTraded, copyMergeFlags);
            BatchId              = l3Q.BatchId;
            SourceQuoteReference = l3Q.SourceQuoteReference;
            ValueDate            = l3Q.ValueDate;
        }
        else if (pql3Q != null)
        {
            // Only copy if changed
            if (recentlyTraded == null && pql3Q.RecentlyTraded != null)
                recentlyTraded = pql3Q.RecentlyTraded.Clone();
            else if (pql3Q.RecentlyTraded != null)
                recentlyTraded?.CopyFrom(pql3Q.RecentlyTraded, copyMergeFlags);

            var hasFullReplace = copyMergeFlags.HasFullReplace();

            if (pql3Q.IsBatchIdUpdated || hasFullReplace) BatchId                           = pql3Q.batchId;
            if (pql3Q.IsSourceQuoteReferenceUpdated || hasFullReplace) SourceQuoteReference = pql3Q.sourceQuoteRef;
            if (pql3Q.IsValueDateUpdated || hasFullReplace) ValueDate                       = pql3Q.ValueDate;
            // ensure flags still match source

            if (hasFullReplace) UpdatedFlags = pql3Q.UpdatedFlags;
        }

        return this;
    }

    public override void EnsureRelatedItemsAreConfigured(ILevel0Quote? quote)
    {
        base.EnsureRelatedItemsAreConfigured(quote);
        if (quote is IPQLevel3Quote pqLevel3Quote)
            recentlyTraded?.EnsureRelatedItemsAreConfigured(pqLevel3Quote.RecentlyTraded?.NameIdLookup);
    }

    public DateTime StorageTime(IStorageTimeResolver<ILevel3Quote>? resolver = null)
    {
        resolver ??= QuoteStorageTimeResolver.Instance;
        return resolver.ResolveStorageTime(this);
    }

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => (ILevel3Quote)Clone();

    ILevel3Quote ILevel3Quote.Clone() => (ILevel3Quote)Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => (IMutableLevel3Quote)Clone();

    IPQLevel3Quote IPQLevel3Quote.Clone() => (IPQLevel3Quote)Clone();

    public override IPQLevel0Quote Clone() =>
        (IPQLevel0Quote?)Recycler?.Borrow<PQLevel3Quote>().CopyFrom(this, CopyMergeFlags.FullReplace)
     ?? new PQLevel3Quote(this);

    public override bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
    {
        if (!(other is ILevel3Quote otherL3)) return false;
        var baseSame = base.AreEquivalent(otherL3, exactTypes);
        var recentlyTradedSame = recentlyTraded?.AreEquivalent(otherL3.RecentlyTraded, exactTypes)
                              ?? otherL3.RecentlyTraded == null;
        var batchIdSame          = batchId == otherL3.BatchId;
        var sourceSequenceIdSame = sourceQuoteRef == otherL3.SourceQuoteReference;
        var valueDateSame        = ValueDate == otherL3.ValueDate;
        var allAreSame           = baseSame && recentlyTradedSame && batchIdSame && sourceSequenceIdSame && valueDateSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel0Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (recentlyTraded != null ? recentlyTraded.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)batchId;
            hashCode = (hashCode * 397) ^ (int)sourceQuoteRef;
            hashCode = (hashCode * 397) ^ valueDate.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({Level3ToStringMembers})";
}
