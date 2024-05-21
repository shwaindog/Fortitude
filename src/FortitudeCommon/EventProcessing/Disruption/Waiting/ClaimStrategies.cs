#region

using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Waiting;

internal static class ClaimStrategies
{
    public static IClaimStrategy GetInstance(this ClaimStrategyType type, string ringName, int ringSize,
        bool logErrors = true, IIntraOSThreadSignal? awaitUpdateSignal = null)
    {
        switch (type)
        {
            case ClaimStrategyType.MultiProducers:
                return new MultiProducersClaimStrategy(ringName, ringSize, logErrors, awaitUpdateSignal);
            case ClaimStrategyType.SingleProducer:
                return new SingleProducerClaimStrategy(ringName, ringSize, logErrors, awaitUpdateSignal);
            default:
                throw new InvalidOperationException("Unsupported strategy type");
        }
    }

    private sealed class MultiProducersClaimStrategy : IClaimStrategy
    {
        private readonly IIntraOSThreadSignal? awaitUpdateSignal;
        private readonly bool logErrors;
        private readonly IFLogger logger;
        private readonly string ringName;
        private readonly int ringSize;
        private PaddedVolatileLong minProcessedSequence = new(Sequence.InitialValue);
        private PaddedAtomicLong sequence = new(Sequence.InitialValue);

        public MultiProducersClaimStrategy(string ringName, int ringSize, bool logErrors, IIntraOSThreadSignal? awaitUpdateSignal = null)
        {
            this.ringName = ringName;
            logger = FLoggerFactory.Instance.GetLogger("EventProcessing." + ringName);
            this.ringSize = ringSize;
            this.logErrors = logErrors;
            this.awaitUpdateSignal = awaitUpdateSignal;
        }

        public long Claim() => sequence.IncrementAndGet();

        public void WaitFor(long sequence, Sequence[] cursors)
        {
            var wrapSequence = sequence - ringSize;
            if (wrapSequence > minProcessedSequence.Value)
            {
                long minSequence;
                var warned = false;
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

        public void Serialize(Sequence cursor, long sequence)
        {
            var expectedSequence = sequence - 1;
            while (expectedSequence != cursor.Value) Thread.Yield();
        }
    }

    private sealed class SingleProducerClaimStrategy : IClaimStrategy
    {
        private readonly IIntraOSThreadSignal? awaitUpdateSignal;
        private readonly bool logErrors;
        private readonly IFLogger logger;
        private readonly string ringName;
        private readonly int ringSize;
        private PaddedLong minProcessedSequence = new(Sequence.InitialValue);
        private PaddedLong sequence = new(Sequence.InitialValue);

        public SingleProducerClaimStrategy(string ringName, int ringSize, bool logErrors, IIntraOSThreadSignal? awaitUpdateSignal = null)
        {
            this.ringName = ringName;
            logger = FLoggerFactory.Instance.GetLogger("EventProcessing." + ringName);
            this.ringSize = ringSize;
            this.logErrors = logErrors;
            this.awaitUpdateSignal = awaitUpdateSignal;
        }

        public long Claim() => ++sequence.Value;

        public void WaitFor(long sequence, Sequence[] cursors)
        {
            var wrapSequence = sequence - ringSize;
            if (wrapSequence > minProcessedSequence.Value)
            {
                long minSequence;
                var warned = false;
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

        public void Serialize(Sequence cursor, long sequence) { }
    }
}
