// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.DataStructures.Lists.LinkedLists;

public interface IDoublyLinkedList<T> : IEnumerable<T> where T : class, IDoublyLinkedListNode<T>
{
    T?   Head    { get; }
    T?   Tail    { get; }
    bool IsEmpty { get; }

    int  Count { get; }
    void DetachNodes();
    T    AddFirst(T node);
    T    AddLast(T node);
    bool UnsafeContains(T node);
    bool SafeContains(T node);
    T    Remove(T node);
    void Swap(DoublyLinkedList<T> doublyLinkedList);
    void Clear();
}
