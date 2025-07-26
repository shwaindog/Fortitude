using System.Text;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;
using FortitudeCommon.Logging.Core.LogEntries;

public class ConsoleLogLevelBackgroundColorMatchTemplatePart : AppenderCommandTemplatePart
{
    private readonly Dictionary<FLogLevel, ConsoleColor> textColorByLogLevel = new ();

    private readonly ConsoleColor defaultColor;

    public ConsoleLogLevelBackgroundColorMatchTemplatePart(string command)
        : base(FormattingAppenderSinkType.Console, command)
    {
        defaultColor = Console.ForegroundColor;
        var split = command.Split(',');
        var i     = 0;
        for (; i < split.Length && i < FLogLevelExtensions.LoggableRange; i++)
        {
            var colorString = split[i];
            var fLogLevel   = FLogLevel.Error - i;
            
            if (Enum.TryParse<ConsoleColor>(colorString, out var conColor))
            {
                textColorByLogLevel.Add(fLogLevel, conColor);
            }
            else
            {
                Console.WriteLine($"Failed to parse ${command} as a valid System.ConsoleColor");
            }
        }
    }

    public override int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        if (formatWriter.OwningAppender.FormatAppenderType.IsConsoleAppender())
        {
            var color               = textColorByLogLevel.GetValueOrDefault(logEntry.LogLevel, defaultColor);
            var colorChangeSequence = ConsoleColorToEscapeLookup.GetConsoleColorChangeString(ConsoleChangeColorType.Background, color);
            formatWriter.Append(colorChangeSequence);
        }
        return 0;
    }
}
