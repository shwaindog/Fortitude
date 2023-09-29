using System.Collections;
using System.Collections.Generic;

namespace FortitudeCommon.DataStructures.LinkedLists
{
    public class SortedDoublyLinkedList<T> : IEnumerable<T> where T : class, IDoublyLinkedListNode<T>
    {
        private readonly IComparer<T> comparer;
        private readonly DoublyLinkedList<T> doublyLinkedList = new DoublyLinkedList<T>();

        public SortedDoublyLinkedList(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        public T Head
        {
            get { return doublyLinkedList.Head; }
        }

        public T Tail
        {
            get { return doublyLinkedList.Tail; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return doublyLinkedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Insert(T node)
        {
            var insert = doublyLinkedList.Head;
            while (insert != null)
            {
                if (comparer.Compare(node, insert) < 0)
                {
                    break;
                }
                insert = insert.Next;
            }
            if (insert == doublyLinkedList.Head)
            {
                doublyLinkedList.AddFirst(node);
            }
            else if (insert == null)
            {
                doublyLinkedList.AddLast(node);
            }
            else
            {
                node.Previous = insert.Previous;
                insert.Previous.Next = node;
                insert.Previous = node;
                node.Next = insert;
            }
        }

        public void Remove(T node)
        {
            doublyLinkedList.Remove(node);
        }
    }
}