// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogStringAppender : IReusableObject<IFLogStringAppender>
{
    // ReSharper disable UnusedMember.Global
    int Count { get; }

    IStyledTypeStringAppender BackingStyledTypeStringAppender { get; }

    IStringBuilder WriteBuffer { get; }

    string Indent { get; set; }

    int IndentLevel { get; }

    IFLogStringAppender IncrementIndent();

    IFLogStringAppender DecrementIndent();


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(bool value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(bool? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append<TNum>(TNum value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append<TNum>(TNum? value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(Span<char> value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(Span<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(string? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(char[]? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(ICharSequence? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(StringBuilder? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(IStyledToStringObject? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(object? value);


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendFormatted<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendFormatted<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum>;


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(bool value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(bool? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine<TNum>(TNum value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine<TNum>(TNum? value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(Span<char> value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(Span<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(string? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(char[]? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ICharSequence? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(StringBuilder? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(IStyledToStringObject? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(object? value);

    void FinalAppend(bool value);
    void FinalAppend(bool? value);
    void FinalAppend<TNum>(TNum value) where TNum : struct, INumber<TNum>;
    void FinalAppend<TNum>(TNum? value) where TNum : struct, INumber<TNum>;
    void FinalAppend<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void FinalAppend<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void FinalAppend(Span<char> value);
    void FinalAppend(Span<char> value, int startIndex, int count = int.MaxValue);
    void FinalAppend(ReadOnlySpan<char> value);
    void FinalAppend(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    void FinalAppend(string? value);
    void FinalAppend(string? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(char[]? value);
    void FinalAppend(char[]? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(ICharSequence? value);
    void FinalAppend(ICharSequence? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(StringBuilder? value);
    void FinalAppend(StringBuilder? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(IStyledToStringObject? value);
    void FinalAppend(object? value);

    // ReSharper restore UnusedMember.Global
}

public class FLogStringAppender : ReusableObject<IFLogStringAppender>, IFLogStringAppender
{
    private IStyledTypeStringAppender? stsa;

    private IStringBuilder sb = null!;

    private Action<IStringBuilder?> onComplete = null!;

    public FLogStringAppender() { }

    public FLogStringAppender(IStyledTypeStringAppender useStyleTypeStringBuilder, Action<IStringBuilder?> callWhenComplete)
    {
        Initialize(useStyleTypeStringBuilder, callWhenComplete);
    }

    public FLogStringAppender(IFLogStringAppender toClone)
    {
        sb = new MutableString();
        sb.AppendRange(toClone.WriteBuffer);
    }

    public FLogStringAppender Initialize(IStyledTypeStringAppender useStyleTypeStringBuilder, Action<IStringBuilder?> callWhenComplete)
    {
        onComplete = callWhenComplete;

        stsa = useStyleTypeStringBuilder;
        sb   = stsa.WriteBuffer;

        return this;
    }

    public string Indent
    {
        get => stsa!.Indent;
        set => stsa!.Indent = value;
    }

    public int Count => sb.Length;

    public int IndentLevel { get; protected set; }

    public IStyledTypeStringAppender BackingStyledTypeStringAppender =>
        stsa ?? throw new NullReferenceException("This should never be the case if Initialize is called");

    public IStringBuilder WriteBuffer => sb;

    public IFLogStringAppender DecrementIndent()
    {
        IndentLevel++;
        return this;
    }

    public IFLogStringAppender IncrementIndent()
    {
        IndentLevel--;
        return this;
    }


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(bool? value) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(bool value) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TNum>(TNum value) where TNum : struct, INumber<TNum> => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TNum>(TNum? value) where TNum : struct, INumber<TNum> => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>(TStruct value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        structStyler(value, stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>(TStruct? value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        if (value != null) structStyler(value.Value, stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(Span<char> value) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(Span<char> value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value[startIndex..(startIndex + count)]).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ReadOnlySpan<char> value) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value[startIndex..(startIndex + count)]).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value, startIndex, count).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value, int startIndex, int count = int.MaxValue) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ICharSequence? value) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ICharSequence? value, int startIndex, int count = int.MaxValue) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(StringBuilder? value) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(StringBuilder? value, int startIndex, int count = int.MaxValue) => sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(IStyledToStringObject? value)
    {
        value?.ToString(stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(object? value) => sb.Append(value).ToAppender(this);


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormatted<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum> =>
        sb.AppendFormat(formatString, value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormatted<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum> =>
        sb.AppendFormat(formatString, value).ToAppender(this);


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine() => sb.AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool value) => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool? value) => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TNum>(TNum value) where TNum : struct, INumber<TNum> => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TNum>(TNum? value) where TNum : struct, INumber<TNum> => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>(TStruct value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        structStyler(value, stsa!);
        return sb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>(TStruct? value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        if (value != null) structStyler(value.Value, stsa!);
        return sb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(Span<char> value) => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(Span<char> value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value[startIndex..(startIndex + count)]).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ReadOnlySpan<char> value) => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value[startIndex..(startIndex + count)]).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value) => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value, startIndex, count).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value) => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value, int startIndex, int count = int.MaxValue) => sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ICharSequence? value) => sb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(StringBuilder? value) => sb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(IStyledToStringObject? value)
    {
        value?.ToString(stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(object? value) => sb.Append(value).AppendLine(this);


    public void FinalAppend(bool value)  => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(bool? value) => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TNum>(TNum value) where TNum : struct, INumber<TNum> => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TNum>(TNum? value) where TNum : struct, INumber<TNum> => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(Span<char> value) => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(Span<char> value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value[startIndex..(startIndex + count)]).ToAppender(this).CallOnComplete();

    public void FinalAppend(ReadOnlySpan<char> value) => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value[startIndex..(startIndex + count)]).ToAppender(this).CallOnComplete();

    public void FinalAppend(string? value) => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(string? value, int startIndex, int count = int.MaxValue) =>
        sb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(char[]? value) => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(char[]? value, int startIndex, int count = int.MaxValue) => 
        sb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(ICharSequence? value) => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(ICharSequence? value, int startIndex, int count = int.MaxValue) => 
        sb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(StringBuilder? value) => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(StringBuilder? value, int startIndex, int count = int.MaxValue) => 
        sb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(IStyledToStringObject? value)
    {
        value?.ToString(stsa!);
        CallOnComplete();
    }

    public void FinalAppend(object? value) => sb.Append(value).ToAppender(this).CallOnComplete();

    protected void CallOnComplete()
    {
        onComplete(null);
        DecrementRefCount();
    }

    public override void StateReset()
    {
        stsa = null!;
        sb   = null!;

        onComplete = null!;

        base.StateReset();
    }

    public override IFLogStringAppender Clone() =>
        Recycler?.Borrow<FLogStringAppender>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogStringAppender(this);

    public override IFLogStringAppender CopyFrom
        (IFLogStringAppender source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        stsa ??= Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(source.BackingStyledTypeStringAppender);
        sb   =   stsa.WriteBuffer;

        if (stsa.Style != source.BackingStyledTypeStringAppender.Style)
        {
            stsa.ClearAndReinitialize(source.BackingStyledTypeStringAppender.Style);
        }
        else if (copyMergeFlags == CopyMergeFlags.FullReplace)
        {
            sb.Clear();
        }
        sb.AppendRange(source.WriteBuffer);

        return this;
    }
}

public static class FLogStringAppenderExtensions
{
    public static FLogStringAppender AppendLine(this IStringBuilder sb, FLogStringAppender toReturn)
    {
        var style = toReturn.BackingStyledTypeStringAppender.Style;
        if (style.IsCompact())
        {
            return toReturn;
        }
        sb.AppendLine();
        if (style.IsPretty())
        {
            var indentLevel  = toReturn.IndentLevel;
            var indentString = toReturn.Indent;
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append(indentString);
            }
        }
        return toReturn;
    }

    public static FLogStringAppender ToAppender(this IStringBuilder _, FLogStringAppender toReturn) => toReturn;
}
