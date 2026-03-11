// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
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
      , CallerContext callerContext  
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, callerContext, createFormatFlags);

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
        if (PortableState.MoldGraphVisit.NoVisitCheckDone) return;
        var usingFormatter = 
            (CreateFormatFlags.HasAsStringContentFlag()
               && (Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
                && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
          || (Mws.MoldGraphVisit.IsARevisit && formatFlags.HasAsStringContentFlag()))
            ? Mws.StyleFormatter.PreviousContextOrThis
            : Mws.StyleFormatter;
      
        StartTypeOpening(usingFormatter, formatFlags);
    }

    public override void FinishTypeOpening(FormatFlags formatFlags)
    {
        if (PortableState.MoldGraphVisit.NoVisitCheckDone) return;
        var usingFormatter = 
            (CreateFormatFlags.HasAsStringContentFlag()
               && (Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
                && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
          || (Mws.MoldGraphVisit.IsARevisit && formatFlags.HasAsStringContentFlag()))
            ? Mws.StyleFormatter.PreviousContextOrThis
            : Mws.StyleFormatter;
        FinishTypeOpening(usingFormatter, formatFlags);
    }

    // public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    // {
    //     if (Mws.SupportsMultipleFields)
    //     {
    //         usingFormatter.StartComplexTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
    //     }
    //     else
    //     {
    //         usingFormatter.StartSimpleTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
    //         MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, MoldStateField.StyleFormatter
    //                           , MoldStateField.CurrentWriteMethod, MoldStateField.MoldWrittenFlags, formatFlags);
    //     }
    // }
    //
    // public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    // {
    //     if (Mws.SupportsMultipleFields)
    //     {
    //         usingFormatter.FinishComplexTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
    //         MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, MoldStateField.StyleFormatter
    //                           , MoldStateField.CurrentWriteMethod, MoldStateField.MoldWrittenFlags, formatFlags);
    //     }
    //     else
    //     {
    //         usingFormatter.FinishSimpleTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
    //     }
    // }

    public override void AppendClosing(FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)
    {
        // if (Mws.CurrentWriteMethod.SupportsMultipleFields())
        // {
        // WrittenAs |= AsComplex;
        // }
        // else
        // {
        //     WrittenAs |= AsContent;
        // }
        var formatter = Mws!.StyleFormatter;
        if (Mws.CreateWriteMethod.SupportsMultipleFields())
        {
            formatter.AppendComplexTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, formatFlags);
        }
        else
        {
            formatter.AppendSimpleTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, formatFlags);
        }
    }

    public override TToContentMold TransitionToNextMold()
    {
        if (Mws.Style.IsJson() && Mws is { InnerSameAsOuterType: true, TypeBeingBuilt.IsValueType: false } 
         && Mws.ValueAddedFormatFlags.HasAsStringContentFlag())
        {
            var visitId = Mws.MoldGraphVisit.VisitId;
            var state   = Mws.Master.ActiveGraphRegistry[visitId.VisitIndex];
            Mws.Master.SetBufferFirstFieldStartAndWrittenAs(visitId, state.TypeOpenBufferIndex, AsSimple | AsContent | AsString);
            // Mws.Master.SetBufferFirstFieldStartAndIndentLevel(visitId, state.TypeOpenBufferIndex, state.IndentLevel + 1);
            Mws.Master.UpdateVisitFormatter(visitId, Mws.Sf.PreviousContextOrThis);
        }
        return base.TransitionToNextMold();
    }

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder() => Mws.StartDelimitedStringBuilder();
}
