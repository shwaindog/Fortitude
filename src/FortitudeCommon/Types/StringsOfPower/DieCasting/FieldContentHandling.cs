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
  , AsValueContent           = 0x_00_40
  , ReformatMultiLine        = 0x_00_80
  , NoItemSeparator          = 0x_01_00
  , UseAltItemSeparator      = 0x_02_00
  , NoItemPadding            = 0x_04_00
  , UseAltItemPadding        = 0x_08_00
  , ToggleEncodeAsBase64     = 0x_10_00
  , AsciiEscapeEncoding      = 0x_20_00 // minimal backslash escaping for console control chars 0-32 and 127 - 159
  , JsamlEncoding            = 0x_40_00 // Json + Yaml => Jsaml
  , MlEncoding               = 0x_80_00 // Not implemented just reserving
  , EncodingMask             = 0x_F0_00

    // Mold Additional
  , ThisStartOnNewLine      = 0x00_01_00_00
  , NextEnsureNewLine       = 0x00_02_00_00
  , NullBecomesEmpty        = 0x00_04_00_00
  , EachItemOnlyOneLine     = 0x00_08_00_00
  , NoFieldSeparator        = 0x00_10_00_00
  , UseAltFieldSeparator    = 0x00_20_00_00
  , NoFieldPadding          = 0x00_40_00_00
  , NoWhitespacesToNext     = 0x00_40_00_00
  , UseAltFieldPadding      = 0x00_80_00_00
  , NextFieldOnSameLine     = 0x00_80_00_00
  , EnsureLogFormatting     = 0x01_00_00_00
  , EnsureJsonFormatting    = 0x02_00_00_00
  , EnsureYamlFormatting    = 0x04_00_00_00 // Not implemented just reserving  
  , EnsureMlFormatting      = 0x08_00_00_00 // Not implemented just reserving
  , EnsureCustomFormatting  = 0x10_00_00_00 // Not implemented just reserving -  One custom style to implement Diagraph
  , EnsureCompact           = 0x20_00_00_00
  , OnOneLine               = 0x20_00_00_00
  , EnsurePretty            = 0x40_00_00_00
  , FormattingMask          = 0x7F_00_00_00
  , AlwaysExclude           = 0x80_00_00_00

  , ExcludeWhenPretty            = 0x01_00_00_00_00
  , IncludeOnlyWhenCompact       = 0x01_00_00_00_00
  , IncludeOnlyWhenPretty        = 0x02_00_00_00_00
  , ExcludeWhenCompact           = 0x02_00_00_00_00
  , ExcludeWhenLogStyle          = 0x04_00_00_00_00
  , IncludeOnlyWhenLogStyle      = 0x78_00_00_00_00
  , ExcludeWhenJsonStyle         = 0x08_00_00_00_00
  , IncludeOnlyWhenJsonStyle     = 0x74_00_00_00_00
  , ExcludeWhenYamlStyle         = 0x10_00_00_00_00 // Not implemented just reserving                                             
  , IncludeOnlyWhenYamlStyle     = 0x6C_00_00_00_00 // Not implemented just reserving          
  , ExcludeWhenMlLStyle          = 0x20_00_00_00_00 // Not implemented just reserving          
  , IncludeOnlyWhenMlStyle       = 0x5C_00_00_00_00 // Not implemented just reserving          
  , ExcludeWhenCustomStyle       = 0x40_00_00_00_00 // Not implemented just reserving - One custom style to implement Diagraph
  , IncludeOnlyWhenCustomStyle   = 0x3C_00_00_00_00 // Not implemented just reserving 
  , ExcludeMask                  = 0x7F_80_00_00_00
  , NoRevisitCheck               = 0x80_00_00_00_00

  , LogSuppressTypeNames        = 0x00_01_00_00_00_00_00
  , AddTypeNameField            = 0x00_02_00_00_00_00_00
  , AddNamespace                = 0x00_04_00_00_00_00_00
  , ContentAllowAnyValueType    = 0x00_08_00_00_00_00_00
  , ContentAllowNumber          = 0x00_10_00_00_00_00_00
  , ContentAllowRawGraphNode    = 0x00_20_00_00_00_00_00
  , ContentAllowText            = 0x00_40_00_00_00_00_00
  , ValidateValueType           = 0x00_80_00_00_00_00_00
  , ValidateNumber              = 0x01_00_00_00_00_00_00
  , ValidateNotNull             = 0x02_00_00_00_00_00_00
  , ValidateNotEmpty            = 0x04_00_00_00_00_00_00
  , AsCollection                = 0x08_00_00_00_00_00_00
  , ContentMismatchViolationOn  = 0x10_00_00_00_00_00_00
  , ContentMismatchViolationOff = 0x20_00_00_00_00_00_00
  , PrettyWrapAtLineWidth       = 0x40_00_00_00_00_00_00
  , PrettyWrapAtContentWidth    = 0x80_00_00_00_00_00_00

  , ViolationThrowsException = 0x01_00_00_00_00_00_00_00
  , ViolationWritesAlert     = 0x02_00_00_00_00_00_00_00
  , ViolationDebuggerBreak   = 0x04_00_00_00_00_00_00_00
  , SuppressOpening          = 0x08_00_00_00_00_00_00_00
  , SuppressClosing          = 0x10_00_00_00_00_00_00_00
  , AsEmbeddedContent        = 0x18_00_01_00_00_00_00_00
  , UseAlternateSeparator    = 0x20_00_00_00_00_00_00_00
}

public static class FieldContentHandlingExtensions
{
    public const FieldContentHandling None = 0;

    public static bool HasDisableAddingAutoCallerTypeFlags(this FieldContentHandling flags) => (flags & NoAutoAddCallerTypeFlags) > 0;
    public static bool HasEnsureFormattedDelimitedFlag(this FieldContentHandling flags)     => (flags & EnsureFormattedDelimited) > 0;
    public static bool HasAsStringContentFlag(this FieldContentHandling flags)              => (flags & AsStringContent) > 0;
    public static bool DoesNotHaveAsStringContentFlag(this FieldContentHandling flags)      => (flags & AsStringContent) == 0;
    public static bool HasAsValueContentFlag(this FieldContentHandling flags)               => (flags & AsValueContent) > 0;
    public static bool DoesNotHaveAsValueContentFlag(this FieldContentHandling flags)       => (flags & AsValueContent) == 0;
    
    public static bool HasReformatMultiLineFlag(this FieldContentHandling flags)    => (flags & ReformatMultiLine) > 0; 
    public static bool HasJsamlEncodingFlag(this FieldContentHandling flags)    => (flags & JsamlEncoding) > 0; 
    public static bool HasNoItemSeparatorFlag(this FieldContentHandling flags) => (flags & NoItemSeparator) > 0; 
    public static bool HasNoItemPaddingFlag(this FieldContentHandling flags)   => (flags & NoItemPadding) > 0; 
    public static bool ShouldAddItemSeparator(this FieldContentHandling flags) => (flags & NoItemSeparator) == 0; 
    public static bool UseMainItemSeparator(this FieldContentHandling flags)   => (flags & UseAltItemSeparator) == 0; 
    public static bool ShouldAddItemPadding(this FieldContentHandling flags)   => (flags & NoItemPadding) == 0; 
    public static bool UseMainItemPadding(this FieldContentHandling flags)     => (flags & UseAltItemSeparator) == 0; 

    public static bool IsUnspecifiedContent(this FieldContentHandling flags) =>
        !flags.IsSpecifiedContent();

    public static bool IsSpecifiedContent(this FieldContentHandling flags) =>
        flags.HasAsStringContentFlag() || flags.HasAsValueContentFlag();

    public static bool HasNoWhitespacesToNextFlag(this FieldContentHandling flags) => (flags & NoWhitespacesToNext) > 0;
    public static bool HasNextOnSameLineFlag(this FieldContentHandling flags)      => (flags & NextFieldOnSameLine) > 0;
    public static bool HasNextEnsureNewLineFlag(this FieldContentHandling flags)   => (flags & NextEnsureNewLine) > 0;

    public static bool CanAddNewLine(this FieldContentHandling flags) =>
        (!flags.HasNoWhitespacesToNextFlag() || flags.HasNextEnsureNewLineFlag()) && flags.DoesNotHaveEnsureCompactFlag();
    
    public static bool HasNoFieldSeparatorFlag(this FieldContentHandling flags) => (flags & NoFieldSeparator) > 0; 
    public static bool HasNoFieldPaddingFlag(this FieldContentHandling flags)   => (flags & NoFieldPadding) > 0; 
    public static bool ShouldAddFieldSeparator(this FieldContentHandling flags) => (flags & NoFieldSeparator) == 0; 
    public static bool UseMainFieldSeparator(this FieldContentHandling flags)   => (flags & UseAltFieldSeparator) == 0; 
    public static bool ShouldAddFieldPadding(this FieldContentHandling flags)   => (flags & NoFieldPadding) == 0; 
    public static bool UseMainFieldPadding(this FieldContentHandling flags)     => (flags & UseAltFieldSeparator) == 0; 

    public static bool HasNullBecomesEmptyFlag(this FieldContentHandling flags)             => (flags & NullBecomesEmpty) > 0;
    public static bool HasEnsureLogFormattingFlag(this FieldContentHandling flags)          => (flags & EnsureLogFormatting) > 0;
    public static bool HasEnsureJsonFormattingFlag(this FieldContentHandling flags)         => (flags & EnsureJsonFormatting) > 0;
    public static bool HasEnsureYamlFormattingFlag(this FieldContentHandling flags)         => (flags & EnsureYamlFormatting) > 0;
    public static bool HasEnsureMlFormattingFlag(this FieldContentHandling flags)           => (flags & EnsureMlFormatting) > 0;
    public static bool HasEnsureCompactFlag(this FieldContentHandling flags)                => (flags & EnsureCompact) > 0;
    public static bool DoesNotHaveEnsureCompactFlag(this FieldContentHandling flags)        => (flags & EnsureCompact) == 0;
    public static bool HasEnsurePrettyFlag(this FieldContentHandling flags)                 => (flags & EnsurePretty) > 0;
    public static bool HasExcludeWhenLogStyleFlag(this FieldContentHandling flags)          => (flags & ExcludeWhenLogStyle) > 0;
    public static bool HasExcludeWhenJsonStyleFlag(this FieldContentHandling flags)         => (flags & ExcludeWhenJsonStyle) > 0;
    public static bool HasExcludeWhenYamlStyleFlag(this FieldContentHandling flags)         => (flags & ExcludeWhenYamlStyle) > 0;
    public static bool HasExcludeWhenXmlLStyleFlag(this FieldContentHandling flags)         => (flags & ExcludeWhenMlLStyle) > 0;
    public static bool HasExcludeWhenPrettyFlag(this FieldContentHandling flags)            => (flags & ExcludeWhenPretty) > 0;
    public static bool HasExcludeWhenCompactFlag(this FieldContentHandling flags)           => (flags & ExcludeWhenCompact) > 0;
    public static bool HasDisableAutoDelimiting(this FieldContentHandling flags)            => (flags & DisableAutoDelimiting) > 0;
    public static bool ShouldDelimit(this FieldContentHandling flags)                       => (flags & EnsureFormattedDelimited) > 0;
    public static bool HasEachItemOnlyOneLineFlag(this FieldContentHandling flags)          => (flags & EachItemOnlyOneLine) > 0;
    public static bool HasSuppressTypeNamesFlag(this FieldContentHandling flags) => (flags & LogSuppressTypeNames) > 0;
    public static bool DoesNotHaveLogSuppressTypeNamesFlag(this FieldContentHandling flags) => (flags & LogSuppressTypeNames) == 0;
    public static bool HasAsCollectionFlag(this FieldContentHandling flags)                 => (flags & AsCollection) > 0;
    public static bool DoesNotHaveAsCollectionFlag(this FieldContentHandling flags)         => (flags & AsCollection) == 0;
    public static bool HasSuppressOpening (this FieldContentHandling flags)                 => (flags & SuppressOpening) > 0;
    public static bool DoesNotHaveSuppressOpening (this FieldContentHandling flags)         => (flags & SuppressOpening) == 0;
    public static bool HasSuppressClosing (this FieldContentHandling flags)                 => (flags & SuppressClosing) > 0;
    public static bool DoesNotHaveSuppressClosing (this FieldContentHandling flags)         => (flags & SuppressClosing) == 0;
    public static bool HasAsEmbeddedContentFlags(this FieldContentHandling flags)           => (flags & AsEmbeddedContent) == AsEmbeddedContent;
    public static bool DoesNotHaveAsEmbeddedContentFlags(this FieldContentHandling flags)   => (flags & AsEmbeddedContent) != AsEmbeddedContent;

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
