namespace FortitudeCommon.DataStructures.Memory
{
    public interface IRecycler
    {
        T Borrow<T>() where T : class;
        void Recycle(object recyclableObject);
    }
}