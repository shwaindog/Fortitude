// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public static class StyleJsonFormattingExtensions
{
    // public static FieldContentHandling MatchToValueFlags<T>(this T check, bool hasDefault)
    // {
    //     if (check == null && !hasDefault) return DefaultCallerTypeFlags;
    //     var typeOfT = typeof(T);
    //     if (typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar())
    //         return DisableAutoDelimiting | AsValueContent;
    //     var isSpanFormattableOrNullable           = typeOfT.IsSpanFormattableOrNullable();
    //     var isDoubleQuoteDelimitedSpanFormattable = check.IsDoubleQuoteDelimitedSpanFormattable();
    //     if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable)
    //         return EnsureFormattedDelimited | AsValueContent;
    //     return AsValueContent;
    // }
    //
    // public static FieldContentHandling MatchToStringFlags<T>(this T check, bool hasDefault)
    // {
    //     if (check == null && !hasDefault) return DefaultCallerTypeFlags;
    //     var typeOfT = typeof(T);
    //     if (typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar())
    //         return DisableAutoDelimiting | AsStringContent;
    //     var isSpanFormattableOrNullable           = typeOfT.IsSpanFormattableOrNullable();
    //     var isDoubleQuoteDelimitedSpanFormattable = check.IsDoubleQuoteDelimitedSpanFormattable();
    //     if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable)
    //         return DisableAutoDelimiting | AsStringContent;
    //     return AsStringContent;
    // }
}
