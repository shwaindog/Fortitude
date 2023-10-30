#region

using FortitudeCommon.Extensions;

#endregion

namespace FortitudeTests.FortitudeCommon.Extensions;

[TestClass]
public class DiffNewUpdatedDeletedTests
{
    [TestMethod]
    public void ItemsWithId_Diffed_ShowCorrectUpdatedDeleted()
    {
        var initialList = new Dictionary<int, string>
        {
            { 1, "Apple" }, { 2, "Dog" }, { 4, "Frog" }, { 5, "Orange" }
        }.ToList();
        var updatedList = new Dictionary<int, string>
        {
            { 2, "Dog" }, { 3, "Banana" }, { 5, "Mouse" }
        }.ToList();

        var results = initialList.Diff(updatedList, kvp => kvp.Key);

        Assert.AreEqual(1, results.NewItems.Count());
        Assert.AreEqual(1, results.UpdatedItems.Count());
        Assert.AreEqual(2, results.DeletedItems.Count());
        Assert.AreEqual(new KeyValuePair<int, string>(3, "Banana"), results.NewItems.ElementAt(0));
        Assert.AreEqual(new KeyValuePair<int, string>(5, "Mouse"), results.UpdatedItems.ElementAt(0));
        Assert.AreEqual(new KeyValuePair<int, string>(1, "Apple"), results.DeletedItems.ElementAt(0));
        Assert.AreEqual(new KeyValuePair<int, string>(4, "Frog"), results.DeletedItems.ElementAt(1));
    }
}
