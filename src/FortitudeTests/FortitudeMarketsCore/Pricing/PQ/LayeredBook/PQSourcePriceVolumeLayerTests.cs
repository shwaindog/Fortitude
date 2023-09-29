using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;
using FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook
{
    [TestClass]
    public class PQSourcePriceVolumeLayerTests
    {
        private IPQNameIdLookupGenerator emptyNameIdLookup;
        private IPQNameIdLookupGenerator nameIdLookup;
        private PQSourcePriceVolumeLayer emptyPvl;
        private PQSourcePriceVolumeLayer populatedPvl;
        private DateTime testDateTime;
        private string wellKnownSourceName;

        [TestInitialize]
        public void SetUp()
        {
            wellKnownSourceName = "TestSourceName";
            emptyNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
            nameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand, 
                PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
            emptyPvl = new PQSourcePriceVolumeLayer(0m, 0m, emptyNameIdLookup.Clone());
            testDateTime = new DateTime(2017, 12, 17, 18, 54, 52);
            populatedPvl = new PQSourcePriceVolumeLayer(4.2949_672m, 42_949_672m, nameIdLookup,
                wellKnownSourceName, true);
        }

        [TestMethod]
        public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
        {
            var newPvl = new PQSourcePriceVolumeLayer(20, 40_000_000, nameIdLookup, wellKnownSourceName, true);
            Assert.AreEqual(20m, newPvl.Price);
            Assert.AreEqual(40_000_000m, newPvl.Volume);
            Assert.AreEqual(wellKnownSourceName, newPvl.SourceName);
            Assert.IsTrue(newPvl.Executable);
            Assert.IsTrue(newPvl.IsPriceUpdated);
            Assert.IsTrue(newPvl.IsVolumeUpdated);
            Assert.IsTrue(newPvl.IsSourceNameUpdated);
            Assert.IsTrue(newPvl.IsExecutableUpdated);

            Assert.AreEqual(0, emptyPvl.Price);
            Assert.AreEqual(0, emptyPvl.Volume);
            Assert.AreEqual(null, emptyPvl.SourceName);
            Assert.IsFalse(emptyPvl.Executable);
            Assert.IsFalse(emptyPvl.IsPriceUpdated);
            Assert.IsFalse(emptyPvl.IsVolumeUpdated);
            Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
            Assert.IsFalse(emptyPvl.IsExecutableUpdated);
        }

        [TestMethod]
        public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
        {
            var newPopulatedPvl = new PQSourcePriceVolumeLayer(20, 40_000_000, nameIdLookup, wellKnownSourceName, true);
            var fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl);
            Assert.AreEqual(20m, fromPQInstance.Price);
            Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
            Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
            Assert.IsTrue(fromPQInstance.Executable);
            Assert.IsTrue(fromPQInstance.IsPriceUpdated);
            Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
            Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
            Assert.IsTrue(fromPQInstance.IsExecutableUpdated);

            var nonPQValueDatePvl = new SourcePriceVolumeLayer(1.23456m, 5_123_456m, wellKnownSourceName, true);
            var fromNonPqInstance = new PQSourcePriceVolumeLayer(nonPQValueDatePvl);
            Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
            Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
            Assert.AreEqual(wellKnownSourceName, fromNonPqInstance.SourceName);
            Assert.IsTrue(fromNonPqInstance.Executable);
            Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
            Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
            Assert.IsTrue(fromNonPqInstance.IsSourceNameUpdated);
            Assert.IsTrue(fromNonPqInstance.IsExecutableUpdated);

            var fromNonSourcePriceVolumeLayer = new PQSourcePriceVolumeLayer(new PriceVolumeLayer(20, 40_000_000));
            Assert.AreEqual(20, fromNonSourcePriceVolumeLayer.Price);
            Assert.AreEqual(40_000_000, fromNonSourcePriceVolumeLayer.Volume);
            Assert.AreEqual(null, fromNonSourcePriceVolumeLayer.SourceName);
            Assert.IsFalse(fromNonSourcePriceVolumeLayer.Executable);
            Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsPriceUpdated);
            Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsVolumeUpdated);
            Assert.IsFalse(fromNonSourcePriceVolumeLayer.IsSourceNameUpdated);
            Assert.IsFalse(fromNonSourcePriceVolumeLayer.IsExecutableUpdated);

            var newEmptyPvl = new PQSourcePriceVolumeLayer(emptyPvl);
            Assert.AreEqual(0, newEmptyPvl.Price);
            Assert.AreEqual(0, newEmptyPvl.Volume);
            Assert.AreEqual(null, newEmptyPvl.SourceName);
            Assert.IsFalse(newEmptyPvl.Executable);
            Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
            Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
            Assert.IsFalse(newEmptyPvl.IsSourceNameUpdated);
            Assert.IsFalse(newEmptyPvl.IsExecutableUpdated);
        }

        [TestMethod]
        public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
        {
            var newPopulatedPvl = new PQSourcePriceVolumeLayer(20, 40_000_000, nameIdLookup, wellKnownSourceName, true)
            {
                IsPriceUpdated = false
            };
            var fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl);
            Assert.AreEqual(20m, fromPQInstance.Price);
            Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
            Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
            Assert.IsTrue(fromPQInstance.Executable);
            Assert.IsFalse(fromPQInstance.IsPriceUpdated);
            Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
            Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
            Assert.IsTrue(fromPQInstance.IsExecutableUpdated);

            newPopulatedPvl = new PQSourcePriceVolumeLayer(20, 40_000_000, nameIdLookup, wellKnownSourceName, true)
            {
                IsVolumeUpdated = false
            };
            fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl);
            Assert.AreEqual(20m, fromPQInstance.Price);
            Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
            Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
            Assert.IsTrue(fromPQInstance.Executable);
            Assert.IsTrue(fromPQInstance.IsPriceUpdated);
            Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
            Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
            Assert.IsTrue(fromPQInstance.IsExecutableUpdated);

            newPopulatedPvl = new PQSourcePriceVolumeLayer(20, 40_000_000, nameIdLookup, wellKnownSourceName, true)
            {
                IsSourceNameUpdated = false
            };
            fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl);
            Assert.AreEqual(20m, fromPQInstance.Price);
            Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
            Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
            Assert.IsTrue(fromPQInstance.Executable);
            Assert.IsTrue(fromPQInstance.IsPriceUpdated);
            Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
            Assert.IsFalse(fromPQInstance.IsSourceNameUpdated);
            Assert.IsTrue(fromPQInstance.IsExecutableUpdated);

            newPopulatedPvl = new PQSourcePriceVolumeLayer(20, 40_000_000, nameIdLookup, wellKnownSourceName, true)
            {
                IsExecutableUpdated = false
            };
            fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl);
            Assert.AreEqual(20m, fromPQInstance.Price);
            Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
            Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
            Assert.IsTrue(fromPQInstance.Executable);
            Assert.IsTrue(fromPQInstance.IsPriceUpdated);
            Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
            Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
            Assert.IsFalse(fromPQInstance.IsExecutableUpdated);
        }

        [TestMethod]
        public void PopulatedPvl_HasUpdatesSetFalse_LookupAndLayerHaveNoUpdates()
        {
            Assert.IsTrue(populatedPvl.HasUpdates);
            Assert.IsTrue(populatedPvl.SourceNameIdLookup.HasUpdates);

            populatedPvl.HasUpdates = false;

            Assert.IsFalse(populatedPvl.HasUpdates);
            Assert.IsFalse(populatedPvl.SourceNameIdLookup.HasUpdates);
        }

        [TestMethod]
        public void EmptyPvl_SourceNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
        {
            Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
            Assert.IsFalse(emptyPvl.HasUpdates);
            Assert.AreEqual(null, emptyPvl.SourceName);
            Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());
            
            emptyPvl.SourceName = wellKnownSourceName;
            Assert.IsTrue(emptyPvl.IsSourceNameUpdated);
            Assert.AreEqual(emptyNameIdLookup[wellKnownSourceName], emptyPvl.SourceId);
            Assert.IsTrue(emptyPvl.HasUpdates);
            Assert.AreEqual(wellKnownSourceName, emptyPvl.SourceName);
            var sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(1, sourceUpdates.Count);

            PQFieldUpdate expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset,
                emptyPvl.SourceId);
            Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

            emptyPvl.IsSourceNameUpdated = false;
            Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
            Assert.IsTrue(emptyPvl.HasUpdates);
            emptyPvl.SourceNameIdLookup.HasUpdates = false;
            Assert.IsFalse(emptyPvl.HasUpdates);
            Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            string nextExpectedSourceName = "AnotherSourceName";
            emptyPvl.SourceName = nextExpectedSourceName;
            Assert.IsTrue(emptyPvl.IsSourceNameUpdated);
            Assert.IsTrue(emptyPvl.HasUpdates);
            Assert.AreEqual(nextExpectedSourceName, emptyPvl.SourceName);
            sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(1, sourceUpdates.Count);
            var stringUpdates = emptyPvl.GetStringUpdates(testDateTime, UpdateStyle.Updates)
                .ToList();
            Assert.AreEqual(1, stringUpdates.Count);
            expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset,
                emptyPvl.SourceId);
            Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
            var expectedStringUpdates = new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(
                    PQFieldKeys.LayerNameDictionaryUpsertCommand, 0u, 1 | PQFieldFlags.IsUpdate),
                StringUpdate = new PQStringUpdate
                {
                    Command = CrudCommand.Update,
                    DictionaryId = emptyPvl.SourceNameIdLookup[emptyPvl.SourceName],
                    Value = emptyPvl.SourceName
                }
            };
            Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

            sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
                             where update.Id == PQFieldKeys.LayerSourceIdOffset
                             select update).ToList();
            Assert.AreEqual(1, sourceUpdates.Count);
            Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
            
            var newEmptyNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
            var newEmpty = new PQSourcePriceVolumeLayer(0m, 0m, newEmptyNameIdLookup);
            newEmpty.UpdateField(sourceUpdates[0]);
            newEmpty.UpdateFieldString(stringUpdates[0]);
            Assert.AreEqual(nextExpectedSourceName, newEmpty.SourceName);
            Assert.IsTrue(newEmpty.IsSourceNameUpdated);
        }

        [TestMethod]
        public void EmptyPvl_LayerExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
        {
            Assert.IsFalse(emptyPvl.IsExecutableUpdated);
            Assert.IsFalse(emptyPvl.HasUpdates);
            Assert.IsFalse(emptyPvl.Executable);
            Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

            emptyPvl.Executable = true;
            Assert.IsTrue(emptyPvl.IsExecutableUpdated);
            Assert.IsTrue(emptyPvl.HasUpdates);
            Assert.IsTrue(emptyPvl.Executable);
            var sourceLayerUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(1, sourceLayerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerBooleanFlagsOffset,
                PQFieldFlags.LayerExecutableFlag);
            Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

            emptyPvl.IsExecutableUpdated = false;
            Assert.IsFalse(emptyPvl.HasUpdates);
            Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            emptyPvl.IsExecutableUpdated = true;
            sourceLayerUpdates =
            (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                where update.Id == PQFieldKeys.LayerBooleanFlagsOffset
                select update).ToList();
            Assert.AreEqual(1, sourceLayerUpdates.Count);
            Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

            var newEmpty = new PQSourcePriceVolumeLayer();
            newEmpty.UpdateField(sourceLayerUpdates[0]);
            Assert.IsTrue(newEmpty.Executable);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(newEmpty.IsExecutableUpdated);
        }

        [TestMethod]
        public void EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected()
        {
            Assert.IsFalse(populatedPvl.IsEmpty);
            Assert.IsTrue(emptyPvl.IsEmpty);
        }

        [TestMethod]
        public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
        {
            Assert.IsFalse(populatedPvl.IsEmpty);
            Assert.AreNotEqual(0m, populatedPvl.Price);
            Assert.AreNotEqual(0m, populatedPvl.Volume);
            Assert.AreNotEqual(null, populatedPvl.SourceName);
            Assert.IsTrue(populatedPvl.Executable);
            Assert.IsTrue(populatedPvl.IsPriceUpdated);
            Assert.IsTrue(populatedPvl.IsVolumeUpdated);
            Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
            Assert.IsTrue(populatedPvl.IsExecutableUpdated);
            populatedPvl.Reset();
            Assert.IsTrue(populatedPvl.IsEmpty);
            Assert.AreEqual(0m, populatedPvl.Price);
            Assert.AreEqual(0m, populatedPvl.Volume);
            Assert.AreEqual(null, populatedPvl.SourceName);
            Assert.IsFalse(populatedPvl.Executable);
            Assert.IsFalse(populatedPvl.IsPriceUpdated);
            Assert.IsFalse(populatedPvl.IsVolumeUpdated);
            Assert.IsFalse(populatedPvl.IsSourceNameUpdated);
            Assert.IsFalse(populatedPvl.IsExecutableUpdated);
        }

        [TestMethod]
        public void PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
        {
            var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
                new DateTime(2017, 12, 17, 12, 33, 1), UpdateStyle.Updates).ToList();
            AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
        }

        [TestMethod]
        public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
        {
            populatedPvl.HasUpdates = false;
            var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
                new DateTime(2017, 12, 17, 12, 33, 1), UpdateStyle.FullSnapshot).ToList();
            AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
        }

        [TestMethod]
        public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
        {
            populatedPvl.HasUpdates = false;
            var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            var pqStringUpdates = populatedPvl.GetStringUpdates(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            Assert.AreEqual(0, pqStringUpdates.Count);
        }

        [TestMethod]
        public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl()
        {
            var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 13, 33, 3), UpdateStyle.Updates | UpdateStyle.Replay).ToList();
            var pqStringUpdates = populatedPvl.GetStringUpdates(
                new DateTime(2017, 11, 04, 13, 33, 3), UpdateStyle.Updates | UpdateStyle.Replay).ToList();
            var newEmpty = new PQSourcePriceVolumeLayer(0m, 0m, emptyNameIdLookup);
            foreach (var pqFieldUpdate in pqFieldUpdates)
            {
                newEmpty.UpdateField(pqFieldUpdate);
            }
            foreach (var pqStringUpdate in pqStringUpdates)
            {
                newEmpty.UpdateFieldString(pqStringUpdate);
            }
            Assert.AreEqual(populatedPvl, newEmpty);
        }

        [TestMethod]
        public void FullyPopulatedPvl_CopyFromToEmptyQuote_PvlsEqualEachOther()
        {
            var nonPQPriceVolume = new SourcePriceVolumeLayer(populatedPvl);
            emptyPvl.CopyFrom(nonPQPriceVolume);
            Assert.AreEqual(populatedPvl, emptyPvl);
        }

        [TestMethod]
        public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
        {
            var emptyPriceVolumeLayer = new PQSourcePriceVolumeLayer(0m, 0m, emptyNameIdLookup);
            populatedPvl.HasUpdates = false;
            emptyPriceVolumeLayer.CopyFrom(populatedPvl);
            Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
            Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
            Assert.AreEqual(null, emptyPriceVolumeLayer.SourceName);
            Assert.IsFalse(emptyPriceVolumeLayer.Executable);
            Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
            Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
            Assert.IsFalse(emptyPriceVolumeLayer.IsSourceNameUpdated);
            Assert.IsFalse(emptyPriceVolumeLayer.IsExecutableUpdated);
        }
        
        [TestMethod]
        public void EmptyPvl_EnsureRelatedItemsAreConfigured_SetsSourceNameIdLookupWhenNullOrSameAsInfo()
        {
            Assert.AreEqual(1, nameIdLookup.Count);

            Mock<IPQSourceTickerQuoteInfo> moqSrcTkrQuoteInfo = new Mock<IPQSourceTickerQuoteInfo>();
            moqSrcTkrQuoteInfo.SetupGet(stqi => stqi.SourceNameIdLookup).Returns(emptyNameIdLookup)
                .Verifiable();

            var newEmpty = new PQSourcePriceVolumeLayer(0m, 0m, emptyNameIdLookup);
            Assert.AreEqual(0, newEmpty.SourceNameIdLookup.Count);
            Assert.AreSame(emptyNameIdLookup, newEmpty.SourceNameIdLookup);

            newEmpty.EnsureRelatedItemsAreConfigured(moqSrcTkrQuoteInfo.Object);

            moqSrcTkrQuoteInfo.Verify();
            Assert.AreNotSame(emptyNameIdLookup, newEmpty.SourceNameIdLookup);
        }

        [TestMethod]
        public void EmptyPvlMissingLookup_EnsureRelatedItemsAreConfigured_SetsTraderNameIdLookupWhenNullOrSameAsInfo()
        {
            Assert.AreEqual(1, nameIdLookup.Count);

            Mock<IPQSourceTickerQuoteInfo> moqSrcTkrQuoteInfo = new Mock<IPQSourceTickerQuoteInfo>();
            moqSrcTkrQuoteInfo.SetupGet(stqi => stqi.SourceNameIdLookup).Returns(emptyNameIdLookup)
                .Verifiable();

            var newEmpty = new PQSourcePriceVolumeLayer { SourceNameIdLookup = null };

            newEmpty.EnsureRelatedItemsAreConfigured(moqSrcTkrQuoteInfo.Object);

            moqSrcTkrQuoteInfo.Verify();
            Assert.IsNotNull(newEmpty.SourceNameIdLookup);
            Assert.AreEqual(0, newEmpty.SourceNameIdLookup.Count);
            Assert.AreNotSame(emptyNameIdLookup, newEmpty.SourceNameIdLookup);
        }

        [TestMethod]
        public void EmptyPvl_EnsureRelatedItemsAreConfigured_SharesTraderNameIdLookupBetweenLayers()
        {
            var newEmpty = new PQSourcePriceVolumeLayer(0m, 0m, emptyNameIdLookup);
            Assert.AreEqual(0, newEmpty.SourceNameIdLookup.Count);
            Assert.AreSame(emptyNameIdLookup, newEmpty.SourceNameIdLookup);

            newEmpty.EnsureRelatedItemsAreConfigured(populatedPvl);

            Assert.AreSame(populatedPvl.SourceNameIdLookup, newEmpty.SourceNameIdLookup);

            newEmpty.EnsureRelatedItemsAreConfigured(new PQPriceVolumeLayer());
        }

        [TestMethod]
        public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
        {
            var clonedQuote = ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone();
            Assert.AreNotSame(clonedQuote, populatedPvl);
            Assert.AreEqual(populatedPvl, clonedQuote);

            var cloned2 = (PQSourcePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
            Assert.AreNotSame(cloned2, populatedPvl);
            Assert.AreEqual(populatedPvl, cloned2);
        }

        [TestMethod]
        public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
        {
            var fullyPopulatedClone = (PQSourcePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedPvl,
                fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedPvl,
                fullyPopulatedClone);
        }

        [TestMethod]
        public void FullyPopulatedPvlSameObj_Equals_ReturnsTrue()
        {
            Assert.AreEqual(populatedPvl, populatedPvl);
            Assert.AreEqual(populatedPvl, ((ICloneable)populatedPvl).Clone());
            Assert.AreEqual(populatedPvl, ((IMutableSourcePriceVolumeLayer)populatedPvl).Clone());
            Assert.AreEqual(populatedPvl, ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone());
            Assert.AreEqual(populatedPvl, ((IPQSourcePriceVolumeLayer)populatedPvl).Clone());
        }

        [TestMethod]
        public void FullyPopulatedPvl_GetHashCode_ReturnNumberNoException()
        {
            var hashCode = populatedPvl.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }

        [TestMethod]
        public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
        {
            var toString = populatedPvl.ToString();

            Assert.IsTrue(toString.Contains(populatedPvl.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Price)}: {populatedPvl.Price:N5}"));
            Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Volume)}: {populatedPvl.Volume:N2}"));
            Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.SourceName)}: {populatedPvl.SourceName}"));
            Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Executable)}: {populatedPvl.Executable}"));
        }

        public static void AssertContainsAllPvlFields(IList<PQFieldUpdate> checkFieldUpdates,
            PQSourcePriceVolumeLayer pvl)
        {
            PQPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl);

            Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset, pvl.SourceId),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    PQFieldKeys.LayerSourceIdOffset), $"For {pvl.GetType().Name} ");

            Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerBooleanFlagsOffset, 
                pvl.Executable ? PQFieldFlags.LayerExecutableFlag : 0),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    PQFieldKeys.LayerBooleanFlagsOffset), $"For {pvl.GetType().Name} ");
        }

        public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
            IPQSourcePriceVolumeLayer original, IPQSourcePriceVolumeLayer changingPriceVolumeLayer,
            IOrderBook originalOrderBook = null,
            IOrderBook changingOrderBook = null,
            ILevel2Quote originalQuote = null,
            ILevel2Quote changingQuote = null)
        {
            if (original == null && changingPriceVolumeLayer == null) return;
            Assert.IsNotNull(original);
            Assert.IsNotNull(changingPriceVolumeLayer);
            
            PQPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
                original, changingPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);
            
            if (original.GetType() == typeof(PQSourcePriceVolumeLayer))
            {
                Assert.AreEqual(!exactComparison,
                    changingPriceVolumeLayer.AreEquivalent(new SourcePriceVolumeLayer(original), exactComparison));
            }

            SourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
                original, changingPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);
            
            changingPriceVolumeLayer.IsSourceNameUpdated = !changingPriceVolumeLayer.IsSourceNameUpdated;
            Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (originalOrderBook != null) Assert.AreEqual(!exactComparison,
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalQuote != null) Assert.AreEqual(!exactComparison,
                originalQuote.AreEquivalent(changingQuote, exactComparison));
            changingPriceVolumeLayer.IsSourceNameUpdated = original.IsSourceNameUpdated;
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (originalOrderBook != null) Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

            changingPriceVolumeLayer.IsExecutableUpdated = !changingPriceVolumeLayer.IsExecutableUpdated;
            Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (originalOrderBook != null) Assert.AreEqual(!exactComparison,
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalQuote != null) Assert.AreEqual(!exactComparison,
                originalQuote.AreEquivalent(changingQuote, exactComparison));
            changingPriceVolumeLayer.IsExecutableUpdated = original.IsExecutableUpdated;
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (originalOrderBook != null) Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
        }
    }
}