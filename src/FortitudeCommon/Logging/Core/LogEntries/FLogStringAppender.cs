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

public interface IFLogStringAppender : IFLogMessageBuilder
{
    // ReSharper disable UnusedMember.Global
    int Count { get; }

    string Indent { get; set; }

    IFLogStringAppender IncrementIndent();

    IFLogStringAppender DecrementIndent();


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendMatch<T>(T value);

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
    IFLogStringAppender Append<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(string? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append((string?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append((string?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(char[]? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append((char[]?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(ICharSequence? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append((ICharSequence?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(StringBuilder? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append((StringBuilder?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(IStyledToStringObject? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender Append(object? value);


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendNumberFormatted<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendNumberFormatted<TNum>((TNum, string) value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendNumberFormatted<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendNumberFormatted<TNum>((TNum?, string) value) where TNum : struct, INumber<TNum>;


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendMatchLine<T>(T value);

    IFLogStringAppender AppendLine();

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
    IFLogStringAppender AppendLine<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(string? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine((string?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine((string?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(char[]? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine((char[]?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ICharSequence? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine((ICharSequence?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(StringBuilder? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine((StringBuilder?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(IStyledToStringObject? value);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendLine(object? value);


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendNumberFormattedLine<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendNumberFormattedLine<TNum>((TNum, string) value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendNumberFormattedLine<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    IFLogStringAppender AppendNumberFormattedLine<TNum>((TNum?, string) value) where TNum : struct, INumber<TNum>;

    void FinalMatchAppend<T>(T value);
    void FinalAppend(bool value);
    void FinalAppend(bool? value);
    void FinalAppend<TNum>(TNum value) where TNum : struct, INumber<TNum>;
    void FinalAppend<TNum>(TNum? value) where TNum : struct, INumber<TNum>;
    void FinalAppend<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void FinalAppend<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    void FinalAppend<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void FinalAppend<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    void FinalAppend(ReadOnlySpan<char> value);
    void FinalAppend(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    void FinalAppend(string? value);
    void FinalAppend((string?, int) valueTuple);
    void FinalAppend((string?, int, int) valueTuple);
    void FinalAppend(string? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(char[]? value);
    void FinalAppend((char[]?, int) valueTuple);
    void FinalAppend((char[]?, int, int) valueTuple);
    void FinalAppend(char[]? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(ICharSequence? value);
    void FinalAppend((ICharSequence?, int) valueTuple);
    void FinalAppend((ICharSequence?, int, int) valueTuple);
    void FinalAppend(ICharSequence? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(StringBuilder? value);
    void FinalAppend((StringBuilder?, int) valueTuple);
    void FinalAppend((StringBuilder?, int, int) valueTuple);
    void FinalAppend(StringBuilder? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(IStyledToStringObject? value);
    void FinalAppend(object? value);

    void FinalAppendNumberFormatted<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum>;
    void FinalAppendNumberFormatted<TNum>((TNum, string) value) where TNum : struct, INumber<TNum>;
    void FinalAppendNumberFormatted<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum>;
    void FinalAppendNumberFormatted<TNum>((TNum?, string) value) where TNum : struct, INumber<TNum>;

    // ReSharper restore UnusedMember.Global
}

public class FLogStringAppender : FLogEntryMessageBuilderBase<FLogStringAppender>, IFLogStringAppender
{
    public FLogStringAppender() { }

    public FLogStringAppender(FLogEntry flogEntry, IStyledTypeStringAppender useStyleTypeStringBuilder
      , Action<IStringBuilder?> callWhenComplete)
    {
        Initialize(flogEntry, useStyleTypeStringBuilder, callWhenComplete);
    }

    public FLogStringAppender(IFLogStringAppender toClone)
    {
        Sb = new MutableString();
        Sb.AppendRange(toClone.WriteBuffer);
    }

    public FLogStringAppender Initialize(FLogEntry flogEntry, IStyledTypeStringAppender useStyleTypeStringBuilder
      , Action<IStringBuilder?> callWhenComplete)
    {
        base.Initialize(flogEntry, callWhenComplete, useStyleTypeStringBuilder);

        Stsa = useStyleTypeStringBuilder;
        Sb   = Stsa.WriteBuffer;

        return this;
    }

    public string Indent
    {
        get => Stsa!.Indent;
        set => Stsa!.Indent = value;
    }

    public int Count => Sb.Length;

    public int IndentLevel { get; protected set; }

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
    public IFLogStringAppender AppendMatch<T>(T value)
    {
        AppendMatchSelect(value, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(bool? value) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(bool value) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TNum>(TNum value) where TNum : struct, INumber<TNum> => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TNum>(TNum? value) where TNum : struct, INumber<TNum> => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>(TStruct value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        structStyler(value, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>((TStruct, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>(TStruct? value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        if (value != null) structStyler(value.Value, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ReadOnlySpan<char> value) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value[startIndex..(startIndex + count)]).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value, startIndex, count).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value, int startIndex, int count = int.MaxValue) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ICharSequence? value) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((ICharSequence?, int) value)
    {
        AppendFromRange(value, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((ICharSequence?, int, int) value)
    {
        AppendFromToRange(value, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ICharSequence? value, int startIndex, int count = int.MaxValue) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(StringBuilder? value) => Sb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((StringBuilder?, int) value)
    {
        AppendFromRange(value, Stsa!);
        return this;
    }


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((StringBuilder?, int, int) value)
    {
        AppendFromToRange(value, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(StringBuilder? value, int startIndex, int count = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Max(0, (value?.Length ?? 0) - startIndex), count);
        return Sb.Append(value, startIndex, Math.Clamp(count, 0, cappedLength))
                 .ToAppender(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(IStyledToStringObject? value)
    {
        value?.ToString(Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(object? value)
    {
        AppendObject(value, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendNumberFormatted<TNum>((TNum, string) valueTuple) where TNum : struct, INumber<TNum>
    {
        AppendFormattedNumber(valueTuple, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendNumberFormatted<TNum>((TNum?, string) valueTuple) where TNum : struct, INumber<TNum>
    {
        AppendFormattedNumber(valueTuple, Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendNumberFormatted<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum> =>
        Sb.AppendFormat(formatString, value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendNumberFormatted<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum> =>
        Sb.AppendFormat(formatString, value).ToAppender(this);

    public IFLogStringAppender AppendMatchLine<T>(T value)
    {
        AppendMatchSelect(value, Stsa!);
        return AppendLine(this);
    }


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine() => Sb.AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool value) => Sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool? value) => Sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TNum>(TNum value) where TNum : struct, INumber<TNum> => 
        Sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TNum>(TNum? value) where TNum : struct, INumber<TNum> => 
        Sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>(TStruct value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        structStyler(value, Stsa!);
        return Sb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>((TStruct, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, Stsa!);
        return Sb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>(TStruct? value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        if (value != null) structStyler(value.Value, Stsa!);
        return Sb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, Stsa!);
        return Sb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ReadOnlySpan<char> value) => Sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value[startIndex..(startIndex + count)]).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value) => Sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value, startIndex, count).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value) => Sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value, int startIndex, int count = int.MaxValue) => Sb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ICharSequence? value) => Sb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((ICharSequence?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(StringBuilder? value) => Sb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((StringBuilder?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(IStyledToStringObject? value)
    {
        value?.ToString(Stsa!);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(object? value) => Sb.Append(value).AppendLine(this);


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendNumberFormattedLine<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum> =>
        Sb.AppendFormat(formatString, value).AppendLine(this);
    

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendNumberFormattedLine<TNum>((TNum, string) valueTuple) where TNum : struct, INumber<TNum>
    {
        AppendFormattedNumber(valueTuple, Stsa!);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendNumberFormattedLine<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum> =>
        Sb.AppendFormat(formatString, value).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendNumberFormattedLine<TNum>((TNum?, string) valueTuple) where TNum : struct, INumber<TNum>
    {
        AppendFormattedNumber(valueTuple, Stsa!);
        return AppendLine(this);
    }

    public void FinalMatchAppend<T>(T value)
    {
        AppendMatchSelect(value, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend(bool value)  => Sb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(bool? value) => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TNum>(TNum value) where TNum : struct, INumber<TNum> => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TNum>(TNum? value) where TNum : struct, INumber<TNum> => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TStruct>((TStruct, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend(ReadOnlySpan<char> value) => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value[startIndex..(startIndex + count)]).ToAppender(this).CallOnComplete();

    public void FinalAppend(string? value) => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend(string? value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(char[]? value) => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend(char[]? value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(ICharSequence? value) => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((ICharSequence?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(StringBuilder? value) => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((StringBuilder?, int) valueTuple)
    {
        AppendFromRange(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        Sb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(IStyledToStringObject? value)
    {
        AppendStyledObject(value, Stsa!);
        CallOnComplete();
    }

    public void FinalAppend(object? value) => Sb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppendNumberFormatted<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum>
    {
        Sb.AppendFormat(formatString, value);
        CallOnComplete();
    }

    public void FinalAppendNumberFormatted<TNum>((TNum, string) valueTuple) where TNum : struct, INumber<TNum>
    {
        AppendFormattedNumber(valueTuple, Stsa!);
        CallOnComplete();
    }

    public void FinalAppendNumberFormatted<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum>
    {
        Sb.AppendFormat(formatString, value);
        CallOnComplete();
    }

    public void FinalAppendNumberFormatted<TNum>((TNum?, string) valueTuple) where TNum : struct, INumber<TNum>
    {
        AppendFormattedNumber(valueTuple, Stsa!);
        CallOnComplete();
    }

    protected void CallOnComplete()
    {
        OnComplete(null);
        DecrementRefCount();
    }

    public override IFLogStringAppender Clone() =>
        Recycler?.Borrow<FLogStringAppender>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogStringAppender(this);

    public override IFLogStringAppender CopyFrom
        (IFLogMessageBuilder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Stsa ??= Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(source.StyledTypeAppender);
        Sb   =   Stsa.WriteBuffer;

        if (Stsa.Style != source.StyledTypeAppender.Style)
        {
            Stsa.ClearAndReinitialize(source.StyledTypeAppender.Style);
        }
        else if (copyMergeFlags == CopyMergeFlags.FullReplace)
        {
            Sb.Clear();
        }
        Sb.AppendRange(source.WriteBuffer);

        return this;
    }
}

public static class FLogStringAppenderExtensions
{
    public static FLogStringAppender AppendLine(this IStringBuilder sb, FLogStringAppender toReturn)
    {
        var style = toReturn.StyledTypeAppender.Style;
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
