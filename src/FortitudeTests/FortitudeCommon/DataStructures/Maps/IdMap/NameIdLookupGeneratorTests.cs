using System.Collections.Generic;
using System.Data;
using System.Linq;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap
{
    [TestClass]
    public class NameIdLookupGeneratorTests
    {
        private INameIdLookupGenerator emptyNameIdLookupGenerator;
        private INameIdLookupGenerator populatedIdLookupGenerator;
        private Dictionary<int, string> initialValues;

        public static INameIdLookupGenerator Dummy3NameIdLookup
        {
            get
            {
                var nameIdLookupGenerator = new NameIdLookupGenerator();
                nameIdLookupGenerator.GetOrAddId("FirstItem");
                nameIdLookupGenerator.GetOrAddId("SecondItem");
                nameIdLookupGenerator.GetOrAddId("ThirdItem");
                return nameIdLookupGenerator;
            }
        }

        [TestInitialize]
        public void Setup()
        {
            initialValues = new Dictionary<int, string>
            {
                {1, "FirstItem"},
                {2, "SecondItem"},
                {3, "ThirdItem"}
            };

            emptyNameIdLookupGenerator = new NameIdLookupGenerator();
            populatedIdLookupGenerator = new NameIdLookupGenerator(initialValues);
        }

        [TestMethod]
        public void NameIdLookup_New_CanBeInitialized()
        {
            Assert.AreEqual(0, emptyNameIdLookupGenerator.Count);
            
            var copy = new NameIdLookupGenerator(populatedIdLookupGenerator);

            Assert.AreNotSame(populatedIdLookupGenerator, copy);
            Assert.AreEqual(populatedIdLookupGenerator, copy);
        }

        [TestMethod]
        public void EmptyLookupGenerator_Indexer_AddsIfMissing()
        {
            Assert.AreEqual(0, emptyNameIdLookupGenerator.Count);

            var id = emptyNameIdLookupGenerator["FirstItem"];
            Assert.AreEqual(1, emptyNameIdLookupGenerator.Count);
            Assert.AreEqual(1, id);
        }

        [TestMethod]
        public void GapPopulatedLookupGenerator_AddLowerIdEntry_NextEntryIsNextHighest()
        {
            var highestValue = initialValues.Where(kvp => kvp.Key == 3)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var lowerTwoValues = initialValues.Where(kvp => kvp.Key != 3)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var justHighestValueLookup = new NameIdLookupGenerator(highestValue);

            justHighestValueLookup.AppendNewNames(lowerTwoValues);

            var id = justHighestValueLookup.GetOrAddId("FourthItem");
            Assert.AreEqual(4, id);
        }

        [TestMethod]
        public void EmptyNameId_GetIdOrGetOrAddId_CreatesIdsIfNotAssigned()
        {
            var id = emptyNameIdLookupGenerator.GetId("FirstItem");
            Assert.AreEqual(1 , id);
            var addId = emptyNameIdLookupGenerator.GetOrAddId("FirstItem");
            id = emptyNameIdLookupGenerator.GetId("FirstItem");
            Assert.AreEqual(1, id);
            Assert.AreEqual(addId, id);
            id = emptyNameIdLookupGenerator.GetId("SecondItem");
            Assert.AreEqual(2, id);
            addId = emptyNameIdLookupGenerator.GetOrAddId("SecondItem");
            id = emptyNameIdLookupGenerator.GetId("SecondItem");
            Assert.AreEqual(2, id);
            Assert.AreEqual(addId, id);
            id = emptyNameIdLookupGenerator["ThirdItem"];
            Assert.AreEqual(3, id);
            
            Assert.AreEqual(populatedIdLookupGenerator, emptyNameIdLookupGenerator);
        }

        [TestMethod]
        public void AddingSameItem_GetOrAddId_DoesNotAddNewIdOrThrowError()
        {
            var id = emptyNameIdLookupGenerator.GetId("FirstItem");
            Assert.AreEqual(1, id);
            emptyNameIdLookupGenerator.GetOrAddId("FirstItem");
            id = emptyNameIdLookupGenerator.GetId("FirstItem");
            Assert.AreEqual(1, id);
            emptyNameIdLookupGenerator.SetIdToName(1, "FirstItem");
        }

        [TestMethod, ExpectedException(typeof(DuplicateNameException))]
        public void PopulatedLookupGenerator_AppendNewNamesWithDifference_ThrowsExpection()
        {
            initialValues[3] = "DifferentName";

            populatedIdLookupGenerator.AppendNewNames(initialValues);
        }

        [TestMethod, ExpectedException(typeof(DuplicateNameException))]
        public void PopulatedLookupGenerator_SetIdToNameWithDifferenceForId_ThrowsExpection()
        {
            populatedIdLookupGenerator.SetIdToName(2, "DifferentName");
        }

        [TestMethod]
        public void PopulatedLookupGenerator_AddingKeyValueCollection_AddsNewItemsWIthSameId()
        {
            Assert.AreEqual(1, populatedIdLookupGenerator.GetId("FirstItem"));
            Assert.AreEqual(2, populatedIdLookupGenerator.GetId("SecondItem"));
            Assert.AreEqual(3, populatedIdLookupGenerator.GetId("ThirdItem"));
            populatedIdLookupGenerator.AppendNewNames(populatedIdLookupGenerator.Clone());
            Assert.AreEqual(1, populatedIdLookupGenerator.GetId("FirstItem"));
            Assert.AreEqual(2, populatedIdLookupGenerator.GetId("SecondItem"));
            Assert.AreEqual(3, populatedIdLookupGenerator.GetId("ThirdItem"));
        }

        [TestMethod]
        public void EmptyNameId_AddingKeyValueCollection_AddsNewItemsWIthSameId()
        {
            Assert.AreEqual(0, emptyNameIdLookupGenerator.Count);
            emptyNameIdLookupGenerator.AppendNewNames(populatedIdLookupGenerator);
            Assert.AreEqual(3, emptyNameIdLookupGenerator.Count);
            Assert.AreEqual(1, emptyNameIdLookupGenerator.GetId("FirstItem"));
            Assert.AreEqual(2, emptyNameIdLookupGenerator.GetId("SecondItem"));
            Assert.AreEqual(3, emptyNameIdLookupGenerator.GetId("ThirdItem"));
        }

        [TestMethod]
        public void EmptyNameIdLookup_CopyFromPopulatedLookupGenerator_CopiesValuesOver()
        {
            Assert.AreEqual(0, emptyNameIdLookupGenerator.Count);

            emptyNameIdLookupGenerator.CopyFrom(populatedIdLookupGenerator);

            Assert.AreEqual(populatedIdLookupGenerator, emptyNameIdLookupGenerator);
        }

        [TestMethod]
        public void PopulatedIdLookupGenerator_CopyFromPopulatedLookupGenerator_DoesNothing()
        {
            var clone = populatedIdLookupGenerator.Clone();

            Assert.AreEqual(3, clone.Count);

            emptyNameIdLookupGenerator.CopyFrom(clone);

            Assert.AreEqual(populatedIdLookupGenerator, clone);
        }

        [TestMethod]
        public void PopulatedIdLookupGenerator_CopyFromSameInstance_DoesNothing()
        {
            Assert.AreEqual(3, populatedIdLookupGenerator.Count);

            populatedIdLookupGenerator.CopyFrom(populatedIdLookupGenerator);

            Assert.AreEqual(3, populatedIdLookupGenerator.Count);
        }

        [TestMethod]
        public void PopulatedReadonlyIdLookup_CopyFromSameInstance_DoesNothing()
        {
            var nameIdLookup = new NameIdLookup(initialValues);
            Assert.AreEqual(3, nameIdLookup.Count);

            emptyNameIdLookupGenerator.CopyFrom(populatedIdLookupGenerator);

            Assert.AreEqual(3, emptyNameIdLookupGenerator.Count);
            Assert.IsTrue(nameIdLookup.AreEquivalent(emptyNameIdLookupGenerator));
            Assert.IsFalse(emptyNameIdLookupGenerator.AreEquivalent(nameIdLookup));
        }

        [TestMethod]
        public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
        {
            var cloneIdLookup = populatedIdLookupGenerator.Clone();
            Assert.AreNotSame(cloneIdLookup, populatedIdLookupGenerator);
            for (int i = 1; i < 4; i++)
            {
                Assert.AreEqual(populatedIdLookupGenerator[i], cloneIdLookup[i]);
                Assert.AreEqual(i, cloneIdLookup[populatedIdLookupGenerator[i]]);
                Assert.AreEqual(populatedIdLookupGenerator[i], cloneIdLookup.GetValue(i));
                Assert.AreEqual(i, cloneIdLookup.GetId(populatedIdLookupGenerator[i]));
            }
            Assert.AreEqual(populatedIdLookupGenerator, cloneIdLookup);
        }

        [TestMethod]
        public void FromBaseTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
        {
            var cloneIdLookup = ((INameIdLookup)populatedIdLookupGenerator).Clone();
            Assert.AreNotSame(cloneIdLookup, populatedIdLookupGenerator);
            for (int i = 1; i < 4; i++)
            {
                Assert.AreEqual(populatedIdLookupGenerator[i], cloneIdLookup[i]);
                Assert.AreEqual(i, cloneIdLookup[populatedIdLookupGenerator[i]]);
                Assert.AreEqual(populatedIdLookupGenerator[i], cloneIdLookup.GetValue(i));
                Assert.AreEqual(i, cloneIdLookup.GetId(populatedIdLookupGenerator[i]));
            }
            Assert.AreEqual(populatedIdLookupGenerator, cloneIdLookup);
        }

        [TestMethod]
        public void FromTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
        {
            var cloneIdLookup = (INameIdLookupGenerator)((NameIdLookupGenerator)populatedIdLookupGenerator).Clone();
            Assert.AreNotSame(cloneIdLookup, populatedIdLookupGenerator);
            for (int i = 1; i < 4; i++)
            {
                Assert.AreEqual(populatedIdLookupGenerator[i], cloneIdLookup[i]);
                Assert.AreEqual(i, cloneIdLookup[populatedIdLookupGenerator[i]]);
                Assert.AreEqual(populatedIdLookupGenerator[i], cloneIdLookup.GetValue(i));
                Assert.AreEqual(i, cloneIdLookup.GetId(populatedIdLookupGenerator[i]));
            }
            Assert.AreEqual(populatedIdLookupGenerator, cloneIdLookup);
        }

        [TestMethod]
        public void SmallerPopulatedLookupId_AreEquivalent_ReturnsExpected()
        {
            var lowerTwoValues = initialValues.Where(kvp => kvp.Key != 3).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var subSetOfFullyPopulated = new NameIdLookupGenerator(lowerTwoValues);

            Assert.IsTrue(populatedIdLookupGenerator.AreEquivalent(subSetOfFullyPopulated));
            Assert.IsFalse(populatedIdLookupGenerator.AreEquivalent(subSetOfFullyPopulated, true));

            Assert.IsFalse(subSetOfFullyPopulated.AreEquivalent(populatedIdLookupGenerator));
            Assert.IsFalse(subSetOfFullyPopulated.AreEquivalent(populatedIdLookupGenerator, true));
        }

        [TestMethod]
        public void LookupId_AreEquivalent_ReturnsFalse()
        {
            var idLookup = new NameIdLookup(initialValues);

            Assert.IsFalse(populatedIdLookupGenerator.AreEquivalent(idLookup));
        }

        [TestMethod]
        public void LookupIdGenerator_AreEquivalentExactTypes_ReturnsFalse()
        {
            var pqIdLookupGenerator = new PQNameIdLookupGenerator(populatedIdLookupGenerator);

            Assert.IsFalse(populatedIdLookupGenerator.AreEquivalent(pqIdLookupGenerator, true));
        }
    }
}