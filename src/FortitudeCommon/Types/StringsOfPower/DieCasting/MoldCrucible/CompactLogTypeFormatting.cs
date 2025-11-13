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
using static FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactLogTypeFormatting : DefaultStringFormatter, IStyledTypeFormatting
{
    protected const string Dot          = ".";
    protected const string CmaSpc       = ", ";
    protected const string Spc          = " ";
    protected const char SpcChar      = ' ';
    protected const string ClnSpc       = ": ";
    protected const string BrcOpnSpc    = "{ ";
    protected const string SpcBrcCls    = " }";
    protected const string Eqls         = "=";
    protected const string EqlsSpc         = "= ";
    protected const string SqBrktOpnSpc = "[ ";
    protected const string SpcSqBrktCls = " ]";
    protected const string RndBrktOpn = "(";
    protected const string RndBrktCls = ")";

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
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags()) { return callerFormattingFlags; }

        FieldContentHandling setFlags = callerFormattingFlags;
        setFlags |= (FieldContentHandling)base.ResolveStringFormattingFlags
            (sb.LastNonWhiteChar(), input, (FormattingHandlingFlags)setFlags, formatString ?? "");

        var typeofT = typeof(T);
        if (typeofT.IsAnyTypeHoldingChars())
        {
            var notAsStringOrValue = !(callerFormattingFlags.HasAsStringContentFlag()
                                    || callerFormattingFlags.HasAsValueContentFlag());
            setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() && notAsStringOrValue
                ? FieldContentHandling.EnsureFormattedDelimited
                : None;
            setFlags |= setFlags.ShouldDelimit() || callerFormattingFlags.HasAsStringContentFlag()
                ? FieldContentHandling.EncodeAll
                : FieldContentHandling.EncodeInnerContent;
        }
        return setFlags;
    }

    public FieldContentHandling ResolveContentAsValueFormattingFlags<T>(T input, bool hasFallbackValue)
    {
        if (input == null && !hasFallbackValue) return DefaultCallerTypeFlags;
        var typeOfT = typeof(T);
        if (typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar())
            return FieldContentHandling.DisableAutoDelimiting | FieldContentHandling.AsValueContent;
        return FieldContentHandling.AsValueContent;
    }

    public FieldContentHandling ResolveContentAsStringFormattingFlags<T>(T input, bool hasFallbackValue)
    {
        if (input == null && !hasFallbackValue) return DefaultCallerTypeFlags;
        var typeOfT = typeof(T);
        if (typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar())
            return FieldContentHandling.DisableAutoDelimiting | FieldContentHandling.AsStringContent;
        var isSpanFormattableOrNullable           = typeOfT.IsSpanFormattableOrNullable();
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable();
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable)
            return FieldContentHandling.DisableAutoDelimiting | FieldContentHandling.AsStringContent;
        return FieldContentHandling.AsStringContent;
    }

    public virtual IStringBuilder AppendValueTypeOpening(IStringBuilder sb, Type valueType, string? alternativeName)
    {
        if (alternativeName != null)
            sb.Append(alternativeName);
        else
            valueType.AppendShortNameInCSharpFormat(sb);
        if (valueType.IsEnum) { return sb.Append(Dot); }
        return sb.Append(EqlsSpc);
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

    public IStringBuilder AppendFormattedNull(IStringBuilder sb, string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (((ReadOnlySpan<char>)formatString).HasFormatStringPadding() || ((ReadOnlySpan<char>)formatString).PrefixSuffixLength() > 0)
        {
            var        formatStringBufferSize = StyleOptions.NullString.Length.CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(formatString);
            Span<char> justPrefixPaddingSuffix            = stackalloc char[formatStringBufferSize];
            justPrefixPaddingSuffix = justPrefixPaddingSuffix.ToPrefixLayoutSuffixOnlyFormatString(formatString);
            Format(StyleOptions.NullString, 0, sb, justPrefixPaddingSuffix
                               ,  formatFlags: DefaultCallerType);
            
        }
        else
        {
            sb.Append(StyleOptions.NullString);
        }
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        keyedCollectionType.AppendShortNameInCSharpFormat(sb);
        sb.Append(Spc);
        return sb.Append(BrcOpnSpc).ToStringBuilder(sb);
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        return sb.Append(SpcBrcCls);
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , TKey key, TValue value, int retrieveCount, string? valueFormatString = null, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVBase> valueStyler, string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder 
        where TValue : TVBase
        where TVBase : notnull
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
        if (value == null)
        {
            AppendFormattedNull(typeMold.Sb, "", valueFlags);
        }
        else { valueStyler(value, typeMold.Master); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKBase, TVBase>(ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key, TValue? value, int retrieveCount, PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder 
        where TKey : TKBase 
        where TValue : TVBase
        where TKBase : notnull
        where TVBase : notnull
    {
        keyStyler(key, typeMold.Master);
        typeMold.FieldEnd();
        if (value == null)
        {
            AppendFormattedNull(typeMold.Sb, "", valueFlags);
        }
        else { valueStyler(value, typeMold.Master); }
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber) => sb.Append(CmaSpc);

    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName) => sb.Append(fieldName);

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        formatString.IsNotNullOrEmpty()
            ? sb.AppendFormat(this, formatString, source)
            : sb.Append(source);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        Format(source, sb, formatString ?? "").ToStringBuilder(sb);

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        (source != null
            ? Format(source, sb, formatString ?? "")
            : base.Format(StyleOptions.NullString, 0, sb, formatString)).ToStringBuilder(sb);

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
      , PalantírReveal<TCloakedBase> styler) 
        where TCloaked : TCloakedBase 
        where TCloakedBase : notnull=>
        styler(toStyle, tos).ToStringBuilder(tos.WriteBuffer);

    public virtual IStringBuilder FormatFieldName<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer =>
        styledObj.RevealState(tos).ToStringBuilder(tos.WriteBuffer);

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        sb.AppendFormat(this, formatString ?? "", source, formatFlags: (FormattingHandlingFlags)formatFlags);

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        return Format(source, sb, formatSpan, formatFlags: (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        return Format(source, sb, formatSpan, formatFlags: (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);
    }

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        formatString ??= "";

        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        base.Format(source, sb, formatSpan, (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        
        formatString ??= "";

        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        if(formatFlags.ShouldDelimit()) sb.Append(DblQtChar);
        base.Format(source, sb, formatSpan, (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);
        if(formatFlags.ShouldDelimit()) sb.Append(DblQtChar);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag() || formatFlags.ShouldDelimit())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        if (formatFlags.ShouldDelimit()) sb.Append(SqBrktOpnChar);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(SqBrktClsChar);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag() || formatFlags.ShouldDelimit())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }
        
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var formatReadOnlySpan = formatString.AsSpan();
        var lhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteOpenReplacement;
        var rhsReplaceWith     = StyleOptions.LogInnerDoubleQuoteCloseReplacement;

        Span<char> formatSpan = stackalloc char[formatReadOnlySpan.BoundsReplaceBufferSize(lhsReplaceWith, rhsReplaceWith)];
        if (formatFlags.HasAsStringContentFlag() || formatFlags.ShouldDelimit())
        {
            formatSpan = formatSpan.ReplaceBounds(formatReadOnlySpan, DblQtChar, lhsReplaceWith, rhsReplaceWith);
        }
        else
        {
            formatReadOnlySpan.CopyTo(formatSpan);
            formatSpan = formatSpan[..formatReadOnlySpan.Length];
        }

        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatFlags: (FormattingHandlingFlags)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TCloaked, TCloakedBase>(ITheOneString tos, TCloaked toStyle
      , PalantírReveal<TCloakedBase> styler) 
        where TCloaked : TCloakedBase 
        where TCloakedBase : notnull =>
        styler(toStyle, tos).ToStringBuilder(tos.WriteBuffer);

    public virtual IStringBuilder FormatFieldContents<TBearer>(ITheOneString tos, TBearer styledObj) where TBearer : IStringBearer =>
        styledObj.RevealState(tos).ToStringBuilder(tos.WriteBuffer);

    public virtual IStringBuilder FormatCollectionStart(IStringBuilder sb, Type itemElementType, bool? hasItems, Type collectionType
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (!hasItems.HasValue) return sb;
        if (!StyleOptions.LogSuppressDisplayCollectionNames.Any(s => collectionType.FullName?.StartsWith(s) ?? false ))
        {
            sb.Append(RndBrktOpn);
            collectionType.AppendShortNameInCSharpFormat(sb);
            sb.Append(RndBrktCls);
        }
        return CollectionStart(itemElementType, sb, hasItems.Value, (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);
    }

    public override int CollectionStart(Type collectionType,  IStringBuilder sb, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        return sb.Append(SqBrktOpnSpc).ReturnCharCount(2);
    }

    public override int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        return destSpan.OverWriteAt(destStartIndex, SqBrktOpnSpc);
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags)
            .ToStringBuilder(sb);

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null)
        {
            AppendFormattedNull(sb, formatString, formatFlags);
            return sb;
        }
        return CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags)
            .ToStringBuilder(sb);
    }

    public IStringBuilder CollectionNextItemFormat<TFmt>(IStringBuilder sb, TFmt? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        if (item == null)
        {
            AppendFormattedNull(sb, formatString, formatFlags);
            return sb;
        }
        return CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags)
            .ToStringBuilder(sb);
    }

    public IStringBuilder CollectionNextItemFormat<TFmtStruct>(IStringBuilder sb, TFmtStruct? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (item == null)
        {
            AppendFormattedNull(sb, formatString, formatFlags);
            return sb;
        }
        return CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormattingHandlingFlags)formatFlags)
            .ToStringBuilder(sb);
    }

    public virtual IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ITheOneString tos
      , TCloaked? item, int retrieveCount, PalantírReveal<TCloakedBase> styler) 
        where TCloaked : TCloakedBase 
        where TCloakedBase : notnull
    {
        if (item == null)
        {
            AppendFormattedNull(tos.WriteBuffer, "");
        }
        else { styler(item, tos); }
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return sb.Append(StyleOptions.NullString); }
        sb.AppendFormat(this, formatString ?? "", item);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return sb.Append(StyleOptions.NullString); }
        sb.AppendFormat(this, formatString ?? "", item);
        return sb;
    }

    public virtual IStringBuilder CollectionNextCharSeqFormat<TCharSeq>(IStringBuilder sb, TCharSeq? item, int retrieveCount
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (item == null) { return sb.Append(StyleOptions.NullString); }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            sb.Append(item, 0, item.Length, formatString, this);
        else
            sb.Append(item, this);
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item, int retrieveCount, string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return sb.Append(StyleOptions.NullString); }
        if (formatString.IsNotNullOrEmpty() && formatString != NoFormatFormatString)
            sb.Append(item, 0, item.Length, formatString, this);
        else
            sb.Append(item, this);
        return sb;
    }

    public virtual IStringBuilder CollectionNextStringBearerFormat<TBearer>(ITheOneString tos, TBearer? item, int retrieveCount) where TBearer : IStringBearer
    {
        if (item == null) { return tos.WriteBuffer.Append(StyleOptions.NullString); }
        item.RevealState(tos);
        return tos.WriteBuffer;
    }

    public virtual IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType, int nextItemNumber
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        (elementType == typeof(byte)
            ? sb.Append(nextItemNumber % 4 == 0 ? Spc : "")
            : sb.Append(CmaSpc));

    public virtual IStringBuilder FormatCollectionEnd(IStringBuilder sb, Type itemElementType
      , int? totalItemCount, string? formatString, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (!totalItemCount.HasValue)
        {
            if (StyleOptions.NullWritesEmpty)
            {
                CollectionStart(itemElementType, sb, false, (FormattingHandlingFlags)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormattingHandlingFlags)formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }
            return sb;
        }
        
        return CollectionEnd(itemElementType, sb, totalItemCount.Value, (FormattingHandlingFlags)formatFlags).ToStringBuilder(sb);
    }

    public override int CollectionEnd(Type collectionType, IStringBuilder sb, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        var lastChar = sb.RemoveLastWhiteSpacedCommaIfFound();
        // if (lastChar != SqBrktOpnChar && lastChar != SpcChar) sb.Append(Spc);
        if (lastChar != SqBrktOpnChar) sb.Append(Spc);
        return sb.Append(SqBrktCls).ReturnCharCount(2);
    }

    public override int CollectionEnd(Type collectionType, Span<char> destSpan, int index, int itemsCount
      , FormattingHandlingFlags formatFlags = FormattingHandlingFlags.EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        var lastChar = destSpan.RemoveLastWhiteSpacedCommaIfFound(ref index);
        if (lastChar != SqBrktOpnChar) index += destSpan.OverWriteAt(index, Spc);
        return destSpan.OverWriteAt(index, SqBrktCls);
    }

    public virtual ITypeMolderDieCast<TMold> FormatCollectionNext<TMold, TItem>(ITypeMolderDieCast<TMold> typeMold
      , TItem toFormat, int itemAt, string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder where TItem : ISpanFormattable =>
        base.CollectionNextItem(toFormat, itemAt, typeMold.Sb).ToInternalTypeBuilder(typeMold);
}
