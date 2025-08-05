// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using System.Text;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogAdditionalFormatterParameterEntry : IFLogFormatterParameterEntry
{
    // ReSharper disable UnusedMember.Global

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(bool value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(bool? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TNum>(TNum value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TNum>(TNum? value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(char[]? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(char[]? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(string? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(Span<char> value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(Span<char> value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(object? value);

    void AndFinalParam(bool value);
    void AndFinalParam(bool? value);
    void AndFinalParam<TNum>(TNum value) where TNum : struct, INumber<TNum>;
    void AndFinalParam<TNum>(TNum? value) where TNum : struct, INumber<TNum>;
    void AndFinalParam<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void AndFinalParam<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void AndFinalParam(char[]? value);
    void AndFinalParam(char[]? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(string? value);
    void AndFinalParam(string? value, int startIndex, int count = int.MaxValue);
    void AndFinalParam(ICharSequence? value);
    void AndFinalParam(ICharSequence? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(StringBuilder? value);
    void AndFinalParam(StringBuilder? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(Span<char> value);
    void AndFinalParam(Span<char> value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(ReadOnlySpan<char> value);
    void AndFinalParam(ReadOnlySpan<char> value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(IStyledToStringObject? value);
    void AndFinalParam(object? value);


    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(bool? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(bool value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TNum>(TNum value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TNum>(TNum? value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(char[]? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(char[]? value, int fromIndex, int count);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(string? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ICharSequence? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(Span<char> value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(Span<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(StringBuilder? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(IStyledToStringObject? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(object? value);


    // ReSharper restore UnusedMember.Global
}

public class FLogAdditionalFormatterParameterEntry : FormatParameterEntry<FLogAdditionalFormatterParameterEntry>, IFLogAdditionalFormatterParameterEntry
{
    public FLogAdditionalFormatterParameterEntry() { }

    public FLogAdditionalFormatterParameterEntry(FLogAdditionalFormatterParameterEntry toClone) : base(toClone) { }


    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TNum>(TNum value) where TNum : struct, INumber<TNum> =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TNum>(TNum? value) where TNum : struct, INumber<TNum> =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(Span<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(Span<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ExpectContinue(value);

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
    public IFLogAdditionalFormatterParameterEntry? And(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    public void AndFinalParam(bool value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(bool? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TNum>(TNum value) where TNum : struct, INumber<TNum> =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TNum>(TNum? value) where TNum : struct, INumber<TNum> =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(Span<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(Span<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(char[]? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(string? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(object? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);


    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender<TNum>(TNum value) where TNum : struct, INumber<TNum> =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender<TNum>(TNum? value) where TNum : struct, INumber<TNum> =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(Span<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(Span<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);


    public override FLogAdditionalFormatterParameterEntry Clone() =>
        Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new FLogAdditionalFormatterParameterEntry(this);

}
