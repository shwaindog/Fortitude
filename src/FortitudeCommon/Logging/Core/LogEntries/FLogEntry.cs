// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Framework.Fillers;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogEntry : IReusableObject<IFLogEntry>, IMaybeFrozen, IStringBearer
{
    uint IssueSequenceNumber { get; }
    uint ReceivedSequenceNumber { get; }
    uint InstanceNumber { get; }
    long CorrelationId { get; }

    object? CallerContextObject { get; }

    Exception? Exception { get; }

    StringStyle Style { get; }

    DateTime LogDateTime { get; }
    DateTime? DispatchedAt { get; }
    FLogCallLocation LogLocation { get; }
    IFLogger Logger { get; }
    FLogLevel LogLevel { get; }
    Thread Thread { get; }
    IFrozenString Message { get; }
    event Action<IFLogEntry> MessageComplete;
}

public interface IMutableFLogEntry : IFLogEntry, IFreezable<IFLogEntry>
{
    const string DefaultCorrelationIdKeyName = "CorrelationId";

    new uint ReceivedSequenceNumber { get; set; }

    new FLogLevel LogLevel { get; set; }

    new IMutableString Message { get; }


    [MustUseReturnValue("Use WithOnlyParam or AndFinalParam to complete LogEntry")]
    IFLogFirstFormatterParameterEntry FormatBuilder([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formattedString
      , StringStyle style = StringStyle.Default);


    [MustUseReturnValue("Use FinalAppend to complete LogEntry")]
    IFLogStringAppender StringAppender(StringStyle style = StringStyle.Default);

    IMutableFLogEntry AddCallerContextObjFromKey(string asyncCallOrThreadKey);
    IMutableFLogEntry WithCallerContextObj(object callerContextObj);
    IMutableFLogEntry AddCorrelationId(string asyncCallOrThreadKey = DefaultCorrelationIdKeyName);
    IMutableFLogEntry WithCorrelationId(long correlationId);

    IMutableFLogEntry AddException(Exception exception);

    void OnMessageComplete(IStringBuilder? warningToPrefix);
}

public record struct LoggerEntryContext
    (IFLogger Logger, ITargetingFLogEntrySource OnCompleteHandler, FLogCallLocation LogLocation, FLogLevel LogLevel);

public class FLogEntry : ReusableObject<IFLogEntry>, IMutableFLogEntry
{
    internal static readonly FLogCallLocation NotSetLocation = new("NotSet", "NotSet", 0);

    private static uint totalInstanceCount;

    private ITargetingFLogEntrySource dispatchHandler = null!;

    private MutableString? messageBuilder;

    public FLogEntry()
    {
        InstanceNumber  = Interlocked.Increment(ref totalInstanceCount);
        MessageComplete = SendToDispatchHandler;
    }

    public FLogEntry(MutableString messageBuilder) : this() => this.messageBuilder = messageBuilder;

    public FLogEntry(IFLogEntry toClone) : this()
    {
        messageBuilder = (MutableString)toClone.Message.SourceThawed;

        LogDateTime     = toClone.LogDateTime;
        LogLocation     = NotSetLocation;
        MessageComplete = SendToDispatchHandler;
    }

    internal FLogEntry Initialize(LoggerEntryContext loggerEntryContext)
    {
        messageBuilder ??= new MutableString();

        Thread          = Thread.CurrentThread;
        dispatchHandler = loggerEntryContext.OnCompleteHandler;
        LogLocation     = loggerEntryContext.LogLocation;
        Logger          = loggerEntryContext.Logger;
        LogLevel        = loggerEntryContext.LogLevel;

        return this;
    }

    public bool LoggerHasDispatched => DispatchedAt != null;

    protected string LogEntryToStringMembers =>
        $"{nameof(messageBuilder)}: {messageBuilder}, {nameof(DispatchedAt)}: {DispatchedAt}, {nameof(IssueSequenceNumber)}: {IssueSequenceNumber}, " +
        $"{nameof(InstanceNumber)}: {InstanceNumber}, {nameof(LogDateTime)}: {LogDateTime}, {nameof(LogLocation)}: {LogLocation}";

    public event Action<IFLogEntry> MessageComplete;

    public DateTime? DispatchedAt { get; protected set; }

    public void OnMessageComplete(IStringBuilder? warningToPrefix)
    {
        if (DispatchedAt == null)
        {
            if (warningToPrefix != null) messageBuilder!.Insert(0, warningToPrefix);
            DispatchedAt = TimeContext.UtcNow;
            MessageComplete.Invoke(this);
        }
        else
        {
            Console.Out.WriteLine($"Attempted to dispatch {this} this entry twice!!!");
        }
    }

    public IFLogStringAppender StringAppender(StringStyle style = StringStyle.Default)
    {
        Thread      = Thread.CurrentThread;
        LogDateTime = TimeContext.UtcNow;
        Style       = style;

        var styleTypeStringAppender = (Recycler?.Borrow<TheOneString>() ?? new TheOneString(style))
            .Initialize(messageBuilder!, style);
        var stringAppender = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(this, styleTypeStringAppender, OnMessageComplete);

        return stringAppender;
    }

    public IFLogFirstFormatterParameterEntry FormatBuilder([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formattedString
      , StringStyle style = StringStyle.Default)
    {
        Thread      = Thread.CurrentThread;
        LogDateTime = TimeContext.UtcNow;
        Style       = style;

        var formatterBuilder = (Recycler?.Borrow<FormatBuilder>() ?? new FormatBuilder())
            .Initialize(formattedString, messageBuilder!);


        var formatAppender = (Recycler?.Borrow<FLogFirstFormatterParameterEntry>() ?? new FLogFirstFormatterParameterEntry())
            .Initialize(this, formatterBuilder, 0, OnMessageComplete);

        return formatAppender;
    }


    public object? CallerContextObject { get; protected set; }
    public long CorrelationId { get; protected set; }
    public Exception? Exception { get; protected set; }

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

    public FLogCallLocation LogLocation { get; private set; }

    public IFLogger Logger { get; private set; } = null!;

    public FLogLevel LogLevel { get; set; }

    public Thread Thread { get; private set; } = null!;

    public StringStyle Style { get; private set; }


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

    protected virtual void SendToDispatchHandler(IFLogEntry me)
    {
        dispatchHandler.PublishLogEntryEvent(new LogEntryPublishEvent(me));
        DecrementRefCount();
    }


    public override void StateReset()
    {
        ReceivedSequenceNumber = 0;
        IssueSequenceNumber    = 0;

        messageBuilder!.StateReset();
        LogDateTime  = DateTime.MinValue;
        DispatchedAt = null;
        Style        = StringStyle.Default;
        LogLocation  = NotSetLocation;
        Logger       = null!;
        Thread       = null!;
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

    public StateExtractStringRange RevealState(ITheOneString tos)
    {
        using var tb =
            tos.StartComplexType(this)
               .Field.AlwaysAdd(nameof(IssueSequenceNumber), IssueSequenceNumber)
               .Field.AlwaysAdd(nameof(InstanceNumber), InstanceNumber)
               .Field.AlwaysAdd(nameof(RefCount), RefCount)
               .Field.WhenNonDefaultAdd(nameof(CorrelationId), CorrelationId)
               .Field.AlwaysAdd(nameof(LogDateTime), LogDateTime, "{0:yyyy-MM-ddTHH:mm:ss.ffffff}")
               .Field.AlwaysAdd(nameof(LogLevel), LogLevel)
               .Field.AlwaysAdd(nameof(LogLocation), LogLocation, LogLocation.Styler())
               .Field.WhenNonDefaultAdd(nameof(Logger), Logger.FullName)
               .Field.WhenNonNullAdd(nameof(Style), Style)
               .Field.WhenNonNullAdd("Thread.Id", Thread?.ManagedThreadId)
               .Field.WhenNonNullAdd("Thread.Name", Thread?.Name)
               .Field.AlwaysAdd(nameof(Message), messageBuilder)
               .Field.WhenNonNullAddObject(nameof(CallerContextObject), CallerContextObject);

        return tb.Complete();
    }

    public override string ToString() => this.DefaultToString();
}
