using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

[Flags]
public enum FormatFlags : ulong
{
    DefaultCallerTypeFlags   = 0x00_00_00
  , NoAutoAddCallerTypeFlags = 0x00_00_01
  , EncodeBounds             = 0x00_00_02
  , EncodeInnerContent       = 0x00_00_04
  , EncodeAll                = 0x00_00_06
  , EnsureFormattedDelimited = 0x00_00_08
  , NoItemSeparator          = 0x00_00_10
  , UseAltItemSeparator      = 0x00_00_20
  , NoItemPadding            = 0x00_00_40
  , UseAltItemPadding        = 0x00_00_80
  , ToggleEncodeAsBase64     = 0x00_01_00
  , AsciiEscapeEncoding      = 0x00_02_00 // minimal backslash escaping for console control chars 0-32 and 127 - 159
  , JsamlEncoding            = 0x00_04_00 // Json + Yaml => Jsaml
  , AngleMlEncoding          = 0x00_08_00 // Not implemented just reserving
  , CustomEncoding           = 0x00_10_00 // Not implemented just reserving
  , EncodingMask             = 0x00_1F_00
  , NoEncoding               = 0x00_1F_00
  , DisableAutoDelimiting    = 0x00_20_00
  , AsStringContent          = 0x00_40_00
  , AsValueContent           = 0x00_80_00
  , AsCollection             = 0x01_00_00
  , ReformatMultiLine        = 0x02_00_00
  , NullBecomesEmpty         = 0x04_00_00
  , OnOneLine                = 0x08_00_00

    // Mold Additional
  , ThisStartOnNewLine   = 0x00_10_00_00
  , NextEnsureNewLine    = 0x00_20_00_00
  , EachItemOnlyOneLine  = 0x00_40_00_00
  , NoRevisitCheck       = 0x00_80_00_00
  , NoFieldSeparator     = 0x01_00_00_00
  , UseAltFieldSeparator = 0x02_00_00_00
  , NoFieldPadding       = 0x04_00_00_00
  , NoWhitespacesToNext  = 0x04_00_00_00
  , UseAltFieldPadding   = 0x08_00_00_00
  , NextFieldOnSameLine  = 0x08_00_00_00

  , ToCompact            = 0x00_10_00_00_00
  , WhenCompact          = 0x00_10_00_00_00
  , ToPretty             = 0x00_20_00_00_00
  , WhenPretty           = 0x00_20_00_00_00
  , LayoutMask           = 0x00_30_00_00_00
  , WhenLogStyle         = 0x00_40_00_00_00
  , ToLogStyle           = 0x00_40_00_00_00
  , WhenJsonStyle        = 0x00_80_00_00_00
  , ToJsonStyle          = 0x00_80_00_00_00
  , WhenJsamlStyle       = 0x01_00_00_00_00 // Not implemented just reserving          
  , ToJsamlStyle         = 0x01_00_00_00_00 // Not implemented just reserving          
  , WhenAngleMlStyle     = 0x02_00_00_00_00 // Not implemented just reserving          
  , ToAngleMlStyle       = 0x02_00_00_00_00 // Not implemented just reserving          
  , WhenCustomStyle      = 0x04_00_00_00_00 // Not implemented just reserving - One custom style to implement Diagraph 
  , ToCustomStyle        = 0x04_00_00_00_00 // Not implemented just reserving - One custom style to implement Diagraph 
  , StyleMask            = 0x07_C0_00_00_00
  , AttemptStyleSwitch   = 0x08_00_00_00_00
  , Exclude         = 0x10_00_00_00_00
  , OnlyInclude     = 0x20_00_00_00_00
  , InvertIncludeExclude = 0x40_00_00_00_00
  , IsFieldName = 0x80_00_00_00_00
    
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
  , ContentMismatchViolationOn  = 0x10_00_00_00_00_00_00
  , ContentMismatchViolationOff = 0x20_00_00_00_00_00_00
  , PrettyWrapAtLineWidth       = 0x40_00_00_00_00_00_00
  , PrettyWrapAtContentWidth    = 0x80_00_00_00_00_00_00

  , ViolationThrowsException   = 0x01_00_00_00_00_00_00_00
  , ViolationWritesAlert       = 0x02_00_00_00_00_00_00_00
  , ViolationDebuggerBreak     = 0x04_00_00_00_00_00_00_00
  , SuppressOpening            = 0x08_00_00_00_00_00_00_00
  , SuppressClosing            = 0x10_00_00_00_00_00_00_00
  , AsEmbeddedContent          = 0x18_00_01_00_00_00_00_00  
  , DisableFieldNameDelimiting = 0x20_00_00_00_00_00_00_00
}

public static class FieldContentHandlingExtensions
{
    public const FormatFlags None = 0;

    public static bool HasEncodeBounds(this FormatFlags flags)                     => (flags & EncodeBounds) > 0;
    public static bool HasEncodeAll(this FormatFlags flags)                        => (flags & EncodeAll) == EncodeAll;
    public static bool HasDisableAddingAutoCallerTypeFlags(this FormatFlags flags) => (flags & NoAutoAddCallerTypeFlags) > 0;
    public static bool HasEnsureFormattedDelimitedFlag(this FormatFlags flags)     => (flags & EnsureFormattedDelimited) > 0;
    public static bool HasAsStringContentFlag(this FormatFlags flags)              => (flags & AsStringContent) > 0;
    public static bool DoesNotHaveAsStringContentFlag(this FormatFlags flags)      => (flags & AsStringContent) == 0;
    public static bool HasAsValueContentFlag(this FormatFlags flags)               => (flags & AsValueContent) > 0;
    public static bool DoesNotHaveAsValueContentFlag(this FormatFlags flags)       => (flags & AsValueContent) == 0;

    public static bool HasContentTreatmentFlags(this FormatFlags flags) => flags.HasAsStringContentFlag() || flags.HasAsValueContentFlag();
    public static bool TreatCharArrayAsString(this FormatFlags flags)   => flags.HasContentTreatmentFlags();
    
    public static bool TreatCharsAsEncodedString(this FormatFlags flags)   => 
        flags.HasContentTreatmentFlags() && !flags.HasReformatMultiLineFlag();

    public static bool HasReformatMultiLineFlag(this FormatFlags flags) => (flags & ReformatMultiLine) > 0;
    public static bool DoesNotHaveReformatMultiLineFlag(this FormatFlags flags) => (flags & ReformatMultiLine) == 0;
    public static bool HasOnOneLine(this FormatFlags flags)             => (flags & OnOneLine) > 0;
    public static bool DoesNotHaveOnOneLine(this FormatFlags flags)             => (flags & OnOneLine) == 0;
    public static bool HasJsamlEncodingFlag(this FormatFlags flags)     => (flags & JsamlEncoding) > 0;
    public static bool HasNoItemSeparatorFlag(this FormatFlags flags)   => (flags & NoItemSeparator) > 0;
    public static bool HasNoItemPaddingFlag(this FormatFlags flags)     => (flags & NoItemPadding) > 0;
    public static bool ShouldAddItemSeparator(this FormatFlags flags)   => (flags & NoItemSeparator) == 0;
    public static bool UseMainItemSeparator(this FormatFlags flags)     => (flags & UseAltItemSeparator) == 0;
    public static bool ShouldAddItemPadding(this FormatFlags flags)     => (flags & NoItemPadding) == 0;
    public static bool UseMainItemPadding(this FormatFlags flags)       => (flags & UseAltItemSeparator) == 0;

    public static bool IsUnspecifiedContent(this FormatFlags flags) =>
        !flags.IsSpecifiedContent();

    public static bool IsSpecifiedContent(this FormatFlags flags) =>
        flags.HasAsStringContentFlag() || flags.HasAsValueContentFlag();

    public static bool HasNoWhitespacesToNextFlag(this FormatFlags flags) => (flags & NoWhitespacesToNext) > 0;
    public static bool HasNextOnSameLineFlag(this FormatFlags flags)      => (flags & NextFieldOnSameLine) > 0;
    public static bool HasNextEnsureNewLineFlag(this FormatFlags flags)   => (flags & NextEnsureNewLine) > 0;

    public static bool CanAddNewLine(this FormatFlags flags) =>
        (!flags.HasNoWhitespacesToNextFlag() || flags.HasNextEnsureNewLineFlag()) 
     && flags.DoesNotHaveEnsureCompactLayout() && flags.DoesNotHaveOnOneLine();

    public static bool HasNoFieldSeparatorFlag(this FormatFlags flags) => (flags & NoFieldSeparator) > 0;
    public static bool HasNoFieldPaddingFlag(this FormatFlags flags)   => (flags & NoFieldPadding) > 0;
    public static bool ShouldAddFieldSeparator(this FormatFlags flags) => (flags & NoFieldSeparator) == 0;
    public static bool UseMainFieldSeparator(this FormatFlags flags)   => (flags & UseAltFieldSeparator) == 0;
    public static bool ShouldAddFieldPadding(this FormatFlags flags)   => (flags & NoFieldPadding) == 0;
    public static bool UseMainFieldPadding(this FormatFlags flags)     => (flags & UseAltFieldSeparator) == 0;

    public static bool HasNullBecomesEmptyFlag(this FormatFlags flags) => (flags & NullBecomesEmpty) > 0;

    public static bool ShowSwitchToLogFormatting(this FormatFlags flags) =>
        (flags & (ToLogStyle | AttemptStyleSwitch)) == (ToLogStyle | AttemptStyleSwitch);

    public static bool ShouldSwitchToJsonFormatting(this FormatFlags flags) =>
        (flags & (ToJsonStyle | AttemptStyleSwitch)) == (ToJsonStyle | AttemptStyleSwitch);

    public static bool ShouldSwitchToJsamlFormatting(this FormatFlags flags) =>
        (flags & (ToJsamlStyle | AttemptStyleSwitch)) == (ToJsamlStyle | AttemptStyleSwitch);

    public static bool ShouldSwitchToAngleMlFormatting(this FormatFlags flags) =>
        (flags & (ToAngleMlStyle | AttemptStyleSwitch)) == (ToAngleMlStyle | AttemptStyleSwitch);

    public static bool ShouldSwitchToCustomFormatting(this FormatFlags flags) =>
        (flags & (ToCustomStyle | AttemptStyleSwitch)) == (ToCustomStyle | AttemptStyleSwitch);

    public static bool ShouldSwitchToCompactLayout(this FormatFlags flags) =>
        (flags & (ToCompact | AttemptStyleSwitch)) == (ToCompact | AttemptStyleSwitch);

    public static bool DoesNotHaveEnsureCompactLayout(this FormatFlags flags) =>
        (flags & (ToCompact | AttemptStyleSwitch)) != (ToCompact | AttemptStyleSwitch);

    public static bool ShouldSwitchToPrettyLayout(this FormatFlags flags) =>
        (flags & (ToPretty | AttemptStyleSwitch)) == (ToPretty | AttemptStyleSwitch);

    public static bool DoesNotHaveEnsurePrettyLayout(this FormatFlags flags) =>
        (flags & (ToPretty | AttemptStyleSwitch)) != (ToPretty | AttemptStyleSwitch);

    public static bool HasExcludeWhenLogStyle(this FormatFlags flags)  => (flags & (ToPretty | Exclude)) == (ToPretty | Exclude);
    public static bool HasExcludeWhenJsonStyle(this FormatFlags flags) => (flags & (WhenJsonStyle | Exclude)) == (WhenJsonStyle | Exclude);
    public static bool HasExcludeWhenJsamlStyle(this FormatFlags flags) => (flags & (WhenJsamlStyle | Exclude)) == (WhenJsamlStyle | Exclude);
    public static bool HasExcludeWhenAngleMlStyle(this FormatFlags flags) => (flags & (WhenAngleMlStyle | Exclude)) == (WhenAngleMlStyle | Exclude);
    public static bool HasExcludeWhenCustomStyle(this FormatFlags flags) => (flags & (WhenCustomStyle | Exclude)) == (WhenCustomStyle | Exclude);

    public static bool HasExcludeWhenPrettyLayout(this FormatFlags flags)            => (flags & (WhenPretty | Exclude)) == (WhenPretty | Exclude);
    public static bool HasExcludeWhenCompactLayout(this FormatFlags flags)           => (flags & (WhenCompact | Exclude)) == (WhenCompact | Exclude);
    public static bool HasDisableAutoDelimiting(this FormatFlags flags)            => (flags & DisableAutoDelimiting) > 0;
    public static bool ShouldDelimit(this FormatFlags flags)                       => (flags & EnsureFormattedDelimited) > 0;
    
    public static bool HasEachItemOnlyOneLineFlag(this FormatFlags flags)                => (flags & EachItemOnlyOneLine) > 0;
    public static bool HasNoRevisitCheck(this FormatFlags flags)                         => (flags & NoRevisitCheck) > 0;
    public static bool HasSuppressTypeNamesFlag(this FormatFlags flags)                  => (flags & LogSuppressTypeNames) > 0;
    public static bool DoesNotHaveLogSuppressTypeNamesFlag(this FormatFlags flags)       => (flags & LogSuppressTypeNames) == 0;
    public static bool HasAddTypeNameFieldFlag(this FormatFlags flags)                   => (flags & AddTypeNameField) > 0;
    public static bool DoesNotHaveAddTypeNameFieldFlag(this FormatFlags flags)           => (flags & AddTypeNameField) == 0;
    public static bool HasAsCollectionFlag(this FormatFlags flags)                       => (flags & AsCollection) > 0;
    public static bool DoesNotHaveAsCollectionFlag(this FormatFlags flags)               => (flags & AsCollection) == 0;
    public static bool HasIsFieldNameFlag(this FormatFlags flags)                        => (flags & IsFieldName) > 0;
    public static bool DoesNotHaveIsFieldNameFlag(this FormatFlags flags)                => (flags & IsFieldName) == 0;
    public static bool HasDisableFieldNameDelimitingFlag(this FormatFlags flags)         => (flags & DisableFieldNameDelimiting) > 0;
    public static bool DoesNotHaveDisableFieldNameDelimitingFlag(this FormatFlags flags) => (flags & DisableFieldNameDelimiting) == 0;
    public static bool HasSuppressOpening(this FormatFlags flags)                        => (flags & SuppressOpening) > 0;
    public static bool DoesNotHaveSuppressOpening(this FormatFlags flags)                => (flags & SuppressOpening) == 0;
    public static bool HasSuppressClosing(this FormatFlags flags)                        => (flags & SuppressClosing) > 0;
    public static bool DoesNotHaveSuppressClosing(this FormatFlags flags)                => (flags & SuppressClosing) == 0;
    public static bool HasAsEmbeddedContentFlags(this FormatFlags flags)                 => (flags & AsEmbeddedContent) == AsEmbeddedContent;
    public static bool DoesNotHaveAsEmbeddedContentFlags(this FormatFlags flags)         => (flags & AsEmbeddedContent) != AsEmbeddedContent;

    public static StringStyle UpdateStringStyle(this FormatFlags flags, StringStyle existingStyle)
    {
        if (flags.ShowSwitchToLogFormatting())
        {
            existingStyle &= ~StringStyle.Json;
            existingStyle |= StringStyle.Log;
        }
        if (flags.ShouldSwitchToJsonFormatting())
        {
            existingStyle &= ~StringStyle.Log;
            existingStyle |= StringStyle.Json;
        }
        if (flags.ShouldSwitchToCompactLayout())
        {
            existingStyle &= ~StringStyle.Pretty;
            existingStyle |= StringStyle.Compact;
        }
        if (flags.ShouldSwitchToPrettyLayout())
        {
            existingStyle &= ~StringStyle.Compact;
            existingStyle |= StringStyle.Pretty;
        }
        return existingStyle;
    }

    public static FormatFlags MoldInheritFlags(this FormatFlags moldCreatedFlags) =>
        moldCreatedFlags & (IsFieldName | DisableFieldNameDelimiting | OnOneLine | DisableAutoDelimiting | AsStringContent | AsValueContent);
}
