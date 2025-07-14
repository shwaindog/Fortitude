// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogEntry : IReusableObject<IFLogEntry>, IMaybeFrozen
{
    uint IssueSequenceNumber { get; }

    uint InstanceNumber      { get; }

    DateTime LogDateTime { get; }

    LoggingLocation LogLocation { get; }

    IFLogger  Logger   { get; }

    FLogLevel LogLevel { get; }

    Thread Thread { get; }

    StringBuildingStyle Style { get; }

    IFrozenString Message { get; }
}

public interface IMutableFLogEntry : IFLogEntry, IFreezable<IFLogEntry>
{
    [MustUseReturnValue("Use WithOnlyParam or AndFinalParam to complete LogEntry")]
    IFLogFirstFormatterParameterEntry? FormatBuilder(string formattedString, StringBuildingStyle style = StringBuildingStyle.Default);


    [MustUseReturnValue("Use FinalAppend to complete LogEntry")]
    IFLogStringAppender StringAppender(StringBuildingStyle style = StringBuildingStyle.Default);

    new IMutableString Message { get; }
}

public record struct LoggerEntryContext(IFLogger Logger, ForwardLogEntry OnCompleteHandler, LoggingLocation LogLocation, FLogLevel LogLevel);

public class FLogEntry : ReusableObject<IFLogEntry>, IMutableFLogEntry
{
    internal static readonly LoggingLocation NotSetLocation = new("NotSet", "NotSet", 0);

    private MutableString?         messageBuilder;
    private readonly Action<StringBuilder?> onComplete;

    private ForwardLogEntry dispatchHandler = null!;

    private static uint totalInstanceCount;

    private DateTime? loggerDispatchedAt;

    public FLogEntry()
    {
        InstanceNumber = Interlocked.Increment(ref totalInstanceCount);
        onComplete     = OnComplete;
    }

    public FLogEntry(MutableString messageBuilder) : this()
    {
        this.messageBuilder = messageBuilder;
    }

    public FLogEntry(IFLogEntry toClone) : this()
    {
        messageBuilder = (MutableString)toClone.Message.SourceThawed;

        LogDateTime = toClone.LogDateTime;
        LogLocation = NotSetLocation;
        onComplete = OnComplete;
    }

    internal FLogEntry Initialize(LoggerEntryContext loggerEntryContext)
    {
        messageBuilder  ??= new MutableString();

        dispatchHandler =   loggerEntryContext.OnCompleteHandler;
        LogLocation     =   loggerEntryContext.LogLocation;
        Logger          =   loggerEntryContext.Logger;
        LogLevel        =   loggerEntryContext.LogLevel;

        return this;
    }

    private void OnComplete(StringBuilder? warningToPrefix)
    {
        if (loggerDispatchedAt == null)
        {
            if (warningToPrefix != null)
            {
                messageBuilder!.Insert(0, warningToPrefix);
            }
            loggerDispatchedAt = TimeContext.UtcNow;
            dispatchHandler.Invoke(Freeze);
        }
        else
        {
            Console.Out.WriteLine($"Attempted to dispatch {this} this entry twice!!!");
        }
    }

    public bool LoggerHasDispatched => loggerDispatchedAt != null;

    public IFLogStringAppender StringAppender(StringBuildingStyle style = StringBuildingStyle.Default)
    {
        Thread      = Thread.CurrentThread;
        LogDateTime = TimeContext.UtcNow;
        Style       = style;

        var styleTypeStringAppender = (Recycler?.Borrow<WrappingStyledTypeStringAppender>() ?? new WrappingStyledTypeStringAppender(style))
            .Initialize(messageBuilder!.BackingStringBuilder, style);
        var stringAppender = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(styleTypeStringAppender, onComplete);

        return stringAppender;
    }

    public IFLogFirstFormatterParameterEntry FormatBuilder(string formattedString, StringBuildingStyle style = StringBuildingStyle.Default)
    {
        Thread      = Thread.CurrentThread;
        LogDateTime = TimeContext.UtcNow;
        Style       = style;

        var formatterBuilder = (Recycler?.Borrow<FormatBuilder>() ?? new FormatBuilder())
            .Initialize(formattedString, messageBuilder!.BackingStringBuilder);


        var formatAppender = (Recycler?.Borrow<FLogFirstFormatterParameterEntry>() ?? new FLogFirstFormatterParameterEntry())
            .Initialize(formatterBuilder, LogLocation, onComplete, style);

        return formatAppender;
    }

    public uint IssueSequenceNumber { get; set; }

    public uint InstanceNumber { get; }

    public bool IsFrozen => messageBuilder!.IsFrozen;

    IMaybeFrozen IFreezable.Freeze   => Freeze;

    public IFLogEntry Freeze 
    {
        get
        {
            _ = messageBuilder!.Freeze;
            return this;
        }
    }

    IFrozenString IFLogEntry.Message => messageBuilder!.Freeze;

    public IMutableString Message  => !IsFrozen ? messageBuilder! :throw new ModifyFrozenObjectAttempt("Attempted to modify a frozen FLogEntry");

    public bool ThrowOnMutateAttempt
    {
        get => messageBuilder!.ThrowOnMutateAttempt;
        set => messageBuilder!.ThrowOnMutateAttempt = value;
    }

    public DateTime        LogDateTime { get; private set; }

    public LoggingLocation LogLocation { get; private set; }

    public IFLogger Logger { get; private set; } = null!;

    public FLogLevel LogLevel { get; private set; }

    public Thread Thread { get; private set; } = null!;

    public StringBuildingStyle Style { get; private set; }

    public override void StateReset()
    {
        messageBuilder!.StateReset();
        LogDateTime = DateTime.MinValue;
        Style       = StringBuildingStyle.Default;
        LogLocation = NotSetLocation;
        Logger      = null!;
        Thread      = null!;
        base.StateReset();
    }

    public override IFLogEntry Clone() => Recycler?.Borrow<FLogEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogEntry(this);

    public override FLogEntry CopyFrom(IFLogEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        messageBuilder!.Clear();
        messageBuilder!.Append(source.Message);

        Logger      = source.Logger;
        Thread      = source.Thread;
        Style       = source.Style;

        LogDateTime = source.LogDateTime;
        LogLocation = source.LogLocation;

        return this;
    }

    protected string LogEntryToStringMembers =>
        $"{nameof(messageBuilder)}: {messageBuilder}, {nameof(loggerDispatchedAt)}: {loggerDispatchedAt}, {nameof(IssueSequenceNumber)}: {IssueSequenceNumber}, " +
        $"{nameof(InstanceNumber)}: {InstanceNumber}, {nameof(LogDateTime)}: {LogDateTime}, {nameof(LogLocation)}: {LogLocation}";

    public override string ToString() => 
        $"{nameof(FLogEntry)}{{{LogEntryToStringMembers}}}";
}
