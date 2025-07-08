// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core;

public interface IFLogFirstFormatterParameterEntry
{
    int RemainingArguments { get; }

    MalformedFormatHandling MalformedFormatHandling { get; }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")] IFLogAdditionalFormatterParameterEntry? WithParams(IMutableString value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(bool value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(sbyte value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(byte value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(char value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(short value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(ushort value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(int value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(uint value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(float value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(long value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(ulong value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(double value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(decimal value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(object value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(char[] value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(string value);
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]  IFLogAdditionalFormatterParameterEntry? WithParams(string value, int startIndex, int length);

    void WithOnlyParam(IMutableString value);
    void WithOnlyParam(IStyledToStringObject value);
    void WithOnlyParam(bool value);
    void WithOnlyParam(sbyte value);
    void WithOnlyParam(byte value);
    void WithOnlyParam(char value);
    void WithOnlyParam(short value);
    void WithOnlyParam(ushort value);
    void WithOnlyParam(int value);
    void WithOnlyParam(uint value);
    void WithOnlyParam(float value);
    void WithOnlyParam(long value);
    void WithOnlyParam(ulong value);
    void WithOnlyParam(double value);
    void WithOnlyParam(decimal value);
    void WithOnlyParam(object value);
    void WithOnlyParam(char[] value);
    void WithOnlyParam(string value);
    void WithOnlyParam(string value, int startIndex, int length);
}

public interface IFLogAdditionalFormatterParameterEntry
{
    int RemainingArguments { get; }

    MalformedFormatHandling MalformedFormatHandling { get; }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(IMutableString value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(bool value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(sbyte value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(byte value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(char value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(short value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(ushort value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(int value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(uint value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(float value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(long value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(ulong value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(double value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(decimal value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(object value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(char[] value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(string value);
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")] IFLogAdditionalFormatterParameterEntry? And(string value, int startIndex, int length);

    void AndFinalParam(IMutableString value);
    void AndFinalParam(IStyledToStringObject value);
    void AndFinalParam(bool value);
    void AndFinalParam(sbyte value);
    void AndFinalParam(byte value);
    void AndFinalParam(char value);
    void AndFinalParam(short value);
    void AndFinalParam(ushort value);
    void AndFinalParam(int value);
    void AndFinalParam(uint value);
    void AndFinalParam(float value);
    void AndFinalParam(long value);
    void AndFinalParam(ulong value);
    void AndFinalParam(double value);
    void AndFinalParam(decimal value);
    void AndFinalParam(object value);
    void AndFinalParam(char[] value);
    void AndFinalParam(string value);
    void AndFinalParam(string value, int startIndex, int length);
}
