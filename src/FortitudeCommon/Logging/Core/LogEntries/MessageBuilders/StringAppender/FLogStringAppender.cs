// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender : FLogEntryMessageBuilderBase<FLogStringAppender>, IFLogStringAppender
{
    protected IStyledTypeStringAppender MessageStsa = null!;
    protected IStringBuilder            MessageSb = null!;
    
    public FLogStringAppender() { }

    public FLogStringAppender(FLogEntry flogEntry, IStyledTypeStringAppender useStyleTypeStringBuilder
      , Action<IStringBuilder?> callWhenComplete)
    {
        Initialize(flogEntry, useStyleTypeStringBuilder, callWhenComplete);
    }

    public FLogStringAppender(IFLogStringAppender toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        IStyledTypeStringAppender? mesgStsa = Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(toClone.Style);
        mesgStsa.ClearAndReinitialize(toClone.Style);
        MessageSb = mesgStsa.WriteBuffer;
        MessageStsa = mesgStsa;
    }

    public FLogStringAppender Initialize(FLogEntry flogEntry, IStyledTypeStringAppender useStyleTypeStringBuilder
      , Action<IStringBuilder?> callWhenComplete)
    {
        base.Initialize(flogEntry, callWhenComplete);

        MessageStsa = useStyleTypeStringBuilder;
        MessageSb   = MessageStsa.WriteBuffer;

        return this;
    }

    public string Indent
    {
        get => MessageStsa.Indent;
        set => MessageStsa.Indent = value;
    }

    public int Count => MessageSb.Length;

    public int IndentLevel { get; protected set; }

    public StringBuildingStyle Style => MessageStsa.Style;

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
        AppendMatchSelect(value, MessageStsa);
        return this;
    }

    public IFLogStringAppender AppendMatchLine<T>(T value)
    {
        AppendMatchSelect(value, MessageStsa);
        return AppendLine(this);
    }


    public void FinalMatchAppend<T>(T value)
    {
        AppendMatchSelect(value, MessageStsa);
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
        if (source is IFLogStringAppender stringAppender)
        {
            var mesgStsa = (IStyledTypeStringAppender?)MessageStsa;
            mesgStsa ??= Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(stringAppender.Style);
            mesgStsa.ClearAndReinitialize(stringAppender.Style);
            MessageSb   =   mesgStsa.WriteBuffer;
            MessageStsa =   mesgStsa;
        }

        return this;
    }
}

public static class FLogStringAppenderExtensions
{
    public static FLogStringAppender AppendLine(this IStringBuilder sb, FLogStringAppender toReturn)
    {
        var style = toReturn.Style;
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
    
    public static FLogStringAppender AppendLine(this IStyledTypeStringAppender stsa, FLogStringAppender toReturn) => 
        stsa.WriteBuffer.AppendLine(toReturn);

    public static FLogStringAppender ToAppender(this IStringBuilder _, FLogStringAppender toReturn) => toReturn;
    public static FLogStringAppender ToAppender(this IStyledTypeStringAppender _, FLogStringAppender toReturn) => toReturn;
}
