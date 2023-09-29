using System;
using System.Collections.Generic;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.LastTraded;

namespace FortitudeMarketsCore.Pricing.PQ.Quotes
{
    public class PQLevel3Quote : PQLevel2Quote, IPQLevel3Quote
    {

        private IPQRecentlyTraded recentlyTraded;
        private uint batchId;
        private uint sourceQuoteRef;
        private DateTime valueDate = DateTimeConstants.UnixEpoch;

        [Obsolete]
        public PQLevel3Quote()
        {
            throw new NotSupportedException();
        }

        public PQLevel3Quote(ISourceTickerQuoteInfo uniqueSourceTickerIdentifier)
            : base(uniqueSourceTickerIdentifier)
        {
            if(uniqueSourceTickerIdentifier.LastTradedFlags != LastTradedFlags.None)
            {
                recentlyTraded = new PQRecentlyTraded(PQSourceTickerQuoteInfo);
            }
        }

        public PQLevel3Quote(ILevel0Quote toClone) : base(toClone)
        {
            var l3QToClone = toClone as ILevel3Quote;
            if (l3QToClone != null)
            {
                BatchId = l3QToClone.BatchId;
                SourceQuoteReference = l3QToClone.SourceQuoteReference;
                ValueDate = l3QToClone.ValueDate;
            }
            if (toClone is IPQLevel3Quote pql3QToClone)
            {
                if (pql3QToClone.RecentlyTraded != null)
                {
                    recentlyTraded = pql3QToClone.RecentlyTraded.Clone();
                }
            }
            else if(l3QToClone?.RecentlyTraded != null)
            {
                recentlyTraded = new PQRecentlyTraded(l3QToClone.RecentlyTraded);
            }
        }

        public uint BatchId
        {
            get => batchId;
            set
            {
                if (batchId == value) return;
                IsBatchIdUpdated = true;
                batchId = value;
            }
        }

        public bool IsBatchIdUpdated
        {
            get => (UpdatedFlags & QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag) > 0;
            set
            {
                if (value)
                {
                    UpdatedFlags |= QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag;
                }
                else if(IsBatchIdUpdated)
                {
                    UpdatedFlags ^= QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag;
                }
            }
        }

        public uint SourceQuoteReference
        {
            get => sourceQuoteRef;
            set
            {
                if (value == sourceQuoteRef) return;
                IsSourceQuoteReferenceUpdated = true;
                sourceQuoteRef = value;
            }
        }

        public bool IsSourceQuoteReferenceUpdated
        {
            get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag) > 0;
            set
            {
                if (value)
                {
                    UpdatedFlags |= QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag;
                }
                else if(IsSourceQuoteReferenceUpdated)
                {
                    UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag;
                }
            }
        }

        public DateTime ValueDate
        {
            get => valueDate;
            set
            {
                if (valueDate == value) return;
                IsValueDateUpdated = true;
                valueDate = value;
            }
        }

        public bool IsValueDateUpdated
        {
            get => (UpdatedFlags & QuoteFieldUpdatedFlags.ValueDateUpdatedFlag) > 0;
            set
            {
                if (value)
                {
                    UpdatedFlags |= QuoteFieldUpdatedFlags.ValueDateUpdatedFlag;
                }
                else if (IsValueDateUpdated)
                {
                    UpdatedFlags ^= QuoteFieldUpdatedFlags.ValueDateUpdatedFlag;
                }
            }
        }

        public IPQRecentlyTraded RecentlyTraded
        {
            get => recentlyTraded;
            set => recentlyTraded = (PQRecentlyTraded)value;
        }

        IMutableRecentlyTraded IMutableLevel3Quote.RecentlyTraded
        {
            get => RecentlyTraded;
            set => RecentlyTraded = (IPQRecentlyTraded)value;
        }

        IRecentlyTraded ILevel3Quote.RecentlyTraded => RecentlyTraded;

        public override bool HasUpdates
        {
            get => base.HasUpdates || (recentlyTraded?.HasUpdates ?? false) || IsBatchIdUpdated || 
                IsSourceQuoteReferenceUpdated || IsValueDateUpdated;
            set
            {
                base.HasUpdates = IsBatchIdUpdated = IsSourceQuoteReferenceUpdated = IsValueDateUpdated = value;
                if (recentlyTraded != null)
                {
                    recentlyTraded.HasUpdates = value;
                }
            }
        }

        public override void ResetFields()
        {
            recentlyTraded?.Reset();
            BatchId = SourceQuoteReference = 0;
            SourceQuoteReference = 0u;
            ValueDate = DateTimeConstants.UnixEpoch;
            base.ResetFields();
        }

        public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle, 
            IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSetting = null)
        {
            var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
            quotePublicationPrecisionSetting = quotePublicationPrecisionSetting ?? PQSourceTickerQuoteInfo;

            foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime,
                updateStyle, quotePublicationPrecisionSetting))
            {
                yield return updatedField;
            }
            if (recentlyTraded != null)
            {
                foreach (var recentlyTradedFields in recentlyTraded.GetDeltaUpdateFields(snapShotTime,
                    updateStyle, quotePublicationPrecisionSetting))
                {
                    yield return recentlyTradedFields;
                }
            }
            if (!updatedOnly || IsBatchIdUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.BatchId, BatchId);
            }
            if (!updatedOnly || IsSourceQuoteReferenceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.SourceQuoteReference, SourceQuoteReference);
            }
            if (!updatedOnly || IsValueDateUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.ValueDate, valueDate.GetHoursFromUnixEpoch());
            }
        }

        public override int UpdateField(PQFieldUpdate pqFieldUpdate)
        {
            if (pqFieldUpdate.Id == PQFieldKeys.LastTraderDictionaryUpsertCommand)
            {
                return (int)pqFieldUpdate.Value;
            }
            if (pqFieldUpdate.Id >= PQFieldKeys.LastTradedRangeStart 
                && pqFieldUpdate.Id <= PQFieldKeys.LastTradedRangeEnd)
            {
                return recentlyTraded.UpdateField(pqFieldUpdate);
            }
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

        public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, 
            UpdateStyle updatedStyle)
        {
            foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, updatedStyle))
            {
                yield return pqFieldStringUpdate;
            }
            if (recentlyTraded != null)
            {
                foreach (var stringUpdate in recentlyTraded.GetStringUpdates(snapShotTime, updatedStyle))
                {
                    yield return stringUpdate;
                }
            }
        }

        public override bool UpdateFieldString(PQFieldStringUpdate updates)
        {
            var found = base.UpdateFieldString(updates);
            if (found) return true;
            if (updates.Field.Id == PQFieldKeys.LastTraderDictionaryUpsertCommand)
            {
                if (recentlyTraded == null)
                {
                    recentlyTraded = new PQRecentlyTraded(PQSourceTickerQuoteInfo);
                }
                return recentlyTraded.UpdateFieldString(updates);
            }
            return false;
        }

        public override void CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {

            base.CopyFrom(source, copyMergeFlags);
            ILevel3Quote l3Q = source as ILevel3Quote;
            PQLevel3Quote pql3Q = source as PQLevel3Quote;
            if (pql3Q == null && l3Q != null)
            {

                // Only copy if changed
                if (recentlyTraded == null && l3Q.RecentlyTraded != null)
                {
                    recentlyTraded = new PQRecentlyTraded(l3Q.RecentlyTraded);
                }
                else
                {
                    recentlyTraded?.CopyFrom(l3Q.RecentlyTraded);
                }
                BatchId = l3Q.BatchId;
                SourceQuoteReference = l3Q.SourceQuoteReference;
                ValueDate = l3Q.ValueDate;
            }
            else if (pql3Q != null)
            {
                // Only copy if changed
                if (recentlyTraded == null && pql3Q.RecentlyTraded != null)
                {
                    recentlyTraded = pql3Q.RecentlyTraded.Clone();
                }
                else
                {
                    recentlyTraded?.CopyFrom(l3Q.RecentlyTraded);
                }
                if (pql3Q.IsBatchIdUpdated)
                {
                    BatchId = pql3Q.batchId;
                }
                if (pql3Q.IsSourceQuoteReferenceUpdated)
                {
                    SourceQuoteReference = pql3Q.sourceQuoteRef;
                }
                if (pql3Q.IsValueDateUpdated)
                {
                    ValueDate = pql3Q.ValueDate;
                }
                // ensure flags still match source
                UpdatedFlags = pql3Q.UpdatedFlags;
            }
        }
        public override void EnsureRelatedItemsAreConfigured(ILevel0Quote quote)
        {
            base.EnsureRelatedItemsAreConfigured(quote);
            recentlyTraded?.EnsureRelatedItemsAreConfigured(PQSourceTickerQuoteInfo);
        }

        ILevel3Quote ICloneable<ILevel3Quote>.Clone()
        {
            return (ILevel3Quote)Clone();
        }

        ILevel3Quote ILevel3Quote.Clone()
        {
            return (ILevel3Quote)Clone();
        }

        IMutableLevel3Quote IMutableLevel3Quote.Clone()
        {
            return (IMutableLevel3Quote)Clone();
        }

        IPQLevel3Quote IPQLevel3Quote.Clone()
        {
            return (IPQLevel3Quote)Clone();
        }

        public override object Clone()
        {
            return new PQLevel3Quote(this);
        }

        public override bool AreEquivalent(ILevel0Quote other, bool exactTypes = false)
        {
            if (!(other is ILevel3Quote otherL3)) return false;
            var baseSame = base.AreEquivalent(otherL3, exactTypes);
            var recentlyTradedSame = recentlyTraded?.AreEquivalent(otherL3.RecentlyTraded, exactTypes) 
                ?? otherL3.RecentlyTraded == null;
            var batchIdSame = batchId == otherL3.BatchId;
            var sourceSequenceIdSame = sourceQuoteRef == otherL3.SourceQuoteReference;
            var valueDateSame = ValueDate == otherL3.ValueDate;
            return baseSame && recentlyTradedSame && batchIdSame && sourceSequenceIdSame && valueDateSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel0Quote, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (recentlyTraded != null ? recentlyTraded.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) batchId;
                hashCode = (hashCode * 397) ^ (int) sourceQuoteRef;
                hashCode = (hashCode * 397) ^ valueDate.GetHashCode();
                return hashCode;
            }
        }
    }
}
