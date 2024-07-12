// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.DataStructures.Lists.LinkedLists;

public interface IDoublyLinkedListNode { }

public interface IDoublyLinkedListNode<T> : IDoublyLinkedListNode
{
    T? Previous { get; set; }
    T? Next     { get; set; }
}
