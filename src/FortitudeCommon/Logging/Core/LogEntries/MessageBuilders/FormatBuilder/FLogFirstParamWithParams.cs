// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using JetBrains.Annotations;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TFmt>(TFmt value) where TFmt : ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceSpanFmtTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TToStyle, TStylerType>(TToStyle value, PalantírReveal<TStylerType> palantírReveal)
        where TToStyle : TStylerType =>
        PreCheckTokensGetStringBuilder(value).ReplaceCustStyleTokens(value, palantírReveal)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TToStyle, TStylerType>((TToStyle, PalantírReveal<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendStyled(valueTuple, FormatStsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value, startIndex, count)?.ToAdditionalFormatBuilder(value);

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
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count)?.ToAdditionalFormatBuilder(value);

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
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count)?.ToAdditionalFormatBuilder(value);

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
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(IStringBearer? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    [CallsObjectToString]
    public IFLogAdditionalFormatterParameterEntry? WithObjectParams(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokensMatch(value)?.ToAdditionalFormatBuilder(value);
}
