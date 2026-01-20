// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

// public struct MoldDieCastSettings(SkipTypeParts skipTypeParts)
// {
//     public SkipTypeParts SkipTypeParts = skipTypeParts;
// }
//
// [Flags]
// public enum SkipTypeParts : byte
// {
//     None      = 0x00
//   , All       = 0xFF
//   , TypeName  = 0x01
//   , TypeStart = 0x02
//   , TypeEnd   = 0x04
// }
//
// public static class IgnoreWriteFlagsExtensions
// {
//     public static bool HasTypeNameFlag(this SkipTypeParts flags) => (flags & SkipTypeParts.TypeName) > 0;
//
//     public static bool HasTypeStartFlag(this SkipTypeParts flags) => (flags & SkipTypeParts.TypeStart) > 0;
//
//     public static bool HasTypeEndFlag(this SkipTypeParts flags) => (flags & SkipTypeParts.TypeEnd) > 0;
//
//     public static FormatFlags ToFormattingFlags(this SkipTypeParts toConvert)
//     {
//         var flags = FormatFlags.DefaultCallerTypeFlags;
//
//         flags |= toConvert.HasTypeStartFlag() ? FormatFlags.SuppressOpening : FormatFlags.DefaultCallerTypeFlags;
//         flags |= toConvert.HasTypeNameFlag() ? FormatFlags.LogSuppressTypeNames : FormatFlags.DefaultCallerTypeFlags;
//         flags |= toConvert.HasTypeEndFlag() ? FormatFlags.SuppressOpening : FormatFlags.DefaultCallerTypeFlags;
//
//         return flags;
//     }
// }