using System;
using System.Threading;
using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal static class WaitStrategies
    {
        public static IWaitStrategy GetInstance(this WaitStrategyType type, params object[] args)
        {
            switch (type)
            {
                case WaitStrategyType.BlockingMultiConsumers:
                    return new BlockingMultiConsumersWaitStrategy();
                case WaitStrategyType.BlockingSingleConsumer:
                    return new BlockingSingleConsumerWaitStrategy();
                case WaitStrategyType.Yielding:
                    return new YieldingWaitStrategy();
                case WaitStrategyType.Spinning:
                    return new SpinningWaitStrategy();
                default:
                    throw new InvalidOperationException("Unsupported strategy type");
            }
        }

        private sealed class BlockingMultiConsumersWaitStrategy : IWaitStrategy
        {
            private readonly object sync = new object();
            private PaddedVolatileFlag interrupt = new PaddedVolatileFlag(false);

            public long WaitFor(Sequence cursor, long sequence)
            {
                var availableSequence = cursor.Value;
                if (availableSequence < sequence)
                {
                    lock (sync)
                    {
                        while ((availableSequence = cursor.Value) < sequence && !interrupt.IsSet())
                        {
                            Monitor.Wait(sync);
                        }
                    }
                }
                return availableSequence;
            }

            public void NotifyAll()
            {
                lock (sync)
                {
                    Monitor.PulseAll(sync);
                }
            }

            public void InterruptAll()
            {
                interrupt.Set();
                NotifyAll();
            }

            public void ClearInterrupt()
            {
                interrupt.Clear();
            }
        }

        private sealed class BlockingSingleConsumerWaitStrategy : IWaitStrategy
        {
            private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);
            private PaddedVolatileFlag interrupt = new PaddedVolatileFlag(false);

            public long WaitFor(Sequence cursor, long sequence)
            {
                var availableSequence = cursor.Value;
                if (availableSequence < sequence)
                {
                    while ((availableSequence = cursor.Value) < sequence && !interrupt.IsSet())
                    {
                        resetEvent.WaitOne();
                    }
                }
                return availableSequence;
            }

            public void NotifyAll()
            {
                resetEvent.Set();
            }

            public void InterruptAll()
            {
                interrupt.Set();
                NotifyAll();
            }

            public void ClearInterrupt()
            {
                interrupt.Clear();
            }
        }

        private sealed class YieldingWaitStrategy : IWaitStrategy
        {
            private PaddedVolatileFlag interrupt = new PaddedVolatileFlag(false);

            public long WaitFor(Sequence cursor, long sequence)
            {
                long availableSequence;
                while ((availableSequence = cursor.Value) < sequence && !interrupt.IsSet())
                {
                    Thread.Sleep(0);
                }
                return availableSequence;
            }

            public void NotifyAll()
            {
            }

            public void InterruptAll()
            {
                interrupt.Set();
            }

            public void ClearInterrupt()
            {
                interrupt.Clear();
            }
        }

        private sealed class SpinningWaitStrategy : IWaitStrategy
        {
            private PaddedVolatileFlag interrupt = new PaddedVolatileFlag(false);

            public long WaitFor(Sequence cursor, long sequence)
            {
                long availableSequence;
                while ((availableSequence = cursor.Value) < sequence && !interrupt.IsSet())
                {
                    Thread.Yield();
                }
                return availableSequence;
            }

            public void NotifyAll()
            {
            }

            public void InterruptAll()
            {
                interrupt.Set();
            }

            public void ClearInterrupt()
            {
                interrupt.Clear();
            }
        }
    }
}