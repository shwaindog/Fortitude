using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry
{
    public void AndFinalParam(bool value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(bool? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(string? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam((string?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(char[]? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam((char[]?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam((ICharSequence?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam((StringBuilder?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(object? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

}
