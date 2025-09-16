// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.Mutable.Strings.CustomFormatting;
using FortitudeCommon.Types.StyledToString.Options;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

public class CompactLogTypeFormatting : DefaultStringFormatter, IStyledTypeFormatting
{
    protected const string Dot       = ".";
    protected const string CmaSpc    = ", ";
    protected const string Spc       = " ";
    protected const string ClnSpc    = ": ";
    protected const string BrcOpnSpc = "{ ";
    protected const string SpcBrcCls = " }";
    public virtual string Name => nameof(CompactLogTypeFormatting);

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendValueTypeOpening<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , Type valueType, string? alternativeName) where TB : StyledTypeBuilder
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

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendValueTypeClosing<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type valueType) where TB : StyledTypeBuilder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        if (valueType.IsEnum)
        {
            return typeBuilder;
        }
        return typeBuilder.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendComplexTypeOpening<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type complexType
      , string? alternativeName = null)
        where TB : StyledTypeBuilder
    {
        typeBuilder.Sb.Append(alternativeName ?? complexType.Name).Append(Spc);
        return typeBuilder.Sb.Append(BrcOpnSpc).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, string fieldName)
        where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(fieldName).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendFieldValueSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder)
        where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(ClnSpc).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> AddNextFieldSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder)
        where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(CmaSpc).ToInternalTypeBuilder(typeBuilder);

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel) 
    {
        return sb.InsertAt(CmaSpc, atIndex).ReturnCharCount(2);
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendTypeClosing<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder)
        where TB : StyledTypeBuilder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        return typeBuilder.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeBuilder);
    }


    public virtual IStyleTypeBuilderComponentAccess<TB> AppendKeyedCollectionStart<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type keyedCollectionType, Type keyType
      , Type valueType) where TB : StyledTypeBuilder
    {
        typeBuilder.Sb.Append(keyedCollectionType.Name).Append(Spc);
        return typeBuilder.Sb.Append(BrcOpnSpc).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendKeyedCollectionEnd<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type keyedCollectionType, Type keyType
      , Type valueType, int totalItemCount) where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(SpcBrcCls).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendKeyValuePair<TB, TKey, TValue>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type keyedCollectionType, TKey key
      , TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null) where TB : StyledTypeBuilder
    {
        _ = keyFormatString.IsNotNullOrEmpty()
            ? typeBuilder.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : typeBuilder.AppendMatchOrNull(key, true).FieldEnd();
        _ = valueFormatString.IsNotNullOrEmpty()
            ? typeBuilder.AppendMatchFormattedOrNull(value, valueFormatString)
            : typeBuilder.AppendMatchOrNull(value);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendKeyValuePair<TB, TKey, TValue, TVBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type keyedCollectionType, TKey key
      , TValue value, int retrieveCount, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TB : StyledTypeBuilder where TValue : TVBase
    {
        _ = keyFormatString.IsNotNullOrEmpty()
            ? typeBuilder.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : typeBuilder.AppendMatchOrNull(key, true).FieldEnd();
        valueStyler(value, typeBuilder.OwningAppender);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendKeyValuePair<TB, TKey, TValue, TKBase, TVBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TB : StyledTypeBuilder where TKey : TKBase where TValue : TVBase
    {
        keyStyler(key, typeBuilder.OwningAppender);
        typeBuilder.FieldEnd();
        valueStyler(value, typeBuilder.OwningAppender);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> AppendKeyedCollectionNextItem<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type keyedCollectionType, Type keyType
      , Type valueType, int previousItemNumber) where TB : StyledTypeBuilder
    {
        typeBuilder.Sb.Append(CmaSpc).ToInternalTypeBuilder(typeBuilder);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatCollectionStart<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , Type itemElementType, bool hasItems, Type collectionType) where TB : StyledTypeBuilder
    {
        if (!(collectionType.FullName?.StartsWith("System") ?? true)) typeBuilder.Sb.Append(collectionType.Name).Append(Spc);
        return base.CollectionStart(itemElementType, typeBuilder.Sb, hasItems).ToInternalTypeBuilder(typeBuilder);
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB, TCustStyle, TCustBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , TCustStyle item
      , int retrieveCount, CustomTypeStyler<TCustBase> styler) where TB : StyledTypeBuilder where TCustStyle : TCustBase
    {
        styler(item, typeBuilder.OwningAppender);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, string? item, int retrieveCount
      , string? formatString = null) where TB : StyledTypeBuilder
    {
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            typeBuilder.Sb.AppendFormat(this, formatString, item);
        else
            typeBuilder.Sb.Append(item, this);
        return typeBuilder;
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB, TCharSeq>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TCharSeq? item
      , int retrieveCount
      , string? formatString = null) where TB : StyledTypeBuilder where TCharSeq : ICharSequence
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

    public virtual IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder? item
      , int retrieveCount
      , string? formatString = null) where TB : StyledTypeBuilder
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

    public virtual IStyleTypeBuilderComponentAccess<TB> CollectionNextItemFormat<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, IStyledToStringObject? item
      , int retrieveCount) where TB : StyledTypeBuilder
    {
        if (item == null)
        {
            typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle);
            return typeBuilder;
        }
        item.ToString(typeBuilder.OwningAppender);
        return typeBuilder;
    }

    public virtual IStringBuilder FormatFieldNameMatch<T>(IStringBuilder sb, T source
      , string? formatString = null)
    {
        return (formatString.IsNotNullOrEmpty()
            ? sb.AppendFormat(this, formatString, source)
            : sb.Append(source));
    }

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool source
      , string? formatString = null) where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(source ? True : False).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool? source
      , string? formatString = null) where TB : StyledTypeBuilder =>
        (source != null
            ? typeBuilder.Sb.Append(source.Value ? True : False)
            : typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle)).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt? source
      , string? formatString = null) where TB : StyledTypeBuilder where TFmt : ISpanFormattable =>
        base.Format(source, typeBuilder.Sb, formatString).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt? source
      , string? formatString = null) where TB : StyledTypeBuilder where TFmt : struct, ISpanFormattable =>
        base.Format(source, typeBuilder.Sb, formatString).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue)
        where TB : StyledTypeBuilder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, ICharSequence source
      , int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder source
      , int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB, T, TBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T toStyle
      , CustomTypeStyler<TBase> styler) where TB : StyledTypeBuilder where T : TBase =>
        styler(toStyle, typeBuilder.OwningAppender.ToTypeStringAppender).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldName<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, IStyledToStringObject styledObj)
        where TB : StyledTypeBuilder =>
        styledObj.ToString(typeBuilder.OwningAppender).ToInternalTypeBuilder(typeBuilder);

    public virtual IStringBuilder FormatFieldContentsMatch<T>(IStringBuilder sb, T source
      , string? formatString = null) =>
        (formatString.IsNotNullOrEmpty()
            ? sb.AppendFormat(this, formatString, source)
            : sb.Append(source));

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool source
      , string? formatString = null) where TB : StyledTypeBuilder =>
        typeBuilder.Sb.Append(source ? True : False).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, bool? source
      , string? formatString = null) where TB : StyledTypeBuilder =>
        (source != null
            ? typeBuilder.Sb.Append(source.Value ? True : False)
            : typeBuilder.Sb.Append(typeBuilder.Settings.NullStyle)).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt? source
      , string? formatString = null) where TB : StyledTypeBuilder where TFmt : ISpanFormattable =>
        base.Format(source, typeBuilder.Sb, formatString).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, TFmt>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, TFmt? source
      , string? formatString = null) where TB : StyledTypeBuilder where TFmt : struct, ISpanFormattable =>
        base.Format(source, typeBuilder.Sb, formatString).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue)
        where TB : StyledTypeBuilder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = Int32.MaxValue) where TB : StyledTypeBuilder =>
        base.Format(source, sourceFrom, typeBuilder.Sb, formatString, maxTransferCount).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB, T, TBase>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, T toStyle
      , CustomTypeStyler<TBase> styler) where TB : StyledTypeBuilder where T : TBase =>
        styler(toStyle, typeBuilder.OwningAppender.ToTypeStringAppender).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatFieldContents<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , IStyledToStringObject styledObj) where TB : StyledTypeBuilder =>
        styledObj.ToString(typeBuilder.OwningAppender).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatCollectionNext<TB, TItem>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , TItem toFormat, int itemAt, string? formatString = null) where TB : StyledTypeBuilder where TItem : ISpanFormattable =>
        base.CollectionNextItem(toFormat, itemAt, typeBuilder.Sb).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> AddCollectionElementSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder
      , Type elementType, int nextItemNumber) where TB : StyledTypeBuilder =>
        (elementType == typeof(byte)
            ? typeBuilder.Sb.Append(nextItemNumber % 4 == 0 ? Spc : "")
            : typeBuilder.Sb.Append(CmaSpc)).ToInternalTypeBuilder(typeBuilder);

    public virtual IStyleTypeBuilderComponentAccess<TB> FormatCollectionEnd<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type itemElementType
      , int totalItemCount) where TB : StyledTypeBuilder
    {
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        return base.CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount).ToInternalTypeBuilder(typeBuilder);
    }
}
