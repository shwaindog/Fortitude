// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public class ComplexOrderedCollectionMold : OrderedCollectionMold<ComplexOrderedCollectionMold>
{
    private ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold>?         logOnlyInternalCollectionField;
    private ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold>?                         logOnlyInternalField;
    private ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ComplexOrderedCollectionMold>? logOnlyInternalMapCollectionField;

    public ComplexOrderedCollectionMold InitializeComplexOrderedCollectionBuilder
    (object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , FormatFlags createFormatFlags)
    {
        InitializeOrderedCollectionBuilder
            (instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
           , remainingGraphDepth, moldGraphVisit, writeMethodType
           , createFormatFlags | FormatFlags.AsCollection);

        return this;
    }

    public virtual bool IsSimple => false;
    public override bool IsComplexType => true;

    protected override void SourceBuilderComponentAccess(WrittenAsFlags writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = 
            recycler
                .Borrow<CollectionMoldWriteState<ComplexOrderedCollectionMold>>()
                .InitializeOrderCollectionComponentAccess(this, PortableState, writeMethod);
    }

    public ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold>>()
                .Initialize(MoldStateField);

    public ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold>>()
                .Initialize(MoldStateField);

    public ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ComplexOrderedCollectionMold> LogOnlyKeyedCollectionField =>
        logOnlyInternalMapCollectionField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ComplexOrderedCollectionMold>>()
                .Initialize(MoldStateField);

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        base.InheritedStateReset();
    }

    public ComplexOrderedCollectionMold AddBaseRevealStateFields<T>(T thisType) where T : IStringBearer
    {
        var msf = MoldStateField;
        
        var markPreBodyStart = msf.Sb.Length;
        if (msf.SkipBody) return msf.Mold;

        MoldStateField.Master.AddBaseFieldsStart();
        TargetStringBearerRevealState.CallBaseStyledToStringIfSupported(thisType, msf.Master);
        
        if (msf.Sb.Length > markPreBodyStart && msf.Sf.Gb.LastContentSeparatorPaddingRanges.SeparatorPaddingRange == null)
        {
            msf.Sf.Gb.StartNextContentSeparatorPaddingSequence(msf.Sb, FormatFlags.DefaultCallerTypeFlags);
            msf.Sf.AddToNextFieldSeparatorAndPadding();
        }

        return Me;
    }
}
