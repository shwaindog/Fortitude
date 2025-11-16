// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
    public void WithOnlyParam(bool? value) => PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).CallEnsureNoMoreTokensAndComplete(value);
    public void WithOnlyParam(bool value)  => PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TFmt>(TFmt value) where TFmt : ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceSpanFmtTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TCloaked, TRevealBase>(TCloaked? value, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TRevealBase 
        where TRevealBase : notnull  =>
        PreCheckTokensGetStringBuilder(value).ReplaceCustStyleTokens(value, palantírReveal).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TCloaked, TRevealBase>((TCloaked?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull 
    {
        FormatSb.Clear();
        AppendStyled(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value, startIndex, count)?.CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(string? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((string?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(char[]? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((char[]?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((ICharSequence?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((StringBuilder?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(IStringBearer? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    [CallsObjectToString]
    public void WithOnlyObjectParam(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokensMatch(value).CallEnsureNoMoreTokensAndComplete(value);
}
