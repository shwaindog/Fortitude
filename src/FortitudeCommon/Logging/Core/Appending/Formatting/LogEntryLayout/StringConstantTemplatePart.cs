// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public class StringConstantTemplatePart(string toAppend) : ITemplatePart, IStyledToStringObject
{
    public FormattingAppenderSinkType TargetingAppenderTypes => FormattingAppenderSinkType.Any;

    public int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        formatWriter.Append(toAppend);
        return toAppend.Length;
    }

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc) =>
        sbc.StartComplexType(nameof(StringConstantTemplatePart))
           .Field.AlwaysAdd(nameof(toAppend), toAppend)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
