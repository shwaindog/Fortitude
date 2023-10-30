#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Maps.IdMap;

#endregion

namespace FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap;

[TestClass]
public class IdLookupTests
{
    private IIdLookup<DateTime> emptyLookup = new IdLookup<DateTime>();
    private IIdLookup<DateTime> populatedDateLookup = new IdLookup<DateTime>();
    private Dictionary<int, DateTime> populatedLookupInitialValuesDict = new();

    [TestInitialize]
    public void Setup()
    {
        populatedLookupInitialValuesDict = new Dictionary<int, DateTime>
        {
            { 1, new DateTime(2018, 02, 03, 12, 58, 11) }, { 2, new DateTime(2018, 02, 02, 12, 58, 11) }
            , { 3, new DateTime(2018, 02, 01, 12, 58, 11) }
        };

        populatedDateLookup = new IdLookup<DateTime>(populatedLookupInitialValuesDict);
        emptyLookup = new IdLookup<DateTime>();
    }

    [TestMethod]
    public void PopulatedLookup_ChangeInitializationDictionary_DoesnAffectLookup()
    {
        var clonedInitialDictionary = new Dictionary<int, DateTime>(populatedLookupInitialValuesDict);

        populatedLookupInitialValuesDict.Clear();

        Assert.AreEqual(3, populatedDateLookup.Count);
        Assert.AreEqual(1, populatedDateLookup.GetId(clonedInitialDictionary[1]));
        Assert.AreEqual(2, populatedDateLookup.GetId(clonedInitialDictionary[2]));
        Assert.AreEqual(3, populatedDateLookup.GetId(clonedInitialDictionary[3]));
        Assert.AreEqual(1, populatedDateLookup[clonedInitialDictionary[1]]);
        Assert.AreEqual(2, populatedDateLookup[clonedInitialDictionary[2]]);
        Assert.AreEqual(3, populatedDateLookup[clonedInitialDictionary[3]]);
        Assert.AreEqual(clonedInitialDictionary[1], populatedDateLookup.GetValue(1));
        Assert.AreEqual(clonedInitialDictionary[2], populatedDateLookup.GetValue(2));
        Assert.AreEqual(clonedInitialDictionary[3], populatedDateLookup.GetValue(3));
        Assert.AreEqual(clonedInitialDictionary[1], populatedDateLookup[1]);
        Assert.AreEqual(clonedInitialDictionary[2], populatedDateLookup[2]);
        Assert.AreEqual(clonedInitialDictionary[3], populatedDateLookup[3]);
    }

    [TestMethod]
    public void EmptyLookup_New_ContainsNoData()
    {
        Assert.AreEqual(0, emptyLookup.Count);
        Assert.AreEqual(-1, emptyLookup.GetId(DateTime.Now));
        Assert.AreEqual(-1, emptyLookup[DateTime.Now]);
        Assert.AreEqual(default, emptyLookup.GetValue(1));
        Assert.AreEqual(default, emptyLookup[1]);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var cloneIdLookup = populatedDateLookup.Clone();
        Assert.AreNotSame(cloneIdLookup, populatedDateLookup);
        for (var i = 1; i < 4; i++)
        {
            Assert.AreEqual(populatedDateLookup[i], cloneIdLookup[i]);
            Assert.AreEqual(i, cloneIdLookup[populatedDateLookup[i]]);
            Assert.AreEqual(populatedDateLookup[i], cloneIdLookup.GetValue(i));
            Assert.AreEqual(i, cloneIdLookup.GetId(populatedDateLookup[i]));
        }

        Assert.AreEqual(populatedDateLookup, cloneIdLookup);
    }

    [TestMethod]
    public void FromTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var cloneIdLookup = (IIdLookup<DateTime>)((IdLookup<DateTime>)populatedDateLookup).Clone();
        Assert.AreNotSame(cloneIdLookup, populatedDateLookup);
        for (var i = 1; i < 4; i++)
        {
            Assert.AreEqual(populatedDateLookup[i], cloneIdLookup[i]);
            Assert.AreEqual(i, cloneIdLookup[populatedDateLookup[i]]);
            Assert.AreEqual(populatedDateLookup[i], cloneIdLookup.GetValue(i));
            Assert.AreEqual(i, cloneIdLookup.GetId(populatedDateLookup[i]));
        }

        Assert.AreEqual(populatedDateLookup, cloneIdLookup);
    }

    [TestMethod]
    public void OneMissingValue_AreEquivalent_ReturnsExpected()
    {
        var clonedInitialDictionary = new Dictionary<int, DateTime>(populatedLookupInitialValuesDict);

        clonedInitialDictionary.Remove(3);

        var missingLastKvp = new IdLookup<DateTime>(clonedInitialDictionary);

        Assert.IsTrue(populatedDateLookup.AreEquivalent(missingLastKvp));
        Assert.IsFalse(missingLastKvp.AreEquivalent(populatedDateLookup));

        Assert.IsFalse(populatedDateLookup.AreEquivalent(missingLastKvp, true));
        Assert.IsFalse(missingLastKvp.AreEquivalent(populatedDateLookup, true));

        Assert.AreNotEqual(populatedDateLookup, missingLastKvp);
    }

    [TestMethod]
    public void OneDiffValue_AreEquivalent_ReturnsExpected()
    {
        var clonedInitialDictionary =
            new Dictionary<int, DateTime>(populatedLookupInitialValuesDict)
            {
                [3] = new(2018, 02, 03, 14, 09, 33)
            };


        var missingLastKvp = new IdLookup<DateTime>(clonedInitialDictionary);

        Assert.IsFalse(populatedDateLookup.AreEquivalent(missingLastKvp));
        Assert.IsFalse(missingLastKvp.AreEquivalent(populatedDateLookup));

        Assert.IsFalse(populatedDateLookup.AreEquivalent(missingLastKvp, true));
        Assert.IsFalse(missingLastKvp.AreEquivalent(populatedDateLookup, true));

        Assert.AreNotEqual(populatedDateLookup, missingLastKvp);
    }

    [TestMethod]
    public void ClonedPopulated_Equals_True()
    {
        var clone = populatedDateLookup.Clone();

        Assert.AreNotSame(populatedDateLookup, clone);
        Assert.AreEqual(populatedDateLookup, clone);
    }

    [TestMethod]
    public void PopulatedLookup_GetHashcode_IsNotZero()
    {
        Assert.AreNotEqual(0, populatedDateLookup.GetHashCode());

        Assert.AreEqual(populatedDateLookup.GetHashCode(), populatedDateLookup.Clone().GetHashCode());
    }

    [TestMethod]
    public void EmptyLookup_GetHashcode_IsZero()
    {
        Assert.AreEqual(0, emptyLookup.GetHashCode());
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public void PopulatedNameId_GetEnumerator_IteratesKeyValuePairs()
    {
        var enumerator = ((IEnumerable)populatedDateLookup).GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(1, ((KeyValuePair<int, DateTime>)enumerator.Current).Key);
        Assert.AreEqual(new DateTime(2018, 02, 03, 12, 58, 11),
            ((KeyValuePair<int, DateTime>)enumerator.Current).Value);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(2, ((KeyValuePair<int, DateTime>)enumerator.Current).Key);
        Assert.AreEqual(new DateTime(2018, 02, 02, 12, 58, 11),
            ((KeyValuePair<int, DateTime>)enumerator.Current).Value);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(3, ((KeyValuePair<int, DateTime>)enumerator.Current).Key);
        Assert.AreEqual(new DateTime(2018, 02, 01, 12, 58, 11),
            ((KeyValuePair<int, DateTime>)enumerator.Current).Value);
        Assert.IsFalse(enumerator.MoveNext());
        enumerator.Reset();
    }

    [TestMethod]
    public void PopulatedNameId_GenericGetEnumerator_IteratesKeyValuePairs()
    {
        var enumerator = populatedDateLookup.GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(1, enumerator.Current.Key);
        Assert.AreEqual(new DateTime(2018, 02, 03, 12, 58, 11), enumerator.Current.Value);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(2, enumerator.Current.Key);
        Assert.AreEqual(new DateTime(2018, 02, 02, 12, 58, 11), enumerator.Current.Value);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(3, enumerator.Current.Key);
        Assert.AreEqual(new DateTime(2018, 02, 01, 12, 58, 11), enumerator.Current.Value);
        Assert.IsFalse(enumerator.MoveNext());
        enumerator.Dispose();
    }
}
