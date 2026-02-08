// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.UnitFieldContentRevisitTests;

[NoMatchingProductionClass]
[TestClass]
public class ObjectUnitSimpleContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoObjectFields>? twoSameObjectFieldsWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>?
        twoSameObjectsOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>?
        twoSameObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedStringContent>?
        twoSameObjectsOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedStringContent>?
        twoSameObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect;
    
    private static InputBearerExpect<TwoObjectFields>? twoSameObjectFieldsWithToggleStyleAsStringLocalTrackingOnlyExpect;
    private static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>?
        twoSameObjectsOneSimpleCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>?
        twoSameObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedStringContent>?
        twoSameObjectsOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect;
    private static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedStringContent>?
        twoSameObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect;
    
    private static InputBearerExpect<TwoObjectFields>? twoSameFieldsShowRevisitsAndValuesExpect;
    private static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>?
        twoSameObjectsOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>?
        twoSameObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedStringContent>?
        twoSameObjectsOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedStringContent>?
        twoSameObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoObjectFields TwoObjectsFields
    {
        get
        {
            var repeated               = new MyOtherTypeClass( "You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameIpAddressFields = new TwoObjectFields(repeated, repeated);
            return twoSameIpAddressFields;
        }
    }

    public static InputBearerExpect<TwoObjectFields> TwoObjectsFieldsWithWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameObjectFieldsWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoObjectFields>(TwoObjectsFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectFields
                         {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectFields {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsFieldsWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsFieldsWithWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoObjectsFieldsWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsFieldsWithWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoObjectsFieldsWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsFieldsWithWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoObjectsFieldsWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsFieldsWithWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoObjectsFirstAsSimpleCloakedValueContent TwoObjectsOneSimpleCloakedValueOneField
    {
        get
        {
            var repeated        = new MyOtherTypeClass("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameObjectsOneCloakedOneFields =
                new TwoObjectsFirstAsSimpleCloakedValueContent(repeated, repeated);
            return twoSameObjectsOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>
        TwoObjectsOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameObjectsOneSimpleCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>(TwoObjectsOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedValueContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondObjectField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedValueContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":You may resume writing you're résumé, or see "Vee" with help with your CV.
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          "SecondObjectField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoObjectsOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoObjectsOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoObjectsOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneSimpleCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoObjectsSecondAsSimpleCloakedValueContent TwoObjectsOneFieldOneSimpleCloakedValue
    {
        get
        {
            var repeated        = new MyOtherTypeClass("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameObjectsOneFieldsOneCloaked
                = new TwoObjectsSecondAsSimpleCloakedValueContent(repeated, repeated);
            return twoSameObjectsOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>
        TwoObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>(TwoObjectsOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedValueContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedValueContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneFieldOneSimpleCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoObjectsFirstAsSimpleCloakedStringContent TwoObjectsOneSimpleCloakedStringOneField
    {
        get
        {
            var repeated        = new MyOtherTypeClass("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameObjectsOneCloakedOneFields =
                new TwoObjectsFirstAsSimpleCloakedStringContent(repeated, repeated);
            return twoSameObjectsOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedStringContent>
        TwoObjectsOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameObjectsOneSimpleCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoObjectsFirstAsSimpleCloakedStringContent>(TwoObjectsOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedStringContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedStringContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondObjectField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstObjectField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondObjectField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneSimpleCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoObjectsSecondAsSimpleCloakedStringContent TwoObjectsOneFieldOneSimpleCloakedString
    {
        get
        {
            var repeated        = new MyOtherTypeClass("You may resume writing you're résumé, or see \"Vee\" with help with your CV.");
            var twoSameObjectsOneFieldsOneCloakedString
                = new TwoObjectsSecondAsSimpleCloakedStringContent(repeated, repeated);
            return twoSameObjectsOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedStringContent>
        TwoObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoObjectsSecondAsSimpleCloakedStringContent>(TwoObjectsOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedStringContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedStringContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                        "SecondObjectField":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstObjectField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV.",
                          "SecondObjectField": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoObjectsOneFieldOneSimpleCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }
    
    public static InputBearerExpect<TwoObjectFields> TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpect
    {
        get
        {
            return twoSameObjectFieldsWithToggleStyleAsStringLocalTrackingOnlyExpect ??=
                new InputBearerExpect<TwoObjectFields>(TwoObjectsFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectFields {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectFields {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpectCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpectCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpectPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpectPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsFieldsWithToggleStyleAsStringLocalTrackingOnlyExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
    
    public static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>
        TwoObjectsOneSimpleCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameObjectsOneSimpleCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>(TwoObjectsOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedValueContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondObjectField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedValueContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":You may resume writing you're résumé, or see "Vee" with help with your CV.
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          "SecondObjectField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingExpect
            , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoObjectsOneSimpleValueOneFieldWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneSimpleValueOneFieldWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedValueOneFieldWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
    
    public static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>
        TwoObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>(TwoObjectsOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedValueContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedValueContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedValueWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedValueWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedValueWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
    
    public static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedStringContent>
        TwoObjectsOneSimpleCloakedStringOneFieldsWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameObjectsOneSimpleCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<
                    TwoObjectsFirstAsSimpleCloakedStringContent>(TwoObjectsOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedStringContent {
                         FirstObjectField: (MyOtherTypeClass) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedStringContent {
                          FirstObjectField: (MyOtherTypeClass) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedStringOneFieldsWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedStringOneFieldsWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedStringOneFieldsWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedStringOneFieldsWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
    
    public static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedStringContent>
        TwoObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
    {
        get
        {
            return twoSameObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect ??=
                new InputBearerExpect<
                    TwoObjectsSecondAsSimpleCloakedStringContent>(TwoObjectsOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedStringContent {
                         FirstObjectField: (MyOtherTypeClass) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) "You may resume writing you're résumé, or see "Vee" with help with your CV."
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedStringContent {
                          FirstObjectField: (MyOtherTypeClass) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) "You may resume writing you're résumé, or see "Vee" with help with your CV."
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedStringWithToggleStyleAsStringLocalTrackingCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedStringWithToggleStyleAsStringLocalTrackingCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedStringWithToggleStyleAsStringLocalTrackingExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
             });
    }
    
    public static InputBearerExpect<TwoObjectFields> TwoObjectsFieldsShowRevisitsAndValuesExpect
    {
        get
        {
            return twoSameFieldsShowRevisitsAndValuesExpect ??=
                new InputBearerExpect<TwoObjectFields>(TwoObjectsFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectFields {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
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
                        TwoObjectFields {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
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
    public void TwoObjectsFieldsShowRevisitsAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsFieldsShowRevisitsAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsFieldsShowRevisitsAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsFieldsShowRevisitsAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }
    
    public static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>
        TwoObjectsOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameObjectsOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoObjectsFirstAsSimpleCloakedValueContent>(TwoObjectsOneSimpleCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedValueContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) You may resume writing you're résumé, or see "Vee" with help with your CV.,
                         SecondObjectField: (MyOtherTypeClass) {
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
                        TwoObjectsFirstAsSimpleCloakedValueContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) You may resume writing you're résumé, or see "Vee" with help with your CV.,
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":You may resume writing you're résumé, or see "Vee" with help with your CV.
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": You may resume writing you're résumé, or see "Vee" with help with your CV.
                          },
                          "SecondObjectField": {
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
    public void TwoObjectsOneCloakedValueOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
            , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }
    
    public static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>
        TwoObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<TwoObjectsSecondAsSimpleCloakedValueContent>(TwoObjectsOneFieldOneSimpleCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedValueContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
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
                        TwoObjectsSecondAsSimpleCloakedValueContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
                        "$ref":"1",
                        "$values":You may resume writing you're résumé, or see "Vee" with help with your CV.
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
                            "$ref": "1",
                            "$values": You may resume writing you're résumé, or see "Vee" with help with your CV.
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }
    
    public static InputBearerExpect<TwoObjectsFirstAsSimpleCloakedStringContent>
        TwoObjectsOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameObjectsOneSimpleCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoObjectsFirstAsSimpleCloakedStringContent>(TwoObjectsOneSimpleCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsFirstAsSimpleCloakedStringContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
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
                        TwoObjectsFirstAsSimpleCloakedStringContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
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
    public void TwoObjectsOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true
             });
    }

    [TestMethod]
    public void TwoObjectsOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneSimpleCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }
    
    public static InputBearerExpect<TwoObjectsSecondAsSimpleCloakedStringContent>
        TwoObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoObjectsSecondAsSimpleCloakedStringContent>(TwoObjectsOneFieldOneSimpleCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoObjectsSecondAsSimpleCloakedStringContent {
                         FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                         SecondObjectField: (MyOtherTypeClass) {
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
                        TwoObjectsSecondAsSimpleCloakedStringContent {
                          FirstObjectField: (MyOtherTypeClass($id: 1)) "You may resume writing you're résumé, or see "Vee" with help with your CV.",
                          SecondObjectField: (MyOtherTypeClass) {
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
                        "FirstObjectField":{
                        "$id":"1",
                        "$values":"You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                        },
                        "SecondObjectField":{
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
                          "FirstObjectField": {
                            "$id": "1",
                            "$values": "You may resume writing you're r\u00e9sum\u00e9, or see \u0022Vee\u0022 with help with your CV."
                          },
                          "SecondObjectField": {
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
    public void TwoObjectsOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceMarkingIncludeObjectToStringContents = true 
             });
    }

    [TestMethod]
    public void TwoObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoObjectsOneFieldOneSimpleCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingAllAsStringHaveLocalTracking = false
               , InstanceMarkingIncludeObjectToStringContents = true 
             });
    }
}
