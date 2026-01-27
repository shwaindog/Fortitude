// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentTypeDieCast<TContentMold, TToContentMold> : TypeMolderDieCast<TContentMold> 
    where TContentMold : ContentTypeMold<TContentMold, TToContentMold>
    where TToContentMold : ContentJoinTypeMold<TContentMold, TToContentMold>, IMigrateFrom<TContentMold, TToContentMold>, new()
{
    public const string DblQt = "\"";

    private bool ignoreSuppressSpanFormattable;
    private int  countNextSkipFieldIsSkipBody;

    protected Type? ContentType;

    public ContentTypeDieCast<TContentMold, TToContentMold> InitializeValueBuilderCompAccess
        (TContentMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WriteMethodType writeMethod)
    {
        var createFmtFlags = typeBuilderPortableState.CreateFormatFlags;
        writeMethod = createFmtFlags.DoesNotHaveContentAllowComplexType()
            ? writeMethod.ToNoFieldEquivalent()
            : writeMethod.ToMultiFieldEquivalent();

        Initialize(externalTypeBuilder, typeBuilderPortableState, writeMethod);

        if (SkipBody && MoldGraphVisit.IsARevisit
                     && !TypeBeingBuilt.IsValueType && TypeBeingBuilt.IsSpanFormattableCached()
                     && Settings.InstanceMarkingIncludeSpanFormattableContents) { ignoreSuppressSpanFormattable = true; }
        OnFinishedWithStringBuilder = FinishUsingStringBuilder;

        return this;
    }

    private void FinishUsingStringBuilder(IScopeDelimitedStringBuilder finishedBuilding)
    {
        if (Style.IsJson()) finishedBuilding.Append(DblQt);
        Sf.Gb.MarkContentEnd();
    }

    private Action<IScopeDelimitedStringBuilder>? OnFinishedWithStringBuilder { get; set; }

    public bool IsLog => Style.IsLog();

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()
    {
        Sf.Gb.StartNextContentSeparatorPaddingSequence(Sb, DefaultCallerTypeFlags, true);
        if (Style.IsJson()) Sb.Append(DblQt);
        var scopedSb = (IScopeDelimitedStringBuilder)Sb;
        scopedSb.OnScopeEndedAction = OnFinishedWithStringBuilder;
        return scopedSb;
    }

    public TToContentMold FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(bool?);
        ContentType    = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags, formatString);
        var callContext   = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = value?.GetType() ?? typeof(bool?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags, formatString);
        var callContext   = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
        using (callContext) { return VettedJoinValue(value, formatString, resolvedFlags); }
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
        var actualType    = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags) && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        else if (SkipBody && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);

        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext   = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TFmt>(TFmt? value
      , ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext   = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
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
        var actualType  = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags) && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        else if (SkipBody && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);

        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            resolvedFlags |= NoRevisitCheck;
        }
        
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
        using (callContext) { return VettedJoinValue(value, formatString, resolvedFlags); }
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
        var actualType  = typeof(TFmtStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TFmtStruct>(TFmtStruct? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
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
        var actualType  = typeof(TFmtStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);

        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
        using (callContext) { return VettedJoinValue(value, formatString, resolvedFlags); }
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
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext   = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
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
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, palantírReveal, formatString, resolvedFlags);
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
            if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { JoinValueJoin(value, palantírReveal, formatString, resolvedFlags); }
        }
        else { JoinValueJoin(value, palantírReveal, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, palantírReveal, formatString, resolvedFlags);
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
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
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
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
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
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags);
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
            if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
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
        var actualType  = typeof(TCloakedStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, resolvedFlags, formatString);
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
        var actualType  = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TBearer>(TBearer value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString);
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
            if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TBearerStruct>(TBearerStruct? value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString);
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
            if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, resolvedFlags, formatString); }
        }
        else { VettedJoinValue(value, resolvedFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TBearer>(TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, resolvedFlags, formatString);
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
            if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
            }
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldValueOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, resolvedFlags, formatString); }
        }
        else { VettedJoinValue(value, resolvedFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TBearerStruct>(TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, resolvedFlags, formatString);
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
        var actualType  = typeof(Span<char>);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsValueFormattingFlags("Span", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(Span<char>);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsValueFormattingFlags("Span", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags);
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
        var actualType  = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, fallbackValue, formatString, resolvedFlags);
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
        var actualType  = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsValueFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
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
        var actualType  = typeof(string);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(string);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags);
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
        var actualType  = typeof(string);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(string);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags);
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
        var actualType  = typeof(char[]);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(char[]? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(char[]);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags);
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
        var actualType  = typeof(char[]);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(char[]);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags);
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
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin<TCharSeq>(TCharSeq value, int startIndex, int length, string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags);
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
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags);
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
        var actualType  = typeof(StringBuilder);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueWithDefaultJoin(StringBuilder? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(StringBuilder);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, resolvedFlags);
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
        var actualType  = typeof(StringBuilder);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(StringBuilder);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, resolvedFlags);
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
        var actualType  = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedValueMatchJoinValue(value, formatString, resolvedFlags); }
        }
        else { VettedValueMatchJoinValue(value, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueMatchJoin<TAny>(TAny? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedValueMatchJoinValue(value, formatString, resolvedFlags);
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
        var actualType  = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinValueMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, resolvedFlags);
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
        var actualType  = typeof(bool?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(bool?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, "", formatFlags)
         && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags) 
         && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        else if (SkipBody && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            if (Settings.InstanceTrackingAllAsStringClassesAreExempt)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TFmt>(TFmt value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, "", formatFlags)
         && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            if (Settings.InstanceTrackingAllAsStringClassesAreExempt)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
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
        else
        {
            this.AppendFormattedOrNull(value, formatString, formatFlags);
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags) 
         && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        else if (SkipBody && Settings.InstanceMarkingIncludeSpanFormattableContents)
        {
            StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
        }
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            if (Settings.InstanceTrackingAllAsStringClassesAreExempt)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, "", formatFlags)
         && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value))
        {
            if (Settings.InstanceTrackingAllAsStringClassesAreExempt)
            {
                Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
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
        var actualType  = typeof(TFmtStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (callContext.HasFormatChange)
        {
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
        var actualType  = typeof(TFmtStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(TFmtStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
    }

    public TToContentMold VettedJoinStringWithDefault<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            }
        }
        else
        {
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            var stateExtractResult = withMoldInherited.HasIsFieldNameFlag() 
                ? StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting) 
                : StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited);
            if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
            {
                if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
                }
            }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrNullNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString , maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sb.Append(DblQt);
                if (addEndDblQt) Sb.Append(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            var stateExtractResult = withMoldInherited.HasIsFieldNameFlag() 
                ? StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting) 
                : StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited);
            if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
            {
                if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
                }
            }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
    }

    public TToContentMold VettedJoinStringWithDefault<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
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
        else
        {
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value.Value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal, formatString, withMoldInherited); }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true) where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (callContext.HasFormatChange)
        {
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
        var actualType  = typeof(TCloakedStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value.Value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal, formatString, withMoldInherited); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TBearer>(TBearer value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinStringWithDefault<TBearer>(TBearer value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else
                {
                    StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString,
                                                       formatFlags: withMoldInherited | DisableAutoDelimiting);
                }
            }
        }
        else
        {
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            var stateExtractResult = withMoldInherited.HasIsFieldNameFlag() 
                ? StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited | DisableFieldNameDelimiting) 
                : StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited);
            if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
            {
                if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
                }
            }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
      , TBearerStruct? value, string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinStringWithDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
            }
        }
        else
        {
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value.Value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value.Value, formatString, withMoldInherited | DisableAutoDelimiting); }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TBearer>(TBearer value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var maybeComplex = formatFlags & ~(SuppressOpening | SuppressClosing);
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, maybeComplex), formatString);
        if (BuildingInstanceEquals(value))
        {
            if (WroteTypeName)
            {
                resolvedFlags |= LogSuppressTypeNames;
            }
            resolvedFlags |= NoRevisitCheck;
        }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString<TBearer>(TBearer value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false) where TBearer : IStringBearer?
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sb.Append(DblQt);
                if (addEndDblQt) Sb.Append(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            var stateExtractResult = withMoldInherited.HasIsFieldNameFlag() 
                ? StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited | DisableFieldNameDelimiting) 
                : StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited);
            if (BuildingInstanceEquals(value) && stateExtractResult.WrittenAs.SupportsMultipleFields())
            {
                if (Master.InstanceIdAtVisit(MoldGraphVisit.CurrentVisitIndex) <= 0)
                {
                    Master.UpdateVisitAddFormatFlags(MoldGraphVisit.CurrentVisitIndex, NoRevisitCheck);
                    Master.UpdateVisitRemoveFormatFlags(stateExtractResult.VisitNumber, NoRevisitCheck);
                }
            }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringRevealOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public TToContentMold VettedJoinString<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null, bool addStartDblQt = false, bool addEndDblQt = false) where TBearerStruct : struct, IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);
                if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if (addStartDblQt) Sf.Gb.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value.Value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value.Value, formatString, withMoldInherited); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public TToContentMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(Span<char>);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(Span<char>);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringWithDefaultJoin(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(string);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(string);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(string);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
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
        var actualType  = typeof(string);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(char[]);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(char[]);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(char[]);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
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
        var actualType  = typeof(char[]);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        
        if (callContext.HasFormatChange)
        {
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
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin<TCharSeq>(TCharSeq value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "",  formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(StringBuilder);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
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
        var actualType  = typeof(StringBuilder);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue,  formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = typeof(StringBuilder);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(StringBuilder);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "",  formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringMatchJoin(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringMatchJoin(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringMatchJoin<TAny>(TAny value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "",  formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (!callContext.HasFormatChange) return VettedJoinStringMatchJoin(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
        var actualType  = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringMatchWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringMatchWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public TToContentMold JoinStringMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
        ContentType = actualType;
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "",  formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringMatchWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
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
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        SkipBody && (!actualType.IsSpanFormattableCached() || !ignoreSuppressSpanFormattable);

    public override bool HasSkipField(Type actualType, ReadOnlySpan<char> fieldName, FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) =>
        countNextSkipFieldIsSkipBody-- > 0 
            ? HasSkipBody(actualType, fieldName, formatFlags) : base.HasSkipField(actualType, fieldName, formatFlags);

    // private int NextSkipFieldCallsSkipBody() => ++countNextSkipFieldIsSkipBody;

    public new TToContentMold WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)

    {
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public override ITypeMolderDieCast CopyFrom(ITypeMolderDieCast? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        base.CopyFrom(source, copyMergeFlags);
        if (source is ContentTypeDieCast<TContentMold, TToContentMold> valueTypeDieCast) { CurrentWriteMethod = valueTypeDieCast.CurrentWriteMethod; }

        return this;
    }

    public override void StateReset()
    {
        ignoreSuppressSpanFormattable = false;
        base.StateReset();
    }
}
