using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.ConsoleOut;

public class FLogConsoleAppender : FLogBufferingFormatAppender
{
    public FLogConsoleAppender(IConsoleAppenderConfig consoleAppenderConfig, IFLogContext context) : base(consoleAppenderConfig, context)
    {
        ConsoleColorEnabled = !consoleAppenderConfig.DisableColoredConsole;
    }

    public bool ConsoleColorEnabled { get; set; }

    protected override IFormatWriter CreatedDirectFormatWriter(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig)
    {
        DirectFormatWriter ??= new ConsoleFormatWriter(this, OnReturningFormatWriter);
        return DirectFormatWriter;
    }

    public override void FlushBuffer(IBufferedFormatWriter toFlush)
    {
        var bufferAndRange  = toFlush.FlushRange();
        var charBufferLength = bufferAndRange.Length;
        Console.Out.Write(bufferAndRange.CharBuffer, 0, charBufferLength);
    }

    public override FormattingAppenderSinkType FormatAppenderType => FormattingAppenderSinkType.Console;
    
    public override IConsoleAppenderConfig GetAppenderConfig() => (IConsoleAppenderConfig)AppenderConfig;

    private class ConsoleFormatWriter(FLogConsoleAppender owningAppender, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
        : FormatWriter<IFormatWriter>(owningAppender, onWriteCompleteCallback)
    {
        public override void Append(string toWrite)
        {
            Console.Write(toWrite);
        }

        public override void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var bufferSize = Math.Min(toWrite.Length - fromIndex, length);
            var charArray = bufferSize.SourceRecyclingCharArray();
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

        public override bool IsIOSynchronous => true;
    }
}
