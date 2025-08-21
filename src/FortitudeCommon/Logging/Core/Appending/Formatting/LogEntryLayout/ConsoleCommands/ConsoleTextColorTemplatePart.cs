using System.Text;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;

public class ConsoleTextColorTemplatePart : AppenderCommandTemplatePart
{
    private readonly ConsoleColor setToColour = ConsoleColor.White;

    public ConsoleTextColorTemplatePart(string command) : base(FormattingAppenderSinkType.Console, command)
    {
        if (Enum.TryParse<ConsoleColor>(command, out var conColor))
        {
            setToColour = conColor;
        }
        else
        {
            Console.WriteLine($"Failed to parse ${command} as a valid System.ConsoleColor");
        }
    }

    public override int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        if (formatWriter.OwningAppender.FormatAppenderType.IsConsoleAppender())
        {
            var colorChangeSequence = ConsoleColorToEscapeLookup.GetConsoleColorChangeString(ConsoleChangeColorType.Text, setToColour);
            formatWriter.Append(colorChangeSequence);
        }
        return 0;
    }
}