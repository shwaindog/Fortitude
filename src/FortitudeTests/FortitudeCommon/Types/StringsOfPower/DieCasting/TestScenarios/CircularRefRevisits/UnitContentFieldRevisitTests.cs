// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits;

[NoMatchingProductionClass]
[TestClass]
public class UnitContentFieldRevisitTests : CommonStyleExpectationTestBase
{
    private static InputBearerExpect<TwoSpanFormattableFields<IPAddress>>? twoSameIpAddressFieldsDefaultRevisitSettingsExpect;
    private static InputBearerExpect<TwoSpanFormattableFields<IPAddress>>? twoIpAddressFieldsShowRevisitInstanceIdsOnlyExpect;
    private static InputBearerExpect<TwoSpanFormattableFields<IPAddress>>? twoIpAddressFieldsShowRevisitsAndValuesExpect;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public override string TestsCommonDescription => "Unit field revisits";

    [TestInitialize]
    public void Setup()
    {
        Node.ResetInstanceIds();
    }

    public static TwoSpanFormattableFields<IPAddress> TwoIpAddressFields
    {
        get
        {
            var loopbackAddress        = IPAddress.Loopback;
            var twoSameIpAddressFields = new TwoSpanFormattableFields<IPAddress>(loopbackAddress, loopbackAddress);
            return twoSameIpAddressFields;
        }
    }

    public static InputBearerExpect<TwoSpanFormattableFields<IPAddress>> TwoIpAddressFieldsWithDefaultRevisitSettingsExpect
    {
        get
        {
            return twoSameIpAddressFieldsDefaultRevisitSettingsExpect ??=
                new InputBearerExpect<TwoSpanFormattableFields<IPAddress>>(TwoIpAddressFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFields<IPAddress>
                         {
                         FirstSpanFormattableField: 127.0.0.1,
                         SecondSpanFormattableField: 127.0.0.1
                         }
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, PrettyLog)
                      , """
                        TwoSpanFormattableFields<IPAddress> {
                          FirstSpanFormattableField: 127.0.0.1,
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
    public void TwoIpAddressFieldsWithDefaultRevisitSettingsCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoIpAddressFieldsWithDefaultRevisitSettingsExpect, CompactLog);
    }

    [TestMethod]
    public void TwoIpAddressFieldsWithDefaultRevisitSettingsCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoIpAddressFieldsWithDefaultRevisitSettingsExpect, CompactJson);
    }

    [TestMethod]
    public void TwoIpAddressFieldsWithDefaultRevisitSettingsPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoIpAddressFieldsWithDefaultRevisitSettingsExpect, PrettyLog);
    }

    [TestMethod]
    public void TwoIpAddressFieldsWithDefaultRevisitSettingsPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectation(TwoIpAddressFieldsWithDefaultRevisitSettingsExpect, PrettyJson);
    }

    public static InputBearerExpect<TwoSpanFormattableFields<IPAddress>> TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpect
    {
        get
        {
            return twoIpAddressFieldsShowRevisitInstanceIdsOnlyExpect ??=
                new InputBearerExpect<TwoSpanFormattableFields<IPAddress>>(TwoIpAddressFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFields<IPAddress>
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
                        TwoSpanFormattableFields<IPAddress> {
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
                        "FirstSpanFormattableField":{
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
    public void TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpectCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(CompactLog)
             {
                 MarkRevisitedSpanFormattableClassInstances = true
             });
    }

    [TestMethod]
    public void TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpectCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(CompactJson)
             {
                 MarkRevisitedSpanFormattableClassInstances = true
             });
    }

    [TestMethod]
    public void TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpectPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(PrettyLog)
             {
                 MarkRevisitedSpanFormattableClassInstances = true
             });
    }

    [TestMethod]
    public void TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpectPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoIpAddressFieldsShowRevisitInstanceIdsOnlyExpect
           , new StyleOptions(PrettyJson)
             {
                 MarkRevisitedSpanFormattableClassInstances = true
             });
    }
    
    public static InputBearerExpect<TwoSpanFormattableFields<IPAddress>> TwoIpAddressFieldsShowRevisitsAndValuesExpect
    {
        get
        {
            return twoIpAddressFieldsShowRevisitsAndValuesExpect ??=
                new InputBearerExpect<TwoSpanFormattableFields<IPAddress>>(TwoIpAddressFields)
                {
                    {
                        new EK(AlwaysWrites | AcceptsStringBearer, CompactLog)
                      , """
                        TwoSpanFormattableFields<IPAddress>
                         {
                         FirstSpanFormattableField:
                         {
                         $id: 1,
                         $values: 127.0.0.1
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
                        TwoSpanFormattableFields<IPAddress> {
                          FirstSpanFormattableField: {
                            $id: 1,
                            $values: 127.0.0.1
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
                        "FirstSpanFormattableField":{
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
    public void TwoIpAddressFieldsShowRevisitsAndValuesCompactLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoIpAddressFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactLog)
             {
                 MarkRevisitedSpanFormattableClassInstances = true
                , IncludeSpanFormattableContentsOnRevisits = true 
             });
    }

    [TestMethod]
    public void TwoIpAddressFieldsShowRevisitsAndValuesCompactJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoIpAddressFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(CompactJson)
             {
                 MarkRevisitedSpanFormattableClassInstances = true
               , IncludeSpanFormattableContentsOnRevisits   = true 
             });
    }

    [TestMethod]
    public void TwoIpAddressFieldsShowRevisitsAndValuesPrettyLogFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoIpAddressFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyLog)
             {
                 MarkRevisitedSpanFormattableClassInstances = true
               , IncludeSpanFormattableContentsOnRevisits   = true 
             });
    }

    [TestMethod]
    public void TwoIpAddressFieldsShowRevisitsAndValuesPrettyJsonFormatTest()
    {
        ExecuteIndividualScaffoldExpectationWithOptions
            (TwoIpAddressFieldsShowRevisitsAndValuesExpect
           , new StyleOptions(PrettyJson)
             {
                 MarkRevisitedSpanFormattableClassInstances = true
               , IncludeSpanFormattableContentsOnRevisits   = true 
             });
    }
}
