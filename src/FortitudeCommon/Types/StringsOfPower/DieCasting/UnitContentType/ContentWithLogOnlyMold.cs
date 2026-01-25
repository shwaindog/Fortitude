// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentWithLogOnlyMold : ContentJoinTypeMold<ComplexContentTypeMold, ContentWithLogOnlyMold>
{
    private bool hasAddedContentFinishLogFieldSeparator;

    private ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ContentWithLogOnlyMold>? logOnlyInternalMapCollectionField;

    private ComplexType.CollectionField.SelectTypeCollectionField<ContentWithLogOnlyMold>? logOnlyInternalCollectionField;

    private SelectTypeField<ContentWithLogOnlyMold>? logOnlyInternalField;

    public SelectTypeField<ContentWithLogOnlyMold> LogOnlyField
    {
        get
        {
            if (logOnlyInternalField != null) return logOnlyInternalField;
            if (IsComplexType && !hasAddedContentFinishLogFieldSeparator)
            {
                MoldStateField.StyleFormatter.AddToNextFieldSeparatorAndPadding(MoldStateField.CreateMoldFormatFlags);
                hasAddedContentFinishLogFieldSeparator = true;
            }
            return logOnlyInternalField ??=
                PortableState.Master.Recycler
                             .Borrow<SelectTypeField<ContentWithLogOnlyMold>>()
                             .Initialize(MoldStateField);
        }
    }

    public ComplexType.CollectionField.SelectTypeCollectionField<ContentWithLogOnlyMold> LogOnlyCollectionField
    {
        get
        {
            if (logOnlyInternalCollectionField != null) return logOnlyInternalCollectionField;
            if (IsComplexType && !hasAddedContentFinishLogFieldSeparator)
            {
                MoldStateField.StyleFormatter.AddToNextFieldSeparatorAndPadding(MoldStateField.CreateMoldFormatFlags);
                hasAddedContentFinishLogFieldSeparator = true;
            }
            return logOnlyInternalCollectionField ??=
                PortableState.Master.Recycler
                             .Borrow<ComplexType.CollectionField.SelectTypeCollectionField<ContentWithLogOnlyMold>>()
                             .Initialize(MoldStateField);
        }
    }

    public ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ContentWithLogOnlyMold> LogOnlyKeyedCollectionField
    {
        get
        {
            if (logOnlyInternalMapCollectionField != null) return logOnlyInternalMapCollectionField;
            if (IsComplexType && !hasAddedContentFinishLogFieldSeparator)
            {
                MoldStateField.StyleFormatter.AddToNextFieldSeparatorAndPadding(MoldStateField.CreateMoldFormatFlags);
                hasAddedContentFinishLogFieldSeparator = true;
            }
            return logOnlyInternalMapCollectionField ??=
                PortableState.Master.Recycler
                             .Borrow<ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ContentWithLogOnlyMold>>()
                             .Initialize(MoldStateField);
        }
    }


    protected override void InheritedStateReset()
    {
        logOnlyInternalMapCollectionField?.DecrementRefCount();
        logOnlyInternalMapCollectionField = null!;
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        base.InheritedStateReset();
    }
}
