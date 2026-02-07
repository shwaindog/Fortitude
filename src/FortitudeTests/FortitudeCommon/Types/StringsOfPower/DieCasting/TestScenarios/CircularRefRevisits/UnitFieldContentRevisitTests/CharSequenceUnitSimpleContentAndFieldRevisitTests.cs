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
public class CharSequenceUnitSimpleContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoCharSequenceFields<ICharSequence>>? twoSameCharSequenceFieldsWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>?
        twoSameCharSequencesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>?
        twoSameCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>?
        twoSameCharSequencesOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>?
        twoSameCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect;
    
    private static InputBearerExpect<TwoCharSequenceFields<ICharSequence>>? twoSameCharSequenceFieldsShowRevisitInstanceIdsOnlyExpect;
    private static InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>?
        twoSameCharSequencesOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>?
        twoSameCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>?
        twoSameCharSequencesOneSimpleCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>?
        twoSameCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect;
    
    private static InputBearerExpect<TwoCharSequenceFields<ICharSequence>>? twoSameCharSequenceFieldsShowRevisitsAndValuesExpect;
    private static InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>?
        twoSameCharSequencesOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>?
        twoSameCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>?
        twoSameCharSequencesOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>?
        twoSameCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoCharSequenceFields<ICharSequence> TwoCharSequencesFields
    {
        get
        {
            var repeated        = new MutableString("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameIpAddressFields = new TwoCharSequenceFields<ICharSequence>(repeated, repeated);
            return twoSameIpAddressFields;
        }
    }

    public static InputBearerExpect<TwoCharSequenceFields<ICharSequence>> TwoCharSequencesFieldsWithWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharSequenceFieldsWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoCharSequenceFields<ICharSequence>>(TwoCharSequencesFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequenceFields<ICharSequence>
                         {
                         FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequenceFields<ICharSequence> {
                          FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
    public void TwoCharSequencesFieldsWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesFieldsWithWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharSequencesFieldsWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesFieldsWithWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharSequencesFieldsWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesFieldsWithWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharSequencesFieldsWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesFieldsWithWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence> TwoCharSequencesOneSimpleCloakedValueOneField
    {
        get
        {
            var repeated        = new CharArrayStringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameCharSequencesOneCloakedOneFields =
                new TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>(repeated, repeated);
            return twoSameCharSequencesOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>
        TwoCharSequencesOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharSequencesOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>(TwoCharSequencesOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>
                         {
                         FirstCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.,
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
    public void TwoCharSequencesOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence> TwoCharSequencesOneFieldOneSimpleCloakedValue
    {
        get
        {
            var repeated        = new MutableString("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameCharSequencesOneFieldsOneCloaked
                = new TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>(repeated, repeated);
            return twoSameCharSequencesOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>
        TwoCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>(TwoCharSequencesOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>
                         {
                         FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondCharSequenceField: You may resume writing you're résumé, or see "Vee" with help with your CV.
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
    public void TwoCharSequencesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence> TwoCharSequencesOneSimpleCloakedStringOneField
    {
        get
        {
            var repeated        = new CharArrayStringBuilder("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameCharSequencesOneCloakedOneFields =
                new TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>(repeated, repeated);
            return twoSameCharSequencesOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>
        TwoCharSequencesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharSequencesOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>(TwoCharSequencesOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
    public void TwoCharSequencesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence> TwoCharSequencesOneFieldOneSimpleCloakedString
    {
        get
        {
            var repeated        = new MutableString("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameCharSequencesOneFieldsOneCloakedString
                = new TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>(repeated, repeated);
            return twoSameCharSequencesOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>
        TwoCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>(TwoCharSequencesOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: "You may resume writing you're résumé, or see "Vee" with help with your CV.",
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
    public void TwoCharSequencesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoCharSequencesOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }
    
    public static InputBearerExpect<TwoCharSequenceFields<ICharSequence>> TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpect
    {
        get
        {
            return twoSameCharSequenceFieldsShowRevisitInstanceIdsOnlyExpect ??=
                new InputBearerExpect<TwoCharSequenceFields<ICharSequence>>(TwoCharSequencesFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequenceFields<ICharSequence> {
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
                        TwoCharSequenceFields<ICharSequence> {
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
    public void TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpectCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpectCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances        = true
                , InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpectPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpectPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances       = true
               , InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
    
    public static InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>
        TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameCharSequencesOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>(TwoCharSequencesOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
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
                        TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
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
    public void TwoCharSequencesOneCloakedValueOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedValueOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
            , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }
    
    public static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>
        TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>(TwoCharSequencesOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence> {
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
                        TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence> {
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
    public void TwoCharSequencesOneFieldOneCloakedValueShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }
    
    public static InputBearerExpect<TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>
        TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameCharSequencesOneSimpleCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>(TwoCharSequencesOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence> {
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
                        TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence> {
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
                        "FirstCharSequenceField":
                        {
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharSequenceField":
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
    public void TwoCharSequencesOneCloakedStringOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }
    
    public static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>
        TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>(TwoCharSequencesOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence> {
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
                        TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence> {
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
    public void TwoCharSequencesOneFieldOneCloakedStringShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
             });
    }
    
    public static InputBearerExpect<TwoCharSequenceFields<ICharSequence>> TwoCharSequencesFieldsShowRevisitsAndValuesExpect
    {
        get
        {
            return twoSameCharSequenceFieldsShowRevisitsAndValuesExpect ??=
                new InputBearerExpect<TwoCharSequenceFields<ICharSequence>>(TwoCharSequencesFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequenceFields<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
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
                        TwoCharSequenceFields<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
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
    public void TwoCharSequencesFieldsShowRevisitsAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances  = true
                , InstanceMarkingIncludeCharSequenceContents = true 
             });
    }

    [TestMethod]
    public void TwoCharSequencesFieldsShowRevisitsAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true 
             });
    }

    [TestMethod]
    public void TwoCharSequencesFieldsShowRevisitsAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true 
             });
    }

    [TestMethod]
    public void TwoCharSequencesFieldsShowRevisitsAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents      = true 
             });
    }
    
    public static InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>
        TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameCharSequencesOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence>>(TwoCharSequencesOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
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
                        TwoCharSequencesFirstAsSimpleCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: You may resume writing you're résumé, or see "Vee" with help with your CV.
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
    public void TwoCharSequencesOneCloakedValueOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
                , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
            , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }
    
    public static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>
        TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence>>(TwoCharSequencesOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         },
                         SecondCharSequenceField: {
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
                        TwoCharSequencesSecondAsSimpleCloakedValueContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
                          },
                          SecondCharSequenceField: {
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
                        "FirstCharSequenceField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondCharSequenceField":{
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
                          "FirstCharSequenceField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondCharSequenceField": {
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
    public void TwoCharSequencesOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }
    
    public static InputBearerExpect<TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>
        TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameCharSequencesOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence>>(TwoCharSequencesOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
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
                        TwCharSequencesFirstAsSimpleCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
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
    public void TwoCharSequencesOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }
    
    public static InputBearerExpect<TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>
        TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence>>(TwoCharSequencesOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence> {
                         FirstCharSequenceField: {
                         $id: 1,
                         $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
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
                        TwoCharSequencesSecondAsSimpleCloakedStringContent<ICharSequence> {
                          FirstCharSequenceField: {
                            $id: 1,
                            $values: "You may resume writing you're résumé, or see "Vee" with help with your CV."
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
    public void TwoCharSequencesOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }

    [TestMethod]
    public void TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoCharSequencesOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeCharSequenceInstances = true
               , InstanceMarkingIncludeCharSequenceContents = true
             });
    }
}
