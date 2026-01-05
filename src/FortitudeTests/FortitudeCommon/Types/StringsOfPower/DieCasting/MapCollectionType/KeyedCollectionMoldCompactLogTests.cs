// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.MapCollectionsFieldsTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

[TestClass]
public partial class KeyedCollectionMoldTests
{
    private static IReadOnlyList<ScaffoldingPartEntry> scafReg = ScaffoldingRegistry.AllScaffoldingTypes;
    
    private static IVersatileFLogger logger = null!;

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

        logger = FLog.FLoggerForType.As<IVersatileFLogger>();
    }

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data)
    {
        return $"{methodInfo.Name}_{(((IFormatExpectation)data[0]).ShortTestName)}_{((ScaffoldingPartEntry)data[1]).Name}";
    }

    private static IEnumerable<object[]> SimpleUnfilteredDictExpect =>
        from kce in SimpleDictTestData.AllUnfilteredSimpleDictExpectations
        where !kce.HasRestrictingPredicateFilter
        from scaffoldToCall in 
            scafReg
                .IsKeyedCollectionType()
                .NotHasSupportsKeyRevealer()
                .NotHasSupportsValueRevealer()
                .NoFilterPredicate()
                .NoSubsetListFilter()
        where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()    
        select new object[] { kce, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(SimpleUnfilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleUnfilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> SimplePredicateFilteredDictExpect =>
        from kce in SimpleDictTestData.AllPredicateFilteredSimpleDictExpectations
        from scaffoldToCall in
            scafReg
                .IsKeyedCollectionType()
                .NotHasSupportsKeyRevealer()
                .NotHasSupportsValueRevealer()
                .HasFilterPredicate()
                .NoSubsetListFilter()
        where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()    
        select new object[] { kce, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(SimplePredicateFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimplePredicateFilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> SimpleSubListFilteredDictExpect =>
        from kce in SimpleDictTestData.AllSubListFilteredDictExpectations
        from scaffoldToCall in
            scafReg
                .IsKeyedCollectionType()
                .NotHasSupportsKeyRevealer()
                .NotHasSupportsValueRevealer()
                .HasSubsetListFilter()
                .NoFilterPredicate()
        select new object[] { kce, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(SimpleSubListFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleSubListFilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> ValueRevealerUnfilteredDict =>
        (from kce in ValueRevealerDictTestData.AllValueRevealerUnfilteredDictExpectations
            where kce.ValueTypeIsNotNullableStruct
            from scaffoldToCall in
                scafReg
                    .IsKeyedCollectionType()
                    .NotHasSupportsKeyRevealer()
                    .HasSupportsValueRevealer()
                    .NoFilterPredicate()
                    .AcceptsStructClassNullableClass()
                    .NoSubsetListFilter()
            where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
            select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in ValueRevealerDictTestData.AllValueRevealerUnfilteredDictExpectations
                where kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .NotHasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .AcceptsNullableStructs()
                        .NoSubsetListFilter()
                where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               );

    [TestMethod]
    [DynamicData(nameof(ValueRevealerUnfilteredDict), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerUnfilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> ValueRevealerPredicateFilteredDictExpect =>
        (from kce in ValueRevealerDictTestData.AllPredicateFilteredDictExpectations
         where kce.ValueTypeIsNotNullableStruct
         from scaffoldToCall in
            scafReg
                .IsKeyedCollectionType()
                .NotHasSupportsKeyRevealer()
                .HasSupportsValueRevealer()
                .HasFilterPredicate()
                .AcceptsStructClassNullableClass()
                .NoSubsetListFilter()
        where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()    
        select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in ValueRevealerDictTestData.AllPredicateFilteredDictExpectations
                where kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .NotHasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasFilterPredicate()
                        .AcceptsNullableStructs()
                        .NoSubsetListFilter()
                where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               );

    [TestMethod]
    [DynamicData(nameof(ValueRevealerPredicateFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerPredicateFilteredCompactLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> ValueRevealerSubListFilteredDictExpect =>
        (from kce in ValueRevealerDictTestData.AllValueRevealerSubListFilteredDictExpectations
            where kce.ValueTypeIsNotNullableStruct
        from scaffoldToCall in
            scafReg
                .IsKeyedCollectionType()
                .NotHasSupportsKeyRevealer()
                .HasSupportsValueRevealer()
                .HasSubsetListFilter()
                .NoFilterPredicate()
                .AcceptsStructClassNullableClass()
        select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in ValueRevealerDictTestData.AllValueRevealerSubListFilteredDictExpectations
                where kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .NotHasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasSubsetListFilter()
                        .NoFilterPredicate()
                        .AcceptsNullableStructs()
                select new object[] { kce, scaffoldToCall }
               );

    [TestMethod]
    [DynamicData(nameof(ValueRevealerSubListFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerSubListFilteredCompactLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> BothRevealersUnfilteredDictExpect =>
        (from kce in BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations
         where kce.KeyTypeIsNotNullableStruct && kce.ValueTypeIsNotNullableStruct
         from scaffoldToCall in
            scafReg
                .IsKeyedCollectionType()
                .HasSupportsKeyRevealer()
                .HasSupportsValueRevealer()
                .NoFilterPredicate()
                .NoSubsetListFilter()
                .AcceptsStructClassNullableClass()
                .AcceptsKeyIsNotNullableStruct()
         where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
        select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNotNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsStructClassNullableClass()
                where scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations
                where kce.KeyTypeIsNotNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyIsNotNullableStruct()
                        .AcceptsNullableStructs()
                where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsNullableStructs()
                where scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               );

    [TestMethod]
    [DynamicData(nameof(BothRevealersUnfilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersUnfilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> BothRevealersPredicateFilteredDictExpect =>
        (from kce in BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations
         where kce.KeyTypeIsNotNullableStruct && kce.ValueTypeIsNotNullableStruct
         from scaffoldToCall in
            scafReg
                .IsKeyedCollectionType()
                .HasSupportsKeyRevealer()
                .HasSupportsValueRevealer()
                .HasFilterPredicate()
                .NoSubsetListFilter()
                .AcceptsStructClassNullableClass()
                .AcceptsKeyIsNotNullableStruct()
         where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()   
        select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNotNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsStructClassNullableClass()
                where scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()   
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations
                where kce.KeyTypeIsNotNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyIsNotNullableStruct()
                        .AcceptsNullableStructs()
                where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsNullableStructs()
                where scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()   
                select new object[] { kce, scaffoldToCall }
               );

    [TestMethod]
    [DynamicData(nameof(BothRevealersPredicateFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersPredicateFilteredCompactLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> BothRevealersSubListFilteredDictExpect =>
        (from kce in BothRevealersDictTestData.AllBothRevealersSubListFilteredDictExpectations
            where kce.KeyTypeIsNotNullableStruct && kce.ValueTypeIsNotNullableStruct
            from scaffoldToCall in
                scafReg
                    .IsKeyedCollectionType()
                    .HasSupportsKeyRevealer()
                    .HasSupportsValueRevealer()
                    .NoFilterPredicate()
                    .HasSubsetListFilter()
                    .AcceptsStructClassNullableClass()
                    .AcceptsKeyIsNotNullableStruct()
            select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersSubListFilteredDictExpectations
                where kce.KeyTypeIsNotNullableStruct && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    scafReg
                        .IsKeyedCollectionType()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .HasSubsetListFilter()
                        .AcceptsKeyIsNotNullableStruct()
                        .AcceptsNullableStructs()
                select new object[] { kce, scaffoldToCall }
               );

    [TestMethod]
    [DynamicData(nameof(BothRevealersSubListFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersSubListFilteredCompactLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    public void CompactLogDictionaryTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        SharedCompactLog(BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations[2], ScaffoldingRegistry.AllScaffoldingTypes[958]);
    }

    private void SharedCompactLog(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Keyed Collection Type  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(scaffoldingToCall.ScaffoldingFlags.ToString("F").Replace(",", " |"))
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");
            
        logger.InfoAppend("To Debug Test past the following code into ")?
              .Append(nameof(SharedCompactLog)).Append("()\n\n")
              .Append("SharedCompactLog(")
              .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
        
        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Compact | Log);

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string compactLogTemplate = "{0} {1}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "";
            }
            return string.Format(compactLogTemplate, className, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            { expectValue = ""; }
            return expectValue;
        }

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildChildExpectedOutput;
        }
        tos.Clear();
        var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, tos.Settings);
        stringBearer.RevealState(tos);
        var buildExpectedOutput =
            BuildExpectedOutput
                (stringBearer.GetType().CachedCSharpNameNoConstraints()
               , ((ISinglePropertyTestStringBearer)stringBearer).PropertyName
               , scaffoldingToCall.ScaffoldingFlags
               , formatExpectation).MakeWhiteSpaceVisible();
        var result = tos.WriteBuffer.ToString().MakeWhiteSpaceVisible();
        if (buildExpectedOutput != result)
        {
            logger.ErrorAppend("Result Did not match Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .AppendLine("Expected it to match -")
                  .AppendLine(buildExpectedOutput)
                  .FinalAppend("");
        }
        else
        {
            logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .FinalAppend("");
        }
        Assert.AreEqual(buildExpectedOutput, result, $"Difference at i={buildExpectedOutput.DiffPosition(result)}");
    }
}


//KeyedFromDictionaryFormatStringsAddFilteredStringBearer<IPAddress,·Uri,·IPAddress,·Uri>·{············0.0.0.0:·==>·http://first-null.com/,··········127.0.0.1:·==>·tcp://localhost/,····255.255.255.255:·==>·http://unknown.com/,········192.168.1.1:·==>·tcp://default-gateway/·}