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
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((string?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((char[]?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((ICharSequence?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And((StringBuilder?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ExpectContinue(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

}
