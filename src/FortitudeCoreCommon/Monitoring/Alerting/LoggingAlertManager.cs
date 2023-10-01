using FortitudeCommon.Monitoring.Logging;

namespace FortitudeCommon.Monitoring.Alerting
{
    public class LoggingAlertManager : IAlertManager
    {
        private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(LoggingAlertManager));

        public void SendAlert(string title, string description, string action, AlertSeverity severity)
        {
            switch (severity)
            {
                case AlertSeverity.Low:
                    logger.Info($"{title} - {description} requires {action}");
                    break;
                case AlertSeverity.Medium:
                    logger.Warn($"{title} - {description} requires {action}");
                    break;
                default:
                    logger.Error($"{title} - {description} requires {action}");
                    break;
            }
        }
    }
}