namespace FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout;

public enum FLogEntryLayoutTokens
{
    // ReSharper disable InconsistentNaming
    // Log entry timestamp names
    TS
  , DATE
  , TIME
  , DATETIME
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
    // Console Background Color
  , BACKGROUNDCOLOR
  , BACKCOLOR
  , BCOLOR
    // Console Log Level Text Color Selection Switch/Match
  , COLOR_MATCH
  , FOREGROUNDCOLOR_MATCH
  , FORECOLOR_MATCH
  , FCOLOR_MATCH
    // Console Log Level Background Color Selection Switch/Match
  , BACKGROUNDCOLOR_MATCH
  , BACKCOLOR_MATCH
  , BCOLOR_MATCH
    // Console Reset to Color Console Defaults
  , RESETCOLOR
    // ReSharper restore InconsistentNaming
}
