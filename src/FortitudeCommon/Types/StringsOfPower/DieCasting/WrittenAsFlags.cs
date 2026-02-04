// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

[Flags]
public enum WrittenAsFlags : ushort
{
    Empty                       = 0
  , AsNull                      = 1
  , AsString                    = 2
  , AsValue                     = 4
  , AsSimple                    = 8
  , AsCollection                = 16
  , AsMapCollection             = 32
  , AsComplex                   = 64
  , UpgradedToComplex           = 128
  , WithInstanceId              = 256
  , WithReferenceToInstanceId   = 512
  , WithDepthVmemAddress        = 1024
  , WithClippedDepthSuppression = 2048
  , RemovedDueDepthSuppression  = 4096
}

public static class WrittenAsFlagsExtensions
{
    public static bool IsEmpty(this WrittenAsFlags flags)                  => flags == Empty;
    public static bool HasAsNullFLag(this WrittenAsFlags flags)            => (flags & AsNull) > 0;
    public static bool HasAsStringFlag(this WrittenAsFlags flags)          => (flags & AsString) > 0;
    public static bool HasAsValueFlag(this WrittenAsFlags flags)           => (flags & AsValue) > 0;
    public static bool HasAsSimpleFlag(this WrittenAsFlags flags)          => (flags & AsSimple) > 0;
    public static bool HasAsCollectionFlag(this WrittenAsFlags flags)      => (flags & AsCollection) > 0;
    public static bool HasAsMapCollectionFlag(this WrittenAsFlags flags)   => (flags & AsMapCollection) > 0;
    public static bool HasAsComplexFlag(this WrittenAsFlags flags)         => (flags & AsComplex) > 0;
    public static bool HasUpgradedToComplexFlag(this WrittenAsFlags flags) => (flags & UpgradedToComplex) > 0;

    public static bool SupportsMultipleFields(this WrittenAsFlags flags) =>
        flags.HasAsComplexFlag() || flags.HasAsMapCollectionFlag();

    public static WrittenAsFlags WrittenAsFromFirstCharacter(this char firstChar) =>
        firstChar switch
        {
            '"' => AsString
          , '[' => AsCollection
          , '{' => AsComplex
          , _   => AsValue
        };

    public static WrittenAsFlags WrittenAsFromFirstCharacters(this IStringBuilder sb, int firstCharIndex, GraphTrackingBuilder gb)
    {
        if (sb.Length <= firstCharIndex) return Empty;
        var length                  = Math.Clamp(sb.Length - firstCharIndex, 0, Math.Max(0, sb.Length - firstCharIndex));
        var layoutEncoderType       = gb.GraphEncoder.LayoutEncoder.Type;
        var parentLayoutEncoderType = gb.GraphEncoder.LayoutEncoder.LayoutEncoder.Type;
        switch (layoutEncoderType)
        {
            case EncodingType.JsonEncoding:
                switch (parentLayoutEncoderType)
                {
                    case EncodingType.PassThrough:
                        if (length >= 6
                         && sb[firstCharIndex] == '\\'
                         && sb[firstCharIndex + 1] == 'u'
                         && sb[firstCharIndex + 2] == '0'
                         && sb[firstCharIndex + 3] == '0'
                         && sb[firstCharIndex + 4] == '2'
                         && sb[firstCharIndex + 5] == '2') { return AsString; }
                        break;
                    case EncodingType.JsonEncoding:
                        if (length >= 7
                         && sb[firstCharIndex] == '\\'
                         && sb[firstCharIndex + 1] == '\\'
                         && sb[firstCharIndex + 2] == 'u'
                         && sb[firstCharIndex + 3] == '0'
                         && sb[firstCharIndex + 4] == '0'
                         && sb[firstCharIndex + 5] == '2'
                         && sb[firstCharIndex + 6] == '2') { return AsString; }
                        break;
                }
                return sb[firstCharIndex].WrittenAsFromFirstCharacter();
            case EncodingType.PassThrough:
            default:
                return sb[firstCharIndex].WrittenAsFromFirstCharacter();
        }
        ;
    }
}
