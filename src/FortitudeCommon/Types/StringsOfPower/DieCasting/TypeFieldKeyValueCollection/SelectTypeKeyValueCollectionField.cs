using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> : RecyclableObject
    where TExt : TypeMolder
{
    private ITypeMolderDieCast<TExt> stb = null!;
    
    public SelectTypeKeyValueCollectionField<TExt> Initialize(ITypeMolderDieCast<TExt> molderDieCast)
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
