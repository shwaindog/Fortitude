// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
public class CharArrayUnitSimpleContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoCharArrayFields>? twoSameCharArrayFieldsWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>?
        twoSameCharArraysOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>?
        twoSameCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedStringContent>?
        twoSameCharArraysOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedStringContent>?
        twoSameCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect;
    
    private static InputBearerExpect<TwoCharArrayFields>? twoSameCharArrayFieldsShowRevisitInstanceIdsOnlyExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>?
        twoSameCharArraysOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>?
        twoSameCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedStringContent>?
        twoSameCharArraysOneSimpleCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedStringContent>?
        twoSameCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect;
    
    private static InputBearerExpect<TwoCharArrayFields>? twoSameCharArrayFieldsShowRevisitsAndValuesExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>?
        twoSameCharArraysOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>?
        twoSameCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedStringContent>?
        twoSameCharArraysOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedStringContent>?
        twoSameCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoCharArrayFields TwoCharArraysFields
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoSameIpAddressFields = new TwoCharArrayFields(repeated, repeated);
            return twoSameIpAddressFields;
        }
    }

    public static InputBearerExpect<TwoCharArrayFields> TwoCharArraysFieldsWithWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharArrayFieldsWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoCharArrayFields>(TwoCharArraysFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArrayFields
                         {
                         FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArrayFields {
                          FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysFieldsWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysFieldsWithWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharArraysFieldsWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysFieldsWithWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharArraysFieldsWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysFieldsWithWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharArraysFieldsWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysFieldsWithWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharArraysFirstAsSimpleCloakedValueContent TwoCharArraysOneSimpleCloakedValueOneField
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoSameCharArraysOneCloakedOneFields =
                new TwoCharArraysFirstAsSimpleCloakedValueContent(repeated, repeated);
            return twoSameCharArraysOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>
        TwoCharArraysOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharArraysOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>(TwoCharArraysOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedValueContent
                         {
                         FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedValueContent {
                          FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstCharArrayField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.,
                        "SecondCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharArrayField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.,
                          "SecondCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharArraysSecondAsSimpleCloakedValueContent TwoCharArraysOneFieldOneSimpleCloakedValue
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoSameCharArraysOneFieldsOneCloaked
                = new TwoCharArraysSecondAsSimpleCloakedValueContent(repeated, repeated);
            return twoSameCharArraysOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>
        TwoCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>(TwoCharArraysOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedValueContent
                         {
                         FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedValueContent {
                          FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondCharArrayField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondCharArrayField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharArraysFirstAsSimpleCloakedStringContent TwoCharArraysOneSimpleCloakedStringOneField
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoSameCharArraysOneCloakedOneFields =
                new TwoCharArraysFirstAsSimpleCloakedStringContent(repeated, repeated);
            return twoSameCharArraysOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedStringContent>
        TwoCharArraysOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharArraysOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsSimpleCloakedStringContent>(TwoCharArraysOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedStringContent {
                         FirstCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedStringContent {
                          FirstCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharArraysSecondAsSimpleCloakedStringContent TwoCharArraysOneFieldOneSimpleCloakedString
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoSameCharArraysOneFieldsOneCloakedString
                = new TwoCharArraysSecondAsSimpleCloakedStringContent(repeated, repeated);
            return twoSameCharArraysOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedStringContent>
        TwoCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsSimpleCloakedStringContent>(TwoCharArraysOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedStringContent {
                         FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedStringContent {
                          FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharArraysOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }
    
    public static InputBearerExpect<TwoCharArrayFields> TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpect
    {
        get
        {
            return twoSameCharArrayFieldsShowRevisitInstanceIdsOnlyExpect ??=
                new InputBearerExpect<TwoCharArrayFields>(TwoCharArraysFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArrayFields {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArrayFields {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharArrayField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpectCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpectCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances        = true
                , InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpectPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpectPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances       = true
               , InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
    
    public static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>
        TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameCharArraysOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>(TwoCharArraysOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedValueContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedValueContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          },
                          "SecondCharArrayField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedValueOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedValueOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
            , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }
    
    public static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>
        TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>(TwoCharArraysOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedValueContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedValueContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharArrayField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedValueShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }
    
    public static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedStringContent>
        TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameCharArraysOneSimpleCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsSimpleCloakedStringContent>(TwoCharArraysOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedStringContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondCharArrayField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedStringContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":
                        {
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharArrayField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }
    
    public static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedStringContent>
        TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsSimpleCloakedStringContent>(TwoCharArraysOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedStringContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedStringContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharArrayField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedStringShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }
    
    public static InputBearerExpect<TwoCharArrayFields> TwoCharArraysFieldsShowRevisitsAndValuesExpect
    {
        get
        {
            return twoSameCharArrayFieldsShowRevisitsAndValuesExpect ??=
                new InputBearerExpect<TwoCharArrayFields>(TwoCharArraysFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArrayFields {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
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
                        TwoCharArrayFields {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharArrayField": {
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
    public void TwoCharArraysFieldsShowRevisitsAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances  = true
                , InstanceMarkingIncludeCharArrayContents = true 
             });
    }

    [TestMethod]
    public void TwoCharArraysFieldsShowRevisitsAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true 
             });
    }

    [TestMethod]
    public void TwoCharArraysFieldsShowRevisitsAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true 
             });
    }

    [TestMethod]
    public void TwoCharArraysFieldsShowRevisitsAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents      = true 
             });
    }
    
    public static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>
        TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameCharArraysOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedValueContent>(TwoCharArraysOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedValueContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
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
                        TwoCharArraysFirstAsSimpleCloakedValueContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          },
                          "SecondCharArrayField": {
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
    public void TwoCharArraysOneCloakedValueOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
                , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
            , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }
    
    public static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>
        TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedValueContent>(TwoCharArraysOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedValueContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
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
                        TwoCharArraysSecondAsSimpleCloakedValueContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharArrayField": {
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
    public void TwoCharArraysOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }
    
    public static InputBearerExpect<TwoCharArraysFirstAsSimpleCloakedStringContent>
        TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameCharArraysOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsSimpleCloakedStringContent>(TwoCharArraysOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsSimpleCloakedStringContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondCharArrayField: {
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
                        TwoCharArraysFirstAsSimpleCloakedStringContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharArrayField": {
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
    public void TwoCharArraysOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }
    
    public static InputBearerExpect<TwoCharArraysSecondAsSimpleCloakedStringContent>
        TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsSimpleCloakedStringContent>(TwoCharArraysOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsSimpleCloakedStringContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
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
                        TwoCharArraysSecondAsSimpleCloakedStringContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
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
                          "FirstCharArrayField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharArrayField": {
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
    public void TwoCharArraysOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharArraysOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }
}
