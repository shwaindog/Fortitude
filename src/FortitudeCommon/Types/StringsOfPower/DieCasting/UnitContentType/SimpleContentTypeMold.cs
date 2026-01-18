using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class SimpleContentTypeMold : ContentTypeMold<SimpleContentTypeMold>
{
    public SimpleContentTypeMold InitializeSimpleValueTypeBuilder
        (
            object instanceOrContainer
          , Type typeBeingBuilt
          , ISecretStringOfPower master
          , MoldDieCastSettings typeSettings
          , string? typeName
          , int remainingGraphDepth
          , VisitResult moldGraphVisit
          , IStyledTypeFormatting typeFormatting  
          , WriteMethodType writeMethodType  
          , FormatFlags createFormatFlags)
    {
        InitializeContentTypeBuilder(instanceOrContainer, typeBeingBuilt, master, typeSettings, typeName
                                 , remainingGraphDepth,  moldGraphVisit, typeFormatting, writeMethodType, createFormatFlags);

        return this;
    }

    protected override void SourceBuilderComponentAccess(WriteMethodType writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<ContentTypeDieCast<SimpleContentTypeMold>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, writeMethod);
    }

    public override void StartFormattingTypeOpening()
    {
        MoldStateField.StyleFormatter.StartContentTypeOpening(MoldStateField);
    }

    public override void AppendClosing()
    {
        MoldStateField.StyleFormatter.AppendContentTypeClosing(MoldStateField);
    }
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(bool value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueNext("", value, formatString ?? ""
                         , formatFlags | Msf.CreateMoldFormatFlags );
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull(bool? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue<TFmt>(TFmt value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull<TFmt>(TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TFmt : ISpanFormattable? =>
        Msf.FieldFmtValueOrNullNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault<TFmt>(TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault<TFmt>(TFmt value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull<TFmtStruct>(TFmtStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext("", value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValue<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValueOrNull<TCloaked, TCloakedBase>(TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValueOrDefault<TCloaked, TCloakedBase>(TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValue<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Msf.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValueOrNull<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent) 
        where TCloakedStruct : struct =>
      Msf.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValueOrDefault<TCloakedStruct>(TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
    , FormatFlags formatFlags = AsValueContent)
      where TCloakedStruct : struct =>
      Msf.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValue<TBearer>(TBearer value, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TBearer : IStringBearer =>
        Msf.FieldValueOrNullNext("", value, formatFlags, formatString ?? "");

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValueOrNull<TBearer>(TBearer? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Msf.FieldValueOrNullNext("", value, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValueOrDefault<TBearer>(TBearer? value, string defaultValue = ""
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Msf.FieldValueOrDefaultNext("", value, defaultValue, formatFlags, formatString ?? "");

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValue<TBearerStruct>(TBearerStruct? value, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrNullNext("", value, formatFlags, formatString ?? "");

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValueOrNull<TBearerStruct>(TBearerStruct? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrNullNext("", value, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsValueOrDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrDefaultNext("", value, defaultValue, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext( "", value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrZero(Span<char> value, string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull(Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault(Span<char> value, ReadOnlySpan<char> defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull(ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrZero(ReadOnlySpan<char> value, string? formatString
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault(ReadOnlySpan<char> value
    , ReadOnlySpan<char> defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(string value, FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(string value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull(string? value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("" , value, startIndex, length, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault(string? value, int startIndex, int length = int.MaxValue
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(char[] value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
      Msf.FieldValueOrDefaultNext("" , value, 0, value?.Length ?? 0, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(char[] value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull(char[]? value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue
    , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(ICharSequence value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(ICharSequence value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull(ICharSequence? value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault(ICharSequence? value, int startIndex, int length = int.MaxValue
    , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(StringBuilder value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValue(StringBuilder value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
    , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueMatch<TAny>(TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrNullNext("", value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueMatchOrNull<TAny>(TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrNullNext("" , value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsValueMatchOrDefault<TAny>(TAny? value, string? defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrDefaultNext("" , value, defaultValue ?? "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(bool value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext("" , value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull(bool? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString<TFmt>(TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) 
      where TFmt : ISpanFormattable =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull<TFmt>(TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault<TFmt>(TFmt? value, TFmt defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable =>
      Msf.FieldStringOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault<TFmt>(TFmt value, string defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Msf.FieldStringOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull<TFmtStruct>(TFmtStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) 
      where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsString<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrDefaultNext("", value, palantírReveal, "", formatString, formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsStringOrNull<TCloaked, TCloakedBase>(TCloaked? value
    , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrNullNext("", value, palantírReveal, formatString, formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsStringOrDefault<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrDefaultNext("", value, palantírReveal, defaultValue, formatString, formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsString<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrDefaultNext("", value, palantírReveal, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsStringOrNull<TCloakedStruct>(TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrNullNext("", value, palantírReveal, formatString, formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsStringOrDefault<TCloakedStruct>(TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) 
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrDefaultNext("", value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsString<TBearer>(TBearer value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext("", value, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsStringOrNull<TBearer>(TBearer? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer =>
      Msf.FieldStringRevealOrNullNext("", value, formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsStringOrDefault<TBearer>(TBearer? value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer => Msf.FieldStringRevealOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsString<TBearerStruct>(TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext("", value, "", formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsStringOrNull<TBearerStruct>(TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrNullNext("", value, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<SimpleContentTypeMold> RevealAsStringOrDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext("", value, defaultValue, formatFlags, formatString ?? "");
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull(Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault(Span<char> value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault(ReadOnlySpan<char> value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull(ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(string value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull(string? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(string value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull(string? value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault(string? value, int startIndex
    , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, defaultValue ?? "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(char[] value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(char[] value, int startIndex = 0
    , int length = int.MaxValue , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) => 
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull(char[]? value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(ICharSequence value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(ICharSequence value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull(ICharSequence? value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault(ICharSequence? value, int startIndex = 0, int length = int.MaxValue
    , string fallbackValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(StringBuilder value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsString(StringBuilder value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
    , string fallbackValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringMatch<TAny>(TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrNullNext("", value, formatString ?? "", formatFlags);
    
    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringMatchOrNull<TAny>(TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrNullNext("", value, formatString ?? "", formatFlags);

    public ContentJoinTypeMold<SimpleContentTypeMold> AsStringMatchOrDefault<TAny>(TAny? value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
}