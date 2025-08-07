// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogFirstFormatterParameterEntry : IFLogFormatterParameterEntry
{
    // ReSharper disable UnusedMember.Global


    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithMatchParams<T>(T value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(bool? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(bool value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TNum>(TNum value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TNum>(TNum? value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((string?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((string?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(object? value);

    void WithOnlyMatchParam<T>(T value);
    void WithOnlyParam(bool? value);
    void WithOnlyParam(bool value);
    void WithOnlyParam<TNum>(TNum value) where TNum : struct, INumber<TNum>;
    void WithOnlyParam<TNum>(TNum? value) where TNum : struct, INumber<TNum>;
    void WithOnlyParam<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void WithOnlyParam<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    void WithOnlyParam<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void WithOnlyParam<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    void WithOnlyParam(ReadOnlySpan<char> value);
    void WithOnlyParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(string? value);
    void WithOnlyParam((string?, int) valueTuple);
    void WithOnlyParam((string?, int, int) valueTuple);
    void WithOnlyParam(string? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(char[]? value);
    void WithOnlyParam((char[]?, int) valueTuple);
    void WithOnlyParam((char[]?, int, int) valueTuple);
    void WithOnlyParam(char[]? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(ICharSequence? value);
    void WithOnlyParam((ICharSequence?, int) valueTuple);
    void WithOnlyParam((ICharSequence?, int, int) valueTuple);
    void WithOnlyParam(ICharSequence? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(StringBuilder? value);
    void WithOnlyParam((StringBuilder?, int) valueTuple);
    void WithOnlyParam((StringBuilder?, int, int) valueTuple);
    void WithOnlyParam(StringBuilder? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(IStyledToStringObject? value);
    void WithOnlyParam(object? value);

    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamMatchToStringAppender<T>(T value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(bool? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(bool value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TNum>(TNum value) where TNum : struct, INumber<TNum>;
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TNum>(TNum? value) where TNum : struct, INumber<TNum>;
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(string? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((string?, int) valueTuple);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((string?, int, int) valueTuple);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(string? value, int startIndex, int count = int.MaxValue);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((char[]?, int) valueTuple);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((char[]?, int, int) valueTuple);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((ICharSequence?, int) valueTuple);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((ICharSequence?, int, int) valueTuple);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((StringBuilder?, int) valueTuple);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((StringBuilder?, int, int) valueTuple);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value, int startIndex, int count = int.MaxValue);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(IStyledToStringObject? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(object? value);

    // ReSharper restore UnusedMember.Global
}

public class FLogFirstFormatterParameterEntry : FormatParameterEntry<FLogFirstFormatterParameterEntry>, IFLogFirstFormatterParameterEntry
{
    public FLogFirstFormatterParameterEntry() { }

    public FLogFirstFormatterParameterEntry(FLogFirstFormatterParameterEntry toClone) : base(toClone) { }


    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithMatchParams<T>(T value)
    {
        Sb.Clear();
        AppendMatchSelect(value, Stsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(value);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(bool value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TNum>(TNum value) where TNum : struct, INumber<TNum> =>
    PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TNum>(TNum? value) where TNum : struct, INumber<TNum> =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        Sb.Clear();
        AppendStruct(valueTuple, Stsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        Sb.Clear();
        AppendStruct(valueTuple, Stsa!);
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
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((string?, int, int) valueTuple)
    {
        Sb.Clear();
        AppendFromToRange(valueTuple, Stsa!);
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
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int, int) valueTuple)
    {
        Sb.Clear();
        AppendFromToRange(valueTuple, Stsa!);
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
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int, int) valueTuple)
    {
        Sb.Clear();
        AppendFromToRange(valueTuple, Stsa!);
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
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        ReplaceTokenNumber();
        return ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int, int) valueTuple)
    {
        Sb.Clear();
        AppendFromToRange(valueTuple, Stsa!);
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
    
    public void WithOnlyMatchParam<T>(T? value)
    {
        Sb.Clear();
        AppendMatchSelect(value, Stsa!);
        ReplaceTokenNumber();
    }

    public void WithOnlyParam(bool? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    public void WithOnlyParam(bool value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TNum>(TNum value) where TNum : struct, INumber<TNum> => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TNum>(TNum? value) where TNum : struct, INumber<TNum> => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        Sb.Clear();
        AppendStruct(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        Sb.Clear();
        AppendStruct(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void WithOnlyParam(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.EnsureNoMoreTokensAndComplete(value);
    
    public void WithOnlyParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(string? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    
    public void WithOnlyParam((string?, int) valueTuple)
    {
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((string?, int, int) valueTuple)
    {
        Sb.Clear();
        AppendFromToRange(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(char[]? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    
    public void WithOnlyParam((char[]?, int) valueTuple)
    {
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((char[]?, int, int) valueTuple)
    {
        Sb.Clear();
        AppendFromToRange(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(char[]? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    
    public void WithOnlyParam((ICharSequence?, int) valueTuple)
    {
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((ICharSequence?, int, int) valueTuple)
    {
        Sb.Clear();
        AppendFromToRange(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    
    public void WithOnlyParam((StringBuilder?, int) valueTuple)
    {
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam((StringBuilder?, int, int) valueTuple)
    {
        Sb.Clear();
        AppendFromToRange(valueTuple, Stsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyParam(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(object? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public IFLogStringAppender AfterOnlyParamMatchToStringAppender<T>(T value)
    {
        Sb.Clear();
        AppendMatchSelect(value, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(bool? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(bool value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TNum>(TNum value) where TNum : struct, INumber<TNum> => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TNum>(TNum? value) where TNum : struct, INumber<TNum> => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct  => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        Sb.Clear();
        AppendStruct(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        Sb.Clear();
        AppendStruct(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(string? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((string?, int) valueTuple)
    {
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(string? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((char[]?, int) valueTuple)
    {
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((ICharSequence?, int) valueTuple)
    {
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((StringBuilder?, int) valueTuple)
    {
        Sb.Clear();
        AppendFromRange(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, Stsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(IStyledToStringObject? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(object? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);


    internal FLogAdditionalFormatterParameterEntry? ToAdditionalFormatBuilder<T>(T paramValue, [CallerMemberName] string callMemberName = "")
    {
        if (!FormatTokens.Any())
        {
            Warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] has no more remaining tokens after replacing with the first parameter")
                    .Append(0)
                    .Append("}");
            CallOnComplete();
            return null;
        }
        var addParamsBuilder = Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>()
                                       .Initialize(LogEntry, FormatBuilder, 1, OnComplete, Stsa!, FormatTokens);
        Stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    internal FLogAdditionalFormatterParameterEntry? ToAdditionalFormatBuilder(ReadOnlySpan<char> paramValue, [CallerMemberName] string callMemberName = "")
    {
        if (!FormatTokens.Any())
        {
            Warnings.Append(callMemberName).Append("(").Append(typeof(ReadOnlySpan<char>).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] has no more remaining tokens after replacing with the first parameter")
                    .Append(0)
                    .Append("}");
            CallOnComplete();
            return null;
        }
        var addParamsBuilder = Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>()
                                       .Initialize(LogEntry, FormatBuilder, 1, OnComplete, Stsa!, FormatTokens);
        Stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    public override FLogFirstFormatterParameterEntry Clone() =>
        Recycler?.Borrow<FLogFirstFormatterParameterEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) 
     ?? new FLogFirstFormatterParameterEntry(this);
}
