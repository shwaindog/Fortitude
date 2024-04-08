#region

using System.Reflection;
using FortitudeBusRules.Messaging;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.TestHelpers;

[TestClass]
[NoMatchingProductionClass]
public class TestMetrics
{
    private const int MaxAllowedUntestedClassesInCommon = 125;
    private const int MaxAllowedUntestedClassesInFortitudeIO = 93;
    private const int MaxAllowedUntestedClassesInFortitudeMarketsApi = 17;
    private const int MaxAllowedUntestedClassesInFortitudeMarketsCore = 88;
    private const int MaxAllowedUntestedClassesInFortitudeBusRules = 57;
    private IDictionary<string, List<Type>> fortitudeBusRulesAssemblyClasses = null!;
    private Type fortitudeBusRulesType = null!;

    private IDictionary<string, List<Type>> fortitudeCommonAssemblyClasses = null!;

    private Type fortitudeCommonType = null!;
    private IDictionary<string, List<Type>> fortitudeIOAssemblyClasses = null!;
    private Type fortitudeIOType = null!;
    private IDictionary<string, List<Type>> fortitudeMarketsApiAssemblyClasses = null!;
    private Type fortitudeMarketsApiType = null!;
    private IDictionary<string, List<Type>> fortitudeMarketsCoreAssemblyClasses = null!;
    private Type fortitudeMarketsCoreType = null!;

    private IDictionary<string, Type> testClasses = null!;
    private IDictionary<string, List<Type>> testClassNames = null!;

    [TestInitialize]
    public void SetUp()
    {
        fortitudeCommonType = typeof(TimeContext);
        fortitudeIOType = typeof(ISocketSessionContext);
        fortitudeMarketsApiType = typeof(ILevel0Quote);
        fortitudeMarketsCoreType = typeof(PQLevel0Quote);
        fortitudeBusRulesType = typeof(Message);

        fortitudeCommonAssemblyClasses = TestableClassesInAssembly(fortitudeCommonType)
            .GroupBy(t => StripOutGenericBackTicks(t.Name)).ToDictionary(gpt => gpt.Key, gpt => gpt.ToList());
        fortitudeIOAssemblyClasses = TestableClassesInAssembly(fortitudeIOType)
            .GroupBy(t => StripOutGenericBackTicks(t.Name)).ToDictionary(gpt => gpt.Key, gpt => gpt.ToList());
        fortitudeMarketsApiAssemblyClasses = TestableClassesInAssembly(fortitudeMarketsApiType)
            .GroupBy(t => StripOutGenericBackTicks(t.Name)).ToDictionary(gpt => gpt.Key, gpt => gpt.ToList());
        fortitudeMarketsCoreAssemblyClasses = TestableClassesInAssembly(fortitudeMarketsCoreType)
            .GroupBy(t => StripOutGenericBackTicks(t.Name)).ToDictionary(gpt => gpt.Key, gpt => gpt.ToList());
        fortitudeBusRulesAssemblyClasses = TestableClassesInAssembly(fortitudeBusRulesType)
            .GroupBy(t => StripOutGenericBackTicks(t.Name)).ToDictionary(gpt => gpt.Key, gpt => gpt.ToList());

        testClasses = ProductionTestingClassesInAssembly(GetType()).ToDictionary(t => t.FullName!, t => t);
        testClassNames = ProductionTestingClassesInAssembly(GetType())
            .GroupBy(t => StripOutGenericBackTicks(t.Name))
            .ToDictionary(gpt => gpt.Key, gpt => gpt.ToList());
    }

    [TestMethod]
    public void ListProdClassesWithoutMatchingTestClass()
    {
        var countUnTestedInAssembly = fortitudeCommonAssemblyClasses.Values.SelectMany(lt => lt)
            .Count(PrintTestClassStateIfApplicable);
        Console.Out.WriteLine($"FortitudeCommon Assembly Classes Without Tests - {countUnTestedInAssembly}");
        Assert.IsTrue(countUnTestedInAssembly <= MaxAllowedUntestedClassesInCommon,
            $"Common has {countUnTestedInAssembly} which is greater " +
            $"than max allowed of {MaxAllowedUntestedClassesInCommon}");

        countUnTestedInAssembly = fortitudeIOAssemblyClasses.Values.SelectMany(lt => lt)
            .Count(PrintTestClassStateIfApplicable);
        Console.Out.WriteLine($"\nFortitudeIO Assembly Classes Without Tests- {countUnTestedInAssembly}");
        Assert.IsTrue(countUnTestedInAssembly <= MaxAllowedUntestedClassesInFortitudeIO,
            $"FortitudeIO has {countUnTestedInAssembly} which is greater " +
            $"than max allowed of {MaxAllowedUntestedClassesInFortitudeIO}");

        countUnTestedInAssembly = fortitudeMarketsApiAssemblyClasses.Values.SelectMany(lt => lt)
            .Count(PrintTestClassStateIfApplicable);
        Console.Out.WriteLine($"\nFortitudeMarketsApi Assembly Classes Without Tests- {countUnTestedInAssembly}");
        Assert.IsTrue(countUnTestedInAssembly <= MaxAllowedUntestedClassesInFortitudeMarketsApi,
            $"FortitudeMarketsApi has {countUnTestedInAssembly} which is greater " +
            $"than max allowed of {MaxAllowedUntestedClassesInFortitudeMarketsApi}");

        countUnTestedInAssembly = fortitudeMarketsCoreAssemblyClasses.Values.SelectMany(lt => lt)
            .Count(PrintTestClassStateIfApplicable);
        Console.Out.WriteLine($"\nFortitudeMarketsCore Assembly Classes Without Tests- {countUnTestedInAssembly}");
        Assert.IsTrue(countUnTestedInAssembly <= MaxAllowedUntestedClassesInFortitudeMarketsCore,
            $"FortitudeMarketsCore has {countUnTestedInAssembly} which is greater " +
            $"than max allowed of {MaxAllowedUntestedClassesInFortitudeMarketsCore}");

        countUnTestedInAssembly = fortitudeBusRulesAssemblyClasses.Values.SelectMany(lt => lt)
            .Count(PrintTestClassStateIfApplicable);
        Console.Out.WriteLine($"\nFortitudeBusRules Assembly Classes Without Tests- {countUnTestedInAssembly}");
        Assert.IsTrue(countUnTestedInAssembly <= MaxAllowedUntestedClassesInFortitudeBusRules,
            $"FortitudeBusRules has {countUnTestedInAssembly} which is greater " +
            $"than max allowed of {MaxAllowedUntestedClassesInFortitudeBusRules}");

        countUnTestedInAssembly = testClassNames.Values.SelectMany(lt => lt)
            .Count(PrintTestClassWithNoMatchingProductionClass);
        Console.Out.WriteLine($"\nFortitudeTests Assembly Test Classes Production Class- {countUnTestedInAssembly}");
        Assert.IsTrue(countUnTestedInAssembly == 0, "FortitudeTests has unaccounted for test classes!");
        Console.Out.WriteLine("End of Test");
    }

    public IEnumerable<Type> TestableClassesInAssembly(Type sampleTypeFoundInAssembly)
    {
        return OuterClassesInAssembly(sampleTypeFoundInAssembly)
            .Where(t => !t.GetCustomAttributes(typeof(TestClassNotRequiredAttribute)).Any());
    }

    public IEnumerable<Type> ProductionTestingClassesInAssembly(Type sampleTypeFoundInAssembly)
    {
        return OuterClassesInAssembly(sampleTypeFoundInAssembly)
            .Where(t => !t.GetCustomAttributes(typeof(NoMatchingProductionClassAttribute)).Any()
                        && t.GetCustomAttributes(typeof(TestClassAttribute)).Any());
    }

    public IEnumerable<Type> OuterClassesInAssembly(Type sampleTypeFoundInAssembly)
    {
        var assembly = sampleTypeFoundInAssembly.Assembly;
        return assembly.GetTypes().Where(t => t.IsClass && (!t.FullName?.Contains("<") ?? false)
                                                        && (!t.FullName?.Contains("+") ?? false))
            .OrderBy(t => t.FullName);
    }

    public bool PrintTestClassStateIfApplicable(Type determineTestClassState)
    {
        if (HasTestClassInCorrectNameSpace(determineTestClassState)) return false;
        if (HasTestClassAtIncorrectNameSpace(determineTestClassState, out var testClassFullName))
        {
            Console.Out.WriteLine($"\t{determineTestClassState.FullName} has {testClassFullName}" +
                                  " in wrong namespace");
            return true;
        }

        Console.Out.WriteLine($"\t{determineTestClassState.FullName} has no test class.");
        return true;
    }

    public bool PrintTestClassWithNoMatchingProductionClass(Type testClass)
    {
        var expectedAssemblyDictionary = GetExpectedTypeDictionary(testClass);
        var prodClassName = GenerateProductionClassFromTestClassName(testClass);
        if (FindTypeInDictionary(expectedAssemblyDictionary, prodClassName)) return false;
        Console.Out.WriteLine($"\t{testClass.FullName} has no matching production class.");
        return true;
    }

    public bool HasTestClassInCorrectNameSpace(Type findTestClassForThis) =>
        testClasses.ContainsKey("FortitudeTests." +
                                StripOutGenericBackTicks(findTestClassForThis.FullName!) + "Tests");

    private string StripOutGenericBackTicks(string fullClassName) =>
        fullClassName.Substring(0, fullClassName.IndexOf("`") < 0 ? fullClassName.Length : fullClassName.IndexOf("`"));

    private string GenerateProductionClassFromTestClassName(Type testClass) => testClass.Name.Replace("Tests", "");

    public bool HasTestClassAtIncorrectNameSpace(Type findTestClassForThis, out string testClassFullName)
    {
        if (testClassNames.TryGetValue(findTestClassForThis.Name + "Tests", out var foundTypes))
        {
            testClassFullName = string.Join(", ", foundTypes);
            return true;
        }

        testClassFullName = null!;
        return false;
    }

    public IDictionary<string, List<Type>> GetExpectedTypeDictionary(Type testClass)
    {
        if (testClass.FullName!.Contains("FortitudeTests.FortitudeCommon")) return fortitudeCommonAssemblyClasses;
        if (testClass.FullName!.Contains("FortitudeTests.FortitudeIO")) return fortitudeIOAssemblyClasses;
        if (testClass.FullName!.Contains("FortitudeTests.FortitudeMarketsApi"))
            return fortitudeMarketsApiAssemblyClasses;
        if (testClass.FullName!.Contains("FortitudeTests.FortitudeMarketsCore"))
            return fortitudeMarketsCoreAssemblyClasses;
        if (testClass.FullName!.Contains("FortitudeTests.FortitudeBusRules"))
            return fortitudeBusRulesAssemblyClasses;
        Console.Out.WriteLine($"{testClass.FullName} cannot determine production class assembly");
        throw new ArgumentException(
            "Did not expect a test class without NoMatchProductionClassAttribute to not map to Common, ForititudeCommon or FotitudeCore");
    }

    public bool FindTypeInDictionary(IDictionary<string, List<Type>> searchDictionary, string findClassNameInDict) =>
        searchDictionary.ContainsKey(findClassNameInDict);
}
