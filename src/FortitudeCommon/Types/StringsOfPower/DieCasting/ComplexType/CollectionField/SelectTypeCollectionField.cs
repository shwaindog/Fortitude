using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TMold>: RecyclableObject
    where TMold : TypeMolder
{
    private IMoldWriteState<TMold> stb = null!;
    
    public SelectTypeCollectionField<TMold> Initialize(IMoldWriteState<TMold> molderDieCast)
    {
        stb = molderDieCast;

        return this;
    }

    public override void StateReset()
    {
        stb = null!;
        base.StateReset();
    }
}
