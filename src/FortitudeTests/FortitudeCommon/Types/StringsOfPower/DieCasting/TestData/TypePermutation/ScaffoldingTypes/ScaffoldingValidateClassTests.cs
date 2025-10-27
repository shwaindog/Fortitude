// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

[NoMatchingProductionClass]
[TestClass]
public class ScaffoldingValidateClassTests
{
    private IReadOnlyList<ScaffoldingPartEntry> scafReg = ScaffoldingRegistry.AllScaffoldingTypes;

    private static IVersatileFLogger logger = null!;

    public static readonly string[] CommonNameCleanup = ["`1", "`2", "`3", "`4", "`5", "`6"];

    public readonly string[] ClassNameCleanup = [..CommonNameCleanup, "Field", "OrderedFrom"];
    public readonly string[] WithSelectKeysNameCleanup =
    [
        KeyedCollectionScaffoldNameAddWithSelectKeysStripOut
      , KeyedCollectionScaffoldNameAlwaysAddAllStripOut
      , KeyedCollectionScaffoldNameAlwaysAddFilteredStripOut
      , KeyedCollectionScaffoldNameWhenNonNullAddAllStripOut
      , KeyedCollectionScaffoldNameWhenNonNullAddFilteredStripOut
      , KeyedCollectionScaffoldNameWhenPopulatedWithFilterStripOut
      , KeyedCollectionScaffoldNameWhenPopulatedStripOut
      , KeyedCollectionScaffoldNameWhenNonNullStripOut
      , KeyedCollectionScaffoldNameWithSelectKeysStripOut
      , KeyedCollectionScaffoldNameAddAllStripOut
      , KeyedCollectionScaffoldNameAlwaysStripOut
      , KeyedCollectionScaffoldNameKeyedFromStripOut
      , KeyedCollectionScaffoldNameKeyValueStripOut
      , KeyedCollectionScaffoldNameStringBearerStripOut
    ];
    

    private readonly string[] complexFieldAllowedNonDefaultExemptions       = [
        "NullableBool"
      // , "NullableSpanFormattable"
      // , "SpanFormattable"
      // , "String"
    ];
    private readonly string[] complexFieldAllowedNonNullExemptions          = [
        "Bool"
      // , "NullableSpanFormattable"
      // , "SpanFormattable"
      // , "String"
    ];
    private readonly string[] complexFieldAllowedNonNullOrDefaultExemptions = [
        "Bool"
      // , "NullableSpanFormattable"
      // , "SpanFormattable"
      // , "String"
    ];

    private const string BulletList = "    * ";

    private const string ComplexFieldAlwaysAddSuffix            = "AlwaysAddStringBearer";
    private const string ComplexFieldWhenNonDefaultSuffix       = "WhenNonDefaultStringBearer";
    private const string ComplexFieldWhenNonNullSuffix          = "WhenNonNullStringBearer";
    private const string ComplexFieldWhenNonNullOrDefaultSuffix = "WhenNonNullOrDefaultStringBearer";


    private const string OrderedCollectionAddAllSuffix      = "AllSimpleOrderedCollectionStringBearer";
    private const string OrderedCollectionAddFilteredSuffix = "FilteredSimpleOrderedCollectionStringBearer";

    private const string KeyedCollectionAllSuffix         = "AllStringBearer";
    private const string KeyedCollectionAddFilteredSuffix = "FilteredStringBearer";

    private const string ComplexCollectionFieldAlwaysAddFilteredSuffix       = "AlwaysAddFilteredStringBearer";
    private const string ComplexCollectionFieldAlwaysAddAllSuffix            = "AlwaysAddAllStringBearer";
    private const string ComplexCollectionFieldWhenPopulatedWithFilterSuffix = "WhenPopulatedWithFilterStringBearer";
    private const string ComplexCollectionFieldWhenPopulatedAddAllSuffix     = "WhenPopulatedAddAllStringBearer";
    private const string ComplexCollectionFieldWhenNonNullAddAllSuffix       = "WhenNonNullAddAllStringBearer";
    private const string ComplexCollectionFieldWhenNonNullAddFilteredSuffix  = "WhenNonNullAddFilteredStringBearer";

    private const string SimpleAsValuePrefix  = "SimpleAsValue";
    private const string SimpleAsStringPrefix = "SimpleAsString";

    private const string SimpleValueTypeSuffix = "SimpleValueTypeStringBearer";

    private const string KeyedCollectionScaffoldNameAddWithSelectKeysStripOut       = "AddWithSelectKeys";
    private const string KeyedCollectionScaffoldNameAlwaysAddAllStripOut            = "AlwaysAddAll";
    private const string KeyedCollectionScaffoldNameAlwaysAddFilteredStripOut       = "AlwaysAddFiltered";
    private const string KeyedCollectionScaffoldNameWhenNonNullAddAllStripOut       = "WhenNonNullAddAll";
    private const string KeyedCollectionScaffoldNameWhenNonNullAddFilteredStripOut  = "WhenNonNullAddFiltered";
    private const string KeyedCollectionScaffoldNameWhenPopulatedWithFilterStripOut = "WhenPopulatedWithFilter";
    private const string KeyedCollectionScaffoldNameWhenNonNullStripOut             = "WhenNonNull";
    private const string KeyedCollectionScaffoldNameWithSelectKeysStripOut          = "WithSelectKeys";
    private const string KeyedCollectionScaffoldNameAddAllStripOut                  = "AddAll";
    private const string KeyedCollectionScaffoldNameWhenPopulatedStripOut           = "WhenPopulated";
    private const string KeyedCollectionScaffoldNameAlwaysStripOut                  = "Always";
    private const string KeyedCollectionScaffoldNameKeyedFromStripOut               = "KeyedFrom";
    private const string KeyedCollectionScaffoldNameKeyValueStripOut                = "KeyValue";
    private const string KeyedCollectionScaffoldNameStringBearerStripOut            = "StringBearer";

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
        alwaysAddInvokers.Sort();
        var alwaysAddUniqueNamePart = new List<string>();

        var le    = logger.InfoAppend("Complex Type Single Value Field -  Always Add Scaffolding Classes - ")?.AppendLine();
        var count = 0;
        foreach (var alwaysAddInvoker in alwaysAddInvokers)
        {
            var uniquePart = alwaysAddInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldAlwaysAddSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(count, "{0,2}").Append(". ").Append(alwaysAddInvoker.Name).Append(" - ").Append(countExisting).Append("\n");
            le = le?.Append(count++, "{0,2}").Append(". ").Append(uniquePart).Append(" - ").Append(countExisting).Append("\n");
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddInvokers.Count).FinalAppend("");

        var nonDefaultInvokers       = scafReg.ComplexTypeFieldWhenNonDefaultAddFilter().ToList();
        nonDefaultInvokers.Sort();
        var nonDefaultUniqueNamePart = new List<string>();

        count = 0;
        le    = logger.WarnAppend("Complex Type Single Value Field -  When Non Default Scaffolding Classes - ")?.AppendLine();
        foreach (var nonDefaultInvoker in nonDefaultInvokers)
        {
            var uniquePart = nonDefaultInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldWhenNonDefaultSuffix, "").TruncateAt('<');
            var countExisting = nonDefaultUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(count, "{0,2}").Append(". ").Append(nonDefaultInvoker.Name).Append(" - ").Append(countExisting).Append("\n");
            le = le?.Append(count++, "{0,2}").Append(". ").Append(uniquePart).Append(" - ").Append(countExisting).Append("\n");
            nonDefaultUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(nonDefaultInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotNonNullOrDefault = 
            alwaysAddUniqueNamePart
                .Except(nonDefaultUniqueNamePart)
                .Except(complexFieldAllowedNonDefaultExemptions).ToList();

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Single Value Field -  Found in Always Add but not Non Default - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotNonNullOrDefault)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(alwaysAddInvokers.Count - complexFieldAllowedNonDefaultExemptions.Length , nonDefaultInvokers.Count);
        Assert.AreEqual(0, inAlwaysAddButNotNonNullOrDefault.Count);
    }


    [TestMethod]
    public void ComplexFieldAlwaysAddFieldCloseToWhenNonNullScaffoldingTypes()
    {
        var alwaysAddInvokers       = scafReg.ComplexTypeFieldAlwaysAddFilter().ToList();
        alwaysAddInvokers.Sort();
        var alwaysAddUniqueNamePart = new List<string>();

        var count = 0;
        var le    = logger.InfoAppend("Complex Type Single Value Field -  AlwaysAdd Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddInvoker in alwaysAddInvokers)
        {
            var uniquePart = alwaysAddInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldAlwaysAddSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(count, "{0,2}").Append(". ").Append(alwaysAddInvoker.Name).Append(" - ").Append(countExisting).Append("\n");
            le = le?.Append(count++, "{0,2}").Append(". ").Append(uniquePart).Append(" - ").Append(countExisting).Append("\n");
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddInvokers.Count).FinalAppend("");

        var nonNullInvokers       = scafReg.ComplexTypeFieldWhenNonNullAddFilter().ToList();
        nonNullInvokers.Sort();
        var nonNullUniqueNamePart = new List<string>();
        
        count = 0;
        le    = logger.WarnAppend("Complex Type Single Value Field -  WhenNonNull Scaffolding Classes - ")?.AppendLine();
        foreach (var nonNullInvoker in nonNullInvokers)
        {
            var uniquePart = nonNullInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldWhenNonNullSuffix, "").TruncateAt('<');
            var countExisting = nonNullUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(count, "{0,2}").Append(". ").Append(nonNullInvoker.Name).Append(" - ").Append(countExisting).Append("\n");
            le = le?.Append(count, "{0,2}").Append(". ").Append(uniquePart).Append(" - ").Append(countExisting).Append("\n");
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

        var inNonNullAddButNotInAlwaysAdd = nonNullUniqueNamePart
            .Except(alwaysAddUniqueNamePart)
            .Except(complexFieldAllowedNonNullExemptions).ToList();

        counter = 0;
        le      = logger.WarnAppend("Complex Type Single Value Field -  Found in NonNull but not AlwaysAdd - ")?.AppendLine();
        foreach (var uniqueNamePart in inNonNullAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");


        Assert.AreEqual(alwaysAddInvokers.Count - complexFieldAllowedNonNullExemptions.Length , nonNullInvokers.Count);
        Assert.AreEqual(0, inNonNullAddButNotInAlwaysAdd.Count);
    }

    [TestMethod]
    public void ComplexFieldAlwaysAddFieldCloseToWhenNonNullOrDefaultScaffoldingTypes()
    {
        var alwaysAddInvokers       = scafReg.ComplexTypeFieldAlwaysAddFilter().ToList();
        var alwaysAddUniqueNamePart = new List<string>();

        var count = 0;
        var le    = logger.InfoAppend("Complex Type Single Value Field -  AlwaysAdd Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddInvoker in alwaysAddInvokers)
        {
            var uniquePart = alwaysAddInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldAlwaysAddSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(count, "{0,2}").Append(". ").Append(alwaysAddInvoker.Name).Append(" - ").Append(countExisting).Append("\n");
            le = le?.Append(count++, "{0,2}").Append(". ").Append(uniquePart).Append(" - ").Append(countExisting).Append("\n");
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddInvokers.Count).FinalAppend("");

        var nonNullOrDefaultInvokers       = scafReg.ComplexTypeFieldWhenNonNullOrDefaultAddFilter().ToList();
        var nonNullOrDefaultUniqueNamePart = new List<string>();
        
        count = 0;
        le    = logger.WarnAppend("Complex Type Single Value Field -  WhenNonNullOrDefault Scaffolding Classes - ")?.AppendLine();
        foreach (var nonNullOrDefaultInvoker in nonNullOrDefaultInvokers)
        {
            var uniquePart = nonNullOrDefaultInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexFieldWhenNonNullOrDefaultSuffix, "").TruncateAt('<');
            var countExisting = nonNullOrDefaultUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(nonNullOrDefaultInvoker.Name).Append(" - ").Append(countExisting).Append("\n");
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").Append(countExisting).Append("\n");
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

        var inNonNullOrDefaultAddButNotInAlwaysAdd = nonNullOrDefaultUniqueNamePart
            .Except(alwaysAddUniqueNamePart)
            .Except(complexFieldAllowedNonNullOrDefaultExemptions).ToList();

        counter = 0;
        le      = logger.WarnAppend("Complex Type Single Value Field -  Found in NonNullOrDefault but not AlwaysAdd - ")?.AppendLine();
        foreach (var uniqueNamePart in inNonNullOrDefaultAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(alwaysAddInvokers.Count - complexFieldAllowedNonNullOrDefaultExemptions.Length, nonNullOrDefaultInvokers.Count);
        Assert.AreEqual(0, inNonNullOrDefaultAddButNotInAlwaysAdd.Count);
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
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddAllSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddAllInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var alwaysAddFilteredInvokers       = scafReg.ComplexTypeCollectionFieldAlwaysAddFilteredFilter().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  AlwaysAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysFilteredInvoker in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddFilteredSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddAllSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddAllInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var nonNullAddFilteredInvokers       = scafReg.ComplexTypeCollectionFieldWhenNonNullAddFilteredFilter().ToList();
        var nonNullAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  NonNullAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var nonNullAddFiltered in nonNullAddFilteredInvokers)
        {
            var uniquePart = nonNullAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldWhenNonNullAddFilteredSuffix, "").TruncateAt('<');
            var countExisting = nonNullAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(nonNullAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddAllSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddAllInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var alwaysAddFilteredInvokers       = scafReg.ComplexTypeCollectionFieldWhenNonNullAddAllFilter().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  WhenNonNullAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysFilteredInvoker in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldWhenNonNullAddAllSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddAllSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddAllInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var alwaysAddFilteredInvokers       = scafReg.ComplexTypeCollectionFieldWhenPopulatedAddAllFilter().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  WhenPopulatedAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysFilteredInvoker in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldWhenPopulatedAddAllSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldAlwaysAddFilteredSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddFilteredInvokers.Count).FinalAppend("");

        var whenPopulatedWithFilteredInvokers     = scafReg.ComplexTypeCollectionFieldWhenPopulatedWithFilter().ToList();
        var whenPopulatedWithFilterUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type Collection Fields -  WhenPopulatedWithFilter Scaffolding Classes - ")?.AppendLine();
        foreach (var whenPopulatedWithFilterInvoker in whenPopulatedWithFilteredInvokers)
        {
            var uniquePart = whenPopulatedWithFilterInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(ComplexCollectionFieldWhenPopulatedWithFilterSuffix, "").TruncateAt('<');
            var countExisting = whenPopulatedWithFilterUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenPopulatedWithFilterInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var alwaysAddFilteredInvokers       = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddFiltered().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  AlwaysAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFilteredInvoker in alwaysAddFilteredInvokers)
        {
            var uniquePart = alwaysAddFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
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
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
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
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenNonNullAddAllInvokers       = scafReg.ComplexTypeKeyedCollectionFieldWhenNonNullAddAllFilter().ToList();
        var whenNonNullAddAllUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  AlwaysAddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var whenNonNullInvoker in whenNonNullAddAllInvokers)
        {
            var uniquePart = whenNonNullInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = whenNonNullAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenNonNullInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenPopulatedWithFilterInvokers       = scafReg.ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilter().ToList();
        var whenPopulatedWithFilterUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  WhenPopulatedWithFilter Scaffolding Classes - ")?.AppendLine();
        foreach (var whenPopulatedWithFilterInvoker in whenPopulatedWithFilterInvokers)
        {
            var uniquePart = whenPopulatedWithFilterInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = whenPopulatedWithFilterUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenPopulatedWithFilterInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenNonNullWithSelectKeysInvokers = scafReg.ComplexTypeKeyedCollectionFieldWhenNonNullAddSelectKeysFilter().ToList();
        var alwaysAddSelectKeysUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  WhenNonNullWithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysWithSelectKeysInvoker in whenNonNullWithSelectKeysInvokers)
        {
            var uniquePart = alwaysWithSelectKeysInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = alwaysAddSelectKeysUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysWithSelectKeysInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
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
        var alwaysAddAllFilteredInvokers    = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddSelectKeysFilter().ToList();
        var alwaysAddFilteredUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type KeyedCollection Fields -  AlwaysAddWithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddAllFilteredInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = alwaysAddFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllFilteredInvokers.Count).FinalAppend("");

        var whenPopulatedWithSelectKeysInvokers       = scafReg.ComplexTypeKeyedCollectionFieldPopulatedWithSelectKeysFilter().ToList();
        var whenPopulatedWithSelectKeysUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Complex Type KeyedCollection Fields -  WhenNonNullWithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var whenPopulatedWithKeysInvoker in whenPopulatedWithSelectKeysInvokers)
        {
            var uniquePart = whenPopulatedWithKeysInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = whenPopulatedWithSelectKeysUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(whenPopulatedWithKeysInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            whenPopulatedWithSelectKeysUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(whenPopulatedWithSelectKeysInvokers.Count).FinalAppend("");

        var inAlwaysWithSelectKeysAllButNotWhenPopulated = alwaysAddFilteredUniqueNamePart.Except(whenPopulatedWithSelectKeysUniqueNamePart);

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

        var inPopulatedAddButNotInAlwaysWithSelectKeys = whenPopulatedWithSelectKeysUniqueNamePart.Except(alwaysAddFilteredUniqueNamePart);

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

    [TestMethod]
    public void OrderedCollectionAlwaysAddAllScaffoldingCloseToAddFilteredScaffoldingTypes()
    {
        var alwaysAddAllInvokers       = scafReg.OrderedCollectionAlwaysAddAllFilter().ToList();
        var alwaysAddAllUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("OrderedCollection -  AlwaysAddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in alwaysAddAllInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).Replace(OrderedCollectionAddAllSuffix, "").TruncateAt('<');
            var countExisting = alwaysAddAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            alwaysAddAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(alwaysAddAllInvokers.Count).FinalAppend("");

        var addFilteredInvokers       = scafReg.OrderedCollectionAddFiltered().ToList();
        var addFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("OrderedCollection -  AddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var addFilteredInvoker in addFilteredInvokers)
        {
            var uniquePart = addFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(OrderedCollectionAddFilteredSuffix, "").TruncateAt('<');
            var countExisting = addFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(addFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            addFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(addFilteredInvokers.Count).FinalAppend("");

        var inAlwaysAddAllButNotAddFiltered = alwaysAddAllUniqueNamePart.Except(addFilteredUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("OrderedCollection -  Found in AlwaysAddAll  but not in AddFiltered - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddAllButNotAddFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddAllButNotAddFiltered.Count());

        var addFilteredNotInAddAll = addFilteredUniqueNamePart.Except(alwaysAddAllUniqueNamePart);

        counter = 0;
        le = logger.WarnAppend("OrderedCollection -  Found in AddFiltered but not AlwaysAddAll - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in addFilteredNotInAddAll)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, addFilteredNotInAddAll.Count());

        Assert.AreEqual(addFilteredInvokers.Count, addFilteredInvokers.Count);
    }

    [TestMethod]
    public void KeyedCollectionAlwaysAddAllScaffoldingCloseToAddFilteredScaffoldingTypes()
    {
        var addAllInvokers       = scafReg.KeyedCollectionAlwaysAddAllFilter().ToList();
        var addAllUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("KeyedCollection -  AddAll Scaffolding Classes - ")?.AppendLine();
        foreach (var alwaysAddFiltered in addAllInvokers)
        {
            var uniquePart = alwaysAddFiltered
                             .Name.RemoveAll(ClassNameCleanup).Replace(KeyedCollectionAllSuffix, "").TruncateAt('<');
            var countExisting = addAllUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(alwaysAddFiltered.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            addAllUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(addAllInvokers.Count).FinalAppend("");

        var addFilteredInvokers       = scafReg.KeyedCollectionAddFiltered().ToList();
        var addFilteredUniqueNamePart = new List<string>();

        le = logger.WarnAppend("KeyedCollection -  AddFiltered Scaffolding Classes - ")?.AppendLine();
        foreach (var addFilteredInvoker in addFilteredInvokers)
        {
            var uniquePart = addFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).Replace(KeyedCollectionAddFilteredSuffix, "").TruncateAt('<');
            var countExisting = addFilteredUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(addFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            addFilteredUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(addFilteredInvokers.Count).FinalAppend("");

        var inAddAllButNotAddFiltered = addAllUniqueNamePart.Except(addFilteredUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("KeyedCollection -  Found in AddAll  but not in AddFiltered - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in inAddAllButNotAddFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAddAllButNotAddFiltered.Count());

        var addFilteredNotInAddAll = addFilteredUniqueNamePart.Except(addAllUniqueNamePart);

        counter = 0;
        le = logger.WarnAppend("KeyedCollection -  Found in AddFiltered but not AddAll - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in addFilteredNotInAddAll)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, addFilteredNotInAddAll.Count());

        Assert.AreEqual(addFilteredInvokers.Count, addFilteredInvokers.Count);
    }

    [TestMethod]
    public void ComplexKeyedCollectionFieldAlwaysAddWithSelectKeysCloseToKeyedCollectionAddWithSelectKeysScaffoldingTypes()
    {
        var complexTypeWithSelectKeyeInvokers        = scafReg.ComplexTypeKeyedCollectionFieldAlwaysAddSelectKeysFilter().ToList();
        var complexTypeWithSelectKeyesUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Complex Type KeyedCollection Fields -  AlwaysAddWithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var complexTypeWithSelectKeysField in complexTypeWithSelectKeyeInvokers)
        {
            var uniquePart = complexTypeWithSelectKeysField
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = complexTypeWithSelectKeyesUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(complexTypeWithSelectKeysField.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            complexTypeWithSelectKeyesUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(complexTypeWithSelectKeyeInvokers.Count).FinalAppend("");

        var withSelectKeysInvokers       = scafReg.KeyedCollectionAddWithSelectedKeysFilter().ToList();
        var withSelectKeysUniqueNamePart = new List<string>();

        le = logger.WarnAppend("KeyedCollection -  WithSelectKeys Scaffolding Classes - ")?.AppendLine();
        foreach (var addFilteredInvoker in withSelectKeysInvokers)
        {
            var uniquePart = addFilteredInvoker
                             .Name.RemoveAll(ClassNameCleanup).RemoveAll(WithSelectKeysNameCleanup).TruncateAt('<');
            var countExisting = withSelectKeysUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(addFilteredInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            withSelectKeysUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(withSelectKeysInvokers.Count).FinalAppend("");

        var inAddAllButNotAddWithSelectKeys = complexTypeWithSelectKeyesUniqueNamePart.Except(withSelectKeysUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Found in Complex Type KeyedCollection Fields AlwaysAddWithSelectKeys  but not in KeyedCollection WithSelectKeys - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in inAddAllButNotAddWithSelectKeys)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAddAllButNotAddWithSelectKeys.Count());

        var addWithSelectKeysNotInAddAll = withSelectKeysUniqueNamePart.Except(complexTypeWithSelectKeyesUniqueNamePart);

        counter = 0;
        le = logger.WarnAppend("Found in KeyedCollection WithSelectKeys but not in Complex Type KeyedCollection Fields AlwaysAddWithSelectKeys  - ")
                   ?.AppendLine();
        foreach (var uniqueNamePart in addWithSelectKeysNotInAddAll)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, addWithSelectKeysNotInAddAll.Count());

        Assert.AreEqual(withSelectKeysInvokers.Count, withSelectKeysInvokers.Count);
    }

    [TestMethod]
    public void AllSimpleValueTypeAsValueScaffoldingHasMatchingAsStringScaffolding()
    {
        var allSimpleTypeInvokers = scafReg.IsSimpleType().ToList();
        var allAsValueInvokers    = allSimpleTypeInvokers.Where(spe => spe.Name.Contains("SimpleAsValue")).ToList();
        var asValueUniqueNamePart = new List<string>();

        var le = logger.InfoAppend("Simple Type Fields -  AsValue Scaffolding Classes - ")?.AppendLine();
        foreach (var asValueInvoker in allAsValueInvokers)
        {
            var uniquePart = asValueInvoker
                             .Name.RemoveAll(CommonNameCleanup).Replace(SimpleAsValuePrefix, "").Replace(SimpleValueTypeSuffix, "").TruncateAt('<');
            var countExisting = asValueUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(asValueInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            asValueUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(allAsValueInvokers.Count).FinalAppend("");

        var asStringInvokers       = allSimpleTypeInvokers.Where(spe => spe.Name.Contains("SimpleAsString")).ToList();
        var asStringUniqueNamePart = new List<string>();

        le = logger.WarnAppend("Simple Type Fields -  AsString Scaffolding Classes - ")?.AppendLine();
        foreach (var asStringInvoker in asStringInvokers)
        {
            var uniquePart = asStringInvoker
                             .Name.RemoveAll(CommonNameCleanup).Replace(SimpleAsStringPrefix, "").Replace(SimpleValueTypeSuffix, "").TruncateAt('<');
            var countExisting = asStringUniqueNamePart.Count(s => s == uniquePart);
            le = le?.Append(BulletList).Append(asStringInvoker.Name).Append(" - ").AppendLine(countExisting);
            le = le?.Append(BulletList).Append(uniquePart).Append(" - ").AppendLine(countExisting);
            asStringUniqueNamePart.Add(uniquePart);
        }
        le?.AppendLine().Append("Total ").AppendLine(asStringInvokers.Count).FinalAppend("");

        var inAlwaysAddButNotInFiltered = asValueUniqueNamePart.Except(asStringUniqueNamePart);

        var counter = 0;
        le = logger.ErrorAppend("Complex Type Collection Fields -  Found in AsValue  but not in AsString - ")?.AppendLine();
        foreach (var uniqueNamePart in inAlwaysAddButNotInFiltered)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inAlwaysAddButNotInFiltered.Count());

        var inFilteredAddButNotInAlwaysAdd = asStringUniqueNamePart.Except(asValueUniqueNamePart);

        counter = 0;
        le      = logger.WarnAppend("Complex Type Collection Fields -  Found in AsString but not AsValue - ")?.AppendLine();
        foreach (var uniqueNamePart in inFilteredAddButNotInAlwaysAdd)
        {
            le = le?.Append(BulletList).AppendLine(uniqueNamePart);
            counter++;
        }
        le?.AppendLine().Append("Total ").AppendLine(counter).FinalAppend("");

        Assert.AreEqual(0, inFilteredAddButNotInAlwaysAdd.Count());

        Assert.AreEqual(allAsValueInvokers.Count, asStringInvokers.Count);
    }

    [TestMethod]
    public void AllComplexTypeSingleValueFieldAcceptsCharsScaffoldingImplementISupportsSettingValueFromString()
    {
        var allComplexSingleFieldsThatAcceptChars =
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsChars().NotHasAcceptsAny().ToList();

        foreach (var checkImplementsSettingValueFromString in allComplexSingleFieldsThatAcceptChars)
        {
            try { Assert.IsTrue(checkImplementsSettingValueFromString.SupportsSettingValueFromString); }
            catch (Exception)
            {
                logger.ErrorAppend("Complex Type Accepts Char Fields -  Expected - ")?
                      .AppendLine(checkImplementsSettingValueFromString.Name)
                      .Append(" to implement ")
                      .FinalAppend(nameof(ISupportsSettingValueFromString));

                throw;
            }
        }
    }

    [TestMethod]
    public void AllComplexTypeSingleValueFieldAcceptsCharsScaffoldingImplementISupportsValueFormatString()
    {
        var allComplexSingleFieldsThatAcceptChars =
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsChars().NotHasAcceptsAny().ToList();

        foreach (var checkImplementsSettingValueFromString in allComplexSingleFieldsThatAcceptChars)
        {
            try { Assert.IsTrue(checkImplementsSettingValueFromString.SupportsValueFormatString); }
            catch (Exception)
            {
                logger.ErrorAppend("Complex Type Accepts Char Fields -  Expected - ")?
                      .AppendLine(checkImplementsSettingValueFromString.Name)
                      .Append(" to implement ")
                      .FinalAppend(nameof(ISupportsValueFormatString));

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeWithsSupportsValueFormatStringImplementsISupportsValueFormatString()
    {
        var allSupportsValueFormatString =
            scafReg.HasSupportsValueFormatString().ToList();

        foreach (var checkImplementsSettingValueFormatString in allSupportsValueFormatString)
        {
            try { Assert.IsTrue(checkImplementsSettingValueFormatString.SupportsValueFormatString); }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with SupportsValueFormatString Flag -  Expected - ")?
                      .AppendLine(checkImplementsSettingValueFormatString.Name)
                      .Append(" to implement ")
                      .FinalAppend(nameof(ISupportsValueFormatString));

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeImplementingISupportsValueFormatStringHasSupportsValueFormatString()
    {
        var allSupportsValueFormatString =
            scafReg.Where(spe => spe.SupportsValueFormatString).ToList();

        foreach (var checkImplementsSettingValueFormatString in allSupportsValueFormatString)
        {
            try
            {
                Assert.IsTrue(checkImplementsSettingValueFormatString
                              .ScaffoldingFlags
                              .HasAllOf(SupportsValueFormatString), $"{checkImplementsSettingValueFormatString.Name} does not have \"SupportsValueFormatString\" flag");
            }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with ISupportsValueFormatString -  Expected - ")?
                      .AppendLine(checkImplementsSettingValueFormatString.Name)
                      .Append(" to implement ")
                      .FinalAppend(nameof(ISupportsValueFormatString));

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeWithsSupportsKeyFormatStringImplementsISupportsKeyFormatString()
    {
        var allSupportsKeyFormatString =
            scafReg.HasSupportsKeyFormatString().ToList();

        foreach (var checkImplementsSettingKeyFormatString in allSupportsKeyFormatString)
        {
            try { Assert.IsTrue(checkImplementsSettingKeyFormatString.SupportsKeyFormatString); }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with SupportsKeyFormatString Flag -  Expected - ")?
                      .AppendLine(checkImplementsSettingKeyFormatString.Name)
                      .Append(" to implement ")
                      .FinalAppend(nameof(ISupportsKeyFormatString));

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeImplementingISupportsKeyFormatStringHasSupportsKeyFormatString()
    {
        var allAcceptsKeyFormatString =
            scafReg.Where(spe => spe.SupportsKeyFormatString).ToList();

        foreach (var checkImplementsSettingKeyFormatString in allAcceptsKeyFormatString)
        {
            try
            {
                Assert.IsTrue(checkImplementsSettingKeyFormatString
                              .ScaffoldingFlags
                              .HasAllOf(SupportsKeyFormatString));
            }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with ISupportsKeyFormatString -  Expected - ")?
                      .AppendLine(checkImplementsSettingKeyFormatString.Name)
                      .Append(" to implement ")
                      .FinalAppend(nameof(ISupportsKeyFormatString));

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeWithsSupportsValueRevealerImplementsISupportsValueRevealer()
    {
        var allSupportsValueRevealer =
            scafReg.HasSupportsValueRevealer().ToList();

        foreach (var checkImplementsValueRevealer in allSupportsValueRevealer)
        {
            try { Assert.IsTrue(checkImplementsValueRevealer.SupportsValueRevealer); }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with SupportsValueRevealer Flag -  Expected - ")?
                      .AppendLine(checkImplementsValueRevealer.Name)
                      .Append(" to implement ")
                      .FinalAppend(typeof(ISupportsValueRevealer<>).Name);

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeImplementingISupportsValueRevealerHasSupportsValueRevealer()
    {
        var allSupportsValueRevealer =
            scafReg.Where(spe => spe.SupportsValueRevealer).ToList();

        foreach (var checkImplementsValueRevealer in allSupportsValueRevealer)
        {
            try
            {
                Assert.IsTrue(checkImplementsValueRevealer
                              .ScaffoldingFlags
                              .HasAllOf(SupportsValueRevealer));
            }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with ")?
                      .Append((typeof(ISupportsValueRevealer<>).Name))
                      .Append("  -  Expected - ")
                      .AppendLine(checkImplementsValueRevealer.Name)
                      .Append(" to implement ")
                      .FinalAppend(SupportsValueRevealer);

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeWithsSupportsKeyRevealerImplementsISupportsKeyRevealer()
    {
        var allSupportsKeyRevealer =
            scafReg.HasSupportsKeyRevealer().ToList();

        foreach (var checkImplementsKeyRevealer in allSupportsKeyRevealer)
        {
            try { Assert.IsTrue(checkImplementsKeyRevealer.SupportsKeyRevealer); }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with SupportsKeyRevealer Flag -  Expected - ")?
                      .AppendLine(checkImplementsKeyRevealer.Name)
                      .Append(" to implement ")
                      .FinalAppend(typeof(ISupportsKeyRevealer<>).Name);

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeImplementingISupportsKeyRevealerHasSupportsKeyRevealer()
    {
        var allSupportsKeyRevealer =
            scafReg.Where(spe => spe.SupportsKeyRevealer).ToList();

        foreach (var checkImplementsValueRevealer in allSupportsKeyRevealer)
        {
            try
            {
                Assert.IsTrue(checkImplementsValueRevealer
                              .ScaffoldingFlags
                              .HasAllOf(SupportsKeyRevealer));
            }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with ")?
                      .Append((typeof(ISupportsKeyRevealer<>).Name))
                      .Append("  -  Expected - ")
                      .AppendLine(checkImplementsValueRevealer.Name)
                      .Append(" to implement ")
                      .FinalAppend(SupportsKeyRevealer);

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeWithSupportsIndexSubRangesImplementsISupportsIndexRangeLimiting()
    {
        var allSupportsIndexSubRanges =
            scafReg.HasSupportsIndexSubRanges().ToList();

        foreach (var checkImplementsSupportsIndexSubRanges in allSupportsIndexSubRanges)
        {
            try { Assert.IsTrue(checkImplementsSupportsIndexSubRanges.SupportsIndexRangeLimiting); }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with SupportsIndexSubRanges Flag -  Expected - ")?
                      .AppendLine(checkImplementsSupportsIndexSubRanges.Name)
                      .Append(" to implement ")
                      .FinalAppend(nameof(ISupportsIndexRangeLimiting));

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeImplementingISupportsIndexRangeLimitingHasSupportsIndexSubRanges()
    {
        var allSupportsIndexSubRanges =
            scafReg.Where(spe => spe.SupportsIndexRangeLimiting).ToList();

        foreach (var checkImplementsSupportsIndexSubRanges in allSupportsIndexSubRanges)
        {
            try
            {
                Assert.IsTrue(checkImplementsSupportsIndexSubRanges
                              .ScaffoldingFlags
                              .HasAllOf(SupportsIndexSubRanges));
            }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with ")?
                      .Append((nameof(ISupportsIndexRangeLimiting)))
                      .Append(" ISupportsValueFormatString -  Expected - ")
                      .AppendLine(checkImplementsSupportsIndexSubRanges.Name)
                      .Append(" to implement ")
                      .FinalAppend(SupportsIndexSubRanges);

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyOrderedCollectionWithsFilterPredicateImplementsISupportsOrderedCollectionPredicate()
    {
        var allOrderedCollectionFilterPredicate =
            scafReg.ProcessesCollection().HasFilterPredicate().ToList();

        foreach (var checkImplementsOrderedCollectionPredicate in allOrderedCollectionFilterPredicate)
        {
            try { Assert.IsTrue(checkImplementsOrderedCollectionPredicate.SupportsOrderedCollectionPredicate); }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with FilterPredicate Flag -  Expected - ")?
                      .AppendLine(checkImplementsOrderedCollectionPredicate.Name)
                      .Append(" to implement ")
                      .FinalAppend(typeof(ISupportsOrderedCollectionPredicate<>).Name);

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeImplementingISupportsOrderedCollectionPredicateHasFilterPredicate()
    {
        var allOrderedCollectionFilterPredicate =
            scafReg.Where(spe => spe.SupportsOrderedCollectionPredicate).ToList();

        foreach (var checkImplementsOrderedCollectionPredicate in allOrderedCollectionFilterPredicate)
        {
            try
            {
                Assert.IsTrue(checkImplementsOrderedCollectionPredicate
                              .ScaffoldingFlags
                              .HasAllOf(FilterPredicate | AcceptsCollection));
            }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with ")?
                      .Append((typeof(ISupportsOrderedCollectionPredicate<>).Name))
                      .Append("  -  Expected - ")
                      .AppendLine(checkImplementsOrderedCollectionPredicate.Name)
                      .Append(" to implement ")
                      .FinalAppend(FilterPredicate);

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyKeyedCollectionWithsFilterPredicateImplementsISupportsKeyedCollectionPredicate()
    {
        var allKeyedCollectionFilterPredicate =
            scafReg.ProcessesKeyedCollection().HasFilterPredicate().ToList();

        foreach (var checkImplementsKeyedCollectionPredicate in allKeyedCollectionFilterPredicate)
        {
            try { Assert.IsTrue(checkImplementsKeyedCollectionPredicate.SupportsKeyedCollectionPredicate); }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with FilterPredicate Flag -  Expected - ")?
                      .AppendLine(checkImplementsKeyedCollectionPredicate.Name)
                      .Append(" to implement ")
                      .FinalAppend(typeof(ISupportsKeyedCollectionPredicate<,>).Name);

                throw;
            }
        }
    }

    [TestMethod]
    public void AnyScaffoldingTypeImplementingISupportsKeyedCollectionPredicateHasFilterPredicate()
    {
        var allKeyedCollectionFilterPredicate =
            scafReg.Where(spe => spe.SupportsKeyedCollectionPredicate).ToList();

        foreach (var checkImplementsKeyedCollectionPredicate in allKeyedCollectionFilterPredicate)
        {
            try
            {
                Assert.IsTrue(checkImplementsKeyedCollectionPredicate
                              .ScaffoldingFlags
                              .HasAllOf(FilterPredicate | AcceptsKeyValueCollection));
            }
            catch (Exception)
            {
                logger.ErrorAppend("Any Scaffolding with ")?
                      .Append((typeof(ISupportsKeyedCollectionPredicate<,>).Name))
                      .Append("  -  Expected - ")
                      .AppendLine(checkImplementsKeyedCollectionPredicate.Name)
                      .Append(" to implement ")
                      .FinalAppend(FilterPredicate);

                throw;
            }
        }
    }
}
