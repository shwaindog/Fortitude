namespace FortitudeCommon.EventProcessing.Disruption.Rings;

public struct QueueEventTime
{
    public QueueEventTime(long messageNumber, DateTime time)
    {
        MessageNumber = messageNumber;
        Time = time;
    }

    public long MessageNumber { get; }
    public DateTime Time { get; }
}
