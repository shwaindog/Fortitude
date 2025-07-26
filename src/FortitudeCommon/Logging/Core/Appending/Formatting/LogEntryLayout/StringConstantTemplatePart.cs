// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public class StringConstantTemplatePart(string toAppend) : ITemplatePart
{
    public FormattingAppenderSinkType TargetingAppenderTypes => FormattingAppenderSinkType.Any;

    public int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        formatWriter.Append(toAppend);
        return toAppend.Length;
    }
}
