using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class SimpleContentJoinMold : ContentJoinTypeMold<SimpleContentTypeMold, SimpleContentJoinMold>;

public class SimpleContentTypeMold : ContentTypeMold<SimpleContentTypeMold, SimpleContentJoinMold>
{
    public SimpleContentTypeMold InitializeSimpleValueTypeBuilder
        (
            object instanceOrContainer
          , Type typeBeingBuilt
          , ISecretStringOfPower master
          , Type typeVisitedAs
          , string? typeName
          , int remainingGraphDepth
          , VisitResult moldGraphVisit
          , WriteMethodType writeMethodType  
          , FormatFlags createFormatFlags)
    {
        InitializeContentTypeBuilder(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                                 , remainingGraphDepth,  moldGraphVisit, writeMethodType, createFormatFlags);

        return this;
    }

    protected override void SourceBuilderComponentAccess(WriteMethodType writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<ContentTypeDieCast<SimpleContentTypeMold, SimpleContentJoinMold>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, writeMethod);
    }

    public override void StartFormattingTypeOpening()
    {
      if (Msf.CurrentWriteMethod.SupportsMultipleFields())
        Msf.StyleFormatter.StartComplexTypeOpening(Msf, Msf.CreateMoldFormatFlags);
      else
        Msf.StyleFormatter.StartContentTypeOpening(Msf, Msf.CreateMoldFormatFlags);
    }
    
    public override void FinishTypeOpening()
    {
      if (Msf.CurrentWriteMethod != WriteMethodType.MoldSimpleContentType)
      {
        Msf.Master.UpdateVisitWriteMethod(MoldVisit.RegistryId, MoldVisit.CurrentVisitIndex, Msf.CurrentWriteMethod);  
      }
      MoldStateField.StyleFormatter.FinishContentTypeOpening(MoldStateField);
    }

    public override void AppendClosing()
    {
      var sf = Msf.Sf;
      if (Msf.CurrentWriteMethod.SupportsMultipleFields())
      {
        sf.AppendComplexTypeClosing(MoldStateField);
      }
      else
      {
        sf.AppendContentTypeClosing(MoldStateField);
      }
    }
    
    public SimpleContentJoinMold  AsValue(bool value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueNext("", value, formatString ?? ""
                         , formatFlags | Msf.CreateMoldFormatFlags );
    
    public SimpleContentJoinMold  AsValueOrNull(bool? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) =>
        Msf.FieldValueNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValue<TFmt>(TFmt value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrNull<TFmt>(TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TFmt : ISpanFormattable? =>
        Msf.FieldFmtValueOrNullNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrDefault<TFmt>(TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrDefault<TFmt>(TFmt value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Msf.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrNull<TFmtStruct>(TFmtStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext("", value, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldFmtValueOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TFmtStruct : struct, ISpanFormattable =>
        Msf.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsValue<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  RevealAsValueOrNull<TCloaked, TCloakedBase>(TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  RevealAsValueOrDefault<TCloaked, TCloakedBase>(TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Msf.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsValue<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Msf.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  RevealAsValueOrNull<TCloakedStruct>(TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = AsValueContent) 
        where TCloakedStruct : struct =>
      Msf.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsValueOrDefault<TCloakedStruct>(TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
    , FormatFlags formatFlags = AsValueContent)
      where TCloakedStruct : struct =>
      Msf.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue, formatFlags, formatString ?? "");
    
    public SimpleContentJoinMold  RevealAsValue<TBearer>(TBearer value, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TBearer : IStringBearer =>
        Msf.FieldValueOrNullNext("", value, formatFlags, formatString ?? "");

    public SimpleContentJoinMold  RevealAsValueOrNull<TBearer>(TBearer? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Msf.FieldValueOrNullNext("", value, formatFlags, formatString ?? "");
    
    public SimpleContentJoinMold  RevealAsValueOrDefault<TBearer>(TBearer? value, string defaultValue = ""
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Msf.FieldValueOrDefaultNext("", value, defaultValue, formatFlags, formatString ?? "");

    public SimpleContentJoinMold  RevealAsValue<TBearerStruct>(TBearerStruct? value, string? formatString = null
      , FormatFlags formatFlags = AsValueContent)
        where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrNullNext("", value, formatFlags, formatString ?? "");

    public SimpleContentJoinMold  RevealAsValueOrNull<TBearerStruct>(TBearerStruct? value
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrNullNext("", value, formatFlags, formatString ?? "");
    
    public SimpleContentJoinMold  RevealAsValueOrDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , string? formatString = null, FormatFlags formatFlags = AsValueContent) where TBearerStruct : struct, IStringBearer =>
        Msf.FieldValueOrDefaultNext("", value, defaultValue, formatFlags, formatString ?? "");
    
    public SimpleContentJoinMold  AsValue(Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext( "", value, "0", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrZero(Span<char> value, string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrNull(Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrDefault(Span<char> value, ReadOnlySpan<char> defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValue(ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrNull(ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrZero(ReadOnlySpan<char> value, string? formatString
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrDefault(ReadOnlySpan<char> value
    , ReadOnlySpan<char> defaultValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValue(string value, FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public SimpleContentJoinMold  AsValue(string value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrNull(string? value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("" , value, startIndex, length, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrDefault(string? value, int startIndex, int length = int.MaxValue
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValue(char[] value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
      Msf.FieldValueOrDefaultNext("" , value, 0, value?.Length ?? 0, "0", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValue(char[] value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrNull(char[]? value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue
    , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValue(ICharSequence value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValue(ICharSequence value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrNull(ICharSequence? value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrDefault(ICharSequence? value, int startIndex, int length = int.MaxValue
    , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValue(StringBuilder value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValue(StringBuilder value, int startIndex, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsValueOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
    , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueMatch<TAny>(TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrNullNext("", value, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueMatchOrNull<TAny>(TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrNullNext("" , value, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsValueMatchOrDefault<TAny>(TAny? value, string? defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = AsValueContent) =>
      Msf.ValueMatchOrDefaultNext("" , value, defaultValue ?? "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsString(bool value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext("" , value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringOrNull(bool? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsString<TFmt>(TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) 
      where TFmt : ISpanFormattable =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringOrNull<TFmt>(TFmt value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringOrDefault<TFmt>(TFmt? value, TFmt defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable =>
      Msf.FieldStringOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrDefault<TFmt>(TFmt value, string defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) where TFmt : ISpanFormattable? =>
      Msf.FieldStringOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringOrNull<TFmtStruct>(TFmtStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) 
      where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TFmtStruct : struct, ISpanFormattable =>
      Msf.FieldStringOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  RevealAsString<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrDefaultNext("", value, palantírReveal, "", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsStringOrNull<TCloaked, TCloakedBase>(TCloaked? value
    , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsStringOrDefault<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
      Msf.FieldStringRevealOrDefaultNext("", value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsString<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrDefaultNext("", value, palantírReveal, "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  RevealAsStringOrNull<TCloakedStruct>(TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll)
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsStringOrDefault<TCloakedStruct>(TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) 
      where TCloakedStruct : struct =>
      Msf.FieldStringRevealOrDefaultNext("", value, palantírReveal, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsString<TBearer>(TBearer value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext("", value, "", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsStringOrNull<TBearer>(TBearer? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer =>
      Msf.FieldStringRevealOrNullNext("", value, formatFlags);

    public SimpleContentJoinMold  RevealAsStringOrDefault<TBearer>(TBearer? value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearer : IStringBearer => Msf.FieldStringRevealOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  RevealAsString<TBearerStruct>(TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext("", value, "", formatFlags, formatString ?? "");
    
    public SimpleContentJoinMold  RevealAsStringOrNull<TBearerStruct>(TBearerStruct? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrNullNext("", value, formatFlags, formatString ?? "");
    
    public SimpleContentJoinMold  RevealAsStringOrDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll)
      where TBearerStruct : struct, IStringBearer =>
      Msf.FieldStringRevealOrDefaultNext("", value, defaultValue, formatFlags, formatString ?? "");
    
    public SimpleContentJoinMold  AsString(Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrNull(Span<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringOrDefault(Span<char> value, string defaultValue = ""
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
    
    
    public SimpleContentJoinMold  AsString(ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrDefault(ReadOnlySpan<char> value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrNull(ReadOnlySpan<char> value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsString(string value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrNull(string? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsString(string value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrNull(string? value, int startIndex
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrDefault(string? value, int startIndex
    , int length = int.MaxValue, string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, defaultValue ?? "", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsString(char[] value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsString(char[] value, int startIndex = 0
    , int length = int.MaxValue , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) => 
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrNull(char[]? value, int startIndex = 0
    , int length = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, defaultValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsString(ICharSequence value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsString(ICharSequence value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringOrNull(ICharSequence? value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrDefault(ICharSequence? value, int startIndex = 0, int length = int.MaxValue
    , string fallbackValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsString(StringBuilder value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsString(StringBuilder value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
    , string fallbackValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringMatch<TAny>(TAny value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrNullNext("", value, formatString ?? "", formatFlags);
    
    public SimpleContentJoinMold  AsStringMatchOrNull<TAny>(TAny? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrNullNext("", value, formatString ?? "", formatFlags);

    public SimpleContentJoinMold  AsStringMatchOrDefault<TAny>(TAny? value
    , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = EncodeAll) =>
      Msf.StringMatchOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);
}