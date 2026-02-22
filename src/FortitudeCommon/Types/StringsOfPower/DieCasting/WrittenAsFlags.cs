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
    Empty                       = 0x00_00
  , AsNull                      = 0x00_01
  , AsRaw                       = 0x00_02
  , AsString                    = 0x00_04
  , AsValue                     = 0x00_08
  , AsContent                   = 0x00_10
  , AsObject                    = 0x00_20
  , AsCollection                = 0x00_40
  , AsMapCollection             = 0x00_80
  , AsSimple                    = 0x01_00
  , AsComplex                   = 0x02_00
  , UpgradedToComplex           = 0x04_00
  , WithInstanceId              = 0x08_00
  , WithReferenceToInstanceId   = 0x10_00
  , WithDepthVmemAddress        = 0x20_00
  , WithClippedDepthSuppression = 0x40_00
  , RemovedDueDepthSuppression  = 0x80_00
}

public static class WrittenAsFlagsExtensions
{
    public static bool IsEmpty(this WrittenAsFlags flags)                  => flags == Empty;
    public static bool HasAsNullFLag(this WrittenAsFlags flags)            => (flags & AsNull) > 0;
    public static bool HasAsRawFlag(this WrittenAsFlags flags)                    => (flags & AsRaw) > 0;
    public static bool HasAsStringFlag(this WrittenAsFlags flags)          => (flags & AsString) > 0;
    public static bool HasAsValueFlag(this WrittenAsFlags flags)           => (flags & AsValue) > 0;
    public static bool HasAsContentFlag(this WrittenAsFlags flags)          => (flags & AsContent) > 0;
    public static bool HasAsCollectionFlag(this WrittenAsFlags flags)      => (flags & AsCollection) > 0;
    public static bool HasAsMapCollectionFlag(this WrittenAsFlags flags)   => (flags & AsMapCollection) > 0;
    public static bool HasAsSimpleFlag(this WrittenAsFlags flags)         => (flags & AsSimple) > 0;
    public static bool HasAsComplexFlag(this WrittenAsFlags flags)         => (flags & AsComplex) > 0;
    public static bool HasUpgradedToComplexFlag(this WrittenAsFlags flags) => (flags & UpgradedToComplex) > 0;

    public static bool HasAllOf(this WrittenAsFlags flags, WrittenAsFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this WrittenAsFlags flags, WrittenAsFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this WrittenAsFlags flags, WrittenAsFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this WrittenAsFlags flags, WrittenAsFlags checkAllFound)   => flags == checkAllFound;
    
    public static WrittenAsFlags ToMultiFieldEquivalent(this WrittenAsFlags value) =>
        value switch
        {
           _ when value.HasAsSimpleFlag() => (value & ~AsSimple) | AsComplex
          , _                            => value
        };
    
    public static WrittenAsFlags ToNoFieldEquivalent(this WrittenAsFlags value) =>
        value switch
        {
            _ when value.HasAsComplexFlag() => (value & ~AsComplex) | AsSimple
          , _                           => value
        };

    public static bool NoFieldNames(this WrittenAsFlags value) =>
        value.HasNoneOf(AsComplex | AsMapCollection) || value.HasAnyOf(AsSimple | AsRaw | AsNull);

    public static bool SupportsMultipleFields(this WrittenAsFlags flags) =>
        flags.HasAnyOf(AsComplex | AsMapCollection);

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
