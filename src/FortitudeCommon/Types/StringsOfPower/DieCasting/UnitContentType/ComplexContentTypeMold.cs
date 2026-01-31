// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

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
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags )
    {
        InitializeContentTypeBuilder(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                                 , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);

        return this;
    }
    
    public override void StartFormattingTypeOpening()
    {
      if (Msf.CurrentWriteMethod == WriteMethodType.MoldComplexContentType)
        MoldStateField.StyleFormatter.StartComplexTypeOpening(MoldStateField, Msf.CreateMoldFormatFlags);
      else
        MoldStateField.StyleFormatter.StartContentTypeOpening(MoldStateField);
    }

    public override void FinishTypeOpening()
    {
      if (Msf.CurrentWriteMethod != WriteMethodType.MoldComplexContentType)
      {
        Msf.Master.UpdateVisitWriteMethod(MoldVisit.RegistryId, MoldVisit.CurrentVisitIndex, Msf.CurrentWriteMethod);  
      }
      base.FinishTypeOpening();
    }

    public override void AppendClosing()
    {
      var sf = Msf.Sf;
      if (IsComplexType)
      {
        sf.AppendComplexTypeClosing(MoldStateField);
      }
      else
      {
        sf.AppendContentTypeClosing(MoldStateField);
      }
    }
    
    public override bool IsComplexType => Msf.IsLog;
    public override bool IsSimpleMold => false;

    protected override void SourceBuilderComponentAccess(WriteMethodType writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<ContentTypeDieCast<ComplexContentTypeMold, ContentWithLogOnlyMold>>()
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
        Msf.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldFmtValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldFmtValueOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , TFmtStruct defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValue<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValueOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValueOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName
      , TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValue<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsValueOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold RevealAsValueOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsValue<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TBearer : IStringBearer =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public ContentWithLogOnlyMold RevealAsValueOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsValueOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string defaultValue = "", string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");

    public ContentWithLogOnlyMold RevealAsValue<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public ContentWithLogOnlyMold RevealAsValueOrNull<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsValueOrDefault<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string defaultValue = "", string? formatString = null, FormatFlags formatFlags = AsValueContent) 
        where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrZero(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , ReadOnlySpan<char> defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrZero(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , ReadOnlySpan<char> defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, string value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
      , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, value?.Length ?? 0, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsValueMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string? defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrDefaultNext(nonJsonfieldName, value, defaultValue ?? "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, bool value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    
    public ContentWithLogOnlyMold AsStringOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TFmt : ISpanFormattable =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value ?? defaultValue, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value, string defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , TFmtStruct defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , string defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold RevealAsString<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
    , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
    , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName
    , TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold RevealAsString<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName
    , TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold RevealAsString<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);
    
  
    public ContentWithLogOnlyMold RevealAsStringOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer =>
      Msf.FieldStringRevealOrNullNext(nonJsonfieldName, value, formatFlags);
    
    public ContentWithLogOnlyMold RevealAsStringOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer => Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold RevealAsString<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatFlags, formatString ?? "");
    
    
    public ContentWithLogOnlyMold RevealAsStringOrNull<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");
    
    public ContentWithLogOnlyMold RevealAsStringOrDefault<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
    , TBearerStruct? value, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");
    
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, string value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
    , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0
    , int length = int.MaxValue , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) => 
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
  
    public ContentWithLogOnlyMold AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);


    public ContentWithLogOnlyMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentWithLogOnlyMold AsStringMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentWithLogOnlyMold AsStringMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
}
