#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public class ObjectPool<T> where T : class
{
    private readonly Func<T> newObjFunc;

    internal DoublyLinkedList<DoublyLinkedListWrapperNode<T>> AvailablePool = new();

    private SpinLock poolSpinLock;
    private SpinLock returnSpinLock;

    internal DoublyLinkedList<DoublyLinkedListWrapperNode<T>> ReturnsPool = new();

    public ObjectPool(Func<T> newObjFunc)
    {
        this.newObjFunc = () =>
        {
            InstanceCount++;
            var newInstance = newObjFunc();
            NewInstanceEvent?.Invoke(newInstance);
            return newInstance;
        };
    }

    public ObjectPool(Func<T> newObjFunc, int initialSize)
        : this(newObjFunc)
    {
        for (var i = 0; i < initialSize; i++)
        {
            var requested = newObjFunc();
            AvailablePool.AddLast(new DoublyLinkedListWrapperNode<T>(requested));
        }
    }

    public int InstanceCount { get; private set; }
    public event Action<T?>? NewInstanceEvent;

    public void DiscardPooledObjects()
    {
        var poolLocked = false;
        var returnLocked = false;
        try
        {
            poolSpinLock.Enter(ref poolLocked);
            AvailablePool = new DoublyLinkedList<DoublyLinkedListWrapperNode<T>>();
            NewInstanceEvent?.Invoke(null);
        }
        finally
        {
            if (poolLocked) poolSpinLock.Exit();
        }

        try
        {
            returnSpinLock.Enter(ref returnLocked);
            ReturnsPool = new DoublyLinkedList<DoublyLinkedListWrapperNode<T>>();
        }
        finally
        {
            if (returnLocked) returnSpinLock.Exit();
        }
    }

    public T Borrow()
    {
        var poolLocked = false;
        var returnLocked = false;
        for (var count = 0; count < 10; count++)
        {
            if (AvailablePool.IsEmpty)
            {
                var requested = newObjFunc();
                var newWrapperObj = new DoublyLinkedListWrapperNode<T>(requested);
                try
                {
                    returnSpinLock.Enter(ref returnLocked);
                    ReturnsPool.AddLast(newWrapperObj);
                }
                finally
                {
                    if (returnLocked) returnSpinLock.Exit();
                }

                return requested;
            }

            DoublyLinkedListWrapperNode<T>? requestedDoublyLinkedListWrapper;

            try
            {
                poolSpinLock.Enter(ref poolLocked);
                requestedDoublyLinkedListWrapper = AvailablePool.Head;
                if (requestedDoublyLinkedListWrapper != null) AvailablePool.Remove(requestedDoublyLinkedListWrapper);
            }
            finally
            {
                if (poolLocked) poolSpinLock.Exit();
            }

            if (requestedDoublyLinkedListWrapper != null)
            {
                var payLoad = requestedDoublyLinkedListWrapper.Payload;
                try
                {
                    returnSpinLock.Enter(ref returnLocked);
                    ReturnsPool.AddLast(requestedDoublyLinkedListWrapper);
                }
                finally
                {
                    if (returnLocked) returnSpinLock.Exit();
                }

                return payLoad;
            }

            poolLocked = false;
        }

        return newObjFunc();
    }

    public void Return(T notUsed)
    {
        var poolLocked = false;
        var returnLocked = false;
        for (var count = 0; count < 10; count++)
        {
            if (ReturnsPool.IsEmpty)
            {
                var newWrapperObj = new DoublyLinkedListWrapperNode<T>(notUsed);
                try
                {
                    poolSpinLock.Enter(ref poolLocked);
                    AvailablePool.AddLast(newWrapperObj);
                }
                finally
                {
                    if (poolLocked) poolSpinLock.Exit();
                }

                return;
            }

            DoublyLinkedListWrapperNode<T>? requestedWrapper;
            try
            {
                returnSpinLock.Enter(ref returnLocked);
                requestedWrapper = ReturnsPool.Head;
                if (requestedWrapper != null) ReturnsPool.Remove(requestedWrapper);
            }
            finally
            {
                if (returnLocked) returnSpinLock.Exit();
            }

            if (requestedWrapper != null)
            {
                requestedWrapper.Payload = notUsed;
                try
                {
                    poolSpinLock.Enter(ref poolLocked);
                    AvailablePool.AddLast(requestedWrapper);
                }
                finally
                {
                    if (poolLocked) poolSpinLock.Exit();
                }

                return;
            }

            returnLocked = false;
        }
    }
}
