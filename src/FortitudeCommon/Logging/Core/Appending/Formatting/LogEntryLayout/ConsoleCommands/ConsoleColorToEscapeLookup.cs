using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;

public enum ConsoleChangeColorType
{
      Text         // aka Foreground
    , Background
}

public class ConsoleColorToEscapeLookup
{
    // System.ConsoleColor Enum
    // Black        0   The color black.
    // DarkBlue	    1   The color dark blue.
    // DarkGreen	2   The color dark green.
    // DarkCyan 	3   The color dark cyan (dark blue-green).
    // DarkRed	    4   The color dark red.
    // DarkMagenta	5   The color dark magenta (dark purplish-red).
    // DarkYellow	6	The color dark yellow (ochre).
    // Gray	        7   The color gray.
    // DarkGray	    8   The color dark gray.
    // Blue	        9   The color blue.
    // Green	    10  The color green.
    // Cyan	        11  The color cyan (blue-green).
    // Red	        12  The color red.
    // Magenta	    13  The color magenta (purplish-red).
    // Yellow	    14  The color yellow.
    // White	    15  The color white.

    // https://en.wikipedia.org/wiki/ANSI_escape_code

    public static readonly string[] UniversalConsoleTextColorEscapeStrings =
    [
        "\x1B1E" // black.
      , "\x1B22" // dark blue.
      , "\x1B20" // dark green.
      , "\x1B24" // dark cyan (dark blue-green).
      , "\x1B1F" // dark red
      , "\x1B23" // dark magenta (dark purplish-red).
      , "\x1B21" // dark yellow (ochre).
      , "\x1B25" // gray.
      , "\x1B5A" // dark gray.
      , "\x1B5E" // blue.
      , "\x1B5C" // green.
      , "\x1B60" // cyan (blue-green).
      , "\x1B5B" // red.
      , "\x1B5F" // magenta (purplish-red).
      , "\x1B5D" // yellow.
      , "\x1B61" // white.
    ];

    public static readonly string[] UniversalConsoleBackgroundColorEscapeStrings =
    [
        "\x1B28" // black.
      , "\x1B2C" // dark blue.
      , "\x1B2A" // dark green.
      , "\x1B2E" // dark cyan (dark blue-green).
      , "\x1B29" // dark red
      , "\x1B2D" // dark magenta (dark purplish-red).
      , "\x1B2B" // dark yellow (ochre).
      , "\x1B2F" // gray.
      , "\x1B64" // dark gray.
      , "\x1B68" // blue.
      , "\x1B66" // green.
      , "\x1B6A" // cyan (blue-green).
      , "\x1B65" // red.
      , "\x1B69" // magenta (purplish-red).
      , "\x1B67" // yellow.
      , "\x1B6B" // white.
    ];

    public static string GetConsoleColorChangeString(ConsoleChangeColorType consoleColorType, ConsoleColor color)
    {
        var colorTypeTable = consoleColorType == ConsoleChangeColorType.Text
            ? UniversalConsoleTextColorEscapeStrings
            : UniversalConsoleBackgroundColorEscapeStrings;

        var colorEscapeSequence = colorTypeTable[(int)color];

        return colorEscapeSequence;
    }
}
