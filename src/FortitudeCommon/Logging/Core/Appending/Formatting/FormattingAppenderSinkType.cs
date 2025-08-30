// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

[Flags]
public enum FormattingAppenderSinkType : byte
{
    None     = 0x00
  , File     = 0x01
  , Console  = 0x02
  , Memory   = 0x04
  , Network  = 0x08
  , Database = 0x10
  , Custom   = 0x20
  , Any      = 0xFF
}

public static class AppenderSinkTypeExtensions
{
    public static bool IsFileAppender(this FormattingAppenderSinkType flags)     => (flags & FormattingAppenderSinkType.File) > 0;
    public static bool IsConsoleAppender(this FormattingAppenderSinkType flags)  => (flags & FormattingAppenderSinkType.Console) > 0;
    public static bool IsNetworkAppender(this FormattingAppenderSinkType flags)  => (flags & FormattingAppenderSinkType.Network) > 0;
    public static bool IsDatabaseAppender(this FormattingAppenderSinkType flags) => (flags & FormattingAppenderSinkType.Database) > 0;
    public static bool IsCustomAppender(this FormattingAppenderSinkType flags)   => (flags & FormattingAppenderSinkType.Custom) > 0;


    public static bool HasAllOf(this FormattingAppenderSinkType flags, FormattingAppenderSinkType checkAllFound) =>
        (flags & checkAllFound) == checkAllFound;

    public static bool HasNoneOf(this FormattingAppenderSinkType flags, FormattingAppenderSinkType checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this FormattingAppenderSinkType flags, FormattingAppenderSinkType checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this FormattingAppenderSinkType flags, FormattingAppenderSinkType checkAllFound)   => flags == checkAllFound;
}
