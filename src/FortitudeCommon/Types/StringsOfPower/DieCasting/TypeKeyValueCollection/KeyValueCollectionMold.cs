// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;


public partial class KeyValueCollectionMold : MultiValueTypeMolder<KeyValueCollectionMold>
{
    private ITypeMolderDieCast<KeyValueCollectionMold> stb = null!;

    protected int ItemCount = 0;

    public KeyValueCollectionMold InitializeKeyValueCollectionBuilder 
    (
        Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , MoldDieCastSettings appendSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting  
      , int existingRefId)
    {
        InitializeMultiValueTypeBuilder(typeBeingBuilt, vesselOfStringOfPower, appendSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        stb = CompAccess;

        return this;
    }

    public override bool IsComplexType => true;
    
    public override void AppendOpening()
    {
        var keyValueTypes = CompAccess.TypeBeingBuilt.GetKeyedCollectionTypes()!; 
        CompAccess.StyleFormatter.AppendKeyedCollectionStart(CompAccess, CompAccess.TypeBeingBuilt, keyValueTypes.Value.Key, keyValueTypes.Value.Value);
    }
    
    public override void AppendClosing()
    {
        var keyValueTypes = CompAccess.TypeBeingBuilt.GetKeyedCollectionTypes()!; 
        CompAccess.StyleFormatter.AppendKeyedCollectionEnd(CompAccess, CompAccess.TypeBeingBuilt, keyValueTypes.Value.Key, keyValueTypes.Value.Value, ItemCount);
    }

    protected override void InheritedStateReset()
    {
        stb  = null!;

        base.InheritedStateReset();
    }
}
