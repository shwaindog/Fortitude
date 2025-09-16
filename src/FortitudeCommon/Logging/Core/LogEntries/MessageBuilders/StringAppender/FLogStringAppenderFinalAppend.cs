// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender
{
    public void FinalAppend(bool value) => MessageSb.Append(value ? "true" : "false").ToAppender(this).CallOnComplete();

    public void FinalAppend(bool? value) =>
        MessageSb.Append(value != null ? value.Value ? "true" : "false" : "null").ToAppender(this).CallOnComplete();


    public void FinalAppend<TFmt>(TFmt value, string? formatString = null) where TFmt : ISpanFormattable
    {
        (formatString != null ? MessageSb.AppendFormat(formatString, value) : MessageSb.Append(value))
            .ToAppender(this).CallOnComplete();
    }

    public void FinalAppend<TFmt>((TFmt, string) valueTuple) where TFmt : ISpanFormattable
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        customTypeStyler(value, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        AppendStyled(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(ReadOnlySpan<char> value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value[startIndex..(startIndex + count)]).ToAppender(this).CallOnComplete();

    public void FinalAppend(string? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(string? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(char[]? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(char[]? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(ICharSequence? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((ICharSequence?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(StringBuilder? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((StringBuilder?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(IStyledToStringObject? value)
    {
        AppendStyledObject(value, MessageStsa);
        CallOnComplete();
    }

    [CallsObjectToString] public void FinalAppendObject(object? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppendFormat<TFmt>(TFmt value, string formatString) where TFmt : ISpanFormattable
    {
        MessageSb.AppendFormat(formatString, value);
        CallOnComplete();
    }

    public void FinalAppendFormat<TFmt>((TFmt, string) valueTuple) where TFmt : ISpanFormattable
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        CallOnComplete();
    }
}
