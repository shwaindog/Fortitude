// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;

public enum ConsoleChangeColorType
{
    Text // aka Foreground
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
        "\x1B[30m" // black.
      , "\x1B[34m" // dark blue.
      , "\x1B[32m" // dark green.
      , "\x1B[36m" // dark cyan (dark blue-green).
      , "\x1B[31m" // dark red
      , "\x1B[35m" // dark magenta (dark purplish-red).
      , "\x1B[33m" // dark yellow (ochre).
      , "\x1B[37m" // gray.
      , "\x1B[90m" // dark gray.
      , "\x1B[94m" // blue.
      , "\x1B[92m" // green.
      , "\x1B[96m" // cyan (blue-green).
      , "\x1B[91m" // red.
      , "\x1B[95m" // magenta (purplish-red).
      , "\x1B[93m" // yellow.
      , "\x1B[97m" // white.
    ];

    public static readonly string[] UniversalConsoleBackgroundColorEscapeStrings =
    [
        "\x1B[40m"  // black.
      , "\x1B[44m"  // dark blue.
      , "\x1B[42m"  // dark green.
      , "\x1B[46m"  // dark cyan (dark blue-green).
      , "\x1B[41m"  // dark red
      , "\x1B[45m"  // dark magenta (dark purplish-red).
      , "\x1B[43m"  // dark yellow (ochre).
      , "\x1B[47m"  // gray.
      , "\x1B[100m" // dark gray.
      , "\x1B[104m" // blue.
      , "\x1B[102m" // green.
      , "\x1B[106m" // cyan (blue-green).
      , "\x1B[101m" // red.
      , "\x1B[105m" // magenta (purplish-red).
      , "\x1B[103m" // yellow.
      , "\x1B[107m" // white.
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
