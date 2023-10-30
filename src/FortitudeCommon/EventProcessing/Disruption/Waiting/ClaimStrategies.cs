using System;
using System.Threading;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.Monitoring.Logging;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal static class ClaimStrategies
    {
        public static IClaimStrategy GetInstance(this ClaimStrategyType type, string ringName, int ringSize,
            bool logErrors = true)
        {
            switch (type)
            {
                case ClaimStrategyType.MultiProducers:
                    return new MultiProducersClaimStrategy(ringName, ringSize, logErrors);
                case ClaimStrategyType.SingleProducer:
                    return new SingleProducerClaimStrategy(ringName, ringSize, logErrors);
                default:
                    throw new InvalidOperationException("Unsupported strategy type");
            }
        }

        private sealed class MultiProducersClaimStrategy : IClaimStrategy
        {
            private readonly bool logErrors;
            private readonly IFLogger logger;
            private readonly string ringName;
            private readonly int ringSize;
            private PaddedVolatileLong minProcessedSequence = new PaddedVolatileLong(Sequence.InitialValue);
            private PaddedAtomicLong sequence = new PaddedAtomicLong(Sequence.InitialValue);

            public MultiProducersClaimStrategy(string ringName, int ringSize, bool logErrors)
            {
                this.ringName = ringName;
                logger = FLoggerFactory.Instance.GetLogger("EventProcessing." + ringName);
                this.ringSize = ringSize;
                this.logErrors = logErrors;
            }

            public long Claim()
            {
                return sequence.IncrementAndGet();
            }

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
                            {
                                logger.Warn("Waiting on slow consumer for {0}, WrapSequence={1}, MinSequence={2}",
                                    ringName, wrapSequence, minSequence);
                            }
                            warned = true;
                        }
                        Thread.Yield();
                    }
                    minProcessedSequence.Value = minSequence;
                }
            }

            public void Serialize(Sequence cursor, long sequence)
            {
                var expectedSequence = sequence - 1;
                while (expectedSequence != cursor.Value)
                {
                    Thread.Yield();
                }
            }
        }

        private sealed class SingleProducerClaimStrategy : IClaimStrategy
        {
            private readonly bool logErrors;
            private readonly IFLogger logger;
            private readonly string ringName;
            private readonly int ringSize;
            private PaddedLong minProcessedSequence = new PaddedLong(Sequence.InitialValue);
            private PaddedLong sequence = new PaddedLong(Sequence.InitialValue);

            public SingleProducerClaimStrategy(string ringName, int ringSize, bool logErrors)
            {
                this.ringName = ringName;
                logger = FLoggerFactory.Instance.GetLogger("EventProcessing." + ringName);
                this.ringSize = ringSize;
                this.logErrors = logErrors;
            }

            public long Claim()
            {
                return ++sequence.Value;
            }

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
                            {
                                logger.Warn("Waiting on slow consumer for {0}, WrapSequence={1}, MinSequence={2}",
                                    ringName, wrapSequence, minSequence);
                            }
                            warned = true;
                        }
                        Thread.Yield();
                    }
                    minProcessedSequence.Value = minSequence;
                }
            }

            public void Serialize(Sequence cursor, long sequence)
            {
            }
        }
    }
}