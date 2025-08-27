using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
        [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceSpanFmtTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceSpanFmtTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendStyled(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(ICharSequence? value) =>
        (PreCheckTokensGetStringBuilder(value)?.ReplaceStagingTokenNumber(value) ?? this).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender WithOnlyParamThenToAppender(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

}
