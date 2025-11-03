// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

#pragma warning disable 420 // volatile semantics not lost as only call by 'ref' are interlocked
namespace FortitudeCommon.DataStructures.MemoryPools;

/// <summary>
///     Garbage and lock free pooled factory.  If loops are exhausted then new objects are created but even under
///     high load this is rare to non-existant.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class GarbageAndLockFreePooledFactory<T> : IPooledFactory<T>, IEnumerable<T>
{
    private const int MaxAttempts = 1_000_000; // number of attempts before giving up and creating a new object

    private const int UnlockedElement      = 0;
    private const int UnlockedSpare        = int.MaxValue;
    private const int StartActualWaitIndex = 3;
    private const int NumberOfSpinLengths  = 10;
    private const int MinRedistributeSpins = 10;  // smallest reattempt wait time 
    private const int MaxRedistributeSpins = 100; // largest reattempt wait time

    // number of missed transfers
    private const int ThreadSpinGap = MaxRedistributeSpins - MinRedistributeSpins / NumberOfSpinLengths; //10% of attempts will re-attempt straightway
    private readonly Func<T> factory;

    private volatile Node elementHead;

    private GarbageAndLockFreePooledFactory<ElementNodesEnumerator>? enumeratorPool;

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
            var count       = 0;
            var currentNode = elementHead.NextElement;
            while (currentNode != null)
            {
                count++;
                currentNode = currentNode.NextElement;
            }
            return count;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
        newOrStoredNode.OwningThreadId = UnlockedElement;
        newOrStoredNode.Item           = item;
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        for (var i = 0; i < MaxAttempts; i++)
        {
            var oldFirst = elementHead.NextElement;
            if (oldFirst != null)
                if (Interlocked.CompareExchange(ref oldFirst.OwningThreadId, currentThreadId, UnlockedElement) != UnlockedElement)
                {
                    DistributeReattemptTime(i);
                    continue;
                }
            newOrStoredNode.NextElement = oldFirst;
            if (Interlocked.CompareExchange(ref elementHead.NextElement, newOrStoredNode, oldFirst) == oldFirst)
            {
                if (oldFirst != null) Interlocked.CompareExchange(ref oldFirst.OwningThreadId, UnlockedElement, currentThreadId);
                return;
            }
            if (oldFirst != null) Interlocked.CompareExchange(ref oldFirst.OwningThreadId, UnlockedElement, currentThreadId);

            DistributeReattemptTime(i);
        }
    }

    public void ReturnBorrowed(object item)
    {
        ReturnBorrowed((T)item);
    }

    public T Borrow()
    {
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        for (var i = 0; i < MaxAttempts; i++)
        {
            var first = elementHead.NextElement;
            if (first == null) return factory();
            var second = first.NextElement;
            if (second != null)
                if (Interlocked.CompareExchange(ref second.OwningThreadId, currentThreadId, UnlockedElement) != UnlockedElement)
                {
                    DistributeReattemptTime(i);
                    continue;
                }
            if (Interlocked.CompareExchange(ref first.OwningThreadId, currentThreadId, UnlockedElement) != UnlockedElement)
            {
                if (second != null) Interlocked.CompareExchange(ref second.OwningThreadId, UnlockedElement, currentThreadId);
                DistributeReattemptTime(i);
                continue;
            }
            if (Interlocked.CompareExchange(ref elementHead.NextElement, second, first) == first)
            {
                if (second != null) Interlocked.CompareExchange(ref second.OwningThreadId, UnlockedElement, currentThreadId);

                var toReturn = first.Item!;
                first.Item = default;
                ReturnNode(first);

                return toReturn;
            }
            if (second != null) Interlocked.CompareExchange(ref second.OwningThreadId, UnlockedElement, currentThreadId);
            Interlocked.CompareExchange(ref first.OwningThreadId, UnlockedElement, currentThreadId);
            DistributeReattemptTime(i);
        }

        return factory();
    }

    object IPooledFactory.Borrow() => Borrow()!;

    private void InitializeLists(int reservePoolOfNodes)
    {
        for (var i = 0; i < reservePoolOfNodes; i++) ReturnNode(new Node()); // create a buffer between used Nodes and pooled Nodes.
    }

    public bool Remove(T itemToRemove)
    {
        var wasFoundInQueue = false;
        for (var i = 0; i < MaxAttempts; i++)
        {
            var currentItem = elementHead.NextElement;
            var previous    = elementHead;
            while (currentItem != null)
            {
                if (ReferenceEquals(currentItem.Item, itemToRemove)) break;

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
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        usedNode.NextElement    = null;
        usedNode.OwningThreadId = UnlockedSpare;
        for (var i = 0; i < MaxAttempts; i++)
        {
            var oldFirst = spareNodeHead.NextSpareNode;
            if (oldFirst != null)
                if (Interlocked.CompareExchange(ref oldFirst.OwningThreadId, currentThreadId, UnlockedSpare) != UnlockedSpare)
                {
                    DistributeReattemptTime(i);
                    continue;
                }
            usedNode.NextSpareNode = oldFirst;
            if (Interlocked.CompareExchange(ref spareNodeHead.NextSpareNode, usedNode, oldFirst) == oldFirst)
            {
                if (oldFirst != null) Interlocked.CompareExchange(ref oldFirst.OwningThreadId, UnlockedSpare, currentThreadId);
                return;
            }
            if (oldFirst != null) Interlocked.CompareExchange(ref oldFirst.OwningThreadId, UnlockedSpare, currentThreadId);

            DistributeReattemptTime(i);
        }
    }

    private Node PooledNodeOrCreate()
    {
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        for (var i = 0; i < MaxAttempts; i++)
        {
            var first = spareNodeHead.NextSpareNode;
            if (first == null) return new Node();
            var second = first.NextSpareNode;
            if (second != null)
                if (Interlocked.CompareExchange(ref second.OwningThreadId, currentThreadId, UnlockedSpare) != UnlockedSpare)
                {
                    DistributeReattemptTime(i);
                    continue;
                }
            if (Interlocked.CompareExchange(ref first.OwningThreadId, currentThreadId, UnlockedSpare) != UnlockedSpare)
            {
                if (second != null) Interlocked.CompareExchange(ref second.OwningThreadId, UnlockedSpare, currentThreadId);
                DistributeReattemptTime(i);
                continue;
            }
            if (Interlocked.CompareExchange(ref spareNodeHead.NextSpareNode, second, first) == first)
            {
                first.NextSpareNode = null;
                if (second != null) Interlocked.CompareExchange(ref second.OwningThreadId, UnlockedSpare, currentThreadId);
                return first;
            }
            if (second != null) Interlocked.CompareExchange(ref second.OwningThreadId, UnlockedSpare, currentThreadId);
            Interlocked.CompareExchange(ref first.OwningThreadId, UnlockedSpare, currentThreadId);
            DistributeReattemptTime(i);
        }

        return new Node();
    }

    private void DistributeReattemptTime(int attemptCount)
    {
        if (attemptCount < StartActualWaitIndex || attemptCount % 3 == 0) return;
        var spinSlot = attemptCount % NumberOfSpinLengths;

        var reattemptSpins = MinRedistributeSpins + spinSlot * ThreadSpinGap;
        for (var i = reattemptSpins; i > 0; i--)
            // spin baby spin.
            // and do something requiring calculation
            if (i == MinRedistributeSpins)
                i--;
    }

    private sealed class Node
    {
        public T?    Item;
        public Node? NextElement;   //used by the list maintaining cached Items
        public Node? NextSpareNode; //used by spareNodeHead to store empty available nodes
        public int   OwningThreadId;
    }

    private sealed class ElementNodesEnumerator : IEnumerator<T>
    {
        private readonly GarbageAndLockFreePooledFactory<ElementNodesEnumerator> enumeratorReturnPool;

        private Node? currentNode;
        private Node? originalNode;

        public ElementNodesEnumerator
            (GarbageAndLockFreePooledFactory<ElementNodesEnumerator> enumeratorReturnPool) =>
            this.enumeratorReturnPool = enumeratorReturnPool;

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
