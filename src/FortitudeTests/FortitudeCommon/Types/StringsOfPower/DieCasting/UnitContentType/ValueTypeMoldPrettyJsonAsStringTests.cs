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
public class ContentTypeMoldPrettyJsonAsStringTests : ContentTypeMoldAsStringTests
{
    public override StringStyle TestStyle => Pretty | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(BoolTestData.AllBoolExpectations[5], ScaffoldingRegistry.AllScaffoldingTypes[1217]);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var prettyJsonTemplate = 
            expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
             ?   (condition.HasAnyOf(DefaultTreatedAsStringOut) 
                     ? "\"{{{0}{1}{2}: {3}{0}}}\"" 
                     : "{{{0}{1}{2}: {3}{0}}}")
            : (expectation.CoreType.ImplementsInterfaceOrIs<IStringBearer>()  && condition.HasAnyOf(DefaultTreatedAsStringOut) 
                ? "\"{3}\"" 
                : "{3}");

        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            maybeNewLine = (condition.HasAnyOf(DefaultTreatedAsStringOut) ? "\\u000a" : "\n");
            maybeIndent  = "  ";
            if (expectValue.Trim().Length == 0)
            {
                expectValue.Clear();
            } else if (expectValue.SequenceMatches("null") && condition.HasAnyOf(DefaultBecomesNull) ) return expectValue;
            else if (condition.HasAnyOf(DefaultTreatedAsStringOut))
            {
                expectValue.Replace("\n", "\\u000a");
            }
        }
        else { expectValue.Clear(); }

        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(prettyJsonTemplate, maybeNewLine, maybeIndent, propertyName, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
    
    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        string prettyJsonTemplate = 
            condition.HasAnyOf(DefaultTreatedAsStringOut)
                ? (expectation.CoreType.ImplementsInterfaceOrIs<IStringBearer>() 
                    ?  "{{{0}{1}\\u0022{2}\\u0022: {3}{0}}}"
                    :  "\"{{{0}{1}\\u0022{2}\\u0022: {3}{0}}}\"") 
                : "{{{0}{1}\"{2}\": {3}{0}}}";

        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            maybeNewLine = (condition.HasAnyOf(DefaultTreatedAsStringOut) ? "\\u000a" : "\n");
            maybeIndent  = "  ";
            expectValue.Replace("\"", "\\u0022");
            expectValue  = (condition.HasComplexTypeFlag() && expectValue.IsBrcBounded() 
                ? expectValue.IndentSubsequentLines() 
                : expectValue);
        }
        else { expectValue.Clear(); }
        
        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(prettyJsonTemplate, maybeNewLine, maybeIndent, propertyName, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
}
