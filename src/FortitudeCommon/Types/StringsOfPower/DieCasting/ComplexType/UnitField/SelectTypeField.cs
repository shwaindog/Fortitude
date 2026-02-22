using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

public partial class SelectTypeField<TMold> : RecyclableObject
    where TMold : TypeMolder
{
    private IMoldWriteState<TMold> stb = null!;

    public SelectTypeField<TMold> Initialize(IMoldWriteState<TMold> molderDieCast)
    {
        stb = molderDieCast;

        return this;
    }

    public void Dispose()
    {
        stb = null!;
    }
}
