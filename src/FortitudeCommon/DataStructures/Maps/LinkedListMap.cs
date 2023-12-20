#region

using System.Collections;
using FortitudeCommon.DataStructures.Lists.LinkedLists;

#endregion

namespace FortitudeCommon.DataStructures.Maps;

public class LinkedListMap<TK, TV> : IMap<TK, TV> where TK : notnull
{
    private readonly object sync = new();

    protected DoublyLinkedList<
        DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>> Chain = new();

    public TV? this[TK key]
    {
        get
        {
            var currentNode =
                Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
                if (currentNode.Payload.Key?.Equals(key) ?? false)
                    return currentNode.Payload.Value;

            throw new KeyNotFoundException($"Could not find '{key}' in SafeChainMap");
        }
        set => Add(key, value!);
    }

    public TV? GetValue(TK key)
    {
        TryGetValue(key, out var value);
        return value;
    }

    public virtual bool TryGetValue(TK key, out TV? value)
    {
        var currentNode = Chain.Head;
        for (; currentNode != null; currentNode = currentNode.Next)
            if (currentNode.Payload.Key?.Equals(key) ?? false)
            {
                value = currentNode.Payload.Value;
                return true;
            }

        value = default;
        return false;
    }

    public TV GetOrPut(TK key, Func<TK, TV> createFunc)
    {
        if (!TryGetValue(key, out var value))
        {
            value = createFunc(key);
            Add(key, value);
        }

        return value!;
    }

    public TV AddOrUpdate(TK key, TV value)
    {
        lock (sync)
        {
            var duplicate =
                new DoublyLinkedList<DoublyLinkedListWrapperNode<
                    KeyValuePair<TK, TV>>>();
            var currentNode = Chain.Head;
            var foundInExisting = false;
            for (; currentNode != null; currentNode = currentNode.Next)
                if (currentNode.Payload.Key?.Equals(key) ?? false)
                {
                    duplicate.AddFirst(
                        new DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>(
                            new KeyValuePair<TK, TV>(key, value)));
                    foundInExisting = true;
                }
                else
                {
                    duplicate.AddFirst(
                        new DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>(
                            currentNode.Payload));
                }

            if (!foundInExisting)
            {
                duplicate.AddFirst(
                    new DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>(
                        new KeyValuePair<TK, TV>(key, value)));

                OnUpdate?.Invoke(duplicate.Select(wn => wn.Payload));
                Chain = duplicate;
            }

            return value;
        }
    }

    public bool Add(TK key, TV value)
    {
        lock (sync)
        {
            var duplicate =
                new DoublyLinkedList<DoublyLinkedListWrapperNode<
                    KeyValuePair<TK, TV>>>();
            var currentNode = Chain.Head;
            var foundInExisting = false;
            for (; currentNode != null; currentNode = currentNode.Next)
                if (currentNode.Payload.Key?.Equals(key) ?? false)
                {
                    duplicate.AddFirst(
                        new DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>(
                            new KeyValuePair<TK, TV>(key, value)));
                    foundInExisting = true;
                }
                else
                {
                    duplicate.AddFirst(
                        new DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>(
                            currentNode.Payload));
                }

            if (!foundInExisting)
            {
                duplicate.AddFirst(
                    new DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>(
                        new KeyValuePair<TK, TV>(key, value)));

                OnUpdate?.Invoke(duplicate.Select(wn => wn.Payload));
                Chain = duplicate;
                return true;
            }

            return false;
        }
    }

    public bool Remove(TK key)
    {
        var foundKey = false;
        lock (sync)
        {
            var duplicate = new DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>();
            var currentNode = Chain.Head;

            for (; currentNode != null; currentNode = currentNode.Next)
                if (!(currentNode.Payload.Key?.Equals(key) ?? false))
                    duplicate.AddFirst(new DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>(currentNode.Payload));
                else
                    foundKey = true;

            OnUpdate?.Invoke(duplicate.Select(wn => wn.Payload));
            Chain = duplicate;
        }

        return foundKey;
    }

    public void Clear()
    {
        lock (sync)
        {
            Chain =
                new DoublyLinkedList<DoublyLinkedListWrapperNode<
                    KeyValuePair<TK, TV>>>();
            OnUpdate?.Invoke(Chain.Select(wn => wn.Payload));
        }
    }

    public int Count => Chain.Count();

    public bool ContainsKey(TK key)
    {
        TV? tempValue;
        return TryGetValue(key, out tempValue);
    }

    public event Action<IEnumerable<KeyValuePair<TK, TV>>>? OnUpdate;

    public virtual IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        return Chain.Select(wn => wn.Payload).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
