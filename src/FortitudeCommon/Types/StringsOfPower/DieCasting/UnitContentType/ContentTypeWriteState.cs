// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public interface IContentTypeWriteState : IMoldWriteState
{
    FormatFlags ValueAddedFormatFlags { get; }
}

public class ContentTypeWriteState<TContentMold, TToContentMold> : MoldWriteState<TContentMold>, IContentTypeWriteState
    where TContentMold : ContentTypeMold<TContentMold, TToContentMold>
    where TToContentMold : ContentJoinTypeMold<TContentMold, TToContentMold>, IMigrateFrom<TContentMold, TToContentMold>, new()
{
    public const string DblQt = "\"";

    private const FormatFlags AlwaysAddAsStringFormatFlags = DisableAutoDelimiting | EncodeAll;

    protected Type? ContentType;

    public FormatFlags ValueAddedFormatFlags { get; set; }

    public ContentTypeWriteState<TContentMold, TToContentMold> InitializeValueBuilderCompAccess
        (TContentMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WrittenAsFlags writeMethod)
    {
        var createFmtFlags = typeBuilderPortableState.CreateFormatFlags;
        writeMethod = createFmtFlags.DoesNotHaveContentAllowComplexType()
            ? writeMethod.ToNoFieldEquivalent()
            : writeMethod.ToMultiFieldEquivalent();

        Initialize(externalTypeBuilder, typeBuilderPortableState, writeMethod);

        OnFinishedWithStringBuilder = FinishUsingStringBuilder;

        return this;
    }

    private void FinishUsingStringBuilder(IScopeDelimitedStringBuilder finishedBuilding)
    {
        if (Style.IsJson()) finishedBuilding.Append(DblQt);
        Sf.Gb.MarkContentEnd();
    }

    protected (WrittenAsFlags, FormatFlags) RegisterBuildInstanceOnActiveRegistry<TContentValue>
        (TContentValue toBeAdded, FormatFlags withFlags, Type? addType = null)
    {
        ValueAddedFormatFlags = withFlags;
        VisitResult visitResult = default;
        if (!TypeBeingBuilt.IsValueType)
        {
            var usedPrevious = false;
            if (Master.ActiveGraphRegistry.Count > 0)
            {
                var prevVisited  = Master.ActiveGraphRegistry[^1];
                var prevMoldType = prevVisited.MoldState?.Mold.GetType();
                if (prevMoldType == typeof(TrackedInstanceMold)
                 && ReferenceEquals(InstanceOrType, prevVisited.VisitedInstance)
                 && prevVisited.MoldState != null)
                {
                    MoldGraphVisit = visitResult = prevVisited.MoldState!.MoldGraphVisit;
                    usedPrevious   = true;
                }
            }
            if (!usedPrevious)
            {
                MoldGraphVisit = visitResult = !Master.IsExemptFromCircularRefNodeTracking(TypeBeingVisitedAs)
                    ? Master.SourceGraphVisitRefIdUpdateGraph(InstanceOrType, TypeBeingVisitedAs, GetType(), CreateMoldFormatFlags)
                    : Master.ActiveGraphRegistry.VisitCheckNotRequired(MoldGraphVisit.RequesterVisitId);
            }
        }
        else { MoldGraphVisit = visitResult = Master.ActiveGraphRegistry.VisitCheckNotRequired(MoldGraphVisit.RequesterVisitId); }

        var proposedWriteMethod = (!Mold.IsSimpleMold || (MoldGraphVisit.IsARevisit && Style.IsNotLog() || RemainingGraphDepth <= 0))
            ? AsComplex | AsContent
            : AsSimple | AsContent;

        var proposedOuterType = proposedWriteMethod | AsOuterType;

        addType ??= toBeAdded?.GetType() ?? typeof(TContentValue);
        var (buildTypeWriteAsFlags, buildTypeContentCreateFlags)
            = Sf.ResolveMoldWriteAsFormatFlags
                (Master, InstanceOrType, TypeBeingBuilt, proposedOuterType, MoldGraphVisit, CreateMoldFormatFlags, Mold.MoldType);

        var buildInstanceSameAsContentInstance = BuildingInstanceEquals(toBeAdded);
        InnerSameAsOuterType = buildInstanceSameAsContentInstance;


        CreateMoldFormatFlags = buildTypeContentCreateFlags;
        CurrentWriteMethod    = buildTypeWriteAsFlags;

        var isAsStringOrAsValue = withFlags & (AsStringContent | AsValueContent);


        var shouldSuppress = MoldGraphVisit.IsARevisit && !buildTypeWriteAsFlags.HasShowSuppressedContents();
        SkipBody   = shouldSuppress && CreateMoldFormatFlags.DoesNotHaveIsFieldNameFlag();
        SkipFields = shouldSuppress || !Style.IsLog();

        GraphNodeVisit? newVisit = null;
        var             reg      = Master.ActiveGraphRegistry;
        if (!TypeBeingBuilt.IsValueType && visitResult.VisitId.IsRegisterable)
        {
            var fmtState = new FormattingState(reg.CurrentDepth + 1, reg.RemainingDepth - 1, CreateMoldFormatFlags | isAsStringOrAsValue
                                             , Settings.IndentSize, Sf, Sf.ContentEncoder, Sf.LayoutEncoder);

            newVisit =
                new GraphNodeVisit(visitResult.VisitId, visitResult.RequesterVisitId
                                 , InstanceOrType as Type ?? (InstanceOrContainer.GetType()), TypeBeingVisitedAs
                                 , this, buildTypeWriteAsFlags, InstanceOrContainer
                                 , IndentLevel, fmtState, buildTypeContentCreateFlags | isAsStringOrAsValue
                                 , Sb.Length, MoldGraphVisit.LastRevisitCount + 1
                                 , WrittenFlags: MoldWrittenFlags);
        }

        if (buildInstanceSameAsContentInstance && MoldGraphVisit.IsARevisit && buildTypeWriteAsFlags.HasShowSuppressedContents())
        {
            withFlags |= NoRevisitCheck;
        }

        if (SkipBody && shouldSuppress)
        {
            if (addType.IsString() && Settings.InstanceMarkingIncludeStringContents)
            {
                SkipBody   =  false;
                withFlags  |= buildInstanceSameAsContentInstance ? NoRevisitCheck : DefaultCallerTypeFlags;
                SkipFields =  !Style.IsLog();
            }
            else if (!addType.IsValueType && TypeBeingBuilt.IsSpanFormattableCached()
                                          && Settings.InstanceMarkingIncludeSpanFormattableContents)
            {
                SkipBody   =  false;
                withFlags  |= buildInstanceSameAsContentInstance ? NoRevisitCheck : DefaultCallerTypeFlags;
                SkipFields =  !Style.IsLog();
            }
            else if (addType.IsCharSequence() && Settings.InstanceMarkingIncludeCharSequenceContents)
            {
                SkipBody   =  false;
                withFlags  |= buildInstanceSameAsContentInstance ? NoRevisitCheck : DefaultCallerTypeFlags;
                SkipFields =  !Style.IsLog();
            }
            else if (addType.IsStringBuilder() && Settings.InstanceMarkingIncludeStringBuilderContents)
            {
                SkipBody   =  false;
                withFlags  |= buildInstanceSameAsContentInstance ? NoRevisitCheck : DefaultCallerTypeFlags;
                SkipFields =  !Style.IsLog();
            }
            else if (addType.IsCharArray() && Settings.InstanceMarkingIncludeCharArrayContents)
            {
                SkipBody   =  false;
                withFlags  |= buildInstanceSameAsContentInstance ? NoRevisitCheck : DefaultCallerTypeFlags;
                SkipFields =  !Style.IsLog();
            }
            else if (!addType.IsStringBearerOrNullableCached() && !addType.IsSpanFormattableOrNullableCached() &&
                     Settings.InstanceMarkingIncludeObjectToStringContents)
            {
                SkipBody   =  false;
                withFlags  |= buildInstanceSameAsContentInstance ? NoRevisitCheck : DefaultCallerTypeFlags;
                SkipFields =  !Style.IsLog();
            }
        }

        Mold.StartTypeOpening(buildTypeContentCreateFlags | isAsStringOrAsValue);

        if (newVisit != null && visitResult.VisitId.IsRegisterable)
        {
            var newVisitValue = newVisit.Value;
            newVisitValue = newVisitValue.UpdateMoldWrittenFlags(MoldWrittenFlags);
            Master.ActiveGraphRegistry.Add(newVisitValue.SetBufferFirstFieldStartAndIndentLevel(Master.WriteBuffer.Length, IndentLevel));
            Master.ActiveGraphRegistry.CurrentGraphNodeIndex = newVisitValue.NodeVisitId.VisitIndex;
        }

        Mold.FinishTypeOpening(buildTypeContentCreateFlags | isAsStringOrAsValue);

        var addContentWriteAsFlags = (buildTypeWriteAsFlags & ~(AsOuterType)) | AsInnerType;
        var addContentCreateFlags  = buildTypeContentCreateFlags;

        if (!SkipBody)
        {
            var addGraphVisit =
                !addType.IsValueType
                    ? !Master.IsExemptFromCircularRefNodeTracking(addType)
                        // ? Master.JustSourceGraphVisitResult(toBeAdded, addType, GetType(), withFlags)
                        ? Master.SourceGraphVisitRefIdUpdateGraph(toBeAdded, addType, GetType(), withFlags)
                        : Master.ActiveGraphRegistry.VisitCheckNotRequired(MoldGraphVisit.RequesterVisitId)
                    : Master.ActiveGraphRegistry.VisitCheckNotRequired(MoldGraphVisit.RequesterVisitId);
            proposedWriteMethod =
                (addType.IsStringBearerOrNullableCached()
              || addType.IsConcreteOfGeneric(typeof(PalantírReveal<>))
              || (addGraphVisit.IsARevisit && Style.IsNotLog() || RemainingGraphDepth <= 0))
                    ? AsComplex | AsContent
                    : AsSimple | AsContent;
            var proposedInnerContent = proposedWriteMethod | AsInnerType;

            withFlags |= (CreateMoldFormatFlags & IsFieldName);
            (addContentWriteAsFlags, addContentCreateFlags)
                = Sf.ResolveMoldWriteAsFormatFlags(Master, toBeAdded, addType, proposedInnerContent, addGraphVisit, withFlags, Mold.MoldType);

            if (addType == TypeBeingBuilt
             && (buildTypeContentCreateFlags.HasAddTypeNameFieldFlag()
              || buildTypeContentCreateFlags.HasNoneOf(SuppressOpening | LogSuppressTypeNames)
              || (!addType.IsValueType && !addContentCreateFlags.HasSuppressOpening() && !addGraphVisit.IsARevisit)))
            {
                addContentCreateFlags &= ~(AddTypeNameField);
                addContentCreateFlags |= LogSuppressTypeNames;
            }
            // CreateMoldFormatFlags |= isAsStringOrAsValue;
            if (buildInstanceSameAsContentInstance)
            {
                if (!TypeBeingBuilt.IsValueType) { MoldGraphVisit = MoldGraphVisit.WitReusedCount(-1); }
                // WroteTypeOpen &= !WroteTypeName;
                WroteTypeOpen = false;
            }
        }
        return (addContentWriteAsFlags, addContentCreateFlags);
    }

    private Action<IScopeDelimitedStringBuilder>? OnFinishedWithStringBuilder { get; set; }

    public bool IsLog => Style.IsLog();

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()
    {
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) { RegisterBuildInstanceOnActiveRegistry("Custom", DefaultCallerTypeFlags); }
        Sf.Gb.StartNextContentSeparatorPaddingSequence(Sb, DefaultCallerTypeFlags, true);
        if (Style.IsJson()) Sb.Append(DblQt);
        var scopedSb = (IScopeDelimitedStringBuilder)Sb;
        scopedSb.OnScopeEndedAction = OnFinishedWithStringBuilder;
        return scopedSb;
    }

    public TToContentMold FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(bool?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var moldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, "", formatString, moldInherited), formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange)
            VettedAppendBoolContent(value, formatString, resolvedFlags);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendBoolContent(value, formatString, resolvedFlags); }
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var moldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, "", formatString, moldInherited), formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendBoolContent(value, formatString, resolvedFlags);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendBoolContent(value, formatString, resolvedFlags); }
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(bool?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        var fieldNameFormatter = Sf;

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, "", formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (!callContext.HasFormatChange)
            VettedAppendBoolContent(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendBoolContent(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (!actualType.IsValueType && !Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(bool?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, "", formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange)
            VettedAppendBoolContent(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendBoolContent(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (!actualType.IsValueType && !Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendBoolContent(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var startAt    = Sb.Length;
        var actualType = value?.GetType() ?? typeof(bool?);
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, actualType);
            }
            var writtenAsNull = AppendNull(formatString, formatFlags);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAsNull, actualType);
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var writtenAs = formatFlags.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(this, value, formatString, formatFlags: formatFlags)
            : StyleFormatter.FormatFieldContents(this, value, formatString, formatFlags);

        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
    }

    public TToContentMold FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (SupportsMultipleFields && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), CurrentWriteMethod, formatFlags);
        }

        var moldInherited = flags; //| AsValueContent not adding this to enable auto delimiting;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, moldInherited), formatString);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange)
            VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags); }
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TFmt>(TFmt? value, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var moldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, moldInherited), formatString);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange)
            VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags); }
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
      , bool addEndDblQt = true)
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (SupportsMultipleFields && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(typeof(TFmt), CurrentWriteMethod, formatFlags);
        }
        AppendSummary appendSum;
        if (!callContext.HasFormatChange)
            appendSum = VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                appendSum = VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        if (!actualType.IsValueType && !Settings.InstanceTrackingAllAsStringHaveLocalTracking && callContext.HasFormatChange)
        {
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TFmt>(TFmt value, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        AppendSummary appendSum;
        if (!callContext.HasFormatChange)
            appendSum = VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                appendSum = VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        if (!actualType.IsValueType && !Settings.InstanceTrackingAllAsStringHaveLocalTracking && callContext.HasFormatChange)
        {
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendSpanFormattableContent<TFmt>(TFmt value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        var startAt    = Sb.Length;
        var actualType = value?.GetType() ?? typeof(TFmt);

        if (value == null)
        {
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var defaultWrittenAs = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, defaultValue.Length, formatFlags | NoRevisitCheck)
                    : (formatFlags.HasAsValueContentFlag()
                        ? StyleFormatter.FormatFallbackFieldContents<TFmt>(this, defaultValue, 0, formatString, defaultValue.Length
                                                                         , formatFlags | NoRevisitCheck)
                        : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, defaultValue.Length, formatFlags | NoRevisitCheck));
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, defaultWrittenAs | AsFallbackValue, actualType);
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, actualType);
            }
            var writtenAsNull = AppendNull(formatString, formatFlags);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAsNull, actualType);
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var snapState = SnapshotWriteState;
        var result    = this.AppendFormattedOrNull(value, formatString, formatFlags);
        SnapshotWriteState = snapState;
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        CurrentWriteMethod |= result.WrittenAs & ~(AsSimple | AsComplex) | (formatFlags.HasAsStringContentFlag() ? AsString : AsValue);
        return result.SetStringRange(startAt, Sb.Length);
    }

    public TToContentMold FieldValueOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var moldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, moldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendNullableStructSpanFormattableContent(value, defaultValue, formatString, resolvedFlags | AsValueContent);
        else
        {
            AppendSummary appendSum;
            using (callContext)
            {
                appendSum = VettedAppendNullableStructSpanFormattableContent(value, defaultValue, formatString, resolvedFlags | AsValueContent);
            }
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TFmtStruct>(TFmtStruct? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var moldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, moldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange)
            VettedAppendNullableStructSpanFormattableContent(value, defaultValue, formatString, resolvedFlags | AsValueContent);
        else
        {
            AppendSummary appendSum;
            using (callContext)
            {
                appendSum = VettedAppendNullableStructSpanFormattableContent(value, defaultValue, formatString, resolvedFlags | AsValueContent);
            }
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
      , bool addEndDblQt = true) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        var flags = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (!callContext.HasFormatChange)
            VettedAppendNullableStructSpanFormattableContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendNullableStructSpanFormattableContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TFmtStruct>(TFmtStruct? value, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        var flags = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendNullableStructSpanFormattableContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary appendSum;
            using (callContext)
            {
                appendSum = VettedAppendNullableStructSpanFormattableContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                                           , addEndDblQt);
            }
            if (!actualType.IsValueType && !Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendNullableStructSpanFormattableContent<TFmtStruct>(TFmtStruct? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var startAt    = Sb.Length;
        var actualType = typeof(TFmtStruct?);

        if (value == null)
        {
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var defaultWrittenAs = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags | NoRevisitCheck)
                    : (formatFlags.HasAsValueContentFlag()
                        ? StyleFormatter.FormatFallbackFieldContents<TFmtStruct>(this, defaultValue, 0, formatString, defaultValue.Length
                                                                               , formatFlags: formatFlags | NoRevisitCheck)
                        : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags | NoRevisitCheck));
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, defaultWrittenAs | AsFallbackValue, actualType);
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, actualType);
            }
            var writtenAsNull = AppendNull(formatString, formatFlags);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAsNull, actualType);
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var writtenAs = this.AppendNullableFormattedOrNull(value, formatString, formatFlags);
        CurrentWriteMethod |= writtenAs & ~(AsSimple | AsComplex) | (formatFlags.HasAsStringContentFlag() ? AsString : AsValue);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
    }

    public TToContentMold FieldValueOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent, palantírReveal.GetType());

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = flags | AsValueContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags     = maybeComplex | StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, maybeComplex);
        var callContext       = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange)
        {
            VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, writeAs);
        }
        else
        {
            using (callContext) { VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, writeAs); }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent, palantírReveal.GetType());
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
        {
            VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, writeAs);
        }
        else
        {
            using (callContext) { VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, writeAs); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName
      , TCloaked value, PalantírReveal<TRevealBase> palantírReveal, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);

        var writeAs = AsComplex | AsContent | AsInnerType;
        var flags   = formatFlags;

        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent, palantírReveal.GetType());

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var maybeComplex = flags | AsStringContent;
        var resolvedFlags = 
            StyleFormatter
                .ResolveContentFormatFlags
                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags
                     (value, defaultValue, formatString, maybeComplex) , formatString) | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (writeAs, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags, palantírReveal.GetType());
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        if (!callContext.HasFormatChange)
        {
            VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                           , writeAs, addStartDblQt, addEndDblQt);
        }
        else
        {
            AppendSummary appendSum;
            using (callContext)
            {
                appendSum = VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                                           , writeAs, addStartDblQt, addEndDblQt);
            }
            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);

        var writeAs = AsComplex | AsContent | AsInnerType;
        var flags   = formatFlags;

        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent, palantírReveal.GetType());
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = flags | AsStringContent;
        var resolvedFlags = 
            StyleFormatter.ResolveContentFormatFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags
                 (value, defaultValue, formatString, maybeComplex) , formatString) | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (writeAs, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags, palantírReveal.GetType());
        if (!callContext.HasFormatChange)
            VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                           , writeAs, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary appendSum;
            using (callContext)
            {
                appendSum = VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                                           , writeAs, addStartDblQt, addEndDblQt);
            }

            if (!actualType.IsValueType && appendSum.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public AppendSummary VettedAppendCloakedBearerContent<TCloaked, TRevealBase>(TCloaked value, PalantírReveal<TRevealBase> palantírReveal
      , string? defaultValue = null, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags
      , WrittenAsFlags writtenAsFlags = AsComplex | AsContent | AsInnerType, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        AppendSummary result;

        var startAt = Sb.Length;
        var writeAs = (formatFlags.HasAsStringContentFlag() ? AsString : AsValue);
        if (value == null)
        {
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var append = this.AppendFormattedOrNull(defaultValue, formatString ?? "", formatFlags: formatFlags | NoRevisitCheck);
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(AsFallbackValue);
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                AppendNull("", formatFlags);
                writtenAsFlags = AsNull;
            }
            result =
                new AppendSummary(Mold.GetType(), Master, new Range(startAt, Sb.Length)
                                , writtenAsFlags, VisitId.NoVisitRequiredId, typeof(TCloaked?));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var snapState = SnapshotWriteState;
            result             = this.RevealCloakedBearerOrNull(value, palantírReveal, formatString, formatFlags, writtenAsFlags);
            SnapshotWriteState = snapState;
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        CurrentWriteMethod |= result.WrittenAs & ~(AsSimple | AsComplex) | writeAs;
        return result.SetStringRange(startAt, Sb.Length);
    }

    public TToContentMold FieldValueOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent, palantírReveal.GetType());

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = 
            StyleFormatter.ResolveContentFormatFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags
                 (value, defaultValue, formatString, withMoldInherited) , formatString) | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, writeAs);
        else
        {
            using (callContext)
            {
                VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, writeAs);
            }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent, palantírReveal.GetType());
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags =
            StyleFormatter.ResolveContentFormatFlags
                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags
                     (value, defaultValue, formatString, withMoldInherited), formatString) | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, writeAs);
        else
        {
            using (callContext)
            {
                VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, writeAs);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent, palantírReveal.GetType());

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var resolvedFlags = 
            StyleFormatter
                .ResolveContentFormatFlags
                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags
                     (value, defaultValue, formatString, flags | AsStringContent), formatString) | AsStringContent;


        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.FormatFieldName(this, nonJsonfieldName);
            Sf.AppendFieldValueSeparator();
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
        {
            VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                                         , writeAs, addStartDblQt, addEndDblQt);
        }
        else
        {
            AppendSummary result;
            using (callContext)
            {
                result = VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                                                      , writeAs, addStartDblQt, addEndDblQt);
            }
            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking && result.VisitNumber.VisitIndex >= 0)
            {
                Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent, palantírReveal.GetType());
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = 
            StyleFormatter.ResolveContentFormatFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, "", formatString, flags | AsStringContent)
            , formatString) | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                                         , writeAs, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                                             , writeAs, addStartDblQt, addEndDblQt);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendNullableStructCloakedBearerContent<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? defaultValue = null, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, WrittenAsFlags writtenAsFlags = AsComplex | AsContent | AsInnerType
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var startAt = Sb.Length;
        if (value == null)
        {
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var append = this.AppendFormattedOrNull(defaultValue, formatString ?? "", formatFlags: formatFlags | NoRevisitCheck);
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(AsFallbackValue);
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                AppendNull("", formatFlags);
                writtenAsFlags = AsNull;
            }
            CurrentWriteMethod |= AsString | writtenAsFlags;
            return new AppendSummary(Mold.GetType(), Master, new Range(startAt, Sb.Length)
                                   , writtenAsFlags, VisitId.NoVisitRequiredId, typeof(TCloakedStruct?));
        }
        var writeAs = (formatFlags.HasAsStringContentFlag() ? AsString : AsValue);
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var result = this.RevealNullableCloakedBearerOrNull(value, palantírReveal, formatString, formatFlags, writtenAsFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        CurrentWriteMethod |= (result.WrittenAs & ~(AsSimple | AsComplex)) | writeAs;
        return result.SetStringRange(startAt, Sb.Length);
    }

    public TToContentMold FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = flags | AsValueContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags     = maybeComplex | StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, maybeComplex);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);


        if (!callContext.HasFormatChange) { VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs); }
        else
        {
            using (callContext) { VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TBearer>(TBearer value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);

        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = 
            StyleFormatter.ResolveContentFormatFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, maybeComplex)
            , formatString) | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs);
        else
        {
            using (callContext) { VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);

        var writeAs = AsComplex | AsContent | AsInnerType | AsString;

        FormatFlags flags = DefaultCallerTypeFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
        {
            (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        }

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = flags | DisableAutoDelimiting | AsStringContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = 
            StyleFormatter
                .ResolveContentFormatFlags
                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags
                     (value, defaultValue, formatString, maybeComplex) , formatString) | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
        {
            (writeAs, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        }
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.PreviousContextOrThis.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (!callContext.HasFormatChange)
            VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs, addStartDblQt, addEndDblQt);
            }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TBearer>(TBearer value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags | DisableAutoDelimiting | AsStringContent;
        var writeAs      = AsComplex | AsContent | AsInnerType | AsString;
        var resolvedFlags = 
            StyleFormatter
                .ResolveContentFormatFlags
                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags
                     (value, defaultValue, formatString, maybeComplex) , formatString) | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (writeAs, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs, addStartDblQt, addEndDblQt);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendStringBearerContent<TBearer>(TBearer value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , WrittenAsFlags writtenAsFlags = AsComplex | AsContent | AsInnerType, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        var startAt = Sb.Length;
        if (value == null)
        {
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var append = this.AppendFormattedOrNull(defaultValue, formatString, formatFlags: formatFlags | NoRevisitCheck);
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(AsFallbackValue);
            }
            else if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                AppendNull("", formatFlags);
                writtenAsFlags = AsNull;
            }
            CurrentWriteMethod |= AsString | writtenAsFlags;
            return new AppendSummary(Mold.GetType(), Master, new Range(startAt, Sb.Length)
                                   , writtenAsFlags, VisitId.NoVisitRequiredId, typeof(TBearer));
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var snapState       = SnapshotWriteState;
        var result          = this.RevealStringBearerOrNull(value, formatString, formatFlags, writtenAsFlags);
        var updateWrittenAs = CurrentWriteMethod;
        SnapshotWriteState = snapState;
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        if (Style.IsLog())
        {
            updateWrittenAs =  snapState.CurrentWriteMethod;
            updateWrittenAs &= ~(AsSimple | AsComplex);
            updateWrittenAs |= (result.WrittenAs & (AsSimple | AsComplex));
        }
        updateWrittenAs    |= formatFlags.HasAsStringContentFlag() ? AsString : AsValue;
        CurrentWriteMethod |= updateWrittenAs;
        return result.SetStringRange(startAt, Sb.Length);
    }

    public TToContentMold FieldValueOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = 
            StyleFormatter
                .ResolveContentFormatFlags
                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags
                     (value, defaultValue, formatString, withMoldInherited) , formatString) | AsValueContent;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs);
        else
        {
            using (callContext) { VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TBearerStruct>(TBearerStruct? value
      , string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = 
            StyleFormatter
                .ResolveContentFormatFlags
                 (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags
                      (value, defaultValue, formatString, withMoldInherited) , formatString) | AsValueContent;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs);
        else
        {
            using (callContext) { VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
      , TBearerStruct? value, string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;

        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var maybeComplex = flags | AsStringContent;
        var resolvedFlags =
            StyleFormatter.ResolveContentFormatFlags
                    (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags
                         (value, defaultValue, formatString, maybeComplex) , formatString) | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs, addStartDblQt, addEndDblQt);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TBearerStruct>(TBearerStruct? value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType          = actualType;
        var (writeAs, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = flags | AsStringContent;
        var resolvedFlags = 
            StyleFormatter.ResolveContentFormatFlags
                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags
                     (value, defaultValue, formatString, maybeComplex), formatString) | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, writeAs, addStartDblQt, addEndDblQt);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendNullableStructStringBearerContent<TBearerStruct>(TBearerStruct? value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , WrittenAsFlags writtenAsFlags = AsComplex | AsContent | AsInnerType, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var startAt = Sb.Length;
        if (value == null)
        {
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var append = this.AppendFormattedOrNull(defaultValue, formatString, formatFlags: formatFlags | NoRevisitCheck);
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(AsFallbackValue);
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                AppendNull("", formatFlags);
                writtenAsFlags = AsNull;
            }
            CurrentWriteMethod |= AsString | writtenAsFlags;
            return new AppendSummary(Mold.GetType(), Master, new Range(startAt, Sb.Length)
                                   , writtenAsFlags, VisitId.NoVisitRequiredId, typeof(TBearerStruct));
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var result = this.RevealNullableStringBearerOrNull(value, formatString, formatFlags, writtenAsFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        CurrentWriteMethod |= (result.WrittenAs & ~(AsSimple | AsComplex)) | (formatFlags.HasAsStringContentFlag() ? AsString : AsValue);
        return result.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(CurrentWriteMethod);
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , ReadOnlySpan<char> fallbackValue, bool emptyIsNull, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = 
            StyleFormatter.ResolveContentFormatFlags
                (Sb, "Span", StyleFormatter.ResolveContentAsValueFormatFlags
                     ("Span", "", formatString, withMoldInherited) , formatString) | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags);
        else
        {
            using (callContext) { VettedAppendCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(Span<char> value, ReadOnlySpan<char> fallbackValue, bool emptyIsNull
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = 
            StyleFormatter.ResolveContentFormatFlags
                (Sb, "Span", StyleFormatter.ResolveContentAsValueFormatFlags
                     ("Span", "", formatString, withMoldInherited), formatString) | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags);
        else
        {
            using (callContext) { VettedAppendCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, ReadOnlySpan<char> defaultValue
      , bool isEmptyNull
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(Span<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormatFlags("Span", "", formatString, withMoldInherited), formatString);
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendCharSpanContent(value, defaultValue, isEmptyNull, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendCharSpanContent(value, defaultValue, isEmptyNull, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(Span<char> value, ReadOnlySpan<char> defaultValue, bool isEmptyNull, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(Span<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, "Span", StyleFormatter.ResolveContentAsStringFormatFlags("Span", "", formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendCharSpanContent(value, defaultValue, isEmptyNull, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendCharSpanContent(value, defaultValue, isEmptyNull, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        return Mold.TransitionToNextMold();
    }

    public WrittenAsFlags VettedAppendCharSpanContent(Span<char> value, ReadOnlySpan<char> defaultValue, bool isEmptyNull, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var writtenAs = (formatFlags.HasAsStringContentFlag() ? AsString : AsValue);
        if (value.Length == 0)
        {
            if (defaultValue.Length > 0 || !isEmptyNull)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAs |= this.AppendFormattedOrEmptyOnZeroLength(defaultValue, formatString, 0, int.MaxValue, formatFlags);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return writtenAs;
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Empty;
            }
            return Sf.AppendFormattedNull(Sb, formatString, formatFlags);
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

        writtenAs |= this.AppendFormattedOrNullOnZeroLength(value, formatString, 0, int.MaxValue, formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        CurrentWriteMethod |= writtenAs;
        return writtenAs;
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , ReadOnlySpan<char> fallbackValue, bool emptyIsNull, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
            (Sb, "ReadOnlySpan"
           , StyleFormatter.ResolveContentAsValueFormatFlags("ReadOnlySpan", "", formatString, withMoldInherited)
           , formatString) | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendReadOnlyCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags);
        else
        {
            using (callContext) { VettedAppendReadOnlyCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue, bool emptyIsNull
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
            (Sb, "ReadOnlySpan"
           , StyleFormatter.ResolveContentAsValueFormatFlags("ReadOnlySpan", "", formatString, withMoldInherited)
           , formatString) | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendReadOnlyCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags);
        else
        {
            using (callContext) { VettedAppendReadOnlyCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags); }
        }

        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , ReadOnlySpan<char> defaultValue, bool isEmptyNull, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);


        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
            (Sb, "ReadOnlySpan"
           , StyleFormatter.ResolveContentAsStringFormatFlags("ReadOnlySpan", "", formatString, withMoldInherited)
           , formatString) | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendReadOnlyCharSpanContent(value, defaultValue, isEmptyNull, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendReadOnlyCharSpanContent(value, defaultValue, isEmptyNull, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(ReadOnlySpan<char> value, ReadOnlySpan<char> defaultValue, bool isEmptyNull
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
            (Sb, "ReadOnlySpan"
           , StyleFormatter.ResolveContentAsStringFormatFlags("ReadOnlySpan", "", formatString, withMoldInherited)
           , formatString) | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!callContext.HasFormatChange)
            VettedAppendReadOnlyCharSpanContent(value, defaultValue, isEmptyNull, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedAppendReadOnlyCharSpanContent(value, defaultValue, isEmptyNull, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }

        return Mold.TransitionToNextMold();
    }

    public WrittenAsFlags VettedAppendReadOnlyCharSpanContent(ReadOnlySpan<char> value, ReadOnlySpan<char> defaultValue, bool isEmptyNull
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var writtenAs = (formatFlags.HasAsStringContentFlag() ? AsString : AsValue);
        if (value.Length == 0)
        {
            if (defaultValue.Length > 0 || !isEmptyNull)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAs |= this.AppendFormattedOrEmptyOnZeroLength(defaultValue, formatString, 0, int.MaxValue, formatFlags);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return writtenAs;
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Empty;
            }
            return AppendNull(formatString, formatFlags);
        }

        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        writtenAs |= this.AppendFormattedOrNullOnZeroLength(value, formatString, 0, int.MaxValue, formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return writtenAs;
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }


        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && Settings.InstanceMarkingIncludeStringContents)
        {
            Sf.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && Settings.InstanceMarkingIncludeStringContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendStringContent(string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var startAt = Sb.Length;

        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var snapState = SnapshotWriteState;
                var result    = this.AppendFormattedOrNull(value, formatString, capStart, capLength, formatFlags);
                SnapshotWriteState = snapState;
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return result.SetStringRange(startAt, Sb.Length);
            }
        }
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var snapState = SnapshotWriteState;
            var result    = this.AppendFormattedOrNull(defaultValue, formatString, 0, defaultValue.Length, formatFlags | NoRevisitCheck);
            SnapshotWriteState = snapState;
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return result.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(AsFallbackValue);
        }
        if (value != null && formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                var result = this.AppendFormattedOrNull("", formatString, 0, prefixSuffixLength, formatFlags | NoRevisitCheck);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return result.SetStringRange(startAt, Sb.Length);
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag())
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, typeof(string));
        var writtenAs = Sf.AppendFormattedNull(Sb, formatString, formatFlags);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(string));
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && Settings.InstanceMarkingIncludeCharArrayContents)
        {
            Sf.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(char[]? value, int startIndex, int length, string? defaultValue = null
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && Settings.InstanceMarkingIncludeCharArrayContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                    , addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(char[]? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                    , addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendCharArrayContent(char[]? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var startAt = Sb.Length;

        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var snapState = SnapshotWriteState;
                var append    = this.AppendFormattedOrNull(value, formatString, capStart, capLength, formatFlags);
                SnapshotWriteState = snapState;
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length);
            }
        }
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var append = this.AppendFormattedOrNull(defaultValue, formatString, 0, defaultValue.Length, formatFlags | NoRevisitCheck);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return append.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(AsFallbackValue);
        }
        if (value != null && formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                var append = this.AppendFormattedOrNull("", formatString, 0, prefixSuffixLength, formatFlags | NoRevisitCheck);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length);
            }
        }
        if (formatFlags.HasNullBecomesEmptyFlag()) return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, typeof(char[]));
        var writtenAs = AppendNull(formatString, formatFlags);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(char[]));
    }

    public TToContentMold FieldValueOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length, string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && Settings.InstanceMarkingIncludeCharSequenceContents)
        {
            Sf.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCharSeq>(TCharSeq value, int startIndex, int length, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length, string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && Settings.InstanceMarkingIncludeCharSequenceContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                   , addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                       , addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCharSeq>(TCharSeq value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        var flags                   = formatFlags;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                   , addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                       , addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendCharSequenceContent<TCharSeq>(TCharSeq value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        var startAt    = Sb.Length;
        var actualType = value?.GetType() ?? typeof(TCharSeq);

        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var snapState = SnapshotWriteState;
                var append    = this.AppendFormattedOrNull(value, formatString, capStart, capLength, formatFlags);
                SnapshotWriteState = snapState;
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length);
            }
        }
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var append = this.AppendFormattedOrNull(defaultValue, formatString, 0, defaultValue.Length, formatFlags | NoRevisitCheck);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return append.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(AsFallbackValue);
        }
        if (value != null && formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var append = this.AppendFormattedOrNull("", formatString, 0, prefixSuffixLength, formatFlags | NoRevisitCheck);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length);
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag())
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, actualType);
        var writtenAs = AppendNull(formatString, formatFlags);

        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && Settings.InstanceMarkingIncludeStringBuilderContents)
        {
            Sf.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(StringBuilder? value, int startIndex, int length, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && Settings.InstanceMarkingIncludeStringBuilderContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                    , addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                        , addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(StringBuilder? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                    , addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                        , addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendStringBuilderContent(StringBuilder? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var startAt = Sb.Length;

        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var snapState = SnapshotWriteState;
                var append    = this.AppendFormattedOrNull(value, formatString, capStart, capLength, formatFlags);
                SnapshotWriteState = snapState;
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length);
            }
        }
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var append = this.AppendFormattedOrNull(defaultValue, formatString, 0, defaultValue.Length, formatFlags | NoRevisitCheck);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return append.SetStringRange(startAt, Sb.Length).AddWrittenAsFlags(AsFallbackValue);
        }
        if (value != null && formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var append = this.AppendFormattedOrNull("", formatString, 0, prefixSuffixLength, formatFlags | NoRevisitCheck);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return append.SetStringRange(startAt, Sb.Length);
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag())
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, typeof(StringBuilder));
        var writtenAs = AppendNull(formatString, formatFlags);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(StringBuilder));
    }

    public TToContentMold ValueMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        var isInputType = TypeBeingBuilt.IsInputConstructionTypeCached();
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && !isInputType && Settings.InstanceMarkingIncludeObjectToStringContents)
        {
            Sf.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueMatchWithDefaultJoin<TAny>(TAny? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType    = actualType;
        var (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags);
        else
        {
            using (callContext) { result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public TToContentMold StringMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        var flags                   = formatFlags;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        var isInputType = TypeBeingBuilt.IsInputConstructionTypeCached();
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (MoldGraphVisit.IsARevisit && SupportsMultipleFields && !isInputType && Settings.InstanceMarkingIncludeObjectToStringContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(actualType, CurrentWriteMethod, formatFlags);
        }

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext) { result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringMatchWithDefaultJoin<TAny>(TAny? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        var flags                   = formatFlags;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !BuildingInstanceEquals(value))
            (_, flags) = RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = flags | AlwaysAddAsStringFormatFlags | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormatFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormatFlags(value, "", formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            (_, resolvedFlags) = RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext) { result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber.VisitIndex >= 0) { Master.UpdateVisitEncoders(result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
        }
        return Mold.TransitionToNextMold();
    }

    public AppendSummary VettedAppendMatchContent<TAny>(TAny? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var startAt = Sb.Length;
        if (value != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var snapState = SnapshotWriteState;
            var result    = this.AppendMatchFormattedOrNull(value, formatString, formatFlags);
            SnapshotWriteState = snapState;
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return result.SetStringRange(startAt, Sb.Length);
        }
        var actualType = value?.GetType() ?? typeof(TAny);

        WrittenAsFlags writtenAs;
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            writtenAs = formatFlags.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags | NoRevisitCheck)
                : (formatFlags.HasAsValueContentFlag()
                    ? StyleFormatter.FormatFallbackFieldContents<TAny>(this, defaultValue, 0, formatString, formatFlags: formatFlags | NoRevisitCheck)
                    : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags | NoRevisitCheck));
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
        }
        if (formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                if (formatFlags.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(this, "", 0, formatString, formatFlags: formatFlags | NoRevisitCheck);
                }
                else { StyleFormatter.FormatFieldContents(this, "", 0, formatString, formatFlags: formatFlags | NoRevisitCheck); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, actualType);
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag())
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, Empty, actualType);
        writtenAs = AppendNull(formatString, formatFlags);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
    }

    protected WrittenAsFlags AppendNull(string formatString, FormatFlags formatFlags) =>
        StyleFormatter.AppendFormattedNull(Sb, formatString, formatFlags);

    public TToContentMold ConditionalValueTypeSuffix()
    {
        if (SupportsMultipleFields) { return Mold.TransitionToNextMold(); }
        Sf.Gb.Complete(Sf.Gb.CurrentSectionRanges.StartedWithFormatFlags);
        return Mold.TransitionToNextMold();
    }

    public override bool HasSkipBody(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => SkipBody;


    public new TToContentMold WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)

    {
        return Mold.TransitionToNextMold();
    }

    public override IMoldWriteState CopyFrom(IMoldWriteState? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        base.CopyFrom(source, copyMergeFlags);
        SkipFields = SkipFields | SkipBody;
        if (source is ContentTypeWriteState<TContentMold, TToContentMold> valueTypeDieCast)
        {
            CurrentWriteMethod = valueTypeDieCast.CurrentWriteMethod;
        }

        return this;
    }
}
