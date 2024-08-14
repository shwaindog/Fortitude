// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging.Diagnostics;

#endregion

namespace FortitudeCommon.Monitoring.Logging;

public class NoOpLoggerFactory : IFLoggerFactory
{
    public static readonly IFLogger SingletonNoopLogger = new NoOpIfLogger("Singleton");

    public static bool StartWithNoOpLoggerFactory = false;

    public IFLogger GetLogger(string loggerName) => SingletonNoopLogger;

    public IFLogger GetLogger(Type ownerType) => GetLogger(ownerType.FullName!);

    private class NoOpIfLogger : IFLogger
    {
        private bool defaultEnabled = true;

        public NoOpIfLogger(string loggerName)
        {
            Name                = loggerName;
            HierarchicalSetting = "";
        }

        public void OnLogEvent(FLogEvent evt) { }

        public bool IsDebugEnabled => false;

        public void Debug(string fmt, params object?[] args) { }

        public void Debug(string msg) { }

        public void Debug(object obj) { }

        public bool IsInfoEnabled => false;

        public void Info(string fmt, params object?[] args) { }

        public void Info(string msg) { }

        public void Info(object obj) { }

        public bool IsWarnEnabled { get; } = false;

        public void Warn(string fmt, params object?[] args) { }

        public void Warn(string msg) { }

        public void Warn(object obj) { }

        public bool IsErrorEnabled => false;

        public void Error(string fmt, params object?[] args) { }

        public void Error(string msg) { }

        public void Error(object obj) { }

        public string Name { get; }

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

        public bool   Enabled             { get; set; }
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

        public void WarnException(string msg, Exception ex) { }

        public void ErrorException(string msg, Exception ex) { }

        public override string ToString() => Name;
    }
}
