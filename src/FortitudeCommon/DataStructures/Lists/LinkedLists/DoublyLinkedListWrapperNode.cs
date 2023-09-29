namespace FortitudeCommon.DataStructures.LinkedLists
{
    public class DoublyLinkedListWrapperNode<T> : IDoublyLinkedListNode<DoublyLinkedListWrapperNode<T>>
    {
        public DoublyLinkedListWrapperNode(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; set; }
        public DoublyLinkedListWrapperNode<T> Previous { get; set; }
        public DoublyLinkedListWrapperNode<T> Next { get; set; }
    }
}