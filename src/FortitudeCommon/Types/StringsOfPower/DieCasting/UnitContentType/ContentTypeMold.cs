// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

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
      , WrittenAsFlags writeMethodType  
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);

        return this;
    }

    protected ContentTypeWriteState<TContentMold, TToContentMold> Mws
    {
        [DebuggerStepThrough]
        get => (ContentTypeWriteState<TContentMold, TToContentMold>)MoldStateField!;
    }

    public override bool IsComplexType => Mws.SupportsMultipleFields;

    public virtual bool IsSimpleMold => true;
    
    public override void StartTypeOpening(FormatFlags formatFlags)
    {
        if (PortableState.MoldGraphVisit.NoVisitCheckDone || PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        var usingFormatter = (CreateFormatFlags.HasAsStringContentFlag()
                           && Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
                           && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
            ? Mws.StyleFormatter.PreviousContextOrThis
            : Mws.StyleFormatter;
      
        StartTypeOpening(usingFormatter, formatFlags);
        MyAppendGraphFields(Mws.InstanceOrType, Mws.MoldGraphVisit, usingFormatter, Mws.CurrentWriteMethod, Mws.CreateMoldFormatFlags);
    }

    public override void FinishTypeOpening(FormatFlags formatFlags)
    {
        if (PortableState.MoldGraphVisit.NoVisitCheckDone || PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        var usingFormatter = (CreateFormatFlags.HasAsStringContentFlag()
                           && Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
                           && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
            ? Mws.StyleFormatter.PreviousContextOrThis
            : Mws.StyleFormatter;
        FinishTypeOpening(usingFormatter, formatFlags);
    }

    public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        if (Mws.SupportsMultipleFields)
        {
            usingFormatter.StartComplexTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
        }
        else
        {
            usingFormatter.StartSimpleTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
        }
    }

    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        if (Mws.SupportsMultipleFields)
        {
            usingFormatter.FinishComplexTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
        }
        else
        {
            usingFormatter.FinishSimpleTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
        }
    }

    public override void AppendClosing()
    {

        if (Mws.CurrentWriteMethod.SupportsMultipleFields())
        {
            WrittenAs = AsComplex;
        }
        else
        {
            WrittenAs = AsContent;
        }
        var formatter = Mws!.StyleFormatter;
        if (Mws.SupportsMultipleFields)
        {
            formatter.AppendComplexTypeClosing(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod);
        }
        else
        {
            formatter.AppendSimpleTypeClosing(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod);
        }
    }

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder() => Mws.StartDelimitedStringBuilder();
}
