// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;

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

    public virtual CompactLogTypeFormatting Initialize(StyleOptions styleOptions)
    {
        Options = styleOptions;

        return this;
    }

    public virtual string Name => nameof(CompactLogTypeFormatting);

    public virtual ITypeMolderDieCast<TMold> AppendValueTypeOpening<TMold>(ITypeMolderDieCast<TMold> typeMold
      , Type valueType, string? alternativeName) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        if (alternativeName != null)
            sb.Append(alternativeName);
        else
            valueType.AppendShortNameInCSharpFormat(sb);
        if (valueType.IsEnum)
        {
            sb.Append(Dot);
            return typeMold;
        }
        return sb.Append(Spc).Append(BrcOpnSpc).ToInternalTypeBuilder(typeMold);
    }

    public virtual ITypeMolderDieCast<TMold> AppendValueTypeClosing<TMold>(ITypeMolderDieCast<TMold> typeMold, Type valueType)
        where TMold : TypeMolder
    {
        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        if (valueType.IsEnum) return typeMold;
        return typeMold.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeMold);
    }

    public virtual ITypeMolderDieCast<TMold> AppendComplexTypeOpening<TMold>(ITypeMolderDieCast<TMold> typeMold, Type complexType
      , string? alternativeName = null) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        if (alternativeName != null)
            sb.Append(alternativeName);
        else
            complexType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        return typeMold.Sb.Append(BrcOpnSpc).ToInternalTypeBuilder(typeMold);
    }

    public virtual ITypeMolderDieCast<TMold> AppendFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, ReadOnlySpan<char> fieldName)
        where TMold : TypeMolder => typeMold.Sb.Append(fieldName).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> AppendFieldValueSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold)
        where TMold : TypeMolder => typeMold.Sb.Append(ClnSpc).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> AddNextFieldSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold)
        where TMold : TypeMolder => typeMold.Sb.Append(CmaSpc).ToInternalTypeBuilder(typeMold);

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel) =>
        sb.InsertAt(CmaSpc, atIndex).ReturnCharCount(2);

    public virtual ITypeMolderDieCast<TMold> AppendTypeClosing<TMold>(ITypeMolderDieCast<TMold> typeMold)
        where TMold : TypeMolder
    {
        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        return typeMold.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeMold);
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyedCollectionStart<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType, Type valueType) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        keyedCollectionType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        return typeMold.Sb.Append(BrcOpnSpc).ToInternalTypeBuilder(typeMold);
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyedCollectionEnd<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType , Type valueType, int totalItemCount) where TMold : TypeMolder =>
        typeMold.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null) where TMold : TypeMolder
    {
        _ = keyFormatString.IsNotNullOrEmpty()
            ? typeMold.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : typeMold.AppendMatchOrNull(key, true).FieldEnd();
        _ = valueFormatString.IsNotNullOrEmpty()
            ? typeMold.AppendMatchFormattedOrNull(value, valueFormatString)
            : typeMold.AppendMatchOrNull(value);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, string? keyFormatString = null)
        where TMold : TypeMolder where TValue : TVBase
    {
        _ = keyFormatString.IsNotNullOrEmpty()
            ? typeMold.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : typeMold.AppendMatchOrNull(key, true).FieldEnd();
        valueStyler(value, typeMold.Master);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler)
        where TMold : TypeMolder where TKey : TKBase where TValue : TVBase
    {
        keyStyler(key, typeMold.Master);
        typeMold.FieldEnd();
        valueStyler(value, typeMold.Master);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyedCollectionNextItem<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber) where TMold : TypeMolder
    {
        typeMold.Sb.Append(CmaSpc).ToInternalTypeBuilder(typeMold);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatCollectionStart<TMold>(ITypeMolderDieCast<TMold> typeMold
      , Type itemElementType, bool hasItems, Type collectionType) where TMold : TypeMolder
    {
        if (!(collectionType.FullName?.StartsWith("System") ?? true))
        {
            var sb = typeMold.Sb;
            collectionType.AppendShortNameInCSharpFormat(sb);
            sb.Append(Spc);
        }
        return base.CollectionStart(itemElementType, typeMold.Sb, hasItems).ToInternalTypeBuilder(typeMold);
    }

    public virtual ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold
      , TCloaked item, int retrieveCount, PalantírReveal<TCloakedBase> styler) where TMold : TypeMolder where TCloaked : TCloakedBase
    {
        styler(item, typeMold.Master);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold>(ITypeMolderDieCast<TMold> typeMold, string? item, int retrieveCount
      , string? formatString = null) where TMold : TypeMolder
    {
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeMold.Sb.AppendFormat(this, formatString, item);
        else
            typeMold.Sb.Append(item, this);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold, TCharSeq>(ITypeMolderDieCast<TMold> typeMold, TCharSeq? item
      , int retrieveCount, string? formatString = null) where TMold : TypeMolder where TCharSeq : ICharSequence
    {
        if (item == null)
        {
            typeMold.Sb.Append(typeMold.Settings.NullStyle);
            return typeMold;
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeMold.Sb.Append(item, 0, item.Length, formatString, this);
        else
            typeMold.Sb.Append(item, this);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold>(ITypeMolderDieCast<TMold> typeMold, StringBuilder? item
      , int retrieveCount, string? formatString = null) where TMold : TypeMolder
    {
        if (item == null)
        {
            typeMold.Sb.Append(typeMold.Settings.NullStyle);
            return typeMold;
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeMold.Sb.Append(item, 0, item.Length, formatString, this);
        else
            typeMold.Sb.Append(item, this);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold>(ITypeMolderDieCast<TMold> typeMold, IStringBearer? item
      , int retrieveCount) where TMold : TypeMolder
    {
        if (item == null)
        {
            typeMold.Sb.Append(typeMold.Settings.NullStyle);
            return typeMold;
        }
        item.RevealState(typeMold.Master);
        return typeMold;
    }

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source
      , string? formatString = null) =>
        formatString.IsNotNullOrEmpty()
            ? sb.AppendFormat(this, formatString, source)
            : sb.Append(source);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, bool source
      , string? formatString = null) where TMold : TypeMolder =>
        typeMold.Sb.Append(source ? Options.True : Options.False).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, bool? source
      , string? formatString = null) where TMold : TypeMolder =>
        (source != null
            ? typeMold.Sb.Append(source.Value ? Options.True : Options.False)
            : typeMold.Sb.Append(typeMold.Settings.NullStyle)).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source
      , string? formatString = null) where TMold : TypeMolder where TFmt : ISpanFormattable =>
        base.Format(source, typeMold.Sb, formatString).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source
      , string? formatString = null) where TMold : TypeMolder where TFmt : struct, ISpanFormattable =>
        base.Format(source, typeMold.Sb, formatString).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue) where TMold : TypeMolder =>
        base.Format(source, sourceFrom, typeMold.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue) where TMold : TypeMolder =>
        base.Format(source, sourceFrom, typeMold.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue) where TMold : TypeMolder =>
        base.Format(source, sourceFrom, typeMold.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue) where TMold : TypeMolder =>
        base.Format(source, sourceFrom, typeMold.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler) where TMold : TypeMolder where TCloaked : TCloakedBase =>
        styler(toStyle, typeMold.Master).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, IStringBearer styledObj)
        where TMold : TypeMolder =>
        styledObj.RevealState(typeMold.Master).ToInternalTypeBuilder(typeMold);

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source
      , string? formatString = null) =>
        formatString.IsNotNullOrEmpty()
            ? sb.AppendFormat(this, formatString, source)
            : sb.Append(source);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, bool source
      , string? formatString = null) where TMold : TypeMolder =>
        typeMold.Sb.Append(source ? Options.True : Options.False).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, bool? source
      , string? formatString = null) where TMold : TypeMolder =>
        (source != null
            ? typeMold.Sb.Append(source.Value ? Options.True : Options.False)
            : typeMold.Sb.Append(typeMold.Settings.NullStyle)).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source
      , string? formatString = null) where TMold : TypeMolder where TFmt : ISpanFormattable =>
        base.Format(source, typeMold.Sb, formatString).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source
      , string? formatString = null) where TMold : TypeMolder where TFmt : struct, ISpanFormattable =>
        base.Format(source, typeMold.Sb, formatString).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue) where TMold : TypeMolder =>
        base.Format(source, sourceFrom, typeMold.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue) where TMold : TypeMolder =>
        base.Format(source, sourceFrom, typeMold.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue) where TMold : TypeMolder =>
        base.Format(source, sourceFrom, typeMold.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue) where TMold : TypeMolder =>
        base.Format(source, sourceFrom, typeMold.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler) where TMold : TypeMolder where TCloaked : TCloakedBase =>
        styler(toStyle, typeMold.Master).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold
      , IStringBearer styledObj) where TMold : TypeMolder =>
        styledObj.RevealState(typeMold.Master).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> AddCollectionElementSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold
      , Type elementType, int nextItemNumber) where TMold : TypeMolder =>
        (elementType == typeof(byte)
            ? typeMold.Sb.Append(nextItemNumber % 4 == 0 ? Spc : "")
            : typeMold.Sb.Append(CmaSpc)).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatCollectionEnd<TMold>(ITypeMolderDieCast<TMold> typeMold, Type itemElementType
      , int totalItemCount) where TMold : TypeMolder
    {
        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        return base.CollectionEnd(itemElementType, typeMold.Sb, totalItemCount).ToInternalTypeBuilder(typeMold);
    }

    public virtual ITypeMolderDieCast<TMold> FormatCollectionNext<TMold, TItem>(ITypeMolderDieCast<TMold> typeMold
      , TItem toFormat, int itemAt, string? formatString = null) where TMold : TypeMolder where TItem : ISpanFormattable =>
        base.CollectionNextItem(toFormat, itemAt, typeMold.Sb).ToInternalTypeBuilder(typeMold);
}
