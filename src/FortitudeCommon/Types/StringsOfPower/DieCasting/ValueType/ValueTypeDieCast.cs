// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

public class ValueTypeDieCast<TVMold> : TypeMolderDieCast<TVMold> where TVMold : TypeMolder
{
    protected bool ValueInComplexType { get; private set; }

    public ValueTypeDieCast<TVMold> InitializeValueBuilderCompAccess
        (TVMold externalTypeBuilder, TypeMolder.StyleTypeBuilderPortableState typeBuilderPortableState, bool isComplex)
    {
        Initialize(externalTypeBuilder, typeBuilderPortableState);

        ValueInComplexType          = isComplex || typeBuilderPortableState.Master.Style.AllowsUnstructured();
        OnFinishedWithStringBuilder = FinishUsingStringBuilder;

        return this;
    }

    private void FinishUsingStringBuilder(IScopeDelimitedStringBuilder finishedBuilding)
    {
        if (Style.IsJson()) finishedBuilding.Append("\"");
    }

    private Action<IScopeDelimitedStringBuilder>? OnFinishedWithStringBuilder { get; set; }

    protected bool NotJson => Style.IsNotJson();

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()
    {
        if (Style.IsJson()) Sb.Append("\"");
        var scopedSb = (IScopeDelimitedStringBuilder)Sb;
        scopedSb.OnScopeEndedAction = OnFinishedWithStringBuilder;
        return scopedSb;
    }

    public TVMold FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
        , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<bool?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<bool?>(value?.GetType(), nonJsonfieldName, formatFlags);
        
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin(bool? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<bool?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<bool?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValue(bool? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Sb, value, formatString, formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, ReadOnlySpan<char> defaultValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmt?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TFmt?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value , formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin<TFmt>(TFmt? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmt?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TFmt?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString));
        return VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue<TFmt>(TFmt? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.GraphBuilder.StartNextContentSeparatorPaddingSequence(Sb, StyleFormatter, formatFlags, true);
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Sb, value, formatString, formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold FieldFmtValueOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TFmt : ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmt?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TFmt?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString) );
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin<TFmt>(TFmt? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmt?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TFmt?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString));
        return VettedJoinValue(value, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValue<TFmt>(TFmt? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        StyleFormatter.FormatFieldContents(Sb, value, formatString, formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, ReadOnlySpan<char> defaultValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags  | StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin<TFmtStruct>(TFmtStruct? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmtStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TFmtStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString));
        return VettedJoinWithDefaultValue(value, defaultValue, formatString, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue<TFmtStruct>(TFmtStruct? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Sb, value, formatString, formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold FieldFmtValueOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin<TFmtStruct>(TFmtStruct? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmtStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TFmtStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value,formatFlags  | StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString));
        return VettedJoinValue(value, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValue<TFmtStruct>(TFmtStruct? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Sb, value, formatString, formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, palantírReveal, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin<TCloaked, TRevealBase>(TCloaked? value , PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloaked?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCloaked?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, palantírReveal, formatFlags);
    }
    
    public TVMold VettedJoinValue<TCloaked, TRevealBase>(TCloaked? value , PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull( "", formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Master, value, palantírReveal); 
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        JoinValueJoin(value, palantírReveal, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin<TCloakedStruct>(TCloakedStruct? value , PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloakedStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCloakedStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, palantírReveal, formatFlags);
    }
    
    public TVMold VettedJoinValue<TCloakedStruct>(TCloakedStruct? value , PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull( "", formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal); 
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin<TCloaked, TRevealBase>(TCloaked? value , PalantírReveal<TRevealBase> palantírReveal
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloaked?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCloaked?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue<TCloaked, TRevealBase>(TCloaked? value , PalantírReveal<TRevealBase> palantírReveal
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, "", formatFlags: formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Master, value, palantírReveal);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value , PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloakedStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCloakedStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinWithDefaultValue(value, palantírReveal, defaultValue, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue<TCloakedStruct>(TCloakedStruct? value , PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, "", formatFlags: formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, defaultValue, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin<TBearer>(TBearer? value
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearer?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TBearer?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinWithDefaultValue(value, defaultValue, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue<TBearer>(TBearer? value
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TBearer : IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, "", formatFlags: formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Master, value);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, defaultValue, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin<TBearerStruct>(TBearerStruct? value
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearerStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TBearerStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinWithDefaultValue(value, defaultValue, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue<TBearerStruct>(TBearerStruct? value
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TBearerStruct : struct, IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, "", formatFlags: formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Master, value.Value);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin<TBearer>(TBearer? value, FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearer?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TBearer?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, formatFlags);
    }
    
    public TVMold VettedJoinValue<TBearer>(TBearer? value, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TBearer : IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull( "", formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Master, value);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin<TBearerStruct>(TBearerStruct? value, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearerStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TBearerStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, formatFlags);
    }
    
    public TVMold VettedJoinValue<TBearerStruct>(TBearerStruct? value, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TBearerStruct : struct, IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull("", formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Master, value.Value);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<Memory<char>?>(value.Length > 0 ?typeof(Span<char>) : null, nonJsonfieldName, formatFlags)) 
            return WasSkipped<Memory<char>?>(value.Length > 0 ?typeof(Span<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "Span", formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin(Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<Memory<char>?>(value.Length > 0 ?typeof(Span<char>) : null, "", formatFlags)) 
            return WasSkipped<Memory<char>?>(value.Length > 0 ?typeof(Span<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "Span", formatFlags);
        return VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue(Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, fallbackValue, 0, formatString, formatFlags: formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ?typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>?>(value.Length > 0 ?typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "ReadOnlySpan", formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ?typeof(ReadOnlySpan<char>) : null, "", formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>?>(value.Length > 0 ?typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "ReadOnlySpan", formatFlags);
        return VettedJoinWithDefaultValue(value, fallbackValue, formatString, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue(ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, fallbackValue, 0, formatString, formatFlags: formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ?typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>?>(value.Length > 0 ?typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "ReadOnlySpan", formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin(ReadOnlySpan<char> value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ?typeof(ReadOnlySpan<char>) : null, "", formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>?>(value.Length > 0 ?typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, "ReadOnlySpan", formatFlags);
        return VettedJoinValue(value, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValue(ReadOnlySpan<char> value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder;
        }
        StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<string?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<string?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<string?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<string?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValueWithDefault(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) 
            { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength
                                               , formatFlags: formatFlags); }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, startIndex, length, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin(string? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<string?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<string?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, startIndex, length, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValue(string? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags); }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        StyleFormatter.FormatFieldContents(Sb,  "",0, formatString, formatFlags: formatFlags);
                        return StyleTypeBuilder;
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue = ""
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin(char[]? value, int startIndex, int length, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<char[]?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<char[]?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValueWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValueWithDefault(char[]? value, int startIndex, int length, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength,  formatFlags: formatFlags);
            }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                StyleFormatter.FormatFieldContents(Sb, defaultValue,0, formatString, formatFlags: formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue,0, formatString, formatFlags:  formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, startIndex, length, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<char[]?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<char[]?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, startIndex, length, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValue(char[]? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags); }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        StyleFormatter.FormatFieldContents(Sb, Array.Empty<char>(), 0, formatString,  formatFlags: formatFlags);
                        return StyleTypeBuilder;
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCharSeq?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCharSeq?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue<TCharSeq>(TCharSeq? value, int startIndex, int length, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.FormatFieldContents
                (Sb, value, capStart, formatString, capLength,  formatFlags: formatFlags); }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                StyleFormatter.FormatFieldContents(Sb, defaultValue,0, formatString,  formatFlags: formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString,  formatFlags: formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, startIndex, length, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCharSeq?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCharSeq?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, startIndex, length, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValue<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.FormatFieldContents
                (Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        StyleFormatter.FormatFieldContents(Sb,  "",0, formatString, formatFlags: formatFlags);
                        return StyleTypeBuilder;
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueWithDefaultJoin(StringBuilder? value, int startIndex, int length, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<StringBuilder?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<StringBuilder?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinWithDefaultValue(value, startIndex, length, defaultValue, formatString, formatFlags);
    }
    
    public TVMold VettedJoinWithDefaultValue(StringBuilder? value, int startIndex, int length, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength
                                                                  , formatFlags: formatFlags); }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                StyleFormatter.FormatFieldContents(Sb, defaultValue,0, formatString, formatFlags: formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue,0, formatString, formatFlags: formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValue(value, startIndex, length, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<StringBuilder?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<StringBuilder?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinValue(value, startIndex, length, formatString, formatFlags);
    }
    
    public TVMold VettedJoinValue(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength
                                                                  , formatFlags: formatFlags); }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        StyleFormatter.FormatFieldContents(Sb, "",0, formatString, formatFlags: formatFlags);
                        return StyleTypeBuilder;
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold ValueMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TAny?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TAny?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString));
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedValueMatchJoinValue(value, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueMatchJoin<TAny>(TAny? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TAny?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TAny?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, "", formatString));
        return VettedValueMatchJoinValue(value, formatString, formatFlags);
    }
    
    public TVMold VettedValueMatchJoinValue<TAny>(TAny? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder;
        }
        this.AppendMatchFormattedOrNull(value, formatString, formatFlags);
        return StyleTypeBuilder;
    }

    public TVMold ValueMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TAny?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TAny?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString));
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, formatFlags);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinValueMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TAny?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TAny?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsValueFormattingFlags(value, defaultValue, formatString));
        return VettedJoinValueMatchWithDefaultValue(value, defaultValue, formatString, formatFlags );
    }
    
    public TVMold VettedJoinValueMatchWithDefaultValue<TAny>(TAny? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
    {
        if (value != null)
        {
            this.AppendMatchFormattedOrNull(value, formatString, formatFlags);
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true) 
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<bool?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<bool?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags   | StyleFormatter.ResolveContentAsStringFormattingFlags( value, "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin(bool? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<bool?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<bool?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags  | StyleFormatter.ResolveContentAsStringFormattingFlags( value, "", formatString));
        return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString(bool? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
        , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull(formatString, formatFlags);
        }
        else
        {
            if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            this.AppendFormattedOrNull(value, formatString, formatFlags);
            if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = ""
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true) 
        where TFmt : ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmt?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TFmt?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags( value, defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin<TFmt>(TFmt? value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmt?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TFmt?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault<TFmt>(TFmt? value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        else { this.AppendMatchFormattedOrNull(value, formatString, formatFlags | DisableAutoDelimiting); }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true) 
        where TFmt : ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmt?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TFmt?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin<TFmt>(TFmt? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmt?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TFmt?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString<TFmt>(TFmt? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmt : ISpanFormattable
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull(formatString, formatFlags);
        }
        else
        {
            if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            this.AppendMatchFormattedOrNull(value, formatString, formatFlags);
            if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue = ""
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true) 
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin<TFmtStruct>(TFmtStruct? value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmtStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TFmtStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault<TFmtStruct>(TFmtStruct? value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        else
        {
            this.AppendMatchFormattedOrNull(value, formatString, formatFlags);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true) 
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TFmtStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin<TFmtStruct>(TFmtStruct? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TFmtStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TFmtStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString<TFmtStruct>(TFmtStruct? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull(formatString, formatFlags);
        }
        else
        {
            if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            this.AppendMatchFormattedOrNull(value, formatString, formatFlags);
            if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        }
        return StyleTypeBuilder;
    }
    
    public TVMold FieldStringRevealOrDefaultNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloaked?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCloaked?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, string defaultValue = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, "", formatFlags: formatFlags);
            }
        }
        else
        {
            StyleFormatter.FormatFieldContents(Master,  value, palantírReveal);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringRevealOrNullNext<TCloaked, TRevealBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCloaked?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, palantírReveal, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloaked?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCloaked?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinString(value, palantírReveal, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString<TCloaked, TRevealBase>(TCloaked? value
      , PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) Sb.Append("\"");
                if(addEndDblQt) Sb.Append("\"");
                return StyleTypeBuilder;
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            StyleFormatter.FormatFieldContents(Master,  value, palantírReveal);
            if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        }
        return StyleTypeBuilder;
    }
    
    public TVMold FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true) 
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloakedStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCloakedStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinStringWithDefault(value, palantírReveal, defaultValue, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                StyleFormatter.FormatFieldContents(Sb,defaultValue, 0, "", formatFlags: formatFlags);
            }
        }
        else
        {
            StyleFormatter.FormatFieldContents(Master,  value.Value, palantírReveal);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringRevealOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true) where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCloakedStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, palantírReveal, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCloakedStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCloakedStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinString(value, palantírReveal, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCloakedStruct : struct
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            StyleFormatter.FormatFieldContents(Master, value.Value, palantírReveal);
            if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        }
        return StyleTypeBuilder;
    }
    
    public TVMold FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, defaultValue, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin<TBearer>(TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearer?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TBearer?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinStringWithDefault(value, defaultValue, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault<TBearer>(TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                StyleFormatter.FormatFieldContents(Sb,defaultValue, 0, "", formatFlags: formatFlags);
            }
        }
        else
        {
            StyleFormatter.FormatFieldContents(Master,  value);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }
    
    public TVMold FieldStringRevealOrDefaultNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, defaultValue, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearerStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TBearerStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinStringWithDefault(value, defaultValue, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                StyleFormatter.FormatFieldContents(Sb,defaultValue, 0, "", formatFlags: formatFlags);
            }
        }
        else
        {
            StyleFormatter.FormatFieldContents(Master,  value.Value);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TBearer?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin<TBearer>(TBearer? value, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearer : IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearer?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TBearer?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinString(value, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString<TBearer>(TBearer? value, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false) where TBearer : IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) Sb.Append("\"");
                if(addEndDblQt) Sb.Append("\"");
                return StyleTypeBuilder;
            }
            AppendNull( "", formatFlags);
        }
        else
        {
            if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            StyleFormatter.FormatFieldContents(Master, value);
            if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldStringRevealOrNullNext<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true) 
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TBearerStruct?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin<TBearerStruct>(TBearerStruct? value, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TBearerStruct : struct, IStringBearer
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TBearerStruct?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TBearerStruct?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags(Sb, value, formatFlags);
        return VettedJoinString(value, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString<TBearerStruct>(TBearerStruct? value, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false) where TBearerStruct : struct, IStringBearer
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull("", formatFlags);
        }
        else
        {
            if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            StyleFormatter.FormatFieldContents(Master, value.Value);
            if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<Memory<char>?>(value.Length > 0 ? typeof(Span<char>) : null, nonJsonfieldName, formatFlags)) 
            return WasSkipped<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin(Span<char> value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<Memory<char>?>(value.Length > 0 ? typeof(Span<char>) : null, "", formatFlags)) 
            return WasSkipped<Memory<char>>(value.Length > 0 ? typeof(Span<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags("Span", "", formatString));
        return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString(Span<char> value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder;
        }
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: formatFlags);
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", ""));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin(ReadOnlySpan<char> value, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", ""));
        return VettedJoinString(value, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString(ReadOnlySpan<char> value, FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull("", formatFlags);
            return StyleTypeBuilder;
        }
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        StyleFormatter.FormatFieldContents(Sb, value, 0, "", formatFlags: formatFlags);
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags("Span", defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "Span", formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags("Span", defaultValue, formatString));
        return VettedJoinStringWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault(ReadOnlySpan<char> value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value.Length == 0)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: formatFlags);
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin(ReadOnlySpan<char> value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<ReadOnlyMemory<char>?>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags)) 
            return WasSkipped<ReadOnlyMemory<char>>(value.Length > 0 ? typeof(ReadOnlySpan<char>) : null, "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, "ReadOnlySpan", formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags("ReadOnlySpan", "", formatString));
        return VettedJoinString(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString(ReadOnlySpan<char> value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value.Length == 0)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder;
        }
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        StyleFormatter.FormatFieldContents(Sb, value, 0, formatString, formatFlags: formatFlags);
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<string?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<string?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin(string? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<string?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<string?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString(string? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags);
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                        StyleFormatter.FormatFieldContents( Sb, "",0, formatString, formatFlags: formatFlags);
                        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                        return StyleTypeBuilder;
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<string?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<string?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<string?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<string?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault(string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); 
            }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin(char[]? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<char[]?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<char[]?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString(char[]? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags);
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                        StyleFormatter.FormatFieldContents(Sb, ((ReadOnlySpan<char>)""),0, formatString, formatFlags: formatFlags);
                        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                        return StyleTypeBuilder;
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<char[]?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin(char[]? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<char[]?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<char[]?>(value?.GetType(), "" , formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault(char[]? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) 
            { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrDefaultNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCharSeq?>(value?.GetType(), nonJsonfieldName , formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin<TCharSeq>(TCharSeq? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCharSeq?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCharSeq?>(value?.GetType(), "" , formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags  | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault<TCharSeq>(TCharSeq? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) 
            { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrNullNext<TCharSeq>(ReadOnlySpan<char> nonJsonfieldName, TCharSeq? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCharSeq?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TCharSeq?>(value?.GetType(), nonJsonfieldName , formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags   | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TCharSeq?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TCharSeq?>(value?.GetType(), "" , formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        
        return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString<TCharSeq>(TCharSeq? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
        where TCharSeq : ICharSequence
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags);
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                        StyleFormatter.FormatFieldContents( Sb, "",0, formatString, formatFlags: formatFlags);
                        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                        return StyleTypeBuilder;
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringWithDefaultJoin(StringBuilder? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<StringBuilder?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<StringBuilder?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags  | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        return VettedJoinStringWithDefault(value, startIndex, length, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringWithDefault(StringBuilder? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) 
            { StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags); }
            else
            {
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<StringBuilder?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags  | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringJoin(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<StringBuilder?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<StringBuilder?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags  | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        return VettedJoinString(value, startIndex, length, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinString(StringBuilder? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                StyleFormatter.FormatFieldContents(Sb, value, capStart, formatString, capLength, formatFlags: formatFlags);
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
            }
            else
            {
                if (formatString.Length > 0)
                {
                    var prefixSuffixLength = ((ReadOnlySpan<char>)formatString).PrefixSuffixLength();
                    if (prefixSuffixLength > 0)
                    {
                        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                        StyleFormatter.FormatFieldContents( Sb, "",0, formatString, formatFlags: formatFlags);
                        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                        return StyleTypeBuilder;
                    }
                }
                if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
                AppendNull(formatString, formatFlags);
            }
        }
        else
        {
            if (formatFlags.HasNullBecomesEmptyFlag()) return StyleTypeBuilder;
            AppendNull(formatString, formatFlags);
        }
        return StyleTypeBuilder;
    }

    public TVMold StringMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TAny?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TAny?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringMatchJoin(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringMatchJoin<TAny>(TAny? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TAny?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TAny?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, "", formatString));
        return VettedJoinStringMatchJoin(value, formatString, formatFlags, addStartDblQt, addEndDblQt);
    }
    
    public TVMold VettedJoinStringMatchJoin<TAny>(TAny? value, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags
      , bool addStartDblQt = false, bool addEndDblQt = false)
    {
        if (value == null)
        {
            if (formatFlags.HasNullBecomesEmptyFlag())
            {
                if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                StyleFormatter.FormatFieldContents( Sb, "",0, formatString, formatFlags: formatFlags | DisableAutoDelimiting);
                if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
                return StyleTypeBuilder;
            }
            AppendNull(formatString, formatFlags);
            return StyleTypeBuilder;
        }
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        this.AppendMatchFormattedOrNull(value, formatString, DisableAutoDelimiting | formatFlags);
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    public TVMold StringMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = true, bool addEndDblQt = true)
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TAny?>(value?.GetType(), nonJsonfieldName, formatFlags)) 
            return WasSkipped<TAny?>(value?.GetType(), nonJsonfieldName, formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        if(ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        VettedJoinStringMatchWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt);
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold JoinStringMatchWithDefaultJoin<TAny>(TAny? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags , bool addStartDblQt = false, bool addEndDblQt = false) 
    {
        var callContext = Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip || SkipField<TAny?>(value?.GetType(), "", formatFlags)) 
            return WasSkipped<TAny?>(value?.GetType(), "", formatFlags);
        formatFlags = StyleFormatter.ResolveContentFormattingFlags
            (Sb, value, formatFlags | StyleFormatter.ResolveContentAsStringFormattingFlags(value, defaultValue, formatString));
        return VettedJoinStringMatchWithDefault(value, defaultValue, formatString, formatFlags, addStartDblQt, addEndDblQt );
    }
    
    public TVMold VettedJoinStringMatchWithDefault<TAny>(TAny? value, ReadOnlySpan<char> defaultValue, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags, bool addStartDblQt = false, bool addEndDblQt = false) 
    {
        if(addStartDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        if (value != null)
        {
            this.AppendMatchFormattedOrNull(value, formatString, FieldContentHandling.DisableAutoDelimiting | formatFlags);
        }
        else
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                StyleFormatter.FormatFieldContents(Sb, defaultValue, 0, formatString, formatFlags: formatFlags);
            }
        }
        if(addEndDblQt) StyleFormatter.GraphBuilder.AppendContent("\"");
        return StyleTypeBuilder;
    }

    protected void AppendNull(string formatString, FieldContentHandling formatFlags)
    {
        StyleFormatter.AppendFormattedNull(Sb, formatString, formatFlags);
    }

    public TVMold ConditionalValueTypeSuffix()
    {
        if (ValueInComplexType) { this.AddGoToNext(); }
        return StyleTypeBuilder;
    }
}
