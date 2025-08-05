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
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(Span<char> value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(Span<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(object? value);

    void WithOnlyParam(bool? value);
    void WithOnlyParam(bool value);
    void WithOnlyParam<TNum>(TNum value) where TNum : struct, INumber<TNum>;
    void WithOnlyParam<TNum>(TNum? value) where TNum : struct, INumber<TNum>;
    void WithOnlyParam<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void WithOnlyParam<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void WithOnlyParam(Span<char> value);
    void WithOnlyParam(Span<char> value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(ReadOnlySpan<char> value);
    void WithOnlyParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(string? value);
    void WithOnlyParam(string? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(char[]? value);
    void WithOnlyParam(char[]? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(ICharSequence? value);
    void WithOnlyParam(ICharSequence? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(StringBuilder? value);
    void WithOnlyParam(StringBuilder? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(IStyledToStringObject? value);
    void WithOnlyParam(object? value);
    
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
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(Span<char> value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(Span<char> value, int startIndex, int count = int.MaxValue);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(string? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(string? value, int startIndex, int count = int.MaxValue);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value);
    
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
    public IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(Span<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(Span<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.ToAdditionalFormatBuilder(value);
    
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
    public IFLogAdditionalFormatterParameterEntry? WithParams(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value)?.ToAdditionalFormatBuilder(value);
    
    public void WithOnlyParam(bool? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    public void WithOnlyParam(bool value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TNum>(TNum value) where TNum : struct, INumber<TNum> => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TNum>(TNum? value) where TNum : struct, INumber<TNum> => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(Span<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.EnsureNoMoreTokensAndComplete(value);
    
    public void WithOnlyParam(Span<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.EnsureNoMoreTokensAndComplete(value);
    
    public void WithOnlyParam(ReadOnlySpan<char> value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.EnsureNoMoreTokensAndComplete(value);
    
    public void WithOnlyParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value)?.EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(string? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(string? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, count).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(char[]? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(char[]? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ICharSequence? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(StringBuilder? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(object? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    
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
    public IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(Span<char> value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(Span<char> value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceCharSpanTokens(value).ToStringAppender(value, this);
    
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
    public IFLogStringAppender AfterOnlyParamToStringAppender(string? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

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
        if (!formatTokens.Any())
        {
            warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(loggingLocation)
                    .Append("] has no more remaining tokens after replacing with the first parameter")
                    .Append(0)
                    .Append("}");
            CallOnComplete();
            return null;
        }
        var addParamsBuilder = Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>()
                                       .Initialize(formatBuilder, 1, loggingLocation, onComplete, stsa!.Style, stsa!, formatTokens);
        stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    internal FLogAdditionalFormatterParameterEntry? ToAdditionalFormatBuilder(ReadOnlySpan<char> paramValue, [CallerMemberName] string callMemberName = "")
    {
        if (!formatTokens.Any())
        {
            warnings.Append(callMemberName).Append("(").Append(typeof(ReadOnlySpan<char>).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(loggingLocation)
                    .Append("] has no more remaining tokens after replacing with the first parameter")
                    .Append(0)
                    .Append("}");
            CallOnComplete();
            return null;
        }
        var addParamsBuilder = Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>()
                                       .Initialize(formatBuilder, 1, loggingLocation, onComplete, stsa!.Style, stsa!, formatTokens);
        stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    internal FLogAdditionalFormatterParameterEntry? ToAdditionalFormatBuilder(Span<char> paramValue, [CallerMemberName] string callMemberName = "")
    {
        if (!formatTokens.Any())
        {
            warnings.Append(callMemberName).Append("(").Append(typeof(Span<char>).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(loggingLocation)
                    .Append("] has no more remaining tokens after replacing with the first parameter")
                    .Append(0)
                    .Append("}");
            CallOnComplete();
            return null;
        }
        var addParamsBuilder = Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>()
                                       .Initialize(formatBuilder, 1, loggingLocation, onComplete, stsa!.Style, stsa!, formatTokens);
        stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    public override FLogFirstFormatterParameterEntry Clone() =>
        Recycler?.Borrow<FLogFirstFormatterParameterEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) 
     ?? new FLogFirstFormatterParameterEntry(this);
}
