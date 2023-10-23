namespace FortitudeCommon.DataStructures.Memory;

public interface IRecycler
{
    T Borrow<T>() where T : class, new();
    void Recycle(object recyclableObject);
}
