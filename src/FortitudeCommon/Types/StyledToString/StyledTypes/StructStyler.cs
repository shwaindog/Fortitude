namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public delegate void StructStyler<in TStruct>(TStruct toStyle, IStyledTypeStringAppender toAppendTo) where TStruct : struct;