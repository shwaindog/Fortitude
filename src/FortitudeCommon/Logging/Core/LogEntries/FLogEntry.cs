// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Framework.Fillers;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogEntry : IReusableObject<IFLogEntry>, IMaybeFrozen
{
    event Action<IFLogEntry> MessageComplete;

    uint IssueSequenceNumber    { get; }
    uint ReceivedSequenceNumber { get; }
    uint InstanceNumber         { get; }
    long CorrelationId { get; }



    object? CallerContextObject { get; }

    Exception? Exception { get; }

    StringBuildingStyle Style { get; }

    DateTime        LogDateTime  { get; }
    DateTime?       DispatchedAt { get; }
    LoggingLocation LogLocation  { get; }
    IFLogger        Logger       { get; }
    FLogLevel       LogLevel     { get; }
    Thread          Thread       { get; }
    IFrozenString   Message      { get; }
}

public interface IMutableFLogEntry : IFLogEntry, IFreezable<IFLogEntry>
{
    const string DefaultCorrelationIdKeyName = "CorrelationId";

    new uint ReceivedSequenceNumber { get; set; }

    new FLogLevel LogLevel { get; set; }


    [MustUseReturnValue("Use WithOnlyParam or AndFinalParam to complete LogEntry")]
    IFLogFirstFormatterParameterEntry? FormatBuilder(string formattedString, StringBuildingStyle style = StringBuildingStyle.Default);


    [MustUseReturnValue("Use FinalAppend to complete LogEntry")]
    IFLogStringAppender StringAppender(StringBuildingStyle style = StringBuildingStyle.Default);

    IMutableFLogEntry AddCallerContextObjFromKey(string asyncCallOrThreadKey);
    IMutableFLogEntry WithCallerContextObj(object callerContextObj);
    IMutableFLogEntry AddCorrelationId(string asyncCallOrThreadKey = DefaultCorrelationIdKeyName);
    IMutableFLogEntry WithCorrelationId(long correlationId);

    IMutableFLogEntry AddException(Exception exception);

    void OnMessageComplete(StringBuilder? warningToPrefix);

    new IMutableString Message { get; }
}

public record struct LoggerEntryContext(IFLogger Logger, IFLogEntrySink OnCompleteHandler, LoggingLocation LogLocation, FLogLevel LogLevel);

public class FLogEntry : ReusableObject<IFLogEntry>, IMutableFLogEntry
{
    internal static readonly LoggingLocation NotSetLocation = new("NotSet", "NotSet", 0);

    private MutableString? messageBuilder;

    public event Action<IFLogEntry> MessageComplete;

    private IFLogEntrySink dispatchHandler = null!;

    private static uint totalInstanceCount;

    public DateTime? DispatchedAt { get; protected set; }

    public FLogEntry()
    {
        InstanceNumber = Interlocked.Increment(ref totalInstanceCount);
        MessageComplete     = SendToDispatchHandler;
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
        MessageComplete  = SendToDispatchHandler;
    }

    internal FLogEntry Initialize(LoggerEntryContext loggerEntryContext)
    {
        messageBuilder ??= new MutableString();

        dispatchHandler = loggerEntryContext.OnCompleteHandler;
        LogLocation     = loggerEntryContext.LogLocation;
        Logger          = loggerEntryContext.Logger;
        LogLevel        = loggerEntryContext.LogLevel;

        return this;
    }

    protected virtual void SendToDispatchHandler(IFLogEntry me)
    {
        dispatchHandler.InBoundListener(new LogEntryPublishEvent(me), Logger.PublishEndpoint);
    }

    public void OnMessageComplete(StringBuilder? warningToPrefix)
    {
        if (DispatchedAt == null)
        {
            if (warningToPrefix != null)
            {
                messageBuilder!.Insert(0, warningToPrefix);
            }
            DispatchedAt = TimeContext.UtcNow;
            MessageComplete.Invoke(this);
        }
        else
        {
            Console.Out.WriteLine($"Attempted to dispatch {this} this entry twice!!!");
        }
    }

    public bool LoggerHasDispatched => DispatchedAt != null;

    public IFLogStringAppender StringAppender(StringBuildingStyle style = StringBuildingStyle.Default)
    {
        Thread      = Thread.CurrentThread;
        LogDateTime = TimeContext.UtcNow;
        Style       = style;

        var styleTypeStringAppender = (Recycler?.Borrow<WrappingStyledTypeStringAppender>() ?? new WrappingStyledTypeStringAppender(style))
            .Initialize(messageBuilder!.BackingStringBuilder, style);
        var stringAppender = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(styleTypeStringAppender, OnMessageComplete);

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
            .Initialize(formatterBuilder, LogLocation, OnMessageComplete, style);

        return formatAppender;
    }


    public object?    CallerContextObject { get; protected set; }
    public long       CorrelationId       { get; protected set; }
    public Exception? Exception           { get; protected set; }

    public uint IssueSequenceNumber { get; set; }

    public uint ReceivedSequenceNumber { get; set; }

    public uint InstanceNumber { get; }

    public bool IsFrozen => messageBuilder!.IsFrozen;

    IMaybeFrozen IFreezable.Freeze => Freeze;

    public IFLogEntry Freeze
    {
        get
        {
            _ = messageBuilder!.Freeze;
            return this;
        }
    }

    IFrozenString IFLogEntry.Message => messageBuilder!.Freeze;

    public IMutableString Message => !IsFrozen ? messageBuilder! : throw new ModifyFrozenObjectAttempt("Attempted to modify a frozen FLogEntry");

    public bool ThrowOnMutateAttempt
    {
        get => messageBuilder!.ThrowOnMutateAttempt;
        set => messageBuilder!.ThrowOnMutateAttempt = value;
    }

    public DateTime LogDateTime { get; private set; }

    public LoggingLocation LogLocation { get; private set; }

    public IFLogger Logger { get; private set; } = null!;

    public FLogLevel LogLevel { get; set; }

    public Thread Thread { get; private set; } = null!;

    public StringBuildingStyle Style { get; private set; }


    public IMutableFLogEntry WithCallerContextObj(object callerContextObj)
    {
        CallerContextObject = callerContextObj;

        return this;
    }

    public IMutableFLogEntry WithCorrelationId(long correlationId)
    {
        CorrelationId = correlationId;

        return this;
    }

    public IMutableFLogEntry AddException(Exception exception) 
    {
        Exception = exception;

        return this;
    }

    public IMutableFLogEntry AddCallerContextObjFromKey(string asyncCallOrThreadKey)
    {
        CallerContextObject = CallContext.GetContextData(asyncCallOrThreadKey);

        return this;
    }

    public IMutableFLogEntry AddCorrelationId(string asyncCallOrThreadKey = IMutableFLogEntry.DefaultCorrelationIdKeyName) 
    {
        CorrelationId = CallContextLongs.GetContextData(asyncCallOrThreadKey) ?? 0;

        return this;
    }


    public override void StateReset()
    {
        ReceivedSequenceNumber = 0;
        IssueSequenceNumber    = 0;

        messageBuilder!.StateReset();
        LogDateTime = DateTime.MinValue;
        DispatchedAt = null;
        Style       = StringBuildingStyle.Default;
        LogLocation = NotSetLocation;
        Logger      = null!;
        Thread      = null!;
        base.StateReset();
    }

    public override IFLogEntry Clone() => Recycler?.Borrow<FLogEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogEntry(this);

    public override FLogEntry CopyFrom(IFLogEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        IssueSequenceNumber    = source.IssueSequenceNumber;
        ReceivedSequenceNumber = source.ReceivedSequenceNumber;

        messageBuilder!.Clear();
        messageBuilder!.Append(source.Message);

        Logger = source.Logger;
        Thread = source.Thread;
        Style  = source.Style;

        LogDateTime  = source.LogDateTime;
        LogLocation  = source.LogLocation;
        DispatchedAt = source.DispatchedAt;

        return this;
    }

    protected string LogEntryToStringMembers =>
        $"{nameof(messageBuilder)}: {messageBuilder}, {nameof(DispatchedAt)}: {DispatchedAt}, {nameof(IssueSequenceNumber)}: {IssueSequenceNumber}, " +
        $"{nameof(InstanceNumber)}: {InstanceNumber}, {nameof(LogDateTime)}: {LogDateTime}, {nameof(LogLocation)}: {LogLocation}";

    public override string ToString() => $"{nameof(FLogEntry)}{{{LogEntryToStringMembers}}}";
}
