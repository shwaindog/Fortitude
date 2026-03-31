// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public interface IKeyedCollectionExtendFunctionality
{
    public void BeforeFirstElementWriteFieldName(string fieldName);
}

public partial class KeyedCollectionMold : MultiValueTypeMolder<KeyedCollectionMold>, IKeyedCollectionExtendFunctionality
{
    private ComplexType.CollectionField.SelectTypeCollectionField<KeyedCollectionMold>?         logOnlyInternalCollectionField;
    private ComplexType.UnitField.SelectTypeField<KeyedCollectionMold>?                         logOnlyInternalField;
    private ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<KeyedCollectionMold>? logOnlyInternalMapCollectionField;

    protected IMoldWriteState<KeyedCollectionMold> Mws = null!;

    public int ItemCount = 0;

    private string? beforeFirstItemFieldName;

    public KeyedCollectionMold InitializeKeyValueCollectionBuilder
    (object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , CallerContext callerContext
      , CreateContext createContext)
    {
        InitializeMultiValueTypeBuilder(instanceOrContainer, typeBeingBuilt, vesselOfStringOfPower, typeVisitedAs, typeName
                                      , remainingGraphDepth, moldGraphVisit, writeMethodType, callerContext, createContext);
        WrittenAs = WrittenAsFlags.AsMapCollection;

        Mws = MoldStateField;

        return this;
    }

    public override bool IsComplexType => true;

    public ComplexType.UnitField.SelectTypeField<KeyedCollectionMold> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.UnitField.SelectTypeField<KeyedCollectionMold>>()
                .Initialize(State);

    public ComplexType.CollectionField.SelectTypeCollectionField<KeyedCollectionMold> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.CollectionField.SelectTypeCollectionField<KeyedCollectionMold>>()
                .Initialize(MoldStateField);

    public ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<KeyedCollectionMold> LogOnlyKeyedCollectionField =>
        logOnlyInternalMapCollectionField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<KeyedCollectionMold>>()
                .Initialize(MoldStateField);

    void IKeyedCollectionExtendFunctionality.BeforeFirstElementWriteFieldName(string fieldName)
    {
        beforeFirstItemFieldName ??= fieldName;
    }

    public virtual void BeforeFirstElement(IMoldWriteState mws)
    {
        if (beforeFirstItemFieldName != null)
        {
            mws.FieldNameJoin(beforeFirstItemFieldName);
            var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!;
            mws.StyleFormatter.StartKeyedCollectionOpen(MoldStateField, keyValueTypes.Value.Key, keyValueTypes.Value.Value);
            if (mws.Style.IsLog() && mws.StartedTypeName)
            {
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, mws.StyleFormatter
                                  , MoldStateField.CreateWriteMethod, MoldStateField.MoldWrittenFlags, mws.CreateMoldFormatFlags);
            }
            mws.StyleFormatter.FinishKeyedCollectionOpen(MoldStateField);
            if (mws.Style.IsNotLog())
            {
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, mws.StyleFormatter
                                  , MoldStateField.CreateWriteMethod, MoldStateField.MoldWrittenFlags, mws.CreateMoldFormatFlags);
            }
            beforeFirstItemFieldName = null;
        }
    }

    public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!;
        usingFormatter.StartKeyedCollectionOpen(MoldStateField, keyValueTypes.Value.Key, keyValueTypes.Value.Value, formatFlags);
    }

    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        usingFormatter.FinishKeyedCollectionOpen(MoldStateField);
    }

    public override void AppendClosing(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (beforeFirstItemFieldName == null)
        {
            var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!;
            MoldStateField.StyleFormatter.AppendKeyedCollectionClose(MoldStateField, keyValueTypes.Value.Key, keyValueTypes.Value.Value, ItemCount
                                                                   , formatFlags);
        }
    }

    protected override void InheritedStateReset()
    {
        Mws = null!;

        beforeFirstItemFieldName = null;

        base.InheritedStateReset();
    }
}
