// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;

public abstract class ConsoleAppenderColorChangeTemplatePart : AppenderCommandTemplatePart
{
    protected ConsoleAppenderColorChangeTemplatePart(FormattingAppenderSinkType targetingAppenderType, string command)
        : base(targetingAppenderType, command) { }

    public bool WasScopeClosed { get; set; }
}
