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
public class StringUnitComplexContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoStringsFirstAsComplexCloakedValueContent>?
        twoSameStringsOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringsSecondAsComplexCloakedValueContent>?
        twoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringsFirstAsComplexCloakedStringContent>?
        twoSameStringsOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoStringsSecondAsComplexCloakedStringContent>?
        twoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect;

    private static InputBearerExpect<TwoStringsFirstAsComplexCloakedValueContent>?
        twoSameStringsOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringsSecondAsComplexCloakedValueContent>?
        twoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringsFirstAsComplexCloakedStringContent>?
        twoSameStringsOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoStringsSecondAsComplexCloakedStringContent>?
        twoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect;

    private static InputBearerExpect<TwoStringsFirstAsComplexCloakedValueContent>?
        twoSameStringsOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringsSecondAsComplexCloakedValueContent>?
        twoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringsFirstAsComplexCloakedStringContent>?
        twoSameStringsOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoStringsSecondAsComplexCloakedStringContent>?
        twoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoStringsFirstAsComplexCloakedValueContent TwoSameStringsOneComplexCloakedValueOneField
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.";
            var twoSameStringsOneCloakedOneFields =
                new TwoStringsFirstAsComplexCloakedValueContent(repeated, repeated);
            return twoSameStringsOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringsFirstAsComplexCloakedValueContent>
        TwoSameStringsOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringsOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringsFirstAsComplexCloakedValueContent>(TwoSameStringsOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsFirstAsComplexCloakedValueContent {
                         FirstStringField: {
                         CloakedRevealerFirstStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringsFirstAsComplexCloakedValueContent {
                          FirstStringField: {
                            CloakedRevealerFirstStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.,
                        "SecondStringField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.,
                          "SecondStringField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringsSecondAsComplexCloakedValueContent TwoSameStringsOneFieldOneComplexCloakedValue
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.";
            var twoSameStringsOneFieldsOneCloaked
                = new TwoStringsSecondAsComplexCloakedValueContent(repeated, repeated);
            return twoSameStringsOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoStringsSecondAsComplexCloakedValueContent>
        TwoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringsSecondAsComplexCloakedValueContent>(TwoSameStringsOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsSecondAsComplexCloakedValueContent {
                         FirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondStringField: {
                         CloakedRevealerSecondStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyMap: { FirstKey: 1, SecondKey: 2, ThirdKey: 3 }
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringsSecondAsComplexCloakedValueContent {
                          FirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondStringField: {
                            CloakedRevealerSecondStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
                        "FirstStringField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondStringField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondStringField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringsFirstAsComplexCloakedStringContent TwoSameStringsOneComplexCloakedStringOneField
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.";
            var twoSameStringsOneCloakedOneFields =
                new TwoStringsFirstAsComplexCloakedStringContent(repeated, repeated);
            return twoSameStringsOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoStringsFirstAsComplexCloakedStringContent>
        TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringsOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringsFirstAsComplexCloakedStringContent>(TwoSameStringsOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsFirstAsComplexCloakedStringContent {
                         FirstStringField: {
                         CloakedRevealerFirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringsFirstAsComplexCloakedStringContent {
                          FirstStringField: {
                            CloakedRevealerFirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstStringField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondStringField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondStringField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoStringsSecondAsComplexCloakedStringContent TwoSameStringsOneFieldOneComplexCloakedString
    {
        get
        {
            var repeated        = "You may resume writing you're résumé, or see \"Vee\" with help with your CV.";
            var twoSameStringsOneFieldsOneCloakedString
                = new TwoStringsSecondAsComplexCloakedStringContent(repeated, repeated);
            return twoSameStringsOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoStringsSecondAsComplexCloakedStringContent>
        TwoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoStringsSecondAsComplexCloakedStringContent>(TwoSameStringsOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsSecondAsComplexCloakedStringContent {
                         FirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondStringField: {
                         CloakedRevealerSecondStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
                        TwoStringsSecondAsComplexCloakedStringContent {
                          FirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondStringField: {
                            CloakedRevealerSecondStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
                        "FirstStringField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondStringField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondStringField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameStringsOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }


    public static InputBearerExpect<TwoStringsFirstAsComplexCloakedValueContent>
        TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringsOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringsFirstAsComplexCloakedValueContent>(TwoSameStringsOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsFirstAsComplexCloakedValueContent {
                         FirstStringField: {
                         $id: 1,
                         CloakedRevealerFirstStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondStringField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringsFirstAsComplexCloakedValueContent {
                          FirstStringField: {
                            $id: 1,
                            CloakedRevealerFirstStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondStringField: {
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
                        "FirstStringField":{
                        "$id":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        },
                        "SecondStringField":{
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
                          "FirstStringField": {
                            "$id": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          },
                          "SecondStringField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedValueOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedValueOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    public static InputBearerExpect<TwoStringsSecondAsComplexCloakedValueContent>
        TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringsSecondAsComplexCloakedValueContent>(TwoSameStringsOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsSecondAsComplexCloakedValueContent {
                         FirstStringField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringsSecondAsComplexCloakedValueContent {
                          FirstStringField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringField: {
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
                        "FirstStringField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringField":{
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
                          "FirstStringField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedValueShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }


    public static InputBearerExpect<TwoStringsFirstAsComplexCloakedStringContent>
        TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringsOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringsFirstAsComplexCloakedStringContent>(TwoSameStringsOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsFirstAsComplexCloakedStringContent {
                         FirstStringField: {
                         $id: 1,
                         CloakedRevealerFirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondStringField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringsFirstAsComplexCloakedStringContent {
                          FirstStringField: {
                            $id: 1,
                            CloakedRevealerFirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondStringField: {
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
                        "FirstStringField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringField":{
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
                          "FirstStringField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }


    public static InputBearerExpect<TwoStringsSecondAsComplexCloakedStringContent>
        TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoStringsSecondAsComplexCloakedStringContent>(TwoSameStringsOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsSecondAsComplexCloakedStringContent {
                         FirstStringField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringsSecondAsComplexCloakedStringContent {
                          FirstStringField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringField: {
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
                        "FirstStringField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringField":{
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
                          "FirstStringField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedStringShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
             });
    }

    public static InputBearerExpect<TwoStringsFirstAsComplexCloakedValueContent>
        TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringsOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringsFirstAsComplexCloakedValueContent>(TwoSameStringsOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsFirstAsComplexCloakedValueContent {
                         FirstStringField: {
                         $id: 1,
                         CloakedRevealerFirstStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondStringField: {
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
                        TwoStringsFirstAsComplexCloakedValueContent {
                          FirstStringField: {
                            $id: 1,
                            CloakedRevealerFirstStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondStringField: {
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
                        "FirstStringField":{
                        "$id":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        },
                        "SecondStringField":{
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
                          "FirstStringField": {
                            "$id": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          },
                          "SecondStringField": {
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
    public void TwoSameStringsOneCloakedValueOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    public static InputBearerExpect<TwoStringsSecondAsComplexCloakedValueContent>
        TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringsSecondAsComplexCloakedValueContent>(TwoSameStringsOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsSecondAsComplexCloakedValueContent {
                         FirstStringField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringField: {
                         $ref: 1,
                         CloakedRevealerSecondStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
                        TwoStringsSecondAsComplexCloakedValueContent {
                          FirstStringField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringField: {
                            $ref: 1,
                            CloakedRevealerSecondStringField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
                        "FirstStringField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringField":{
                        "$ref":"1",
                        "CloakedRevealerSecondStringField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringField": {
                            "$ref": "1",
                            "CloakedRevealerSecondStringField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }


    public static InputBearerExpect<TwoStringsFirstAsComplexCloakedStringContent>
        TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringsOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringsFirstAsComplexCloakedStringContent>(TwoSameStringsOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsFirstAsComplexCloakedStringContent {
                         FirstStringField: {
                         $id: 1,
                         CloakedRevealerFirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondStringField: {
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
                        TwoStringsFirstAsComplexCloakedStringContent {
                          FirstStringField: {
                            $id: 1,
                            CloakedRevealerFirstStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondStringField: {
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
                        "FirstStringField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringField":{
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
                          "FirstStringField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringField": {
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
    public void TwoSameStringsOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }


    public static InputBearerExpect<TwoStringsSecondAsComplexCloakedStringContent>
        TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoStringsSecondAsComplexCloakedStringContent>(TwoSameStringsOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoStringsSecondAsComplexCloakedStringContent {
                         FirstStringField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondStringField: {
                         $ref: 1,
                         CloakedRevealerSecondStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyList: [ FirstCharSeq, SecondCharSeq, ThirdCharSeq ]
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoStringsSecondAsComplexCloakedStringContent {
                          FirstStringField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondStringField: {
                            $ref: 1,
                            CloakedRevealerSecondStringField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
                        "FirstStringField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondStringField":{
                        "$ref":"1",
                        "CloakedRevealerSecondStringField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstStringField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondStringField": {
                            "$ref": "1",
                            "CloakedRevealerSecondStringField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceMarkingIncludeStringContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking   = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeStringContents = true
             });
    }


    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeStringContents = true
             });
    }

    [TestMethod]
    public void TwoSameStringsOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameStringsOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeStringInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeStringContents = true
             });
    }
}
