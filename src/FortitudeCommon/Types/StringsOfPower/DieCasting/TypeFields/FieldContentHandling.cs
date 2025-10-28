using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

[Flags]
public enum FieldContentHandling : ulong
{
    // FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags
    DefaultCallerTypeFlags   = 0x_00_00
  , NoAutoAddCallerTypeFlags = 0x_00_01
  , EncodeBounds             = 0x_00_02
  , EncodeInnerContent       = 0x_00_04
  , EncodeAll                = 0x_00_06
  , EnsureFormattedDelimited = 0x_00_08
  , DisableAutoDelimiting    = 0x_00_10
  , AsStringContent          = 0x_00_20
  , SkipAllEncoding          = 0x_00_40
  , EncodeAsBase64           = 0x_00_80
  , AsciiEscapeEncoding      = 0x_01_00
  , JsonEncoding             = 0x_02_00
  , YamlEncoding             = 0x_04_00 // Not implemented just reserving
  , MlEncoding               = 0x_08_00 // Not implemented just reserving
  , EncodingMask             = 0x_0F_00
  , ReformatMultiLine        = 0x_10_00
  , UnsetEncodeBounds        = 0x_20_00
  , UnsetEncodeInnerContent  = 0x_40_00
  , NullBecomesEmpty         = 0x_80_00

    // Mold Additional
  , EnsureLogFormatting         = 0x00_00_00_01_00_00
  , EnsureJsonFormatting        = 0x00_00_00_02_00_00
  , EnsureYamlFormatting        = 0x00_00_00_04_00_00 // Not implemented just reserving  
  , EnsureMlFormatting          = 0x00_00_00_08_00_00 // Not implemented just reserving
  , EnsureCompact               = 0x00_00_00_10_00_00  
  , EnsurePretty                = 0x00_00_00_20_00_00  
  , FormattingMask              = 0x00_00_00_2F_00_00 
  , ExcludeWhenPretty           = 0x00_00_00_40_00_00
  , ExcludeWhenCompact          = 0x00_00_00_80_00_00
  , ExcludeWhenLogStyle         = 0x00_00_01_00_00_00
  , ExcludeWhenJsonStyle        = 0x00_00_02_00_00_00
  , ExcludeWhenYamlStyle        = 0x00_00_04_00_00_00 // Not implemented just reserving                                             
  , ExcludeWhenXmlLStyle        = 0x00_00_08_00_00_00 // Not implemented just reserving          
  , ExcludeMask                 = 0x00_00_0F_C0_00_00    
  , ContentAllowAnyValueType    = 0x00_00_10_00_00_00 
  , ContentAllowNumber          = 0x00_00_20_00_00_00 
  , ContentAllowRawGraphNode    = 0x00_01_00_00_00_00 
  , ContentAllowText            = 0x00_02_00_00_00_00 
  , ValidateValueType           = 0x00_04_00_00_00_00 
  , ValidateNumber              = 0x00_08_00_00_00_00 
  , ValidateNotNull             = 0x00_10_00_00_00_00 
  , ValidateNotEmpty            = 0x00_20_00_00_00_00 
  , AsCollection                = 0x00_40_00_00_00_00 
  , ContentMismatchViolationOn  = 0x00_80_00_00_00_00 
  , ContentMismatchViolationOff = 0x01_00_00_00_00_00 
  , PrettyTreatAsCompact        = 0x02_00_00_00_00_00 
  , PrettyNewLinePerElement     = 0x04_00_00_00_00_00 
  , PrettyElementWrapAtWidth    = 0x08_00_00_00_00_00 
  , TempAlwaysExclude           = 0x10_00_00_00_00_00
  , ViolationThrowsException    = 0x20_00_00_00_00_00
  , ViolationWritesAlert        = 0x40_00_00_00_00_00
  , ViolationDebuggerBreak      = 0x80_00_00_00_00_00
}

public static class FieldContentHandlingExtensions
{
    public const FieldContentHandling None = 0;

    public static bool HasNullBecomesEmptyFlag(this FieldContentHandling flags)             => (flags & NullBecomesEmpty) > 0;
    public static bool HasEnsureLogFormattingFlag(this FieldContentHandling flags)          => (flags & EnsureLogFormatting) > 0;
    public static bool HasEnsureJsonFormattingFlag(this FieldContentHandling flags)         => (flags & EnsureJsonFormatting) > 0;
    public static bool HasEnsureYamlFormattingFlag(this FieldContentHandling flags)         => (flags & EnsureYamlFormatting) > 0;
    public static bool HasEnsureMlFormattingFlag(this FieldContentHandling flags)           => (flags & EnsureMlFormatting) > 0;
    public static bool HasEnsureCompactFlag(this FieldContentHandling flags)                => (flags & EnsureCompact) > 0;
    public static bool HasEnsurePrettyFlag(this FieldContentHandling flags)                 => (flags & EnsurePretty) > 0;
    public static bool HasExcludeWhenLogStyleFlag(this FieldContentHandling flags)          => (flags & ExcludeWhenLogStyle) > 0;
    public static bool HasExcludeWhenJsonStyleFlag(this FieldContentHandling flags)         => (flags & ExcludeWhenJsonStyle) > 0;
    public static bool HasExcludeWhenYamlStyleFlag(this FieldContentHandling flags)         => (flags & ExcludeWhenYamlStyle) > 0;
    public static bool HasExcludeWhenXmlLStyleFlag(this FieldContentHandling flags)         => (flags & ExcludeWhenXmlLStyle) > 0;
    public static bool HasExcludeWhenPrettyFlag(this FieldContentHandling flags)            => (flags & ExcludeWhenPretty) > 0;
    public static bool HasExcludeWhenCompactFlag(this FieldContentHandling flags)           => (flags & ExcludeWhenCompact) > 0;
    public static bool HasDisableAddingAutoCallerTypeFlags(this FieldContentHandling flags) => (flags & NoAutoAddCallerTypeFlags) > 0;
    public static bool HasDisableAutoDelimiting(this FieldContentHandling flags)            => (flags & DisableAutoDelimiting) > 0;
    public static bool ShouldDelimit(this FieldContentHandling flags)                       => (flags & EnsureFormattedDelimited) > 0;

    public static StringStyle UpdateStringStyle(this FieldContentHandling flags, StringStyle existingStyle)
    {
        if (flags.HasEnsureLogFormattingFlag())
        {
            existingStyle &= ~StringStyle.Json;
            existingStyle |= StringStyle.Log;
        }
        if (flags.HasEnsureJsonFormattingFlag())
        {
            existingStyle &= ~StringStyle.Log;
            existingStyle |= StringStyle.Json;
        }
        if (flags.HasEnsureCompactFlag())
        {
            existingStyle &= ~StringStyle.Pretty;
            existingStyle |= StringStyle.Compact;
        }
        if (flags.HasEnsurePrettyFlag())
        {
            existingStyle &= ~StringStyle.Compact;
            existingStyle |= StringStyle.Pretty;
        }
        return existingStyle;
    }
    
    
}
