// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.DataStructures.Lists.LinkedLists;

public class DoublyLinkedList<T> : RecyclableObject, IDoublyLinkedList<T> where T : class, IDoublyLinkedListNode<T>
{
    public T? Head { get; private set; }
    public T? Tail { get; private set; }

    public bool IsEmpty => Head == null;

    public IEnumerator<T> GetEnumerator()
    {
        var next = Head;
        for (var node = next; node != null; node = next)
        {
            next = node.Next;
            yield return node;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T AddFirst(T node)
    {
        if (Head == null)
        {
            Head      = Tail = node;
            node.Next = null;

            node.Previous = null;
        }
        else
        {
            Head.Previous = node;

            node.Next = Head;
            Head      = node;

            node.Previous = null;
        }

        return node;
    }

    public T AddLast(T node)
    {
        if (Head == null)
        {
            Head = Tail = node;

            node.Next     = null;
            node.Previous = null;
        }
        else
        {
            node.Previous = Tail;

            Tail!.Next = node;
            Tail       = node;
            node.Next  = null;
        }

        return node;
    }

    public bool UnsafeContains(T node) => node.Previous != null || node.Next != null || node == Head;

    public bool SafeContains(T node)
    {
        foreach (var n in this)
            if (n == node)
                return true;
        return false;
    }


    public int Count
    {
        get
        {
            var count = 0;
            var next  = Head;
            for (var node = next; node != null; node = next)
            {
                count++;
                next = node.Next;
            }
            return count;
        }
    }

    public T Remove(T node)
    {
        if (node == Head && node == Tail)
        {
            Head = Tail = null;
        }
        else if (node == Head)
        {
            (Head = node.Next!).Previous = null;
        }
        else if (node == Tail)
        {
            (Tail = node.Previous!).Next = null;
        }
        else if (node.Previous != null && node.Next != null)
        {
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
        }

        node.Previous = node.Next = null;
        return node;
    }

    public void DetachNodes()
    {
        Head = null;
        Tail = null;
    }

    public void Swap(DoublyLinkedList<T> doublyLinkedList)
    {
        var head = Head;
        var tail = Tail;

        Head = doublyLinkedList.Head;
        Tail = doublyLinkedList.Tail;

        doublyLinkedList.Head = head;
        doublyLinkedList.Tail = tail;
    }

    public override void StateReset()
    {
        var currentNode = Head;
        while (currentNode != null)
        {
            if (currentNode is IRecyclableObject recyclableNode) recyclableNode.DecrementRefCount();
            currentNode = currentNode.Next;
        }
        Clear();

        base.StateReset();
    }

    public void Clear()
    {
        Head = Tail = null;
    }
}
