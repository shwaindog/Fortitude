using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

public class SimpleValueTypeMold : ValueTypeMold<SimpleValueTypeMold>
{
    public SimpleValueTypeMold InitializeSimpleValueTypeBuilder
        (
            Type typeBeingBuilt
          , ISecretStringOfPower master
          , MoldDieCastSettings typeSettings
          , string typeName
          , int remainingGraphDepth
          , IStyledTypeFormatting typeFormatting  
          , int existingRefId)
    {
        InitializeValueTypeBuilder(typeBeingBuilt, master, typeSettings, typeName, remainingGraphDepth, typeFormatting,  existingRefId);

        return this;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        CompAccess = recycler.Borrow<ValueTypeDieCast<SimpleValueTypeMold>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, true);
    }

}