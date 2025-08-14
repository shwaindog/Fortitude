using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
        [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

}
