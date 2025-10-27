using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TMold> : RecyclableObject
    where TMold : TypeMolder
{
    private ITypeMolderDieCast<TMold> stb = null!;

    public SelectTypeField<TMold> Initialize(ITypeMolderDieCast<TMold> molderDieCast)
    {
        stb = molderDieCast;

        return this;
    }

    public void Dispose()
    {
        stb = null!;
    }
}
