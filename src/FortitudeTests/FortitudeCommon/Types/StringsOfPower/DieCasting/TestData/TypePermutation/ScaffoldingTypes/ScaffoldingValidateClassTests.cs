// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

[NoMatchingProductionClass]
[TestClass]
public class ScaffoldingValidateClassTests
{
    private IReadOnlyList<ScaffoldingPartEntry> scafReg = ScaffoldingRegistry.AllScaffoldingTypes;

    private static IVersatileFLogger logger = null!;

    public readonly string[] ClassNameCleanup = ["`1", "`2", "`3", "`4", "`5", "`6", "Field"];
    public readonly string[] WithSelectKeysNameCleanup =
    [
        ComplexKeyedCollectionFieldWithSelectKeysStripOut
      , ComplexKeyedCollectionFieldAlwaysAddAllStripOut
      , ComplexKeyedCollectionFieldAlwaysAddFilteredStripOut
      , ComplexKeyedCollectionFieldWhenNonNullAddAllStripOut
      , ComplexKeyedCollectionFieldWhenNonNullAddFilteredStripOut
      , ComplexKeyedCollectionFieldWhenPopulatedWithFilterStripOut
      , ComplexKeyedCollectionFieldWhenPopulatedStripOut
      , ComplexKeyedCollectionFieldWhenNonNullAddStripOut
      , ComplexKeyedCollectionFieldAlwaysStripOut
      , ComplexKeyedCollectionFieldStringBearerStripOut
    ];

    private readonly string[] complexFieldAllowedNonDefaultExemptions       = ["NullableBool"];
    private readonly string[] complexFieldAllowedNonNullExemptions          = ["Bool"];
    private readonly string[] complexFieldAllowedNonNullOrDefaultExemptions = ["Bool"];

    private const string BulletList = "    * ";

    private const string ComplexFieldAlwaysAddSuffix            = "AlwaysAddStringBearer";
    private const string ComplexFieldWhenNonDefaultSuffix       = "WhenNonDefaultStringBearer";
    private const string ComplexFieldWhenNonNullSuffix          = "WhenNonNullStringBearer";
    private const string ComplexFieldWhenNonNullOrDefaultSuffix = "WhenNonNullOrDefaultStringBearer";

    private const string ComplexCollectionFieldAlwaysAddFilteredSuffix       = "AlwaysAddFilteredStringBearer";
    private const string ComplexCollectionFieldAlwaysAddAllSuffix            = "AlwaysAddAllStringBearer";
    private const string ComplexCollectionFieldWhenPopulatedWithFilterSuffix = "WhenPopulatedWithFilterStringBearer";
    private const string ComplexCollectionFieldWhenPopulatedAddAllSuffix     = "WhenPopulatedAddAllStringBearer";
    private const string ComplexCollectionFieldWhenNonNullAddAllSuffix       = "WhenNonNullAddAllStringBearer";
    private const string ComplexCollectionFieldWhenNonNullAddFilteredSuffix  = "WhenNonNullAddFilteredStringBearer";

    private const string ComplexKeyedCollectionFieldAlwaysAddFilteredSuffix        = "AlwaysAddFilteredStringBearer";
    private const string ComplexKeyedCollectionFieldAlwaysAddAllSuffix             = "AlwaysAddAllStringBearer";
    private const string ComplexKeyedCollectionFieldWithSelectKeysStripOut         = "WithSelectKeys";
    private const string ComplexKeyedCollectionFieldAlwaysAddAllStripOut           = "AlwaysAddAll";
    private const string ComplexKeyedCollectionFieldAlwaysAddFilteredStripOut      = "AlwaysAddFiltered";
    private const string ComplexKeyedCollectionFieldWhenNonNullAddAllStripOut      = "WhenNonNullAddAll";
    private const string ComplexKeyedCollectionFieldWhenNonNullAddFilteredStripOut = "WhenNonNullAddFiltered";
    private const string ComplexKeyedCollectionFieldWhenPopulatedWithFilterStripOut = "WhenPopulatedWithFilter";
    private const string ComplexKeyedCollectionFieldWhenNonNullAddStripOut         = "WhenNonNullAdd";
    private const string ComplexKeyedCollectionFieldWhenPopulatedStripOut          = "WhenPopulated";
    private const string ComplexKeyedCollectionFieldAlwaysStripOut                 = "Always";
    private const string ComplexKeyedCollectionFieldStringBearerStripOut           = "StringBearer";
    private const string ComplexKeyedCollectionFieldWhenPopulatedAddAllSuffix      = "WhenPopulatedAddAllStringBearer";
    private const string ComplexKeyedCollectionFieldWhenNonNullAddAllSuffix        = "WhenNonNullAddAllStringBearer";
    private const string ComplexKeyedCollectionFieldWhenNonNullAddFilteredSuffix   = "WhenNonNullAddFilteredStringBearer";

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

        logger = FLog.FLoggerForType.As<IVersatileFLogger>();
    }


    [TestMethod]
    public void ComplexFieldAlwaysAddFieldCloseToWhenNonDefaultScaffoldingTypes()
    {
        var alwaysAddInvokers       = scafReg.ComplexTypeFieldAlwaysAddFilter().ToList();
        var alwaysAddUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type Single Value Field -  Always Add Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddInvoker in alwaysAddInvokers)
        {
            var uniquePart = alwaysAddInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldAlwaysAddSuffix, "");
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddInvokers.Count).FinalAppend("");

        var nonDefaultInvokers       = scafReg.ComplexTypeFieldWhenNonDefaultAddFilter().ToList();
        var nonDefaultUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Single Value Field -  When Non Default Scaffolding Classes - ")?.AppendLine();
        foreach (var nonDefaultInvoker in nonDefaultInvokers)
        {
            var uniquePart = nonDefaultInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldWhenNonDefaultSuffix, "");
            var countExisting = nonDefaultUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(nonDefaultInvoker.Name).Append(" - ").AppendLine(countExisting);
            nonDefaultUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(nonDefaultInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotNonNullOrDefault = alwaysAddUniqueNamePart.Except(nonDefaultUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Single Value Field -  Found in Always Add but not Non Default - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotNonNullOrDefault)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(alwaysAddInvokers.Count - complexFieldAllowedNonDefaultExemptions.Length, nonDefaultInvokers.Count);
        Assert.AreEqual(0, inAlwaysAddButNotNonNullOrDefault.Except(complexFieldAllowedNonDefaultExemptions).Count());
    }


    [TestMethod]
    public void ComplexFieldAlwaysAddFieldCloseToWhenNonNullScaffoldingTypes()
    {
        var alwaysAddInvokers       = scafReg.ComplexTypeFieldAlwaysAddFilter().ToList();
        var alwaysAddUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type Single Value Field -  AlwaysAdd Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddInvoker in alwaysAddInvokers)
        {
            var uniquePart = alwaysAddInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldAlwaysAddSuffix, "");
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddInvokers.Count).FinalAppend("");

        var nonNullInvokers       = scafReg.ComplexTypeFieldWhenNonNullAddFilter().ToList();
        var nonNullUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Single Value Field -  WhenNonNull Scaffolding Classes - ")?.AppendLine();
        foreach (var nonNullInvoker in nonNullInvokers)
        {
            var uniquePart = nonNullInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldWhenNonNullSuffix, "");
            var countExisting = nonNullUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(nonNullInvoker.Name).Append(" - ").AppendLine(countExisting);
            nonNullUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(nonNullInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotNonNull = alwaysAddUniqueNamePart.Except(nonNullUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Single Value Field -  Found in AlwaysAdd but not NonNull - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotNonNull)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");
        Assert.AreEqual(alwaysAddInvokers.Count - complexFieldAllowedNonNullExemptions.Length, nonNullInvokers.Count);

        var inNonNullAddButNotInAlwaysAdd = nonNullUniqueNamePart.Except(alwaysAddUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Single Value Field -  Found in NonNull but not AlwaysAdd - ")?.AppendLine();
        foreach (var uniqueNamePart in inNonNullAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");


        Assert.AreEqual(0, inAlwaysAddButNotNonNull.Except(complexFieldAllowedNonNullExemptions).Count());
    }

    [TestMethod]
    public void ComplexFieldAlwaysAddFieldCloseToWhenNonNullOrDefaultScaffoldingTypes()
    {
        var alwaysAddInvokers       = scafReg.ComplexTypeFieldAlwaysAddFilter().ToList();
        var alwaysAddUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type Single Value Field -  AlwaysAdd Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddInvoker in alwaysAddInvokers)
        {
            var uniquePart = alwaysAddInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldAlwaysAddSuffix, "");
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddInvokers.Count).FinalAppend("");

        var nonNullOrDefaultInvokers       = scafReg.ComplexTypeFieldWhenNonNullOrDefaultAddFilter().ToList();
        var nonNullOrDefaultUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Single Value Field -  WhenNonNullOrDefault Scaffolding Classes - ")?.AppendLine();
        foreach (var nonNullOrDefaultInvoker in nonNullOrDefaultInvokers)
        {
            var uniquePart = nonNullOrDefaultInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldWhenNonNullOrDefaultSuffix, "");
            var countExisting = nonNullOrDefaultUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(nonNullOrDefaultInvoker.Name).Append(" - ").AppendLine(countExisting);
            nonNullOrDefaultUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(nonNullOrDefaultInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotNonNullOrDefault = alwaysAddUniqueNamePart.Except(nonNullOrDefaultUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Single Value Field -  Found in AlwaysAdd but not NonNullOrDefault - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotNonNullOrDefault)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddButNotNonNullOrDefault.Except(complexFieldAllowedNonNullOrDefaultExemptions).Count());

        var inNonNullOrDefaultAddButNotInAlwaysAdd = nonNullOrDefaultUniqueNamePart.Except(alwaysAddUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Single Value Field -  Found in NonNullOrDefault but not AlwaysAdd - ")?.AppendLine();
        foreach (var uniqueNamePart in inNonNullOrDefaultAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(alwaysAddInvokers.Count - complexFieldAllowedNonNullOrDefaultExemptions.Length, nonNullOrDefaultInvokers.Count);
    }

    [TestMethod]
    public void ComplexCollectionFieldAlwaysAddAllScaffoldingCloseToAlwaysAddFilteredScaffoldingTypes()
    {
        var alwaysAddAllInvokers    = scafReg.ComplexTypeCollectionFieldAlwaysAddAllFilter().ToList();
        var alwaysAddUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type Collection Fields -  AlwaysAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddAllInvoker in alwaysAddAllInvokers)
        {
            var uniquePart = alwaysAddAllInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddAllSuffix, "");
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddAllInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var alwaysAddFilteredInvokers       = scafReg.ComplexTypeCollectionFieldAlwaysAddFilteredFilter().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  AlwaysAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysFilteredInvoker in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddFilteredSuffix, "");
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddFilteredInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotInFiltered = alwaysAddUniqueNamePart.Except(alwaysAddFilteredUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddAll  but not in AlwaysAddFiltered - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAdd = alwaysAddFilteredUniqueNamePart.Except(alwaysAddUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in AlwaysAddFiltered but not AlwaysAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAdd.Count());

        Assert.AreEqual(alwaysAddAllInvokers.Count, alwaysAddFilteredInvokers.Count);
    }

    [TestMethod]
    public void ComplexCollectionFieldAlwaysAddAllScaffoldingCloseToWhenNonNullAddFilteredScaffoldingTypes()
    {
        var alwaysAddAllInvokers    = scafReg.ComplexTypeCollectionFieldAlwaysAddAllFilter().ToList();
        var alwaysAddUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type Collection Fields -  AlwaysAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddAllInvoker in alwaysAddAllInvokers)
        {
            var uniquePart = alwaysAddAllInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddAllSuffix, "");
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddAllInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var nonNullAddFilteredInvokers       = scafReg.ComplexTypeCollectionFieldWhenNonNullAddFilteredFilter().ToList();
        var nonNullAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  NonNullAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var nonNullAddFiltered in nonNullAddFilteredInvokers)
        {
            var uniquePart = nonNullAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldWhenNonNullAddFilteredSuffix, "");
            var countExisting = nonNullAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(nonNullAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            nonNullAddFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(nonNullAddFilteredInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotInFiltered = alwaysAddUniqueNamePart.Except(nonNullAddFilteredUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddAll  but not in NonNullAddFiltered - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAdd = nonNullAddFilteredUniqueNamePart.Except(alwaysAddUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in NonNullAddFiltered but not AlwaysAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAdd.Count());

        Assert.AreEqual(alwaysAddAllInvokers.Count, nonNullAddFilteredInvokers.Count);
    }

    [TestMethod]
    public void ComplexCollectionFieldAlwaysAddAllScaffoldingCloseToWhenNonNullAddAllScaffoldingTypes()
    {
        var alwaysAddAllInvokers    = scafReg.ComplexTypeCollectionFieldAlwaysAddAllFilter().ToList();
        var alwaysAddUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type Collection Fields -  AlwaysAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddAllInvoker in alwaysAddAllInvokers)
        {
            var uniquePart = alwaysAddAllInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddAllSuffix, "");
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddAllInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var alwaysAddFilteredInvokers       = scafReg.ComplexTypeCollectionFieldWhenNonNullAddAllFilter().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  WhenNonNullAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysFilteredInvoker in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldWhenNonNullAddAllSuffix, "");
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddFilteredInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotInFiltered = alwaysAddUniqueNamePart.Except(alwaysAddFilteredUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddAll  but not in WhenNonNullAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAdd = alwaysAddFilteredUniqueNamePart.Except(alwaysAddUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in WhenNonNullAddAll but not AlwaysAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAdd.Count());

        Assert.AreEqual(alwaysAddAllInvokers.Count, alwaysAddFilteredInvokers.Count);
    }

    [TestMethod]
    public void ComplexCollectionFieldAlwaysAddAllScaffoldingCloseToWhenPopulatedAddAllScaffoldingTypes()
    {
        var alwaysAddAllInvokers    = scafReg.ComplexTypeCollectionFieldAlwaysAddAllFilter().ToList();
        var alwaysAddUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type Collection Fields -  AlwaysAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddAllInvoker in alwaysAddAllInvokers)
        {
            var uniquePart = alwaysAddAllInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddAllSuffix, "");
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddAllInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var alwaysAddFilteredInvokers       = scafReg.ComplexTypeCollectionFieldWhenPopulatedAddAllFilter().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  WhenPopulatedAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysFilteredInvoker in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldWhenPopulatedAddAllSuffix, "");
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddFilteredInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotInFiltered = alwaysAddUniqueNamePart.Except(alwaysAddFilteredUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddAll  but not in WhenPopulatedAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAdd = alwaysAddFilteredUniqueNamePart.Except(alwaysAddUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in WhenPopulatedAddAll but not AlwaysAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAdd.Count());

        Assert.AreEqual(alwaysAddAllInvokers.Count, alwaysAddFilteredInvokers.Count);
    }

    [TestMethod]
    public void ComplexCollectionFieldAlwaysAddFilteredScaffoldingCloseToWhenPopulatedWithFilterScaffoldingTypes()
    {
        var alwaysAddFilteredInvokers = scafReg.ComplexTypeCollectionFieldAlwaysAddFilteredFilter().ToList();
        var alwaysAddUniqueNamePart   = new List<string>();

        var le = logger.InfoAppend("Complex Type Collection Fields -  AlwaysAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddFilteredSuffix, "");
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddFilteredInvokers.Count).FinalAppend("");

        var whenPopulatedWithFilteredInvokers     = scafReg.ComplexTypeCollectionFieldWhenPopulatedWithFilter().ToList();
        var whenPopulatedWithFilterUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  WhenPopulatedWithFilter Scaffolding Classes - ")?.AppendLine();
        foreach (var whenPopulatedWithFilterInvoker in whenPopulatedWithFilteredInvokers)
        {
            var uniquePart = whenPopulatedWithFilterInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldWhenPopulatedWithFilterSuffix, "");
            var countExisting = whenPopulatedWithFilterUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenPopulatedWithFilterInvoker.Name).Append(" - ").AppendLine(countExisting);
            whenPopulatedWithFilterUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(whenPopulatedWithFilteredInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotInFiltered = alwaysAddUniqueNamePart.Except(whenPopulatedWithFilterUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddFiltered  but not in WhenPopulatedWithFilter - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAdd = whenPopulatedWithFilterUniqueNamePart.Except(alwaysAddUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in WhenPopulatedWithFilter but not AlwaysAddFiltered - ")?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAdd.Count());

        Assert.AreEqual(whenPopulatedWithFilteredInvokers.Count, whenPopulatedWithFilteredInvokers.Count);
    }

    [TestMethod]
    public void ComplexKeyedCollectionFieldAlwaysAddAllScaffoldingCloseToAlwaysAddFilteredScaffoldingTypes()
    {
        var alwaysAddAllFilteredInvokers = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddAllFilter().ToList();
        var alwaysAddAllUniqueNamePart   = new List<string>();

        var le = logger.InfoAppend("Complex Type KeyedCollection Fields -  AlwaysAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddAllFilteredInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var alwaysAddFilteredInvokers       = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddFiltered().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  AlwaysAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFilteredInvoker in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysAddFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddFilteredInvokers.Count).FinalAppend("");

        var inAlwaysAddAllButNotInFiltered = alwaysAddAllUniqueNamePart.Except(alwaysAddFilteredUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddAll  but not in AlwaysAddFiltered - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddAllButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddAllButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAddAll = alwaysAddFilteredUniqueNamePart.Except(alwaysAddAllUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in AlwaysAddFiltered but not AlwaysAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAddAll)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAddAll.Count());

        Assert.AreEqual(alwaysAddFilteredInvokers.Count, alwaysAddFilteredInvokers.Count);
    }

    [TestMethod]
    public void ComplexKeyedCollectionFieldAlwaysAddFilteredScaffoldingCloseToWhenNonNullAddFilteredScaffoldingTypes()
    {
        var alwaysAddAllFilteredInvokers = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddAllFilter().ToList();
        var alwaysAddAllUniqueNamePart   = new List<string>();

        var le = logger.InfoAppend("Complex Type KeyedCollection Fields -  AlwaysAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddAllFilteredInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenNonNullAddFilteredInvokers       = scafReg.ComplexTypeKeyedCollectionFieldWhenNonNullAddFiltered().ToList();
        var whenNonNullAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  WhenNonNullAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var whenNonNullAddFilteredInvoker in whenNonNullAddFilteredInvokers)
        {
            var uniquePart = whenNonNullAddFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = whenNonNullAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenNonNullAddFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            whenNonNullAddFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(whenNonNullAddFilteredInvokers.Count).FinalAppend("");

        var inAlwaysAddFilteredButNotInNonNullFiltered = alwaysAddAllUniqueNamePart.Except(whenNonNullAddFilteredUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddFiltered  but not in WhenNonNullAddFiltered - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddFilteredButNotInNonNullFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddFilteredButNotInNonNullFiltered.Count());

        var inNonNullFilteredButAlwaysAddFiltered = whenNonNullAddFilteredUniqueNamePart.Except(alwaysAddAllUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in WhenNonNullAddFiltered but not AlwaysAddFiltered - ")?.AppendLine();
        foreach (var uniqueNamePart in inNonNullFilteredButAlwaysAddFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inNonNullFilteredButAlwaysAddFiltered.Count());

        Assert.AreEqual(whenNonNullAddFilteredInvokers.Count, whenNonNullAddFilteredInvokers.Count);
    }

    [TestMethod]
    public void ComplexKeyedCollectionFieldAlwaysAddAllScaffoldingCloseToWhenNonNullAddAllScaffoldingTypes()
    {
        var alwaysAddAllFilteredInvokers = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddAllFilter().ToList();
        var alwaysAddAllUniqueNamePart   = new List<string>();

        var le = logger.InfoAppend("Complex Type KeyedCollection Fields -  AlwaysAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddAllFilteredInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenNonNullAddAllInvokers       = scafReg.ComplexTypeKeyedCollectionFieldWhenNonNullAddAllFilter().ToList();
        var whenNonNullAddAllUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  AlwaysAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var whenNonNullInvoker in whenNonNullAddAllInvokers)
        {
            var uniquePart = whenNonNullInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = whenNonNullAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenNonNullInvoker.Name).Append(" - ").AppendLine(countExisting);
            whenNonNullAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(whenNonNullAddAllInvokers.Count).FinalAppend("");

        var inAlwaysAddAllButNotInWhenNonNull = alwaysAddAllUniqueNamePart.Except(whenNonNullAddAllUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddAll  but not in WhenNonNullAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddAllButNotInWhenNonNull)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddAllButNotInWhenNonNull.Count());

        var inWhenNonNullButNotInAlwaysAddAll = whenNonNullAddAllUniqueNamePart.Except(alwaysAddAllUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in WhenNonNullAddAll but not AlwaysAddAll - ")?.AppendLine();
        foreach (var uniqueNamePart in inWhenNonNullButNotInAlwaysAddAll)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inWhenNonNullButNotInAlwaysAddAll.Count());

        Assert.AreEqual(whenNonNullAddAllInvokers.Count, whenNonNullAddAllInvokers.Count);
    }

    [TestMethod]
    public void ComplexKeyedCollectionFieldAlwaysAddFilteredScaffoldingCloseToWhenPopulatedWithFilterScaffoldingTypes()
    {
        var alwaysAddAllFilteredInvokers = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddFiltered().ToList();
        var alwaysAddAllUniqueNamePart   = new List<string>();

        var le = logger.InfoAppend("Complex Type KeyedCollection Fields -  AlwaysAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddAllFilteredInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenPopulatedWithFilterInvokers       = scafReg.ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilter().ToList();
        var whenPopulatedWithFilterUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  WhenPopulatedWithFilter Scaffolding Classes - ")?.AppendLine();
        foreach (var whenPopulatedWithFilterInvoker in whenPopulatedWithFilterInvokers)
        {
            var uniquePart = whenPopulatedWithFilterInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = whenPopulatedWithFilterUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenPopulatedWithFilterInvoker.Name).Append(" - ").AppendLine(countExisting);
            whenPopulatedWithFilterUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(whenPopulatedWithFilterInvokers.Count).FinalAppend("");

        var inAlwaysAddAllButNotInFiltered = alwaysAddAllUniqueNamePart.Except(whenPopulatedWithFilterUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddFiltered  but not in WhenPopulatedWithFilter - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddAllButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddAllButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAddAll = whenPopulatedWithFilterUniqueNamePart.Except(alwaysAddAllUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in WhenPopulatedWithFilter but not AlwaysAddFiltered - ")?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAddAll)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAddAll.Count());

        Assert.AreEqual(whenPopulatedWithFilterInvokers.Count, whenPopulatedWithFilterInvokers.Count);
    }

    [TestMethod]
    public void ComplexKeyedCollectionFieldAlwaysAddWithSelectKeysScaffoldingCloseToWhenNonNullAddWithSelectKeysScaffoldingTypes()
    {
        var alwaysAddAllFilteredInvokers = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddSelectKeysFilter().ToList();
        var alwaysAddAllUniqueNamePart   = new List<string>();

        var le = logger.InfoAppend("Complex Type KeyedCollection Fields -  AlwaysAddWithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddAllFilteredInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenNonNullWithSelectKeysInvokers = scafReg.ComplexTypeKeyedCollectionFieldWhenNonNullAddSelectKeysFilter().ToList();
        var alwaysAddSelectKeysUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  WhenNonNullWithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysWithSelectKeysInvoker in whenNonNullWithSelectKeysInvokers)
        {
            var uniquePart = alwaysWithSelectKeysInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = alwaysAddSelectKeysUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysWithSelectKeysInvoker.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddSelectKeysUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(whenNonNullWithSelectKeysInvokers.Count).FinalAppend("");

        var inAlwaysAddAllButNotInFiltered = alwaysAddAllUniqueNamePart.Except(alwaysAddSelectKeysUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddWithSelectKeys  but not in WhenNonNullWithSelectKeys - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddAllButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddAllButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAddAll = alwaysAddSelectKeysUniqueNamePart.Except(alwaysAddAllUniqueNamePart);

        counter = 0;
        le = logger.WarnAppend("Complex Type Collection Fields -  Found in WhenNonNullWithSelectKeys but not AlwaysAddWithSelectKeys - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAddAll)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAddAll.Count());

        Assert.AreEqual(whenNonNullWithSelectKeysInvokers.Count, whenNonNullWithSelectKeysInvokers.Count);
    }

    [TestMethod]
    public void ComplexKeyedCollectionFieldAlwaysAddWithSelectKeysScaffoldingCloseToWhenPopulatedWithSelectKeysScaffoldingTypes()
    {
        var alwaysAddAllFilteredInvokers = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddSelectKeysFilter().ToList();
        var alwaysAddAllUniqueNamePart   = new List<string>();

        var le = logger.InfoAppend("Complex Type KeyedCollection Fields -  AlwaysAddWithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddAllFilteredInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenPopulatedWithSelectKeysInvokers       = scafReg.ComplexTypeKeyedCollectionFieldPopulatedWithSelectKeysFilter().ToList();
        var whenPopulatedWithSelectKeysUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  WhenNonNullWithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var whenPopulatedWithKeysInvoker in whenPopulatedWithSelectKeysInvokers)
        {
            var uniquePart = whenPopulatedWithKeysInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup);
            var countExisting = whenPopulatedWithSelectKeysUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenPopulatedWithKeysInvoker.Name).Append(" - ").AppendLine(countExisting);
            whenPopulatedWithSelectKeysUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(whenPopulatedWithSelectKeysInvokers.Count).FinalAppend("");

        var inAlwaysWithSelectKeysAllButNotWhenPopulated = alwaysAddAllUniqueNamePart.Except(whenPopulatedWithSelectKeysUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AlwaysAddWithSelectKeys  but not in WhenPopulatedWithSelectKeys - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysWithSelectKeysAllButNotWhenPopulated)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysWithSelectKeysAllButNotWhenPopulated.Count());

        var inPopulatedAddButNotInAlwaysWithSelectKeys = whenPopulatedWithSelectKeysUniqueNamePart.Except(alwaysAddAllUniqueNamePart);

        counter = 0;
        le = logger.WarnAppend("Complex Type Collection Fields -  Found in WhenPopulatedWithSelectKeys but not AlwaysAddWithSelectKeys - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in inPopulatedAddButNotInAlwaysWithSelectKeys)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inPopulatedAddButNotInAlwaysWithSelectKeys.Count());

        Assert.AreEqual(whenPopulatedWithSelectKeysInvokers.Count, whenPopulatedWithSelectKeysInvokers.Count);
    }
}
