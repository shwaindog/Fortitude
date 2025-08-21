using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout;

public enum FLogEntryLayoutTokens
{
    // ReSharper disable InconsistentNaming
    // Log entry timestamp names
    TS
  , DATE
  , TIME
  , DATETIME
    // ReSharper disable InconsistentNaming
    // Log entry timestamp names
  , DATEONLY
  , DO
  , TIMEONLY
  , TO
    // Log entry level
  , LOGLEVEL
  , LEVEL
  , LVL
    // Correlation Id - Async correlation Id object
  , CORRELATIONID
  , CID
    // Thread Id
  , THREADID
  , TID
    // Thread Name
  , THREADNAME
  , TNAME
    // Logger Full Name
  , LOGGERNAME
  , LOGGER
  , LGRNAME
  , LGR
    // Logger Message Body
  , MESSAGE
  , MESG
  , MSG
    // Logger Attached Exception
  , EXCEPTION
  , EXCEP
  , EX
    // Logger Attached Exception New lines removed
  , EXCEPTION_ONELINE
  , EXCEP_ONELINE
  , EX_ONELINE
  , EXCEPTION_1L
  , EXCEP_1L
  , EX_1L
    // Caller Context attached obj - Async/Thread caller attached
  , CALLERATTACHED
  , CALLERATTCH
  , CALLERAT

    // Conditional formatting on LogLevel
  , LOGLEVEL_COND
  , LEVEL_COND
  , LVL_COND
    // Conditional formatting if logger has Attached Exception
  , EXCEPTION_COND
  , EXCEP_COND
  , EX_COND

    // Console Text Color
  , COLOR
  , FOREGROUNDCOLOR
  , FORECOLOR
  , FCOLOR
  , FC
    // Console Background Color
  , BACKGROUNDCOLOR
  , BACKCOLOR
  , BCOLOR
  , BC
    // Console Log Level Text Color Selection Switch/Match
  , COLOR_MATCH
  , FOREGROUNDCOLOR_MATCH
  , FORECOLOR_MATCH
  , FCOLOR_MATCH
  , FCM
    // Console Log Level Background Color Selection Switch/Match
  , BACKGROUNDCOLOR_MATCH
  , BACKCOLOR_MATCH
  , BCOLOR_MATCH
  , BCM
    // Console Reset to Color Console Defaults
  , RESETCOLOR
  , RC

    // LOG Entry call Line number
  , LOGLINENUMBER
  , LOGLINENUM
  , LLN
    // Log Entry caller member name
  , LOGMEMBERNAME
  , LMN
    // Log Entry caller filename with extension
  , LOGFILENAME_WITHEXT
  , LFNE
    // Log Entry caller filename no extension
  , LOGFILENAME
  , LFN
    // Log Entry caller compile time source file filesystem path
  , LOGFULLFILEPATH
  , LFFP

    // Add an environment aware newline (optional carriage return ) 
  , NEWLINE
  , NL
    // Always add unix new lines even on windows
  , UNIXNEWLINE
  , UNIXNL
    // Always add windows new lines even on unix/linux
  , WINDOWSNEWLINE
  , WINDOWSNL
  , WINNEWLINE
  , WINNL

    // Could also do the string literal but is her incase find/replace replacement is required
  , COMMA
  , C
  , COMMASPACE
  , CS

    // launching assemblby name 
  , StartAssemblyName
  , StartAssemblyDirPath
  , StartCommandArg0
  , StartCommandArg1
  , StartCommandArg2
  , StartCommandArg3
  , StartCommandArg4
  , StartCommandArg5
  , StartCommandArg6
  , StartCommandArg7
  , StartCommandArg8
  , StartCommandArg9
    // Environment.HostName
  , HostName
    // Environment.Login
  , Login
  , LoginDomainName

    // followed by : is the name of an environment variable to inject
  , Env

    // ReSharper restore InconsistentNaming
}

public static class FLogEntryLayoutTokensExtensions
{
    public static bool IsLogEntryDateTimeTokenName(this string checkName) =>
        checkName switch
        {
            $"{nameof(TS)}"
             or $"{nameof(DATE)}"
             or $"{nameof(TIME)}"
             or $"{nameof(DATETIME)}"
             or $"{nameof(DATEONLY)}"
             or $"{nameof(DO)}"
             or $"{nameof(TO)}"
             or $"{nameof(TIMEONLY)}" => true
          , _ => false
        };

    public static bool IsLogEntryDatumTokenName(this string checkName) =>
        checkName switch
        {
            $"{nameof(DATETIME)}"
             or $"{nameof(DATE)}"
             or $"{nameof(TIME)}"
             or $"{nameof(TS)}"
             or $"{nameof(DATEONLY)}"
             or $"{nameof(DO)}"
             or $"{nameof(TIMEONLY)}"
             or $"{nameof(TO)}"
             or $"{nameof(LOGLEVEL)}"
             or $"{nameof(LEVEL)}"
             or $"{nameof(LVL)}"
             or $"{nameof(CORRELATIONID)}"
             or $"{nameof(CID)}"
             or $"{nameof(THREADID)}"
             or $"{nameof(TID)}"
             or $"{nameof(THREADNAME)}"
             or $"{nameof(TNAME)}"
             or $"{nameof(LOGGERNAME)}"
             or $"{nameof(LOGGER)}"
             or $"{nameof(LGRNAME)}"
             or $"{nameof(LGR)}"
             or $"{nameof(MESSAGE)}"
             or $"{nameof(MESG)}"
             or $"{nameof(MSG)}"
             or $"{nameof(EXCEPTION)}"
             or $"{nameof(EXCEP)}"
             or $"{nameof(EX)}"
             or $"{nameof(LOGLINENUMBER)}"
             or $"{nameof(LOGLINENUM)}"
             or $"{nameof(LLN)}"
             or $"{nameof(LOGMEMBERNAME)}"
             or $"{nameof(LMN)}"
             or $"{nameof(LOGFILENAME_WITHEXT)}"
             or $"{nameof(LFNE)}"
             or $"{nameof(LOGFILENAME)}"
             or $"{nameof(LFN)}"
             or $"{nameof(LOGFULLFILEPATH)}"
             or $"{nameof(LFFP)}"
             or $"{nameof(EXCEPTION_ONELINE)}"
             or $"{nameof(EXCEP_ONELINE)}"
             or $"{nameof(EX_ONELINE)}"
             or $"{nameof(EXCEPTION_1L)}"
             or $"{nameof(EXCEP_1L)}"
             or $"{nameof(EX_1L)}"
             or $"{nameof(CALLERATTACHED)}"
             or $"{nameof(CALLERATTCH)}"
             or $"{nameof(CALLERAT)}" => true
          , _ => false
        };

    public static bool IsEnvironmentTokenName(this string checkName) =>
        checkName switch
        {
            $"{nameof(StartAssemblyName)}"
             or $"{nameof(StartAssemblyDirPath)}"
             or $"{nameof(StartCommandArg0)}"
             or $"{nameof(StartCommandArg1)}"
             or $"{nameof(StartCommandArg2)}"
             or $"{nameof(StartCommandArg3)}"
             or $"{nameof(StartCommandArg4)}"
             or $"{nameof(StartCommandArg5)}"
             or $"{nameof(StartCommandArg6)}"
             or $"{nameof(StartCommandArg7)}"
             or $"{nameof(StartCommandArg8)}"
             or $"{nameof(StartCommandArg9)}"
             or $"{nameof(HostName)}"
             or $"{nameof(Login)}"
             or $"{nameof(LoginDomainName)}" => true
          , _ => false
        };

    public static bool IsScopedFormattingTokenName(this string checkName) =>
        checkName switch
        {
            $"{nameof(LOGLEVEL_COND)}"
             or $"{nameof(LEVEL_COND)}"
             or $"{nameof(LVL_COND)}"
             or $"{nameof(EXCEPTION_COND)}"
             or $"{nameof(EXCEP_COND)}"
             or $"{nameof(EX_COND)}"
             or $"{nameof(COLOR)}"
             or $"{nameof(FOREGROUNDCOLOR)}"
             or $"{nameof(FORECOLOR)}"
             or $"{nameof(FCOLOR)}"
             or $"{nameof(FC)}"
             or $"{nameof(BACKGROUNDCOLOR)}"
             or $"{nameof(BACKCOLOR)}"
             or $"{nameof(BCOLOR)}"
             or $"{nameof(BC)}"
             or $"{nameof(COLOR_MATCH)}"
             or $"{nameof(FOREGROUNDCOLOR_MATCH)}"
             or $"{nameof(FORECOLOR_MATCH)}"
             or $"{nameof(FCOLOR_MATCH)}"
             or $"{nameof(FCM)}"
             or $"{nameof(BACKGROUNDCOLOR_MATCH)}"
             or $"{nameof(BACKCOLOR_MATCH)}"
             or $"{nameof(BCOLOR_MATCH)}"
             or $"{nameof(BCM)}"
             or $"{nameof(RESETCOLOR)}"
             or $"{nameof(RC)}" => true
          , _ => false
        };
}
