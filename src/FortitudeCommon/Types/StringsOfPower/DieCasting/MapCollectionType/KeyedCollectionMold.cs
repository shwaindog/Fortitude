// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;


public partial class KeyedCollectionMold : MultiValueTypeMolder<KeyedCollectionMold>
{
    private ITypeMolderDieCast<KeyedCollectionMold> stb = null!;

    protected int ItemCount = 0;

    public KeyedCollectionMold InitializeKeyValueCollectionBuilder 
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType  
      , FormatFlags createFormatFlags )
    {
        InitializeMultiValueTypeBuilder(instanceOrContainer, typeBeingBuilt, vesselOfStringOfPower, typeVisitedAs, typeName
                                      , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);
        WrittenAs = WrittenAsFlags.AsMapCollection;

        stb = MoldStateField;

        return this;
    }

    public override bool IsComplexType => true;
    
    public override void StartFormattingTypeOpening(IStyledTypeFormatting usingFormatter)
    {
        var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!; 
        usingFormatter.AppendKeyedCollectionStart(MoldStateField.Sb, MoldStateField.TypeBeingBuilt, keyValueTypes.Value.Key, keyValueTypes.Value.Value);
    }

    public override void CompleteTypeOpeningToTypeFields(IStyledTypeFormatting usingFormatter) { }

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
