using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public interface ISelectKeyedCollectionField
{
}

public partial class SelectTypeKeyedCollectionField<TMold> : RecyclableObject, ISelectKeyedCollectionField
    where TMold : TypeMolder
{
    public IMoldWriteState<TMold> Mws { get; private set; } = null!;
    
    internal TMold Mold => Mws.Mold;
    
    public int ItemCount { get; set; }
    
    public Type? CollectionType { get; set; }
    
    public SelectTypeKeyedCollectionField<TMold> Initialize(IMoldWriteState<TMold> moldWriteState)
    {
        Mws = moldWriteState;

        return this;
    }

    public override void StateReset()
    {
        Mws = null!;
        ItemCount = 0;
        CollectionType = null;
        base.StateReset();
    }
}
