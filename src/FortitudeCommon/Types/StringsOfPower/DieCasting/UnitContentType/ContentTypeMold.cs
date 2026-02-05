// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentTypeMold<TContentMold, TToContentMold> : TransitioningTypeMolder<TContentMold, TToContentMold>
    where TContentMold : ContentTypeMold<TContentMold, TToContentMold>
    where TToContentMold : ContentJoinTypeMold<TContentMold, TToContentMold>, IMigrateFrom<TContentMold, TToContentMold>, new()
{
    public ContentTypeMold<TContentMold, TToContentMold> InitializeContentTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);

        return this;
    }

    protected ContentTypeDieCast<TContentMold, TToContentMold> Msf
    {
        [DebuggerStepThrough]
        get => (ContentTypeDieCast<TContentMold, TToContentMold>)MoldStateField!;
    }

    public override bool IsComplexType => Msf.SupportsMultipleFields;

    public virtual bool IsSimpleMold => true;

    
    public override void StartTypeOpening()
    {
        if (PortableState.MoldGraphVisit.NoVisitCheckDone || PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        var usingFormatter = (CreateFormatFlags.HasAsStringContentFlag()
                           && Msf.CurrentWriteMethod == WriteMethodType.MoldComplexContentType
                           && Msf.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
            ? Msf.StyleFormatter.PreviousContextOrThis
            : Msf.StyleFormatter;
      
        StartFormattingTypeOpening(usingFormatter);
        MyAppendGraphFields(usingFormatter);
    }

    public override void FinishTypeOpening()
    {
        if (PortableState.MoldGraphVisit.NoVisitCheckDone || PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        CompleteTypeOpeningToTypeFields();
    }

    public override void StartFormattingTypeOpening(IStyledTypeFormatting usingFormatter)
    {
        if (Msf.SupportsMultipleFields)
        {
            usingFormatter.StartComplexTypeOpening(Msf);
        }
        else
        {
            usingFormatter.StartContentTypeOpening(Msf);
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

        if (Msf.CurrentWriteMethod.SupportsMultipleFields())
        {
            WrittenAs = WrittenAsFlags.AsComplex;
        }
        else
        {
            WrittenAs = WrittenAsFlags.AsSimple;
        }
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
