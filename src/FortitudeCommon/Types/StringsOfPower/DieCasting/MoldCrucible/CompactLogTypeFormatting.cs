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
    public virtual string Name => nameof(CompactLogTypeFormatting);

    public virtual ITypeMolderDieCast<TB> AppendValueTypeOpening<TB>(ITypeMolderDieCast<TB> typeBuilder
      , Type valueType, string? alternativeName) where TB : TypeMolder
    {
        var sb = typeBuilder.Sb;
        sb.Append(alternativeName ?? valueType.Name);
        if (valueType.IsEnum)
        {
            sb.Append(Dot);
            return typeBuilder;
        }
        return sb.Append(Spc).Append(BrcOpnSpc).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual ITypeMolderDieCast<TB> AppendValueTypeClosing<TB>(ITypeMolderDieCast<TB> typeBuilder, Type valueType) where TB : TypeMolder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        if (valueType.IsEnum)
        {
            return typeBuilder;
        }
        return typeBuilder.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual ITypeMolderDieCast<TB> AppendComplexTypeOpening<TB>(ITypeMolderDieCast<TB> typeBuilder, Type complexType
      , string? alternativeName = null)
        where TB : TypeMolder
    {
        typeBuilder.Sb.Append(alternativeName ?? complexType.Name).Append(Spc);
        return typeBuilder.Sb.Append(BrcOpnSpc).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual ITypeMolderDieCast<TB> AppendFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, string fieldName)
        where TB : TypeMolder =>
        typeBuilder.Sb.Append(fieldName).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> AppendFieldValueSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder)
        where TB : TypeMolder =>
        typeBuilder.Sb.Append(ClnSpc).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> AddNextFieldSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder)
        where TB : TypeMolder =>
        typeBuilder.Sb.Append(CmaSpc).ToInternalTypeBuilder(typeBuilder);

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel) 
    {
        return sb.InsertAt(CmaSpc, atIndex).ReturnCharCount(2);
    }

    public virtual ITypeMolderDieCast<TB> AppendTypeClosing<TB>(ITypeMolderDieCast<TB> typeBuilder)
        where TB : TypeMolder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        return typeBuilder.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeBuilder);
    }


    public virtual ITypeMolderDieCast<TB> AppendKeyedCollectionStart<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType, Type keyType
      , Type valueType) where TB : TypeMolder
    {
        typeBuilder.Sb.Append(keyedCollectionType.Name).Append(Spc);
        return typeBuilder.Sb.Append(BrcOpnSpc).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyedCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType, Type keyType
      , Type valueType, int totalItemCount) where TB : TypeMolder =>
        typeBuilder.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType, TKey key
      , TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null) where TB : TypeMolder
    {
        _ = keyFormatString.IsNotNullOrEmpty()
            ? typeBuilder.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : typeBuilder.AppendMatchOrNull(key, true).FieldEnd();
        _ = valueFormatString.IsNotNullOrEmpty()
            ? typeBuilder.AppendMatchFormattedOrNull(value, valueFormatString)
            : typeBuilder.AppendMatchOrNull(value);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue, TVBase>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType, TKey key
      , TValue value, int retrieveCount, StringBearerRevealState<TVBase> valueStyler, string? keyFormatString = null) where TB : TypeMolder where TValue : TVBase
    {
        _ = keyFormatString.IsNotNullOrEmpty()
            ? typeBuilder.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : typeBuilder.AppendMatchOrNull(key, true).FieldEnd();
        valueStyler(value, typeBuilder.Master);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyValuePair<TB, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, StringBearerRevealState<TVBase> valueStyler, StringBearerRevealState<TKBase> keyStyler) where TB : TypeMolder where TKey : TKBase where TValue : TVBase
    {
        keyStyler(key, typeBuilder.Master);
        typeBuilder.FieldEnd();
        valueStyler(value, typeBuilder.Master);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> AppendKeyedCollectionNextItem<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType, Type keyType
      , Type valueType, int previousItemNumber) where TB : TypeMolder
    {
        typeBuilder.Sb.Append(CmaSpc).ToInternalTypeBuilder(typeBuilder);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> FormatCollectionStart<TB>(ITypeMolderDieCast<TB> typeBuilder
      , Type itemElementType, bool hasItems, Type collectionType) where TB : TypeMolder
    {
        if (!(collectionType.FullName?.StartsWith("System") ?? true)) typeBuilder.Sb.Append(collectionType.Name).Append(Spc);
        return base.CollectionStart(itemElementType, typeBuilder.Sb, hasItems).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB, TCustStyle, TCustBase>(ITypeMolderDieCast<TB> typeBuilder
      , TCustStyle item
      , int retrieveCount, StringBearerRevealState<TCustBase> styler) where TB : TypeMolder where TCustStyle : TCustBase
    {
        styler(item, typeBuilder.Master);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder, string? item, int retrieveCount
      , string? formatString = null) where TB : TypeMolder
    {
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeBuilder.Sb.AppendFormat(this, formatString, item);
        else
            typeBuilder.Sb.Append(item, this);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB, TCharSeq>(ITypeMolderDieCast<TB> typeBuilder, TCharSeq? item
      , int retrieveCount
      , string? formatString = null) where TB : TypeMolder where TCharSeq : ICharSequence
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeBuilder.Sb.Append(item, 0, item.Length, formatString, this);
        else
            typeBuilder.Sb.Append(item, this);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder? item
      , int retrieveCount
      , string? formatString = null) where TB : TypeMolder
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeBuilder.Sb.Append(item, 0, item.Length, formatString, this);
        else
            typeBuilder.Sb.Append(item, this);
        return typeBuilder;
    }

    public virtual ITypeMolderDieCast<TB> CollectionNextItemFormat<TB>(ITypeMolderDieCast<TB> typeBuilder, IStringBearer? item
      , int retrieveCount) where TB : TypeMolder
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        item.RevealState(typeBuilder.Master);
        return typeBuilder;
    }

    public virtual IStringBuilder FormatFieldNameMatch<T>(IStringBuilder sb, T source
      , string? formatString = null)
    {
        return (formatString.IsNotNullOrEmpty()
            ? sb.AppendFormat(this, formatString, source)
            : sb.Append(source));
    }

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, bool source
      , string? formatString = null) where TB : TypeMolder =>
        typeBuilder.Sb.Append(source ? True : False).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, bool? source
      , string? formatString = null) where TB : TypeMolder =>
        (source != null
            ? typeBuilder.Sb.Append(source.Value ? True : False)
            : typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle)).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source
      , string? formatString = null) where TB : TypeMolder where TFmt : ISpanFormattable =>
        base.Format(source, typeBuilder.Sb, formatString).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source
      , string? formatString = null) where TB : TypeMolder where TFmt : struct, ISpanFormattable =>
        base.Format(source, typeBuilder.Sb, formatString).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue)
        where TB : TypeMolder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue) where TB : TypeMolder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, ICharSequence source
      , int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : TypeMolder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder source
      , int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : TypeMolder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB, T, TBase>(ITypeMolderDieCast<TB> typeBuilder, T toStyle
      , StringBearerRevealState<TBase> styler) where TB : TypeMolder where T : TBase =>
        styler(toStyle, typeBuilder.Master).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldName<TB>(ITypeMolderDieCast<TB> typeBuilder, IStringBearer styledObj)
        where TB : TypeMolder =>
        styledObj.RevealState(typeBuilder.Master).ToInternalTypeBuilder(typeBuilder);

    public virtual IStringBuilder FormatFieldContentsMatch<T>(IStringBuilder sb, T source
      , string? formatString = null) =>
        (formatString.IsNotNullOrEmpty()
            ? sb.AppendFormat(this, formatString, source)
            : sb.Append(source));

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, bool source
      , string? formatString = null) where TB : TypeMolder =>
        typeBuilder.Sb.Append(source ? True : False).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, bool? source
      , string? formatString = null) where TB : TypeMolder =>
        (source != null
            ? typeBuilder.Sb.Append(source.Value ? True : False)
            : typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle)).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source
      , string? formatString = null) where TB : TypeMolder where TFmt : ISpanFormattable =>
        base.Format(source, typeBuilder.Sb, formatString).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB, TFmt>(ITypeMolderDieCast<TB> typeBuilder, TFmt? source
      , string? formatString = null) where TB : TypeMolder where TFmt : struct, ISpanFormattable =>
        base.Format(source, typeBuilder.Sb, formatString).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue)
        where TB : TypeMolder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue) where TB : TypeMolder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue) where TB : TypeMolder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue) where TB : TypeMolder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB, T, TBase>(ITypeMolderDieCast<TB> typeBuilder, T toStyle
      , StringBearerRevealState<TBase> styler) where TB : TypeMolder where T : TBase =>
        styler(toStyle, typeBuilder.Master).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatFieldContents<TB>(ITypeMolderDieCast<TB> typeBuilder
      , IStringBearer styledObj) where TB : TypeMolder =>
        styledObj.RevealState(typeBuilder.Master).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatCollectionNext<TB, TItem>(ITypeMolderDieCast<TB> typeBuilder
      , TItem toFormat, int itemAt, string? formatString = null) where TB : TypeMolder where TItem : ISpanFormattable =>
        base.CollectionNextItem(toFormat, itemAt, typeBuilder.Sb).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> AddCollectionElementSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder
      , Type elementType, int nextItemNumber) where TB : TypeMolder =>
        (elementType == typeof(byte)
            ? typeBuilder.Sb.Append(nextItemNumber % 4 == 0 ? Spc : "")
            : typeBuilder.Sb.Append(CmaSpc)).ToInternalTypeBuilder(typeBuilder);

    public virtual ITypeMolderDieCast<TB> FormatCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type itemElementType
      , int totalItemCount) where TB : TypeMolder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        return base.CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount).ToInternalTypeBuilder(typeBuilder);
    }
}
