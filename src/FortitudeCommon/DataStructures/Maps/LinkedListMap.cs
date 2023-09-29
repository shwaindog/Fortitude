using System;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.DataStructures.LinkedLists;

namespace FortitudeCommon.DataStructures.Maps
{
    public class LinkedListMap<Tk, Tv> : IMap<Tk, Tv>
    {
        private readonly object sync = new object();

        protected DoublyLinkedList<DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>> Chain = 
            new DoublyLinkedList<DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>>();

        public Tv this[Tk key]
        {
            get
            {
                DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>> currentNode = Chain.Head;
                for (;currentNode != null; currentNode = currentNode.Next)
                {
                    if (currentNode.Payload.Key.Equals(key))
                    {
                        return currentNode.Payload.Value;
                    }
                }
                throw new KeyNotFoundException($"Could not find '{key}' in SafeChainMap");
            }
            set => Add(key, value);
        }

        public virtual bool TryGetValue(Tk key, out Tv value)
        {
            DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>> currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
            {
                if (currentNode.Payload.Key.Equals(key))
                {
                    value = currentNode.Payload.Value;
                    return true;
                }
            }
            value = default(Tv);
            return false;
        }
        
        public void Add(Tk key, Tv value)
        {
            lock (sync)
            {
                DoublyLinkedList<DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>> duplicate = 
                    new DoublyLinkedList<DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>>();
                DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>> currentNode = Chain.Head;
                bool foundInExisting = false;
                for (; currentNode != null; currentNode = currentNode.Next)
                {
                    if (currentNode.Payload.Key.Equals(key))
                    {
                        duplicate.AddFirst(new DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>(
                            new FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>(key, value, true)));
                        foundInExisting = true;
                    }
                    else
                    {
                        duplicate.AddFirst(new DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>(currentNode.Payload));
                    }
                }
                if (!foundInExisting)
                {
                    duplicate.AddFirst(new DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>(
                        new FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>(key, value, true)));
                }
                OnUpdate?.Invoke(duplicate.Select(wn => wn.Payload));
                Chain = duplicate;
            }
        }

        public KeyValuePair<Tk, Tv> Remove(Tk key)
        {
            FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv> removedItem = new FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>(false);
            lock (sync)
            {
                DoublyLinkedList<DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>> duplicate = 
                    new DoublyLinkedList<DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>>();
                DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>> currentNode = Chain.Head;

                for (; currentNode != null; currentNode = currentNode.Next)
                {
                    if (!currentNode.Payload.Key.Equals(key))
                    {
                        duplicate.AddFirst(new DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>(currentNode.Payload));
                    }
                    else
                    {
                        removedItem = 
                            new FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>(currentNode.Payload.Key, currentNode.Payload.Value, true);
                    }
                }
                OnUpdate?.Invoke(duplicate.Select(wn => wn.Payload));
                Chain = duplicate;
            }
            return removedItem;
        }

        public void Clear()
        {
            lock (sync)
            {
                Chain = new DoublyLinkedList<DoublyLinkedListWrapperNode<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>>>();
                OnUpdate?.Invoke(Chain.Select(wn => wn.Payload));
            }
        }

        public int Count => Chain.Count();

        public bool ContainsKey(Tk key)
        {
            Tv tempValue;
            return TryGetValue(key, out tempValue);
        }

        public event Action<IEnumerable<KeyValuePair<Tk, Tv>>> OnUpdate;

        public virtual IEnumerator<KeyValuePair<Tk, Tv>> GetEnumerator()
        {
            return Chain.Select(wn => wn.Payload).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}