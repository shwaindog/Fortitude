using System.Runtime.CompilerServices;
using static FortitudeCommon.Logging.Core.LoggerActivationFlags;

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface ILegacyFLogger
{
    ISingleInvokeArgumentChain? AtLevelAppend<T>(T firstAppend, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel(string constString, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0>(string formatString, T0 p1, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1>(string formatString, T0 p0, T1 p1, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2>(string formatString, T0 p0, T1 p1, T2 p2, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3>(string formatString, T0 p0, T1 p1, T2 p2, T3 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    ISingleInvokeArgumentChain? TraceAppend<T>(T firstAppend, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace(string constString, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0>(string formatString, T0 p1, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1>(string formatString, T0 p0, T1 p1, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2>(string formatString, T0 p0, T1 p1, T2 p2, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3>(string formatString, T0 p0, T1 p1, T2 p2, T3 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    ISingleInvokeArgumentChain? DebugAppend<T>(T firstAppend, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug(string constString, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0>(string formatString, T0 p1, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1>(string formatString, T0 p0, T1 p1, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2>(string formatString, T0 p0, T1 p1, T2 p2, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3>(string formatString, T0 p0, T1 p1, T2 p2, T3 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    ISingleInvokeArgumentChain? InfoAppend<T>(T firstAppend, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info(string constString, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0>(string formatString, T0 p1, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1>(string formatString, T0 p0, T1 p1, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2>(string formatString, T0 p0, T1 p1, T2 p2, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3>(string formatString, T0 p0, T1 p1, T2 p2, T3 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    ISingleInvokeArgumentChain? WarnAppend<T>(T firstAppend, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn(string constString, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0>(string formatString, T0 p1, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1>(string formatString, T0 p0, T1 p1, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2>(string formatString, T0 p0, T1 p1, T2 p2, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3>(string formatString, T0 p0, T1 p1, T2 p2, T3 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    ISingleInvokeArgumentChain? ErrorAppend<T>(T firstAppend, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error(string constString, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0>(string formatString, T0 p1, [CallerMemberName] string memberName = "", LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1>(string formatString, T0 p0, T1 p1, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2>(string formatString, T0 p0, T1 p1, T2 p2, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3>(string formatString, T0 p0, T1 p1, T2 p2, T3 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string formatString, T0 p0, T1 p1, T2 p2, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , LoggerActivationFlags activationFlags = DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
}
