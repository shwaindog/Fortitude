using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogFormatterParameterEntry : IFLogMessageBuilder
{
    // ReSharper disable UnusedMember.Global
    int RemainingArguments { get; }
}

public class FormatParameterEntry<TFormatEntry> : FLogEntryMessageBuilderBase<TFormatEntry>, IFLogFormatterParameterEntry
where TFormatEntry : FormatParameterEntry<TFormatEntry>
{
    protected FormatBuilder FormatBuilder = null!;

    protected List<StringFormatTokenParams> FormatTokens = null!;

    protected int CurrentParamNum = 1;

    public FormatParameterEntry() { }

    public FormatParameterEntry(FormatParameterEntry<TFormatEntry> toClone)
    {
        Stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        Stsa.CopyFrom(toClone.StyledTypeAppender);
        Sb = Stsa.WriteBuffer;
        Warnings.Clear();
        Warnings.AppendRange(toClone.Warnings);
        OnComplete      = toClone.OnComplete;
        FormatBuilder   = toClone.FormatBuilder;
        FormatTokens    = toClone.FormatTokens.ToList();
    }
    
    public TFormatEntry Initialize
    (FLogEntry fLogEntry, FormatBuilder stringFormatBuilder, int paramNum, Action<IStringBuilder?> onCompleteHandler
      , IStyledTypeStringAppender? styledTypeStringAppender = null, List<StringFormatTokenParams>? remainingTokens = null)
    {
        Initialize(fLogEntry, onCompleteHandler, styledTypeStringAppender);

        CurrentParamNum = paramNum;

        FormatBuilder   = stringFormatBuilder;
        FormatTokens    = FormatBuilder.RemainingTokens();

        return (TFormatEntry)this;
    }

    protected FLogCallLocation FLogCallLocation => LogEntry.LogLocation;

    public int RemainingArguments => FormatTokens.Count;

    protected TFormatEntry? PreCheckTokensGetStringBuilder<T>(T param, [CallerMemberName] string memberName = "")
    {
        if (FormatTokens.Any()) return (TFormatEntry)this;
        Warnings.Append(memberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(param).Append(") at [").Append(FLogCallLocation)
                .Append("] no formatting tokens remaining");
        return null;
    }


    protected TFormatEntry? PreCheckTokensGetStringBuilder(Span<char> param, [CallerMemberName] string memberName = "")
    {
        if (FormatTokens.Any()) return (TFormatEntry)this;
        Warnings.Append(memberName).Append("(").Append(nameof(Span<char>)).Append(" ")
                .Append(param).Append(") at [").Append(FLogCallLocation)
                .Append("] no formatting tokens remaining");
        return null;
    }

    protected TFormatEntry? PreCheckTokensGetStringBuilder(ReadOnlySpan<char> param
      , [CallerMemberName] string memberName = "")
    {
        if (FormatTokens.Any()) return (TFormatEntry)this;
        Warnings.Append(memberName).Append("(").Append(nameof(ReadOnlySpan<char>)).Append(" ")
                .Append(param).Append(") at [").Append(FLogCallLocation)
                .Append("] no formatting tokens remaining");
        return null;
    }

    internal TFormatEntry ReplaceTokenNumber(StringBuilder? param)
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                Sb.Clear();
                Sb.AppendFormat(token.StringFormat, param);
                FormatBuilder.ReplaceTokenWith(token, Sb);
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

    internal TFormatEntry ReplaceTokenNumber()
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                FormatBuilder.ReplaceTokenWith(token, Sb);
            }
        }
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(ICharSequence? param)
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                Sb.Clear();
                Sb.AppendFormat(token.StringFormat, param);
                FormatBuilder.ReplaceTokenWith(token, param);
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
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                Sb.Clear();
                Sb.AppendFormat(token.StringFormat, param);
                FormatBuilder.ReplaceTokenWith(token, Sb);
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
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                Sb.Clear();
                Sb.AppendFormat(token.StringFormat, param);
                FormatBuilder.ReplaceTokenWith(token, Sb);
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
        Sb.Clear();
        structStyler(param.Value, Stsa!);
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                FormatBuilder.ReplaceTokenWith(token, Sb);
            }
        }
        return (TFormatEntry)this;
    }

    internal TFormatEntry ReplaceTokenNumber(IStyledToStringObject? param)
    {
        Sb.Clear();
        param?.ToString(Stsa!);
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                FormatBuilder.ReplaceTokenWith(token, Sb);
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
        if (!FormatTokens.Any())
        {
            Warnings
                .Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                .Append("] has no more remaining tokens after replacing parameter {")
                .Append(CurrentParamNum).Append("}");
            CallOnComplete();
            return null;
        }
        CurrentParamNum++;
        return (TFormatEntry)this;
    }

    internal FLogStringAppender ToStringAppender<T>(T paramValue, string callMemberName)
    {
        if (FormatTokens.Any())
        {
            Warnings
                .Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                .Append("] has remaining tokens after replacing parameter {")
                .Append(CurrentParamNum).Append("} and converting to StringAppender");
        }

        var formattedStringSoFar = FormatBuilder.BackingStringBuilder;

        if (Warnings.Length > 0)
        {
            formattedStringSoFar.InsertAt(Warnings);
        }

        var styleTypeStringAppender = (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(Stsa!.Style))
            .Initialize(formattedStringSoFar, Stsa!.Style);

        var addParamsBuilder = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(LogEntry, styleTypeStringAppender, OnComplete);
        Stsa.DecrementRefCount();
        Stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }


    internal FLogStringAppender ToStringAppender(ReadOnlySpan<char> paramValue, string callMemberName)
    {
        if (FormatTokens.Any())
        {
            Warnings.Append(callMemberName).Append("(").Append(nameof(ReadOnlySpan<char>)).Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] has remaining tokens after replacing parameter {")
                    .Append(CurrentParamNum)
                    .Append("} and converting to StringAppender");
        }

        var formattedStringSoFar = FormatBuilder.BackingStringBuilder;

        if (Warnings.Length > 0)
        {
            formattedStringSoFar.InsertAt(Warnings);
        }

        var styleTypeStringAppender = (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(Stsa!.Style))
            .Initialize(formattedStringSoFar, Stsa!.Style);

        var addParamsBuilder = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(LogEntry, styleTypeStringAppender, OnComplete);
        Stsa.DecrementRefCount();
        Stsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    internal void EnsureNoMoreTokensAndComplete<T>(T paramValue, string callMemberName)
    {
        if (FormatTokens.Any())
        {
            Warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] still has more tokens after replacing with the first parameter");
            CallOnComplete();
            return;
        }

        CallOnComplete();
        DecrementRefCount();
    }

    protected void CallOnComplete()
    {
        OnComplete(Warnings.Length > 0 ? Warnings : null);
        DecrementRefCount();
    }

    public override void StateReset()
    {
        FormatBuilder   = null!;
        FormatTokens    = null!;

        base.StateReset();
    }

    public override FormatParameterEntry<TFormatEntry> Clone() => 
        Recycler?.Borrow<FormatParameterEntry<TFormatEntry>>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FormatParameterEntry<TFormatEntry>(this);

    public override TFormatEntry CopyFrom(IFLogMessageBuilder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => 
        CopyFrom((TFormatEntry)source, copyMergeFlags);

    public TFormatEntry CopyFrom(TFormatEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        Stsa.CopyFrom(source.StyledTypeAppender);
        Sb = Stsa.WriteBuffer;
        Warnings.Clear();
        Warnings.AppendRange(source.Warnings);
        OnComplete      = source.OnComplete;
        FormatBuilder   = source.FormatBuilder;
        FormatTokens    = source.FormatTokens;

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
