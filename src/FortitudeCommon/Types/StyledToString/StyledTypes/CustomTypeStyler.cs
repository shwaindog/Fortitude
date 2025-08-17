namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public delegate StyledTypeBuildResult CustomTypeStyler<in TToStyle>(TToStyle toStyle, IStyledTypeStringAppender toAppendTo);