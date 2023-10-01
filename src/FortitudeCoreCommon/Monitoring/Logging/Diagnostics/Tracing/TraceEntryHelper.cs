namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing
{
    internal static class TraceEntryHelper
    {
        public static string ToStringNoBoxing(this TraceEntry thisToString)
        {
            return string.Format("Time: {0:HH\\:mm\\:ss.ffffff}, Identifier: '{1}', Subject: '{2}'", thisToString.Time,
                thisToString.Identifier, thisToString.Subject);
        }
    }
}