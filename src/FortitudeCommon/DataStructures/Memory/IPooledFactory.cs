namespace FortitudeCommon.DataStructures.Memory;

public interface IPooledFactory
{
    void ReturnBorrowed(object item);
    object Borrow();
}

public interface IPooledFactory<T> : IPooledFactory
{
    void ReturnBorrowed(T item);
    new T Borrow();
}
