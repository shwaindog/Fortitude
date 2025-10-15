// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactJsonTypeFormatting : JsonFormatter, IStyledTypeFormatting
{
    protected const string Cln = ":";
    public virtual string Name => nameof(CompactJsonTypeFormatting);
    
    public StyleOptions StyleOptions => (StyleOptions)Options;

    public virtual CompactJsonTypeFormatting Initialize(StyleOptions styleOptions)
    {
        Options = styleOptions;

        return this;
    }

    public virtual IStringBuilder AppendValueTypeOpening(IStringBuilder sb, Type valueType, string? alternativeName = null) => sb;

    public virtual IStringBuilder AppendValueTypeClosing(IStringBuilder sb, Type valueType) => sb.RemoveLastWhiteSpacedCommaIfFound();

    public virtual IStringBuilder AppendComplexTypeOpening(IStringBuilder sb, Type complexType, string? alternativeName = null)  =>
        sb.Append(BrcOpn);

    public virtual IStringBuilder AppendFieldValueSeparator(IStringBuilder sb) => sb.Append(Cln);

    public virtual IStringBuilder AddNextFieldSeparator(IStringBuilder sb)  => sb.Append(Cma);

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel) => 
        sb.InsertAt(Cma, atIndex).ReturnCharCount(1);

    public virtual IStringBuilder AppendTypeClosing(IStringBuilder sb) => sb.RemoveLastWhiteSpacedCommaIfFound().Append(BrcCls);

    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName) =>
        sb.Append(DblQt).Append(fieldName).Append(DblQt);

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null)
    {
        if (formatString.IsNotNullOrEmpty())
            sb.Append(DblQt).AppendFormat(this, formatString, source).Append(DblQt);
        else
            sb.Append(DblQt).Append(source).Append(DblQt);

        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null) => 
        Format(source, sb, formatString).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null) => 
        Format(source, sb, formatString).ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null) where TFmt : ISpanFormattable
    {
        var origValuesInQuotes = StyleOptions.WrapValuesInQuotes;
        StyleOptions.WrapValuesInQuotes = true;
        base.Format(source, sb, formatString);
        StyleOptions.WrapValuesInQuotes = origValuesInQuotes;
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null)
        where TFmt : struct, ISpanFormattable
    {
        if (!source.HasValue)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        return FormatFieldName(sb, source.Value, formatString);
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue)
    {
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        return sb.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue)
    {
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        return sb.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue)
    {
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        return sb.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue)
    {
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        return sb.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldName<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle, PalantírReveal<TCloakedBase> styler) 
        where TCloaked : TCloakedBase
    {
        var sb           = tos.WriteBuffer;
        var preAppendLen = sb.Length;
        styler(toStyle, tos);
        if (sb.Length == preAppendLen) return tos.WriteBuffer;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return tos.WriteBuffer;
        sb.Insert(preAppendLen, DblQt);
        return sb.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldName(ITheOneString tos, IStringBearer styledObj)
    {
        var preAppendLen = tos.WriteBuffer.Length;
        styledObj.RevealState(tos);
        if (tos.WriteBuffer.Length == preAppendLen) return tos.WriteBuffer;
        ProcessAppendedRange(tos.WriteBuffer, preAppendLen);
        if (tos.WriteBuffer[preAppendLen] == DblQtChar) return tos.WriteBuffer;
        tos.WriteBuffer.Insert(preAppendLen, DblQt);
        return tos.WriteBuffer.Append(DblQt);
    }

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null)
    {
        if (source == null) return sb;
        string rawValue;
        if (source is JsonNode jsonNode)
        {
            rawValue = jsonNode.ToJsonString();
        }
        else
        {
            rawValue = source.ToString() ?? "";
        }
        sb.AppendFormat(this, formatString ?? "{0}", rawValue);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source
      , string? formatString = null)
    {
        if (JsonOptions.WrapValuesInQuotes) sb.Append(DblQt);
        sb.Append(source ? Options.True : Options.False);
        if (JsonOptions.WrapValuesInQuotes) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source
      , string? formatString = null)
    {
        if (JsonOptions.WrapValuesInQuotes) sb.Append(DblQt);
        if (source != null)
            sb.Append(source.Value ? Options.True : Options.False);
        else
            sb.Append(StyleOptions.NullStyle);
        if (JsonOptions.WrapValuesInQuotes) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null) 
        where TFmt : ISpanFormattable =>
        base.Format(source, sb, formatString ?? "").ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        if (!source.HasValue)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        return FormatFieldContents(sb, source.Value, formatString);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue)
    {
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue)
    {
        if (JsonOptions.CharArrayWritesString)
        {
            sb.Append(DblQt);
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
            sb.Append(DblQt);
        }
        else
        {
            var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
            var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
            var cappedEnd = cappedFrom + cappedLength;
            var charType     = typeof(char);
            base.CollectionStart(charType, sb, cappedLength > 0);

            var lastAdded    = 0;
            var previousChar = '\0';
            for (int i = cappedFrom; i < cappedEnd; i++)
            {
                if (i > 0) AddCollectionElementSeparator(charType, sb, i);

                var nextChar = source[cappedFrom + i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItem(new Rune(previousChar, nextChar), i, sb)
                    : CollectionNextItem(nextChar, i, sb);
                previousChar = lastAdded == 0 ? nextChar : '\0'; 
            }
            sb.RemoveLastWhiteSpacedCommaIfFound();
            base.CollectionEnd(charType, sb, cappedLength);
        }
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue)
    {
        if (JsonOptions.CharArrayWritesString)
        {
            sb.Append(DblQt);
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
            sb.Append(DblQt);
        }
        else
        {
            var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
            var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

            var charType = typeof(char);
            CollectionStart(charType, sb, cappedLength > 0);

            var lastAdded    = 0;
            var previousChar = '\0';
            for (int i = cappedFrom; i < cappedLength; i++)
            {
                if (i > 0  && lastAdded > 0) AddCollectionElementSeparator(charType, sb, i);

                var nextChar = source[cappedFrom + i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, formatString ?? "")
                    : CollectionNextItemFormat(nextChar, i, sb, formatString ?? "");
                previousChar = lastAdded == 0 ? nextChar : '\0'; 
            }
            CollectionEnd(charType, sb, cappedLength);
        }
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue)
    {
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler) where TCloaked : TCloakedBase
    {
        var preAppendLen = tos.WriteBuffer.Length;
        styler(toStyle, tos);
        if (tos.WriteBuffer.Length != preAppendLen) ProcessAppendedRange(tos.WriteBuffer, preAppendLen);
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder FormatFieldContents(ITheOneString tos, IStringBearer styledObj)
    {
        var sb           = tos.WriteBuffer;
        var preAppendLen = sb.Length;
        styledObj.RevealState(tos);
        if (sb.Length != preAppendLen) ProcessAppendedRange(sb, preAppendLen);
        return sb;
    }


    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType) 
    {
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            sb.Append(SqBrktOpn);
        }
        else
        {
            sb.Append(BrcOpn);
        }
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , int totalItemCount) 
    {
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            sb.Append(SqBrktCls);
        }
        else
        {
            sb.Append(BrcCls);
        }
        return sb;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null)
        where TMold : TypeMolder
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold.Sb, keyedCollectionType);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", true);
            AddNextFieldSeparator(typeMold.Sb);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this);
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "");
            AppendTypeClosing(typeMold.Sb);
        }
        else
        {

            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", true).FieldEnd();
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "");
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler, string? keyFormatString = null)
        where TMold : TypeMolder where TValue : TVBase
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold.Sb, keyedCollectionType);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", true);
            AddNextFieldSeparator(typeMold.Sb);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this);
            valueStyler(value, typeMold.Master);
            AppendTypeClosing(typeMold.Sb);
        }
        else
        {
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", true).FieldEnd();
            valueStyler(value, typeMold.Master);
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, PalantírReveal<TVBase> valueStyler
      , PalantírReveal<TKBase> keyStyler)
        where TMold : TypeMolder where TKey : TKBase where TValue : TVBase
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold.Sb, keyedCollectionType);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this);
            keyStyler(key, typeMold.Master);
            AddNextFieldSeparator(typeMold.Sb);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this);
            valueStyler(value, typeMold.Master);
            AppendTypeClosing(typeMold.Sb);
        }
        else
        {
            keyStyler(key, typeMold.Master);
            typeMold.FieldEnd();
            valueStyler(value, typeMold.Master);
        }
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb
      , Type keyedCollectionType, Type keyType, Type valueType, int previousItemCount) => sb.Append(Cma);

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems)
    {
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
        return sb.Append(SqBrktOpn).ReturnCharCount(1);
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems)
    {
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return destSpan.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return destSpan.OverWriteAt(destStartIndex, DblQt);
        return destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
    }

    public virtual IStringBuilder FormatCollectionStart(IStringBuilder sb
      , Type itemElementType, bool hasItems, Type collectionType) => base.CollectionStart(itemElementType, sb, hasItems).ToStringBuilder(sb);

    public virtual IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ITheOneString tos
      , TCloaked item, int retrieveCount, PalantírReveal<TCloakedBase> styler) where TCloaked : TCloakedBase
    {
        styler(item, tos);
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount, string? formatString = null)
    {
        if (item == null)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        
        sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount, string? formatString = null)
    {
        if (item == null)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        Format(item, 0, sb, formatString ?? "");
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat<TCharSeq>(IStringBuilder sb, TCharSeq? item, int retrieveCount
      , string? formatString = null) where TCharSeq : ICharSequence
    {
        if (item == null)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        Format(item, 0, sb, formatString ?? "");
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item
      , int retrieveCount, string? formatString = null) 
    {
        if (item == null)
        {
            return sb.Append(StyleOptions.NullStyle);
        }
        Format(item, 0, sb, formatString ?? "");
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(ITheOneString tos
      , IStringBearer? item, int retrieveCount)
    {
        if (item == null)
        {
            return tos.WriteBuffer.Append(StyleOptions.NullStyle);
        }
        item.RevealState(tos);
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder AddCollectionElementSeparator(IStringBuilder sb
      , Type elementType, int nextItemNumber)
    {
        base.AddCollectionElementSeparator(elementType, sb, nextItemNumber);
        return sb;
    }

    public virtual IStringBuilder FormatCollectionEnd(IStringBuilder sb, Type itemElementType
      , int totalItemCount) 
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        base.CollectionEnd(itemElementType, sb, totalItemCount);
        return sb;
    }
}
