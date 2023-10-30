namespace FortitudeCommon.Configuration
{
    public interface IMuteableRepository<in T> 
    {
        bool ContainsItem(T checkItem);
        bool Update(T item);
        bool Add(T item);
        bool AddOrUpdate(T item);
        bool Delete(T item);
    }
}
