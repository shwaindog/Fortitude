// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry
{
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender<TFmt>(TFmt value) where TFmt : ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceSpanFmtTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender<TToStyle, TStylerType>(TToStyle value, PalantírReveal<TStylerType> palantírReveal)
        where TToStyle : TStylerType =>
        PreCheckTokensGetStringBuilder(value).ReplaceCustStyleTokens(value, palantírReveal).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogStringAppender AndFinalParamThenToAppender<TToStyle, TStylerType>((TToStyle, PalantírReveal<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendStyled(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender((string?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender((char[]?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender((ICharSequence?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender((StringBuilder?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalParamThenToAppender(IStringBearer? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalObjectParamThenToAppender(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokensMatch(value).ToStringAppender(value, this);
}
