using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CompareToSystemTextJson.TypePermutation;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CompareToSystemTextJson;

[TestClass]
public class CompactLogTypeFormattingTests
{
    private static IVersatileFLogger logger = null!;

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

        logger = FLog.FLoggerForType.As<IVersatileFLogger>();
    }
    
    [TestMethod]
    public void StandardSinglePropertyFieldClassLogCompactSerializeMatchesAllFields()
    {
        var singlePropertyFieldClass = new StandardSinglePropertyFieldClass();

        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(new StyleOptions
        {
            Style                          = StringStyle.Log | StringStyle.Compact
          , WriteKeyValuePairsAsCollection = true
        });
        singlePropertyFieldClass.RevealState(styledStringBuilder);
        var oneStringify = styledStringBuilder.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd(oneStringify)?.Args("\n");
    }
    
    [TestMethod]
    public void StandardSinglePropertyFieldClassLogPrettySerializeMatchesAllFields()
    {
        var singlePropertyFieldClass = new StandardSinglePropertyFieldClass();

        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(new StyleOptions
        {
            Style                          = StringStyle.Log | StringStyle.Pretty
          , WriteKeyValuePairsAsCollection = true
        });
        singlePropertyFieldClass.RevealState(styledStringBuilder);
        var oneStringify = styledStringBuilder.WriteBuffer.ToString();

        logger.ErrApnd("TheOneString")?.Args("\n");
        logger.WrnApnd(oneStringify)?.Args("\n");
    }
    
}
