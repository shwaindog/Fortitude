// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandlingExtensions;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactLogTypeFormatting : DefaultStringFormatter, IStyledTypeFormatting
{
    protected const string Dot       = ".";
    protected const string CmaSpc    = ", ";
    protected const string Spc       = " ";
    protected const string ClnSpc    = ": ";
    protected const string BrcOpnSpc = "{ ";
    protected const string SpcBrcCls = " }";
    protected const string Eqls = "=";

    public virtual CompactLogTypeFormatting Initialize(StyleOptions styleOptions)
    {
        Options = styleOptions;

        return this;
    }

    public virtual string Name => nameof(CompactLogTypeFormatting);
    public StyleOptions StyleOptions => (StyleOptions)Options;

    public FieldContentHandling ResolveContentFormattingFlags<T>(IStringBuilder sb, T input, FieldContentHandling callerFormattingFlags
      , string? formatString = "", bool isFieldName = false)
    {
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags())
        {
            return callerFormattingFlags;
        }

        FieldContentHandling setFlags = callerFormattingFlags;
        setFlags |= (FieldContentHandling)base.ResolveStringFormattingFlags
            (sb.LastNonWhiteChar(), input, (FormattingHandlingFlags)setFlags, formatString ?? "");
        
        var typeofT = typeof(T);
        if (typeofT.IsAnyTypeHoldingChars())
        {
            setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() ? EnsureFormattedDelimited : None;
        }
        return setFlags;
    }

    public virtual IStringBuilder AppendValueTypeOpening(IStringBuilder sb, Type valueType, string? alternativeName)
    {
        if (alternativeName != null)
            sb.Append(alternativeName);
        else
            valueType.AppendShortNameInCSharpFormat(sb);
        if (valueType.IsEnum)
        {
            return sb.Append(Dot);
        }
        return sb.Append(Eqls);
    }

    public virtual IStringBuilder AppendValueTypeClosing(IStringBuilder sb, Type valueType)
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        return sb;
    }

    public virtual IStringBuilder AppendComplexTypeOpening(IStringBuilder sb, Type complexType, string? alternativeName = null)
    {
        if (alternativeName != null)
            sb.Append(alternativeName);
        else
            complexType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        return sb.Append(BrcOpnSpc);
    }

    public virtual IStringBuilder AppendFieldValueSeparator(IStringBuilder sb) => sb.Append(ClnSpc);

    public virtual IStringBuilder AddNextFieldSeparator(IStringBuilder sb) => sb.Append(CmaSpc);

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel) =>
        sb.InsertAt(CmaSpc, atIndex).ReturnCharCount(2);

    public virtual IStringBuilder AppendTypeClosing(IStringBuilder sb)
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        return sb.Append(SpcBrcCls);
    }

    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
    {
        keyedCollectionType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        return sb.Append(BrcOpnSpc).ToStringBuilder(sb);
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType , Type valueType, int totalItemCount, FieldContentHandling formatFlags = DefaultCallerTypeFlags) => sb.Append(SpcBrcCls);

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder where TValue : TVBase
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        valueStyler(value, typeMold.Master);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder where TKey : TKBase where TValue : TVBase
    {
        keyStyler(key, typeMold.Master);
        typeMold.FieldEnd();
        valueStyler(value, typeMold.Master);
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber) => sb.Append(CmaSpc);

    public virtual IStringBuilder FormatCollectionStart(IStringBuilder sb, Type itemElementType, bool hasItems, Type collectionType
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (!(collectionType.FullName?.StartsWith("System") ?? true))
        {
            collectionType.AppendShortNameInCSharpFormat(sb);
            sb.Append(Spc);
        }
        return base.CollectionStart(itemElementType, sb, hasItems).ToStringBuilder(sb);
    }

    public virtual IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ITheOneString tos
      , TCloaked item, int retrieveCount, PalantírReveal<TCloakedBase> styler) where TCloaked : TCloakedBase
    {
        styler(item, tos);
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        sb.AppendFormat(this, formatString ?? "", item);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        sb.AppendFormat(this, formatString ?? "", item);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat<TCharSeq>(IStringBuilder sb, TCharSeq? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (item == null)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            sb.Append(item, 0, item.Length, formatString, this);
        else
            sb.Append(item, this);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            sb.Append(item, 0, item.Length, formatString, this);
        else
            sb.Append(item, this);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat<TBearer>(ITheOneString tos, TBearer? item, int retrieveCount) where TBearer : IStringBearer 
    {
        if (item == null)
        {
            return tos.WriteBuffer.Append(StyleOptions.NullStyle);
        }
        item.RevealState(tos);
        return tos.WriteBuffer;
    }
    
    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName)=> sb.Append(fieldName);
    
    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        formatString.IsNotNullOrEmpty()
            ? sb.AppendFormat(this, formatString, source)
            : sb.Append(source);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)  => 
        Format(source, sb, formatString ?? "").ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)  =>
        (source != null
            ? Format(source, sb, formatString ?? "")
            : base.Format(StyleOptions.NullStyle, 0, sb, formatString)).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable =>
        base.Format(source, sb, formatString ?? "").ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TFmt : struct, ISpanFormattable => base.Format(source, sb, formatString ?? "").ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags)
            .ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler) where TCloaked : TCloakedBase =>
        styler(toStyle, tos).ToStringBuilder(tos.WriteBuffer);

    public virtual IStringBuilder FormatFieldName<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer =>
        styledObj.RevealState(tos).ToStringBuilder(tos.WriteBuffer);

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        sb.AppendFormat(this, formatString ?? "", source, formatFlags: (FormattingHandlingFlags)formatFlags);

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)  => 
        Format(source, sb, formatString ?? "", formatFlags: (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)  =>
        Format(source, sb, formatString ?? "", formatFlags: (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable =>
        base.Format(source, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        base.Format(source, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if(formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if(formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if(formatFlags.ShouldDelimit()) sb.Append(SqBrktOpnChar);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if(formatFlags.ShouldDelimit()) sb.Append(SqBrktClsChar);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if(formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if(formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if(formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if(formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler)  where TCloaked : TCloakedBase =>
        styler(toStyle, tos).ToStringBuilder(tos.WriteBuffer);

    public virtual IStringBuilder FormatFieldContents<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer  =>
        styledObj.RevealState(tos).ToStringBuilder(tos.WriteBuffer);

    public virtual IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)  =>
        (elementType == typeof(byte)
            ? sb.Append(nextItemNumber % 4 == 0 ? Spc : "")
            : sb.Append(CmaSpc));

    public virtual IStringBuilder FormatCollectionEnd(IStringBuilder sb, Type itemElementType
      , int totalItemCount, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        return base.CollectionEnd(itemElementType, sb, totalItemCount).ToStringBuilder(sb);
    }

    public virtual ITypeMolderDieCast<TMold> FormatCollectionNext<TMold, TItem>(ITypeMolderDieCast<TMold> typeMold
      , TItem toFormat, int itemAt, string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TMold : TypeMolder where TItem : ISpanFormattable =>
        base.CollectionNextItem(toFormat, itemAt, typeMold.Sb).ToInternalTypeBuilder(typeMold);
}
