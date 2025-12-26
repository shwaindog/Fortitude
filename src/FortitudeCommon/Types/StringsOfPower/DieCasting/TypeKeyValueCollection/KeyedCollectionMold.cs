// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;


public partial class KeyedCollectionMold : MultiValueTypeMolder<KeyedCollectionMold>
{
    private ITypeMolderDieCast<KeyedCollectionMold> stb = null!;

    protected int ItemCount = 0;

    public KeyedCollectionMold InitializeKeyValueCollectionBuilder 
    (
        Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , MoldDieCastSettings appendSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting  
      , int existingRefId
      , FormatFlags createFormatFlags )
    {
        InitializeMultiValueTypeBuilder(typeBeingBuilt, vesselOfStringOfPower, appendSettings, typeName
                                      , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);

        stb = MoldStateField;

        return this;
    }

    public override bool IsComplexType => true;
    
    public override void AppendOpening()
    {
        var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!; 
        MoldStateField.StyleFormatter.AppendKeyedCollectionStart(MoldStateField.Sb, MoldStateField.TypeBeingBuilt, keyValueTypes.Value.Key, keyValueTypes.Value.Value);
    }
    
    public override void AppendClosing()
    {
        var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!; 
        MoldStateField.StyleFormatter.AppendKeyedCollectionEnd(MoldStateField.Sb, MoldStateField.TypeBeingBuilt, keyValueTypes.Value.Key, keyValueTypes.Value.Value, ItemCount);
    }

    protected override void InheritedStateReset()
    {
        stb  = null!;

        base.InheritedStateReset();
    }
}
