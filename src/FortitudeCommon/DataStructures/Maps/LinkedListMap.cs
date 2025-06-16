#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Lists.LinkedLists;

#endregion

namespace FortitudeCommon.DataStructures.Maps;

public class LinkedListMap<TK, TV> : IMap<TK, TV> where TK : notnull
{
    private readonly object sync = new();

    protected DoublyLinkedList<
        DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>> Chain = new();

    public LinkedListMap() { }

    public LinkedListMap(IMap<TK, TV> toClone)
    {
        foreach (var keyValuePair in toClone) Add(keyValuePair.Key, keyValuePair.Value);
    }

    public TV this[TK key]
    {
        get
        {
            var currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
                if (currentNode.Payload.Key.Equals(key))
                    return currentNode.Payload.Value;

            throw new KeyNotFoundException($"Could not find '{key}' in SafeChainMap");
        }
        set => AddOrUpdate(key, value!);
    }

    
    public Func<ReusableList<TK>> KeysListFactory { get; set; } = () => new ReusableList<TK>();

    public Func<ReusableList<TV>> ValuesListFactory { get; set; } = () => new ReusableList<TV>();


    public ICollection<TK> Keys
    {
        get
        {
            var keysList    = KeysListFactory();
            var currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
                keysList.Add(currentNode.Payload.Key);
            return keysList;
        }
    }

    public ICollection<TV> Values
    {
        get
        {
            var valuesList = ValuesListFactory();
            var currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
                valuesList.Add(currentNode.Payload.Value);
            return valuesList;
        }
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
            if (currentNode.Payload.Key.Equals(key))
            {
                value = currentNode.Payload.Value!;
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
                if (currentNode.Payload.Key.Equals(key))
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
            {
                if (currentNode.Payload.Key.Equals(key)) foundInExisting = true;

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
                if (!(currentNode.Payload.Key.Equals(key)))
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
        return TryGetValue(key, out _);
    }

    public event Action<IEnumerable<KeyValuePair<TK, TV>>>? OnUpdate;

    object ICloneable.Clone() => Clone();

    public IMap<TK, TV> Clone() => new LinkedListMap<TK, TV>(this);

    public virtual IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        return Chain.Select(wn => wn.Payload).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
