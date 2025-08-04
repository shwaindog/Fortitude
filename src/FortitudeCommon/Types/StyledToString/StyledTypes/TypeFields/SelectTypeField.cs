using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public partial class SelectTypeField<TExt> : RecyclableObject
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    public SelectTypeField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styleTypeBuilder)
    {
        stb = styleTypeBuilder;

        return this;
    }

    public void Dispose()
    {
        stb = null!;
    }
}
