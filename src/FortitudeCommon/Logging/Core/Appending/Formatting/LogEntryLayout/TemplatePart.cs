// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface ITemplatePart
{
    FormattingAppenderSinkType TargetingAppenderTypes { get; }

    int Apply(IFormatWriter formatWriter, IFLogEntry logEntry);
}
