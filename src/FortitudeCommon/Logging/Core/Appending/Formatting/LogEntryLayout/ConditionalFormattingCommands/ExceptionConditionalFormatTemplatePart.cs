using System.Text;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConditionalFormattingCommands;

public class LogLevelConditionalFormatTemplatePart(FLogLevel fLogLevel, string command) 
    : AppenderCommandTemplatePart(FormattingAppenderSinkType.Any, command)
{
    private readonly List<ITemplatePart> conditionalMetRunParts = TokenisedLogEntryFormatStringParser.Instance.BuildTemplateParts(command);

    public override int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        var charsAppended = 0;
        if (logEntry.LogLevel == fLogLevel)
        {
            foreach (var conditionalMetRunPart in conditionalMetRunParts)
            {
                charsAppended += conditionalMetRunPart.Apply(formatWriter, logEntry);
            }
        }
        return charsAppended;
    }
}
