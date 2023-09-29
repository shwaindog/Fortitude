using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo
{
    [TestClass]
    public class PQUniqueSourceTickerIdentifierTests
    {
        private PQUniqueSourceTickerIdentifier fullyPopulatedId;
        private PQUniqueSourceTickerIdentifier copyFullyPopulated;
        private PQUniqueSourceTickerIdentifier emptyId;
        private DateTime testDateTime;

        [TestInitialize]
        public void SetUp()
        {
            fullyPopulatedId = new PQUniqueSourceTickerIdentifier(uint.MaxValue, "TestSource", "TestTicker");
            copyFullyPopulated = new PQUniqueSourceTickerIdentifier("TestSource", "TestTicker", 
                ushort.MaxValue, ushort.MaxValue);
            emptyId = new PQUniqueSourceTickerIdentifier(0, null, null);

            testDateTime = new DateTime(2017, 11, 07, 18, 33, 24);
        }

        [TestMethod]
        public void EmptyQuoteInfo_IdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
        {
            Assert.IsFalse(emptyId.IsIdUpdated);
            Assert.IsFalse(emptyId.HasUpdates);
            Assert.AreEqual(0u, emptyId.Id);
            Assert.IsTrue(emptyId.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            var expectedId = uint.MaxValue;
            emptyId.Id = expectedId;
            Assert.IsTrue(emptyId.IsIdUpdated);
            Assert.IsTrue(emptyId.HasUpdates);
            Assert.AreEqual(expectedId, emptyId.Id);
            var sourceUpdates = emptyId.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(1, sourceUpdates.Count);
            PQFieldUpdate expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.SourceTickerId, expectedId);
            Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

            emptyId.IsIdUpdated = false;
            Assert.IsFalse(emptyId.IsIdUpdated);
            Assert.IsFalse(emptyId.HasUpdates);
            Assert.IsTrue(emptyId.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            sourceUpdates = (from update in emptyId.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
                             where update.Id == PQFieldKeys.SourceTickerId
                             select update).ToList();
            Assert.AreEqual(1, sourceUpdates.Count);
            Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

            var newEmpty = new PQUniqueSourceTickerIdentifier(emptyId);
            newEmpty.UpdateField(sourceUpdates[0]);
            Assert.AreEqual(expectedId, newEmpty.Id);
            Assert.IsTrue(newEmpty.IsIdUpdated);
        }

        [TestMethod]
        public void EmptyQuoteInfo_SourceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
        {
            Assert.IsFalse(emptyId.IsSourceUpdated);
            Assert.IsFalse(emptyId.HasUpdates);
            Assert.AreEqual(null, emptyId.Source);
            Assert.IsTrue(emptyId.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            var expectedSource = "NewSourceName";
            emptyId.Source = expectedSource;
            Assert.IsTrue(emptyId.IsSourceUpdated);
            Assert.IsTrue(emptyId.HasUpdates);
            Assert.AreEqual(expectedSource, emptyId.Source);
            var sourceUpdates = emptyId.GetStringUpdates(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(1, sourceUpdates.Count);
            PQFieldStringUpdate expectedFieldUpdate = ExpectedSourceStringUpdate(expectedSource);
            Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

            emptyId.IsSourceUpdated = false;
            Assert.IsFalse(emptyId.IsSourceUpdated);
            Assert.IsFalse(emptyId.HasUpdates);
            Assert.IsTrue(emptyId.GetStringUpdates(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            sourceUpdates = (from update in emptyId.GetStringUpdates(testDateTime, UpdateStyle.FullSnapshot)
                             where update.Field.Id == PQFieldKeys.SourceTickerNames && update.StringUpdate.DictionaryId == 0
                             select update).ToList();
            Assert.AreEqual(1, sourceUpdates.Count);
            Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

            var newEmpty = new PQUniqueSourceTickerIdentifier(emptyId);
            newEmpty.UpdateFieldString(sourceUpdates[0]);
            Assert.AreEqual(expectedSource, newEmpty.Source);
            Assert.IsTrue(newEmpty.IsSourceUpdated);
        }

        [TestMethod]
        public void EmptyQuoteInfo_TickerChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
        {
            Assert.IsFalse(emptyId.IsTickerUpdated);
            Assert.IsFalse(emptyId.HasUpdates);
            Assert.AreEqual(null, emptyId.Ticker);
            Assert.IsTrue(emptyId.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            var expectedTicker = "NewTickerName";
            emptyId.Ticker = expectedTicker;
            Assert.IsTrue(emptyId.IsTickerUpdated);
            Assert.IsTrue(emptyId.HasUpdates);
            Assert.AreEqual(expectedTicker, emptyId.Ticker);
            var tickerUpdates = emptyId.GetStringUpdates(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(1, tickerUpdates.Count);
            PQFieldStringUpdate expectedFieldUpdate = ExpectedTickerStringUpdate(expectedTicker);
            Assert.AreEqual(expectedFieldUpdate, tickerUpdates[0]);

            emptyId.IsTickerUpdated = false;
            Assert.IsFalse(emptyId.IsTickerUpdated);
            Assert.IsFalse(emptyId.HasUpdates);
            Assert.IsTrue(emptyId.GetStringUpdates(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            tickerUpdates = (from update in emptyId.GetStringUpdates(testDateTime, UpdateStyle.FullSnapshot)
                where update.Field.Id == PQFieldKeys.SourceTickerNames && update.StringUpdate.DictionaryId == 1
                select update).ToList();
            Assert.AreEqual(1, tickerUpdates.Count);
            Assert.AreEqual(expectedFieldUpdate, tickerUpdates[0]);

            var newEmpty = new PQUniqueSourceTickerIdentifier(emptyId);
            newEmpty.UpdateFieldString(tickerUpdates[0]);
            Assert.AreEqual(expectedTicker, newEmpty.Ticker);
            Assert.IsTrue(newEmpty.IsTickerUpdated);
        }


        [TestMethod]
        public void PopulatedQuoteInfo_GetDeltaUpdateFieldsAsUpdate_ReturnsAllQuoteInfoFields()
        {
            var pqFieldUpdates = fullyPopulatedId.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            AssertSourceTickerInfoContainsAllFields(pqFieldUpdates, fullyPopulatedId);
            var pqStringFieldUpdates = fullyPopulatedId.GetStringUpdates(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            Assert.AreEqual(2, pqStringFieldUpdates.Count);
        }

        [TestMethod]
        public void PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllQuoteInfoFields()
        {
            fullyPopulatedId.HasUpdates = false;
            var pqFieldUpdates = fullyPopulatedId.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.FullSnapshot).ToList();
            AssertSourceTickerInfoContainsAllFields(pqFieldUpdates, fullyPopulatedId);
            var pqStringFieldUpdates = fullyPopulatedId.GetStringUpdates(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.FullSnapshot).ToList();
            Assert.AreEqual(2, pqStringFieldUpdates.Count);
        }

        [TestMethod]
        public void PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
        {
            fullyPopulatedId.HasUpdates = false;
            var pqFieldUpdates = fullyPopulatedId.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            var pqStringFieldUpdates = fullyPopulatedId.GetStringUpdates(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            Assert.AreEqual(0, pqStringFieldUpdates.Count);
        }

        [TestMethod]
        public void PopulatedQuote_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
        {
            fullyPopulatedId.IsIdUpdated = true;
            fullyPopulatedId.IsSourceUpdated = true;
            fullyPopulatedId.IsTickerUpdated = true;
            var pqFieldUpdates = fullyPopulatedId.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            var newEmpty = new PQUniqueSourceTickerIdentifier(new UniqueSourceTickerIdentifier(0, null, null));
            foreach (var pqFieldUpdate in pqFieldUpdates)
            {
                newEmpty.UpdateField(pqFieldUpdate);
            }
            var stringFieldUpdates = fullyPopulatedId.GetStringUpdates(new DateTime(2017, 11, 04, 16, 33, 59),
                    UpdateStyle.Updates);
            foreach (var stringUpdate in stringFieldUpdates)
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            Assert.AreEqual(fullyPopulatedId, newEmpty);
        }

        [TestMethod]
        public void PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerQuoteInfo()
        {
            var pqFieldUpdates = fullyPopulatedId.GetStringUpdates(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            Assert.AreEqual(ExpectedSourceStringUpdate(
                    fullyPopulatedId.Source),
                PQLevel0QuoteTests.ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFieldKeys.SourceTickerNames, 0));
            Assert.AreEqual(ExpectedTickerStringUpdate(
                    fullyPopulatedId.Ticker),
                PQLevel0QuoteTests.ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFieldKeys.SourceTickerNames, 1));
        }

        [TestMethod]
        public void EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateField_ReturnsSizeFoundInField()
        {
            var expectedSize = 37;
            var pqStringFieldSize = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, expectedSize);
            var sizeToReadFromBuffer = emptyId.UpdateField(pqStringFieldSize);
            Assert.AreEqual(expectedSize, sizeToReadFromBuffer);
        }

        [TestMethod]
        public void EmptyQuoteInfo_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues()
        {
            var expectedNewTicker = "NewTestTickerName";
            var expectedNewSource = "NewTestSourceName";

            var tickerStringUpdate = ExpectedTickerStringUpdate(expectedNewTicker);
            var sourceStringUpdate = ExpectedSourceStringUpdate(expectedNewSource);

            emptyId.UpdateFieldString(tickerStringUpdate);
            Assert.AreEqual(expectedNewTicker, emptyId.Ticker);
            emptyId.UpdateFieldString(sourceStringUpdate);
            Assert.AreEqual(expectedNewSource, emptyId.Source);
        }

        [TestMethod]
        public void FullyPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualEachOther()
        {
            emptyId.CopyFrom(fullyPopulatedId);

            Assert.AreEqual(fullyPopulatedId, emptyId);
        }

        [TestMethod]
        public void NonPQPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualToEachOther()
        {
            var nonPQQuoteInfo = new UniqueSourceTickerIdentifier(fullyPopulatedId);
            emptyId.CopyFrom(nonPQQuoteInfo);
            Assert.IsTrue(fullyPopulatedId.AreEquivalent(emptyId));
        }

        [TestMethod]
        public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
        {
            var cloned = ((ICloneable<IUniqueSourceTickerIdentifier>)fullyPopulatedId).Clone();
            Assert.AreEqual(fullyPopulatedId, cloned);

            var cloned2 = (IUniqueSourceTickerIdentifier)((ICloneable)fullyPopulatedId).Clone();
            Assert.AreEqual(fullyPopulatedId, cloned2);
        }

        [TestMethod]
        public void TwoFullyPopulatedIds_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
        {
            var fullyPopulatedClone = (PQUniqueSourceTickerIdentifier)fullyPopulatedId.Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedId, fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedId, fullyPopulatedClone);
        }

        [TestMethod]
        public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
        {
            Assert.AreEqual(fullyPopulatedId, fullyPopulatedId);
            Assert.AreEqual(fullyPopulatedId, ((ICloneable<IUniqueSourceTickerIdentifier>)fullyPopulatedId).Clone());
        }

        [TestMethod]
        public void EmptyQuote_GetHashCode_ReturnNumberNoException()
        {
            var hashCode = emptyId.GetHashCode();
            Assert.IsTrue(hashCode == 0);

            hashCode = fullyPopulatedId.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }

        public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
            PQUniqueSourceTickerIdentifier original, PQUniqueSourceTickerIdentifier changingSrcTkrId)
        {
            Assert.IsTrue(original.AreEquivalent(changingSrcTkrId));
            Assert.IsTrue(changingSrcTkrId.AreEquivalent(original));

            Assert.IsFalse(changingSrcTkrId.AreEquivalent(null, exactComparison));
            Assert.AreEqual(!exactComparison,
                changingSrcTkrId.AreEquivalent(new UniqueSourceTickerIdentifier(original), exactComparison));

            changingSrcTkrId.Id = uint.MaxValue/2;
            Assert.IsFalse(original.AreEquivalent(changingSrcTkrId, exactComparison));
            changingSrcTkrId.Id = original.Id;
            Assert.IsTrue(changingSrcTkrId.AreEquivalent(original, exactComparison));

            changingSrcTkrId.Source = "ChangedSourceName";
            Assert.IsFalse(changingSrcTkrId.AreEquivalent(original, exactComparison));
            changingSrcTkrId.Source = original.Source;
            Assert.IsTrue(original.AreEquivalent(changingSrcTkrId, exactComparison));

            changingSrcTkrId.Ticker = "ChangedTickerName";
            Assert.IsFalse(original.AreEquivalent(changingSrcTkrId, exactComparison));
            changingSrcTkrId.Ticker = original.Ticker;
            Assert.IsTrue(changingSrcTkrId.AreEquivalent(original, exactComparison));
        }

        public static PQFieldStringUpdate ExpectedSourceStringUpdate(string sourceValue)
        {
            return new PQFieldStringUpdate
            {

                Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpdate),
                StringUpdate = new PQStringUpdate
                {
                    DictionaryId = 0,
                    Value = sourceValue,
                    Command = CrudCommand.Update
                }
            };
        }

        public static PQFieldStringUpdate ExpectedTickerStringUpdate(string tickerValue)
        {
            return new PQFieldStringUpdate
            {

                Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpdate),
                StringUpdate = new PQStringUpdate
                {
                    DictionaryId = 1,
                    Value = tickerValue,
                    Command = CrudCommand.Update
                }
            };
        }

        public static void AssertSourceTickerInfoContainsAllFields(IList<PQFieldUpdate> checkFieldUpdates,
            IUniqueSourceTickerIdentifier srcTkrId)
        {
            Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceTickerId, srcTkrId.Id),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceTickerId));
        }
    }
}