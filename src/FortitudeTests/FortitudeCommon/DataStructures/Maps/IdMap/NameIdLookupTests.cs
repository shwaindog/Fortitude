using System;
using System.Collections.Generic;
using FortitudeCommon.DataStructures.Maps.IdMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap
{
    [TestClass()]
    public class NameIdLookupTests
    {
        private INameIdLookup nameIdLookup;
        private Dictionary<int, string> initialValues;

        [TestInitialize]
        public void Setup()
        {
            initialValues = new Dictionary<int, string>
            {
                {1, "FirstItem"},
                {2, "SecondItem"},
                {3, "ThirdItem"}
            };

            nameIdLookup = new NameIdLookup(initialValues);
        }

        [TestMethod]
        public void NameIdLookup_New_CanBeInitialized()
        {
            var empty = new NameIdLookup();

            Assert.AreEqual(0, empty.Count);

            var copy = new NameIdLookup(nameIdLookup);

            Assert.AreNotSame(nameIdLookup, copy);
            Assert.AreEqual(nameIdLookup, copy);
        }

        [TestMethod]
        public void GivenAnId_GetName_GetExpectedName()
        {
            Assert.AreEqual("FirstItem", nameIdLookup.GetName(1));
            Assert.AreEqual("SecondItem", nameIdLookup.GetName(2));
            Assert.AreEqual("ThirdItem", nameIdLookup.GetName(3));
        }

        [TestMethod]
        public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
        {
            var cloneIdLookup = nameIdLookup.Clone();
            Assert.AreNotSame(cloneIdLookup, nameIdLookup);
            for (int i = 1; i < 4; i++)
            {
                Assert.AreEqual(nameIdLookup[i], cloneIdLookup[i]);
                Assert.AreEqual(i, cloneIdLookup[nameIdLookup[i]]);
                Assert.AreEqual(nameIdLookup[i], cloneIdLookup.GetValue(i));
                Assert.AreEqual(i, cloneIdLookup.GetId(nameIdLookup[i]));
            }
            Assert.AreEqual(nameIdLookup, cloneIdLookup);
        }

        [TestMethod]
        public void FromBaseTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
        {
            var cloneIdLookup = ((IIdLookup<string>)nameIdLookup).Clone();
            Assert.AreNotSame(cloneIdLookup, nameIdLookup);
            for (int i = 1; i < 4; i++)
            {
                Assert.AreEqual(nameIdLookup[i], cloneIdLookup[i]);
                Assert.AreEqual(i, cloneIdLookup[nameIdLookup[i]]);
                Assert.AreEqual(nameIdLookup[i], cloneIdLookup.GetValue(i));
                Assert.AreEqual(i, cloneIdLookup.GetId(nameIdLookup[i]));
            }
            Assert.AreEqual(nameIdLookup, cloneIdLookup);
        }

        [TestMethod]
        public void FromTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
        {
            var cloneIdLookup = (INameIdLookup)((NameIdLookup)nameIdLookup).Clone();
            Assert.AreNotSame(cloneIdLookup, nameIdLookup);
            for (int i = 1; i < 4; i++)
            {
                Assert.AreEqual(nameIdLookup[i], cloneIdLookup[i]);
                Assert.AreEqual(i, cloneIdLookup[nameIdLookup[i]]);
                Assert.AreEqual(nameIdLookup[i], cloneIdLookup.GetValue(i));
                Assert.AreEqual(i, cloneIdLookup.GetId(nameIdLookup[i]));
            }
            Assert.AreEqual(nameIdLookup, cloneIdLookup);
        }

        [TestMethod]
        public void LookupId_AreEquivalent_ReturnsFalse()
        {
            var idLookup = new IdLookup<string>(initialValues);

            Assert.IsFalse(nameIdLookup.AreEquivalent(idLookup));
        }

        [TestMethod]
        public void LookupIdGenerator_AreEquivalentExactTypes_ReturnsFalse()
        {
            var idLookupGenerator = new NameIdLookupGenerator(initialValues);

            Assert.IsFalse(nameIdLookup.AreEquivalent(idLookupGenerator, true));
        }

        [TestMethod]
        public void ClonedPopulated_Equals_True()
        {
            var clone = nameIdLookup.Clone();

            Assert.AreNotSame(nameIdLookup, clone);
            Assert.AreEqual(nameIdLookup, clone);
        }
    }
}