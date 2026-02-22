// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ComplexContentTypeMold : ContentTypeMold<ComplexContentTypeMold, ContentWithLogOnlyMold>
{

    public ComplexContentTypeMold InitializeComplexValueTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType  
      , FormatFlags createFormatFlags )
    {
        InitializeContentTypeBuilder(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                                 , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);

        return this;
    }
    
    // public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    // {
    //   if (Mws.CurrentWriteMethod.SupportsMultipleFields())
    //     usingFormatter.StartComplexTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags |  Mws.CreateMoldFormatFlags);
    //   else
    //     usingFormatter.StartSimpleTypeOpening(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod, formatFlags | Mws.CreateMoldFormatFlags);
    // }

    public override void FinishTypeOpening(FormatFlags formatFlags)
    {
      if (Mws.CurrentWriteMethod.SupportsMultipleFields())
      {
        Mws.Master.UpdateVisitWriteMethod(MoldVisit.VisitId, Mws.CurrentWriteMethod);  
      }
      base.FinishTypeOpening(formatFlags);
    }

    // public override void AppendClosing()
    // {
    //   var sf = Mws.Sf;
    //   if (Mws.CurrentWriteMethod.SupportsMultipleFields())
    //   {
    //     sf.AppendComplexTypeClosing(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod);
    //   }
    //   else
    //   {
    //     sf.AppendSimpleTypeClosing(Mws.InstanceOrType, Mws, Mws.CurrentWriteMethod);
    //   }
    // }
    
    public override bool IsComplexType => Mws.IsLog;
    public override bool IsSimpleMold => false;

    protected override void SourceBuilderComponentAccess(WrittenAsFlags writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<ContentTypeWriteState<ComplexContentTypeMold, ContentWithLogOnlyMold>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, writeMethod);
    }
    
    protected override void InheritedStateReset()
    {
        MoldStateField.DecrementIndent();
        MoldStateField = null!;
    }

    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value ?? defaultValue, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , TFmtStruct defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value ?? defaultValue, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValue<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValueOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValueOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName
      , TCloaked value, PalantírReveal<TCloakedBase> palantírReveal, string? defaultValue, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue ?? "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValue<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, null, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsValueOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, null, formatFlags, formatString ?? "");

    public ContentWithLogOnlyMold RevealAsValueOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? defaultValue, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue ?? "", formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsValue<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TBearer : IStringBearer? =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, null, formatFlags, formatString ?? "");

    public ContentWithLogOnlyMold RevealAsValueOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer? =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, null, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsValueOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string? defaultValue = "", string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer? =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");

    public ContentWithLogOnlyMold RevealAsValue<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TBearerStruct : struct, IStringBearer =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, null, formatFlags, formatString ?? "");

    public ContentWithLogOnlyMold RevealAsValueOrNull<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearerStruct : struct, IStringBearer =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, null, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsValueOrDefault<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string defaultValue = "", string? formatString = null, FormatFlags formatFlags = AsValueContent) 
        where TBearerStruct : struct, IStringBearer =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", false, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrZero(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", false, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, "", true, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , ReadOnlySpan<char> defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, false, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", false, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, "", true, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrZero(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", false, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , ReadOnlySpan<char> defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, false, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, string value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", false, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
      , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, value?.Length ?? 0, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Mws.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Mws.ValueMatchOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Mws.ValueMatchOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string? defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Mws.ValueMatchOrDefaultNext(nonJsonfieldName, value, defaultValue ?? "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, bool value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);
    
    
    public ContentWithLogOnlyMold AsStringOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TFmt : ISpanFormattable =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value ?? defaultValue, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value, string defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TFmtStruct : struct, ISpanFormattable =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , TFmtStruct defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value ?? defaultValue, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , string defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold RevealAsString<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
    , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase?
      where TCloakedBase : notnull =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
    , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName
    , TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold RevealAsString<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TCloakedStruct : struct =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName
    , TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsString<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer? =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);
    
  
    public ContentWithLogOnlyMold RevealAsStringOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer? =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer? => Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold RevealAsString<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatFlags, formatString ?? "");
    
    
    public ContentWithLogOnlyMold RevealAsStringOrNull<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, null, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsStringOrDefault<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
    , TBearerStruct? value, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Mws.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");
    
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, "", false, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, false, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, "", true, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, "", false, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, false, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, "", true, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, string value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, value?.Length ?? int.MaxValue, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, value?.Length ?? int.MaxValue, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
    , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, value?.Length ?? int.MaxValue, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0
    , int length = int.MaxValue , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) => 
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
  
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, null, formatString ?? "", formatFlags);


    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.StringMatchOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.StringMatchOrDefaultNext(nonJsonfieldName, value, null, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Mws.StringMatchOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
}
