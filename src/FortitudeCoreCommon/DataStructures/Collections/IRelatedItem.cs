namespace FortitudeCommon.DataStructures.Collections
{
    public interface IRelatedItem<in T>
    {
        void EnsureRelatedItemsAreConfigured(T referenceInstance);
    }
}
