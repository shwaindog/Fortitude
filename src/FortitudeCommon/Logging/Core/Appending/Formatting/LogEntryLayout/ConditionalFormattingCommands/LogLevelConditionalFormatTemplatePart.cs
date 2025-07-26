using System.Text;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConditionalFormattingCommands;

public class ExceptionConditionalFormatTemplatePart(string command, string exceptionContentsMatch = "") 
    : AppenderCommandTemplatePart(FormattingAppenderSinkType.Any, command)
{
    private readonly string[] matchExceptionContents = exceptionContentsMatch.IsNotEmpty() ? exceptionContentsMatch.Split(",") : [];

    private readonly List<ITemplatePart> conditionalMetRunParts = TokenisedLogEntryFormatStringParser.Instance.BuildTemplateParts(command);

    public override int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        var charsAppended = 0;
        if (logEntry.Exception != null)
        {
            if (matchExceptionContents.IsNotNullOrNone())
            {
                var exceptionString = logEntry.Exception.ToString();
                foreach (var matchExceptionContent in matchExceptionContents)
                {
                    if (exceptionString.Contains(matchExceptionContent))
                    {
                        foreach (var conditionalMetRunPart in conditionalMetRunParts)
                        {
                            charsAppended += conditionalMetRunPart.Apply(formatWriter, logEntry);
                        }
                    }
                }
            }
            else
            {
                foreach (var conditionalMetRunPart in conditionalMetRunParts)
                {
                    charsAppended += conditionalMetRunPart.Apply(formatWriter, logEntry);
                }
            }
        }
        return charsAppended;
    }
}
