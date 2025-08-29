using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry
{
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TFmt>(TFmt value) where TFmt : ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceSpanFmtTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType =>
        PreCheckTokensGetStringBuilder(value).ReplaceCustStyleTokens(value, customTypeStyler).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendStyled(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((string?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((char[]?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((ICharSequence?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((StringBuilder?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().CallExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObject(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokensMatch(value).CallExpectContinue(value);

}
