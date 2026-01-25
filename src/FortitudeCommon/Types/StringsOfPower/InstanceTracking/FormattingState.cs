// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
  , IEncodingTransfer? GraphEncoder
  , IEncodingTransfer? ParentEncoder
);