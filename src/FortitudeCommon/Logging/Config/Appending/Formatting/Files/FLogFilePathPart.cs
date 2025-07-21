using FortitudeCommon.Types;
using static FortitudeCommon.Logging.Config.Appending.Formatting.Files.FLogFilePathPart;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Files;

public enum FLogFilePathPart
{
    ConfigLoadingAssemblyName
  , StartAssemblyName
  , DateYearToSecond
  , DateYearToDay
  , DateUsingFormat
  , DateYear
  , DateMonthNumber
  , DateMonthShortName
  , DateMonthName
  , DateDayNumber
  , DateDayWeekShortName
  , DateDayWeekName
  , LogLevel
  , LogLevelShort
  , LogLevelOrdinal
  , LoggerFullName
  , LoggerLastName
  , LoggerReducedFullName
  , LoggerCallerMemberName
  , LoggerCallerFileName
  , LoggerCaller1DirFilePath
  , LoggerCaller2DirFilePath
  , LoggerCaller3DirFilePath
  , LoggerCaller4DirFilePath
  , LoggerCallerRootNameSpaceFilePath
}

public static class FLogFilePathPartExtensions
{
    public static Action<FLogFilePathPart, IStyledTypeStringAppender> FLogFilePathPartFormatter
        = FormatFLogFilePathPartAppender;

    public static void FormatFLogFilePathPartAppender(this FLogFilePathPart filePathPart, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.BackingStringBuilder;

        switch (filePathPart)
        {
            case ConfigLoadingAssemblyName: sb.Append($"{nameof(ConfigLoadingAssemblyName)}"); break;

            case StartAssemblyName:        sb.Append($"{nameof(StartAssemblyName)}"); break;
            case DateYearToSecond:         sb.Append($"{nameof(DateYearToSecond)}"); break;
            case DateYearToDay:            sb.Append($"{nameof(DateYearToDay)}"); break;
            case DateUsingFormat:          sb.Append($"{nameof(DateUsingFormat)}"); break;
            case DateYear:                 sb.Append($"{nameof(DateYear)}"); break;
            case DateMonthNumber:          sb.Append($"{nameof(DateMonthNumber)}"); break;
            case DateMonthShortName:       sb.Append($"{nameof(DateMonthShortName)}"); break;
            case DateMonthName:            sb.Append($"{nameof(DateMonthName)}"); break;
            case DateDayNumber:            sb.Append($"{nameof(DateDayNumber)}"); break;
            case DateDayWeekShortName:     sb.Append($"{nameof(DateDayWeekShortName)}"); break;
            case DateDayWeekName:          sb.Append($"{nameof(DateDayWeekName)}"); break;
            case LogLevel:                 sb.Append($"{nameof(LogLevel)}"); break;
            case LogLevelShort:            sb.Append($"{nameof(LogLevelShort)}"); break;
            case LogLevelOrdinal:          sb.Append($"{nameof(LogLevelOrdinal)}"); break;
            case LoggerFullName:           sb.Append($"{nameof(LoggerFullName)}"); break;
            case LoggerLastName:           sb.Append($"{nameof(LoggerLastName)}"); break;
            case LoggerReducedFullName:    sb.Append($"{nameof(LoggerReducedFullName)}"); break;
            case LoggerCallerMemberName:   sb.Append($"{nameof(LoggerCallerMemberName)}"); break;
            case LoggerCallerFileName:     sb.Append($"{nameof(LoggerCallerFileName)}"); break;
            case LoggerCaller1DirFilePath: sb.Append($"{nameof(LoggerCaller1DirFilePath)}"); break;
            case LoggerCaller2DirFilePath: sb.Append($"{nameof(LoggerCaller2DirFilePath)}"); break;
            case LoggerCaller3DirFilePath: sb.Append($"{nameof(LoggerCaller3DirFilePath)}"); break;
            case LoggerCaller4DirFilePath: sb.Append($"{nameof(LoggerCaller4DirFilePath)}"); break;

            case LoggerCallerRootNameSpaceFilePath: sb.Append($"{nameof(LoggerCallerRootNameSpaceFilePath)}"); break;

            default: sb.Append($"{nameof(StartAssemblyName)}"); break;
        }
    }
}
