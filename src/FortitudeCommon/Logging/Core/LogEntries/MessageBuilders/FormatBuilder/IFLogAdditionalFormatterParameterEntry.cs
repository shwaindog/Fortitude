// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public interface IFLogAdditionalFormatterParameterEntry : IFLogFormatterParameterEntry
{
    // ReSharper disable UnusedMember.Global

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndMatch<T>(T value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(bool value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(bool? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TFmt>(TFmt value) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(string? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((string?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((string?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(char[]? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((char[]?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(char[]? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    [CallsObjectToString]
    IFLogAdditionalFormatterParameterEntry? AndObject(object? value);

    IFLogAdditionalParamCollectionAppend AndCollection
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")]
        get;
    }

    void AndFinalMatchParam<T>(T value);
    void AndFinalParam(bool value);
    void AndFinalParam(bool? value);
    void AndFinalParam<TFmt>(TFmt value) where TFmt : ISpanFormattable;
    void AndFinalParam<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    void AndFinalParam<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;
    void AndFinalParam(ReadOnlySpan<char> value);
    void AndFinalParam(ReadOnlySpan<char> value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(string? value);
    void AndFinalParam((string?, int) valueTuple);
    void AndFinalParam((string?, int, int) valueTuple);
    void AndFinalParam(string? value, int startIndex, int count = int.MaxValue);
    void AndFinalParam(char[]? value);
    void AndFinalParam((char[]?, int) valueTuple);
    void AndFinalParam((char[]?, int, int) valueTuple);
    void AndFinalParam(char[]? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(ICharSequence? value);
    void AndFinalParam((ICharSequence?, int) valueTuple);
    void AndFinalParam((ICharSequence?, int, int) valueTuple);
    void AndFinalParam(ICharSequence? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(StringBuilder? value);
    void AndFinalParam((StringBuilder?, int) valueTuple);
    void AndFinalParam((StringBuilder?, int, int) valueTuple);
    void AndFinalParam(StringBuilder? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(IStyledToStringObject? value);

    [CallsObjectToString] void AndFinalObjectParam(object? value);

    IFinalCollectionAppend AndFinalParamCollection
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")]
        get;
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalMatchParamThenToAppender<T>(T? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(bool? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(bool value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender<TFmt>(TFmt value) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(string? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((string?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((string?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(char[]? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((char[]?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(char[]? value, int fromIndex, int count);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(ICharSequence? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(StringBuilder? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(IStyledToStringObject? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    [CallsObjectToString]
    IFLogStringAppender AndFinalObjectParamThenToAppender(object? value);

    IStringAppenderCollectionBuilder AndFinalParamCollectionThenToAppender
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")]
        get;
    }

    // ReSharper restore UnusedMember.Global
}
