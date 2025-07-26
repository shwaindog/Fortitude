using System.Text;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public abstract class AppenderCommandTemplatePart : ITemplatePart
{
    public string Command { get; }

    public AppenderCommandTemplatePart(FormattingAppenderSinkType targetingAppenderType, string command)
    {
        TargetingAppenderTypes = targetingAppenderType;

        Command = command;
    }

    public abstract int Apply(IFormatWriter formatWriter, IFLogEntry logEntry);

    public FormattingAppenderSinkType TargetingAppenderTypes { get; }
}
