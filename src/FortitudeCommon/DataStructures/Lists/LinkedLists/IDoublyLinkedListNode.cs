namespace FortitudeCommon.DataStructures.LinkedLists
{
    public interface IDoublyLinkedListNode<T>
    {
        T Previous { get; set; }
        T Next { get; set; }
    }
}