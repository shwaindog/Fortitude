// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentTypeMold<TContentMold> : TransitioningTypeMolder<TContentMold, ContentJoinTypeMold<TContentMold>>
    where TContentMold : ContentTypeMold<TContentMold>
{
    public ContentTypeMold<TContentMold> InitializeContentTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);

        return this;
    }

    protected ContentTypeDieCast<TContentMold> Msf => (ContentTypeDieCast<TContentMold>)MoldStateField!;

    public override bool IsComplexType => Msf.SupportsMultipleFields;


    public override void StartFormattingTypeOpening()
    {
        var formatter         = Msf!.StyleFormatter;
        if (Msf.SupportsMultipleFields)
        {
            formatter.StartComplexTypeOpening(Msf);
        }
        else
        {
            formatter.StartContentTypeOpening(Msf);
        }
    }

    public override void CompleteTypeOpeningToTypeFields()
    {
        var formatter = Msf!.StyleFormatter;
        if (IsComplexType)
        {
            formatter.FinishComplexTypeOpening(Msf);
        }
        else
        {
            formatter.FinishContentTypeOpening(Msf);
        }
    }

    public override void AppendClosing()
    {
        var formatter = MoldStateField!.StyleFormatter;
        if (IsComplexType)
        {
            formatter.AppendComplexTypeClosing(MoldStateField);
        }
        else
        {
            formatter.AppendContentTypeClosing(MoldStateField);
        }
    }

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder() => Msf.StartDelimitedStringBuilder();
}
