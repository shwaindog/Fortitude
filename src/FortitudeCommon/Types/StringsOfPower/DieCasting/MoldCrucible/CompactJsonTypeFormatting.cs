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

    public virtual ITypeMolderDieCast<TMold> AppendValueTypeOpening<TMold>(ITypeMolderDieCast<TMold> typeMold
      , Type valueType, string? alternativeName = null) where TMold : TypeMolder =>
        typeMold;

    public virtual ITypeMolderDieCast<TMold> AppendValueTypeClosing<TMold>(ITypeMolderDieCast<TMold> typeMold, Type valueType) where TMold : TypeMolder
    {
        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendComplexTypeOpening<TMold>(ITypeMolderDieCast<TMold> typeMold, Type complexType
      , string? alternativeName = null)
        where TMold : TypeMolder =>
        typeMold.Sb.Append(BrcOpn).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> AppendFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, ReadOnlySpan<char> fieldName)
        where TMold : TypeMolder =>
        typeMold.Sb.Append(DblQt).Append(fieldName).Append(DblQt).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> AppendFieldValueSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold)
        where TMold : TypeMolder =>
        typeMold.Sb.Append(Cln).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> AddNextFieldSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold)
        where TMold : TypeMolder =>
        typeMold.Sb.Append(Cma).ToInternalTypeBuilder(typeMold);

    public virtual int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel)
    {
        return sb.InsertAt(Cma, atIndex).ReturnCharCount(1);
    }

    public virtual ITypeMolderDieCast<TMold> AppendTypeClosing<TMold>(ITypeMolderDieCast<TMold> typeMold)
        where TMold : TypeMolder
    {
        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        return typeMold.Sb.Append(BrcCls).ToInternalTypeBuilder(typeMold);
    }

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null)
    {
        if (formatString.IsNotNullOrEmpty())
            sb.Append(DblQt).AppendFormat(this, formatString, source).Append(DblQt);
        else
            sb.Append(DblQt).Append(source).Append(DblQt);

        return sb;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, bool source
      , string? formatString = null) where TMold : TypeMolder =>
        typeMold.Sb.Append(DblQt).Append(source ? Options.True : Options.False).Append(DblQt).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, bool? source
      , string? formatString = null) where TMold : TypeMolder =>
        (source != null
            ? typeMold.Sb.Append(DblQt).Append(source.Value ? Options.True : Options.False).Append(DblQt)
            : typeMold.Sb.Append(DblQt).Append(typeMold.Settings.NullStyle).Append(DblQt)).ToInternalTypeBuilder(typeMold);

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source
      , string? formatString = null)
        where TMold : TypeMolder where TFmt : ISpanFormattable
    {
        var sb                 = typeMold.Sb;
        var origValuesInQuotes = StyleOptions.WrapValuesInQuotes;
        StyleOptions.WrapValuesInQuotes = true;
        base.Format(source, sb, formatString);
        StyleOptions.WrapValuesInQuotes = origValuesInQuotes;
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source
      , string? formatString = null)
        where TMold : TypeMolder where TFmt : struct, ISpanFormattable
    {
        var sb      = typeMold.Sb;
        if (!source.HasValue)
        {
            sb.Append(typeMold.Settings.NullStyle);
            return typeMold;
        }
        return FormatFieldName(typeMold, source.Value, formatString);
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold
      , ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, char[] source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, ICharSequence source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, StringBuilder source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler)
        where TMold : TypeMolder where TCloaked : TCloakedBase
    {
        var sb           = typeMold.Sb;
        var preAppendLen = sb.Length;
        styler(toStyle, typeMold.Master);
        if (sb.Length == preAppendLen) return typeMold;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return typeMold;
        sb.Insert(preAppendLen, DblQt);
        sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldName<TMold>(ITypeMolderDieCast<TMold> typeMold, IStringBearer styledObj)
        where TMold : TypeMolder
    {
        var sb           = typeMold.Sb;
        var preAppendLen = sb.Length;
        styledObj.RevealState(typeMold.Master);
        if (sb.Length == preAppendLen) return typeMold;
        ProcessAppendedRange(sb, preAppendLen);
        if (sb[preAppendLen] == DblQtChar) return typeMold;
        sb.Insert(preAppendLen, DblQt);
        sb.Append(DblQt);
        return typeMold;
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
        if (formatString.IsNotNullOrEmpty())
            sb.AppendFormat(this, formatString, rawValue);
        else
            sb.Append(rawValue);
        return sb;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, bool source
      , string? formatString = null)
        where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        if (JsonOptions.WrapValuesInQuotes) sb.Append(DblQt);
        typeMold.Sb.Append(source ? Options.True : Options.False);
        if (JsonOptions.WrapValuesInQuotes) sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, bool? source
      , string? formatString = null)
        where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        if (JsonOptions.WrapValuesInQuotes) sb.Append(DblQt);
        if (source != null)
            typeMold.Sb.Append(source.Value ? Options.True : Options.False);
        else
            typeMold.Sb.Append(typeMold.Settings.NullStyle);
        if (JsonOptions.WrapValuesInQuotes) sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source
      , string? formatString = null)
        where TMold : TypeMolder where TFmt : ISpanFormattable
    {
        var sb            = typeMold.Sb;

        base.Format(source, sb, formatString ?? "");
        
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TFmt>(ITypeMolderDieCast<TMold> typeMold, TFmt? source
      , string? formatString = null)
        where TMold : TypeMolder where TFmt : struct, ISpanFormattable
    {
        var sb      = typeMold.Sb;
        if (!source.HasValue)
        {
            sb.Append(typeMold.Settings.NullStyle);
            return typeMold;
        }
        return FormatFieldContents(typeMold, source.Value, formatString);
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold
      , ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue)
        where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, char[] source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
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
            var charType     = typeof(char);
            FormatCollectionStart(typeMold, charType, cappedLength > 0, source.GetType());

            var lastAdded    = 0;
            var previousChar = '\0';
            for (int i = cappedFrom; i < cappedLength; i++)
            {
                if (i > 0) AddCollectionElementSeparator(typeMold, charType, i);

                var nextChar = source[cappedFrom + i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItem(new Rune(previousChar, nextChar), i, sb)
                    : CollectionNextItem(nextChar, i, sb);
                previousChar = lastAdded == 0 ? nextChar : '\0'; 
            }
            FormatCollectionEnd(typeMold, charType, cappedLength);
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, ICharSequence source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
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
            FormatCollectionStart(typeMold, charType, cappedLength > 0, source.GetType());

            var lastAdded    = 0;
            var previousChar = '\0';
            for (int i = cappedFrom; i < cappedLength; i++)
            {
                if (i > 0  && lastAdded > 0) AddCollectionElementSeparator(typeMold, charType, i);

                var nextChar = source[cappedFrom + i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, formatString ?? "")
                    : CollectionNextItemFormat(nextChar, i, sb, formatString ?? "");
                previousChar = lastAdded == 0 ? nextChar : '\0'; 
            }
            FormatCollectionEnd(typeMold, charType, cappedLength);
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold, StringBuilder source
      , int sourceFrom = 0
      , string? formatString = null
      , int maxTransferCount = int.MaxValue) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount);
        sb.Append(DblQt);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler)
        where TMold : TypeMolder where TCloaked : TCloakedBase
    {
        var sb           = typeMold.Sb;
        var preAppendLen = sb.Length;
        styler(toStyle, typeMold.Master);
        if (sb.Length != preAppendLen) ProcessAppendedRange(sb, preAppendLen);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatFieldContents<TMold>(ITypeMolderDieCast<TMold> typeMold
      , IStringBearer styledObj)
        where TMold : TypeMolder
    {
        var sb           = typeMold.Sb;
        var preAppendLen = sb.Length;
        styledObj.RevealState(typeMold.Master);
        if (sb.Length != preAppendLen) ProcessAppendedRange(sb, preAppendLen);
        return typeMold;
    }


    public virtual ITypeMolderDieCast<TMold> AppendKeyedCollectionStart<TMold>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, Type keyType, Type valueType) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            sb.Append(SqBrktOpn);
        }
        else
        {
            sb.Append(BrcOpn).ToInternalTypeBuilder(typeMold);
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyedCollectionEnd<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount) where TMold : TypeMolder
    {
        var sb = typeMold.Sb;
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            sb.Append(SqBrktCls);
        }
        else
        {
            sb.Append(BrcCls).ToInternalTypeBuilder(typeMold);
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null)
        where TMold : TypeMolder
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            AppendComplexTypeOpening(typeMold, keyedCollectionType);
            AppendFieldName(typeMold, "Key").FieldEnd();
            _ = keyFormatString.IsNotNullOrEmpty()
                ? typeMold.AppendMatchFormattedOrNull(key, keyFormatString, true)
                : typeMold.AppendMatchOrNull(key, true);
            AddNextFieldSeparator(typeMold);
            AppendFieldName(typeMold, "Value").FieldEnd();
            _ = valueFormatString.IsNotNullOrEmpty()
                ? typeMold.AppendMatchFormattedOrNull(value, valueFormatString)
                : typeMold.AppendMatchOrNull(value);
            AppendTypeClosing(typeMold);
        }
        else
        {
            _ = keyFormatString.IsNotNullOrEmpty()
                ? typeMold.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                : typeMold.AppendMatchOrNull(key, true).FieldEnd();
            _ = valueFormatString.IsNotNullOrEmpty()
                ? typeMold.AppendMatchFormattedOrNull(value, valueFormatString)
                : typeMold.AppendMatchOrNull(value);
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
            AppendComplexTypeOpening(typeMold, keyedCollectionType);
            AppendFieldName(typeMold, "Key").FieldEnd();
            _ = keyFormatString.IsNotNullOrEmpty()
                ? typeMold.AppendMatchFormattedOrNull(key, keyFormatString, true)
                : typeMold.AppendMatchOrNull(key, true);
            AddNextFieldSeparator(typeMold);
            AppendFieldName(typeMold, "Value").FieldEnd();
            valueStyler(value, typeMold.Master);
            AppendTypeClosing(typeMold);
        }
        else
        {
            _ = keyFormatString.IsNotNullOrEmpty()
                ? typeMold.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                : typeMold.AppendMatchOrNull(key, true).FieldEnd();
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
            AppendComplexTypeOpening(typeMold, keyedCollectionType);
            AppendFieldName(typeMold, "Key").FieldEnd();
            keyStyler(key, typeMold.Master);
            AddNextFieldSeparator(typeMold);
            AppendFieldName(typeMold, "Value").FieldEnd();
            valueStyler(value, typeMold.Master);
            AppendTypeClosing(typeMold);
        }
        else
        {
            keyStyler(key, typeMold.Master);
            typeMold.FieldEnd();
            valueStyler(value, typeMold.Master);
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyedCollectionNextItem<TMold>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, Type keyType, Type valueType, int previousItemCount) where TMold : TypeMolder
    {
        typeMold.Sb.Append(Cma).ToInternalTypeBuilder(typeMold);
        return typeMold;
    }

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

    public virtual ITypeMolderDieCast<TMold> FormatCollectionStart<TMold>(ITypeMolderDieCast<TMold> typeMold
      , Type itemElementType, bool hasItems, Type collectionType) where TMold : TypeMolder
    {
        base.CollectionStart(itemElementType, typeMold.Sb, hasItems);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold, TCloaked, TCloakedBase>(ITypeMolderDieCast<TMold> typeMold
      , TCloaked item
      , int retrieveCount, PalantírReveal<TCloakedBase> styler) where TMold : TypeMolder where TCloaked : TCloakedBase
    {
        styler(item, typeMold.Master);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold>(ITypeMolderDieCast<TMold> typeMold, string? item
      , int retrieveCount, string? formatString = null) where TMold : TypeMolder
    {
        if (item == null)
        {
            typeMold.Sb.Append(typeMold.Settings.NullStyle);
            return typeMold;
        }
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

    public virtual ITypeMolderDieCast<TMold> CollectionNextItemFormat<TMold>(ITypeMolderDieCast<TMold> typeMold
      , IStringBearer? item, int retrieveCount) where TMold : TypeMolder
    {
        if (item == null)
        {
            typeMold.Sb.Append(typeMold.Settings.NullStyle);
            return typeMold;
        }
        item.RevealState(typeMold.Master);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AddCollectionElementSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold
      , Type elementType, int nextItemNumber)
        where TMold : TypeMolder
    {
        base.AddCollectionElementSeparator(elementType, typeMold.Sb, nextItemNumber);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> FormatCollectionEnd<TMold>(ITypeMolderDieCast<TMold> typeMold, Type itemElementType
      , int totalItemCount) where TMold : TypeMolder
    {
        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        base.CollectionEnd(itemElementType, typeMold.Sb, totalItemCount);
        return typeMold;
    }
}
