using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> : RecyclableObject
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;
    
    public SelectTypeKeyValueCollectionField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styleTypeBuilder)
    {
        stb = styleTypeBuilder;

        return this;
    }

    public override void StateReset()
    {
        stb = null!;
        base.StateReset();
    }
}
