// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
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
public class SpanFormattableUnitComplexContentAndFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect;

    private static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect;

    private static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect;
    private static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>?
        twoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress> TwoSameIpAddressesOneComplexCloakedValueOneField
    {
        get
        {
            var loopbackAddress = IPAddress.Loopback;
            var twoSameIpAddressesOneCloakedOneFields =
                new TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>(loopbackAddress, loopbackAddress);
            return twoSameIpAddressesOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameIpAddressesOneComplexCloakedValueOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         CloakedRevealerFirstSpanFormattableField: 127.0.0.1,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondSpanFormattableField: 127.0.0.1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: {
                            CloakedRevealerFirstSpanFormattableField: 127.0.0.1,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondSpanFormattableField: 127.0.0.1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":"127.0.0.1",
                        "SecondSpanFormattableField":"127.0.0.1"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": "127.0.0.1",
                          "SecondSpanFormattableField": "127.0.0.1"
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedValueOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedValueOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneComplexCloakedValueOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress> TwoSameIpAddressesOneFieldOneComplexCloakedValue
    {
        get
        {
            var loopbackAddress = IPAddress.Loopback;
            var twoSameIpAddressesOneFieldsOneCloaked
                = new TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>(loopbackAddress, loopbackAddress);
            return twoSameIpAddressesOneFieldsOneCloaked;
        }
    }

    public static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField: 127.0.0.1,
                         SecondSpanFormattableField:
                         {
                         CloakedRevealerSecondSpanFormattableField: 127.0.0.1,
                         logOnlyMap: { FirstKey: 1, SecondKey: 2, ThirdKey: 3 }
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: 127.0.0.1,
                          SecondSpanFormattableField: {
                            CloakedRevealerSecondSpanFormattableField: 127.0.0.1,
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
                        "FirstSpanFormattableField":"127.0.0.1",
                        "SecondSpanFormattableField":"127.0.0.1"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": "127.0.0.1",
                          "SecondSpanFormattableField": "127.0.0.1"
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneComplexCloakedValueWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress> TwoSameIpAddressesOneComplexCloakedStringOneField
    {
        get
        {
            var loopbackAddress = IPAddress.Loopback;
            var twoSameIpAddressesOneCloakedOneFields =
                new TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>(loopbackAddress, loopbackAddress);
            return twoSameIpAddressesOneCloakedOneFields;
        }
    }

    public static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameIpAddressesOneComplexCloakedStringOneFieldWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         CloakedRevealerFirstSpanFormattableField: "127.0.0.1",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondSpanFormattableField: 127.0.0.1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            CloakedRevealerFirstSpanFormattableField: "127.0.0.1",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondSpanFormattableField: 127.0.0.1
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":"127.0.0.1",
                        "SecondSpanFormattableField":"127.0.0.1"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": "127.0.0.1",
                          "SecondSpanFormattableField": "127.0.0.1"
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress> TwoSameIpAddressesOneFieldOneComplexCloakedString
    {
        get
        {
            var loopbackAddress = IPAddress.Loopback;
            var twoSameIpAddressesOneFieldsOneCloakedString
                = new TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>(loopbackAddress, loopbackAddress);
            return twoSameIpAddressesOneFieldsOneCloakedString;
        }
    }

    public static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField: 127.0.0.1,
                         SecondSpanFormattableField:
                         {
                         CloakedRevealerSecondSpanFormattableField: "127.0.0.1",
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
                        TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: 127.0.0.1,
                          SecondSpanFormattableField: {
                            CloakedRevealerSecondSpanFormattableField: "127.0.0.1",
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
                        "FirstSpanFormattableField":"127.0.0.1",
                        "SecondSpanFormattableField":"127.0.0.1"
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": "127.0.0.1",
                          "SecondSpanFormattableField": "127.0.0.1"
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoSameIpAddressesOneFieldOneComplexCloakedStringWithDefaultRevisitSettingsExpect, PrettyJson);
    }


    public static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         CloakedRevealerFirstSpanFormattableField: 127.0.0.1,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondSpanFormattableField:
                         {
                         $ref:
                         1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            CloakedRevealerFirstSpanFormattableField: 127.0.0.1,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondSpanFormattableField: {
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
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":
                        "1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedValueOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedValueOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneSimpleValueOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    public static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
                         },
                         SecondSpanFormattableField:
                         {
                         $ref:
                         1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
                          },
                          SecondSpanFormattableField: {
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
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
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
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }


    public static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameIpAddressesOneComplexCloakedStringOneFieldShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         CloakedRevealerFirstSpanFormattableField: "127.0.0.1",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondSpanFormattableField:
                         {
                         $ref:
                         1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            CloakedRevealerFirstSpanFormattableField: "127.0.0.1",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondSpanFormattableField: {
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
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":
                        "1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }


    public static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
                          },
                          SecondSpanFormattableField: {
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
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":
                        "1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitInstanceIdsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitInstanceIdsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitInstanceIdsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
             });
    }

    public static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneComplexCloakedValueOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         CloakedRevealerFirstSpanFormattableField: 127.0.0.1,
                         logOnlyArray: [ 1, 2, 3 ]
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1,
                         $values: 127.0.0.1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            CloakedRevealerFirstSpanFormattableField: 127.0.0.1,
                            logOnlyArray: [
                              1,
                              2,
                              3
                            ]
                          },
                          SecondSpanFormattableField: {
                            $ref: 1,
                            $values: 127.0.0.1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":"1",
                        "$values":"127.0.0.1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1",
                            "$values": "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedValueOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedValueOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneSimpleValueOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneSimpleValueOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedValueOneFieldShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    public static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>>(TwoSameIpAddressesOneFieldOneComplexCloakedValue)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1,
                         CloakedRevealerSecondSpanFormattableField: 127.0.0.1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedValueContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
                          },
                          SecondSpanFormattableField: {
                            $ref: 1,
                            CloakedRevealerSecondSpanFormattableField: 127.0.0.1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":"1",
                        "CloakedRevealerSecondSpanFormattableField":"127.0.0.1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1",
                            "CloakedRevealerSecondSpanFormattableField": "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedValueShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedValueShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }


    public static InputBearerExpect<TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameIpAddressesOneComplexCloakedStringOneFieldShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneComplexCloakedStringOneField)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         CloakedRevealerFirstSpanFormattableField: "127.0.0.1",
                         logOnlyStringBuilder: "For your eyes only"
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1,
                         $values: 127.0.0.1
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFirstAsComplexCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            CloakedRevealerFirstSpanFormattableField: "127.0.0.1",
                            logOnlyStringBuilder: "For your eyes only"
                          },
                          SecondSpanFormattableField: {
                            $ref: 1,
                            $values: 127.0.0.1
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":"1",
                        "$values":"127.0.0.1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1",
                            "$values": "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneCloakedStringOneFieldShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }


    public static InputBearerExpect<TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>
        TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
    {
        get
        {
            return twoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect ??=
                new InputBearerExpect<
                    TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>>(TwoSameIpAddressesOneFieldOneComplexCloakedString)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
                         },
                         SecondSpanFormattableField:
                         {
                         $ref: 1,
                         CloakedRevealerSecondSpanFormattableField: "127.0.0.1"
                         }
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableSecondAsComplexCloakedStringContent<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
                          },
                          SecondSpanFormattableField: {
                            $ref: 1,
                            CloakedRevealerSecondSpanFormattableField: "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactJson)
                      , """ 
                        {
                        "FirstSpanFormattableField":
                        {
                        "$id":"1",
                        "$values":"127.0.0.1"
                        },
                        "SecondSpanFormattableField":
                        {
                        "$ref":"1",
                        "CloakedRevealerSecondSpanFormattableField":"127.0.0.1"
                        }
                        }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyJson)
                      , """ 
                        {
                          "FirstSpanFormattableField": {
                            "$id": "1",
                            "$values": "127.0.0.1"
                          },
                          "SecondSpanFormattableField": {
                            "$ref": "1",
                            "CloakedRevealerSecondSpanFormattableField": "127.0.0.1"
                          }
                        }
                        """.Dos2Unix()
                    }
                };
        }
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceTrackingAllAsStringClassesAreExempt   = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneCloakedStringShowRevisitAndValuesButAsStringRevisitExemptCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(CompactJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceTrackingAllAsStringClassesAreExempt   = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }


    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyLog)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceTrackingAllAsStringClassesAreExempt   = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }

    [TestMethod]
    public void TwoSameIpAddressesOneFieldOneComplexCloakedStringShowRevisitAndValuesButAsStringRevisitExemptPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoSameIpAddressesOneComplexCloakedStringOneFieldsWithDefaultRevisitSettingsExpect
           , new StyleOptions(PrettyJson)
             {
                 InstanceTrackingIncludeSpanFormattableClasses = true
               , InstanceTrackingAllAsStringClassesAreExempt   = true
               , InstanceMarkingIncludeSpanFormattableContents = true
             });
    }
}
