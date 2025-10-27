namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

[Flags]
public enum FieldContentHandling : ulong
{
    // FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags
    DefaultCallerTypeFlags               = 0x_00_00
  , NoCallerTypeFlags                 = 0x_00_01
  , EncodeSuffixLast                  = 0x_00_02
  , EncodeSuffixPenultimate           = 0x_00_04
  , EncodeContent                     = 0x_00_08
  , EncodePrefixPreUltimate           = 0x_00_10
  , EncodePrefixFirst                 = 0x_00_20
  , EncodeEntireFormatted             = 0x_00_2E
  , EncodeAllButPrefixFirstSuffixLast = 0x_00_1C
  , EnsureFormattedDelimited          = 0x_00_40
  , DisableAutoDelimiting             = 0x_00_80
  , AsStringContent                   = 0x_01_00
  , SkipAllEncoding                   = 0x_02_00
  , EncodeAsBase64                    = 0x_04_00
  , AsciiEscapeEncoding               = 0x_08_00
  , JsonEncoding                      = 0x_10_00
  , YamlEncoding                      = 0x_20_00 // Not implemented just reserving
  , MlEncoding                        = 0x_40_00 // Not implemented just reserving
  , ReformatMultiLine                 = 0x_80_00

    // Mold Additional
  , UnsetEncodeSuffixLast         = 0x00_00_00_01_00_00
  , UnsetEncodeSuffixPenUltimate  = 0x00_00_00_02_00_00
  , UnsetEncodeContent            = 0x00_00_00_04_00_00
  , UnsetEncodePrefixPreUltimate  = 0x00_00_00_08_00_00
  , UnsetEncodePrefixFirst        = 0x00_00_00_10_00_00
  , UnsetEncodeEntireFormatted    = 0x00_00_00_1F_00_00
  , UnsetEscapeEncodeContent      = 0x00_00_00_20_00_00
  , UnsetEnsureFormattedDelimited = 0x00_00_00_40_00_00
  , ContentAllowAnyValueType      = 0x00_00_01_00_00_00
  , ContentAllowNumber            = 0x00_00_02_00_00_00
  , ContentAllowRawGraphNode      = 0x00_00_04_00_00_00
  , ContentAllowText              = 0x00_00_08_00_00_00
  , ValidateValueType             = 0x00_00_10_00_00_00
  , ValidateNumber                = 0x00_00_20_00_00_00
  , ValidateNotNull               = 0x00_00_40_00_00_00
  , ValidateNotEmpty              = 0x00_00_80_00_00_00
  , AsCollection                  = 0x00_01_00_00_00_00
  , ContentMismatchViolationOn    = 0x00_02_00_00_00_00
  , ContentMismatchViolationOff   = 0x00_04_00_00_00_00
  , PrettyTreatAsCompact          = 0x00_08_00_00_00_00
  , PrettyNewLinePerElement       = 0x00_10_00_00_00_00
  , PrettyElementWrapAtWidth      = 0x00_20_00_00_00_00
  , TempAlwaysExclude             = 0x00_40_00_00_00_00
  , ExcludeWhenLogStyle           = 0x00_80_00_00_00_00 // Not implemented just reserving       
  , ExcludeWhenJsonStyle          = 0x01_00_00_00_00_00 // Not implemented just reserving       
  , ExcludeWhenYamlStyle          = 0x02_00_00_00_00_00
  , ExcludeWhenXmlLStyle          = 0x04_00_00_00_00_00
  , ExcludeWhenPretty             = 0x08_00_00_00_00_00
  , ExcludeWhenCompact            = 0x10_00_00_00_00_00
  , ViolationThrowsException      = 0x20_00_00_00_00_00
  , ViolationWritesAlert          = 0x40_00_00_00_00_00
  , ViolationDebuggerBreak        = 0x80_00_00_00_00_00
}

public static class FieldContentHandlingExtensions
{
    public const FieldContentHandling None = 0; 
     
    public static bool HasDisableAddingAutoCallerTypeFlags(this FieldContentHandling flags) => (flags & FieldContentHandling.NoCallerTypeFlags) > 0;
    
    public static bool HasDisableAutoDelimiting(this FieldContentHandling flags)    => 
        (flags & FieldContentHandling.DisableAutoDelimiting) > 0;
    
    public static bool ShouldDelimit(this FieldContentHandling flags)    => 
        (flags & FieldContentHandling.EnsureFormattedDelimited) > 0;
} 
