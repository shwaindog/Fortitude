using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> : RecyclableObject
    where TExt : TypeMolder
{
    private IMoldWriteState<TExt> stb = null!;
    
    public SelectTypeKeyedCollectionField<TExt> Initialize(IMoldWriteState<TExt> molderDieCast)
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
