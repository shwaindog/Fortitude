// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.ConsoleOut;

public class FLogConsoleAppender : FLogBufferingFormatAppender
{
    public FLogConsoleAppender(IConsoleAppenderConfig consoleAppenderConfig, IFLogContext context) : base(consoleAppenderConfig, context) =>
        ConsoleColorEnabled = !consoleAppenderConfig.DisableColoredConsole;

    public bool ConsoleColorEnabled { get; set; }

    public override FormattingAppenderSinkType FormatAppenderType => FormattingAppenderSinkType.Console;

    protected override IFormatWriter CreatedAppenderDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback) =>
        new ConsoleFormatWriter().Initialize(this, onWriteCompleteCallback);

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

    public override IConsoleAppenderConfig GetAppenderConfig() => (IConsoleAppenderConfig)AppenderConfig;

    private class ConsoleFormatWriter : CharBufferFlushingFormatWriter<ConsoleFormatWriter>
    {
        private readonly object syncLock = new();

        private FLogConsoleAppender consoleAppender = null!;

        public ConsoleFormatWriter Initialize(FLogConsoleAppender owningAppender
          , FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
        {
            base.Initialize(owningAppender, $"Direct{nameof(ConsoleFormatWriter)}", onWriteCompleteCallback);

            IsIOSynchronous = true;
            consoleAppender = owningAppender;

            return this;
        }

        public override void Append(string toWrite)
        {
            Console.Write(toWrite);
        }

        public override void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            if (fromIndex == 0 && length == int.MaxValue)
            {
                Console.Write(toWrite);
                return;
            }
            var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
            var cappedLength = Math.Clamp(length, 0, toWrite.Length);
            var charArray    = cappedLength.SourceRecyclingCharArray();
            charArray.Add(toWrite, fromIndex, cappedLength);
            Console.Write(charArray.BackingArray, cappedFrom, cappedLength);
            charArray.DecrementRefCount();
        }

        public override void Append(ICharSequence toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            if (toWrite is CharArrayStringBuilder charArrayBuilder)
            {
                var writtenCharArrayRange = charArrayBuilder.AsCharArrayRange;
                var cappedFrom            = Math.Clamp(fromIndex, writtenCharArrayRange.FromIndex, writtenCharArrayRange.Length - 1);
                var cappedLength          = Math.Clamp(length, 0, writtenCharArrayRange.Length);

                Console.Write(writtenCharArrayRange.CharBuffer, cappedFrom, cappedLength);
            }
            else if (toWrite is MutableString mutableString)
            {
                var sb = mutableString.BackingStringBuilder;
                Append(sb, fromIndex, length);
            }
            else
            {
                var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
                var cappedLength = Math.Clamp(length, 0, toWrite.Length);
                var charArray    = cappedLength.SourceRecyclingCharArray();
                charArray.Add(toWrite, cappedFrom, cappedLength);
                Console.Write(charArray.BackingArray, 0, cappedLength);
                charArray.DecrementRefCount();
            }
        }

        public override void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
            var cappedLength = Math.Clamp(length, 0, toWrite.Length);
            var charArray    = cappedLength.SourceRecyclingCharArray();
            charArray.Add(toWrite, cappedFrom, cappedLength);
            Console.Write(charArray.BackingArray, 0, cappedLength);
            charArray.DecrementRefCount();
        }

        public override void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
            var cappedLength = Math.Clamp(length, 0, toWrite.Length);
            Console.Write(toWrite, cappedFrom, cappedLength);
        }

        public override void NotifyEntryAppendComplete()
        {
            consoleAppender.IncrementLogEntriesProcessed();
        }

        public override void FlushBufferToAppender(ICharArrayFlushedBufferedFormatWriter toFlush)
        {
            lock (syncLock)
            {
                var bufferAndRange = toFlush.FlushRange();
                Console.Out.Write(bufferAndRange.CharBuffer, 0, bufferAndRange.Length);
            }
            consoleAppender.IncrementLogEntriesProcessed(toFlush.BufferedFormattedLogEntries);
            toFlush.Clear();
            consoleAppender.FormatWriterRequestCache.TryToReturnUsedFormatWriter(toFlush);
        }
    }
}
