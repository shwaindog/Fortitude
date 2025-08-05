using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries;


public interface IFLogFormatterParameterEntry : IReusableObject<IFLogFormatterParameterEntry>
{
    // ReSharper disable UnusedMember.Global
    int RemainingArguments { get; }

    IStyledTypeStringAppender BackingStyledTypeStringAppender { get; }

    IStringBuilder BackingStringBuilder { get; }
}

public class FormatParameterEntry<TFormatEntry> : ReusableObject<IFLogFormatterParameterEntry>, IFLogFormatterParameterEntry
where TFormatEntry : FormatParameterEntry<TFormatEntry>
{
    protected IStyledTypeStringAppender? stsa;

    protected IStringBuilder sb = null!;

    protected readonly IStringBuilder warnings = new MutableString();

    protected Action<IStringBuilder?> onComplete = null!;

    protected LoggingLocation loggingLocation;

    protected FormatBuilder formatBuilder = null!;

    protected List<StringFormatTokenParams> formatTokens = null!;

    protected int currentParamNum = 1;

    public FormatParameterEntry() { }

    public FormatParameterEntry(FormatParameterEntry<TFormatEntry> toClone)
    {
        stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        stsa.CopyFrom(toClone.BackingStyledTypeStringAppender);
        sb = stsa.WriteBuffer;
        warnings.Clear();
        warnings.AppendRange(toClone.warnings);
        onComplete      = toClone.onComplete;
        loggingLocation = toClone.loggingLocation;
        formatBuilder   = toClone.formatBuilder;
        formatTokens    = toClone.formatTokens.ToList();
    }
    
    public TFormatEntry Initialize
    (FormatBuilder stringFormatBuilder, int paramNum
      , LoggingLocation logLocation, Action<IStringBuilder?> onCompleteHandler
      , StringBuildingStyle style, IStyledTypeStringAppender? styledTypeStringAppender = null, List<StringFormatTokenParams>? remainingTokens = null)
    {
        stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender()).Initialize(style);
        sb   = stsa.WriteBuffer;

        currentParamNum = paramNum;

        onComplete = onCompleteHandler;

        loggingLocation = logLocation;
        formatBuilder   = stringFormatBuilder;
        formatTokens    = formatBuilder.RemainingTokens();

        return (TFormatEntry)this;
    }
    
    public IStringBuilder BackingStringBuilder => sb;
    
    public IStyledTypeStringAppender BackingStyledTypeStringAppender
    {
        get
        {
            stsa ??= Recycler?.Borrow<StyledTypeStringAppender>().Initialize() ?? new StyledTypeStringAppender();
            sb   =   stsa.WriteBuffer;
            return stsa;
        }
    }

    public int RemainingArguments => formatTokens.Count;

    protected TFormatEntry? PreCheckTokensGetStringBuilder<T>(T param, [CallerMemberName] string memberName = "")
    {
        if (formatTokens.Any()) return (TFormatEntry)this;
        warnings.Append(memberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(param).Append(") at [").Append(loggingLocation)
                .Append("] no formatting tokens remaining");
        return null;
    }


    protected TFormatEntry? PreCheckTokensGetStringBuilder(Span<char> param, [CallerMemberName] string memberName = "")
    {
        if (formatTokens.Any()) return (TFormatEntry)this;
        warnings.Append(memberName).Append("(").Append(nameof(Span<char>)).Append(" ")
                .Append(param).Append(") at [").Append(loggingLocation)
                .Append("] no formatting tokens remaining");
        return null;
    }

    protected TFormatEntry? PreCheckTokensGetStringBuilder(ReadOnlySpan<char> param
      , [CallerMemberName] string memberName = "")
    {
        if (formatTokens.Any()) return (TFormatEntry)this;
        warnings.Append(memberName).Append("(").Append(nameof(ReadOnlySpan<char>)).Append(" ")
                .Append(param).Append(") at [").Append(loggingLocation)
                .Append("] no formatting tokens remaining");
        return null;
    }

    internal TFormatEntry ReplaceTokenNumber(StringBuilder? param)
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
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(StringBuilder? param, int fromIndex, int count)
    {
        if (param == null)
        {
            return ReplaceTokenNumber("");
        }
        var cappedFrom   = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo     = Math.Clamp(fromIndex + count, 0, param.Length);
        var cappedLength = cappedFrom - cappedTo;
        if (cappedLength <= 0)
        {
            return ReplaceTokenNumber("");
        }
        if (cappedFrom == 0 && cappedTo == param.Length) return ReplaceTokenNumber(param);
        if (cappedLength < 1024)
        {
            var onStack = stackalloc char[cappedLength].ResetMemory();
            for (int i = 0; i < cappedLength; i++)
            {
                onStack[i] = param[cappedFrom + i];
            }
            return ReplaceTokenNumber(onStack);
        }
        var recyclingCharArray = cappedLength.SourceRecyclingCharArray();
        recyclingCharArray.Add(param, fromIndex, count);
        var arraySpan = recyclingCharArray.AsSpan();
        ReplaceTokenNumber(arraySpan);
        recyclingCharArray.DecrementRefCount();
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(ICharSequence? param)
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
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(ICharSequence? param, int fromIndex, int count)
    {
        if (param == null)
        {
            return ReplaceTokenNumber("");
        }
        var cappedFrom   = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo     = Math.Clamp(fromIndex + count, 0, param.Length);
        var cappedLength = cappedFrom - cappedTo;
        if (cappedLength <= 0)
        {
            return ReplaceTokenNumber("");
        }
        if (cappedFrom == 0 && cappedTo == param.Length) return ReplaceTokenNumber(param);
        if (cappedLength < 1024)
        {
            var onStack = stackalloc char[cappedLength].ResetMemory();
            for (int i = 0; i < cappedLength; i++)
            {
                onStack[i] = param[cappedFrom + i];
            }
            return ReplaceTokenNumber(onStack);
        }
        var recyclingCharArray = cappedLength.SourceRecyclingCharArray();
        recyclingCharArray.Add(param, fromIndex, count);
        var arraySpan = recyclingCharArray.AsSpan();
        ReplaceTokenNumber(arraySpan);
        recyclingCharArray.DecrementRefCount();
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(char[] param)
    {
        var asReadOnly = (ReadOnlySpan<char>)param;
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntry ReplaceTokenNumber(char[]? param, int fromIndex, int count)
    {
        if (param == null)
        {
            return ReplaceTokenNumber("");
        }
        var cappedFrom = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo   = Math.Clamp(fromIndex + count, 0, param.Length);

        var asReadOnly = (ReadOnlySpan<char>)param[cappedFrom..cappedTo];
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntry ReplaceTokenNumber(Span<char> param)
    {
        var asReadOnly = (ReadOnlySpan<char>)param;
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntry ReplaceTokenNumber(Span<char> param, int fromIndex, int count)
    {
        var cappedFrom = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo   = Math.Clamp(fromIndex + count, 0, param.Length);

        var asReadOnly = (ReadOnlySpan<char>)param[cappedFrom..cappedTo];
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntry ReplaceTokenNumber(ReadOnlySpan<char> param)
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
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(ReadOnlySpan<char> param, int fromIndex, int count)
    {
        var cappedFrom = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo   = Math.Clamp(fromIndex + count, 0, param.Length);

        var asReadOnly = param[cappedFrom..cappedTo];
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntry ReplaceTokenNumber<T>(T? param)
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
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber<TStruct>(TStruct? param, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        if (param == null)
        {
            return ReplaceTokenNumber("");
        }
        sb.Clear();
        structStyler(param.Value, stsa!);
        for (var i = 0; i < formatTokens.Count; i++)
        {
            var token = formatTokens[i];
            if (token.ParameterNumber == currentParamNum)
            {
                formatBuilder.ReplaceTokenWith(token, sb);
            }
        }
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(IStyledToStringObject? param)
    {
        sb.Clear();
        param?.ToString(stsa!);
        for (var i = 0; i < formatTokens.Count; i++)
        {
            var token = formatTokens[i];
            if (token.ParameterNumber == currentParamNum)
            {
                formatBuilder.ReplaceTokenWith(token, sb);
            }
        }
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(string param, int fromIndex, int count)
    {
        var cappedFrom = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo   = Math.Clamp(fromIndex + count, 0, param.Length);

        var asReadOnly = (ReadOnlySpan<char>)param[cappedFrom..cappedTo];
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntry? ExpectContinue<T>(T paramValue, string callMemberName)
    {
        if (!formatTokens.Any())
        {
            warnings
                .Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(paramValue).Append(") at [").Append(loggingLocation)
                .Append("] has no more remaining tokens after replacing parameter {")
                .Append(currentParamNum).Append("}");
            CallOnComplete();
            return null;
        }
        currentParamNum++;
        return (TFormatEntry)this;
    }

    internal FLogStringAppender ToStringAppender<T>(T paramValue, string callMemberName)
    {
        if (formatTokens.Any())
        {
            warnings
                .Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(paramValue).Append(") at [").Append(loggingLocation)
                .Append("] has remaining tokens after replacing parameter {")
                .Append(currentParamNum).Append("} and converting to StringAppender");
        }

        var formattedStringSoFar = formatBuilder.BackingStringBuilder;

        if (warnings.Length > 0)
        {
            formattedStringSoFar.InsertAt(warnings);
        }

        var styleTypeStringAppender = (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(stsa!.Style))
            .Initialize(formattedStringSoFar, stsa!.Style);

        var addParamsBuilder = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(styleTypeStringAppender, onComplete);
        stsa.DecrementRefCount();
        stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }


    internal FLogStringAppender ToStringAppender(ReadOnlySpan<char> paramValue, string callMemberName)
    {
        if (formatTokens.Any())
        {
            warnings.Append(callMemberName).Append("(").Append(nameof(ReadOnlySpan<char>)).Append(" ")
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

        var styleTypeStringAppender = (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(stsa!.Style))
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

    public override FormatParameterEntry<TFormatEntry> Clone() => 
        Recycler?.Borrow<FormatParameterEntry<TFormatEntry>>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FormatParameterEntry<TFormatEntry>(this);

    public override TFormatEntry CopyFrom(IFLogFormatterParameterEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => 
        CopyFrom((TFormatEntry)source, copyMergeFlags);

    public TFormatEntry CopyFrom(TFormatEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        stsa.CopyFrom(source.BackingStyledTypeStringAppender);
        sb = stsa.WriteBuffer;
        warnings.Clear();
        warnings.AppendRange(source.warnings);
        onComplete      = source.onComplete;
        loggingLocation = source.loggingLocation;
        formatBuilder   = source.formatBuilder;
        formatTokens    = source.formatTokens;

        return (TFormatEntry)this;
    }
}


public static class FLogAdditionalFormatterParameterEntryExtensions
{
    public static TFormatEntry? ReplaceCharSpanTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , ReadOnlySpan<char> paramValue) where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static TFormatEntry? ReplaceCharSpanTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , Span<char> paramValue) where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry, T>(this TFormatEntry? maybeParam, T paramValue)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry, TStruct>(this TFormatEntry? maybeParam
      , TStruct? paramValue, StructStyler<TStruct> structStyler)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
        where TStruct : struct
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , char[]? paramValue, int fromIndex, int count)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , ICharSequence? paramValue, int fromIndex, int count)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , ReadOnlySpan<char> paramValue, int fromIndex, int count)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , Span<char> paramValue, int fromIndex, int count)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , StringBuilder? paramValue, int fromIndex, int count)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , IStyledToStringObject? paramValue)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue);
    }

    public static TFormatEntry? ReplaceTokens<TFormatEntry>(this TFormatEntry? maybeParam
      , string? paramValue, int startIndex, int length)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ReplaceTokenNumber(paramValue ?? "", startIndex, length);
    }

    public static void EnsureNoMoreTokensAndComplete<TFormatEntry>(this TFormatEntry? maybeParam
      , ReadOnlySpan<char> paramValue, [CallerMemberName] string callMemberName = "")
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        maybeParam?.EnsureNoMoreTokensAndComplete(paramValue, callMemberName);
    }

    public static void EnsureNoMoreTokensAndComplete<TFormatEntry, T>(this TFormatEntry? maybeParam, T paramValue
      , [CallerMemberName] string callMemberName = "")
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        maybeParam?.EnsureNoMoreTokensAndComplete(paramValue, callMemberName);
    }

    public static TFormatEntry? ExpectContinue<TFormatEntry>(this TFormatEntry? maybeParam
      , ReadOnlySpan<char> paramValue, [CallerMemberName] string callMemberName = "")
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ExpectContinue(paramValue, callMemberName);
    }

    public static TFormatEntry? ExpectContinue<TFormatEntry, T>(this TFormatEntry? maybeParam, T paramValue
      , [CallerMemberName] string callMemberName = "")
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return maybeParam?.ExpectContinue(paramValue, callMemberName);
    }

    public static FLogStringAppender ToStringAppender<TFormatEntry, T>(this TFormatEntry? maybeParam
      , T paramValue, TFormatEntry ensureRef, [CallerMemberName] string callMemberName = "")
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return ensureRef.ToStringAppender(paramValue, callMemberName);
    }

    public static FLogStringAppender ToStringAppender<TFormatEntry>(this StyledTypeBuildResult? stb, FLogStringAppender fsa)
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return fsa;
    }

    public static FLogStringAppender ToStringAppender<TFormatEntry>(this TFormatEntry? maybeParam
      , ReadOnlySpan<char> paramValue, TFormatEntry ensureRef, [CallerMemberName] string callMemberName = "")
        where TFormatEntry : FormatParameterEntry<TFormatEntry>
    {
        return ensureRef.ToStringAppender(paramValue, callMemberName);
    }
}
