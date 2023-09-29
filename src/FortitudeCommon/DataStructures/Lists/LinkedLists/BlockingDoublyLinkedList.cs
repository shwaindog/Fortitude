using System;
using System.Threading;

namespace FortitudeCommon.DataStructures.LinkedLists
{
    public class BlockingDoublyLinkedList<T> : IDisposable where T : class, IDoublyLinkedListNode<T>
    {
        private readonly AutoResetEvent canDispatch = new AutoResetEvent(false);
        private readonly DoublyLinkedList<T> doublyLinkedList = new DoublyLinkedList<T>();
        private readonly object sync = new object();
        private volatile bool disposed;

        public void Dispose()
        {
            disposed = true;
            canDispatch.Set();
        }

        public void Offer(T node)
        {
            lock (sync)
            {
                doublyLinkedList.AddLast(node);
            }
            canDispatch.Set();
        }

        public void PushBack(T node)
        {
            lock (sync)
            {
                doublyLinkedList.Remove(node);
                doublyLinkedList.AddLast(node);
            }
            canDispatch.Set();
        }

        public T Take()
        {
            while (true)
            {
                if (disposed)
                {
                    return null;
                }
                lock (sync)
                {
                    if (!doublyLinkedList.IsEmpty)
                    {
                        var node = doublyLinkedList.Head;
                        doublyLinkedList.Remove(node);
                        return node;
                    }
                }
                canDispatch.WaitOne();
            }
        }

        public void Drain(DoublyLinkedList<T> dest)
        {
            canDispatch.WaitOne();
            lock (sync)
            {
                doublyLinkedList.Swap(dest);
                doublyLinkedList.Clear();
            }
        }

        public void Clear()
        {
            lock (sync)
            {
                doublyLinkedList.Clear();
            }
        }
    }
}