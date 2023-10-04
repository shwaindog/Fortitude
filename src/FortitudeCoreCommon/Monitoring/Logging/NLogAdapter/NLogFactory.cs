#region

using System.Globalization;
using FortitudeCommon.Monitoring.Logging.Diagnostics;
using NLog;

#endregion

namespace FortitudeCommon.Monitoring.Logging.NLogAdapter;

public class NLogFactory : IFLoggerFactory
{
    public IFLogger GetLogger(string loggerName) => new NLogIfLogger(LogManager.GetLogger(loggerName));

    public IFLogger GetLogger(Type ownerType) => GetLogger(ownerType.FullName!);

    private class NLogIfLogger : IFLogger
    {
        private readonly Logger logger;
        private bool defaultEnabled = true;

        public NLogIfLogger(Logger logger)
        {
            this.logger = logger;
            HierarchicalSetting = "";
        }

        public bool IsDebugEnabled => logger.IsDebugEnabled;

        public void OnLogEvent(FLogEvent evt)
        {
            LogLevel? level = null;
            switch (evt.Level)
            {
                case FLogLevel.Debug:
                    level = LogLevel.Debug;
                    break;
                case FLogLevel.Info:
                    level = LogLevel.Info;
                    break;
                case FLogLevel.Warn:
                    level = LogLevel.Warn;
                    break;
                case FLogLevel.Error:
                    level = LogLevel.Error;
                    break;
            }

            LogEventInfo theEvent;
            if (evt.Exception != null)
                theEvent = new LogEventInfo(level, Name, CultureInfo.InvariantCulture, evt.MsgFormat,
                    evt.MsgParams, evt.Exception);
            else if (evt.MsgObject != null)
                theEvent = new LogEventInfo(level, Name, evt.MsgObject.ToString());
            else if (evt.MsgParams != null)
                theEvent = new LogEventInfo(level, Name, CultureInfo.InvariantCulture, evt.MsgFormat,
                    evt.MsgParams);
            else
                theEvent = new LogEventInfo(level, Name, evt.MsgFormat);

            theEvent.Properties["PreciseTimestamp"] = evt.LogTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            logger.Log(theEvent);
        }

        public void Debug(string fmt, params object?[] args)
        {
            logger.Debug(fmt, args);
        }

        public void Debug(string msg)
        {
            logger.Debug(msg);
        }

        public void Debug(object obj)
        {
            logger.Debug(obj);
        }

        public bool IsInfoEnabled => logger.IsInfoEnabled;

        public void Info(string fmt, params object?[] args)
        {
            logger.Info(fmt, args);
        }

        public void Info(string msg)
        {
            logger.Info(msg);
        }

        public void Info(object obj)
        {
            logger.Info(obj);
        }

        public bool IsWarnEnabled => logger.IsWarnEnabled;

        public void Warn(string fmt, params object?[] args)
        {
            logger.Warn(fmt, args);
        }

        public void Warn(string msg)
        {
            logger.Warn(msg);
        }

        public void Warn(object obj)
        {
            logger.Warn(obj);
        }

        public bool IsErrorEnabled => logger.IsErrorEnabled;

        public void Error(string fmt, params object?[] args)
        {
            logger.Error(fmt, args);
        }

        public void Error(string msg)
        {
            logger.Error(msg);
        }

        public void Error(object obj)
        {
            logger.Error(obj);
        }

        public string Name => logger.Name;

        public string FullNameOfLogger => Name;

        public Action<IHierarchicalLogger, string> SettingTranslation
        {
            get
            {
                return (hl, setting) =>
                {
                    var currentIfLogger = hl as IFLogger;
                    if (currentIfLogger == null || setting == null) return;
                    currentIfLogger.HierarchicalSetting = setting;
                    if (HierarchicalConfigurationUpdater.IsActivationKeyWord(setting))
                    {
                        currentIfLogger.Enabled = true;
                        return;
                    }

                    currentIfLogger.Enabled = currentIfLogger.DefaultEnabled;
                };
            }
        }

        public string DefaultStringValue => "default";

        public bool Enabled { get; set; }
        public string HierarchicalSetting { get; set; }

        public bool DefaultEnabled
        {
            get => defaultEnabled;
            set
            {
                defaultEnabled = value;
                SettingTranslation(this, HierarchicalSetting);
            }
        }

        public override string ToString() => Name;
    }
}
