// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public class KeyedCollectionWriteState: MoldWriteState<KeyedCollectionMold>, IHasCollectionItems
{
    public int ItemCount { get; set; }

    public KeyedCollectionWriteState InitializeKeyedCollectionComponentAccess
        (KeyedCollectionMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WrittenAsFlags writeMethod)
    {
        Initialize(externalTypeBuilder, typeBuilderPortableState, writeMethod);
        
        return this;
    }

    public override void StateReset()
    {
        ItemCount = 0;
        base.StateReset();
    }
}
