using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal static class WaitStrategies
    {
        public static IWaitStrategyInt GetInstance(this WaitStrategyType type, int ringMask)
        {
            switch (type)
            {
                case WaitStrategyType.BlockingMultiConsumers:
                    return new BlockingMultiConsumersWaitStrategy(ringMask);
                case WaitStrategyType.BlockingSingleConsumer:
                    return new BlockingSingleConsumerWaitStrategy(ringMask);
                case WaitStrategyType.Yielding:
                    return new YieldingWaitStrategy(ringMask);
                case WaitStrategyType.Spinning:
                    return new SpinningWaitStrategy(ringMask);
                default:
                    throw new InvalidOperationException("Unsupported strategy type");
            }
        }

        public static IWaitStrategyLong GetInstanceLong(this WaitStrategyType type)
        {
            switch (type)
            {
                case WaitStrategyType.BlockingMultiConsumers:
                    return new BlockingMultiConsumersWaitStrategyLong();
                case WaitStrategyType.BlockingSingleConsumer:
                    return new BlockingSingleConsumerWaitStrategyLong();
                case WaitStrategyType.Yielding:
                    return new YieldingWaitStrategyLong();
                case WaitStrategyType.Spinning:
                    return new SpinningWaitStrategyLong();
                default:
                    throw new InvalidOperationException("Unsupported strategy type");
            }
        }

        private sealed class BlockingMultiConsumersWaitStrategy(int ringMask) : IWaitStrategyInt
        {
            private readonly object sync = new ();
            private PaddedVolatileFlag interrupt = new (false);

            public int WaitFor(Sequence cursor, int sequence)
            {
                sequence &= ringMask;
                var availableSequence = cursor.Value & ringMask;
                if (availableSequence < sequence)
                {
                    lock (sync)
                    {
                        while ((availableSequence = cursor.Value & ringMask) != sequence && !interrupt.IsSet())
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

        private sealed class BlockingMultiConsumersWaitStrategyLong : IWaitStrategyLong
        {
            private readonly object sync = new ();
            private PaddedVolatileFlag interrupt = new (false);

            public long WaitFor(SequenceLong cursor, long sequence)
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

        private sealed class BlockingSingleConsumerWaitStrategy(int ringMask) : IWaitStrategyInt
        {
            private readonly AutoResetEvent resetEvent = new (false);
            private PaddedVolatileFlag interrupt = new (false);

            public int WaitFor(Sequence cursor, int sequence)
            {
                sequence &= ringMask;
                var availableSequence = cursor.Value & ringMask;
                if (availableSequence < sequence)
                {
                    while ((availableSequence = cursor.Value & ringMask) != sequence && !interrupt.IsSet())
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

        private sealed class BlockingSingleConsumerWaitStrategyLong : IWaitStrategyLong
        {
            private readonly AutoResetEvent resetEvent = new (false);
            private PaddedVolatileFlag interrupt = new (false);

            public long WaitFor(SequenceLong cursor, long sequence)
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

        private sealed class YieldingWaitStrategy(int ringMask) : IWaitStrategyInt
        {
            private PaddedVolatileFlag interrupt = new (false);

            public int WaitFor(Sequence cursor, int sequence)
            {
                sequence &= ringMask;
                int availableSequence;
                while ((availableSequence = cursor.Value & ringMask) != sequence && !interrupt.IsSet())
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

        private sealed class YieldingWaitStrategyLong : IWaitStrategyLong
        {
            private PaddedVolatileFlag interrupt = new (false);

            public long WaitFor(SequenceLong cursor, long sequence)
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

        private sealed class SpinningWaitStrategy(int ringMask) : IWaitStrategyInt
        {
            private PaddedVolatileFlag interrupt = new (false);

            public int WaitFor(Sequence cursor, int sequence)
            {
                sequence &= ringMask;
                int availableSequence;
                while ((availableSequence = cursor.Value & ringMask) != sequence && !interrupt.IsSet())
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

        private sealed class SpinningWaitStrategyLong : IWaitStrategyLong
        {
            private PaddedVolatileFlag interrupt = new (false);

            public long WaitFor(SequenceLong cursor, long sequence)
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
