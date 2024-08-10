// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.Chronometry;

#endregion

#pragma warning disable 420 // volatile semantics not lost as only call by 'ref' are interlocked
namespace FortitudeCommon.DataStructures.Memory
{
    /// <summary>
    ///     Garbage and lock free pooled factory.  If loops are exhausted then new objects are created but even under
    ///     high load this is rare to non-existant.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GarbageAndLockFreePooledFactory<T> : IPooledFactory<T>, IEnumerable<T>
    {
        private const int MaxAttempts = 1_000_000; // number of attempts before giving up and creating a new object

        private const int MaxRandomSpins        = 20_000; // targetting quickest execution time with lowest 
        private const int MinimumCycleStartSlot = 6;
        private const int SlotIncrement         = 2;
        private const int MaxCycleSlotDepth     = 64;

        // number of missed transfers
        private const    int     PassThroughSpins = MaxRandomSpins / 50; //10% of attempts will re-attempt straightway
        private readonly Func<T> factory;

        private readonly Random random = new Random(TimeContext.UtcNow.Millisecond);

        private volatile Node elementHead;

        private GarbageAndLockFreePooledFactory<ElementNodesEnumerator>? enumeratorPool;

        private int slotIndex = 0;

        private volatile Node spareNodeHead;

        public GarbageAndLockFreePooledFactory
        (Func<GarbageAndLockFreePooledFactory<T>, T> factory,
            int reservePoolOfNodes = 1)
        {
            elementHead   = new Node();
            spareNodeHead = new Node();
            this.factory  = () => factory(this);
            InitializeLists(reservePoolOfNodes);
        }

        public GarbageAndLockFreePooledFactory(Func<T> factory, int reservePoolOfNodes = 1)
        {
            elementHead   = new Node();
            spareNodeHead = new Node();
            this.factory  = factory;
            InitializeLists(reservePoolOfNodes);
        }

        public int Count
        {
            get
            {
                int count       = 0;
                var currentNode = elementHead.NextElement;
                while (currentNode != null)
                {
                    count++;
                    currentNode = currentNode.NextElement;
                }
                return count;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            enumeratorPool ??= new GarbageAndLockFreePooledFactory<ElementNodesEnumerator>
                (thisPool => new ElementNodesEnumerator(thisPool));

            var enumerator = enumeratorPool.Borrow();
            enumerator.SetStartingNode(elementHead);
            return enumerator;
        }

        public void ReturnBorrowed(T item)
        {
            Node newOrStoredNode = PooledNodeOrCreate();
            newOrStoredNode.AccessTokenRequired = newOrStoredNode.CurrentTokenValue + 1;
            newOrStoredNode.Item                = item;
            for (int i = 0; i < MaxAttempts; i++)
            {
                Node? currentFirst = elementHead.NextElement;
                newOrStoredNode.NextElement = currentFirst;
                if (Interlocked.CompareExchange(ref elementHead.NextElement, newOrStoredNode, currentFirst) == currentFirst)
                {
                    return;
                }

                DistributeReattemptTime(i);
            }
        }

        public void ReturnBorrowed(object item)
        {
            ReturnBorrowed((T)item);
        }

        public T Borrow()
        {
            for (var i = 0; i < MaxAttempts; i++)
            {
                var currentItem = elementHead.NextElement;

                if (currentItem == null)
                {
                    return factory();
                }
                var last   = elementHead;
                var count  = 0;
                var stopAt = (MaxCycleSlotDepth - Interlocked.Add(ref slotIndex, SlotIncrement) % MaxCycleSlotDepth) + MinimumCycleStartSlot;
                while (currentItem is { NextElement: not null, NextSpareNode: null } && spareNodeHead?.NextElement != currentItem && count++ < stopAt)
                {
                    last        = currentItem;
                    currentItem = currentItem.NextElement;
                }
                if (currentItem is not { NextSpareNode: null } || spareNodeHead?.NextElement == currentItem || last.NextSpareNode != null)
                {
                    continue;
                }
                var threadToken = Interlocked.Increment(ref currentItem.CurrentTokenValue);
                if (threadToken == currentItem.AccessTokenRequired)
                {
                    var skipTo = currentItem.NextElement;
                    if (Interlocked.CompareExchange(ref last.NextElement, skipTo, currentItem) == currentItem)
                    {
                        var toReturn = currentItem.Item!;
                        currentItem.Item                = default;
                        currentItem.AccessTokenRequired = currentItem.CurrentTokenValue + 1;
                        ReturnNode(currentItem);
                        return toReturn;
                    }
                    currentItem.AccessTokenRequired += 3;
                    continue;
                }
                if (currentItem.AccessTokenRequired + 1 < threadToken)
                {
                    currentItem.AccessTokenRequired += 3;
                    continue;
                }
                if (threadToken < currentItem.AccessTokenRequired)
                {
                    continue;
                }
                DistributeReattemptTime(i);
            }

            return factory();
        }

        object IPooledFactory.Borrow()
        {
            return Borrow()!;
        }

        private void InitializeLists(int reservePoolOfNodes)
        {
            for (int i = 0; i < reservePoolOfNodes; i++)
            {
                ReturnNode(new Node()); // create a buffer between used Nodes and pooled Nodes.
            }
        }

        public bool Remove(T itemToRemove)
        {
            bool wasFoundInQueue = false;
            for (int i = 0; i < MaxAttempts; i++)
            {
                var currentItem = elementHead.NextElement;
                var previous    = elementHead;
                while (currentItem != null)
                {
                    if (ReferenceEquals(currentItem.Item, itemToRemove))
                    {
                        break;
                    }

                    previous    = currentItem;
                    currentItem = currentItem.NextElement;
                }

                if (currentItem == null) return wasFoundInQueue;

                wasFoundInQueue = true;
                var foundNext = currentItem.NextElement;

                if (Interlocked.CompareExchange(ref previous.NextElement, foundNext, currentItem) == currentItem)
                {
                    currentItem.Item        = default;
                    currentItem.NextElement = null;
                    ReturnNode(currentItem);
                }
                DistributeReattemptTime(i);
            }

            return wasFoundInQueue;
        }

        private void ReturnNode(Node usedNode)
        {
            usedNode.NextElement = null;
            for (int i = 0; i < MaxAttempts; i++)
            {
                var firstNode = spareNodeHead.NextSpareNode;
                usedNode.NextSpareNode = firstNode;
                if (Interlocked.CompareExchange(ref spareNodeHead.NextSpareNode, usedNode, firstNode) == firstNode)
                {
                    return;
                }

                DistributeReattemptTime(i);
            }
        }

        private Node PooledNodeOrCreate()
        {
            for (var i = 0; i < MaxAttempts; i++)
            {
                var currentItem = spareNodeHead.NextSpareNode;
                if (currentItem == null)
                {
                    return new Node();
                }
                var last   = spareNodeHead;
                var count  = 0;
                var stopAt = (MaxCycleSlotDepth - Interlocked.Add(ref slotIndex, SlotIncrement) % MaxCycleSlotDepth) + MinimumCycleStartSlot;
                while (currentItem is { NextSpareNode: not null, NextElement: null }
                    && elementHead?.NextElement != currentItem
                    && elementHead?.NextElement != currentItem && count++ < stopAt)
                {
                    last        = currentItem;
                    currentItem = currentItem.NextSpareNode;
                }
                if (currentItem is not { NextElement: null } || elementHead?.NextElement == currentItem || last.NextElement != null)
                {
                    continue;
                }
                var threadToken = Interlocked.Increment(ref currentItem.CurrentTokenValue);
                if (threadToken == currentItem.AccessTokenRequired)
                {
                    var skipTo = currentItem.NextSpareNode;
                    if (Interlocked.CompareExchange(ref last.NextSpareNode, skipTo, currentItem) == currentItem)
                    {
                        currentItem.NextSpareNode = null;
                        return currentItem;
                    }
                    currentItem.AccessTokenRequired += 3;
                    continue;
                }
                if (currentItem.AccessTokenRequired + 1 < threadToken)
                {
                    currentItem.AccessTokenRequired = currentItem.AccessTokenRequired + 3;
                    continue;
                }
                if (threadToken < currentItem.AccessTokenRequired)
                {
                    continue;
                }
                DistributeReattemptTime(i);
            }

            return new Node();
        }

        private void DistributeReattemptTime(int attemptCount)
        {
            if (attemptCount < 10 && attemptCount % 7 == 0) return;
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
            public int   AccessTokenRequired;
            public int   CurrentTokenValue;
            public T?    Item;
            public Node? NextElement;   //used by the list maintaining cached Items
            public Node? NextSpareNode; //used by spareNodeHead to store empty available nodes
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
    }
}
