// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class ComplexContentTypeMold : ContentTypeMold<ComplexContentTypeMold>
{
    private ComplexType.CollectionField.SelectTypeCollectionField<ComplexContentTypeMold>? logOnlyInternalCollectionField;
    private SelectTypeField<ComplexContentTypeMold>?                                       logOnlyInternalField;

    public ComplexContentTypeMold InitializeComplexValueTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , IStyledTypeFormatting typeFormatting
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags )
    {
        InitializeContentTypeBuilder(instanceOrContainer, typeBeingBuilt, master, typeName
                                 , remainingGraphDepth, moldGraphVisit, typeFormatting, writeMethodType, createFormatFlags);

        return this;
    }
    
    public override void StartFormattingTypeOpening()
    {
      if (IsComplexType)
        MoldStateField.StyleFormatter.StartComplexTypeOpening(MoldStateField);
      else
        MoldStateField.StyleFormatter.StartContentTypeOpening(MoldStateField);
    }
    
    public override void AppendClosing()
    {
      var formatter = MoldStateField.StyleFormatter;
      if (IsComplexType)
      {
        formatter.AppendComplexTypeClosing(MoldStateField);
      }
      else
      {
        formatter.AppendContentTypeClosing(MoldStateField);
      }
    }
    
    public override bool IsComplexType => Msf.IsLog;

    protected override void SourceBuilderComponentAccess(WriteMethodType writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<ContentTypeDieCast<ComplexContentTypeMold>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, writeMethod);
    }
    
    public SelectTypeField<ComplexContentTypeMold> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState.Master.Recycler
                         .Borrow<SelectTypeField<ComplexContentTypeMold>>().Initialize(MoldStateField);

    public ComplexType.CollectionField.SelectTypeCollectionField<ComplexContentTypeMold> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState.Master.Recycler
                         .Borrow<ComplexType.CollectionField.SelectTypeCollectionField<ComplexContentTypeMold>>().Initialize(MoldStateField);

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        MoldStateField.DecrementIndent();
        MoldStateField = null!;
    }

    public ComplexContentTypeMold AddBaseFieldsStart()
    {
        MoldStateField.Master.AddBaseFieldsStart();

        return Me;
    }
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldFmtValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldFmtValueOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , TFmtStruct defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , string defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValue<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValueOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValueOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName
      , TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValue<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValueOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValueOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValue<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TBearer : IStringBearer =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValueOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValueOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string defaultValue = "", string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");

    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValue<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValueOrNull<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsValueOrDefault<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , string defaultValue = "", string? formatString = null, FormatFlags formatFlags = AsValueContent) 
        where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrZero(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , ReadOnlySpan<char> defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrZero(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , ReadOnlySpan<char> defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, string value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
      , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, value?.Length ?? 0, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
      , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
      , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsValueMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string? defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrDefaultNext(nonJsonfieldName, value, defaultValue ?? "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, bool value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsString<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TFmt : ISpanFormattable =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value ?? defaultValue, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value, string defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , TFmtStruct defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
    , string defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsString<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
    , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatString, formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsStringOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
    , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal, formatString, formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsStringOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName
    , TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString, formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsString<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsStringOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal, formatString, formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsStringOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName
    , TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsString<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);
    
  
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsStringOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer =>
      Msf.FieldStringRevealOrNullNext(nonJsonfieldName, value, formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsStringOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer => Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsString<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatFlags, formatString ?? "");
    
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsStringOrNull<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<ComplexContentTypeMold> RevealAsStringOrDefault<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName
    , TBearerStruct? value, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");
    
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, string value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex
    , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0
    , int length = int.MaxValue , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) => 
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
  
    public ContentJoinTypeMold<ComplexContentTypeMold> AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);


    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0
    , int length = int.MaxValue, string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<ComplexContentTypeMold> AsStringMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);
}
