using FortitudeCommon.DataStructures.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeCommon.DataStructures.Maps
{
    [TestClass]
    public class ConcurrentMapTests
    {
        [TestMethod]
        public void EmptyConcurrentMap_AddThenClear_SavesItemWithKeyClearRemovesAll()
        {
            var concurrentMap = new ConcurrentMap<string, string> {{"firstKey", "firstValue"}};
            
            Assert.AreEqual(1, concurrentMap.Count);
            Assert.IsTrue(concurrentMap.ContainsKey("firstKey"));
            Assert.AreEqual("firstValue", concurrentMap["firstKey"]);

            concurrentMap["secondKey"] = "secondValue";

            Assert.AreEqual(2, concurrentMap.Count);
            Assert.IsTrue(concurrentMap.ContainsKey("secondKey"));
            Assert.AreEqual("secondValue", concurrentMap["secondKey"]);

            concurrentMap.Clear();
            Assert.AreEqual(0, concurrentMap.Count);
            Assert.IsFalse(concurrentMap.ContainsKey("firstKey"));
            Assert.IsFalse(concurrentMap.ContainsKey("secondKey"));
        }
        
        [TestMethod]
        public void ConcurrentMapWithItems_TryGetValue_FindsSomeOfTheItems()
        {
            var concurrentMap = new ConcurrentMap<string, string>
            {
                {"firstKey", "firstValue"},
                { "secondKey", "secondValue"}
            };

            Assert.IsTrue(concurrentMap.TryGetValue("firstKey", out var checkValue));
            Assert.AreEqual("firstValue", checkValue);

            Assert.IsFalse(concurrentMap.TryGetValue("nonExistantKey", out checkValue));
        }
        
        [TestMethod]
        public void ConcurrentMapWithItems_Remove_LeavesRemainingItems()
        {
            var concurrentMap = new ConcurrentMap<string, string>
            {
                { "firstKey", "firstValue" },
                { "secondKey", "secondValue" } 
            };

            concurrentMap.Remove("firstKey");
            Assert.AreEqual(1, concurrentMap.Count);
            Assert.IsFalse(concurrentMap.ContainsKey("firstKey"));
            Assert.IsTrue(concurrentMap.ContainsKey("secondKey"));
            Assert.AreEqual("secondValue", concurrentMap["secondKey"]);
        }

        [TestMethod]
        public void ConcurrentMapWithItems_Edit_FiresOnUpdateCallback()
        {
            int countCallbackCalled = 0;

            var concurrentMap = new ConcurrentMap<string, string>();
            concurrentMap.Updated += (_, _, _, _)  => countCallbackCalled++;

            concurrentMap.TryAdd("firstKey", "firstValue");
            Assert.AreEqual(1, countCallbackCalled);
            concurrentMap["secondKey"] = "secondValue";
            Assert.AreEqual(2, countCallbackCalled);
            concurrentMap.Remove("firstKey");
            Assert.AreEqual(3, countCallbackCalled);
            concurrentMap.Clear();
            Assert.AreEqual(4, countCallbackCalled);
        }

        [TestMethod]
        public void ConcurrentMapWithItems_GetEnumerator_ReturnsMapValues()
        {
            var concurrentMap = new ConcurrentMap<string, string>
            {
                { "firstKey", "firstValue" },
                { "secondKey", "secondValue" }
            };

            var enumerator = concurrentMap.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            try
            {
                Assert.AreEqual("firstValue", enumerator.Current.Value);
            }
            catch (AssertFailedException)
            {
                Assert.AreEqual("secondValue", enumerator.Current.Value);
            }

            Assert.IsTrue(enumerator.MoveNext());
            try
            {
                Assert.AreEqual("secondValue", enumerator.Current.Value);
            }
            catch (AssertFailedException)
            {
                Assert.AreEqual("firstValue", enumerator.Current.Value);
            }
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }
    }
}