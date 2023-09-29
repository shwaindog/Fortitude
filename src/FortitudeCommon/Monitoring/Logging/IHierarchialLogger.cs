using System;

namespace FortitudeCommon.Monitoring.Logging
{
    public interface IHierarchialLogger
    {
        string FullNameOfLogger { get; }
        Action<IHierarchialLogger, string> SettingTranslation { get; }
        string DefaultStringValue { get; }
        bool Enabled { get; set; }
    }
}