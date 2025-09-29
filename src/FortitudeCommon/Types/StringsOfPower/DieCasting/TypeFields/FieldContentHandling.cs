namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

[Flags]
public enum FieldContentHandling
{
    DefaultForValueType         = 0x00_00_00_00
  , IncludeValueTypeDefaults    = 0x00_00_00_01
  , NoEscapeEncoding            = 0x00_00_00_02
  , EscapeEncodeContent         = 0x00_00_00_04
  , Base64EncodeContent         = 0x00_00_00_08
  , SimpleValueType             = 0x00_00_00_10
  , ContentAllowNumber          = 0x00_00_00_20
  , ContentAllowRawGraphNode    = 0x00_00_00_40
  , MustNotBeNull               = 0x00_00_00_80
  , MustNotBeEmpty              = 0x00_00_01_00
  , ContentAllowText            = 0x00_00_02_00
  , MustConvertToString         = 0x00_00_04_00
  , AsCollection                = 0x00_00_08_00
  , ContentMismatchViolationOn  = 0x00_00_10_00
  , ContentMismatchViolationOff = 0x00_00_20_00
  , SplitPreserveMultiLine      = 0x00_00_40_00
  , PrettyTreatAsCompact        = 0x00_00_80_00
  , PrettyNewLinePerElement     = 0x00_01_00_00
  , PrettyElementWrapAtWidth    = 0x00_02_00_00
  , TempAlwaysExclude           = 0x00_04_00_00
  , ExcludeWhenLogStyle         = 0x00_08_00_00
  , ExcludeWhenJsonStyle        = 0x00_10_00_00
  , ExcludeWhenYamlStyle        = 0x00_20_00_00 // Not implemented just reserving
  , ExcludeWhenXmlLStyle        = 0x00_40_00_00 // Not implemented just reserving
  , ExcludeWhenPretty           = 0x00_80_00_00
  , ExcludeWhenCompact          = 0x01_00_00_00
  , ViolationThrowsException    = 0x02_00_00_00
  , ViolationWritesAlert        = 0x04_00_00_00
  , ViolationDebuggerBreak      = 0x08_00_00_00
}
