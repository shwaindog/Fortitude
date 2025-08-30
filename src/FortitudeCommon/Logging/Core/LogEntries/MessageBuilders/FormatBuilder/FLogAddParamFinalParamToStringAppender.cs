using System.Numerics;
using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
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
    public IFLogStringAppender AndFinalParamThenToAppender<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType =>
        PreCheckTokensGetStringBuilder(value).ReplaceCustStyleTokens(value, customTypeStyler).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogStringAppender AndFinalParamThenToAppender<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) 
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
    public IFLogStringAppender AndFinalParamThenToAppender(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AndFinalObjectParamThenToAppender(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokensMatch(value).ToStringAppender(value, this);

}
