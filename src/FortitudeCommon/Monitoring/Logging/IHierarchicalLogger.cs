namespace FortitudeCommon.Monitoring.Logging;

public interface IHierarchicalLogger
{
    string FullNameOfLogger { get; }
    Action<IHierarchicalLogger, string> SettingTranslation { get; }
    string DefaultStringValue { get; }
    bool Enabled { get; set; }
}
