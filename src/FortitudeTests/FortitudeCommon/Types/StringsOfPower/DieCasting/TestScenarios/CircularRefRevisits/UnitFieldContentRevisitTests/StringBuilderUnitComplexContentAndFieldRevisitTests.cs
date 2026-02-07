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
public class StringBuilderUnitComplexContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedValueContent>?
        twoSameStringBuildersOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedValueContent>?
        twoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedStringContent>?
        twoSameStringBuildersOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedStringContent>?
        twoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect;

    private static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedValueContent>?
        twoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedValueContent>?
        twoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedStringContent>?
        twoSameStringBuildersOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedStringContent>?
        twoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect;

    private static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedValueContent>?
        twoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedValueContent>?
        twoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedStringContent>?
        twoSameStringBuildersOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedStringContent>?
        twoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoStringBuildersFirstAsComplexCloakedValueContent TwoSameStringBuildersOneComplexCloakedValueOneField
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameStringBuildersOneCloakedOneFields =
                new TwoStringBuildersFirstAsComplexCloakedValueContent(repeated, repeated);
            return twoSameStringBuildersOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedValueContent>
        TwoSameStringBuildersOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuildersOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsComplexCloakedValueContent>(TwoSameStringBuildersOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsComplexCloakedValueContent {
                         FirstStringBuilderField: {
                         CloakedRevealerFirstStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersFirstAsComplexCloakedValueContent {
                          FirstStringBuilderField: {
                            CloakedRevealerFirstStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
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
    public void TwoSameStringBuildersOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBuildersSecondAsComplexCloakedValueContent TwoSameStringBuildersOneFieldOneComplexCloakedValue
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameStringBuildersOneFieldsOneCloaked
                = new TwoStringBuildersSecondAsComplexCloakedValueContent(repeated, repeated);
            return twoSameStringBuildersOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedValueContent>
        TwoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsComplexCloakedValueContent>(TwoSameStringBuildersOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedValueContent {
                         FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondStringBuilderField: {
                         CloakedRevealerSecondStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyMap: { FirstKey: 1, SecondKey: 2, ThirdKey: 3 }
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedValueContent {
                          FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondStringBuilderField: {
                            CloakedRevealerSecondStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyMap: {
                              FirstKey: 1,
                              SecondKey: 2,
                              ThirdKey: 3
                            }
                          }
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
    public void TwoSameStringBuildersOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBuildersFirstAsComplexCloakedStringContent TwoSameStringBuildersOneComplexCloakedStringOneField
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameStringBuildersOneCloakedOneFields =
                new TwoStringBuildersFirstAsComplexCloakedStringContent(repeated, repeated);
            return twoSameStringBuildersOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedStringContent>
        TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuildersOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsComplexCloakedStringContent>(TwoSameStringBuildersOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsComplexCloakedStringContent {
                         FirstStringBuilderField: {
                         CloakedRevealerFirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersFirstAsComplexCloakedStringContent {
                          FirstStringBuilderField: {
                            CloakedRevealerFirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
                          },
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
    public void TwoSameStringBuildersOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringBuildersSecondAsComplexCloakedStringContent TwoSameStringBuildersOneFieldOneComplexCloakedString
    {
        get
        {
            var repeated        = new StringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameStringBuildersOneFieldsOneCloakedString
                = new TwoStringBuildersSecondAsComplexCloakedStringContent(repeated, repeated);
            return twoSameStringBuildersOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedStringContent>
        TwoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsComplexCloakedStringContent>(TwoSameStringBuildersOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedStringContent {
                         FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondStringBuilderField: {
                         CloakedRevealerSecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyList: [
                         FirstCharSeq,
                         SecondCharSeq,
                         ThirdCharSeq
                         ]
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedStringContent {
                          FirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondStringBuilderField: {
                            CloakedRevealerSecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyList: [
                              FirstCharSeq,
                              SecondCharSeq,
                              ThirdCharSeq
                            ]
                          }
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
    public void TwoSameStringBuildersOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringBuildersOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }


    public static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedValueContent>
        TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsComplexCloakedValueContent>(TwoSameStringBuildersOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsComplexCloakedValueContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         CloakedRevealerFirstStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
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
                        TwoStringBuildersFirstAsComplexCloakedValueContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            CloakedRevealerFirstStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
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
    public void TwoSameStringBuildersOneCloakedValueOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedValueOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    public static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedValueContent>
        TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsComplexCloakedValueContent>(TwoSameStringBuildersOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedValueContent {
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
                        TwoStringBuildersSecondAsComplexCloakedValueContent {
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
    public void TwoSameStringBuildersOneFieldOneCloakedValueShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }


    public static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedStringContent>
        TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringBuildersOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsComplexCloakedStringContent>(TwoSameStringBuildersOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsComplexCloakedStringContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         CloakedRevealerFirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
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
                        TwoStringBuildersFirstAsComplexCloakedStringContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            CloakedRevealerFirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
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
    public void TwoSameStringBuildersOneCloakedStringOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }


    public static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedStringContent>
        TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsComplexCloakedStringContent>(TwoSameStringBuildersOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedStringContent {
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
                        TwoStringBuildersSecondAsComplexCloakedStringContent {
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
    public void TwoSameStringBuildersOneFieldOneCloakedStringShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
             });
    }

    public static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedValueContent>
        TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsComplexCloakedValueContent>(TwoSameStringBuildersOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsComplexCloakedValueContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         CloakedRevealerFirstStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
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
                        TwoStringBuildersFirstAsComplexCloakedValueContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            CloakedRevealerFirstStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
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
    public void TwoSameStringBuildersOneCloakedValueOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    public static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedValueContent>
        TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsComplexCloakedValueContent>(TwoSameStringBuildersOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedValueContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1,
                         CloakedRevealerSecondStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyMap: {
                         FirstKey: 1,
                         SecondKey: 2,
                         ThirdKey: 3
                         }
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedValueContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1,
                            CloakedRevealerSecondStringBuilderField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyMap: {
                              FirstKey: 1,
                              SecondKey: 2,
                              ThirdKey: 3
                            }
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
                        "CloakedRevealerSecondStringBuilderField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
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
                            "CloakedRevealerSecondStringBuilderField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }


    public static InputBearerExpect<TwoStringBuildersFirstAsComplexCloakedStringContent>
        TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringBuildersOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersFirstAsComplexCloakedStringContent>(TwoSameStringBuildersOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersFirstAsComplexCloakedStringContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         CloakedRevealerFirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
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
                        TwoStringBuildersFirstAsComplexCloakedStringContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            CloakedRevealerFirstStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
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
    public void TwoSameStringBuildersOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }


    public static InputBearerExpect<TwoStringBuildersSecondAsComplexCloakedStringContent>
        TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringBuildersSecondAsComplexCloakedStringContent>(TwoSameStringBuildersOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedStringContent {
                         FirstStringBuilderField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringBuilderField: {
                         $ref: 1,
                         CloakedRevealerSecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyList: [ FirstCharSeq, SecondCharSeq, ThirdCharSeq ]
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringBuildersSecondAsComplexCloakedStringContent {
                          FirstStringBuilderField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringBuilderField: {
                            $ref: 1,
                            CloakedRevealerSecondStringBuilderField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyList: [
                              FirstCharSeq,
                              SecondCharSeq,
                              ThirdCharSeq
                            ]
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
                        "CloakedRevealerSecondStringBuilderField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
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
                            "CloakedRevealerSecondStringBuilderField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceMarkingIncludeStringBuilderContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking   = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }


    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringBuildersOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringBuildersOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringBuilderInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeStringBuilderContents = true
             });
    }
}
