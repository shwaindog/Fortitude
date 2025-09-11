// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender
{
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine() => MessageSb.AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool value) => MessageSb.Append(value ? "true" : "false").AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool? value) =>
        MessageSb.Append(value != null ? value.Value ? "true" : "false" : "null")
                 .AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TFmt>(TFmt value, string? formatString = null) where TFmt : ISpanFormattable =>
        (formatString != null ? MessageSb.AppendFormat(formatString, value) : MessageSb.Append(value)).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TFmt>((TFmt, string) valueTuple) where TFmt : ISpanFormattable
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType
    {
        customTypeStyler(value, MessageStsa);
        return MessageSb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        AppendStyled(valueTuple, MessageStsa);
        return MessageSb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ReadOnlySpan<char> value) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value[startIndex..(startIndex + count)]).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ICharSequence? value) => MessageSb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((ICharSequence?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(StringBuilder? value) => MessageSb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((StringBuilder?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return AppendObjectLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(IStyledToStringObject? value)
    {
        value?.ToString(MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    [CallsObjectToString]
    public IFLogStringAppender AppendObjectLine(object? value) => MessageSb.Append(value).AppendLine(this);
}
