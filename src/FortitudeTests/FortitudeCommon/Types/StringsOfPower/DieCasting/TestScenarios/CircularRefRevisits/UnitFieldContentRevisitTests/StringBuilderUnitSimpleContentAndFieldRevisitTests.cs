// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.UnitFieldContentRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class StringBuilderUnitSimpleContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoStringBuilderFields>? twoSameStringBuilderFieldsWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>?
        twoSameStringBuildersOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>?
        twoSameStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedStringContent>?
        twoSameStringBuildersOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedStringContent>?
        twoSameStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect;
    
    private static InputBearerExpect<TwoStringBuilderFields>? twoSameStringBuilderFieldsShowRevisitInstanceIdsOnlyExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>?
        twoSameStringBuildersOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>?
        twoSameStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedStringContent>?
        twoSameStringBuildersOneSimpleCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedStringContent>?
        twoSameStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect;
    
    private static InputBearerExpect<TwoStringBuilderFields>? twoSameFieldsShowRevisitsAndValuesExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>?
        twoSameStringBuildersOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>?
        twoSameStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedStringContent>?
        twoSameStringBuildersOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedStringContent>?
        twoSameStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoStringBuilderFields TwoStringBuildersFields
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameIpAddressFields = new TwoStringBuilderFields(repeated, repeated);
            return twoSameIpAddressFields;
        }
    }

    public static InputBearerExpect<TwoStringBuilderFields> TwoStringBuildersFieldsWithWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuilderFieldsWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBuilderFields>(TwoStringBuildersFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuilderFields
                         {
                         FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuilderFields {
                          FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersFieldsWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersFieldsWithWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoStringBuildersFieldsWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersFieldsWithWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoStringBuildersFieldsWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersFieldsWithWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoStringBuildersFieldsWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersFieldsWithWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBuildersFirstAsSimpleCloakedValueContent TwoStringBuildersOneSimpleCloakedValueOneField
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameStringBuildersOneCloakedOneFields =
                new TwoStringBuildersFirstAsSimpleCloakedValueContent(repeated, repeated);
            return twoSameStringBuildersOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>
        TwoStringBuildersOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuildersOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>(TwoStringBuildersOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedValueContent
                         {
                         FirstStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedValueContent {
                          FirstStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.,
                        "SecondStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.,
                          "SecondStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBuildersSecondAsSimpleCloakedValueContent TwoStringBuildersOneFieldOneSimpleCloakedValue
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameStringBuildersOneFieldsOneCloaked
                = new TwoStringBuildersSecondAsSimpleCloakedValueContent(repeated, repeated);
            return twoSameStringBuildersOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>
        TwoStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>(TwoStringBuildersOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedValueContent
                         {
                         FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedValueContent {
                          FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondStringBuilderField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondStringBuilderField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBuildersFirstAsSimpleCloakedStringContent TwoStringBuildersOneSimpleCloakedStringOneField
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameStringBuildersOneCloakedOneFields =
                new TwoStringBuildersFirstAsSimpleCloakedStringContent(repeated, repeated);
            return twoSameStringBuildersOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedStringContent>
        TwoStringBuildersOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuildersOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsSimpleCloakedStringContent>(TwoStringBuildersOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedStringContent {
                         FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedStringContent {
                          FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBuildersSecondAsSimpleCloakedStringContent TwoStringBuildersOneFieldOneSimpleCloakedString
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameStringBuildersOneFieldsOneCloakedString
                = new TwoStringBuildersSecondAsSimpleCloakedStringContent(repeated, repeated);
            return twoSameStringBuildersOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedStringContent>
        TwoStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsSimpleCloakedStringContent>(TwoStringBuildersOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedStringContent {
                         FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedStringContent {
                          FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoStringBuildersOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }
    
    public static InputBearerExpect<TwoStringBuilderFields> TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpect
    {
        get
        {
            return twoSameStringBuilderFieldsShowRevisitInstanceIdsOnlyExpect ??=
                new InputBearerExpect<TwoStringBuilderFields>(TwoStringBuildersFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuilderFields {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuilderFields {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "FirstStringBuilderField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringBuilderField":{
                        "$ref":"1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpectCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpectCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
                , InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpectPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpectPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
    
    public static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>
        TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringBuildersOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>(TwoStringBuildersOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedValueContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondStringBuilderField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedValueContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondStringBuilderField: {
                            $ref: 1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":{
                        "$id":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        },
                        "SecondStringBuilderField":{
                        "$ref":"1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedValueOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedValueOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
            , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }
    
    public static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>
        TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>(TwoStringBuildersOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedValueContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedValueContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringBuilderField":{
                        "$ref":"1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedValueShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }
    
    public static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedStringContent>
        TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringBuildersOneSimpleCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsSimpleCloakedStringContent>(TwoStringBuildersOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedStringContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedStringContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":
                        {
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringBuilderField":
                        {
                        "$ref":"1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }
    
    public static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedStringContent>
        TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsSimpleCloakedStringContent>(TwoStringBuildersOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedStringContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedStringContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringBuilderField":{
                        "$ref":"1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedStringShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }
    
    public static InputBearerExpect<TwoStringBuilderFields> TwoStringBuildersFieldsShowRevisitsAndValuesExpect
    {
        get
        {
            return twoSameFieldsShowRevisitsAndValuesExpect ??=
                new InputBearerExpect<TwoStringBuilderFields>(TwoStringBuildersFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuilderFields {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuilderFields {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """
                        {
                        "FirstStringBuilderField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringBuilderField":{
                        "$ref":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersFieldsShowRevisitsAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances  = true
                , InstanceMarkingIncludeStringBuilderContents = true 
             });
    }

    [TestMethod]
    public void TwoStringBuildersFieldsShowRevisitsAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true 
             });
    }

    [TestMethod]
    public void TwoStringBuildersFieldsShowRevisitsAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true 
             });
    }

    [TestMethod]
    public void TwoStringBuildersFieldsShowRevisitsAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents      = true 
             });
    }
    
    public static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>
        TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringBuildersOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedValueContent>(TwoStringBuildersOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedValueContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondStringBuilderField: {
                         $ref: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedValueContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondStringBuilderField: {
                            $ref: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":{
                        "$id":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        },
                        "SecondStringBuilderField":{
                        "$ref":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedValueOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
                , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
            , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }
    
    public static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>
        TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedValueContent>(TwoStringBuildersOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedValueContent
                         {
                         FirstStringBuilderField:
                         {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField:
                         {
                         $ref: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedValueContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":
                        {
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringBuilderField":
                        {
                        "$ref":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }
    
    public static InputBearerExpect<TwoStringBuildersFirstAsSimpleCloakedStringContent>
        TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringBuildersOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsSimpleCloakedStringContent>(TwoStringBuildersOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedStringContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersFirstAsSimpleCloakedStringContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringBuilderField":{
                        "$ref":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }
    
    public static InputBearerExpect<TwoStringBuildersSecondAsSimpleCloakedStringContent>
        TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsSimpleCloakedStringContent>(TwoStringBuildersOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedStringContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsSimpleCloakedStringContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringBuilderField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringBuilderField":{
                        "$ref":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringBuilderField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringBuilderField": {
                            "$ref": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoStringBuildersOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }
}
