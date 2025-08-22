using FortitudeCommon.Logging.Core.Appending.Formatting.ConsoleOut;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;

public class ConsoleColorResetTemplatePart(string command) : AppenderCommandTemplatePart(FormattingAppenderSinkType.Console, command)
{
    private static ConsoleColor onStartTextColor;
    private static ConsoleColor onStartBackgroundColor;

    static ConsoleColorResetTemplatePart()
    {
        onStartTextColor       = Console.ForegroundColor;
        onStartBackgroundColor = Console.BackgroundColor;
    }

    public static ConsoleColor DefaultTextColor
    {
        get => onStartTextColor;
        set => onStartTextColor = value;
    }
    public static ConsoleColor DefaultBackgroundColor
    {
        get => onStartBackgroundColor;
        set => onStartBackgroundColor = value;
    }

    public override int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        if (formatWriter.OwningAppender.FormatAppenderType.IsConsoleAppender() && formatWriter.OwningAppender is FLogConsoleAppender console)
        {
            var colorChangeSequence = ConsoleColorToEscapeLookup.GetConsoleColorChangeString(ConsoleChangeColorType.Text, DefaultTextColor);
            formatWriter.Append(colorChangeSequence);
            colorChangeSequence = ConsoleColorToEscapeLookup.GetConsoleColorChangeString(ConsoleChangeColorType.Background, DefaultBackgroundColor);
            formatWriter.Append(colorChangeSequence);
        }
        return 0;
    }
}