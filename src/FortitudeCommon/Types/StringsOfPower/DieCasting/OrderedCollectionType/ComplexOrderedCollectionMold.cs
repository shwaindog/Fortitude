// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public class ComplexOrderedCollectionMold : OrderedCollectionMold<ComplexOrderedCollectionMold>
{
    private ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold>?         logOnlyInternalCollectionField;
    private ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold>?                         logOnlyInternalField;
    private ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ComplexOrderedCollectionMold>? logOnlyInternalMapCollectionField;

    public override MoldType MoldType => MoldType.ComplexOrderedCollectionMold;
    
    public ComplexOrderedCollectionMold InitializeComplexOrderedCollectionBuilder
    (object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , CallerContext callerContext  
      , CreateContext createContext)
    {
        InitializeOrderedCollectionBuilder
            (instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
           , remainingGraphDepth, moldGraphVisit, writeMethodType, callerContext
           , createContext with{ FormatFlags = createContext.FormatFlags | AsCollection });

        return this;
    }

    public virtual bool IsSimple => !IsComplexType;
    public override bool IsComplexType => State.CurrentWriteMethod.SupportsMultipleFields() && State.Style.IsLog();

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
        var msf         = WriteStateAsCollectionMoldWriteState;
        
        var visitResult      = msf.MoldGraphVisit;
        var visitIndex       = visitResult.VisitId.VisitIndex;
        var preBaseMoldState = msf.SnapshotWriteState;
        
        var markPreBodyStart = msf.Sb.Length;
        if (msf.SkipBody) return msf.Mold;

        MoldStateField.Master.AddBaseFieldsStart(msf);
        TargetStringBearerRevealState.CallBaseStyledToStringIfSupported(thisType, msf.Master);

        var maybePreviousCompleted = MoldStateField.Master.PreviousCompleted; 
        if (maybePreviousCompleted != null)
        {
            var prevCompleted = maybePreviousCompleted.Value;
            if (prevCompleted.ItemCount > 0 
             && (prevCompleted.MoldType == typeof(SimpleOrderedCollectionMold)
             || prevCompleted.MoldType == typeof(ComplexOrderedCollectionMold)
             || prevCompleted.MoldType.ExtendsGenericBaseType(typeof(ExplicitOrderedCollectionMold<>))))
            {
                msf.ItemCount += prevCompleted.ItemCount ?? 0;   
            }
        }
        
        // to avoid cicular references reusing this visit
        msf.MoldGraphVisit     = msf.MoldGraphVisit.IncrementUsedCount();
        msf.SnapshotWriteState = preBaseMoldState;
        var master           = msf.Master;
        var reg              = master.ActiveGraphRegistry;
        var restoreMoldState = reg[visitIndex];
        reg[visitIndex] = restoreMoldState.UpdateMoldWriteState(msf);
        
        if (msf.Sb.Length > markPreBodyStart && msf.Sf.Gb.LastContentSeparatorPaddingRanges.SeparatorPaddingRange == null)
        {
            msf.Sf.Gb.StartNextContentSeparatorPaddingSequence(msf.Sb, DefaultCallerTypeFlags);
            msf.Sf.AddToNextFieldSeparatorAndPadding();
        }

        return Me;
    }
}
