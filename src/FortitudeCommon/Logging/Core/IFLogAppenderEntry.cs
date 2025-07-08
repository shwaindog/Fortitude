// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core;

public interface IFLogAppenderEntry
{
    int Count { get; }


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(IMutableString value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(IStyledToStringObject value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(bool value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(sbyte value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(byte value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(char value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(short value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(ushort value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(int value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(uint value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(float value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(long value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(ulong value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(double value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(decimal value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(object value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(char[] value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(string value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry Append(string value, int startIndex, int length);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine();
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(IMutableString value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(IStyledToStringObject value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(bool value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(sbyte value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(byte value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(char value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(short value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(ushort value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(int value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(uint value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(float value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(long value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(ulong value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(double value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(decimal value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(object value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(char[] value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(string value);
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")] IFLogAppenderEntry AppendLine(string value, int startIndex, int length);

    void FinalAppend(IMutableString value);
    void FinalAppend(IStyledToStringObject value);
    void FinalAppend(bool value);
    void FinalAppend(sbyte value);
    void FinalAppend(byte value);
    void FinalAppend(char value);
    void FinalAppend(short value);
    void FinalAppend(ushort value);
    void FinalAppend(int value);
    void FinalAppend(uint value);
    void FinalAppend(float value);
    void FinalAppend(long value);
    void FinalAppend(ulong value);
    void FinalAppend(double value);
    void FinalAppend(decimal value);
    void FinalAppend(object value);
    void FinalAppend(char[] value);
    void FinalAppend(string value);
    void FinalAppend(string value, int startIndex, int length);
}
