// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.DataStructures.Lists.LinkedLists;

public class DoublyLinkedListWrapperNode<T> : RecyclableObject, IDoublyLinkedListNode<DoublyLinkedListWrapperNode<T>>
{
    public DoublyLinkedListWrapperNode() => Payload = default!;

    public DoublyLinkedListWrapperNode(T payload) => Payload = payload;

    public DoublyLinkedListWrapperNode<T> Initialize(T payload)
    {
        Payload = payload;

        return this;
    }

    public T                               Payload  { get; set; }
    public DoublyLinkedListWrapperNode<T>? Previous { get; set; }
    public DoublyLinkedListWrapperNode<T>? Next     { get; set; }

    public void Configure(T payload)
    {
        Payload = payload;
    }

    public override void StateReset()
    {
        if (Payload is RecyclableObject recyclablePayload) recyclablePayload.DecrementRefCount();
        Payload  = default!;
        Previous = Next = null;
        base.StateReset();
    }
}
