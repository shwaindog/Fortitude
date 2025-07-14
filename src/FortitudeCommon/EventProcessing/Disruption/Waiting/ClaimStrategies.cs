#region

using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Waiting;

internal static class ClaimStrategies
{
    public static IClaimStrategy GetInstance
    (this ClaimStrategyType type, string ringName, int ringSize,
        bool logErrors = true, IIntraOSThreadSignal? awaitUpdateSignal = null)
    {
        switch (type)
        {
            case ClaimStrategyType.MultiProducers: return new MultiProducersClaimStrategy(ringName, ringSize, logErrors, awaitUpdateSignal);
            case ClaimStrategyType.SingleProducer: return new SingleProducerClaimStrategy(ringName, ringSize, logErrors, awaitUpdateSignal);

            default: throw new InvalidOperationException("Unsupported strategy type");
        }
    }

    public static IClaimStrategyLong GetInstanceLong
    (this ClaimStrategyType type, string ringName, int ringSize,
        bool logErrors = true, IIntraOSThreadSignal? awaitUpdateSignal = null)
    {
        switch (type)
        {
            case ClaimStrategyType.MultiProducers: return new MultiProducersClaimStrategyLong(ringName, ringSize, logErrors, awaitUpdateSignal);
            case ClaimStrategyType.SingleProducer: return new SingleProducerClaimStrategyLong(ringName, ringSize, logErrors, awaitUpdateSignal);

            default: throw new InvalidOperationException("Unsupported strategy type");
        }
    }

    private sealed class MultiProducersClaimStrategy(string ringName, int ringSize, bool logErrors, IIntraOSThreadSignal? awaitUpdateSignal = null)
        : IClaimStrategy
    {
        private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger("EventProcessing." + ringName);

        private readonly int ringMask = ringSize - 1;

        private PaddedVolatileInt lastMaxDistance = new(Sequence.InitialValue);

        private PaddedAtomicInt sequence = new(Sequence.InitialValue);

        public int Claim() => PaddedAtomicInt.Extensions.IncrementAndGet(ref sequence.iValue);

        public void WaitFor(int sequenceValue, Sequence[] cursors)
        {
            var newDistance = ++lastMaxDistance.Value;
            if (ringSize < newDistance)
            {
                var warned = false;
                while (true)
                {
                    var maxDistance = 0;
                    for (int i = 0; i < cursors.Length; i++)
                    {
                        var cursorValue    = (cursors[i].Value & ringMask);
                        var cursorDistance = cursorValue > sequenceValue ? (ringSize - cursorValue) + sequenceValue : sequenceValue - cursorValue;
                        if (cursorDistance > maxDistance)
                        {
                            maxDistance = cursorDistance;
                        }
                    }
                    if (ringSize >= maxDistance)
                    {
                        lastMaxDistance.Value = maxDistance;
                        break;
                    }
                    if (!warned)
                    {
                        if (logErrors)
                            logger.Warn("Waiting on slow consumer for {0}, WrapSequence={1}, MinSequence={2}",
                                        ringName, ringSize, maxDistance);
                        warned = true;
                    }

                    awaitUpdateSignal?.WaitOne(50);
                    Thread.Yield();
                }
            }
        }

        public void Serialize(Sequence cursor, int sequenceValue)
        {
            var expectedSequence = (sequenceValue - 1) & ringMask;
            while (expectedSequence != cursor.Value) Thread.Yield();
        }
    }

    private sealed class MultiProducersClaimStrategyLong
        (string ringName, int ringSize, bool logErrors, IIntraOSThreadSignal? awaitUpdateSignal = null)
        : IClaimStrategyLong
    {
        private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger("EventProcessing." + ringName);

        private PaddedVolatileLong minProcessedSequence = new(SequenceLong.InitialValue);

        private PaddedAtomicLong sequence = new(SequenceLong.InitialValue);

        public long Claim() => PaddedAtomicLong.Extensions.IncrementAndGet(ref sequence.lValue);

        public void WaitFor(long sequenceValue, SequenceLong[] cursors)
        {
            var wrapSequence = sequenceValue - ringSize;
            if (wrapSequence > minProcessedSequence.Value)
            {
                long minSequence;
                var  warned = false;
                while (wrapSequence > (minSequence = cursors.Min()))
                {
                    if (!warned)
                    {
                        if (logErrors)
                            logger.Warn("Waiting on slow consumer for {0}, WrapSequence={1}, MinSequence={2}",
                                        ringName, wrapSequence, minSequence);
                        warned = true;
                    }

                    awaitUpdateSignal?.WaitOne(50);
                    Thread.Yield();
                }

                minProcessedSequence.Value = minSequence;
            }
        }

        public void Serialize(SequenceLong cursor, long sequenceValue)
        {
            var expectedSequence = sequenceValue - 1;
            while (expectedSequence != cursor.Value) Thread.Yield();
        }
    }

    private sealed class SingleProducerClaimStrategy(string ringName, int ringSize, bool logErrors, IIntraOSThreadSignal? awaitUpdateSignal = null)
        : IClaimStrategy
    {
        private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger("EventProcessing." + ringName);

        private readonly int ringMask = ringSize - 1;

        private PaddedInt lastMaxDistance = new(Sequence.InitialValue);

        private PaddedInt sequence = new(Sequence.InitialValue);

        public int Claim() => PaddedInt.Extensions.IncrementAndGet(ref sequence.Value);

        public void WaitFor(int sequenceValue, Sequence[] cursors)
        {
            var newDistance = ++lastMaxDistance.Value;
            if (ringSize < newDistance)
            {
                var warned = false;
                while (true)
                {
                    var maxDistance = 0;
                    for (int i = 0; i < cursors.Length; i++)
                    {
                        var cursorValue    = (cursors[i].Value & ringMask);
                        var cursorDistance = cursorValue > sequenceValue ? (ringSize - cursorValue) + sequenceValue : sequenceValue - cursorValue;
                        if (cursorDistance > maxDistance)
                        {
                            maxDistance = cursorDistance;
                        }
                    }
                    if (ringSize >= maxDistance)
                    {
                        lastMaxDistance.Value = maxDistance;
                        break;
                    }
                    if (!warned)
                    {
                        if (logErrors)
                            logger.Warn("Waiting on slow consumer for {0}, WrapSequence={1}, MinSequence={2}",
                                        ringName, ringSize, maxDistance);
                        warned = true;
                    }

                    awaitUpdateSignal?.WaitOne(50);
                    Thread.Yield();
                }
            }
        }

        public void Serialize(Sequence cursor, int sequenceValue) { }
    }

    private sealed class SingleProducerClaimStrategyLong
        (string ringName, int ringSize, bool logErrors, IIntraOSThreadSignal? awaitUpdateSignal = null)
        : IClaimStrategyLong
    {
        private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger("EventProcessing." + ringName);

        private PaddedLong minProcessedSequence = new(SequenceLong.InitialValue);

        private PaddedLong sequence = new(SequenceLong.InitialValue);

        public long Claim() => PaddedLong.Extensions.IncrementAndGet(ref sequence.Value);

        public void WaitFor(long sequenceValue, SequenceLong[] cursors)
        {
            var wrapSequence = sequenceValue - ringSize;
            if (wrapSequence > minProcessedSequence.Value)
            {
                long minSequence;
                var  warned = false;
                while (wrapSequence > (minSequence = cursors.Min()))
                {
                    if (!warned)
                    {
                        if (logErrors)
                            logger.Warn("Waiting on slow consumer for {0}, WrapSequence={1}, MinSequence={2}",
                                        ringName, wrapSequence, minSequence);
                        warned = true;
                    }

                    awaitUpdateSignal?.WaitOne(50);
                    Thread.Yield();
                }

                minProcessedSequence.Value = minSequence;
            }
        }

        public void Serialize(SequenceLong cursor, long sequenceValue) { }
    }
}
