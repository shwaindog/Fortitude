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

    public ContentTypeDieCast<TContentMold> InitializeValueBuilderCompAccess
        (TContentMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, bool isComplex)
    {
        Initialize(externalTypeBuilder, typeBuilderPortableState, isComplex && typeBuilderPortableState.Master.Style.IsLog());

        OnFinishedWithStringBuilder = FinishUsingStringBuilder;

        return this;
    }

    private void FinishUsingStringBuilder(IScopeDelimitedStringBuilder finishedBuilding)
    {
        if (Style.IsJson()) finishedBuilding.Append(DblQt);
    }

    private Action<IScopeDelimitedStringBuilder>? OnFinishedWithStringBuilder { get; set; }

    public bool IsLog => Style.IsLog();

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()
    {
        if (Style.IsJson()) Sb.Append(DblQt);
        var scopedSb = (IScopeDelimitedStringBuilder)Sb;
        scopedSb.OnScopeEndedAction = OnFinishedWithStringBuilder;
        return scopedSb;
    }

    public ContentJoinTypeMold<TContentMold> FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<bool?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<bool?>(value?.GetType(), nonJsonfieldName, formatFlags);

        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, formatString, formatFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<bool?>(value?.GetType(), "", formatFlags))
            return WasSkipped<bool?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, formatString, formatFlags); }
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (callContext.ShouldSkip || HasSkipBody<TFmt?>(actualType, nonJsonfieldName, formatFlags))
            return WasSkipped<TFmt?>(actualType, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags));
        
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TFmt>(TFmt? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (callContext.ShouldSkip || HasSkipBody<TFmt?>(actualType, formatString, formatFlags))
            return WasSkipped<TFmt?>(actualType, formatString, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags); }
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
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldFmtValueOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (callContext.ShouldSkip || HasSkipBody<TFmt?>(actualType, nonJsonfieldName, formatFlags))
            return WasSkipped<TFmt?>(actualType, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags));

        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, formatString, formatFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (callContext.ShouldSkip || HasSkipBody<TFmt?>(actualType, "", formatFlags))
            return WasSkipped<TFmt?>(actualType, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, formatString, formatFlags); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , ReadOnlySpan<char> defaultValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TFmtStruct>(TFmtStruct? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TFmtStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TFmtStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags); }
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
            else
            {
                StyleFormatter.FormatFallbackFieldContents<TFmtStruct>(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited);
            }
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TFmtStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TFmtStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, formatString, formatFlags); }
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
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, palantírReveal, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, palantírReveal, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TCloaked, TRevealBase>(TCloaked? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloaked?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCloaked?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinValue(value, palantírReveal, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, palantírReveal, formatString, formatFlags); }
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
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { JoinValueJoin(value, palantírReveal, formatString, formatFlags); }
        }
        else { JoinValueJoin(value, palantírReveal, formatString, formatFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloakedStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCloakedStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, palantírReveal, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, palantírReveal, formatString, formatFlags); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
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
      , PalantírReveal<TRevealBase> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, formatFlags); }
        }
        else { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, formatFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal
      , ReadOnlySpan<char> defaultValue, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloaked?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCloaked?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, formatFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatString, formatFlags); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal
      , ReadOnlySpan<char> defaultValue, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
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
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null) where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloakedStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCloakedStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatFlags, formatString);
        using (callContext) { return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatFlags, formatString); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, formatFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, formatFlags, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TBearer>(TBearer? value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearer?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TBearer?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, formatFlags, formatString);
        using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, formatFlags, formatString); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TBearer>(TBearer? value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
        where TBearer : IStringBearer
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
      , string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, defaultValue, formatFlags, formatString); }
        }
        else { VettedJoinWithDefaultValue(value, defaultValue, formatFlags, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TBearerStruct>(TBearerStruct? value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearerStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TBearerStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, defaultValue, formatFlags, formatString);
        using (callContext) { return VettedJoinWithDefaultValue(value, defaultValue, formatFlags, formatString); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TBearerStruct>(TBearerStruct? value
      , ReadOnlySpan<char> defaultValue, FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null) where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatFlags, formatString); }
        }
        else { VettedJoinValue(value, formatFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TBearer>(TBearer? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearer?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TBearer?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatFlags, formatString);
        using (callContext) { return VettedJoinValue(value, formatFlags, formatString); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TBearer>(TBearer? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null)
        where TBearer : IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString ?? "", formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag()) { StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited); }
        else { StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null) where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatFlags, formatString); }
        }
        else { VettedJoinValue(value, formatFlags, formatString); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TBearerStruct>(TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearerStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TBearerStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatFlags, formatString);
        using (callContext) { return VettedJoinValue(value, formatFlags, formatString); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinValue<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null)
        where TBearerStruct : struct, IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();
            AppendNull(formatString ?? "", formatFlags);
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<Memory<char>?>(value.Length > 0 ? typeof(Span<char>) : null, nonJsonfieldName, formatFlags))
            return WasSkipped<Memory<char>?>(value.Length > 0 ? typeof(Span<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "Span", formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags); }
        }
        else { VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<Memory<char>?>(value.Length > 0 ? typeof(Span<char>) : null, "", formatFlags))
            return WasSkipped<Memory<char>?>(value.Length > 0 ? typeof(Span<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "Span", formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags); }
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
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited);
        }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , ReadOnlySpan<char> fallbackValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip
         || HasSkipBody<ReadOnlyMemory<char>?>(value.Length > 0
                                                   ? typeof(ReadOnlySpan<char>)
                                                   : null, nonJsonfieldName, formatFlags))
            return WasSkipped<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "ReadOnlySpan", formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags); }
        }
        else { VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip 
         || HasSkipBody<ReadOnlyMemory<char>?>(value.Length > 0 
                                                   ? typeof(ReadOnlySpan<char>) 
                                                   : null, "", formatFlags))
            return WasSkipped<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "ReadOnlySpan", formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags); }
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
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited);
        }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip ||
            HasSkipBody<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags))
            return WasSkipped<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "ReadOnlySpan", formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, formatString, formatFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip 
         || HasSkipBody<ReadOnlyMemory<char>?>(value.Length > 0 
                                                   ? typeof(ReadOnlySpan<char>) 
                                                   : null, "", formatFlags))
            return WasSkipped<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "ReadOnlySpan", formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, formatString, formatFlags); }
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<string?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<string?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags); }
        }
        else { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<string?>(value?.GetType(), "", formatFlags))
            return WasSkipped<string?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags);
        using (callContext) { return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags); }
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<ReadOnlyMemory<char>?>(value?.GetType(), "", formatFlags))
            return WasSkipped<ReadOnlyMemory<char>?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, formatFlags); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<string?>(value?.GetType(), "", formatFlags))
            return WasSkipped<string?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
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
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags); }
        }
        else { VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(char[]? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<char[]?>(value?.GetType(), "", formatFlags))
            return WasSkipped<char[]?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags);
        using (callContext) { return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags); }
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<char[]?>(value?.GetType(), "", formatFlags))
            return WasSkipped<char[]?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
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

    public ContentJoinTypeMold<TContentMold> FieldValueOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex
      , int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags); }
        }
        else { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCharSeq?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCharSeq?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinWithDefaultValue<TCharSeq>(TCharSeq? value, int startIndex, int length
      , string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
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
      , int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<TCharSeq?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCharSeq?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags); }
        }
        else { VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueWithDefaultJoin(StringBuilder? value, int startIndex, int length, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<StringBuilder?>(value?.GetType(), "", formatFlags))
            return WasSkipped<StringBuilder?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags);
        using (callContext) { return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags); }
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
      , int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
        }
        else { VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        if (callContext.ShouldSkip || HasSkipBody<StringBuilder?>(value?.GetType(), "", formatFlags))
            return WasSkipped<StringBuilder?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinValue(value, startIndex, length, formatString, formatFlags);
        using (callContext) { return VettedJoinValue(value, startIndex, length, formatString, formatFlags); }
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        var actualType  = value?.GetType() ?? typeof(TAny);
        if (callContext.ShouldSkip || HasSkipBody<TAny?>(actualType, nonJsonfieldName, formatFlags))
            return WasSkipped<TAny?>(actualType, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedValueMatchJoinValue(value, formatString, formatFlags); }
        }
        else { VettedValueMatchJoinValue(value, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueMatchJoin<TAny>(TAny? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        var actualType  = value?.GetType() ?? typeof(TAny);
        if (callContext.ShouldSkip || HasSkipBody<TAny?>(actualType, "", formatFlags))
            return WasSkipped<TAny?>(actualType, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedValueMatchJoinValue(value, formatString, formatFlags);
        using (callContext) { return VettedValueMatchJoinValue(value, formatString, formatFlags); }
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
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        var actualType  = value?.GetType() ?? typeof(TAny);
        if (callContext.ShouldSkip || HasSkipBody<TAny?>(actualType, nonJsonfieldName, formatFlags))
            return WasSkipped<TAny?>(actualType, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, formatFlags); }
        }
        else { VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, formatFlags); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinValueMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsValueContent);
        var actualType  = value?.GetType() ?? typeof(TAny);
        if (callContext.ShouldSkip || HasSkipBody<TAny?>(actualType, "", formatFlags))
            return WasSkipped<TAny?>(actualType, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, formatFlags);
        using (callContext) { return VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, formatFlags); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinValueMatchWithDefaultValue<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<bool?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<bool?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<bool?>(value?.GetType(), "", formatFlags))
            return WasSkipped<bool?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString(bool? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
        }
        else
        {
            if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, value, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited); }

            if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true
      , bool addEndDblQt = true)
        where TFmt : ISpanFormattable?
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (callContext.ShouldSkip || HasSkipBody<TFmt?>(actualType, nonJsonfieldName, formatFlags))
            return WasSkipped<TFmt?>(actualType, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TFmt>(TFmt value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (callContext.ShouldSkip || HasSkipBody<TFmt?>(actualType, "", formatFlags))
            return WasSkipped<TFmt?>(actualType, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TFmt>(TFmt value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Sb, value, formatString, withMoldInherited | DisableAutoDelimiting); }
        }
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmt : ISpanFormattable?
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (callContext.ShouldSkip || HasSkipBody<TFmt?>(actualType, nonJsonfieldName, formatFlags))
            return WasSkipped<TFmt?>(actualType, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));

        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        var actualType  = value?.GetType() ?? typeof(TFmt);
        if (callContext.ShouldSkip || HasSkipBody<TFmt?>(actualType, "", formatFlags))
            return WasSkipped<TFmt?>(actualType, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));
        
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TFmt>(TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable?
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
        }
        else
        {
            if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { this.AppendMatchFormattedOrNull(value, formatString, withMoldInherited | DisableAutoDelimiting); }
            if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TFmtStruct>(TFmtStruct? value, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TFmtStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TFmtStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TFmtStruct>(TFmtStruct? value, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TFmtStruct>(TFmtStruct? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TFmtStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TFmtStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
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
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
        }
        else
        {
            if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Sb, value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { this.AppendMatchFormattedOrNull(value, formatString, withMoldInherited | DisableAutoDelimiting); }
            if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName
      , TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloaked?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCloaked?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrNullNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, palantírReveal, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, palantírReveal, formatString, formatFlags, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloaked?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCloaked?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinString(value, palantírReveal, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, palantírReveal, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase
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
            if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value, palantírReveal, formatString, withMoldInherited); }
            if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloakedStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCloakedStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true) where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, palantírReveal, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, palantírReveal, formatString, formatFlags, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCloakedStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCloakedStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinString(value, palantírReveal, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, palantírReveal, formatString, formatFlags, addStartDblQt, addEndDblQt); }
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
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value.Value, palantírReveal, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal, formatString, withMoldInherited); }
            if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatFlags, formatString, addStartDblQt, addEndDblQt); }

        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TBearer>(TBearer? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearer?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TBearer?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags, formatString);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, formatFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TBearer>(TBearer? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string formatString = ""
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
      , TBearerStruct? value, string defaultValue = "", FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatFlags, formatString, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearerStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TBearerStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags, formatString ?? "");
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, formatFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatFlags, formatString, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TBearer>(TBearer? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearer?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TBearer?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TBearer>(TBearer? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null, bool addStartDblQt = false, bool addEndDblQt = false) where TBearer : IStringBearer
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
            if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value, formatString, withMoldInherited); }
            if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringRevealOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FormatFlags formatFlags = DefaultCallerTypeFlags, string? formatString = null, bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatFlags, formatString, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatFlags, formatString, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TBearerStruct?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TBearerStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatFlags, formatString, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatFlags, formatString, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TBearerStruct>(TBearerStruct? value, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? formatString = null, bool addStartDblQt = false, bool addEndDblQt = false) where TBearerStruct : struct, IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (withMoldInherited.HasIsFieldNameFlag())
            {
                StyleFormatter.FormatFieldName(Master, value.Value, formatString, withMoldInherited | DisableFieldNameDelimiting);
            }
            else { StyleFormatter.FormatFieldContents(Master, value.Value, formatString, withMoldInherited); }
            if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        }
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<Memory<char>?>(value.Length > 0 ? typeof(Span<char>) : null, nonJsonfieldName, formatFlags))
            return WasSkipped<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<Memory<char>?>(value.Length > 0 ? typeof(Span<char>) : null, "", formatFlags))
            return WasSkipped<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString(Span<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string defaultValue = ""
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip ||
            HasSkipBody<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags))
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", defaultValue, formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags))
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", StyleFormatter.ResolveContentAsStringFormattingFlags("Span", defaultValue, formatString, formatFlags));
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip ||
            HasSkipBody<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags))
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags))
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString(ReadOnlySpan<char> value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, value, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: withMoldInherited); }
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<string?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<string?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(string? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<string?>(value?.GetType(), "", formatFlags))
            return WasSkipped<string?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
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
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited); }

                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

                        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<string?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<string?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<string?>(value?.GetType(), "", formatFlags))
            return WasSkipped<string?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags); }
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<char[]?>(value?.GetType(), "", formatFlags))
            return WasSkipped<char[]?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
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
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

                var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

                        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                         , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, ((ReadOnlySpan<char>)""), 0, formatString, formatFlags: formatFlags); }
                        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin(char[]? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<char[]?>(value?.GetType(), "", formatFlags))
            return WasSkipped<char[]?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault(char[]? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags); }
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex
      , int length
      , string defaultValue = "", string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin<TCharSeq>(TCharSeq? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCharSeq?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCharSeq?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault<TCharSeq>(TCharSeq? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

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
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex
      , int length
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<TCharSeq?>(value?.GetType(), "", formatFlags))
            return WasSkipped<TCharSeq?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));

        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinString<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);

            var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
            if (capLength > 0)
            {
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

                if (withMoldInherited.HasIsFieldNameFlag())
                {
                    StyleFormatter.FormatFieldName(Sb, value, capStart, formatString, capLength
                                                 , formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                }
                else { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: withMoldInherited); }
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);


                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext)
            {
                VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
            }
        }
        else { VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringWithDefaultJoin(StringBuilder? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<StringBuilder?>(value?.GetType(), "", formatFlags))
            return WasSkipped<StringBuilder?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!callContext.HasFormatChange)
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext)
        {
            return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringWithDefault(StringBuilder? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

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
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
        }
        if (value == null && formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder.TransitionToNextMold();

        if (withMoldInherited.HasIsFieldNameFlag())
        {
            StyleFormatter.FormatFieldName(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
        }
        else { StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: withMoldInherited); }
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex
      , int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags))
            return WasSkipped<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));

        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        if (callContext.ShouldSkip || HasSkipBody<StringBuilder?>(value?.GetType(), "", formatFlags))
            return WasSkipped<StringBuilder?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));
        if (!callContext.HasFormatChange) return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt); }
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
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

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
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

                        if (withMoldInherited.HasIsFieldNameFlag())
                        {
                            StyleFormatter.FormatFieldName(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableFieldNameDelimiting);
                        }
                        else { StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited); }
                        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        var actualType  = value?.GetType() ?? typeof(TAny);
        if (callContext.ShouldSkip || HasSkipBody<TAny?>(actualType, nonJsonfieldName, formatFlags))
            return WasSkipped<TAny?>(actualType, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringMatchJoin(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringMatchJoin(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringMatchJoin<TAny>(TAny? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        var actualType  = value?.GetType() ?? typeof(TAny);
        if (callContext.ShouldSkip || HasSkipBody<TAny?>(actualType, "", formatFlags)) return WasSkipped<TAny?>(actualType, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }

        if (!callContext.HasFormatChange) return VettedJoinStringMatchJoin(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringMatchJoin(value, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringMatchJoin<TAny>(TAny? value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var withMoldInherited = formatFlags | CreateMoldFormatFlags.MoldInheritFlags();
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                StyleFormatter.FormatFieldContents(Sb, "", 0, formatString, formatFlags: withMoldInherited | DisableAutoDelimiting);
                if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
                return StyleTypeBuilder.TransitionToNextMold();
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder.TransitionToNextMold();
        }
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);

        this.AppendMatchFormattedOrNull(value, formatString, DisableAutoDelimiting | withMoldInherited | DisableFieldNameDelimiting);
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public ContentJoinTypeMold<TContentMold> StringMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = ""
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        var actualType  = value?.GetType() ?? typeof(TAny);
        if (callContext.ShouldSkip || HasSkipBody<TAny?>(actualType, nonJsonfieldName, formatFlags))
            return WasSkipped<TAny?>(actualType, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (WriteAsComplex && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (callContext.HasFormatChange)
        {
            using (callContext) { VettedJoinStringMatchWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        }
        else { VettedJoinStringMatchWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
        return ConditionalValueTypeSuffix();
    }

    public ContentJoinTypeMold<TContentMold> JoinStringMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags | AsStringContent);
        var actualType  = value?.GetType() ?? typeof(TAny);
        if (callContext.ShouldSkip || HasSkipBody<TAny?>(actualType, "", formatFlags)) return WasSkipped<TAny?>(actualType, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString, formatFlags));
        if (!actualType.IsValueType && BuildingInstanceEquals(value)) { formatFlags |= NoRevisitCheck; }
        if (!callContext.HasFormatChange)
            return VettedJoinStringMatchWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        using (callContext) { return VettedJoinStringMatchWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt); }
    }

    public ContentJoinTypeMold<TContentMold> VettedJoinStringMatchWithDefault<TAny>(TAny? value, ReadOnlySpan<char> defaultValue
      , string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (addStartDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
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
        if (addEndDblQt) StyleFormatter.GraphBuilder.AppendParentContent(DblQt);
        return StyleTypeBuilder.TransitionToNextMold();
    }

    protected void AppendNull(string formatString, FormatFlags formatFlags)
    {
        StyleFormatter.AppendFormattedNull(Sb, formatString, formatFlags);
    }

    public ContentJoinTypeMold<TContentMold> ConditionalValueTypeSuffix()
    {
        if (WriteAsComplex)
        {
            this.AddGoToNext();
            return StyleTypeBuilder.TransitionToNextMold();
        }
        StyleFormatter.GraphBuilder.Complete(StyleFormatter.GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags);
        return StyleTypeBuilder.TransitionToNextMold();
    }


    public new ContentJoinTypeMold<TContentMold> WasSkipped<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)

    {
        return StyleTypeBuilder.TransitionToNextMold();
    }

    public override ITypeMolderDieCast CopyFrom(ITypeMolderDieCast? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        base.CopyFrom(source, copyMergeFlags);
        if (source is ContentTypeDieCast<TContentMold> valueTypeDieCast) { WriteAsComplex = valueTypeDieCast.WriteAsComplex; }

        return this;
    }
}
