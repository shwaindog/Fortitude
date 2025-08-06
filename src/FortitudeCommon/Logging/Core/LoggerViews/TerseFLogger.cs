using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Logging.Config;

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface ITerseFLogger
{
    ISingleInvokeArgumentChain? AtLvlApnd<T>(FLogLevel level, T firstAppend, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? AtLvlFmt(FLogLevel level, string formatString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void AtLvl(FLogLevel level, string constantString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? TrcApnd<T>(T firstAppend, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? TrcFmt(string formatString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void Trc(string constantString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? DbgApnd<T>(T firstAppend, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? DbgFmt(string formatString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void Dbg(string constantString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? InfApnd<T>(T firstAppend, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? InfFmt(string formatString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void Inf(string constantString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? WrnApnd<T>(T firstAppend, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? WrnFmt(string formatString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void Wrn(string constantString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? ErrApnd<T>(T firstAppend, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    ISingleInvokeArgumentChain? ErrFmt(string formatString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void Err(string constantString, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

}