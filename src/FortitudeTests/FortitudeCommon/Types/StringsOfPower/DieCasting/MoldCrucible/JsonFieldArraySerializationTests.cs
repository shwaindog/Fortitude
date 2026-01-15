using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Scenarios.CompareToSystemTextJson.TypePermutation;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CompareToSystemTextJson.TextJsonConverters;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

[TestClass]
[NoMatchingProductionClass]
public class JsonFieldArraySerializationTests
{
    private JsonSerializerOptions jsonMatchOneStringSerializerOptions = null!;

    private static IVersatileFLogger logger = null!;

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

        logger = FLog.FLoggerForType.As<IVersatileFLogger>();
    }

    [TestInitialize]
    public void Setup()
    {
        jsonMatchOneStringSerializerOptions = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals
          , IncludeFields  = true
          , Converters =
            {
                new IPAddressConverter()
              , new IPNetworkConverter()
              , new ComplexConverter()
              , new RuneConverter()
              , new StringBuilderConverter()
              , new TestCustomSpanFormattableConverter()
              , new HalfConverter()  
              , new NullableHalfConverter()  
              , new Int128Converter()  
              , new NullableInt128Converter()  
              , new UInt128Converter()  
              , new NullableUInt128Converter()  
              , new BigIntegerConverter()
              , new NullableBigIntegerConverter()
              , new JsonStringEnumConverter<NoDefaultLongNoFlagsEnum>()
              , new JsonStringEnumConverter<NoDefaultULongNoFlagsEnum>()
              , new JsonStringEnumConverter<WithDefaultLongNoFlagsEnum>()
              , new JsonStringEnumConverter<WithDefaultULongNoFlagsEnum>()
              , new JsonStringEnumConverter<NoDefaultLongWithFlagsEnum>()
              , new JsonStringEnumConverter<NoDefaultULongWithFlagsEnum>()
              , new JsonStringEnumConverter<WithDefaultLongWithFlagsEnum>()
              , new JsonStringEnumConverter<WithDefaultULongWithFlagsEnum>()
            }
        };
    }

    [TestMethod]
    public void ArrayFieldsAddAlwaysTheOneStringJsonCompactMatchesTextJsonSerialize()
    {
        var arrayPropertyFieldClass = new StandardArrayPropertyFieldClass();

        jsonMatchOneStringSerializerOptions.Encoder                = new MinimalTextJsonEncoder();
        var textJsonStringify = JsonSerializer.Serialize(arrayPropertyFieldClass, jsonMatchOneStringSerializerOptions);

        logger.WrnApnd("Json Serializer")?.Args("\n");
        logger.InfApnd(textJsonStringify)?.Args("\n");

        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(new StyleOptions
        {
            Style                          = StringStyle.Json | StringStyle.Compact
          , JsonEncodingTransferType       = JsonEncodingTransferType.BkSlEscCtrlCharsDblQtAndBkSlOnly
          , WriteKeyValuePairsAsCollection = true
        });
        arrayPropertyFieldClass.RevealState(styledStringBuilder);
        var oneStringify = styledStringBuilder.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd(oneStringify)?.Args("\n");

        oneStringify.Should().BeEquivalentTo(textJsonStringify);
    }

    [TestMethod]
    public void AllDefaultArrayFieldsAlwaysAddAllTheOneStringJsonCompactMatchesTextJsonSerialize()
    {
        var arrayPropertyFieldClass = new StandardArrayPropertyFieldClass();
        arrayPropertyFieldClass.InitializeAllNull();

        var textJsonStringify = JsonSerializer.Serialize(arrayPropertyFieldClass, jsonMatchOneStringSerializerOptions);

        logger.WrnApnd("Json Serializer")?.Args("\n");
        logger.InfApnd(textJsonStringify)?.Args("\n");

        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(new StyleOptions
        {
            Style                          = StringStyle.Json | StringStyle.Compact
          , JsonEncodingTransferType       = JsonEncodingTransferType.UniCdEscCtrlCharsDblQtAndNonAscii
          , WriteKeyValuePairsAsCollection = true
        });
        arrayPropertyFieldClass.RevealState(styledStringBuilder);
        var oneStringify = styledStringBuilder.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd(oneStringify)?.Args("\n");

        oneStringify.Should().BeEquivalentTo(textJsonStringify);
    }

    [TestMethod]
    public void AllDefaultArrayFieldsWhenPopulatedTheOneStringJsonCompactMatchesTextJsonSerialize()
    {
        var arrayPropertyFieldClass = new StandardArrayPropertyFieldClass();
        arrayPropertyFieldClass.InitializeAllNull();
        arrayPropertyFieldClass.TestCollectionFieldRevealMode = TestCollectionFieldRevealMode.WhenPopulated;
        
        jsonMatchOneStringSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        
        var textJsonStringify = JsonSerializer.Serialize(arrayPropertyFieldClass, jsonMatchOneStringSerializerOptions);

        logger.WrnApnd("Json Serializer")?.Args("\n");
        logger.InfApnd(textJsonStringify)?.Args("\n");

        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(new StyleOptions
        {
            Style                          = StringStyle.Json | StringStyle.Compact
          , JsonEncodingTransferType       = JsonEncodingTransferType.UniCdEscCtrlCharsDblQtAndNonAscii
          , WriteKeyValuePairsAsCollection = true
        });
        arrayPropertyFieldClass.RevealState(styledStringBuilder);
        var oneStringify = styledStringBuilder.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd(oneStringify)?.Args("\n");

        oneStringify.Should().BeEquivalentTo(textJsonStringify);
    }

    [TestMethod]
    public void ArrayFieldsAlwaysAddAllTheOneStringJsonPrettyMatchesTextJsonSerialize()
    {
        var arrayPropertyFieldClass = new StandardArrayPropertyFieldClass();

        jsonMatchOneStringSerializerOptions.WriteIndented = true;
        jsonMatchOneStringSerializerOptions.Encoder       = new MinimalTextJsonEncoder();

        arrayPropertyFieldClass.TestCollectionFieldRevealMode = TestCollectionFieldRevealMode.AlwaysAddAll;
        
        var textJsonStringify =
            JsonSerializer.Serialize
                (arrayPropertyFieldClass, jsonMatchOneStringSerializerOptions);

        logger.WrnApnd("Json Serializer")?.Args("\n");
        logger.InfApnd("\n")?.Args(textJsonStringify, "\n");

        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(new StyleOptions
        {
            Style = StringStyle.Json | StringStyle.Pretty
          , JsonEncodingTransferType       = JsonEncodingTransferType.BkSlEscCtrlCharsDblQtAndBkSlOnly
          , WriteKeyValuePairsAsCollection = true
        });
        arrayPropertyFieldClass.RevealState(styledStringBuilder);
        var oneStringify = styledStringBuilder.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd("\n")?.Args(oneStringify, "\n");

        oneStringify.Should().BeEquivalentTo(textJsonStringify);
    }

    [TestMethod]
    public void ArrayFieldsWhenNonDefaultTheOneStringJsonPrettyMatchesTextJsonSerialize()
    {
        var arrayPropertyFieldClass = new StandardArrayPropertyFieldClass();
        
        arrayPropertyFieldClass.TestCollectionFieldRevealMode = TestCollectionFieldRevealMode.WhenPopulated;
        var theOneString = new TheOneString();
        theOneString.ClearAndReinitialize(new StyleOptions
        {
            Style                          = StringStyle.Json | StringStyle.Pretty
          , JsonEncodingTransferType       = JsonEncodingTransferType.BkSlEscCtrlCharsDblQtAndBkSlOnly
          , WriteKeyValuePairsAsCollection = true
        });
        arrayPropertyFieldClass.RevealState(theOneString);
        
        jsonMatchOneStringSerializerOptions.WriteIndented          = true;
        jsonMatchOneStringSerializerOptions.Encoder                = new MinimalTextJsonEncoder();
        jsonMatchOneStringSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        
        var singlePropertyFieldClassStringified = theOneString.WriteBuffer.ToString();
        
        logger.TrcApnd("Original Json String")?.Args("\n");
        logger.DbgApnd("\n")?.Args(singlePropertyFieldClassStringified, "\n");

        var jsonStringPropertyClassStringified =
            JsonSerializer.Serialize
                (arrayPropertyFieldClass, jsonMatchOneStringSerializerOptions);
        logger.WrnApnd("Json Serializer")?.Args("\n");
        logger.InfApnd("\n")?.Args(jsonStringPropertyClassStringified, "\n");

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd("\n")?.Args(singlePropertyFieldClassStringified, "\n");

        singlePropertyFieldClassStringified.Should().BeEquivalentTo(jsonStringPropertyClassStringified);
    }

    [TestMethod]
    public void JsonStringAddAlwaysDefaultEscapingTheOneStringJsonCompactMatchesTextJsonSerialize()
    {
        var arrayPropertyFieldClass = new StandardArrayPropertyFieldClass();
        
        jsonMatchOneStringSerializerOptions.Encoder = new MinimalTextJsonEncoder();
        var singlePropertyFieldClassStringified =
            JsonSerializer.Serialize
                (arrayPropertyFieldClass, jsonMatchOneStringSerializerOptions);
        logger.TrcApnd("Original Json String")?.Args("\n");
        logger.DbgApnd("\n")?.Args(singlePropertyFieldClassStringified, "\n");

        var jsonStringPropertyClass = new JsonStringPropertyClass(singlePropertyFieldClassStringified);
        var jsonStringPropertyClassStringified =
            JsonSerializer.Serialize
                (jsonStringPropertyClass, jsonMatchOneStringSerializerOptions);
        logger.WrnApnd("Json Serializer")?.Args("\n");
        logger.InfApnd("\n")?.Args(jsonStringPropertyClassStringified, "\n");

        var theOneString = new TheOneString();
        theOneString.ClearAndReinitialize(new StyleOptions
        {
            Style                          = StringStyle.Json | StringStyle.Compact
          , JsonEncodingTransferType       = JsonEncodingTransferType.BkSlEscCtrlCharsDblQtAndBkSlOnly
          , WriteKeyValuePairsAsCollection = true
        });
        jsonStringPropertyClass.RevealState(theOneString);
        var oneStringify = theOneString.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd("\n")?.Args(oneStringify, "\n");

        oneStringify.Should().BeEquivalentTo(jsonStringPropertyClassStringified);
    }

    [TestMethod]
    public void JsonStringMinimalEscapingAddAlwaysTheOneStringJsonCompactMatchesTextJsonSerialize()
    {
        var arrayPropertyFieldClass = new StandardArrayPropertyFieldClass();
        
        var theOneString = new TheOneString();
        theOneString.ClearAndReinitialize(new StyleOptions
        {
            Style                          = StringStyle.Json | StringStyle.Compact
          , JsonEncodingTransferType       = JsonEncodingTransferType.BkSlEscCtrlCharsDblQtAndBkSlOnly
          , WriteKeyValuePairsAsCollection = true
        });
        arrayPropertyFieldClass.RevealState(theOneString);
        
        jsonMatchOneStringSerializerOptions.Encoder = new MinimalTextJsonEncoder();
        var singlePropertyFieldClassStringified = theOneString.WriteBuffer.ToString();
        var jsonStringPropertyClass             = new JsonStringPropertyClass(singlePropertyFieldClassStringified);
        jsonStringPropertyClass.RevealState(theOneString);
        
        logger.TrcApnd("Original Json String")?.Args("\n");
        logger.DbgApnd("\n")?.Args(singlePropertyFieldClassStringified, "\n");

        var jsonStringPropertyClassStringified =
            JsonSerializer.Serialize
                (jsonStringPropertyClass, jsonMatchOneStringSerializerOptions);
        logger.WrnApnd("Json Serializer")?.Args("\n");
        logger.InfApnd("\n")?.Args(jsonStringPropertyClassStringified, "\n");

        theOneString.Clear();
        jsonStringPropertyClass.RevealState(theOneString);
        var oneStringify = theOneString.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd("\n")?.Args(oneStringify, "\n");

        oneStringify.Should().BeEquivalentTo(jsonStringPropertyClassStringified);
    }
}
