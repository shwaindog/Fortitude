// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using JetBrains.Annotations;

#endregion

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public interface IFLogFormatterParameterEntry : IFLogMessageBuilder
{
    // ReSharper disable UnusedMember.Global
    int RemainingArguments { get; }

    ITheOneString Format { get; }

    IStringBuilder FormatWriteBuffer { get; }
}

public abstract class FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>
    : FLogEntryMessageBuilderBase<TIFormatEntry, TFormatEntryImpl>, IFLogFormatterParameterEntry
    where TFormatEntryImpl : FLogEntryMessageBuilderBase<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
    where TIFormatEntry : class, IFLogMessageBuilder
{
    protected int CurrentParamNum = 1;

    protected FormatBuilder FormatBuilder = null!;

    protected IStringBuilder FormatSb = null!;

    protected ITheOneString? FormatStsa;

    protected List<StringFormatTokenParams> FormatTokens = null!;

    protected bool OnCompleteSwitchesToStringAppender;

    protected FormatParameterEntry() { }

    protected FormatParameterEntry(FormatParameterEntry<TIFormatEntry, TFormatEntryImpl> toClone)
    {
        FormatStsa ??= Recycler?.Borrow<TheOneString>() ?? new TheOneString();
        FormatStsa.CopyFrom(toClone.FormatStsa!);
        FormatSb = FormatStsa.WriteBuffer;
        Warnings.Clear();
        Warnings.AppendRange(toClone.Warnings);
        OnComplete    = toClone.OnComplete;
        FormatBuilder = toClone.FormatBuilder;
        FormatTokens  = toClone.FormatTokens.ToList();
    }

    protected FLogCallLocation FLogCallLocation => LogEntry.LogLocation;

    protected FormatParameterToStringAppender LastParamToStringAppenderCollectionBuilder
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")]
        get
        {
            OnCompleteSwitchesToStringAppender = true;

            NextPostAppendIsLast = true;

            var completeParamCollection =
                Recycler?.Borrow<FinalAppenderCollectionBuilder<TIFormatEntry, TFormatEntryImpl>>() ??
                new FinalAppenderCollectionBuilder<TIFormatEntry, TFormatEntryImpl>();

            completeParamCollection.Initialize(Me, LogEntry);

            var formatToStringAppenderCollBldr =
                (Recycler?.Borrow<FormatParameterToStringAppender>() ?? new FormatParameterToStringAppender())
                .Initialize(LogEntry, OnComplete, completeParamCollection, FormatBuilder.BackingStringBuilder);
            return formatToStringAppenderCollBldr;
        }
    }

    protected IFLogAdditionalParamCollectionAppend CollectionBuilderThenAdditionalParam
    {
        get
        {
            var nextAdditionalParamCollectionBuilder =
                Recycler?.Borrow<AdditionalParamCollectionAppend>() ??
                new AdditionalParamCollectionAppend();

            nextAdditionalParamCollectionBuilder.Initialize(ToAdditionalFormatBuilder(), LogEntry);
            return nextAdditionalParamCollectionBuilder;
        }
    }

    protected TFormatEntryImpl Me => (TFormatEntryImpl)(object)this;

    public int RemainingArguments => FormatTokens.Count;

    public IStringBuilder FormatWriteBuffer => FormatSb;

    public ITheOneString Format
    {
        get
        {
            FormatStsa ??= Recycler?.Borrow<TheOneString>().Initialize() ?? new TheOneString();
            FormatSb   =   FormatStsa.WriteBuffer;
            return FormatStsa;
        }
    }

    public override void StateReset()
    {
        FormatStsa?.DecrementRefCount();
        FormatStsa = null;
        FormatSb   = null!;

        FormatBuilder = null!;
        FormatTokens  = null!;

        base.StateReset();
    }

    protected override ITheOneString? PreappendCheckGetStringAppender<T>(T param, string memberName = "")
    {
        if (!FormatTokens.Any())
        {
            Warnings
                .Append(memberName).Append("(").Append(typeof(T).Name)
                .Append(") at [").Append(FLogCallLocation)
                .Append("] has no more format tokens requiring substitution. > ");
            CallOnComplete();
            return null;
        }
        var tempStsa = Temp;
        tempStsa.ClearAndReinitialize(new StyleOptionsValue(StringStyle.Default));
        return tempStsa;
    }

    protected override TIFormatEntry? PostAppendContinueOnMessageEntry<T>(ITheOneString? justAppended, T param
      , string callMemberName = "")
    {
        if (justAppended == null) return null;
        ReplaceStagingTokenNumber(justAppended.WriteBuffer);
        if (!NextPostAppendIsLast)
        {
            if (!FormatTokens.Any())
            {
                Warnings
                    .Append(callMemberName).Append("(").Append(typeof(T).Name)
                    .Append(") at [").Append(FLogCallLocation)
                    .Append("] has no more remaining tokens after replacing parameter {")
                    .Append(CurrentParamNum).Append("}. > ");
                if (OnCompleteSwitchesToStringAppender)
                {
                    if (Warnings.Length > 0) FormatBuilder.BackingStringBuilder.InsertAt(Warnings);
                    return Me;
                }
                CallOnComplete();
                return null;
            }
            CurrentParamNum++;
            return Me;
        }
        if (OnCompleteSwitchesToStringAppender)
        {
            if (Warnings.Length > 0) FormatBuilder.BackingStringBuilder.InsertAt(Warnings);
            return Me;
        }
        CallOnComplete();
        return null;
    }

    public TFormatEntryImpl Initialize
    (FLogEntry fLogEntry, FormatBuilder stringFormatBuilder, int paramNum, Action<IStringBuilder?> onCompleteHandler
      , ITheOneString? styledTypeStringAppender = null, List<StringFormatTokenParams>? remainingTokens = null)
    {
        Initialize(fLogEntry, onCompleteHandler);
        FormatStsa ??= (Recycler?.Borrow<TheOneString>() ?? new TheOneString()).Initialize(fLogEntry.Style);
        FormatSb   =   FormatStsa.WriteBuffer;

        CurrentParamNum = paramNum;

        FormatBuilder = stringFormatBuilder;
        FormatTokens  = FormatBuilder.RemainingTokens();

        return Me;
    }

    protected TFormatEntryImpl? PreCheckTokensGetStringBuilder<T>(T param, [CallerMemberName] string memberName = "")
    {
        if (FormatTokens.Any()) return Me;
        Warnings.Append(memberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(param).Append(") at [").Append(FLogCallLocation)
                .Append("] no formatting tokens remaining. > ");
        return null;
    }


    protected TFormatEntryImpl? PreCheckTokensGetStringBuilder(Span<char> param, [CallerMemberName] string memberName = "")
    {
        if (FormatTokens.Any()) return Me;
        Warnings.Append(memberName).Append("(").Append(nameof(Span<char>)).Append(" ")
                .Append(param).Append(") at [").Append(FLogCallLocation)
                .Append("] no formatting tokens remaining. > ");
        return null;
    }

    protected TFormatEntryImpl? PreCheckTokensGetStringBuilder(ReadOnlySpan<char> param
      , [CallerMemberName] string memberName = "")
    {
        if (FormatTokens.Any()) return Me;
        Warnings.Append(memberName).Append("(").Append(nameof(ReadOnlySpan<char>)).Append(" ")
                .Append(param).Append(") at [").Append(FLogCallLocation)
                .Append("] no formatting tokens remaining. > ");
        return null;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(bool param)
    {
        FormatSb.Clear();
        FormatSb.Append(param ? "true" : "false");
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(bool? param)
    {
        if (param != null) return ReplaceTokenNumber(param.Value);
        FormatSb.Append("null");
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceSpanFmtTokenNumber<TFmt>(TFmt spanFormattable)
        where TFmt : ISpanFormattable
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                FormatSb.Clear();
                FormatSb.AppendFormat(token.StringFormat, spanFormattable);
                FormatBuilder.ReplaceTokenWith(token, FormatSb);
            }
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceEnumTokenNumber<TEnum>(TEnum spanFormattable)
        where TEnum : Enum
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                FormatSb.Clear();
                FormatSb.AppendFormat(token.StringFormat, spanFormattable);
                FormatBuilder.ReplaceTokenWith(token, FormatSb);
            }
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(StringBuilder? param)
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                FormatSb.Clear();
                FormatSb.AppendFormat(token.StringFormat, param);
                FormatBuilder.ReplaceTokenWith(token, FormatSb);
            }
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(StringBuilder? param, int fromIndex, int count)
    {
        if (param == null) return ReplaceTokenNumber("");
        var cappedFrom   = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo     = Math.Clamp(fromIndex + count, 0, param.Length);
        var cappedLength = cappedTo - cappedFrom;
        if (cappedLength <= 0) return ReplaceTokenNumber("");
        if (cappedFrom == 0 && cappedTo == param.Length) return ReplaceTokenNumber(param);
        if (cappedLength < 1024)
        {
            var onStack                                       = stackalloc char[cappedLength].ResetMemory();
            for (var i = 0; i < cappedLength; i++) onStack[i] = param[cappedFrom + i];
            return ReplaceTokenNumber(onStack);
        }
        var recyclingCharArray = cappedLength.SourceRecyclingCharArray();
        recyclingCharArray.Add(param, fromIndex, count);
        var arraySpan = recyclingCharArray.WrittenAsSpan();
        ReplaceTokenNumber(arraySpan);
        recyclingCharArray.DecrementRefCount();
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber()
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceStagingTokenNumber(ICharSequence? param)
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, param);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(ICharSequence? param, int fromIndex, int count)
    {
        if (param == null) return ReplaceTokenNumber("");
        var cappedFrom   = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo     = Math.Clamp(fromIndex + count, 0, param.Length);
        var cappedLength = cappedTo - cappedFrom;
        if (cappedLength <= 0) return ReplaceTokenNumber("");
        if (cappedFrom == 0 && cappedTo == param.Length) return ReplaceStagingTokenNumber(param);
        if (cappedLength < 1024)
        {
            var onStack                                       = stackalloc char[cappedLength].ResetMemory();
            for (var i = 0; i < cappedLength; i++) onStack[i] = param[cappedFrom + i];
            return ReplaceTokenNumber(onStack);
        }
        FormatSb.Clear();
        FormatSb.Append(param);
        ReplaceStagingTokenNumber(FormatSb);
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(char[]? param)
    {
        FormatSb.Clear();
        FormatSb.Append(param);
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(char[]? param, int fromIndex, int count)
    {
        FormatSb.Clear();
        FormatSb.Append(param, fromIndex, count);
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(Span<char> param)
    {
        var asReadOnly = (ReadOnlySpan<char>)param;
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntryImpl ReplaceTokenNumber(Span<char> param, int fromIndex, int count)
    {
        var cappedFrom = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo   = Math.Clamp(fromIndex + count, 0, param.Length);

        var asReadOnly = (ReadOnlySpan<char>)param[cappedFrom..cappedTo];
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntryImpl ReplaceTokenNumber(ReadOnlySpan<char> param)
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                FormatSb.Clear();
                FormatSb.AppendFormat(token.StringFormat, param);
                FormatBuilder.ReplaceTokenWith(token, FormatSb);
            }
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(ReadOnlySpan<char> param, int fromIndex, int count)
    {
        var cappedFrom = Math.Clamp(fromIndex, 0, param.Length);
        var cappedTo   = Math.Clamp(fromIndex + count, 0, param.Length);

        var asReadOnly = param[cappedFrom..cappedTo];
        return ReplaceTokenNumber(asReadOnly);
    }

    internal TFormatEntryImpl ReplaceMatchTokenNumber<T>(T? param)
    {
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum)
            {
                FormatSb.Clear();
                FormatSb.AppendFormat(token.StringFormat, param);
                FormatBuilder.ReplaceTokenWith(token, FormatSb);
            }
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber<TToStyle, TStylerType>(TToStyle? param, PalantírReveal<TStylerType> palantírReveal)
        where TToStyle : TStylerType
    {
        if (param == null) return ReplaceTokenNumber("");
        FormatSb.Clear();
        palantírReveal(param, FormatStsa!);
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceStyledTokenNumber(IStringBearer? param)
    {
        FormatSb.Clear();
        param?.RevealState(FormatStsa!);
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(string? param)
    {
        FormatSb.Clear();
        FormatSb.Append(param);
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl ReplaceTokenNumber(string? param, int fromIndex, int count)
    {
        FormatSb.Clear();
        FormatSb.Append(param, fromIndex, count);
        for (var i = 0; i < FormatTokens.Count; i++)
        {
            var token = FormatTokens[i];
            if (token.ParameterNumber == CurrentParamNum) FormatBuilder.ReplaceTokenWith(token, FormatSb);
        }
        return Me;
    }

    internal TFormatEntryImpl? ExpectContinue<T>(T paramValue, string callMemberName = "")
    {
        if (!FormatTokens.Any())
        {
            Warnings
                .Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                .Append("] has no more remaining tokens after replacing parameter {")
                .Append(CurrentParamNum).Append("}. > ");
            CallOnComplete();
            return null;
        }
        CurrentParamNum++;
        return Me;
    }

    internal TFormatEntryImpl? ExpectContinue(ReadOnlySpan<char> paramValue, string callMemberName)
    {
        if (!FormatTokens.Any())
        {
            Warnings
                .Append(callMemberName).Append("(").Append("ReadOnlySpan<char>").Append(" ")
                .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                .Append("] has no more remaining tokens after replacing parameter {")
                .Append(CurrentParamNum).Append("}. > ");
            CallOnComplete();
            return null;
        }
        CurrentParamNum++;
        return Me;
    }

    internal FLogStringAppender ToStringAppender<T>(T paramValue, string callMemberName)
    {
        if (FormatTokens.Any())
            Warnings
                .Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                .Append("] has remaining tokens after replacing parameter {")
                .Append(CurrentParamNum).Append("} and converting to StringAppender. > ");

        var formattedStringSoFar = FormatBuilder.BackingStringBuilder;

        if (Warnings.Length > 0) formattedStringSoFar.InsertAt(Warnings);

        var styleTypeStringAppender = (Recycler?.Borrow<TheOneString>() ?? new TheOneString(FormatStsa!.Style))
            .Initialize(formattedStringSoFar, FormatStsa!.Style);

        var addParamsBuilder = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(LogEntry, styleTypeStringAppender, OnComplete);
        FormatStsa.DecrementRefCount();
        FormatStsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }


    internal FLogStringAppender ToStringAppender(ReadOnlySpan<char> paramValue, string callMemberName)
    {
        if (FormatTokens.Any())
            Warnings.Append(callMemberName).Append("(").Append(nameof(ReadOnlySpan<char>)).Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] has remaining tokens after replacing parameter {")
                    .Append(CurrentParamNum)
                    .Append("} and converting to StringAppender. > ");

        var formattedStringSoFar = FormatBuilder.BackingStringBuilder;

        if (Warnings.Length > 0) formattedStringSoFar.InsertAt(Warnings);

        var styleTypeStringAppender = (Recycler?.Borrow<TheOneString>() ?? new TheOneString(FormatStsa!.Style))
            .Initialize(formattedStringSoFar, FormatStsa!.Style);

        var addParamsBuilder = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(LogEntry, styleTypeStringAppender, OnComplete);
        FormatStsa.DecrementRefCount();
        FormatStsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }


    protected abstract FLogAdditionalFormatterParameterEntry ToAdditionalFormatBuilder();

    protected FLogAdditionalFormatterParameterEntry? ToAdditionalFormatBuilder<T>(T paramValue, [CallerMemberName] string callMemberName = "")
    {
        if (!FormatTokens.Any())
        {
            Warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] has no more remaining tokens after replacing with the first parameter")
                    .Append(0)
                    .Append("}. > ");
            CallOnComplete();
            return null;
        }
        var addParamsBuilder = Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>()
                                       .Initialize(LogEntry, FormatBuilder, 1, OnComplete, FormatStsa!, FormatTokens);
        FormatStsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    protected FLogAdditionalFormatterParameterEntry? ToAdditionalFormatBuilder(ReadOnlySpan<char> paramValue
      , [CallerMemberName] string callMemberName = "")
    {
        if (!FormatTokens.Any())
        {
            Warnings.Append(callMemberName).Append("(").Append(typeof(ReadOnlySpan<char>).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] has no more remaining tokens after replacing with the first parameter")
                    .Append(0)
                    .Append("}. > ");
            CallOnComplete();
            return null;
        }
        var addParamsBuilder = Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>()
                                       .Initialize(LogEntry, FormatBuilder, 1, OnComplete, FormatStsa!, FormatTokens);
        FormatStsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }

    internal void EnsureNoMoreTokensAndComplete<T>(T paramValue, string callMemberName)
    {
        if (FormatTokens.Any())
        {
            Warnings.Append(callMemberName).Append("(").Append(typeof(T).Name).Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] still has more tokens after replacing with the first parameter. > ");
            CallOnComplete();
            return;
        }

        CallOnComplete();
        DecrementRefCount();
    }

    internal void EnsureNoMoreTokensAndComplete(ReadOnlySpan<char> paramValue, string callMemberName)
    {
        if (FormatTokens.Any())
        {
            Warnings.Append(callMemberName).Append("(").Append("ReadOnlySpan<char>").Append(" ")
                    .Append(paramValue).Append(") at [").Append(FLogCallLocation)
                    .Append("] still has more tokens after replacing with the first parameter. > ");
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
}

public static class FLogAdditionalFormatterParameterEntryExtensions
{
    public static TFormatEntryImpl? ReplaceCharSpanTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , ReadOnlySpan<char> paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceCharSpanTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , ReadOnlySpan<char> paramValue, int fromIndex, int count)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);

    public static TFormatEntryImpl? ReplaceBoolTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam, bool? paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceSpanFmtTokens<TFormatEntryImpl, TIFormatEntry, T>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam, T paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder
        where T : ISpanFormattable =>
        maybeParam?.ReplaceSpanFmtTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceCustStyleTokens<TFormatEntryImpl, TIFormatEntry, TToStyle, TStylerType>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , TToStyle? paramValue, PalantírReveal<TStylerType> palantírReveal)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder
        where TToStyle : TStylerType =>
        maybeParam?.ReplaceTokenNumber(paramValue, palantírReveal);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , char[]? paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , char[]? paramValue, int fromIndex, int count)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);

    public static TFormatEntryImpl? ReplaceTokensMatch<TFormatEntryImpl, TIFormatEntry, T>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam, T paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceMatchTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , ICharSequence? paramValue, int fromIndex, int count)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , ICharSequence? paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceStagingTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , ReadOnlySpan<char> paramValue, int fromIndex, int count)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , Span<char> paramValue, int fromIndex, int count)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , StringBuilder? paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , StringBuilder? paramValue, int fromIndex, int count)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue, fromIndex, count);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , IStringBearer? paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceStyledTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , string? paramValue)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue);

    public static TFormatEntryImpl? ReplaceTokens<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , string? paramValue, int startIndex, int length)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ReplaceTokenNumber(paramValue ?? "", startIndex, length);

    public static void CallEnsureNoMoreTokensAndComplete<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , ReadOnlySpan<char> paramValue, [CallerMemberName] string callMemberName = "")
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.EnsureNoMoreTokensAndComplete(paramValue, callMemberName);

    public static void CallEnsureNoMoreTokensAndComplete<TFormatEntryImpl, TIFormatEntry, T>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam, T paramValue
      , [CallerMemberName] string callMemberName = "")
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.EnsureNoMoreTokensAndComplete(paramValue, callMemberName);

    public static TFormatEntryImpl? CallExpectContinue<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , ReadOnlySpan<char> paramValue, [CallerMemberName] string callMemberName = "")
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ExpectContinue(paramValue, callMemberName);

    public static TFormatEntryImpl? CallExpectContinue<TFormatEntryImpl, TIFormatEntry, T>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam, T paramValue
      , [CallerMemberName] string callMemberName = "")
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        maybeParam?.ExpectContinue(paramValue, callMemberName);

    public static FLogStringAppender ToStringAppender<TFormatEntryImpl, TIFormatEntry, T>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , T paramValue, TFormatEntryImpl ensureRef, [CallerMemberName] string callMemberName = "")
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        ensureRef.ToStringAppender(paramValue, callMemberName);

    public static FLogStringAppender ToStringAppender<TFormatEntryImpl, TIFormatEntry>(this StateExtractStringRange? stb, FLogStringAppender fsa)
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        fsa;

    public static FLogStringAppender ToStringAppender<TFormatEntryImpl, TIFormatEntry>(
        this FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>? maybeParam
      , ReadOnlySpan<char> paramValue, TFormatEntryImpl ensureRef, [CallerMemberName] string callMemberName = "")
        where TFormatEntryImpl : FormatParameterEntry<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
        where TIFormatEntry : class, IFLogMessageBuilder =>
        ensureRef.ToStringAppender(paramValue, callMemberName);
}
