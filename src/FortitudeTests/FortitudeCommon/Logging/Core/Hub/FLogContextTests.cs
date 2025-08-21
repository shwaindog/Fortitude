using System.Linq.Expressions;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Config.Appending.Formatting.Files;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeTests.FortitudeCommon.Logging.Core.Hub;

[TestClass]
public class FLogContextTests
{
    [TestMethod]
    public void DefaultContextStartsAndLogsToConsole()
    {
        var startedContext = FLogContext.NewUninitializedContext.InitializeStartAndSetAsCurrentContext();

        var logger = FLog.FLoggerForType.As<IVersatileFLogger>();
        var appReg = (IMutableFLogAppenderRegistry)startedContext.AppenderRegistry;

        var spyRing = new LogEntrySpyRingInvestigation();

        var loggerSpy = spyRing.TrainNewSpy("Test Logger Spy");
        logger.Logger.PublishEndpoint.Insert(loggerSpy);

        var consoleAppender = appReg.GetAppender(IConsoleAppenderConfig.DefaultConsoleAppenderName)!;
        var appenderSpy = spyRing.TrainNewSpy("Test Appender Spy");
        consoleAppender.ReceiveEndpoint.Insert(appenderSpy);
        
        logger.Info("Testing 1 2 3, testing");
        logger.Info("Testing 4,5,6, testing");
        logger.Info("Testing 7 8 9, testing");
        logger.Info("Testing 1 2 3, testing");
        logger.Info("Testing 4 5 6, testing");
        logger.Info("Testing 7 8 9, testing");

        var logEntries = spyRing.DeadDropLatestIntelToHq();

        foreach (var logEntry in logEntries)
        {
            Console.Out.WriteLine("Intercepted " + logEntry);
        }
    }

    [TestMethod]
    public void TestValueStructReflection()
    {
        var fileAppenderType = FileAppenderType.RollingLogFile;
        var styler           = FileAppenderTypeExtensions.FileAppenderTypeFormatter;

        var checkRuntimeType = (fileAppenderType, styler);

        // CheckGeneric(checkRuntimeType);

        BuildStructInvoke(checkRuntimeType);
    }

    protected void CheckGeneric<T>(T value)
    {
        var type = value.GetType();
        Console.Out.WriteLine($"value is of Type : {type}");

        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(ValueTuple<,>))
            {
                Console.Out.WriteLine("It is a value Tuple with two items");
            }
        }

        Console.Out.WriteLine("No Op");
    }

    protected void BuildStructInvoke<T>(T value)
    {
        Func<FLogContextTests, T, string> invokeMethod = BuildInvoke(value);

        var runResult = invokeMethod(this, value);
    }

    public string InvokeStructStyler<TStruct>(TStruct value, CustomTypeStyler<TStruct> styler) where TStruct : struct
    {
        var stsa = new StyledTypeStringAppender();
        stsa.Initialize();

        styler(value, stsa);

        var output = stsa.WriteBuffer.ToString();
        Console.Out.WriteLine("Ran result " + output);
        return output;
    }

    public Func<FLogContextTests, TTuple, string> BuildInvoke<TTuple>(TTuple toConvert)
    {
        var type = toConvert.GetType();

        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(ValueTuple<,>))
            {
                var arguments = type.GenericTypeArguments;
                var item1Type = arguments[0];
                var item2Type = arguments[1];

                if (item2Type.IsGenericType)
                {
                    var item2GenericType = item2Type.GetGenericTypeDefinition();
                    if (item2GenericType == typeof(CustomTypeStyler<>))
                    {
                        var myType = GetType();

                        var ownerParameter = Expression.Parameter(myType, "methodOwner");
                        var tupleParameter = Expression.Parameter(type, "genericTuple");

                        var structValue  = Expression.Parameter(item1Type, "structValue");
                        var structStyler = Expression.Parameter(item2Type, "structStyler");

                        var item1FieldInfo = type.GetField("Item1") ??
                                             throw new InvalidOperationException($"Item1 does not exist on {type.FullName}");
                        var getItem1Expr = Expression.Field(tupleParameter, item1FieldInfo);
                        var item2FieldInfo = type.GetField("Item2") ??
                                             throw new InvalidOperationException($"Item2 does not exist on {type.FullName}");
                        var getItem2Expr = Expression.Field(tupleParameter, item2FieldInfo);

                        var invokeMethodDef =
                            myType.GetMethods()
                                  .FirstOrDefault(mi =>
                                  {
                                      var genericParams = mi.GetGenericArguments();
                                      var methodParams  = mi.GetParameters();
                                      return
                                          mi.Name == nameof(InvokeStructStyler)
                                       && methodParams.Length == 2
                                       && genericParams[0] == methodParams[0].ParameterType &&
                                          methodParams[1].ParameterType.GetGenericTypeDefinition() ==
                                          typeof(CustomTypeStyler<>);
                                  }) ??
                            throw new InvalidOperationException("Method does not exist");

                        var invokeMethod = invokeMethodDef.MakeGenericMethod(item1Type);

                        var block = Expression.Block
                            (
                             typeof(string),
                             [structValue, structStyler],
                             Expression.Assign(structValue, getItem1Expr),
                             Expression.Assign(structStyler, getItem2Expr),
                             Expression.Call(ownerParameter, invokeMethod, structValue, structStyler)
                            );

                        Expression<Func<FLogContextTests, TTuple, string>> invoke =
                            Expression.Lambda<Func<FLogContextTests, TTuple, string>>(block, [ownerParameter, tupleParameter]);
                        return invoke.Compile();
                    }
                }
            }
        }
        Console.Out.WriteLine("Boom!");
        throw new InvalidOperationException("Method does not exist");
    }
}
