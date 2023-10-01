#region

using System.Collections;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Metrics;

#endregion

#pragma warning disable 420 // volatile semantics not lost as only call by 'ref' are interlocked
namespace FortitudeCommon.DataStructures.Memory
{
    /// <summary>
    /// Garbage and lock free pooled factory.  If loops are exhausted then new objects are created but even under
    /// high load this is rare to non-existant.  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GarbageAndLockFreePooledFactory<T> : IPooledFactory<T>, IEnumerable<T>
    {
        private const int MaxAttempts = 1_000_000; // number of attempts before giving up and creating a new object

        private const int MaxRandomSpins = 20_000; // targetting quickest execution time with lowest 

        // number of missed transfers
        private const int PassThroughSpins = MaxRandomSpins / 50; //10% of attempts will re-attempt straightway
        private readonly Func<T> factory;

        private readonly Random random = new Random(TimeContext.UtcNow.Millisecond);

        private volatile Node? elementHead;
        private volatile Node? elementTail;

        private GarbageAndLockFreePooledFactory<ElementNodesEnumerator>? enumeratorPool;

        internal IMetricRepository<QueueMetrics> MetricRepo = new MetricRepository<QueueMetrics>(
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.MissedNodeReturns),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.MissedNodeBorrow),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.NodeReturns),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.NodeReturnLoopExhausted),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.NodeBorrowLoopExhausted),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.NodesCreated),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.CacheNodeReturned),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.MissedBorrowReturns),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.MissedBorrows),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.ItemsCreated),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.CachedItemReturned),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.NextItemIsNull),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.ItemQueueEmpty),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.BorrowOnHeadInProgress),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.ItemReturned),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.ItemMissedReturned),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.ItemReturnLoopExhausted),
            new CounterMetricRecorder<QueueMetrics>(QueueMetrics.BorrowLoopExhausted));

        private volatile Node? spareNodeHead;
        private volatile Node? spareNodeTail;

        public GarbageAndLockFreePooledFactory(Func<GarbageAndLockFreePooledFactory<T>, T> factory,
            int reservePoolOfNodes = 1)
        {
            this.factory = () => factory(this);
            InitializeLists(reservePoolOfNodes);
        }

        public GarbageAndLockFreePooledFactory(Func<T> factory, int reservePoolOfNodes = 1)
        {
            this.factory = factory;
            InitializeLists(reservePoolOfNodes);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (enumeratorPool == null)
            {
                enumeratorPool = new GarbageAndLockFreePooledFactory<ElementNodesEnumerator>(
                    thisPool => new ElementNodesEnumerator(thisPool));
            }

            var enumerator = enumeratorPool.Borrow();
            enumerator.SetStartingNode(elementHead);
            return enumerator;
        }

        public void ReturnBorrowed(T item)
        {
            Node newNode = PooledNodeOrCreate();
            //Node newNode = new Node();
            newNode.Item = item;
            newNode.NextElement = null;
            newNode.NodeInstanceAttemptCount += 100;
            newNode.NodeSelectionCount = newNode.NodeInstanceAttemptCount;
            Thread.MemoryBarrier();
            for (int i = 0; i < MaxAttempts; i++)
            {
                Node? curTail = elementTail!;
                int instanceCount = curTail.ElementInstanceAttemptCount;
                if (Interlocked.Increment(ref curTail.ElementSelectionCount) == instanceCount + 1)
                    // required because tail node could be dequeued, requeud and appended to before the following executes
                    // prevents circular lists where tail points back to nodes from the head.
                {
                    if (curTail != newNode)
                    {
                        Node? elementNext;
                        if ((elementNext = Interlocked.CompareExchange(ref curTail.NextElement, newNode, null)) == null)
                        {
                            MetricRepo[QueueMetrics.ItemReturned].Increment();
                            Interlocked.CompareExchange(ref elementTail, newNode, curTail);
                            return;
                        }

                        Interlocked.CompareExchange(ref elementTail, elementNext, curTail);
                    }
                }

                Interlocked.Decrement(ref curTail.ElementSelectionCount);

                if (i > 100) // take a rest and see if the blocking thread can finish.
                {
                    Thread.Yield();
                }

                MetricRepo[QueueMetrics.ItemMissedReturned].Increment();
                DistributeReattemptTime(i);
            }

            MetricRepo[QueueMetrics.ItemReturnLoopExhausted].Increment();
        }

        public void ReturnBorrowed(object item)
        {
            ReturnBorrowed((T)item);
        }

        public T Borrow()
        {
            for (int i = 0; i < MaxAttempts; i++)
            {
                Node curHead = elementHead!;
                Node curTail = elementTail!;
                int queueCount = curHead.NodeInstanceAttemptCount;
                if (Interlocked.Increment(ref curHead.NodeSelectionCount) == queueCount + 1)
                    // when nodes are reused to prevent curHead being dequeud and requed and put to front of queue
                {
                    Node? curHeadNext = curHead.NextElement;
                    if (curHead == curTail)
                    {
                        if (curHeadNext == null)
                        {
                            MetricRepo[QueueMetrics.ItemsCreated].Increment();
                            Interlocked.Decrement(ref curHead.NodeSelectionCount);
                            return factory();
                        }

                        Interlocked.CompareExchange(ref elementTail, curHeadNext, curTail);
                        MetricRepo[QueueMetrics.ItemQueueEmpty].Increment();
                    }
                    else
                    {
                        if (curHeadNext != null && curHeadNext != curHead)
                        {
                            T? cachedItem = curHeadNext.Item;
                            if (cachedItem != null &&
                                Interlocked.CompareExchange(ref elementHead, curHeadNext, curHead) == curHead)
                            {
                                curHead.ElementSelectionCount = 0;
                                ReturnNode(curHead);
                                MetricRepo[QueueMetrics.CachedItemReturned].Increment();
                                return cachedItem;
                            }
                        }
                        else
                        {
                            MetricRepo[QueueMetrics.NextItemIsNull].Increment();
                        }
                    }
                }

                Interlocked.Decrement(ref curHead.NodeSelectionCount);
                MetricRepo[QueueMetrics.MissedBorrows].Increment();
                DistributeReattemptTime(i);
            }

            MetricRepo[QueueMetrics.BorrowLoopExhausted].Increment();
            MetricRepo[QueueMetrics.ItemsCreated].Increment();
            return factory();
        }

        object IPooledFactory.Borrow()
        {
            return Borrow()!;
        }

        private void InitializeLists(int reservePoolOfNodes)
        {
            elementHead = elementTail = new Node();
            spareNodeHead = spareNodeTail = new Node { ElementSelectionCount = 100 };
            MetricRepo[QueueMetrics.NodesCreated].Recorder(2);
            for (int i = 0; i < reservePoolOfNodes; i++)
            {
                ReturnNode(new Node()); // create a buffer between used Nodes and pooled Nodes.
                MetricRepo[QueueMetrics.NodesCreated].Increment();
            }
        }

        public bool Remove(T itemToRemove)
        {
            bool wasFoundInQueue = false;
            for (int i = 0; i < MaxAttempts; i++)
            {
                Node? currentItem = elementHead;
                Node previous = elementHead!;
                while (currentItem != null)
                {
                    if (ReferenceEquals(currentItem.Item, itemToRemove))
                    {
                        break;
                    }

                    previous = currentItem;
                    currentItem = currentItem.NextElement;
                }

                if (currentItem == null) return wasFoundInQueue;

                wasFoundInQueue = true;
                Node curHead = elementHead!;
                Node curTail = elementTail!;
                int currQueueCount = currentItem.NodeInstanceAttemptCount;
                int prevQueueCount = previous.NodeInstanceAttemptCount;
                if (Interlocked.Increment(ref currentItem.NodeSelectionCount) == currQueueCount + 1)
                    // when nodes are reused to prevent curHead being dequeud and requed and put to front of queue
                {
                    Node? curItemNext = currentItem.NextElement;
                    if (curHead == curTail)
                    {
                        if (curItemNext == null)
                        {
                            Interlocked.Decrement(ref currentItem.NodeSelectionCount);
                            return true;
                        }

                        Interlocked.CompareExchange(ref elementTail, curItemNext, curTail);
                        MetricRepo[QueueMetrics.ItemQueueEmpty].Increment();
                    }
                    else
                    {
                        if (Interlocked.Increment(ref previous.NodeSelectionCount) == prevQueueCount + 1)
                        {
                            if (previous == curHead && curItemNext != null)
                            {
                                if (Interlocked.CompareExchange(ref elementHead, curItemNext, curHead) == curHead)
                                {
                                    // ReSharper disable once PossibleNullReferenceException
                                    curHead.ElementSelectionCount = 0;
                                    ReturnNode(curHead);
                                    return true;
                                }
                            }
                            else
                            {
                                if (Interlocked.CompareExchange(ref previous.NextElement, curItemNext, currentItem)
                                    == currentItem)
                                {
                                    Interlocked.Decrement(ref previous.NodeSelectionCount);
                                    currentItem.ElementSelectionCount = 0;
                                    ReturnNode(curHead);
                                    return true;
                                }
                            }
                        }

                        Interlocked.Decrement(ref previous.NodeSelectionCount);
                    }
                }

                Interlocked.Decrement(ref currentItem.NodeSelectionCount);
                DistributeReattemptTime(i);
            }

            return wasFoundInQueue;
        }

        private void ReturnNode(Node usedNode)
        {
            usedNode.NextElement = null;
            usedNode.SpareNodeNext = null;
            usedNode.NodeInstanceAttemptCount += 100;
            usedNode.NodeSelectionCount = usedNode.NodeInstanceAttemptCount;
            Thread.MemoryBarrier();
            for (int i = 0; i < MaxAttempts; i++)
            {
                Node curTail = spareNodeTail!;
                if (Interlocked.Increment(ref curTail.ElementSelectionCount) == 101)
                {
                    if (usedNode != curTail) // increment as is being used by this thread
                    {
                        Node? tailNext;
                        if ((tailNext = Interlocked.CompareExchange(ref curTail.SpareNodeNext, usedNode, null)) == null)
                        {
                            Interlocked.CompareExchange(ref spareNodeTail, usedNode, curTail);
                            Interlocked.Add(ref usedNode.ElementSelectionCount, 100);
                            MetricRepo[QueueMetrics.NodeReturns].Increment();
                            return;
                        }

                        Interlocked.CompareExchange(ref spareNodeTail, tailNext, curTail);
                    }
                }

                Interlocked.Decrement(ref curTail.ElementSelectionCount);
                if (i > 100)
                {
                    Thread.Yield();
                }

                MetricRepo[QueueMetrics.MissedNodeReturns].Increment();
                DistributeReattemptTime(i);
            }

            MetricRepo[QueueMetrics.NodeReturnLoopExhausted].Increment();
        }

        private Node PooledNodeOrCreate()
        {
            for (int i = 0; i < MaxAttempts; i++)
            {
                Node curHead = spareNodeHead!;
                int queueCount = curHead.NodeInstanceAttemptCount;
                if (Interlocked.Increment(ref curHead.NodeSelectionCount) ==
                    queueCount + 1) // increment as is being used by this thread
                {
                    Node curTail = spareNodeTail!;
                    Node? curHeadNext = curHead.SpareNodeNext;
                    if (curHead == curTail)
                    {
                        if (curHeadNext == null)
                        {
                            Interlocked.Decrement(ref curHead.NodeSelectionCount);
                            MetricRepo[QueueMetrics.NodesCreated].Increment();
                            return new Node();
                        }

                        Interlocked.CompareExchange(ref spareNodeTail, curHeadNext, curTail);
                    }
                    else
                    {
                        if (curHeadNext != null)
                        {
                            if (Interlocked.CompareExchange(ref spareNodeHead, curHeadNext, curHead) == curHead)
                            {
                                MetricRepo[QueueMetrics.CacheNodeReturned].Increment();
                                curHead.ElementInstanceAttemptCount += 100;
                                curHead.ElementSelectionCount = curHead.ElementInstanceAttemptCount;
                                return curHead;
                            }
                        }
                    }

                    Interlocked.Decrement(ref curHead.NodeSelectionCount);
                }
                else
                {
                    Interlocked.Decrement(ref curHead.NodeSelectionCount);
                    Thread.Yield();
                }

                MetricRepo[QueueMetrics.MissedNodeBorrow].Increment();
                DistributeReattemptTime(i);
            }

            MetricRepo[QueueMetrics.NodeBorrowLoopExhausted].Increment();
            MetricRepo[QueueMetrics.NodesCreated].Increment();
            return new Node();
        }

        private void DistributeReattemptTime(int attemptCount)
        {
            if (attemptCount < 2) return;
            int reattemptSpins = random.Next(0, MaxRandomSpins);
            if (reattemptSpins > PassThroughSpins)
            {
                // ReSharper disable once EmptyForStatement
                for (int i = 0; i < reattemptSpins; i++)
                {
                    // spin baby spin.
                }
            }
        }

        private sealed class Node
        {
            // threads are still using a reference to this node 
            public int ElementInstanceAttemptCount; // used by borrow to ensure no other 
            public int ElementSelectionCount; // used to ensure a full loop of a tail is not made between queues
            public T? Item;
            public Node? NextElement; //used by the list maintaining cached Items

            public int NodeInstanceAttemptCount; // used by pooledNodeOrCreate to ensure no other 

            // threads are still using a reference to this node 
            public int NodeSelectionCount; // used with QueueCount to ensure only one node is selected
            public Node? SpareNodeNext; // used by the list storing nodes in and available pool
        }

        private sealed class ElementNodesEnumerator : IEnumerator<T>
        {
            private readonly GarbageAndLockFreePooledFactory<ElementNodesEnumerator> enumeratorReturnPool;
            private Node? currentNode;
            private Node? originalNode;

            public ElementNodesEnumerator(GarbageAndLockFreePooledFactory<ElementNodesEnumerator> enumeratorReturnPool)
            {
                this.enumeratorReturnPool = enumeratorReturnPool;
            }

            public void Dispose()
            {
                enumeratorReturnPool.ReturnBorrowed(this);
            }

            public bool MoveNext()
            {
                currentNode = currentNode?.NextElement;
                return currentNode != null;
            }

            public void Reset()
            {
                currentNode = originalNode;
            }

            object IEnumerator.Current => Current!;
            public T Current => currentNode!.Item!;

            internal void SetStartingNode(Node? currNode)
            {
                currentNode = originalNode = currNode;
            }
        }

        internal enum QueueMetrics
        {
            MissedNodeReturns
            , MissedNodeBorrow
            , NodeReturns
            , NodeReturnLoopExhausted
            , NodeBorrowLoopExhausted
            , NodesCreated
            , CacheNodeReturned
            , MissedBorrowReturns
            , MissedBorrows
            , BorrowLoopExhausted
            , ItemsCreated
            , NextItemIsNull
            , ItemQueueEmpty
            , BorrowOnHeadInProgress
            , CachedItemReturned
            , ItemReturned
            , ItemMissedReturned
            , ItemReturnLoopExhausted
        }
    }
}
