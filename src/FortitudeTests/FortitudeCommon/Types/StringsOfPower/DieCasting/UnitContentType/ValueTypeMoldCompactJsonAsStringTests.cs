// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;


[NoMatchingProductionClass]
[TestClass]
public class ContentTypeMoldTestsCompactJsonAsStringTests : ContentTypeMoldAsStringTests
{
    public override StringStyle TestStyle => Compact | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(CloakedBearerTestData.AllCloakedBearerExpectations[57]
                                           , ScaffoldingRegistry.AllScaffoldingTypes[1197], StringBuilderType.MutableString);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    {
        var compactJsonTemplate =
            expectation.CoreType.ImplementsInterfaceOrIs<IStringBearer>() && condition.HasAnyOf(DefaultTreatedAsStringOut)
                ? "\"{0}\""
                : "{0}";

        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
            
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            if (expectValue.SequenceMatches("null") && condition.HasAnyOf(DefaultBecomesNull) ) return expectValue;
        }
        else { expectValue.Clear(); }
        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(compactJsonTemplate, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
    
    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        string compactJsonTemplate = 
            condition.HasAnyOf(DefaultTreatedAsStringOut)
            ? (expectation.CoreType.ImplementsInterfaceOrIs<IStringBearer>() 
                ?  "{{\\u0022{0}\\u0022:{1}}}"
                :  "\"{{\\u0022{0}\\u0022:{1}}}\"")
            : "{{\"{0}\":{1}}}";

        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            expectValue.Replace("\"", "\\u0022");
        }
        else { expectValue.Clear(); }
        
        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(compactJsonTemplate, propertyName, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
}
