using System;
using FortitudeCommon.Monitoring.Logging.Diagnostics;

namespace FortitudeCommon.Monitoring.Logging
{
    internal class NoopFactory : IFLoggerFactory
    {
        public IFLogger GetLogger(string loggerName)
        {
            return new NoopIfLogger(loggerName);
        }
        public IFLogger GetLogger(Type ownerType)
        {
            return GetLogger(ownerType.FullName);
        }

        private class NoopIfLogger : IFLogger
        {
            private bool defaultEnabled = true;

            public NoopIfLogger(string loggerName)
            {
                this.Name = loggerName;
            }

            public void OnLogEvent(FLogEvent evt)
            {
            }

            public bool IsDebugEnabled
            {
                get { return false; }
            }

            public void Debug(string fmt, params object[] args)
            {
            }

            public void Debug(string msg)
            {
            }

            public void Debug(object obj)
            {
            }

            public bool IsInfoEnabled => false;

            public void Info(string fmt, params object[] args)
            {
            }

            public void Info(string msg)
            {
            }

            public void Info(object obj)
            {
            }

            public bool IsWarnEnabled { get; } = false;

            public void Warn(string fmt, params object[] args)
            {
            }

            public void Warn(string msg)
            {
            }

            public void Warn(object obj)
            {
            }

            public bool IsErrorEnabled => false;

            public void Error(string fmt, params object[] args)
            {
            }

            public void Error(string msg)
            {
            }

            public void Error(object obj)
            {
            }

            public string Name { get; }

            public string FullNameOfLogger => Name;

            public Action<IHierarchialLogger, string> SettingTranslation
            {
                get
                {
                    return (hl, setting) =>
                    {
                        var currentIfLogger = hl as IFLogger;
                        if (currentIfLogger == null || setting == null) return;
                        currentIfLogger.HierarchialSetting = setting;
                        if (HierarchialConfigurationUpdater.IsActivationKeyWord(setting))
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
            public string HierarchialSetting { get; set; }

            public bool DefaultEnabled
            {
                get { return defaultEnabled; }
                set
                {
                    defaultEnabled = value;
                    SettingTranslation(this, HierarchialSetting);
                }
            }

            public void WarnException(string msg, Exception ex)
            {
            }

            public void ErrorException(string msg, Exception ex)
            {
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}