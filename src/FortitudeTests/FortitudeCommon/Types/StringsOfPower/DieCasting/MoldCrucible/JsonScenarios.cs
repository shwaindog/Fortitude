using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

    /// <summary>
    /// List of scenarios to test Json formatting of styled types
    /// </summary>
    ///
    ///  Standard types include
    ///     1.One of each  C# keyword value type
    ///     2. System.String
    ///     3. Enum with and without FlagsAttribute
    ///     4  Other SpanFormatable types like DateTime, DateTimeOffset, Guid, TimeSpan,
    ///     5. Nullable of the above value types
    ///     6. System.Text.Json Node types
    ///     7. StringBuilder
    ///     8. ICharSequence(MutableString)
    ///     9. Custom struct type with a custom styler
    ///     10 Custom StyleToStringObject type
    ///
    /// 1. Single class serialization for both Pretty and Compact styles
    ///     1.1 Field and property standard type defined above
    ///         1.1.1 Enums as a number
    ///         1.1.2 Enums as a string
    ///         1.1.3 string containing unescaped Json
    ///         1.1.4 string containing escaped Json
    ///         1.1.5 DateTime in each of the Json standard formats
    ///     1.2 Field and property of an array of each standard type defined above plus object[] with one of each standard type except CustomStyler
    ///         1.2.1 byte[] as base64
    ///         1.2.2 byte[] as array of numbers
    ///         1.2.3 char[] as string
    ///         1.2.4 char[] as array of characters
    ///         1.2.5 string[] with escaped Json
    ///         1.2.6 string[] with unescaped Json
    ///         1.2.7 DateTime[] in each of the Json standard formats
    ///         1.2.8 Above with Filtering Top 10, Top and Last 10,  Even index
    ///     1.3 Field and property a list of each standard type defined above plus List<object> with one of each standard type except CustomStyler
    ///         1.3.1 List<byte> as base64
    ///         1.3.2 List<byte> as array of numbers
    ///         1.3.3 List<char> as string
    ///         1.3.4 List<char> as array of characters
    ///         1.3.5 List<string> with escaped Json
    ///         1.3.6 List<string> with unescaped Json
    ///         1.3.7 List<DateTime> in each of the Json standard formats
    ///         1.3.8 Above with Filtering Top 10, Top and Last 10,  Even index 
    ///     1.4 Field and property a enumerable of each standard type defined above plus IEnumerable<object> with one of each standard type except CustomStyler
    ///         1.4.1 IEnumerable<byte> as base64
    ///         1.4.2 IEnumerable<byte> as array of numbers
    ///         1.4.3 IEnumerable<char> as string
    ///         1.4.4 IEnumerable<char> as array of characters
    ///         1.4.5 IEnumerable<string> with escaped Json
    ///         1.4.6 IEnumerable<string> with unescaped Json
    ///         1.4.7 IEnumerable<DateTime> in each of the Json standard formats
    ///     1.5 Field and property a enumerator of each standard type defined above plus IEnumerator<object> with one of each standard type except CustomStyler
    ///         1.5.1 IEnumerator<byte> as base64
    ///         1.5.2 IEnumerator<byte> as array of numbers
    ///         1.5.3 IEnumerator<char> as string
    ///         1.5.4 IEnumerator<char> as array of characters
    ///         1.5.5 IEnumerator<string> with escaped Json
    ///         1.5.6 IEnumerator<string> with unescaped Json
    ///         1.5.7 IEnumerator<DateTime> in each of the Json standard formats
    ///     1.6 Types that represents a serialization to on of each of the Standard value types above
    ///         1.6.1 Type that when in Log style shows internal field values
    ///     1.7 All of the tests above but when using *Match in the builder
    ///     1.8 Field and property a dictionary of each standard types except except Nullable for keys and all standard types for values
    ///         1.8.1 KeyValuePair<TKey,TValue>[] and List<KeyValuePair<TKey,TValue>> for the above
    ///             1.8.1.1 Serialized in object notation
    ///             1.8.1.2 Serialized in collection key value notation
    ///     1.9 Field and property a dictionary of each standard types except except Nullable for keys and another Type with a custom styler for values
    ///         1.9.1 KeyValuePair<TKey,TValue>[] and List<KeyValuePair<TKey,TValue>>
    ///             1.9.1.1 Serialized in object notation
    ///             1.9.1.2 Serialized in collection key value notation
    ///     1.10 Field and property a dictionary a type with a CustomerStyler for the key and another Type with a custom styler for values
    ///         1.10.1 KeyValuePair<TKey,TValue>[] and List<KeyValuePair<TKey,TValue>>
    ///             1.10.1.1 Serialized in object notation
    ///             1.10.1.2 Serialized in collection key value notation
    ///
    ///  2. Polymorphic handling
    ///     2.1 Calling AddBaseStyledToStringFields at the start puts base fields first
    ///     2.2 Calling AddBaseStyledToStringFields at the end puts base fields last
    ///     2.3 Grandchild with abstract parent calls grandparent ToString(IStyledTypeStringAppender stsa)
    ///         2.3.1 Shows child nodes clipped with $clipped="maxdepth"
    ///     2.4 Calling AddBaseStyledToStringFields on a base type with no ToString(IStyleTypeStringAppender stsa) silently does nothing
    ///     2.5 Classes can be styled with BaseTypeStylers
    ///
    ///  3. Object graphs
    ///     3.1 Root object node serialized to Depth 1
    ///         3.1.1 Shows child nodes clipped with $clipped="maxdepth"
    ///     3.2 Root object with children with circular references serialized with $id and $ref included
    ///     3.3 Root object with children with circular references serialized with no circular reference handling serialized to max depth of 64 nodes
    
[TestClass]
[NoMatchingProductionClass]
public class JsonScenarios
{
    private static IVersatileFLogger logger = null!;

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

        logger  = FLog.FLoggerForType.As<IVersatileFLogger>();
    }
    
    [TestMethod]
    public void StandardSinglePropertyFieldClassSerializesAllFields()
    {
        var singlePropertyFieldClass = new StandardSinglePropertyFieldClass();
        
        var textJsonStringify = JsonSerializer.Serialize(singlePropertyFieldClass, new JsonSerializerOptions()
        {
          NumberHandling   = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals 
          , IncludeFields = true
          ,  Converters = { 
                 new BigIntegerConverter()
               , new ComplexConverter()
               , new RuneConverter()
               , new StringBuilderConverter()
               , new TestCustomSpanFormattableConverter()
               , new JsonStringEnumConverter<NoDefaultLongNoFlagsEnum>()
               , new JsonStringEnumConverter<NoDefaultULongNoFlagsEnum>()
               , new JsonStringEnumConverter<WithDefaultLongNoFlagsEnum>()
               , new JsonStringEnumConverter<WithDefaultULongNoFlagsEnum>()
               , new JsonStringEnumConverter<NoDefaultLongWithFlagsEnum>()
               , new JsonStringEnumConverter<NoDefaultULongWithFlagsEnum>()
               , new JsonStringEnumConverter<WithDefaultLongWithFlagsEnum>()
               , new JsonStringEnumConverter<WithDefaultULongWithFlagsEnum>()
                
            }
        });

        logger.WrnApnd("Json Serializer")?.Args("\n");
        logger.InfApnd(textJsonStringify)?.Args("\n");
        
        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(StringStyle.Json | StringStyle.Compact);
        styledStringBuilder.Settings = new StyleOptions
        {
            WriteKeyValuePairsAsCollection = true
        };
        singlePropertyFieldClass.RevealState(styledStringBuilder);
        var oneStringify = styledStringBuilder.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n" );
        logger.WrnApnd(oneStringify)?.Args("\n" );

        oneStringify.Should().BeEquivalentTo(textJsonStringify);


    }
}
