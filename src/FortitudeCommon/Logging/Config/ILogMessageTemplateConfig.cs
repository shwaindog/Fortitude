using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Logging.Config;

public interface ILogMessageTemplateConfig
{
    FLogLevel LogLevel { get; }

    string MessageTemplate { get; }

    string TemplateLoggerName { get; }
}