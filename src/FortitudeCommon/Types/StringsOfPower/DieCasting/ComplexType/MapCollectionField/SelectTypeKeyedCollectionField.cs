using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> : RecyclableObject
    where TExt : TypeMolder
{
    private ITypeMolderDieCast<TExt> stb = null!;
    
    public SelectTypeKeyedCollectionField<TExt> Initialize(ITypeMolderDieCast<TExt> molderDieCast)
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
