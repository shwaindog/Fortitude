// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IFLogFormattingAppender : IFLogAppender, IFormatWriterResolver
{
    int FormatWriterRequestQueue { get; }

    long TotalBytesAppended { get; }

    long TotalCharsAppended { get; }

    FormattingAppenderSinkType FormatAppenderType { get; }

    IFLogEntryFormatter Formatter { get; }
}

public interface IMutableFLogFormattingAppender : IFLogFormattingAppender, IMutableFLogAppender
{
    new IFLogEntryFormatter Formatter { get; set; }

    IFormatWriter CreatedDirectFormatWriter(IFLogContext context, string targetName
      , FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback);
}

public delegate void FormatWriterReceivedHandler<in T>(T formatWriter) where T : IFormatWriter;

public abstract class FLogFormattingAppender : FLogAppender, IMutableFLogFormattingAppender
{
    protected IFormatWriterRequestCache FormatWriterRequestCache = null!;

    protected FLogFormattingAppender(IFormattingAppenderConfig formattingAppenderConfig, IFLogContext context
      , bool isSingleDestinationAppender = true)
        : base(formattingAppenderConfig, context)
    {
        Formatter = new FLogEntryFormatter(formattingAppenderConfig.LogEntryFormatLayout, this);
        IsOpen    = true;

        if (isSingleDestinationAppender) FormatWriterRequestCache = CreateFormatWriterRequestCache(formattingAppenderConfig, context);
    }

    public IFLogEntryFormatter Formatter { get; set; }

    public bool IsOpen { get; private set; }

    public virtual int FormatWriterRequestQueue => FormatWriterRequestCache.FormatWriterRequestQueue;

    public virtual IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry) =>
        FormatWriterRequestCache.FormatWriterResolver(logEntry);

    public abstract FormattingAppenderSinkType FormatAppenderType { get; }

    IFormatWriter IMutableFLogFormattingAppender.CreatedDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback) =>
        CreatedAppenderDirectFormatWriter(context, targetName, onWriteCompleteCallback);

    public long TotalBytesAppended { get; protected set; }

    public long TotalCharsAppended { get; protected set; }

    public void Close()
    {
        if (IsOpen)
        {
            IsOpen = false;
            FormatWriterRequestCache.Close();
        }
    }

    protected virtual IFormatWriterRequestCache CreateFormatWriterRequestCache(IFormattingAppenderConfig formattingAppenderConfig
      , IFLogContext context) =>
        new SingleDestDirectFormatWriterRequestCache().Initialize(this, context);

    public override void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        if (!IsOpen) return;
        if (logEntryEvent.LogEntryEventType == LogEntryEventType.SingleEntry)
        {
            var fLogEntry = logEntryEvent.LogEntry;
            if (fLogEntry != null) Formatter.ApplyFormatting(fLogEntry);
        }
        else
        {
            var logEntriesBatch = logEntryEvent.LogEntriesBatch;
            var count           = logEntriesBatch?.Count ?? 0;
            for (var i = 0; i < count; i++)
            {
                var flogEntry = logEntriesBatch![i];
                Formatter.ApplyFormatting(flogEntry);
            }
        }
    }

    protected abstract IFormatWriter CreatedAppenderDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback);
}
