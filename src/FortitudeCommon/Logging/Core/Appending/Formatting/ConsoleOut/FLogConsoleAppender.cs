using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.ConsoleOut;

public class FLogConsoleAppender : FLogBufferingFormatAppender
{
    public FLogConsoleAppender(IConsoleAppenderConfig consoleAppenderConfig, IFLogContext context) : base(consoleAppenderConfig, context)
    {
        ConsoleColorEnabled = !consoleAppenderConfig.DisableColoredConsole;
    }

    public bool ConsoleColorEnabled { get; set; }

    protected override IFormatWriter CreatedAppenderDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback) =>
        new ConsoleFormatWriter().Initialize(this, onWriteCompleteCallback);


    public override FormattingAppenderSinkType FormatAppenderType => FormattingAppenderSinkType.Console;

    public override IConsoleAppenderConfig GetAppenderConfig() => (IConsoleAppenderConfig)AppenderConfig;

    private class ConsoleFormatWriter : CharBufferFlushingFormatWriter<ConsoleFormatWriter>
    {
        private readonly object syncLock = new();

        private FLogConsoleAppender owningAppender;

        public ConsoleFormatWriter Initialize(FLogConsoleAppender owningAppender
          , FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
        {
            base.Initialize(owningAppender, $"Direct{nameof(ConsoleFormatWriter)}", onWriteCompleteCallback);

            IsIOSynchronous = true;
            this.owningAppender = owningAppender;

            return this;
        }

        public override void Append(string toWrite)
        {
            Console.Write(toWrite);
        }

        public override void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var bufferSize = Math.Min(toWrite.Length - fromIndex, length);
            var charArray  = bufferSize.SourceRecyclingCharArray();
            charArray.Add(toWrite, fromIndex, bufferSize);
            Console.Write(charArray.BackingArray, 0, bufferSize);
            charArray.DecrementRefCount();
        }

        public override void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var bufferSize = Math.Min(toWrite.Length - fromIndex, length);
            var charArray  = bufferSize.SourceRecyclingCharArray();
            charArray.Add(toWrite, fromIndex, bufferSize);
            Console.Write(charArray.BackingArray, 0, bufferSize);
            charArray.DecrementRefCount();
        }

        public override void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var bufferSize = Math.Min(toWrite.Length - fromIndex, length);
            Console.Write(toWrite, 0, bufferSize);
        }

        public override void FlushBufferToAppender(ICharArrayFlushedBufferedFormatWriter toFlush)
        {
            lock (syncLock)
            {
                var bufferAndRange = toFlush.FlushRange();
                Console.Out.Write(bufferAndRange.CharBuffer, 0, bufferAndRange.Length);
            }
            toFlush.Clear();
            owningAppender.FormatWriterRequestCache.TryToReturnUsedFormatWriter(toFlush);
        }
    }
}
