namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Performance
{
    public interface IPerfLoggerPool : IHierarchialLogger
    {
        IPerfLogger StartNewTrace();

        void StopTrace<T1, TU, TV>(IPerfLogger traceLogger, T1 optionalData, TU moreOptionalData,
            TV evenMoreOptionalData);

        void StopTrace<T1, TU>(IPerfLogger traceLogger, T1 optionalData, TU moreOptionalData);
        void StopTrace<T1>(IPerfLogger traceLogger, T1 optionalData);
        void StopTrace(IPerfLogger traceLogger);
    }
}