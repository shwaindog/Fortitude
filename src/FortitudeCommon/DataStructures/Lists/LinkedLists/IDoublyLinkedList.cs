using System.Collections.Generic;

namespace FortitudeCommon.DataStructures.LinkedLists
{
    public interface IDoublyLinkedList<T> : IEnumerable<T> where T : class, IDoublyLinkedListNode<T>
    {
        T Head { get; }
        T Tail { get; }
        bool IsEmpty { get; }
        T AddFirst(T node);
        T AddLast(T node);
        bool UnsafeContains(T node);
        bool SafeContains(T node);
        T Remove(T node);
        void Swap(DoublyLinkedList<T> doublyLinkedList);
        void Clear();
    }
}