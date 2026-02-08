// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.UnitFieldContentRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class CharSequenceUnitComplexContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>?
        twoCharArraysOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>?
        twoCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>?
        twoCharArraysOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>?
        twoCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect;

    private static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>?
        twoCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>?
        twoCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>?
        twoCharArraysOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>?
        twoCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect;

    private static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>?
        twoCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>?
        twoCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>?
        twoCharArraysOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>?
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

    public static TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence> TwoSameCharArraysOneComplexCloakedValueOneField
    {
        get
        {
            var repeated        = new CharArrayStringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoCharArraysOneCloakedOneFields =
                new TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>(repeated, repeated);
            return twoCharArraysOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>
        TwoSameCharArraysOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>(TwoSameCharArraysOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: {
                         CloakedRevealerFirstCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: {
                            CloakedRevealerFirstCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstCharSequenceField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.,
                        "SecondCharSequenceField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharSequenceField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.,
                          "SecondCharSequenceField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
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

    public static TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence> TwoSameCharArraysOneFieldOneComplexCloakedValue
    {
        get
        {
            var repeated        = new MutableString("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoCharArraysOneFieldsOneCloaked
                = new TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>(repeated, repeated);
            return twoCharArraysOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>
        TwoSameCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>(TwoSameCharArraysOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondCharSequenceField: {
                         CloakedRevealerSecondCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyMap: { FirstKey: 1, SecondKey: 2, ThirdKey: 3 }
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondCharSequenceField: {
                            CloakedRevealerSecondCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
                        "FirstCharSequenceField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondCharSequenceField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharSequenceField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondCharSequenceField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
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

    public static TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence> TwoSameCharArraysOneComplexCloakedStringOneField
    {
        get
        {
            var repeated        = new CharArrayStringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoCharArraysOneCloakedOneFields =
                new TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>(repeated, repeated);
            return twoCharArraysOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>
        TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>(TwoSameCharArraysOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: {
                         CloakedRevealerFirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: {
                            CloakedRevealerFirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstCharSequenceField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondCharSequenceField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharSequenceField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondCharSequenceField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
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

    public static TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence> TwoSameCharArraysOneFieldOneComplexCloakedString
    {
        get
        {
            var repeated        = new MutableString("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoCharArraysOneFieldsOneCloakedString
                = new TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>(repeated, repeated);
            return twoCharArraysOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>
        TwoSameCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>(TwoSameCharArraysOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondCharSequenceField: {
                         CloakedRevealerSecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
                        TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondCharSequenceField: {
                            CloakedRevealerSecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
                        "FirstCharSequenceField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondCharSequenceField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharSequenceField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondCharSequenceField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
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


    public static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>
        TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>(TwoSameCharArraysOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         CloakedRevealerFirstCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondCharSequenceField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            CloakedRevealerFirstCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondCharSequenceField: {
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        },
                        "SecondCharSequenceField":{
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
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          },
                          "SecondCharSequenceField": {
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
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedValueOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    public static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>
        TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>(TwoSameCharArraysOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondCharSequenceField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondCharSequenceField: {
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharSequenceField":{
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
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharSequenceField": {
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
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }


    public static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>
        TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>(TwoSameCharArraysOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         CloakedRevealerFirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondCharSequenceField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            CloakedRevealerFirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondCharSequenceField: {
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharSequenceField":{
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
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharSequenceField": {
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
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }


    public static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>
        TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>(TwoSameCharArraysOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondCharSequenceField: {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondCharSequenceField: {
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharSequenceField":{
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
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharSequenceField": {
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
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    public static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>
        TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence>>(TwoSameCharArraysOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         CloakedRevealerFirstCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondCharSequenceField: {
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
                        TwoCharSequencesFirstAsComplexCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            CloakedRevealerFirstCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondCharSequenceField: {
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        },
                        "SecondCharSequenceField":{
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
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                          },
                          "SecondCharSequenceField": {
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
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    public static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>
        TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence>>(TwoSameCharArraysOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondCharSequenceField: {
                         $ref: 1,
                         CloakedRevealerSecondCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
                        TwoCharSequencesSecondAsComplexCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondCharSequenceField: {
                            $ref: 1,
                            CloakedRevealerSecondCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharSequenceField":{
                        "$ref":"1",
                        "CloakedRevealerSecondCharSequenceField":You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharSequenceField": {
                            "$ref": "1",
                            "CloakedRevealerSecondCharSequenceField": You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.
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
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }


    public static InputBearerExpect<TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>
        TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoCharArraysOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence>>(TwoSameCharArraysOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         CloakedRevealerFirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondCharSequenceField: {
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
                        TwoCharSequencesFirstAsComplexCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            CloakedRevealerFirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondCharSequenceField: {
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharSequenceField":{
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
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharSequenceField": {
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
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents   = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
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
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = false
             });
    }


    public static InputBearerExpect<TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>
        TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence>>(TwoSameCharArraysOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondCharSequenceField: {
                         $ref: 1,
                         CloakedRevealerSecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         logOnlyList: [ FirstCharSeq, SecondCharSeq, ThirdCharSeq ]
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesSecondAsComplexCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondCharSequenceField: {
                            $ref: 1,
                            CloakedRevealerSecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharSequenceField":{
                        "$ref":"1",
                        "CloakedRevealerSecondCharSequenceField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharSequenceField": {
                            "$ref": "1",
                            "CloakedRevealerSecondCharSequenceField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
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
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
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
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
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
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking   = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }


    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoSameCharArraysOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameCharArraysOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceTrackingAllAsStringHaveLocalTracking  = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }
}
