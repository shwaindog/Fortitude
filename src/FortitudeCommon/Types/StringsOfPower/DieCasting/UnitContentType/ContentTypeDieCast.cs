// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentTypeDieCast<TContentMold, TToContentMold> : TypeMolderDieCast<TContentMold>
    where TContentMold : ContentTypeMold<TContentMold, TToContentMold>
    where TToContentMold : ContentJoinTypeMold<TContentMold, TToContentMold>, IMigrateFrom<TContentMold, TToContentMold>, new()
{
    public const string DblQt = "\"";

    protected Type? ContentType;

    public ContentTypeDieCast<TContentMold, TToContentMold> InitializeValueBuilderCompAccess
        (TContentMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WriteMethodType writeMethod)
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

    protected void RegisterBuildInstanceOnActiveRegistry<TContentValue>(TContentValue toBeAdded, FormatFlags withFlags)
    {
        if (!TypeBeingBuilt.IsValueType)
        {
            MoldGraphVisit = !Master.IsExemptFromCircularRefNodeTracking(TypeBeingVisitedAs)
                ? Master.SourceGraphVisitRefId(InstanceOrContainer, TypeBeingVisitedAs, CreateMoldFormatFlags)
                : Master.ActiveGraphRegistry.VisitCheckNotRequired();
        }
        else { MoldGraphVisit = Master.ActiveGraphRegistry.VisitCheckNotRequired(); }

        var proposedWriteMethod = (!StyleTypeBuilder.IsSimpleMold || (MoldGraphVisit.IsARevisit || RemainingGraphDepth <= 0))
            ? WriteMethodType.MoldComplexContentType
            : WriteMethodType.MoldSimpleContentType;

        var addType = toBeAdded?.GetType() ?? typeof(TContentValue);
        var nextCreateFlags
            = Sf.GetFormatterContentHandlingFlags(Master, toBeAdded, addType
                                                , proposedWriteMethod, MoldGraphVisit, withFlags);

        var newWriteMethod = nextCreateFlags.HasContentAllowComplexType() || StyleTypeBuilder.IsComplexType
            ? proposedWriteMethod.ToMultiFieldEquivalent()
            : proposedWriteMethod.ToNoFieldEquivalent();

        if (CurrentWriteMethod != newWriteMethod)
        {
            var newFlags = CreateMoldFormatFlags;
            if (newWriteMethod == WriteMethodType.MoldComplexContentType)
            {
                newFlags &= ~(SuppressOpening | SuppressClosing);
                newFlags |= ContentAllowComplexType;
            }
            else
            {
                newFlags &= ~(ContentAllowComplexType);
                newFlags |= SuppressOpening | SuppressClosing;
            }

            CreateMoldFormatFlags = newFlags;
            CurrentWriteMethod    = newWriteMethod;
        }

        var isAsStringOrAsValue = withFlags & (AsStringContent | AsValueContent);

        GraphNodeVisit? newVisit = null;
        if (!TypeBeingBuilt.IsValueType)
        {
            var reg = Master.ActiveGraphRegistry;

            var fmtState = new FormattingState(reg.CurrentDepth + 1, reg.RemainingDepth - 1, CreateMoldFormatFlags | isAsStringOrAsValue
                                             , Settings.IndentSize, Sf, Sf.ContentEncoder, Sf.LayoutEncoder);

            newVisit =
                new GraphNodeVisit(reg.RegistryId, reg.Count, reg.CurrentGraphNodeIndex
                                 , InstanceOrContainer?.GetType() ?? TypeBeingBuilt, TypeBeingVisitedAs
                                 , this, newWriteMethod, InstanceOrContainer
                                 , IndentLevel, Master.CallerContext, fmtState, CreateMoldFormatFlags | isAsStringOrAsValue
                                 , Sb.Length, MoldGraphVisit.LastRevisitCount + 1);
        }

        StyleTypeBuilder.StartTypeOpening();
        
        if (!TypeBeingBuilt.IsValueType && !MoldGraphVisit.NoVisitCheckDone && newVisit != null)
        {
            Master.ActiveGraphRegistry.Add(newVisit.Value.SetBufferFirstFieldStart(Master.WriteBuffer.Length, IndentLevel));
            Master.ActiveGraphRegistry.CurrentGraphNodeIndex = newVisit.Value.ObjVisitIndex;
        }

        StyleTypeBuilder.FinishTypeOpening();
        CreateMoldFormatFlags |= isAsStringOrAsValue;

        var fmtFlags             = CreateMoldFormatFlags;
        var hasBeenVisitedBefore = MoldGraphVisit.IsARevisit;
        SkipBody   = hasBeenVisitedBefore && fmtFlags.DoesNotHaveIsFieldNameFlag();
        SkipFields = hasBeenVisitedBefore || Style.IsJson();

        if (SkipBody && hasBeenVisitedBefore
                     && !addType.IsValueType && TypeBeingBuilt.IsSpanFormattableCached()
                     && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            SkipBody   = false;
            SkipFields = Style.IsJson();
        }
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, moldInherited), formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange)
            VettedAppendBoolContent(value, formatString, resolvedFlags);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendBoolContent(value, formatString, resolvedFlags); }
            if (!actualType.IsValueType && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, moldInherited), formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendBoolContent(value, formatString, resolvedFlags);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendBoolContent(value, formatString, resolvedFlags); }
            if (!actualType.IsValueType && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, withMoldInherited)
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
            if (!actualType.IsValueType && !Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, withMoldInherited)
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
            if (!actualType.IsValueType && !Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
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
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, actualType);
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
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (SupportsMultipleFields && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }

        var moldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags();
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, moldInherited), formatString);

        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            var valueVisit = MoldGraphVisit;
            var valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, valueVisit, formatFlags);
            if (!CurrentWriteMethod.SupportsMultipleFields()
             && valueFormatAs.HasContentAllowComplexType()
             && Settings.InstanceTrackingIncludeSpanFormattableClasses)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
            else { resolvedFlags |= NoRevisitCheck; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange)
            VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags); }
            if (!actualType.IsValueType && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TFmt>(TFmt? value, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var moldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags();
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, moldInherited), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            var valueVisit = MoldGraphVisit;
            var valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, valueVisit, formatFlags);
            if (!CurrentWriteMethod.SupportsMultipleFields()
             && valueFormatAs.HasContentAllowComplexType()
             && Settings.InstanceTrackingIncludeSpanFormattableClasses)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
            }
            else { resolvedFlags |= NoRevisitCheck; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange)
            VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary appendSum;
            using (callContext) { appendSum = VettedAppendSpanFormattableContent(value, defaultValue, formatString, resolvedFlags); }
            if (!actualType.IsValueType && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
      , bool addEndDblQt = true)
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (!actualType.IsValueType && valueEqualsBuildingType)
        {
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
            else { resolvedFlags |= NoRevisitCheck; }
        }
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (SupportsMultipleFields && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
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
            if (valueEqualsBuildingType)
            {
                if (!CurrentWriteMethod.SupportsMultipleFields() && appendSum.WrittenAs.SupportsMultipleFields())
                {
                    Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(MoldGraphVisit.RegistryId, appendSum.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder); }
            } 
            else 
            {
                if (!CurrentWriteMethod.SupportsMultipleFields() &&  !appendSum.WrittenAs.SupportsMultipleFields() && appendSum.VisitNumber >= 0)
                    Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
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
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (!actualType.IsValueType && valueEqualsBuildingType)
        {
            var valueVisit = MoldGraphVisit;
            var valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, valueVisit, formatFlags);
            if (!CurrentWriteMethod.SupportsMultipleFields()
             && valueFormatAs.HasContentAllowComplexType()
             && Settings.InstanceTrackingIncludeSpanFormattableClasses)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
            else { resolvedFlags |= NoRevisitCheck; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
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
            if (valueEqualsBuildingType)
            {
                if (!CurrentWriteMethod.SupportsMultipleFields() && appendSum.WrittenAs.SupportsMultipleFields())
                {
                    Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(MoldGraphVisit.RegistryId, appendSum.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder); }
            } 
            else 
            {
                if (!CurrentWriteMethod.SupportsMultipleFields() &&  !appendSum.WrittenAs.SupportsMultipleFields() && appendSum.VisitNumber >= 0)
                    Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
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
                var writtenAs = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags)
                    : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, actualType);
            }
            var writtenAsNull = AppendNull(formatString, formatFlags);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAsNull, actualType);
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var result = this.AppendFormattedOrNull(value, formatString, formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return result;
    }

    public TToContentMold FieldValueOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var moldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, moldInherited)
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
            if (!actualType.IsValueType && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TFmtStruct>(TFmtStruct? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var moldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, moldInherited)
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
            if (!actualType.IsValueType && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
      , bool addEndDblQt = true) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
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
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
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
            if (!actualType.IsValueType && !Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSum.VisitNumber >= 0)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, appendSum.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
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
                var writtenAsDefault = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags)
                    : (formatFlags.HasAsValueContentFlag()
                        ? StyleFormatter.FormatFallbackFieldContents<TFmtStruct>(this, defaultValue, 0, formatString, formatFlags: formatFlags)
                        : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags));
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAsDefault, actualType);
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, actualType);
            }
            var writtenAsNull = AppendNull(formatString, formatFlags);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAsNull, actualType);
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var writtenAs = formatFlags.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(this, value, formatString, formatFlags)
            : StyleFormatter.FormatFieldContents(this, value, formatString, formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
    }

    public TToContentMold FieldValueOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsValueContent;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange) { VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags); }
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags); }

            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && result.WrittenAs.SupportsMultipleFields()
                 && (Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0 || !valueEqualsBuildingType))
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
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
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsValueContent;
        var callContext             = Master.ResolveContextForCallerFlags(resolvedFlags);
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        if (!callContext.HasFormatChange) { VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags); }
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && result.WrittenAs.SupportsMultipleFields()
                 && (Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0 || !valueEqualsBuildingType))
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
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
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing) | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsStringContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
            else { resolvedFlags |= NoRevisitCheck; }
        }
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        if (!callContext.HasFormatChange)
        {
            VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
        else
        {
            AppendSummary result;
            using (callContext)
            {
                result = VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                        , addEndDblQt);
            }
            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
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
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = ((formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags()) & ~(SuppressOpening | SuppressClosing)) | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
            else { resolvedFlags |= NoRevisitCheck; }
        }
        if (!callContext.HasFormatChange)
            VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext)
            {
                result = VettedAppendCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                        , addEndDblQt);
            }

            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public AppendSummary VettedAppendCloakedBearerContent<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string? defaultValue = null, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        AppendSummary result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAsFlags = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, value, palantírReveal, formatString, formatFlags).WrittenAs
                    : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags);
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                AppendNull("", formatFlags);
                writtenAsFlags = WrittenAsFlags.AsNull;
            }
            result =
                new AppendSummary(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                , writtenAsFlags, -1, typeof(TCloaked?));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            result = formatFlags.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(this, value, palantírReveal, formatString, formatFlags)
                : StyleFormatter.FormatFieldContents(this, value, palantírReveal, formatString, formatFlags);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldValueOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext)
            {
                result = VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags);
            }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext)
            {
                result = VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags);
            }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? defaultValue = null, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value
                               , StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString
                                                                                    , formatFlags | AsStringContent), formatString)
                          | AsStringContent;


        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.FormatFieldName(this, nonJsonfieldName);
            Sf.AppendFieldValueSeparator();
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
        {
            VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                         , addEndDblQt);
        }
        else
        {
            AppendSummary result;
            using (callContext)
            {
                result = VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                                                      , addStartDblQt, addEndDblQt);
            }
            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value
                               , StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags | AsStringContent)
                               , formatString)
                          | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt
                                                         , addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext)
            {
                result = VettedAppendNullableStructCloakedBearerContent(value, palantírReveal, defaultValue, formatString, resolvedFlags
                                                                      , addStartDblQt, addEndDblQt);
            }
            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public AppendSummary VettedAppendNullableStructCloakedBearerContent<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? defaultValue = null, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (!formatFlags.HasNullBecomesEmptyFlag())
                {
                    writtenAsFlags = formatFlags.HasIsFieldNameFlag()
                        ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags)
                        : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags);
                }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                AppendNull("", formatFlags);
                writtenAsFlags = WrittenAsFlags.AsNull;
            }
            WrittenAsFlags |= WrittenAsFlags.AsString | writtenAsFlags;
            return new AppendSummary(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                   , writtenAsFlags, -1, typeof(TCloakedStruct?));
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var result = formatFlags.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(this, value.Value, palantírReveal, formatString, formatFlags)
            : StyleFormatter.FormatFieldContents(this, value.Value, palantírReveal, formatString, formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }

        AppendSummary result;
        if (!callContext.HasFormatChange)
        {
            result = VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString);
        }
        else
        {
            using (callContext) { result = VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString); }
        }
        if (result.VisitNumber >= 0)
        {
            var visit = MoldGraphVisit;
            if (!CurrentWriteMethod.SupportsMultipleFields()
             && !callContext.HasFormatChange
             && result.WrittenAs.SupportsMultipleFields()
             && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
            }
            else if (callContext.HasFormatChange)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TBearer>(TBearer value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var maybeComplex      = withMoldInherited & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsValueContent;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var           callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString);
        else
        {
            using (callContext) { result = VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString); }
        }
        if (result.VisitNumber >= 0)
        {
            var visit = MoldGraphVisit;
            if (!CurrentWriteMethod.SupportsMultipleFields()
             && !callContext.HasFormatChange
             && result.WrittenAs.SupportsMultipleFields()
             && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
            }
            else if (callContext.HasFormatChange)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var maybeComplex = ((formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags()) & ~(SuppressOpening | SuppressClosing)) | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
            else { resolvedFlags |= NoRevisitCheck; }
        }
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.PreviousContextOrThis.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else if (callContext.HasFormatChange)
                {
                    Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
                }
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
        var maybeComplex = ((formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags()) & ~(SuppressOpening | SuppressClosing)) | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsStringContent;

        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
        }

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendStringBearerContent(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else if (callContext.HasFormatChange)
                {
                    Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
                }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public AppendSummary VettedAppendStringBearerContent<TBearer>(TBearer value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (!formatFlags.HasNullBecomesEmptyFlag())
                {
                    writtenAsFlags = formatFlags.HasIsFieldNameFlag()
                        ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString
                                                       , formatFlags: formatFlags)
                        : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString
                                                           , formatFlags: formatFlags);
                }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                AppendNull("", formatFlags);
                writtenAsFlags = WrittenAsFlags.AsNull;
            }
            WrittenAsFlags |= WrittenAsFlags.AsString | writtenAsFlags;
            return new AppendSummary(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                   , writtenAsFlags, -1, typeof(TBearer));
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var result = formatFlags.HasIsFieldNameFlag()
            ? StyleFormatter.FormatBearerFieldName(this, value, formatString, formatFlags)
            : StyleFormatter.FormatBearerFieldContents(this, value, formatString, formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldValueOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString);
        else
        {
            using (callContext) { result = VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString); }
        }
        var visit = MoldGraphVisit;
        if (!CurrentWriteMethod.SupportsMultipleFields()
         && !callContext.HasFormatChange
         && !result.WrittenAs.SupportsMultipleFields()
         && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
        {
            Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
            Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
        }
        else if (callContext.HasFormatChange)
        {
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TBearerStruct>(TBearerStruct? value
      , string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString);
        else
        {
            using (callContext) { result = VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString); }
        }
        var visit = MoldGraphVisit;
        if (!CurrentWriteMethod.SupportsMultipleFields()
         && !callContext.HasFormatChange
         && !result.WrittenAs.SupportsMultipleFields()
         && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
        {
            Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
            Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
        }
        else if (callContext.HasFormatChange)
        {
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
      , TBearerStruct? value, string? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var maybeComplex = ((formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags()) & ~(SuppressOpening | SuppressClosing)) | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            var visit = MoldGraphVisit;
            if (!CurrentWriteMethod.SupportsMultipleFields()
             && !callContext.HasFormatChange
             && !result.WrittenAs.SupportsMultipleFields()
             && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
            }
            else if (callContext.HasFormatChange)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TBearerStruct>(TBearerStruct? value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = ((formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags()) & ~(SuppressOpening | SuppressClosing)) | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;

        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        AppendSummary result;
        if (!callContext.HasFormatChange)
            result = VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                result = VettedAppendNullableStructStringBearerContent(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            var visit = MoldGraphVisit;
            if (!CurrentWriteMethod.SupportsMultipleFields()
             && !callContext.HasFormatChange
             && !result.WrittenAs.SupportsMultipleFields()
             && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
            }
            else if (callContext.HasFormatChange)
            {
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public AppendSummary VettedAppendNullableStructStringBearerContent<TBearerStruct>(TBearerStruct? value, string? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (defaultValue != null)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (!formatFlags.HasNullBecomesEmptyFlag())
                {
                    writtenAsFlags = formatFlags.HasIsFieldNameFlag()
                        ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString
                                                       , formatFlags: formatFlags)
                        : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags);
                }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                AppendNull("", formatFlags);
                writtenAsFlags = WrittenAsFlags.AsNull;
            }
            WrittenAsFlags |= WrittenAsFlags.AsString | writtenAsFlags;
            return new AppendSummary(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                   , writtenAsFlags, -1, typeof(TBearerStruct));
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var result = formatFlags.HasIsFieldNameFlag()
            ? StyleFormatter.FormatBearerFieldName(this, value.Value, formatString, formatFlags)
            : StyleFormatter.FormatBearerFieldContents(this, value.Value, formatString, formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, "Span", StyleFormatter.ResolveContentAsValueFormattingFlags("Span", "", formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, "Span", StyleFormatter.ResolveContentAsValueFormattingFlags("Span", "", formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags);
        else
        {
            using (callContext) { VettedAppendCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags); }
        }
        return StyleTypeBuilder.TransitionToNextMold();
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString, withMoldInherited), formatString);
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString, withMoldInherited)
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
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public WrittenAsFlags VettedAppendCharSpanContent(Span<char> value, ReadOnlySpan<char> defaultValue, bool isEmptyNull, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        WrittenAsFlags writtenAs;
        if (value.Length == 0)
        {
            if (defaultValue.Length > 0 || !isEmptyNull)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAs = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags)
                    : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return writtenAs;
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return WrittenAsFlags.Empty;
            }
            return AppendNull(formatString, formatFlags);
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

        writtenAs = formatFlags.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(this, value, 0, formatString, formatFlags: formatFlags)
            : StyleFormatter.FormatFieldContents(this, value, 0, formatString, formatFlags: formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan"
           , StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, withMoldInherited)
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan"
           , StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, withMoldInherited)
           , formatString) | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (!callContext.HasFormatChange)
            VettedAppendReadOnlyCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags);
        else
        {
            using (callContext) { VettedAppendReadOnlyCharSpanContent(value, fallbackValue, emptyIsNull, formatString, resolvedFlags); }
        }

        return StyleTypeBuilder.TransitionToNextMold();
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan"
           , StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, withMoldInherited)
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan"
           , StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, withMoldInherited)
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

        return StyleTypeBuilder.TransitionToNextMold();
    }

    public WrittenAsFlags VettedAppendReadOnlyCharSpanContent(ReadOnlySpan<char> value, ReadOnlySpan<char> defaultValue, bool isEmptyNull
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        WrittenAsFlags writtenAs;
        if (value.Length == 0)
        {
            if (defaultValue.Length > 0 || !isEmptyNull)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAs = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, value, 0, formatString, formatFlags: formatFlags)
                    : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return writtenAs;
            }
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return WrittenAsFlags.Empty;
            }
            return AppendNull(formatString, formatFlags);
        }

        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        writtenAs = formatFlags.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(this, value, 0, formatString, formatFlags: formatFlags)
            : StyleFormatter.FormatFieldContents(this, value, 0, formatString, formatFlags: formatFlags);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return writtenAs;
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        if (!callContext.HasFormatChange)
            VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
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
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendStringContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public AppendSummary VettedAppendStringContent(string? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var startAt = Sb.Length;

        WrittenAsFlags writtenAs;
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAs = formatFlags.HasIsFieldNameFlag()
                 ? StyleFormatter.FormatFieldName(this, value, capStart, formatString, capLength , formatFlags: formatFlags)
                  : StyleFormatter.FormatFieldContents(this, value, capStart, formatString, capLength, formatFlags: formatFlags);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(string));
            }
        }
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags); }
            else { StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(string));
        }
        if (value != null && formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, "", 0, formatString, formatFlags: formatFlags); }
                else { StyleFormatter.FormatFieldContents(this, "", 0, formatString, formatFlags: formatFlags); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(string));
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) 
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(string));
        writtenAs = AppendNull(formatString, formatFlags);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(string));
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(char[]? value, int startIndex, int length, string? defaultValue = null
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        if (!callContext.HasFormatChange)
            VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
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
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCharArrayContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public AppendSummary VettedAppendCharArrayContent(char[]? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var startAt = Sb.Length;

        WrittenAsFlags writtenAs;
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAs = formatFlags.HasIsFieldNameFlag()
                  ? StyleFormatter.FormatFieldName(this, value, capStart, formatString, capLength , formatFlags: formatFlags)
                  : StyleFormatter.FormatFieldContents(this, value, capStart, formatString, capLength, formatFlags: formatFlags);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(char[]));
            }
        }
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags); }
            else { StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(char[]));
        }
        if (value != null && formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, "", 0, formatString, formatFlags: formatFlags); }
                else { StyleFormatter.FormatFieldContents(this, "", 0, formatString, formatFlags: formatFlags); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(char[]));
            }
        }
        if (formatFlags.HasNullBecomesEmptyFlag()) 
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(char[]));
        writtenAs = AppendNull(formatString, formatFlags);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(char[]));
    }

    public TToContentMold FieldValueOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length, string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCharSeq>(TCharSeq value, int startIndex, int length, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length, string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        
        if (!callContext.HasFormatChange)
            VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
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
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        
        if (!callContext.HasFormatChange)
            VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendCharSequenceContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public AppendSummary VettedAppendCharSequenceContent<TCharSeq>(TCharSeq value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        var startAt    = Sb.Length;
        var actualType = value?.GetType() ?? typeof(TCharSeq);

        WrittenAsFlags writtenAs;
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAs = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, value, capStart, formatString, capLength , formatFlags: formatFlags)
                    : StyleFormatter.FormatFieldContents(this, value, capStart, formatString, capLength, formatFlags: formatFlags); 
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
            }
        }
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags); }
            else { StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, actualType);
        }
        if (value != null && formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, "", 0, formatString, formatFlags: formatFlags); }
                else { StyleFormatter.FormatFieldContents(this, "", 0, formatString, formatFlags: formatFlags); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, actualType);
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) 
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, actualType);
        writtenAs = AppendNull(formatString, formatFlags);

        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        
        if (!callContext.HasFormatChange)
            VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(StringBuilder? value, int startIndex, int length, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        
        if (!callContext.HasFormatChange)
            VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        
        if (!callContext.HasFormatChange)
            VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
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
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        
        if (!callContext.HasFormatChange)
            VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendStringBuilderContent(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                if (result.VisitNumber >= 0)
                {
                    var visit = MoldGraphVisit;
                    if (!CurrentWriteMethod.SupportsMultipleFields()
                     && !callContext.HasFormatChange
                     && result.WrittenAs.SupportsMultipleFields()
                     && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                    {
                        Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                        Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                    }
                    else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
                }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public AppendSummary VettedAppendStringBuilderContent(StringBuilder? value, int startIndex, int length
      , string? defaultValue = null, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var startAt = Sb.Length;

        WrittenAsFlags writtenAs;
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                writtenAs = formatFlags.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(this, value, capStart, formatString, capLength , formatFlags: formatFlags)
                    :  StyleFormatter.FormatFieldContents(this, value, capStart, formatString, capLength, formatFlags: formatFlags); 
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(StringBuilder));
            }
        }
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags); }
            else { StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(StringBuilder));
        }
        if (value != null && formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, "", 0, formatString, formatFlags: formatFlags); }
                else { StyleFormatter.FormatFieldContents(this, "", 0, formatString, formatFlags: formatFlags); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(StringBuilder));
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag())
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, typeof(StringBuilder));
        writtenAs = AppendNull(formatString, formatFlags);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, typeof(StringBuilder));
    }

    public TToContentMold ValueMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        
        if (!callContext.HasFormatChange)
            VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueMatchWithDefaultJoin<TAny>(TAny? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsValueContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsValueContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsValueContent;
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        
        if (!callContext.HasFormatChange)
            VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold StringMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(this, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        
        if (!callContext.HasFormatChange)
            VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringMatchWithDefaultJoin<TAny>(TAny? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !BuildingInstanceEquals(value))
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, withMoldInherited)
                               , formatString)
                          | AsStringContent;
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        resolvedFlags |= resolvedFlags.HasIsFieldNameFlag() ? DisableFieldNameDelimiting : DisableAutoDelimiting;
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        
        if (!callContext.HasFormatChange)
            VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            AppendSummary result;
            using (callContext) { result = VettedAppendMatchContent(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
            if (result.VisitNumber >= 0)
            {
                var visit = MoldGraphVisit;
                if (!CurrentWriteMethod.SupportsMultipleFields()
                 && !callContext.HasFormatChange
                 && result.WrittenAs.SupportsMultipleFields()
                 && Master.InstanceIdAtVisit(visit.RegistryId, visit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(visit.RegistryId, visit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(visit.RegistryId, result.VisitNumber, NoRevisitCheck);
                }
                else { Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder); }
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public AppendSummary VettedAppendMatchContent<TAny>(TAny? value, string? defaultValue = null
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var result = this.AppendMatchFormattedOrNull(value, formatString, formatFlags);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return result;
        }
        var startAt    = Sb.Length;
        var actualType = value?.GetType() ?? typeof(TAny);
        
        WrittenAsFlags writtenAs;
        if (defaultValue != null)
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            writtenAs = formatFlags.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(this, defaultValue, 0, formatString, formatFlags: formatFlags)
                : (formatFlags.HasAsValueContentFlag()
                  ? StyleFormatter.FormatFallbackFieldContents<TAny>(this, defaultValue, 0, formatString, formatFlags: formatFlags)
                  : StyleFormatter.FormatFieldContents(this, defaultValue, 0, formatString, formatFlags: formatFlags));
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
        }
        if (formatString.Length > 0)
        {
            var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
            if (prefixSuffixLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                if (formatFlags.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(this, "", 0, formatString, formatFlags: formatFlags); }
                else { StyleFormatter.FormatFieldContents(this, "", 0, formatString, formatFlags: formatFlags); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, actualType);
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) 
            return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, WrittenAsFlags.Empty, actualType);
        writtenAs = AppendNull(formatString, formatFlags);
        return Master.UnregisteredAppend(TypeBeingBuilt, startAt, Sb.Length, writtenAs, actualType);
    }

    protected WrittenAsFlags AppendNull(string formatString, FormatFlags formatFlags) =>
        StyleFormatter.AppendFormattedNull(Sb, formatString, formatFlags);

    public TToContentMold ConditionalValueTypeSuffix()
    {
        if (SupportsMultipleFields) { return StyleTypeBuilder.TransitionToNextMold(); }
        Sf.Gb.Complete(Sf.Gb.CurrentSectionRanges.StartedWithFormatFlags);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public override bool HasSkipBody(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => SkipBody;


    public new TToContentMold WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)

    {
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public override ITypeMolderDieCast CopyFrom(ITypeMolderDieCast? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        base.CopyFrom(source, copyMergeFlags);
        SkipFields = SkipFields | SkipBody;
        if (source is ContentTypeDieCast<TContentMold, TToContentMold> valueTypeDieCast) { CurrentWriteMethod = valueTypeDieCast.CurrentWriteMethod; }

        return this;
    }
}
