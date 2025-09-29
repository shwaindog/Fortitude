// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using JetBrains.Annotations;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender
{
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(bool? value) => MessageSb.Append(value != null ? value.Value ? "true" : "false" : "null").ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(bool value) => MessageSb.Append(value ? "true" : "false").ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TFmt>(TFmt value, string? formatString = null) where TFmt : ISpanFormattable =>
        MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TFmt>((TFmt, string) valueTuple) where TFmt : ISpanFormattable
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        return this;
    }


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TToStyle, TStylerType>(TToStyle value, PalantírReveal<TStylerType> palantírReveal)
        where TToStyle : TStylerType
    {
        palantírReveal(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TToStyle, TStylerType>((TToStyle, PalantírReveal<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        AppendStyled(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ReadOnlySpan<char> value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value[startIndex..(startIndex + count)]).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ICharSequence? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((ICharSequence?, int) value)
    {
        AppendFromRange(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((ICharSequence?, int, int) value)
    {
        AppendFromToRange(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(StringBuilder? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((StringBuilder?, int) value)
    {
        AppendFromRange(value, MessageStsa);
        return this;
    }


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((StringBuilder?, int, int) value)
    {
        AppendFromToRange(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(IStringBearer? value)
    {
        value?.RevealState(MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    [CallsObjectToString]
    public IFLogStringAppender AppendObject(object? value)
    {
        AppendObject(value, MessageStsa);
        return this;
    }
}
