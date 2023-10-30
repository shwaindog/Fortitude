
namespace FortitudeCommon.Monitoring.Alerting
{
    public interface IAlertManager
    {
        void SendAlert(string title, string description, string action, AlertSeverity severity);
    }
}
