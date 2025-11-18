// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry
{
    public void AndFinalParam(bool value) => PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(bool? value) => PreCheckTokensGetStringBuilder(value).ReplaceBoolTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TFmt>(TFmt value) where TFmt : ISpanFormattable =>
        PreCheckTokensGetStringBuilder(value).ReplaceSpanFmtTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TCloaked, TRevealBase>(TCloaked? value, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TRevealBase 
        where TRevealBase : notnull  =>
        PreCheckTokensGetStringBuilder(value).ReplaceCustStyleTokens(value, palantírReveal).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TCloaked, TRevealBase>((TCloaked?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase 
        where TRevealBase : notnull 
    {
        FormatSb.Clear();
        AppendStyled(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(string? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam((string?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam((string?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(char[]? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam((char[]?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam((char[]?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam((ICharSequence?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam((ICharSequence?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam((StringBuilder?, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam((StringBuilder?, int, int) valueTuple)
    {
        FormatSb.Clear();
        AppendFromToRange(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalParam(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(IStringBearer? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).CallEnsureNoMoreTokensAndComplete(value);

    public void AndFinalObjectParam(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokensMatch(value).CallEnsureNoMoreTokensAndComplete(value);
}
