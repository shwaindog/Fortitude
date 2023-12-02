#region

using Fortitude.EventProcessing.BusRules.Rules;

#endregion

namespace FortitudeTests.FortitudeBusRules.Rules;

public class IncrementingRule : Rule
{
    private static int instanceNumber;
    private int startCount;
    private int stopCount;

    public IncrementingRule() : base("IncrementingRule", Interlocked.Increment(ref instanceNumber).ToString()) { }

    public int StartCount => startCount;
    public int StopCount => stopCount;

    public override void Start()
    {
        base.Start();
        IncrementLifeTimeCount();
        Interlocked.Increment(ref startCount);
    }

    public override void Stop()
    {
        base.Stop();
        Interlocked.Increment(ref stopCount);
    }

    public override string ToString() =>
        $"{FriendlyName}({base.ToString()}, {nameof(startCount)}: {startCount}, {nameof(stopCount)}: {stopCount})";
}
