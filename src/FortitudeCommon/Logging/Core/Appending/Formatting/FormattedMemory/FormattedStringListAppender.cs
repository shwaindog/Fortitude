using System.Text;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormattedMemory;

public class FormattedStringListAppender(IFormattingAppenderConfig formattingAppenderConfig, IFLogContext context) 
    : FLogFormattingAppender(formattingAppenderConfig, context)
{
    public FormattedStringListAppender(string appenderName, string formattingTemplate) 
        : this(new FormattingAppenderConfig(appenderName, logEntryFormatLayout: formattingTemplate), FLogContext.Context) { }

    public List<string> LogEntries { get; } = new ();

    public event Action<string, FormattedStringListAppender>? NewEntry;

    protected void OnNewEntry(string newEntry)
    {
        NewEntry?.Invoke(newEntry, this);
    }

    public override FormattingAppenderSinkType FormatAppenderType  => FormattingAppenderSinkType.Memory;

    public override IFormattingAppenderConfig  GetAppenderConfig() => (IFormattingAppenderConfig)AppenderConfig;

    protected override IFormatWriter CreatedAppenderDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
    {
        return new FormattedStringList().Initialize(this, onWriteCompleteCallback);
    }

    private class FormattedStringList : FormatWriter<IFormatWriter>
    {
        private readonly MutableString entryBuilder = new();

        public FormattedStringList Initialize(FormattedStringListAppender owningAppender
          , FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
        {
            base.Initialize(owningAppender, $"{nameof(FormattedStringList)}", onWriteCompleteCallback);
            IsIOSynchronous = false;
            
            return this;
        }

        private FormattedStringListAppender AsStringListAppender => (FormattedStringListAppender)OwningAppender;

        public override bool NotifyStartEntryAppend(IFLogEntry forEntry)
        {
            entryBuilder.Clear();
            return true;
        }

        public override void NotifyEntryAppendComplete()
        {
            var newEntry = entryBuilder.ToString();
            AsStringListAppender.LogEntries.Add(newEntry);
            AsStringListAppender.OnNewEntry(newEntry);
            AsStringListAppender.IncrementLogEntriesProcessed();
        }

        public override void Append(string toWrite)
        {
            entryBuilder.Append(toWrite);
        }

        public override void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            entryBuilder.Append(toWrite, fromIndex, length);
        }

        public override void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            entryBuilder.Append(toWrite, fromIndex, length);
        }

        public override void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            entryBuilder.Append(toWrite, fromIndex, length);
        }
        
        
    }
}