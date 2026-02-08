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
public class CharArrayUnitComplexContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedValueContent>?
        twoCharArraysOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedValueContent>?
        twoCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedStringContent>?
        twoCharArraysOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedStringContent>?
        twoCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect;

    private static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedValueContent>?
        twoCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedValueContent>?
        twoCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedStringContent>?
        twoCharArraysOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedStringContent>?
        twoCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect;

    private static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedValueContent>?
        twoCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedValueContent>?
        twoCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedStringContent>?
        twoCharArraysOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedStringContent>?
        twoCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoCharArraysFirstAsComplexCloakedValueContent TwoSameCharArraysOneComplexCloakedValueOneField
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoCharArraysOneCloakedOneFields =
                new TwoCharArraysFirstAsComplexCloakedValueContent(repeated, repeated);
            return twoCharArraysOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedValueContent>
        TwoSameCharArraysOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsComplexCloakedValueContent>(TwoSameCharArraysOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsComplexCloakedValueContent {
                         FirstCharArrayField: {
                         CloakedRevealerFirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysFirstAsComplexCloakedValueContent {
                          FirstCharArrayField: {
                            CloakedRevealerFirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
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
    public void TwoSameCharArraysOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharArraysSecondAsComplexCloakedValueContent TwoSameCharArraysOneFieldOneComplexCloakedValue
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoCharArraysOneFieldsOneCloaked
                = new TwoCharArraysSecondAsComplexCloakedValueContent(repeated, repeated);
            return twoCharArraysOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedValueContent>
        TwoSameCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsComplexCloakedValueContent>(TwoSameCharArraysOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsComplexCloakedValueContent {
                         FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondCharArrayField: {
                         CloakedRevealerSecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyMap: { FirstKey: 1, SecondKey: 2, ThirdKey: 3 }
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysSecondAsComplexCloakedValueContent {
                          FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondCharArrayField: {
                            CloakedRevealerSecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
    public void TwoSameCharArraysOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharArraysFirstAsComplexCloakedStringContent TwoSameCharArraysOneComplexCloakedStringOneField
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoCharArraysOneCloakedOneFields =
                new TwoCharArraysFirstAsComplexCloakedStringContent(repeated, repeated);
            return twoCharArraysOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedStringContent>
        TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsComplexCloakedStringContent>(TwoSameCharArraysOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsComplexCloakedStringContent {
                         FirstCharArrayField: {
                         CloakedRevealerFirstCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysFirstAsComplexCloakedStringContent {
                          FirstCharArrayField: {
                            CloakedRevealerFirstCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
                          },
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
    public void TwoSameCharArraysOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharArraysSecondAsComplexCloakedStringContent TwoSameCharArraysOneFieldOneComplexCloakedString
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.".ToCharArray();
            var twoCharArraysOneFieldsOneCloakedString
                = new TwoCharArraysSecondAsComplexCloakedStringContent(repeated, repeated);
            return twoCharArraysOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedStringContent>
        TwoSameCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsComplexCloakedStringContent>(TwoSameCharArraysOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsComplexCloakedStringContent {
                         FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondCharArrayField: {
                         CloakedRevealerSecondCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
                        TwoCharArraysSecondAsComplexCloakedStringContent {
                          FirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondCharArrayField: {
                            CloakedRevealerSecondCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
    public void TwoSameCharArraysOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }


    public static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedValueContent>
        TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsComplexCloakedValueContent>(TwoSameCharArraysOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsComplexCloakedValueContent {
                         FirstCharArrayField: {
                         $id: 1,
                         CloakedRevealerFirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
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
                        TwoCharArraysFirstAsComplexCloakedValueContent {
                          FirstCharArrayField: {
                            $id: 1,
                            CloakedRevealerFirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
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
    public void TwoSameCharArraysOneCloakedValueOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedValueOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    public static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedValueContent>
        TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsComplexCloakedValueContent>(TwoSameCharArraysOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsComplexCloakedValueContent {
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
                        TwoCharArraysSecondAsComplexCloakedValueContent {
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
    public void TwoSameCharArraysOneFieldOneCloakedValueShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }


    public static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedStringContent>
        TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsComplexCloakedStringContent>(TwoSameCharArraysOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsComplexCloakedStringContent {
                         FirstCharArrayField: {
                         $id: 1,
                         CloakedRevealerFirstCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
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
                        TwoCharArraysFirstAsComplexCloakedStringContent {
                          FirstCharArrayField: {
                            $id: 1,
                            CloakedRevealerFirstCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
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
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }


    public static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedStringContent>
        TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsComplexCloakedStringContent>(TwoSameCharArraysOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsComplexCloakedStringContent {
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
                        TwoCharArraysSecondAsComplexCloakedStringContent {
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
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
             });
    }

    public static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedValueContent>
        TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsComplexCloakedValueContent>(TwoSameCharArraysOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsComplexCloakedValueContent {
                         FirstCharArrayField: {
                         $id: 1,
                         CloakedRevealerFirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
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
                        TwoCharArraysFirstAsComplexCloakedValueContent {
                          FirstCharArrayField: {
                            $id: 1,
                            CloakedRevealerFirstCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
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
    public void TwoSameCharArraysOneCloakedValueOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    public static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedValueContent>
        TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsComplexCloakedValueContent>(TwoSameCharArraysOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsComplexCloakedValueContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
                         $ref: 1,
                         CloakedRevealerSecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
                        TwoCharArraysSecondAsComplexCloakedValueContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
                            $ref: 1,
                            CloakedRevealerSecondCharArrayField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
                        "$ref":"1",
                        "CloakedRevealerSecondCharArrayField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
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
                            "CloakedRevealerSecondCharArrayField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }


    public static InputBearerExpect<TwoCharArraysFirstAsComplexCloakedStringContent>
        TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharArraysFirstAsComplexCloakedStringContent>(TwoSameCharArraysOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysFirstAsComplexCloakedStringContent {
                         FirstCharArrayField: {
                         $id: 1,
                         CloakedRevealerFirstCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
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
                        TwoCharArraysFirstAsComplexCloakedStringContent {
                          FirstCharArrayField: {
                            $id: 1,
                            CloakedRevealerFirstCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
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
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents   = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }


    public static InputBearerExpect<TwoCharArraysSecondAsComplexCloakedStringContent>
        TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharArraysSecondAsComplexCloakedStringContent>(TwoSameCharArraysOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharArraysSecondAsComplexCloakedStringContent {
                         FirstCharArrayField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         },
                         SecondCharArrayField: {
                         $ref: 1,
                         CloakedRevealerSecondCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyList: [ FirstCharSeq, SecondCharSeq, ThirdCharSeq ]
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharArraysSecondAsComplexCloakedStringContent {
                          FirstCharArrayField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          SecondCharArrayField: {
                            $ref: 1,
                            CloakedRevealerSecondCharArrayField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
                        "FirstCharArrayField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharArrayField":{
                        "$ref":"1",
                        "CloakedRevealerSecondCharArrayField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
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
                            "CloakedRevealerSecondCharArrayField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceMarkingIncludeCharArrayContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking   = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }


    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharArrayInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeCharArrayContents = true
             });
    }
}
