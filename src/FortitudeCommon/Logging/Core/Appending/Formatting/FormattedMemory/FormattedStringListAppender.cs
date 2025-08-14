using System.Text;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.Hub;
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

    protected override IFormatWriter CreatedDirectFormatWriter(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig)
    {
        DirectFormatWriter ??= new FormattedStringList(this, OnReturningFormatWriter);
        return DirectFormatWriter;
    }

    private class FormattedStringList(FormattedStringListAppender owningAppender, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
        : FormatWriter<IFormatWriter>(owningAppender, onWriteCompleteCallback)
    {
        private readonly MutableString entryBuilder = new();

        private FormattedStringListAppender AsStringListAppender => (FormattedStringListAppender)OwningAppender;

        public override void NotifyStartEntryAppend()
        {
            entryBuilder.Clear();
        }

        public override void NotifyEntryAppendComplete()
        {
            var newEntry = entryBuilder.ToString();
            AsStringListAppender.LogEntries.Add(newEntry);
            AsStringListAppender.OnNewEntry(newEntry);
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

        public override bool IsIOSynchronous => true;
    }

}