using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public interface ISelectKeyedCollectionField
{
}

public partial class SelectTypeKeyedCollectionField<TExt> : RecyclableObject, ISelectKeyedCollectionField
    where TExt : TypeMolder
{
    public IMoldWriteState<TExt> Mws { get; private set; } = null!;
    
    internal TExt Mold => Mws.Mold;
    
    public int ItemCount { get; set; }
    
    public Type? CollectionType { get; set; }
    
    public SelectTypeKeyedCollectionField<TExt> Initialize(IMoldWriteState<TExt> moldWriteState)
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
