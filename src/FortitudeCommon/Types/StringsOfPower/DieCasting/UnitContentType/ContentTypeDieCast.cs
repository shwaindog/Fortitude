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

    private int countNextSkipFieldIsSkipBody;

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
                : VisitResult.VisitNotChecked;
        }
        else
        {
            MoldGraphVisit = VisitResult.NoVisitCheckRequired;
        }

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

        GraphNodeVisit? newVisit = null;
        if (!TypeBeingBuilt.IsValueType)
        {
            var reg = Master.ActiveGraphRegistry;

            var fmtState = new FormattingState(reg.CurrentDepth + 1, reg.RemainingDepth - 1, CreateMoldFormatFlags
                                             , Settings.IndentSize, Sf, Sf.ContentEncoder, Sf.LayoutEncoder);

            newVisit =
                new GraphNodeVisit(reg.RegistryId, reg.Count, reg.CurrentGraphNodeIndex
                                 , InstanceOrContainer?.GetType() ?? TypeBeingBuilt, TypeBeingVisitedAs
                                 , this, newWriteMethod, InstanceOrContainer
                                 , Sb.Length, IndentLevel, Master.CallerContext, fmtState, CreateMoldFormatFlags
                                 , MoldGraphVisit.LastRevisitCount + 1);
        }

        StyleTypeBuilder.StartTypeOpening();

        if (!TypeBeingBuilt.IsValueType &&  !MoldGraphVisit.NoVisitCheckDone && newVisit != null)
        {
            Master.ActiveGraphRegistry.Add(newVisit.Value.SetBufferFirstFieldStart(Master.WriteBuffer.Length, IndentLevel));
        }

        StyleTypeBuilder.FinishTypeOpening();

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
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags, formatString);
        var callContext   = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags, formatString);
        var callContext   = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
        using (callContext)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            return VettedJoinValue(value, formatString, resolvedFlags);
        }
    }

    public TToContentMold VettedJoinValue(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited); }

        WrittenAsFlags = WrittenAsFlags.AsValue;
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (SupportsMultipleFields && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);

        FormatFlags valueFormatAs;
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            var valueVisit = MoldGraphVisit;
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
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
        else
        {
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, VisitResult.VisitNotChecked, formatFlags);
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange)
        {
            VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
            return ConditionalValueTypeSuffix();
        }
        if (actualType.IsValueType || valueFormatAs.HasContentAllowComplexType())
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        }
        else
        {
            callContext.Dispose();
            VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TFmt>(TFmt? value
      , ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        FormatFlags valueFormatAs;
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            var valueVisit = MoldGraphVisit;
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
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
        else
        {
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, VisitResult.VisitNotChecked, formatFlags);
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags);

        if (actualType.IsValueType || valueFormatAs.HasContentAllowComplexType())
        {
            using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        }
        else
        {
            callContext.Dispose();
            return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
        }
    }

    public TToContentMold VettedJoinWithDefaultValue<TFmt>(TFmt? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFallbackFieldContents<TFmt>(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            return StyleTypeBuilder.TransitionToNextMold();
        }

        this.AppendFormattedOrNull(value, formatString, formatFlags);
        WrittenAsFlags = WrittenAsFlags.AsValue;
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldFmtValueOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
            this.FieldNameJoin(nonJsonfieldName);
        else if (SupportsMultipleFields && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);

        FormatFlags valueFormatAs;
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            var valueVisit = MoldGraphVisit;
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
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
        else
        {
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, VisitResult.VisitNotChecked, formatFlags);
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (!callContext.HasFormatChange)
        {
            VettedJoinValue(value, formatString, resolvedFlags);
            return ConditionalValueTypeSuffix();
        }
        if (actualType.IsValueType || valueFormatAs.HasContentAllowComplexType())
        {
            using (callContext) { VettedJoinValue(value, formatString, resolvedFlags); }
        }
        else
        {
            callContext.Dispose();
            VettedJoinValue(value, formatString, resolvedFlags);
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var valueVisit = VisitResult.VisitNotChecked;
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            valueVisit = MoldGraphVisit;
            if (Settings.InstanceTrackingIncludeSpanFormattableClasses)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
            }
            else { resolvedFlags |= NoRevisitCheck; }
        }

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
        using (callContext)
        {
            if (!actualType.IsValueType)
            {
                var valueFormatAs = Sf.GetFormatterContentHandlingFlags
                    (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, valueVisit, formatFlags);
                if (valueFormatAs.HasContentAllowComplexType())
                    Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            }
            return VettedJoinValue(value, formatString, resolvedFlags);
        }
    }

    public TToContentMold VettedJoinValue<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        this.AppendFormattedOrNull(value, formatString, withMoldInherited);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TFmtStruct>(TFmtStruct? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
        using (callContext)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
        }
    }

    public TToContentMold VettedJoinWithDefaultValue<TFmtStruct>(TFmtStruct? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFallbackFieldContents<TFmtStruct>(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldFmtValueOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
        using (callContext)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            return VettedJoinValue(value, formatString, resolvedFlags);
        }
    }

    public TToContentMold VettedJoinValue<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TCloaked, TRevealBase>(TCloaked? value, PalantírReveal<TRevealBase> palantírReveal
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, palantírReveal, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValue<TCloaked, TRevealBase>(TCloaked? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull("", formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        var stateExtractResult = withMoldInherited.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited)
            : StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited);
        if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
        {
            if (Master.InstanceIdAtVisit(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(MoldGraphVisit.RegistryId, stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, palantírReveal, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValue<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull("", formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Master, value.Value, palantírReveal, formatString, withMoldInherited);
        }
        else { StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinWithDefaultValue<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var stateExtractResult = withMoldInherited.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited)
            : StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited);
        if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
        {
            if (Master.InstanceIdAtVisit(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(MoldGraphVisit.RegistryId, stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, resolvedFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, resolvedFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, resolvedFlags, formatString);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, resolvedFlags, formatString); }
    }

    public TToContentMold VettedJoinWithDefaultValue<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TCloakedStruct : struct
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Master, value.Value, palantírReveal, formatString, withMoldInherited);
        }
        else { StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TBearer>(TBearer value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
    }

    public TToContentMold VettedJoinWithDefaultValue<TBearer>(TBearer value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited); }
            else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var stateExtractResult = withMoldInherited.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited)
            : StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited);
        if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
        {
            if (Master.InstanceIdAtVisit(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(MoldGraphVisit.RegistryId, stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TBearerStruct>(TBearerStruct? value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
    }

    public TToContentMold VettedJoinWithDefaultValue<TBearerStruct>(TBearerStruct? value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var stateExtractResult = withMoldInherited.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(Master, value.Value, formatString, withMoldInherited)
            : StyleFormatter.FormatFieldContents(Master, value.Value, formatString, withMoldInherited);
        if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
        {
            if (Master.InstanceIdAtVisit(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(MoldGraphVisit.RegistryId, stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, resolvedFlags, formatString); }
        }
        else { VettedJoinValue(value, resolvedFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TBearer>(TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, resolvedFlags, formatString);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, resolvedFlags, formatString); }
    }

    public TToContentMold VettedJoinValue<TBearer>(TBearer value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string formatString = "")
        where TBearer : IStringBearer?
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        var stateExtractResult = withMoldInherited.HasIsFieldNameFlag()
            ? StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited)
            : StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited);
        if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
        {
            if (Master.InstanceIdAtVisit(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(MoldGraphVisit.RegistryId, stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, resolvedFlags, formatString); }
        }
        else { VettedJoinValue(value, resolvedFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TBearerStruct>(TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, resolvedFlags, formatString);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, resolvedFlags, formatString); }
    }

    public TToContentMold VettedJoinValue<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Master, value.Value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Master, value.Value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , ReadOnlySpan<char> fallbackValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsValueFormattingFlags("Span", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsValueFormattingFlags("Span", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinWithDefaultValue(Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, fallbackValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFieldContents(Sb, fallbackValue, 0, formatString, formatFlags: withMoldInherited); }
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , ReadOnlySpan<char> fallbackValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinWithDefaultValue(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, fallbackValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFieldContents(Sb, fallbackValue, 0, formatString, formatFlags: withMoldInherited); }
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValue(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValueWithDefault(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength
                                                     , formatFlags: withMoldInherited);
                }
                return StyleTypeBuilder.TransitionToNextMold();
            }
        }
        if (value == null && withMoldInherited.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
        }
        else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValue(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            var capStart          = Math.Clamp(startIndex, 0, value.Length);
            var capLength         = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, withMoldInherited); }
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        return StyleTypeBuilder.TransitionToNextMold();
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(char[]? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValueWithDefault(char[]? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString
                                                     , capLength, formatFlags: withMoldInherited);
                }
                return StyleTypeBuilder.TransitionToNextMold();
            }
        }
        if (value == null && withMoldInherited.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
        }
        else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValue(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, withMoldInherited); }
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, Array.Empty<char>(), 0, formatString, formatFlags: withMoldInherited);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, Array.Empty<char>(), 0, formatString, formatFlags: withMoldInherited); }
                        return StyleTypeBuilder.TransitionToNextMold();
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length, string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCharSeq>(TCharSeq value, int startIndex, int length, string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinWithDefaultValue<TCharSeq>(TCharSeq value, int startIndex, int length
      , string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
                else
                {
                    StyleFormatter.FormatFieldContents
                        (Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
            }
            else
            {
                if (defaultValue.IsEmpty() && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
                }
                else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            }
        }
        else
        {
            if (defaultValue.IsEmpty() && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex
      , int length, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValue<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
                else
                {
                    StyleFormatter.FormatFieldContents
                        (Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        return StyleTypeBuilder.TransitionToNextMold();
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(StringBuilder? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinWithDefaultValue(StringBuilder? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength
                                                     , formatFlags: withMoldInherited);
                }
            }
            else
            {
                if (defaultValue.IsEmpty() && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
                }
                else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            }
        }
        else
        {
            if (defaultValue.IsEmpty() && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
            }
            else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValue(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength
                                                     , formatFlags: withMoldInherited);
                }
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                         , formatFlags: withMoldInherited);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        return StyleTypeBuilder.TransitionToNextMold();
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold ValueMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedValueMatchJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedValueMatchJoinValue(value, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueMatchJoin<TAny>(TAny? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedValueMatchJoinValue(value, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedValueMatchJoinValue(value, formatString, resolvedFlags); }
    }

    public TToContentMold VettedValueMatchJoinValue<TAny>(TAny? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        this.AppendMatchFormattedOrNull(value, formatString, withMoldInherited);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold ValueMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags)
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags)) { return WasSkipped(actualType, nonJsonfieldName, formatFlags); }

        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
    }

    public TToContentMold VettedJoinValueMatchWithDefaultValue<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null) { this.AppendMatchFormattedOrNull(value, formatString, withMoldInherited); }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited);
                }
                else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
            }
            else { StyleFormatter.FormatFallbackFieldContents<TAny>(Sb, defaultValue, 0, formatString, formatFlags: formatFlags); }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(bool?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(bool?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, value, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited); }

            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
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

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        FormatFlags valueFormatAs;
        if (!actualType.IsValueType && valueEqualsBuildingType)
        {
            var valueVisit = MoldGraphVisit;
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
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
        else
        {
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, VisitResult.VisitNotChecked, formatFlags);
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (SupportsMultipleFields && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }
        if (!callContext.HasFormatChange)
        {
            VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            return ConditionalValueTypeSuffix();
        }
        if (actualType.IsValueType || valueFormatAs.HasContentAllowComplexType())
        {
            if (!actualType.IsValueType)
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else
        {
            callContext.Dispose();
            VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TFmt>(TFmt value, string defaultValue = "", string formatString = ""
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        FormatFlags valueFormatAs;
        if (!actualType.IsValueType && valueEqualsBuildingType)
        {
            var valueVisit = MoldGraphVisit;
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
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
        else
        {
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, VisitResult.VisitNotChecked, formatFlags);
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (valueFormatAs.HasContentAllowComplexType())
        {
            if (!actualType.IsValueType)
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        callContext.Dispose();
        return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
    }

    public TToContentMold VettedJoinStringWithDefault<TFmt>(TFmt value
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableAutoDelimiting);
                }
            }
        }
        else { this.AppendFormattedOrNull(value, formatString, formatFlags); }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
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

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        FormatFlags valueFormatAs;
        if (!actualType.IsValueType && valueEqualsBuildingType)
        {
            var valueVisit = MoldGraphVisit;
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
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
        else
        {
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, VisitResult.VisitNotChecked, formatFlags);
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        else if (SupportsMultipleFields && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            fieldNameFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }

        if (!callContext.HasFormatChange)
        {
            VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            return ConditionalValueTypeSuffix();
        }
        if (actualType.IsValueType || valueFormatAs.HasContentAllowComplexType())
        {
            if (!actualType.IsValueType)
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else
        {
            callContext.Dispose();
            VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TFmt>(TFmt value, string formatString = ""
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        FormatFlags valueFormatAs;
        if (!actualType.IsValueType && valueEqualsBuildingType)
        {
            var valueVisit = MoldGraphVisit;
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
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
        else
        {
            valueFormatAs = Sf.GetFormatterContentHandlingFlags
                (Master, value, value?.GetType() ?? typeof(TFmt), CurrentWriteMethod, VisitResult.VisitNotChecked, formatFlags);
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (valueFormatAs.HasContentAllowComplexType())
        {
            if (!actualType.IsValueType)
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        callContext.Dispose();
        return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
    }

    public TToContentMold VettedJoinString<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        if (addStartDblQt && value != null) Sf.Gb.AppendParentContent(DblQt);
        this.AppendFormattedOrNull(value, formatString, formatFlags);
        if (addEndDblQt && value != null) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TFmtStruct>(TFmtStruct? value, string defaultValue = ""
      , string formatString = ""
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinStringWithDefault<TFmtStruct>(TFmtStruct? value, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            }
        }
        else
        {
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited | DisableAutoDelimiting); }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TFmtStruct>(TFmtStruct? value, string formatString = ""
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
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { this.AppendMatchFormattedOrNull(value, formatString, withMoldInherited | DisableAutoDelimiting); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName
      , TCloaked value, PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string formatString = ""
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

        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
            else
            {
                resolvedFlags |= NoRevisitCheck;
            }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType) RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        
        StateExtractStringRange result;
        if (!callContext.HasFormatChange)
        {
            result = VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
        else
        {
            using (callContext)
            {
                result = VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if(callContext.HasFormatChange && !result.WrittenAs.SupportsMultipleFields()) 
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string formatString = ""
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
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex | AsStringContent), formatString)
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
            else
            {
                resolvedFlags |= NoRevisitCheck;
            }
        }
        StateExtractStringRange result;
        if (!callContext.HasFormatChange)
            result = VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else {
            using (callContext)
            {
                result = VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if(callContext.HasFormatChange && !result.WrittenAs.SupportsMultipleFields()) 
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
        }
        return ConditionalValueTypeSuffix();
    }

    public StateExtractStringRange VettedJoinStringWithDefault<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        StateExtractStringRange result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                writtenAsFlags = withMoldInherited.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting).WrittenAs
                    : StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); 
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            result =
                new StateExtractStringRange(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                          , writtenAsFlags, -1, typeof(TCloaked?));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            result = withMoldInherited.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting)
                : StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldStringRevealOrNullNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
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

        var maybeComplex = (formatFlags & ~(SuppressOpening | SuppressClosing)) | AsStringContent;
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, maybeComplex), formatString)
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
            else
            {
                resolvedFlags |= NoRevisitCheck;
            }
        }
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        StateExtractStringRange result;
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                result = VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { result = VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if(callContext.HasFormatChange && !result.WrittenAs.SupportsMultipleFields()) 
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
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
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, maybeComplex | AsStringContent), formatString)
                          | AsStringContent;
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
            else
            {
                resolvedFlags |= NoRevisitCheck;
            }
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, resolvedFlags);

        StateExtractStringRange result;
        if (!callContext.HasFormatChange) result = VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else {using (callContext) { result = VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }}

        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if(callContext.HasFormatChange && !result.WrittenAs.SupportsMultipleFields()) 
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public StateExtractStringRange VettedJoinString<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        StateExtractStringRange result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (formatFlags.HasNullBecomesEmptyFlag())
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
                new StateExtractStringRange(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                          , writtenAsFlags, -1, typeof(TCloaked));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            result = withMoldInherited.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting)
                : StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string formatString = ""
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
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags | AsStringContent), formatString)
                          | AsStringContent;
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.FormatFieldName(Sb, nonJsonfieldName);
            Sf.AppendFieldValueSeparator();
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
        {
            VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
        else 
        {
            using (callContext) { VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = ""
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
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (!callContext.HasFormatChange)
            VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public StateExtractStringRange VettedJoinStringWithDefault<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        StateExtractStringRange result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                writtenAsFlags = withMoldInherited.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting)
                    : StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableAutoDelimiting);
            }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            result =
                new StateExtractStringRange(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                          , writtenAsFlags, -1, typeof(TCloakedStruct?));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            result = withMoldInherited.HasIsFieldNameFlag()
             ? StyleFormatter.FormatFieldName(Master, value.Value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting)
             : StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal, formatString, withMoldInherited); 
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldStringRevealOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true) where TCloakedStruct : struct
    {
        var actualType = typeof(TCloakedStruct?);
        ContentType = actualType;
        RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.FormatFieldName(Sb, nonJsonfieldName);
            Sf.AppendFieldValueSeparator();
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
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
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (!callContext.HasFormatChange)
            VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext) { VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public StateExtractStringRange VettedJoinString<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        StateExtractStringRange result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (formatFlags.HasNullBecomesEmptyFlag())
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
                new StateExtractStringRange(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                          , writtenAsFlags, -1, typeof(TCloakedStruct?));
            
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            result = withMoldInherited.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(Master, value.Value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting)
                : StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal, formatString, withMoldInherited); 
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
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

        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
            else
            {
                resolvedFlags |= NoRevisitCheck;
            }
        }
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.PreviousContextOrThis.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        StateExtractStringRange result;
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                result = VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
            }
        }
        else { result = VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }

        
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if(callContext.HasFormatChange && !result.WrittenAs.SupportsMultipleFields()) 
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
        }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TBearer>(TBearer value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
        }

        StateExtractStringRange result;
        if (!callContext.HasFormatChange)
        {
            result = VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        }
        else
        {
            using (callContext)
            {
                result = VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
            }
        }
        
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if(callContext.HasFormatChange && !result.WrittenAs.SupportsMultipleFields()) 
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public StateExtractStringRange VettedJoinStringWithDefault<TBearer>(TBearer value
      , string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        StateExtractStringRange result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                writtenAsFlags = withMoldInherited.HasIsFieldNameFlag()
                    ? StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting)
                    : StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableAutoDelimiting);
            }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            result =
                new StateExtractStringRange(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                          , writtenAsFlags, -1, typeof(TBearer));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            result = withMoldInherited.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited | DisableFieldNameDelimiting)
                : StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
      , TBearerStruct? value, string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
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

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.FormatFieldName(Sb, nonJsonfieldName);
            Sf.AppendFieldValueSeparator();
        }
        
        if (!callContext.HasFormatChange)
            VettedJoinStringWithDefault( value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags | AsStringContent);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags)
                               , formatString)
                          | AsStringContent;
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext        = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        
        if (!callContext.HasFormatChange)
            VettedJoinStringWithDefault( value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public StateExtractStringRange VettedJoinStringWithDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        StateExtractStringRange result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            result =
                new StateExtractStringRange(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                          , writtenAsFlags, -1, typeof(TBearerStruct));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            result = withMoldInherited.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(Master, value.Value, formatString, withMoldInherited | DisableFieldNameDelimiting)
                : StyleFormatter.FormatFieldContents(Master, value.Value, formatString, withMoldInherited);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
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

        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, maybeComplex), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
            else
            {
                resolvedFlags |= NoRevisitCheck;
            }
        }
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.PreviousContextOrThis.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        StateExtractStringRange result;
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                result = VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
            }
        }
        else { result = VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if(callContext.HasFormatChange && !result.WrittenAs.SupportsMultipleFields()) 
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TBearer>(TBearer value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking && !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, maybeComplex), formatString);

        var callContext        = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (valueEqualsBuildingType)
        {
            if (WroteTypeName) { resolvedFlags |= LogSuppressTypeNames; }
            if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
            {
                Master.RemoveVisitAt(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex);
            }
        }

        StateExtractStringRange result;
        if (!callContext.HasFormatChange) result = VettedJoinString( value, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else {using (callContext) { result = VettedJoinString( value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }}

        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            if(callContext.HasFormatChange && !result.WrittenAs.SupportsMultipleFields()) 
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, result.VisitNumber, Sf.ContentEncoder, Sf.LayoutEncoder);
        }
        return StyleTypeBuilder.TransitionToNextMold();
        
    }

    public StateExtractStringRange VettedJoinString<TBearer>(TBearer value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false) where TBearer : IStringBearer?
    {
        StateExtractStringRange result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (formatFlags.HasNullBecomesEmptyFlag())
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
                new StateExtractStringRange(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                          , writtenAsFlags, -1, typeof(TBearer));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            result = withMoldInherited.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited | DisableFieldNameDelimiting)
                : StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldStringRevealOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            Sf.FormatFieldName(Sb, nonJsonfieldName);
            Sf.AppendFieldValueSeparator();
        }
        
        if (!callContext.HasFormatChange)
            VettedJoinString( value, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext) { VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TBearerStruct>(TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(TBearerStruct?);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
                                (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString)
                          | AsStringContent;
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }

        var callContext        = Master.ResolveContextForCallerFlags(resolvedFlags);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        
        if (!callContext.HasFormatChange)
            VettedJoinString( value, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        else
        {
            using (callContext) { VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public StateExtractStringRange VettedJoinString<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null, bool addStartDblQt = false, bool addEndDblQt = false) where TBearerStruct : struct, IStringBearer
    {
        StateExtractStringRange result;
        if (value == null)
        {
            var startedAt = Sb.Length;

            WrittenAsFlags writtenAsFlags = WrittenAsFlags.Empty;
            if (formatFlags.HasNullBecomesEmptyFlag())
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
                new StateExtractStringRange(StyleTypeBuilder.GetType(), Master, new Range(startedAt, Sb.Length)
                                          , writtenAsFlags, -1, typeof(TBearerStruct));
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            result = withMoldInherited.HasIsFieldNameFlag()
                ? StyleFormatter.FormatFieldName(Master, value.Value, formatString, withMoldInherited | DisableFieldNameDelimiting)
                : StyleFormatter.FormatFieldContents(Master, value.Value, formatString, withMoldInherited);
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        WrittenAsFlags |= WrittenAsFlags.AsString | result.WrittenAs;
        return result;
    }

    public TToContentMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(Span<char>);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(Span<char>);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("Span", formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString(Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
      , bool addEndDblQt = true)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinStringWithDefault(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        if (value.Length == 0)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            }
        }
        else
        {
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking) RegisterBuildInstanceOnActiveRegistry("ReadOnlySpan", formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited); }

                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                        return StyleTypeBuilder.TransitionToNextMold();
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext)
            {
                VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(string);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
    }

    public TToContentMold VettedJoinStringWithDefault(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags); }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !BuildingInstanceEquals(value))
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                         , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, ((ReadOnlySpan<char>)""), 0, formatString, formatFlags: formatFlags); }
                        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                        return StyleTypeBuilder.TransitionToNextMold();
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext)
            {
                VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(char[]? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(char[]);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
    }

    public TToContentMold VettedJoinStringWithDefault(char[]? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags); }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length, string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext)
            {
                VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCharSeq>(TCharSeq value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
    }

    public TToContentMold VettedJoinStringWithDefault<TCharSeq>(TCharSeq value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
            }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrNullNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }

        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TCharSeq>(TCharSeq value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString<TCharSeq>(TCharSeq value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);


                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                        return StyleTypeBuilder.TransitionToNextMold();
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext)
            {
                VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(StringBuilder? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
    }

    public TToContentMold VettedJoinStringWithDefault(StringBuilder? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited); }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = typeof(StringBuilder);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !BuildingInstanceEquals(value))
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString
                                                     , capLength, formatFlags: withMoldInherited);
                }
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                        return StyleTypeBuilder.TransitionToNextMold();
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold StringMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinStringMatchJoin(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringMatchJoin(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringMatchJoin<TAny>(TAny value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!callContext.HasFormatChange) return VettedJoinStringMatchJoin(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinStringMatchJoin(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinStringMatchJoin<TAny>(TAny value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableAutoDelimiting);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

        this.AppendMatchFormattedOrNull(value, formatString, DisableAutoDelimiting | withMoldInherited | DisableFieldNameDelimiting);
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold StringMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);

        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }

        var fieldNameFormatter = Sf;

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0)
        {
            fieldNameFormatter.FormatFieldName(Sb, nonJsonfieldName);
            fieldNameFormatter.AppendFieldValueSeparator();
        }
        if (callContext.HasFormatChange)
        {
            if (CurrentWriteMethod.SupportsMultipleFields())
                Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
            using (callContext) { VettedJoinStringMatchWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringMatchWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        var valueEqualsBuildingType = BuildingInstanceEquals(value);
        if (!Settings.InstanceTrackingAllAsStringHaveLocalTracking || !BuildingInstanceEquals(value))
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking && valueEqualsBuildingType)
            RegisterBuildInstanceOnActiveRegistry(value, formatFlags);
        if (!callContext.HasFormatChange)
            return VettedJoinStringMatchWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        if (CurrentWriteMethod.SupportsMultipleFields())
            Master.UpdateVisitEncoders(MoldGraphVisit.RegistryId, MoldGraphVisit.CurrentVisitIndex, Sf.ContentEncoder, Sf.LayoutEncoder);
        using (callContext) { return VettedJoinStringMatchWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinStringMatchWithDefault<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value != null)
        {
            this.AppendMatchFormattedOrNull(value, formatString, DisableAutoDelimiting | withMoldInherited | DisableFieldNameDelimiting);
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, "", 0, formatString
                                                     , formatFlags: DisableAutoDelimiting | withMoldInherited);
                }
            }
            else
            {
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString
                                                     , formatFlags: DisableAutoDelimiting | withMoldInherited | DisableFieldNameDelimiting);
                }
            }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    protected void AppendNull(string formatString, FormatFlags formatFlags)
    {
        StyleFormatter.AppendFormattedNull(Sb, formatString, formatFlags);
    }

    public TToContentMold ConditionalValueTypeSuffix()
    {
        if (SupportsMultipleFields)
        {
            this.AddGoToNext();
            return StyleTypeBuilder.TransitionToNextMold();
        }
        Sf.Gb.Complete(Sf.Gb.CurrentSectionRanges.StartedWithFormatFlags);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public override bool HasSkipBody(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => SkipBody;

    public override bool HasSkipField(Type actualType, ReadOnlySpan<char> fieldName, FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) =>
        countNextSkipFieldIsSkipBody-- > 0
            ? HasSkipBody(actualType, fieldName, formatFlags)
            : base.HasSkipField(actualType, fieldName, formatFlags);

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
