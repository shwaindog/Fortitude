// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.ConsoleOut;

public class NullConsoleAppender(IConsoleAppenderConfig consoleAppenderConfig, IFLogContext context)
    : FLogConsoleAppender(consoleAppenderConfig, context)
{
    protected override IFormatWriter CreatedAppenderDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback) =>
        new NullConsoleFormatWriter().Initialize(this, onWriteCompleteCallback);

    private class NullConsoleFormatWriter : CharBufferFlushingFormatWriter<NullConsoleFormatWriter>
    {
        private readonly object syncLock = new();

        private NullConsoleAppender consoleAppender = null!;

        private CharArrayStringBuilder dummyWriteBuffer = new(32 * 1024);

        public NullConsoleFormatWriter Initialize(NullConsoleAppender owningAppender
          , FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
        {
            base.Initialize(owningAppender, $"Direct{nameof(NullConsoleFormatWriter)}", onWriteCompleteCallback);

            IsIOSynchronous = true;
            consoleAppender = owningAppender;

            return this;
        }

        public override void Append(string toWrite)
        {
            dummyWriteBuffer.Insert(0, toWrite);
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
            dummyWriteBuffer.Insert(0, charArray.BackingArray, cappedFrom, cappedLength);
            charArray.DecrementRefCount();
        }

        public override void Append(ICharSequence toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            if (toWrite is CharArrayStringBuilder charArrayBuilder)
            {
                var writtenCharArrayRange = charArrayBuilder.AsCharArrayRange;
                var cappedFrom            = Math.Clamp(fromIndex, writtenCharArrayRange.FromIndex, writtenCharArrayRange.Length - 1);
                var cappedLength          = Math.Clamp(length, 0, writtenCharArrayRange.Length);

                dummyWriteBuffer.Insert(0, writtenCharArrayRange.CharBuffer, cappedFrom, cappedLength);
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
                dummyWriteBuffer.Insert(0, charArray.BackingArray, cappedFrom, cappedLength);
                charArray.DecrementRefCount();
            }
        }

        public override void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
            var cappedLength = Math.Clamp(length, 0, toWrite.Length);
            var charArray    = cappedLength.SourceRecyclingCharArray();
            charArray.Add(toWrite, cappedFrom, cappedLength);
            dummyWriteBuffer.Insert(0, charArray.BackingArray, 0, cappedLength);
            charArray.DecrementRefCount();
        }

        public override void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
            var cappedLength = Math.Clamp(length, 0, toWrite.Length);
            dummyWriteBuffer.Insert(0, toWrite, cappedFrom, cappedLength);
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
                dummyWriteBuffer.Insert(0, bufferAndRange.CharBuffer, 0, bufferAndRange.Length);
            }
            consoleAppender.IncrementLogEntriesProcessed(toFlush.BufferedFormattedLogEntries);
            toFlush.Clear();
            consoleAppender.FormatWriterRequestCache.TryToReturnUsedFormatWriter(toFlush);
        }
    }
}
