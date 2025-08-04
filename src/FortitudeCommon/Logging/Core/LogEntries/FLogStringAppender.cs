// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
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
    IFLogStringAppender Append(IMutableString? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(IStyledToStringObject? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append<TNum>(TNum? value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append<TNum>(TNum value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(object? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(char[]? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(string? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(string? value, int startIndex, int length);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine();

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(IMutableString? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(IStyledToStringObject? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(bool? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(sbyte? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(byte? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(char? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(short? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ushort? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(int? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(uint? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(float? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(long? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ulong? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(double? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(decimal? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(object? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(char[]? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(string? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(string? value, int startIndex, int length);

    void FinalAppend(IMutableString? value);
    void FinalAppend(IStyledToStringObject? value);
    void FinalAppend(bool? value);
    void FinalAppend(sbyte? value);
    void FinalAppend(byte? value);
    void FinalAppend(char? value);
    void FinalAppend(short? value);
    void FinalAppend(ushort? value);
    void FinalAppend(int? value);
    void FinalAppend(uint? value);
    void FinalAppend(float? value);
    void FinalAppend(long? value);
    void FinalAppend(ulong? value);
    void FinalAppend(double? value);
    void FinalAppend(decimal? value);
    void FinalAppend(object? value);
    void FinalAppend(char[]? value);
    void FinalAppend(string? value);
    void FinalAppend(string? value, int startIndex, int length);
    
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
    public IFLogStringAppender Append(string? value, int startIndex, int length) => sb.Append(value, startIndex, length).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value)  => sb.Append(value).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value)  => sb.Append(value).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(object? value)  => sb.Append(value).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TNum>(TNum? value) where TNum: struct, INumber<TNum> => sb.Append(value).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TNum>(TNum value) where TNum: struct, INumber<TNum> => sb.Append(value).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(IMutableString? value) => sb.Append(value).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(IStyledToStringObject? value)
    {
        value?.ToString(stsa!);
        return this;
    }
    
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value, int startIndex, int length) =>
        sb.Append(value, startIndex, length).AppendLine(this);
    
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(object? value)  => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(decimal? value) => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(double? value)  => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ulong? value)   => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(long? value)    => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(float? value)   => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(uint? value)    => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ushort? value)  => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(int? value)     => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char? value)    => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(byte? value)    => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(sbyte? value)   => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool? value)    => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value)  => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(short? value)   => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value)  => sb.Append(value).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine()               => sb.AppendLine().ToAppender(this);
    
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(IMutableString? value) => sb.Append(value).AppendLine().ToAppender(this);
    
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(IStyledToStringObject? value)
    {
        value?.ToString(stsa!);
        return this;
    }

    public int Count => sb.Length;

    public void FinalAppend(char[]? value)  => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(object? value)  => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(decimal? value) => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(double? value)  => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(ulong? value)   => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(long? value)    => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(float? value)   => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(string? value)  => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(uint? value)    => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(ushort? value)  => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(short? value)   => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(char? value)    => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(byte? value)    => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(sbyte? value)   => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(bool? value)    => sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(int? value)     => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(IMutableString? value) => sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(IStyledToStringObject? value)
    {
        value?.ToString(stsa!);
        CallOnComplete();
    }

    public void FinalAppend(string? value, int startIndex, int length) => sb.Append(value, startIndex, length).ToAppender(this).CallOnComplete();

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
