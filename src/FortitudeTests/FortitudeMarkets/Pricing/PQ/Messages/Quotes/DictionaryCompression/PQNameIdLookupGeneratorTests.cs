// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;

[TestClass]
public class PQNameIdLookupGeneratorTests
{
    private IPQNameIdLookupGenerator firstGeneratorSubKey1 = null!;

    private Dictionary<int, string>  initialValues          = null!;
    private IPQNameIdLookupGenerator secondGeneratorSubKey1 = null!;
    private DateTime                 snapshotTime;

    [TestInitialize]
    public void SetUp()
    {
        initialValues = new Dictionary<int, string>
        {
            { 1, "FirstItem" }, { 2, "SecondItem" }, { 3, "ThirdItem" }
        };

        firstGeneratorSubKey1  = new PQNameIdLookupGenerator(PQQuoteFields.SourceId);
        secondGeneratorSubKey1 = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerNames);

        foreach (var kvp in initialValues)
        {
            firstGeneratorSubKey1.GetOrAddId(kvp.Value);
            secondGeneratorSubKey1.GetOrAddId(kvp.Value);
        }

        snapshotTime = new DateTime(2018, 02, 03);
    }

    [TestMethod]
    public void PopulatedPQNameIdGenerator_New_CopiesValues()
    {
        var copyFirstSubKey1 = new PQNameIdLookupGenerator(firstGeneratorSubKey1);
        Assert.AreEqual(firstGeneratorSubKey1, copyFirstSubKey1);
    }

    [TestMethod]
    public void NewlyPopulatedPQNameIdLookup_HasUpdates_ExpectNoStringUpdatesWhenSetFalse()
    {
        Assert.IsTrue(firstGeneratorSubKey1.HasUpdates);
        Assert.IsTrue(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        firstGeneratorSubKey1.HasUpdates = false;
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        firstGeneratorSubKey1.HasUpdates = true;
        Assert.IsTrue(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());
    }

    [TestMethod]
    public void NoNewlyPopulatedLookups_AddNewEntry_MarksNewEntryForStringUpdate()
    {
        var firstSubKey1AsType = (PQNameIdLookupGenerator)firstGeneratorSubKey1;
        firstGeneratorSubKey1.HasUpdates = false;
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        Assert.IsFalse(firstSubKey1AsType.IsIdUpdated(4));

        var id = firstGeneratorSubKey1.GetOrAddId("FourthItem");

        Assert.IsTrue(firstSubKey1AsType.IsIdUpdated(4));

        var expectedStringUpdates = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(PQQuoteFields.SourceId, CrudCommand.Upsert.ToPQSubFieldId(), 0u), StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.Upsert, DictionaryId = id, Value = "FourthItem"
            }
        };

        var retrieved = firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).First();

        Assert.AreEqual(expectedStringUpdates, retrieved);
    }

    [TestMethod]
    public void PerDictionaryIdSubDictId_GetStringUpdates_PlacesDictionaryAndSubDictIdsInCorrectUpdateField()
    {
        var expectedStringUpdates = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(PQQuoteFields.SourceTickerId, CrudCommand.Upsert.ToPQSubFieldId(), 0u), StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.Upsert, DictionaryId = 1, Value = "FirstItem"
            }
        };

        expectedStringUpdates = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(PQQuoteFields.SourceTickerNames, CrudCommand.Upsert.ToPQSubFieldId(), 0u), StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.Upsert, DictionaryId = 1, Value = "FirstItem"
            }
        };
        var retrieved = secondGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).First();
        Assert.AreEqual(expectedStringUpdates, retrieved);
    }

    [TestMethod]
    public void NoNewlyPopulatedLookups_GetStringUpdatesAsFullSnapshot_ReturnsAllEntries()
    {
        firstGeneratorSubKey1.HasUpdates = false;
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        var allFields = firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Snapshot).ToList();

        Assert.AreEqual(3, allFields.Count);

        var newEmptyFirstDictSubKey1 = new PQNameIdLookupGenerator(PQQuoteFields.SourceId);
        foreach (var pqFieldStringUpdate in allFields) Assert.IsTrue(newEmptyFirstDictSubKey1.UpdateFieldString(pqFieldStringUpdate));

        Assert.AreEqual(firstGeneratorSubKey1, newEmptyFirstDictSubKey1);
    }

    [TestMethod]
    public void NoNewlyPopulatedLookups_UpdateFieldStringDifferentSubKey_IgnoresAllUpdates()
    {
        firstGeneratorSubKey1.HasUpdates = false;
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        var allFields = firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Snapshot).ToList();

        Assert.AreEqual(3, allFields.Count);

        var newEmptyFirstDictSubKey1 = new PQNameIdLookupGenerator(PQQuoteFields.SourceId);
        foreach (var pqFieldStringUpdate in allFields) Assert.IsTrue(newEmptyFirstDictSubKey1.UpdateFieldString(pqFieldStringUpdate));
    }

    [TestMethod]
    public void NoNewlyPopulatedLookups_UpdateFieldStringDifferentDictId_IgnoresAllUpdates()
    {
        firstGeneratorSubKey1.HasUpdates = false;
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        var allFields = firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Snapshot).ToList();

        Assert.AreEqual(3, allFields.Count);

        var newEmptyFirstDictSubKey1 = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerNames);
        foreach (var pqFieldStringUpdate in allFields) Assert.IsFalse(newEmptyFirstDictSubKey1.UpdateFieldString(pqFieldStringUpdate));

        Assert.AreEqual(0, newEmptyFirstDictSubKey1.Count);
    }

    [TestMethod]
    public void NonUpdateInsertAttempt_UpdateFieldString_IgnoresRequest()
    {
        var nonUpdateInsertPQFieldStringUpdate = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(PQQuoteFields.SourceTickerId, (ushort)CrudCommand.Upsert, 0u), StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.None, DictionaryId = 3, Value = "SomethingElse"
            }
        };

        Assert.IsFalse(firstGeneratorSubKey1.UpdateFieldString(nonUpdateInsertPQFieldStringUpdate));

        Assert.AreEqual("ThirdItem", firstGeneratorSubKey1.GetName(3));
    }

    [TestMethod]
    public void EmptyPQLookupGenerator_CopyFromSameInstance_NoChange()
    {
        Assert.AreEqual(3, firstGeneratorSubKey1.Count);
        var beforeCopyFrom = firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Snapshot).ToList();

        firstGeneratorSubKey1.CopyFrom(firstGeneratorSubKey1);

        Assert.AreEqual(3, firstGeneratorSubKey1.Count);
        var afterCopyFrom = firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Snapshot).ToList();
        Assert.IsTrue(beforeCopyFrom.SequenceEqual(afterCopyFrom));
    }

    [TestMethod]
    public void EmptyPQLookupGenerator_CopyFromPopulatedPQLookupGeneratorDefault_AreEqual()
    {
        var empty = new PQNameIdLookupGenerator(PQQuoteFields.SourceId);

        empty.CopyFrom(firstGeneratorSubKey1);

        Assert.AreEqual(firstGeneratorSubKey1, empty);
    }

    [TestMethod]
    public void UpdatedDicttionary_CopyFromOnlyUpdated_OnlyChangesCopiedAcross()
    {
        firstGeneratorSubKey1.HasUpdates = false;
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());
        var empty = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerId);

        empty.CopyFrom((INameIdLookup)firstGeneratorSubKey1, CopyMergeFlags.JustDifferences);

        Assert.AreEqual(3, empty.Count);

        firstGeneratorSubKey1.GetOrAddId("FourthItem");

        empty.CopyFrom((INameIdLookup)firstGeneratorSubKey1, CopyMergeFlags.JustDifferences);

        Assert.AreEqual(4, empty.Count);

        empty = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerId);

        empty.CopyFrom(firstGeneratorSubKey1);

        Assert.AreEqual(4, empty.Count);
    }

    [TestMethod]
    public void UpdatedDicttionary_CopyFromNonUpdatedAsWell_OnlyChangesCopiedAcross()
    {
        firstGeneratorSubKey1.HasUpdates = false;
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        firstGeneratorSubKey1.GetOrAddId("FourthItem");

        var empty = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerId);

        empty.CopyFrom((INameIdLookup)firstGeneratorSubKey1, CopyMergeFlags.FullReplace);

        Assert.AreEqual(4, empty.Count);
    }

    [TestMethod]
    public void PopulatedPQLookupGenerator_CopyFromNoAppend_ClearsPreviousValuesBeforeCopy()
    {
        firstGeneratorSubKey1.HasUpdates = false;
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        secondGeneratorSubKey1.CopyFrom(firstGeneratorSubKey1);

        Assert.AreEqual(3, secondGeneratorSubKey1.Count);

        firstGeneratorSubKey1.GetOrAddId("FourthItem");

        secondGeneratorSubKey1.CopyFrom(firstGeneratorSubKey1);

        Assert.AreEqual(4, secondGeneratorSubKey1.Count);
    }

    [TestMethod]
    public void UpdatedPQLookupGenerator_CopyFrom_CopyKeepsUpdatedTracking()
    {
        firstGeneratorSubKey1.Clear();
        Assert.IsFalse(firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).Any());

        firstGeneratorSubKey1.GetOrAddId("FourthItem");

        var originalUpdate = firstGeneratorSubKey1.GetStringUpdates(snapshotTime, StorageFlags.Update).First();

        var empty = new PQNameIdLookupGenerator(PQQuoteFields.SourceId);
        empty.CopyFrom((INameIdLookup)firstGeneratorSubKey1);
        var copyUpdate = empty.GetStringUpdates(snapshotTime, StorageFlags.Update).First();
        Assert.AreEqual(originalUpdate, copyUpdate);

        empty = new PQNameIdLookupGenerator(PQQuoteFields.SourceId);
        empty.CopyFrom((INameIdLookup)firstGeneratorSubKey1, CopyMergeFlags.FullReplace);
        copyUpdate = empty.GetStringUpdates(snapshotTime, StorageFlags.Update).First();
        Assert.AreEqual(originalUpdate, copyUpdate);

        empty = new PQNameIdLookupGenerator(PQQuoteFields.SourceId);
        empty.CopyFrom(firstGeneratorSubKey1);
        copyUpdate = empty.GetStringUpdates(snapshotTime, StorageFlags.Update).First();
        Assert.AreEqual(originalUpdate, copyUpdate);
    }

    [TestMethod]
    public void NonPQLookupGenerator_EmptyCopyFrom_CopiesEverythingNothingMarkedUpdated()
    {
        var populatedIdLookupGenerator = new NameIdLookupGenerator(initialValues);

        var empty = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerId);

        empty.CopyFrom(populatedIdLookupGenerator);

        Assert.AreEqual(3, empty.Count);
        Assert.IsTrue(populatedIdLookupGenerator.AreEquivalent(empty));
        Assert.IsFalse(populatedIdLookupGenerator.AreEquivalent(empty, true));
    }

    [TestMethod]
    public void NonPQLookupGenerator_PopulatedCopyFrom_NoAppendCopiesRemainingIsOnlyWhatWasPassedIn()
    {
        initialValues = new Dictionary<int, string>
        {
            { 5, "FifthItem" }, { 6, "SixthItem" }
        };

        var populatedIdLookupGenerator = new NameIdLookupGenerator(initialValues);

        Assert.AreEqual(3, secondGeneratorSubKey1.Count);
        Assert.AreEqual("FirstItem", secondGeneratorSubKey1.GetName(1));
        Assert.AreEqual("SecondItem", secondGeneratorSubKey1.GetName(2));
        Assert.AreEqual("ThirdItem", secondGeneratorSubKey1.GetName(3));

        secondGeneratorSubKey1.CopyFrom(populatedIdLookupGenerator);

        Assert.AreEqual(5, secondGeneratorSubKey1.Count);
        Assert.AreEqual("FirstItem", secondGeneratorSubKey1.GetName(1));
        Assert.AreEqual("SecondItem", secondGeneratorSubKey1.GetName(2));
        Assert.AreEqual("ThirdItem", secondGeneratorSubKey1.GetName(3));
        Assert.AreEqual("FifthItem", secondGeneratorSubKey1.GetName(5));
        Assert.AreEqual("SixthItem", secondGeneratorSubKey1.GetName(6));
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var cloneIdLookup = firstGeneratorSubKey1.Clone();
        Assert.AreNotSame(cloneIdLookup, firstGeneratorSubKey1);
        for (var i = 1; i < 4; i++)
        {
            Assert.AreEqual(firstGeneratorSubKey1[i], cloneIdLookup[i]);
            Assert.AreEqual(i, cloneIdLookup[firstGeneratorSubKey1[i]!]);
            Assert.AreEqual(firstGeneratorSubKey1[i], cloneIdLookup.GetValue(i));
            Assert.AreEqual(i, cloneIdLookup.GetId(firstGeneratorSubKey1[i]));
        }

        Assert.AreEqual(firstGeneratorSubKey1, cloneIdLookup);
    }

    [TestMethod]
    public void FromBaseTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var cloneIdLookup = ((INameIdLookupGenerator)firstGeneratorSubKey1).Clone();
        Assert.AreNotSame(cloneIdLookup, firstGeneratorSubKey1);
        for (var i = 1; i < 4; i++)
        {
            Assert.AreEqual(firstGeneratorSubKey1[i], cloneIdLookup[i]);
            Assert.AreEqual(i, cloneIdLookup[firstGeneratorSubKey1[i]!]);
            Assert.AreEqual(firstGeneratorSubKey1[i], cloneIdLookup.GetValue(i));
            Assert.AreEqual(i, cloneIdLookup.GetId(firstGeneratorSubKey1[i]));
        }

        Assert.AreEqual(firstGeneratorSubKey1, cloneIdLookup);
    }

    [TestMethod]
    public void FromTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var cloneIdLookup = (IPQNameIdLookupGenerator)((PQNameIdLookupGenerator)firstGeneratorSubKey1).Clone();
        Assert.AreNotSame(cloneIdLookup, firstGeneratorSubKey1);
        for (var i = 1; i < 4; i++)
        {
            Assert.AreEqual(firstGeneratorSubKey1[i], cloneIdLookup[i]);
            Assert.AreEqual(i, cloneIdLookup[firstGeneratorSubKey1[i]!]);
            Assert.AreEqual(firstGeneratorSubKey1[i], cloneIdLookup.GetValue(i));
            Assert.AreEqual(i, cloneIdLookup.GetId(firstGeneratorSubKey1[i]));
        }

        Assert.AreEqual(firstGeneratorSubKey1, cloneIdLookup);
    }

    [TestMethod]
    public void DifferingPqDictsNumAndSubNumber_AreEquivalent_ReturnsExpected()
    {
        var removedLastEntry = initialValues;
        removedLastEntry.Remove(3);

        Assert.IsTrue(firstGeneratorSubKey1.AreEquivalent(secondGeneratorSubKey1));

        var subFirstSubKey1 = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerId);
        PopulateGenerator(subFirstSubKey1, removedLastEntry);
        Assert.IsTrue(firstGeneratorSubKey1.AreEquivalent(subFirstSubKey1));
        Assert.IsFalse(firstGeneratorSubKey1.AreEquivalent(subFirstSubKey1, true));

        var subFirstSubKey2 = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerId);
        PopulateGenerator(subFirstSubKey2, removedLastEntry);
        Assert.IsTrue(secondGeneratorSubKey1.AreEquivalent(subFirstSubKey2));

        var subSecondSubKey1 = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerNames);
        PopulateGenerator(subSecondSubKey1, removedLastEntry);
        Assert.IsTrue(secondGeneratorSubKey1.AreEquivalent(subSecondSubKey1));
        Assert.IsFalse(secondGeneratorSubKey1.AreEquivalent(subSecondSubKey1, true));
        Assert.IsTrue(firstGeneratorSubKey1.AreEquivalent(subSecondSubKey1));
    }

    [TestMethod]
    public void FullyPopulatedPQNameLookupGenerator_ToString_ReturnsNameAndValues()
    {
        var toString = firstGeneratorSubKey1.ToString()!;

        Assert.IsTrue(toString.Contains(firstGeneratorSubKey1.GetType().Name));

        foreach (var kvp in firstGeneratorSubKey1) Assert.IsTrue(toString.Contains($"{kvp.Key}:{kvp.Value}"));
        Assert.IsTrue(toString.Contains($"dictionaryFieldKey: {PQQuoteFields.SourceId.ToString()}"));
    }

    private void PopulateGenerator
    (IPQNameIdLookupGenerator pqNameIdLookupGenerator,
        IDictionary<int, string> source)
    {
        foreach (var kvp in source) pqNameIdLookupGenerator.GetOrAddId(kvp.Value);
    }
}
