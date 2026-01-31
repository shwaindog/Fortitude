// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Xml;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public record struct FormattingState
(
    int GraphDepth
  , int RemainingGraphDepth
  , FormatFlags OriginalCreateFormatFlags
  , int IndentChars
  , IStyledTypeFormatting Formatter
  , IEncodingTransfer ContentEncoder
  , IEncodingTransfer LayoutEncoder
)
{
    public FormattingState UpdateEncoders(IEncodingTransfer contentEncoder, IEncodingTransfer layoutEncoder)
    {
        return this with
        {
            ContentEncoder = contentEncoder
          , LayoutEncoder = layoutEncoder
        };
    }
};