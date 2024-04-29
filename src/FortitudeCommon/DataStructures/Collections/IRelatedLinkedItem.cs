namespace FortitudeCommon.DataStructures.Collections;

public interface IRelatedItem<in T>
{
    void EnsureRelatedItemsAreConfigured(T? referenceInstance);
}

public interface IRelatedLinkedItem<in T, in TLink>
{
    void EnsureRelatedItemsAreConfigured(T? referenceInstance, TLink? link);
}
