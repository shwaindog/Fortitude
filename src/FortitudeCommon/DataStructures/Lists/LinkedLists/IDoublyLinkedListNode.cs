// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeCommon.DataStructures.Lists.LinkedLists;

public interface IDoublyLinkedListNode { }

public interface IDoublyLinkedListNode<T> : IDoublyLinkedListNode
{
    [JsonIgnore] T? Previous { get; set; }
    [JsonIgnore] T? Next     { get; set; }
}
