namespace FortitudeCommon.DataStructures.Collections;

public interface IRelatedItems<in TItem>
{
    void EnsureRelatedItemsAreConfigured(TItem? item);
}

public interface IRelatedItems<in TItem1, in TItem2>
{
    void EnsureRelatedItemsAreConfigured(TItem1? firstItem, TItem2? secondItem);
}

public interface IRelatedItems<in TItem1, in TItem2, in TItem3>
{
    void EnsureRelatedItemsAreConfigured(TItem1? firstItem, TItem2? secondItem, TItem3? thirdItem);
}
