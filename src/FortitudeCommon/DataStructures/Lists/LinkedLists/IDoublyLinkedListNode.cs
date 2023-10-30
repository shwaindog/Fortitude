namespace FortitudeCommon.DataStructures.Lists.LinkedLists;

public interface IDoublyLinkedListNode<T>
{
    T? Previous { get; set; }
    T? Next { get; set; }
}
