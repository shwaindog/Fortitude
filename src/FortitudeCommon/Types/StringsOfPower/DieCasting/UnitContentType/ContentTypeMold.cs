// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
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
      , CreateContext createContext)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, callerContext, createContext);

        return this;
    }

    protected ContentTypeWriteState<TContentMold, TToContentMold> Mws
    {
        [DebuggerStepThrough] get => (ContentTypeWriteState<TContentMold, TToContentMold>)MoldStateField!;
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

    public override AppendSummary Complete(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (State == null) { throw new NullReferenceException("Expected MoldState to be set"); }
        AppendClosing(State.CreateMoldFormatFlags);
        return RunShutdown();
    }

    public override void AppendClosing(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var usingFormatter =
            (CreateFormatFlags.HasAsStringContentFlag()
          && (Mws.CurrentWriteMethod.HasAllOf(AsComplex | AsContent)
           && Mws.StyleFormatter.LayoutEncoder.Type != EncodingType.PassThrough)
          || (Mws.MoldGraphVisit.IsARevisit && Mws.ValueAddedFormatFlags.HasAsStringContentFlag()))
                ? Mws.StyleFormatter.PreviousContextOrThis
                : Mws.StyleFormatter;
        if (Mws.CreateWriteMethod.SupportsMultipleFields())
        {
            usingFormatter.AppendComplexTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, formatFlags);
        }
        else { usingFormatter.AppendSimpleTypeClosing(Mws.InstanceOrType, Mws, Mws.CreateWriteMethod, formatFlags); }
        if (Mws.Style.IsJson()
         && Mws is { InnerSameAsOuterType: true, TypeBeingBuilt.IsValueType: false }
         && Mws.CurrentWriteMethod.HasAsSimpleFlag()
         && Mws.ValueAddedFormatFlags.HasAsStringContentFlag())
        {
            var visitId = Mws.MoldGraphVisit.VisitId;
            var state   = Mws.Master.ActiveGraphRegistry[visitId.VisitIndex];
            Mws.Master.SetBufferFirstFieldStartAndWrittenAs(visitId, state.TypeOpenBufferIndex, AsSimple | AsContent | AsString);
            Mws.Master.SetBufferFirstFieldStartAndIndentLevel(visitId, state.TypeOpenBufferIndex, state.IndentLevel + 1);
            Mws.Master.UpdateVisitFormatter(visitId, usingFormatter);
        }
    }

    public override TToContentMold TransitionToNextMold()
    {
        var nextMold = base.TransitionToNextMold();
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (Mws.InnerSameAsOuterType && !ReferenceEquals(Mws.Mold, nextMold))
        {
            if (nextMold is ITypeBuilderComponentSource moldWithState)
            {
                var state = moldWithState.MoldState;
                state.WroteTypeClose = false;
            }
        }
        return nextMold;
    }

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder() => Mws.StartDelimitedStringBuilder();
}
