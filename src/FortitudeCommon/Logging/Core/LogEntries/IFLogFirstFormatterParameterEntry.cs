// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogFirstFormatterParameterEntry : IReusableObject<IFLogFirstFormatterParameterEntry>
{
    // ReSharper disable UnusedMember.Global
    int RemainingArguments { get; }

    IStyledTypeStringAppender BackingStyledTypeStringAppender { get; }

    StringBuilder BackingStringBuilder { get; }


    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(IMutableString? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(bool? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(sbyte? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(byte? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(short? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ushort? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(int? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(uint? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(float? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(long? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ulong? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(double? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(decimal? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(object? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value, int startIndex, int length);

    void WithOnlyParam(IMutableString? value);
    void WithOnlyParam(IStyledToStringObject? value);
    void WithOnlyParam(bool? value);
    void WithOnlyParam(sbyte? value);
    void WithOnlyParam(byte? value);
    void WithOnlyParam(char? value);
    void WithOnlyParam(short? value);
    void WithOnlyParam(ushort? value);
    void WithOnlyParam(int? value);
    void WithOnlyParam(uint? value);
    void WithOnlyParam(float? value);
    void WithOnlyParam(long? value);
    void WithOnlyParam(ulong? value);
    void WithOnlyParam(double? value);
    void WithOnlyParam(decimal? value);
    void WithOnlyParam(object? value);
    void WithOnlyParam(char[]? value);
    void WithOnlyParam(string? value);
    void WithOnlyParam(string? value, int startIndex, int length);

    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(IMutableString? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(IStyledToStringObject? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(bool? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(sbyte? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(byte? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(char? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(short? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ushort? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(int? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(uint? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(float? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(long? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ulong? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(double? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(decimal? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(object? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(string? value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(string? value, int startIndex, int length);

    // ReSharper restore UnusedMember.Global
}

public class FLogFirstFormatterParameterEntry : ReusableObject<IFLogFirstFormatterParameterEntry>, IFLogFirstFormatterParameterEntry
{
    private IStyledTypeStringAppender? stsa;

    private StringBuilder sb = null!;

    private readonly StringBuilder warnings = new();

    private Action<StringBuilder?> onComplete = null!;
    private LoggingLocation        loggingLocation;

    private FormatBuilder                 formatBuilder = null!;
    private List<StringFormatTokenParams> formatTokens  = null!;

    public FLogFirstFormatterParameterEntry() { }

    public FLogFirstFormatterParameterEntry(IFLogFirstFormatterParameterEntry toClone)
    {
        stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        stsa.CopyFrom(toClone.BackingStyledTypeStringAppender);
        sb = stsa.WriteBuffer;
        if (toClone is FLogFirstFormatterParameterEntry firstFormatterParameter)
        {
            warnings.Clear();
            warnings.AppendRange(firstFormatterParameter.warnings);
            onComplete      = firstFormatterParameter.onComplete;
            loggingLocation = firstFormatterParameter.loggingLocation;
            formatBuilder   = firstFormatterParameter.formatBuilder;
            formatTokens    = firstFormatterParameter.formatTokens;
        }
    }

    public FLogFirstFormatterParameterEntry Initialize
    (FormatBuilder stringFormatBuilder
      , LoggingLocation logLocation
      , Action<StringBuilder?> onCompleteHandler
      , StringBuildingStyle style = StringBuildingStyle.Default)
    {
        stsa = (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender()).Initialize(style);
        sb   = stsa.WriteBuffer;

        onComplete              = onCompleteHandler;

        loggingLocation = logLocation;

        formatBuilder = stringFormatBuilder;
        formatTokens  = formatBuilder.RemainingTokens();

        return this;
    }

    public int RemainingArguments => formatTokens.Count;

    public StringBuilder BackingStringBuilder => sb;

    public IStyledTypeStringAppender BackingStyledTypeStringAppender => stsa!;
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(string? value, int startIndex, int length) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, length).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(IMutableString? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(sbyte? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(byte? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(char? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(short? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ushort? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(int? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(float? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(long? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(ulong? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(double? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(decimal? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(uint? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithParams(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToAdditionalFormatBuilder(value);
    
    public void WithOnlyParam(bool? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(sbyte? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(byte? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(char? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(short? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ushort? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(int? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(uint? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(float? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(long? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(ulong? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(double? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(decimal? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(object? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(char[]? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam
        (IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam
        (IMutableString? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(string? value, int startIndex, int length) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, length).EnsureNoMoreTokensAndComplete(value);

    public void WithOnlyParam(string? value) => PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(IMutableString? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(object? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(decimal? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(double? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ulong? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(long? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(float? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(string? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(uint? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(ushort? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(short? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(char? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(byte? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(sbyte? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(bool? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(IStyledToStringObject? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(int? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyParamToStringAppender(string? value, int startIndex, int length) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);

    protected FLogFirstFormatterParameterEntry? PreCheckTokensGetStringBuilder<T>(T param, [CallerMemberName] string memberName = "")
    {
        if (formatTokens.Any()) return this;
        warnings.Append(memberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(param).Append(") at [").Append(loggingLocation)
                .Append("] no formatting tokens remaining");
        return null;
    }

    internal FLogFirstFormatterParameterEntry ReplaceTokenNumber<T>(T? param)
    {
        for (var i = 0; i < formatTokens.Count; i++)
        {
            var token = formatTokens[i];
            if (token.ParameterNumber == 0)
            {
                sb.Clear();
                sb.AppendFormat(token.StringFormat, param);
                formatBuilder.ReplaceTokenWith(token, sb);
            }
        }
        return this;
    }

    internal FLogFirstFormatterParameterEntry ReplaceTokenNumber(string? value, int startIndex, int length)
    {
        for (var i = 0; i < formatTokens.Count; i++)
        {
            var token = formatTokens[i];
            if (token.ParameterNumber == 0)
            {
                sb.Clear();
                sb.Append(value, startIndex, length);
                formatBuilder.ReplaceTokenWith(token, sb);
            }
        }
        return this;
    }

    internal FLogFirstFormatterParameterEntry ReplaceTokenNumber(IStyledToStringObject? param)
    {
        for (var i = 0; i < formatTokens.Count; i++)
        {
            var token = formatTokens[i];
            if (token.ParameterNumber == 0)
            {
                if (param != null)
                {
                    param.ToString(stsa!);
                }
                else
                {
                    sb.Append("null");
                }
                formatBuilder.ReplaceTokenWith(token, sb);
            }
        }
        return this;
    }

    internal FLogAdditionalFormatterParameterEntry? ToAdditionalFormatBuilder<T>(T paramValue, string callMemberName)
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
                                       .Initialize(formatBuilder, formatTokens, loggingLocation, onComplete, stsa!);
        stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    internal FLogStringAppender ToStringAppender<T>(T paramValue, string callMemberName)
    {
        if (formatTokens.Any())
        {
            warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(loggingLocation)
                    .Append("] has remaining tokens after with the first parameter {")
                    .Append(0)
                    .Append("} and converting to StringAppender");
        }

        var formattedStringSoFar = formatBuilder.BackingStringBuilder;

        if (warnings.Length > 0)
        {
            formattedStringSoFar.InsertAt(warnings);
        }

        var styleTypeStringAppender = (Recycler?.Borrow<StyledTypeStringAppender>()  ?? new StyledTypeStringAppender(stsa!.Style))
            .Initialize(formattedStringSoFar, stsa!.Style);

        var addParamsBuilder = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
                                       .Initialize(styleTypeStringAppender, onComplete);
        stsa.DecrementRefCount();
        stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    internal void EnsureNoMoreTokensAndComplete<T>(T paramValue, string callMemberName)
    {
        if (formatTokens.Any())
        {
            warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(loggingLocation)
                    .Append("] still has more tokens after replacing with the first parameter");
            CallOnComplete();
            return;
        }

        CallOnComplete();
        DecrementRefCount();
    }

    protected void CallOnComplete()
    {
        onComplete(warnings.Length > 0 ? warnings : null);
        DecrementRefCount();
    }

    public override void StateReset()
    {
        stsa?.DecrementRefCount();
        stsa = null;
        sb   = null!;
        warnings.Clear();
        onComplete      = null!;
        loggingLocation = default;
        formatBuilder   = null!;
        formatTokens    = null!;

        base.StateReset();
    }

    public override IFLogFirstFormatterParameterEntry Clone() =>
        Recycler?.Borrow<FLogFirstFormatterParameterEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogFirstFormatterParameterEntry(this);

    public override IFLogFirstFormatterParameterEntry CopyFrom
        (IFLogFirstFormatterParameterEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        stsa.CopyFrom(source.BackingStyledTypeStringAppender);
        sb = stsa.WriteBuffer;
        if (source is FLogFirstFormatterParameterEntry firstFormatterParameter)
        {
            warnings.Clear();
            warnings.AppendRange(firstFormatterParameter.warnings);
            onComplete      = firstFormatterParameter.onComplete;
            loggingLocation = firstFormatterParameter.loggingLocation;
            formatBuilder   = firstFormatterParameter.formatBuilder;
            formatTokens    = firstFormatterParameter.formatTokens;
        }
        return this;
    }
}

public static class FLogFirstFormatterParameterEntryExtensions
{
    public static FLogFirstFormatterParameterEntry? ReplaceTokens<T>(this FLogFirstFormatterParameterEntry? maybeParam, T paramValue)
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static FLogFirstFormatterParameterEntry? ReplaceTokens
        (this FLogFirstFormatterParameterEntry? maybeParam, IStyledToStringObject? paramValue)
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static FLogFirstFormatterParameterEntry? ReplaceTokens
        (this FLogFirstFormatterParameterEntry? maybeParam, string? paramValue, int startIndex, int length)
    {
        return maybeParam?.ReplaceTokenNumber(paramValue, startIndex, length);
    }

    public static void EnsureNoMoreTokensAndComplete<T>
        (this FLogFirstFormatterParameterEntry? maybeParam, T paramValue, [CallerMemberName] string callMemberName = "")
    {
        maybeParam?.EnsureNoMoreTokensAndComplete(paramValue, callMemberName);
    }

    public static FLogAdditionalFormatterParameterEntry? ToAdditionalFormatBuilder<T>
        (this FLogFirstFormatterParameterEntry? maybeParam, T paramValue, [CallerMemberName] string callMemberName = "")
    {
        return maybeParam?.ToAdditionalFormatBuilder(paramValue, callMemberName);
    }

    public static FLogStringAppender ToStringAppender<T>
        (this FLogFirstFormatterParameterEntry? maybeParam, T paramValue, FLogFirstFormatterParameterEntry ensureRef, [CallerMemberName] string callMemberName = "")
    {
        return ensureRef.ToStringAppender(paramValue, callMemberName);
    }
}
