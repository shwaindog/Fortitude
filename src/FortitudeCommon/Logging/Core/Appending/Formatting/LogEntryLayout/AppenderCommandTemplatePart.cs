// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public abstract class AppenderCommandTemplatePart : ITemplatePart
{
    public AppenderCommandTemplatePart(FormattingAppenderSinkType targetingAppenderType, string command)
    {
        TargetingAppenderTypes = targetingAppenderType;

        Command = command;
    }

    public string Command { get; }

    public abstract int Apply(IFormatWriter formatWriter, IFLogEntry logEntry);

    public FormattingAppenderSinkType TargetingAppenderTypes { get; }
}
