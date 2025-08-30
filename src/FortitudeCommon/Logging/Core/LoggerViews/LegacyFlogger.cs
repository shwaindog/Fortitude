// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Core.ActivationProfiles;
using static FortitudeCommon.Logging.Core.ActivationProfiles.LoggerActivationFlags;

// ReSharper disable ExplicitCallerInfoArgument

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface ILegacyFLogger : ILoggerView
{
    ISingleInvokeArgumentChain? AtLevelAppend<T>(FLogLevel level, T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T>(FLogLevel level, T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , T3 p4, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(FLogLevel level,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    ISingleInvokeArgumentChain? TraceAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    ISingleInvokeArgumentChain? DebugAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? InfoAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    ISingleInvokeArgumentChain? WarnAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? ErrorAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
}

public class LegacyFLogger(IFLogger logger) : LoggerView(logger), ILegacyFLogger
{
    public ISingleInvokeArgumentChain? AtLevelAppend<T>(FLogLevel level, T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender().AppendMatch(firstAppend));
        return null;
    }

    public void AtLevel<T>(FLogLevel level, T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender().FinalMatchAppend(toLog);
    }

    public void AtLevel<T0>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithOnlyMatchParam(p1);
    }

    public void AtLevel<T0, T1>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndFinalMatchParam(p1);
    }

    public void AtLevel<T0, T1, T2>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndFinalMatchParam(p2);
    }

    public void AtLevel<T0, T1, T2, T3>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndFinalMatchParam(p3);
    }


    public void AtLevel<T0, T1, T2, T3, T4>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndFinalMatchParam(p4);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndFinalMatchParam(p5);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndFinalMatchParam(p6);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndFinalMatchParam(p7);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8>(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndFinalMatchParam(p8);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndFinalMatchParam(p9);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndFinalMatchParam(p10);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndFinalMatchParam(p11);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndFinalMatchParam(p12);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndFinalMatchParam(p13);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
                .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)
                ?.AndFinalMatchParam(p14);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndFinalMatchParam(p15);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndFinalMatchParam(p16);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndFinalMatchParam(p17);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndFinalMatchParam(p18);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndFinalMatchParam(p19);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndFinalMatchParam(p20);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndFinalMatchParam(p21);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndFinalMatchParam(p22);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(FLogLevel level
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndFinalMatchParam(p23);
    }

    public void AtLevel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , T22 p22, T23 p23, T24 p24, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndMatch(p23)?.AndFinalMatchParam(p24);
    }

    public ISingleInvokeArgumentChain? TraceAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender().AppendMatch(firstAppend));
        return null;
    }

    public void Trace<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender().FinalMatchAppend(toLog);
    }

    public void Trace<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithOnlyMatchParam(p1);
    }

    public void Trace<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndFinalMatchParam(p1);
    }

    public void Trace<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndFinalMatchParam(p2);
    }

    public void Trace<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndFinalMatchParam(p3);
    }

    public void Trace<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndFinalMatchParam(p4);
    }

    public void Trace<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndFinalMatchParam(p5);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndFinalMatchParam(p6);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndFinalMatchParam(p7);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndFinalMatchParam(p8);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndFinalMatchParam(p9);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndFinalMatchParam(p10);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndFinalMatchParam(p11);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndFinalMatchParam(p12);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndFinalMatchParam(p13);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
                .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)
                ?.AndFinalMatchParam(p14);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndFinalMatchParam(p15);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndFinalMatchParam(p16);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndFinalMatchParam(p17);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndFinalMatchParam(p18);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndFinalMatchParam(p19);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndFinalMatchParam(p20);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndFinalMatchParam(p21);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndFinalMatchParam(p22);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndFinalMatchParam(p23);
    }

    public void Trace<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , T22 p22, T23 p23, T24 p24, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndMatch(p23)?.AndFinalMatchParam(p24);
    }

    public ISingleInvokeArgumentChain? DebugAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender().AppendMatch(firstAppend));
        return null;
    }

    public void Debug<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender().FinalMatchAppend(toLog);
    }

    public void Debug<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithOnlyMatchParam(p1);
    }

    public void Debug<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndFinalMatchParam(p1);
    }

    public void Debug<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndFinalMatchParam(p2);
    }

    public void Debug<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndFinalMatchParam(p3);
    }

    public void Debug<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndFinalMatchParam(p4);
    }

    public void Debug<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndFinalMatchParam(p5);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndFinalMatchParam(p6);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndFinalMatchParam(p7);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndFinalMatchParam(p8);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndFinalMatchParam(p9);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndFinalMatchParam(p10);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndFinalMatchParam(p11);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndFinalMatchParam(p12);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndFinalMatchParam(p13);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
                .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)
                ?.AndFinalMatchParam(p14);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndFinalMatchParam(p15);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndFinalMatchParam(p16);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndFinalMatchParam(p17);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndFinalMatchParam(p18);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndFinalMatchParam(p19);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndFinalMatchParam(p20);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndFinalMatchParam(p21);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndFinalMatchParam(p22);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndFinalMatchParam(p23);
    }

    public void Debug<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , T22 p22, T23 p23, T24 p24, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndMatch(p23)?.AndFinalMatchParam(p24);
    }

    public ISingleInvokeArgumentChain? InfoAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender().AppendMatch(firstAppend));
        return null;
    }

    public void Info<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender().FinalMatchAppend(toLog);
    }

    public void Info<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithOnlyMatchParam(p1);
    }

    public void Info<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndFinalMatchParam(p1);
    }

    public void Info<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndFinalMatchParam(p2);
    }

    public void Info<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndFinalMatchParam(p3);
    }

    public void Info<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndFinalMatchParam(p4);
    }

    public void Info<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndFinalMatchParam(p5);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndFinalMatchParam(p6);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndFinalMatchParam(p7);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndFinalMatchParam(p8);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndFinalMatchParam(p9);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndFinalMatchParam(p10);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndFinalMatchParam(p11);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndFinalMatchParam(p12);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndFinalMatchParam(p13);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
                .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)
                ?.AndFinalMatchParam(p14);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndFinalMatchParam(p15);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndFinalMatchParam(p16);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndFinalMatchParam(p17);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndFinalMatchParam(p18);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndFinalMatchParam(p19);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndFinalMatchParam(p20);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndFinalMatchParam(p21);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndFinalMatchParam(p22);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndFinalMatchParam(p23);
    }

    public void Info<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , T22 p22, T23 p23, T24 p24, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndMatch(p23)?.AndFinalMatchParam(p24);
    }

    public ISingleInvokeArgumentChain? WarnAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender().AppendMatch(firstAppend));
        return null;
    }

    public void Warn<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender().FinalMatchAppend(toLog);
    }

    public void Warn<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithOnlyMatchParam(p1);
    }

    public void Warn<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndFinalMatchParam(p1);
    }

    public void Warn<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndFinalMatchParam(p2);
    }

    public void Warn<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndFinalMatchParam(p3);
    }

    public void Warn<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndFinalMatchParam(p4);
    }

    public void Warn<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndFinalMatchParam(p5);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndFinalMatchParam(p6);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndFinalMatchParam(p7);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndFinalMatchParam(p8);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndFinalMatchParam(p9);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndFinalMatchParam(p10);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndFinalMatchParam(p11);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndFinalMatchParam(p12);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndFinalMatchParam(p13);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
                .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)
                ?.AndFinalMatchParam(p14);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndFinalMatchParam(p15);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndFinalMatchParam(p16);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndFinalMatchParam(p17);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndFinalMatchParam(p18);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndFinalMatchParam(p19);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndFinalMatchParam(p20);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndFinalMatchParam(p21);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndFinalMatchParam(p22);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndFinalMatchParam(p23);
    }

    public void Warn<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , T22 p22, T23 p23, T24 p24, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndMatch(p23)?.AndFinalMatchParam(p24);
    }

    public ISingleInvokeArgumentChain? ErrorAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender().AppendMatch(firstAppend));
        return null;
    }

    public void Error<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender().FinalMatchAppend(toLog);
    }

    public void Error<T0>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithOnlyMatchParam(p1);
    }

    public void Error<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndFinalMatchParam(p1);
    }

    public void Error<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndFinalMatchParam(p2);
    }

    public void Error<T0, T1, T2, T3>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndFinalMatchParam(p3);
    }

    public void Error<T0, T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndFinalMatchParam(p4);
    }

    public void Error<T0, T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndFinalMatchParam(p5);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndFinalMatchParam(p6);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndFinalMatchParam(p7);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndFinalMatchParam(p8);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndFinalMatchParam(p9);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndFinalMatchParam(p10);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndFinalMatchParam(p11);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndFinalMatchParam(p12);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString
      , T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndFinalMatchParam(p13);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
                .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)
                ?.AndFinalMatchParam(p14);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndFinalMatchParam(p15);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndFinalMatchParam(p16);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndFinalMatchParam(p17);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndFinalMatchParam(p18);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndFinalMatchParam(p19);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndFinalMatchParam(p20);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndFinalMatchParam(p21);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndFinalMatchParam(p22);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndFinalMatchParam(p23);
    }

    public void Error<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string formatString, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21
      , T22 p22, T23 p23, T24 p24, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.FormatBuilder(formatString).WithMatchParams(p0)?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?
            .AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p13)?
            .AndMatch(p14)?.AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndMatch(p23)?.AndFinalMatchParam(p24);
    }
}
