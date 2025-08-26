using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout;

public enum FLogEntryLayoutTokens
{
    // ReSharper disable InconsistentNaming
    // Log entry timestamp names
    DATETIME
  , DATE
  , TIME
  , TS
    // ReSharper disable InconsistentNaming
    // Log entry timestamp names
  , DATEONLY
  , DO
  , TIMEONLY
  , TO
  // Log entry timestamp names
  , DATETIME_MICROSECONDS
  , DATE_MICROS
  , TIME_MICROS
  , TS_US
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
  , STARTASSEMBLYNAME
  , STARTASSEMBLYDIRPATH
  , STARTCOMMANDARG0
  , STARTCOMMANDARG1
  , STARTCOMMANDARG2
  , STARTCOMMANDARG3
  , STARTCOMMANDARG4
  , STARTCOMMANDARG5
  , STARTCOMMANDARG6
  , STARTCOMMANDARG7
  , STARTCOMMANDARG8
  , STARTCOMMANDARG9
    // Environment.HostName
  , HOSTNAME
    // Environment.Login
  , LOGIN
  , LOGINDOMAINNAME

    // followed by : is the name of an environment variable to inject
  , ENV

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
             or $"{nameof(TIMEONLY)}"
             or $"{nameof(DATETIME_MICROSECONDS)}"
             or $"{nameof(DATE_MICROS)}"
             or $"{nameof(TIME_MICROS)}"
             or $"{nameof(TS_US)}" => true
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
             or $"{nameof(DATETIME_MICROSECONDS)}"
             or $"{nameof(DATE_MICROS)}"
             or $"{nameof(TIME_MICROS)}"
             or $"{nameof(TS_US)}"
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
    
    public static bool IsLogEntryMessageTokenName(this string checkName) =>
      checkName switch
      {
        $"{nameof(MESSAGE)}"
       or $"{nameof(MESG)}"
       or $"{nameof(MSG)}" => true
      , _ => false
      };
    
    public static bool IsLogEntryExceptionTokenName(this string checkName) =>
      checkName switch
      {
        $"{nameof(EXCEPTION)}"
       or $"{nameof(EXCEP)}"
       or $"{nameof(EX)}" => true
      , _ => false
      };
    
    public static bool IsLargeBufferCheckRequiredTokenName(this string checkName) =>
      checkName.IsLogEntryMessageTokenName() || checkName.IsLogEntryExceptionTokenName();

    public static bool IsEnvironmentTokenName(this string checkName) =>
        checkName switch
        {
            $"{nameof(STARTASSEMBLYNAME)}"
             or $"{nameof(STARTASSEMBLYDIRPATH)}"
             or $"{nameof(STARTCOMMANDARG0)}"
             or $"{nameof(STARTCOMMANDARG1)}"
             or $"{nameof(STARTCOMMANDARG2)}"
             or $"{nameof(STARTCOMMANDARG3)}"
             or $"{nameof(STARTCOMMANDARG4)}"
             or $"{nameof(STARTCOMMANDARG5)}"
             or $"{nameof(STARTCOMMANDARG6)}"
             or $"{nameof(STARTCOMMANDARG7)}"
             or $"{nameof(STARTCOMMANDARG8)}"
             or $"{nameof(STARTCOMMANDARG9)}"
             or $"{nameof(HOSTNAME)}"
             or $"{nameof(LOGIN)}"
             or $"{nameof(LOGINDOMAINNAME)}" => true
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
