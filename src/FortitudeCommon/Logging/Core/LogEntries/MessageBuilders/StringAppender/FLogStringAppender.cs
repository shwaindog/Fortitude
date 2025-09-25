// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender : FLogEntryMessageBuilderBase<IFLogStringAppender, FLogStringAppender>, IFLogStringAppender
{
    protected IStringBuilder MessageSb = null!;

    protected ITheOneString MessageStsa = null!;

    protected bool NextPostAppendIsNewLine;

    public FLogStringAppender() { }

    public FLogStringAppender(FLogEntry flogEntry, ITheOneString useStyleTypeTheOneStringBuilder
      , Action<IStringBuilder?> callWhenComplete)
    {
        Initialize(flogEntry, useStyleTypeTheOneStringBuilder, callWhenComplete);
    }

    public FLogStringAppender(IFLogStringAppender toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        ITheOneString? mesgStsa = Recycler?.Borrow<TheOneString>() ?? new TheOneString(toClone.Style);
        mesgStsa.ClearAndReinitialize(new StyleOptionsValue(toClone.Style));
        MessageSb   = mesgStsa.WriteBuffer;
        MessageStsa = mesgStsa;
    }

    public FLogStringAppender Initialize(FLogEntry flogEntry, ITheOneString useStyleTypeTheOneStringBuilder
      , Action<IStringBuilder?> callWhenComplete)
    {
        base.Initialize(flogEntry, callWhenComplete);

        MessageStsa = useStyleTypeTheOneStringBuilder;
        MessageSb   = MessageStsa.WriteBuffer;

        return this;
    }

    public int IndentLevel { get; protected set; }

    public string Indent
    {
        get => MessageStsa.Settings.Indent;
        set => MessageStsa.Settings.Indent = value;
    }

    public int Count => MessageSb.Length;

    public StringStyle Style => MessageStsa.Style;

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

    public IStringAppenderCollectionBuilder AppendCollection
    {
        get
        {
            var stringAppenderCollectionBuilder = Recycler?.Borrow<StringAppenderCollectionBuilder>() ?? new StringAppenderCollectionBuilder();
            stringAppenderCollectionBuilder.Initialize(this, LogEntry);
            return stringAppenderCollectionBuilder;
        }
    }
    public IStringAppenderCollectionBuilder AppendLineCollection
    {
        get
        {
            NextPostAppendIsNewLine = true;
            return AppendCollection;
        }
    }

    public IFinalCollectionAppend FinalAppendCollection
    {
        get
        {
            NextPostAppendIsLast = true;

            var completeParamCollection =
                Recycler?.Borrow<FinalStringAppenderCollectionBuilder>() ??
                new FinalStringAppenderCollectionBuilder();

            completeParamCollection.Initialize(this, LogEntry);

            return completeParamCollection;
        }
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendMatch<T>(T value)
    {
        AppendMatchSelect(value, MessageStsa);
        return this;
    }

    public IFLogStringAppender AppendMatchLine<T>(T value)
    {
        AppendMatchSelect(value, MessageStsa);
        return AppendObjectLine(this);
    }

    public void FinalMatchAppend<T>(T value)
    {
        AppendMatchSelect(value, MessageStsa);
        CallOnComplete();
    }

    protected override ITheOneString? PreappendCheckGetStringAppender<T>(T param, [CallerMemberName] string memberName = "") =>
        MessageStsa;

    protected override IFLogStringAppender? PostAppendContinueOnMessageEntry<T>(ITheOneString? justAppended, T param,
        [CallerMemberName] string memberName = "")
    {
        if (NextPostAppendIsLast) return CallOnComplete();
        if (NextPostAppendIsNewLine)
        {
            NextPostAppendIsNewLine = false;
            return AppendLine();
        }
        return this;
    }

    protected IFLogStringAppender? CallOnComplete()
    {
        OnComplete(null);
        DecrementRefCount();
        return null;
    }
}

public static class FLogStringAppenderExtensions
{
    public static FLogStringAppender AppendLine(this IStringBuilder sb, FLogStringAppender toReturn)
    {
        var style = toReturn.Style;
        if (style.IsCompact()) return toReturn;
        sb.AppendLine();
        if (style.IsPretty())
        {
            var indentLevel  = toReturn.IndentLevel;
            var indentString = toReturn.Indent;
            for (var i = 0; i < indentLevel; i++) sb.Append(indentString);
        }
        return toReturn;
    }

    public static FLogStringAppender AppendLine(this ITheOneString? stsa, FLogStringAppender toReturn) =>
        stsa?.WriteBuffer.AppendLine(toReturn) ?? toReturn;

    public static FLogStringAppender ToAppender(this IStringBuilder _, FLogStringAppender toReturn)             => toReturn;
    public static FLogStringAppender ToAppender(this ITheOneString? _, FLogStringAppender toReturn) => toReturn;
}
