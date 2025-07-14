// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogAdditionalFormatterParameterEntry : IReusableObject<IFLogAdditionalFormatterParameterEntry>
{
    // ReSharper disable UnusedMember.Global
    int RemainingArguments { get; }

    IStyledTypeStringAppender BackingStyledTypeStringAppender { get; }

    StringBuilder BackingStringBuilder { get; }

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(IMutableString? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(bool? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(sbyte? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(byte? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(char? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(short? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ushort? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(int? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(uint? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(float? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(long? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ulong? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(double? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(decimal? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(object? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(char[]? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(string? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(string? value, int startIndex, int length);

    void AndFinalParam(IMutableString? value);
    void AndFinalParam(IStyledToStringObject? value);
    void AndFinalParam(bool? value);
    void AndFinalParam(sbyte? value);
    void AndFinalParam(byte? value);
    void AndFinalParam(char? value);
    void AndFinalParam(short? value);
    void AndFinalParam(ushort? value);
    void AndFinalParam(int? value);
    void AndFinalParam(uint? value);
    void AndFinalParam(float? value);
    void AndFinalParam(long? value);
    void AndFinalParam(ulong? value);
    void AndFinalParam(double? value);
    void AndFinalParam(decimal? value);
    void AndFinalParam(object? value);
    void AndFinalParam(char[]? value);
    void AndFinalParam(string? value);
    void AndFinalParam(string? value, int startIndex, int length);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(IMutableString? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(IStyledToStringObject? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(bool? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(sbyte? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(byte? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(char? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(short? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ushort? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(int? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(uint? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(float? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(long? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ulong? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(double? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(decimal? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(object? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(char[]? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(string? value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(string? value, int startIndex, int length);
    

    // ReSharper restore UnusedMember.Global
}

public class FLogAdditionalFormatterParameterEntry : ReusableObject<IFLogAdditionalFormatterParameterEntry>, IFLogAdditionalFormatterParameterEntry
{
    private IStyledTypeStringAppender? stsa;

    private StringBuilder sb = null!;

    private readonly StringBuilder warnings = new();

    private Action<StringBuilder?> onComplete = null!;

    private LoggingLocation loggingLocation;

    private FormatBuilder formatBuilder = null!;

    private List<StringFormatTokenParams> formatTokens = null!;
    private int currentParamNum = 1;

    public FLogAdditionalFormatterParameterEntry() { }

    public FLogAdditionalFormatterParameterEntry(IFLogAdditionalFormatterParameterEntry toClone)
    {
        stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        stsa.CopyFrom(toClone.BackingStyledTypeStringAppender);
        sb = stsa.BackingStringBuilder;
        if (toClone is FLogAdditionalFormatterParameterEntry addFormatterParameter)
        {
            warnings.Clear();
            warnings.AppendRange(addFormatterParameter.warnings);
            onComplete      = addFormatterParameter.onComplete;
            loggingLocation = addFormatterParameter.loggingLocation;
            formatBuilder   = addFormatterParameter.formatBuilder;
            formatTokens    = addFormatterParameter.formatTokens;
        }
    }

    public FLogAdditionalFormatterParameterEntry Initialize
    (FormatBuilder stringFormatBuilder
      , List<StringFormatTokenParams> remainingTokens
      , LoggingLocation logLocation
      , Action<StringBuilder?> onCompleteHandler
      , IStyledTypeStringAppender styledTypeStringAppender)
    {
        stsa = styledTypeStringAppender;
        sb   = stsa.BackingStringBuilder;

        currentParamNum = 1;

        onComplete = onCompleteHandler;

        loggingLocation = logLocation;
        formatBuilder   = stringFormatBuilder;
        formatTokens    = formatBuilder.RemainingTokens();

        return this;
    }

    public int RemainingArguments => formatTokens.Count;

    public StringBuilder BackingStringBuilder => sb;

    public IStyledTypeStringAppender BackingStyledTypeStringAppender => stsa!;
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(string? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(string? value, int startIndex, int length) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, length).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(char[]? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(object? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(decimal? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(double? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ulong? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(float? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(uint? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(int? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(long? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(short? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(char? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(byte? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(sbyte? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(bool? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(IMutableString? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? And(ushort? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ExpectContinue(value);

    public void AndFinalParam(float? value) =>
    PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(long? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ulong? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(object? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(decimal? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(char[]? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(uint? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(double? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(int? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(string? value, int startIndex, int length) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value, startIndex, length).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(short? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(char? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(byte? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(sbyte? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(bool? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(IStyledToStringObject? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(IMutableString? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(string? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);

    public void AndFinalParam(ushort? value) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).EnsureNoMoreTokensAndComplete(value);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(IMutableString? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(char[]? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(object? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(decimal? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(double? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(ulong? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(long? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(float? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(string? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(uint? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(ushort? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(short? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(char? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(byte? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(sbyte? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(bool? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(IStyledToStringObject? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(int? value) => 
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);
    
    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterFinalParamToStringAppender(string? value, int startIndex, int length) =>
        PreCheckTokensGetStringBuilder(value).ReplaceTokens(value).ToStringAppender(value, this);


    protected FLogAdditionalFormatterParameterEntry? PreCheckTokensGetStringBuilder<T>(T param, [CallerMemberName] string memberName = "")
    {
        if (formatTokens.Any()) return this;
        warnings.Append(memberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(param).Append(") at [").Append(loggingLocation)
                .Append("] no formatting tokens remaining");
        return null;
    }

    internal FLogAdditionalFormatterParameterEntry ReplaceTokenNumber<T>(T? param)
    {
        for (var i = 0; i < formatTokens.Count; i++)
        {
            var token = formatTokens[i];
            if (token.ParameterNumber == currentParamNum)
            {
                sb.Clear();
                sb.AppendFormat(token.StringFormat, param);
                formatBuilder.ReplaceTokenWith(token, sb);
            }
        }
        return this;
    }

    internal FLogAdditionalFormatterParameterEntry ReplaceTokenNumber(IStyledToStringObject? param)
    {
        for (var i = 0; i < formatTokens.Count; i++)
        {
            var token = formatTokens[i];
            if (token.ParameterNumber == currentParamNum)
            {
                sb.Clear();
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

    internal FLogAdditionalFormatterParameterEntry ReplaceTokenNumber(string? value, int startIndex, int length)
    {
        for (var i = 0; i < formatTokens.Count; i++)
        {
            var token = formatTokens[i];
            if (token.ParameterNumber == currentParamNum)
            {
                sb.Clear();
                sb.Append(value, startIndex, length );
                formatBuilder.ReplaceTokenWith(token, sb);
            }
        }
        return this;
    }

    internal FLogAdditionalFormatterParameterEntry? ExpectContinue<T>(T paramValue, string callMemberName)
    {
        if (!formatTokens.Any())
        {
            warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(loggingLocation)
                    .Append("] has no more remaining tokens after replacing parameter {")
                    .Append(currentParamNum)
                    .Append("}");
            CallOnComplete();
            return null;
        }
        currentParamNum++;
        return this;
    }

    internal FLogStringAppender ToStringAppender<T>(T paramValue, string callMemberName)
    {
        if (formatTokens.Any())
        {
            warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(loggingLocation)
                    .Append("] has remaining tokens after replacing parameter {")
                    .Append(currentParamNum)
                    .Append("} and converting to StringAppender");
        }

        var formattedStringSoFar = formatBuilder.BackingStringBuilder;

        if (warnings.Length > 0)
        {
            formattedStringSoFar.InsertAt(warnings);
        }

        var styleTypeStringAppender = (Recycler?.Borrow<WrappingStyledTypeStringAppender>()  ?? new WrappingStyledTypeStringAppender(stsa!.Style))
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

    public override FLogAdditionalFormatterParameterEntry Clone() =>
        Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new FLogAdditionalFormatterParameterEntry(this);

    public override FLogAdditionalFormatterParameterEntry CopyFrom
        (IFLogAdditionalFormatterParameterEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        stsa.CopyFrom(source.BackingStyledTypeStringAppender);
        sb = stsa.BackingStringBuilder;
        if (source is FLogAdditionalFormatterParameterEntry addFormatterParameter)
        {
            warnings.Clear();
            warnings.AppendRange(addFormatterParameter.warnings);
            onComplete      = addFormatterParameter.onComplete;
            loggingLocation = addFormatterParameter.loggingLocation;
            formatBuilder   = addFormatterParameter.formatBuilder;
            formatTokens    = addFormatterParameter.formatTokens;
        }
        return this;
    }
}

public static class FLogAdditionalFormatterParameterEntryExtensions
{
    public static FLogAdditionalFormatterParameterEntry? ReplaceTokens<T>(this FLogAdditionalFormatterParameterEntry? maybeParam, T paramValue)
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static FLogAdditionalFormatterParameterEntry? ReplaceTokens(this FLogAdditionalFormatterParameterEntry? maybeParam, IStyledToStringObject? paramValue)
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static FLogAdditionalFormatterParameterEntry? ReplaceTokens(this FLogAdditionalFormatterParameterEntry? maybeParam, string? paramValue, int startIndex, int length)
    {
        return maybeParam?.ReplaceTokenNumber(paramValue, startIndex, length);
    }

    public static void EnsureNoMoreTokensAndComplete<T>(this FLogAdditionalFormatterParameterEntry? maybeParam, T paramValue, [CallerMemberName] string callMemberName = "")
    {
        maybeParam?.EnsureNoMoreTokensAndComplete(paramValue, callMemberName);
    }

    public static FLogAdditionalFormatterParameterEntry? ExpectContinue<T>(this FLogAdditionalFormatterParameterEntry? maybeParam, T paramValue, [CallerMemberName] string callMemberName = "")
    {
        return maybeParam?.ExpectContinue(paramValue, callMemberName);
    }

    public static FLogStringAppender ToStringAppender<T>
        (this FLogAdditionalFormatterParameterEntry? maybeParam, T paramValue, FLogAdditionalFormatterParameterEntry ensureRef, [CallerMemberName] string callMemberName = "")
    {
        return ensureRef.ToStringAppender(paramValue, callMemberName);
    }
}