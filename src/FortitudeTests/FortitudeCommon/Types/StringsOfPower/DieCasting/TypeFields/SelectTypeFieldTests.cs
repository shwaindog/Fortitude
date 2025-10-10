// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

[TestClass]
public class SelectTypeFieldTests
{
    private IReadOnlyList<ScaffoldingPartEntry> scafReg = ScaffoldingRegistry.AllScaffoldingTypes;
    

    private static IVersatileFLogger logger     = null!;
    private const  string            BulletList = "    * ";

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

        logger = FLog.FLoggerForType.As<IVersatileFLogger>();
    }

    [TestMethod]
    public void WithCompactLogAllComplexTypeSpanFmtFieldFormattingTests()
    {
        var nonNullableSpanFormattableInvokers = 
            scafReg.IsComplexType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsNonNullables().ToList();

        var le = logger.InfoAppend("Complex Type Single Value Field -  Always Add Scaffolding Classes - ")?.AppendLine();
        foreach (var nonNullFormatInvoker in nonNullableSpanFormattableInvokers)
        {
            le = le?.Append(BulletList).AppendLine(nonNullFormatInvoker.Name);
        }
        le?.AppendLine().Append("Total ").AppendLine(nonNullableSpanFormattableInvokers.Count).FinalAppend("");

        var formatExpect = SpanFormattableTestData.AllSpanFormattableExpectations.Where( fe => !fe.IsNullableStruct).ToList();
        
        var tos = new TheOneString().Initialize(StringStyle.Compact | StringStyle.Log);
        foreach (var nonNullFormatInvoker in nonNullableSpanFormattableInvokers)
        {
            foreach (var formatExpectation in formatExpect)
            {
                tos.Clear();
                var stringBearer = formatExpectation.CreateStringBearerWithValueFor(nonNullFormatInvoker);
                stringBearer.RevealState(tos);
                logger.InfoAppend("Result - ")?
                    .FinalAppend(tos.WriteBuffer.ToString());
            }
        }
    }

    [TestMethod]
    public void WithCompactLogAllComplexTypeNullFmtStructFieldFormattingTests()
    {
        var nonNullableSpanFormattableInvokers = 
            scafReg.IsComplexType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().OnlyAcceptsNullableStructs().ToList();

        var le = logger.InfoAppend("Complex Type Single Value Field -  Always Add Scaffolding Classes - ")?.AppendLine();
        foreach (var nonNullFormatInvoker in nonNullableSpanFormattableInvokers)
        {
            le = le?.Append(BulletList).AppendLine(nonNullFormatInvoker.Name);
        }
        le?.AppendLine().Append("Total ").AppendLine(nonNullableSpanFormattableInvokers.Count).FinalAppend("");

        var formatExpect = SpanFormattableTestData.AllSpanFormattableExpectations.Where( fe => fe.IsNullableStruct).ToList();
        
        var tos = new TheOneString().Initialize(StringStyle.Compact | StringStyle.Log);
        foreach (var nonNullFormatInvoker in nonNullableSpanFormattableInvokers)
        {
            foreach (var formatExpectation in formatExpect)
            {
                tos.Clear();
                var stringBearer = formatExpectation.CreateStringBearerWithValueFor(nonNullFormatInvoker);
                stringBearer.RevealState(tos);
                logger.InfoAppend("Result - ")?
                    .FinalAppend(tos.WriteBuffer.ToString());
            }
        }
    }
    
    
}
