﻿using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt>: RecyclableObject
    where TExt : TypeMolder
{
    private ITypeMolderDieCast<TExt> stb = null!;
    
    public SelectTypeCollectionField<TExt> Initialize(ITypeMolderDieCast<TExt> molderDieCast)
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
