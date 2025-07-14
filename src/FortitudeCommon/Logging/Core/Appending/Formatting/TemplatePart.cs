// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface ITemplatePart
{
    void Apply(StringBuilder sb, IFLogEntry logEntry);
}

public class StringConstantTemplatePart(string toAppend) : ITemplatePart
{
    public void Apply(StringBuilder sb, IFLogEntry logEntry) => sb.Append(toAppend);
}