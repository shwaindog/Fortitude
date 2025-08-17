using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendStruct(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((string?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);


}
