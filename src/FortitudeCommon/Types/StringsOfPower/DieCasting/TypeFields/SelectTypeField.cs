using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeField<TExt> : RecyclableObject
    where TExt : TypeMolder
{
    private ITypeMolderDieCast<TExt> stb = null!;

    public SelectTypeField<TExt> Initialize(ITypeMolderDieCast<TExt> molderDieCast)
    {
        stb = molderDieCast;

        return this;
    }

    public void Dispose()
    {
        stb = null!;
    }
}
