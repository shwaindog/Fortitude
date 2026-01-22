// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ContentTypeDieCast<TContentMold> : TypeMolderDieCast<TContentMold> where TContentMold : ContentTypeMold<TContentMold>
{
    public const string DblQt = "\"";

    private bool ignoreSuppressSpanFormattable;
    private int  countNextSkipFieldIsSkipBody;

    public ContentTypeDieCast<TContentMold> InitializeValueBuilderCompAccess
        (TContentMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WriteMethodType writeMethod)
    {
        var createFmtFlags = typeBuilderPortableState.CreateFormatFlags;
        writeMethod = createFmtFlags.DoesNotHaveContentAllowComplexType()
            ? writeMethod.ToNoFieldEquivalent()
            : writeMethod;

        Initialize(externalTypeBuilder, typeBuilderPortableState, writeMethod);

        if (SkipBody && MoldGraphVisit.HasExistingInstanceId
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

    public ContentJoinTypeMold<TContentMold> FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(bool?);
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = value?.GetType() ?? typeof(bool?);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }

        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags, formatString);
        var callContext   = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, resolvedFlags);
        using (callContext) { return VettedJoinValue(value, formatString, resolvedFlags); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinValue(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType    = value?.GetType() ?? typeof(TFmt);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TFmt>(TFmt? value
      , ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TFmt>(TFmt? value, ReadOnlySpan<char> defaultValue, string formatString = ""
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
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldFmtValueOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        this.AppendFormattedOrNull(value, formatString, withMoldInherited);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TFmtStruct>(TFmtStruct? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TFmtStruct>(TFmtStruct? value, ReadOnlySpan<char> defaultValue
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

    public ContentJoinTypeMold<TContentMold> FieldFmtValueOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TFmtStruct>(TFmtStruct? value, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext   = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }
        }
        else { VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TCloaked, TRevealBase>(TCloaked? value, PalantírReveal<TRevealBase> palantírReveal
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, palantírReveal, formatString, resolvedFlags);
        using (callContext) { return VettedJoinValue(value, palantírReveal, formatString, resolvedFlags); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TCloaked, TRevealBase>(TCloaked? value, PalantírReveal<TRevealBase> palantírReveal
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
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited);
        }
        else { StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        
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
            using (callContext) { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags); }
        }
        else { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, resolvedFlags); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TCloaked, TRevealBase>(TCloaked? value
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
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited);
        }
        else { StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TCloakedStruct>(TCloakedStruct? value
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        
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
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TBearer>(TBearer value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TBearer>(TBearer value
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
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        
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
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, resolvedFlags, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TBearerStruct>(TBearerStruct? value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TBearerStruct>(TBearerStruct? value
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
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Master, value.Value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Master, value.Value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, resolvedFlags, formatString); }
        }
        else { VettedJoinValue(value, resolvedFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TBearer>(TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsValueContent);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, resolvedFlags, formatString);
        using (callContext) { return VettedJoinValue(value, resolvedFlags, formatString); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TBearer>(TBearer value, FormatFlags formatFlags = DefaultCallerTypeFlags
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
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "") where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TBearerStruct>(TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "")
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , ReadOnlySpan<char> fallbackValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(Span<char>);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(Span<char>);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue(Span<char> value, ReadOnlySpan<char> fallbackValue
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , ReadOnlySpan<char> fallbackValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue(ReadOnlySpan<char> value, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(string);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(string);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValueWithDefault(string? value, int startIndex, int length
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(string);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(string);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue(string? value, int startIndex, int length, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(char[]);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(char[]? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(char[]);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValueWithDefault(char[]? value, int startIndex, int length, string defaultValue = ""
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(char[]);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(char[]);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue(char[]? value, int startIndex, int length, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length, string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TCharSeq>(TCharSeq value, int startIndex, int length, string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TCharSeq>(TCharSeq value, int startIndex, int length
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex
      , int length, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(StringBuilder);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(StringBuilder? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(StringBuilder);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue(StringBuilder? value, int startIndex, int length, string defaultValue = ""
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(StringBuilder);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = typeof(StringBuilder);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValue(StringBuilder? value, int startIndex, int length, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> ValueMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueMatchJoin<TAny>(TAny? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
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

    public ContentJoinTypeMold<TContentMold> VettedValueMatchJoinValue<TAny>(TAny? value, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> ValueMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
        
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

    public ContentJoinTypeMold<TContentMold> JoinValueMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinValueMatchWithDefaultValue<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
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

    public ContentJoinTypeMold<TContentMold> FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(bool?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(bool?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
      , bool addEndDblQt = true)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        
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
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TFmt>(TFmt value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, "", formatFlags)
         && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TFmt>(TFmt value
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        
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
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) 
         || HasSkipBody(actualType, "", formatFlags)
         && !ignoreSuppressSpanFormattable)
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags), formatString);
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);

        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        if (addStartDblQt && value != null) Sf.Gb.AppendParentContent(DblQt);
        this.AppendFormattedOrNull(value, formatString, formatFlags);
        if (addEndDblQt && value != null) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TFmtStruct>(TFmtStruct? value, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TFmtStruct>(TFmtStruct? value, string defaultValue = ""
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType  = typeof(TFmtStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TFmtStruct>(TFmtStruct? value, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName
      , TCloaked value, PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TCloaked, TRevealBase>(TCloaked value
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
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited); }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrNullNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        
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
            using (callContext) { VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TCloaked, TRevealBase>(TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType  = value?.GetType() ?? typeof(TCloaked);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString , formatFlags), formatString);
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, palantírReveal, formatString, resolvedFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TCloaked, TRevealBase>(TCloaked value
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
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TCloakedStruct>(TCloakedStruct? value
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

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true) where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var actualType  = typeof(TCloakedStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TCloakedStruct>(TCloakedStruct? value
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

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        
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
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TBearer>(TBearer value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TBearer>(TBearer value, string defaultValue = ""
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
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited); }
        }
        if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
      , TBearerStruct? value, string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null
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

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, nonJsonfieldName, formatFlags))
        {
            return WasSkipped(actualType, nonJsonfieldName, formatFlags);
        }
        
        if (SupportsMultipleFields && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString ?? "", formatFlags));
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TBearer>(TBearer value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer?
    {
        var actualType  = value?.GetType() ?? typeof(TBearer);
        if (!Master.ContinueGivenFormattingFlags(formatFlags) || HasSkipBody(actualType, "", formatFlags))
        {
            return WasSkipped(actualType, "", formatFlags);
        }
        var resolvedFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString ?? "", formatFlags));
        if (BuildingInstanceEquals(value)) { resolvedFlags |= NoRevisitCheck; }
        var callContext = Master.ResolveContextForCallerFlags(resolvedFlags | AsStringContent);
        if (!callContext.HasFormatChange) return VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, resolvedFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TBearer>(TBearer value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null, bool addStartDblQt = false, bool addEndDblQt = false) where TBearer : IStringBearer?
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
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited); }
            if (addEndDblQt) Sf.Gb.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = "", bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string formatString = "", bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType  = typeof(TBearerStruct?);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
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

    public ContentJoinTypeMold<TContentMold> FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(Span<char>);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(Span<char>);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString(Span<char> value, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
      , bool addEndDblQt = true)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(ReadOnlySpan<char>);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString(ReadOnlySpan<char> value, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(string);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(string);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString(string? value, int startIndex, int length, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(string);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(string);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault(string? value, int startIndex, int length
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(char[]);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(char[]);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString(char[]? value, int startIndex, int length, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(char[]);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin(char[]? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(char[]);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault(char[]? value, int startIndex, int length
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length, string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence?
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TCharSeq>(TCharSeq value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TCharSeq>(TCharSeq value, int startIndex, int length
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq value, int startIndex
      , int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence?
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TCharSeq>(TCharSeq value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence?
    {
        var actualType  = value?.GetType() ?? typeof(TCharSeq);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TCharSeq>(TCharSeq value, int startIndex, int length, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(StringBuilder);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin(StringBuilder? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(StringBuilder);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault(StringBuilder? value, int startIndex, int length
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

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = typeof(StringBuilder);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = typeof(StringBuilder);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinString(StringBuilder? value, int startIndex, int length, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> StringMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringMatchJoin<TAny>(TAny value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringMatchJoin<TAny>(TAny value, string formatString = ""
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

    public ContentJoinTypeMold<TContentMold> StringMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
        
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

    public ContentJoinTypeMold<TContentMold> JoinStringMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var actualType  = value?.GetType() ?? typeof(TAny);
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

    public ContentJoinTypeMold<TContentMold> VettedJoinStringMatchWithDefault<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
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

    public ContentJoinTypeMold<TContentMold> ConditionalValueTypeSuffix()
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

    public new ContentJoinTypeMold<TContentMold> WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)

    {
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public override ITypeMolderDieCast CopyFrom(ITypeMolderDieCast? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        base.CopyFrom(source, copyMergeFlags);
        if (source is ContentTypeDieCast<TContentMold> valueTypeDieCast) { CurrentWriteMethod = valueTypeDieCast.CurrentWriteMethod; }

        return this;
    }

    public override void StateReset()
    {
        ignoreSuppressSpanFormattable = false;
        base.StateReset();
    }
}
