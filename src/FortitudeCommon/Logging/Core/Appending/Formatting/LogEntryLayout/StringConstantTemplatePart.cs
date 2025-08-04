// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public class StringConstantTemplatePart(string toAppend) : ITemplatePart, IStyledToStringObject
{
    public FormattingAppenderSinkType TargetingAppenderTypes => FormattingAppenderSinkType.Any;

    public int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        formatWriter.Append(toAppend);
        return toAppend.Length;
    }

    public IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
        sbc.AddTypeName(nameof(StringConstantTemplatePart))
           .AddTypeStart()
           .AddField(nameof(toAppend), toAppend)
           .AddTypeEnd();
    }

    public override string ToString() => this.DefaultToString();
}
