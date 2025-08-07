using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Logging.Core.LoggerViews;


public delegate T CreateFLoggerView<out T>(IFLogger fLogger) where T : ISwitchFLoggerView;

public interface ISwitchFLoggerView
{
    T As<T>() where T : ISwitchFLoggerView;
}

