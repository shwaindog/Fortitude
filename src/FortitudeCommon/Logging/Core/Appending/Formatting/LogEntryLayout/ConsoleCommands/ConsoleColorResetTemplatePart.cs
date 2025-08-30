// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.Appending.Formatting.ConsoleOut;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;

public class ConsoleColorResetTemplatePart : ConsoleAppenderColorChangeTemplatePart
{
    static ConsoleColorResetTemplatePart()
    {
        DefaultTextColor       = Console.ForegroundColor;
        DefaultBackgroundColor = Console.BackgroundColor;
    }

    public ConsoleColorResetTemplatePart(string command) : base(FormattingAppenderSinkType.Console, command) => WasScopeClosed = true;

    public static ConsoleColor DefaultTextColor { get; set; }
    public static ConsoleColor DefaultBackgroundColor { get; set; }

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
