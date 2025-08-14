using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
        public void WithOnlyParam(bool? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    public void WithOnlyParam(bool value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(string? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((string?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(char[]? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((char[]?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((ICharSequence?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((StringBuilder?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(object? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

}
