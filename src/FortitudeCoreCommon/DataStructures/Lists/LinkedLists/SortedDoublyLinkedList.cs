#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Lists.LinkedLists;

public class SortedDoublyLinkedList<T> : IEnumerable<T> where T : class, IDoublyLinkedListNode<T>
{
    private readonly IComparer<T> comparer;
    private readonly DoublyLinkedList<T> doublyLinkedList = new();

    public SortedDoublyLinkedList(IComparer<T> comparer) => this.comparer = comparer;

    public T? Head => doublyLinkedList.Head;

    public T? Tail => doublyLinkedList.Tail;

    public IEnumerator<T> GetEnumerator() => doublyLinkedList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Insert(T node)
    {
        var insert = doublyLinkedList.Head;
        while (insert != null)
        {
            if (comparer.Compare(node, insert) < 0) break;
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
            insert.Previous!.Next = node;
            insert.Previous = node;
            node.Next = insert;
        }
    }

    public void Remove(T node)
    {
        doublyLinkedList.Remove(node);
    }
}
