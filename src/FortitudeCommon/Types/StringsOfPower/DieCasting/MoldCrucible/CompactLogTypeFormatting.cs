// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FieldContentHandlingExtensions;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactLogTypeFormatting : DefaultStringFormatter, IStyledTypeFormatting
{
    protected const string Dot        = ".";
    protected const string Cma        = ",";
    protected const string CmaSpc     = ", ";
    protected const string Spc        = " ";
    protected const string Cln        = ":";
    protected const string ClnSpc     = ": ";
    protected const char   BrcOpnChar = '{';
    protected const string BrcOpn     = "{";
    protected const string BrcCls     = "}";
    protected const string EqlsSpc    = "= ";

    protected const string SqBrktOpnSpc = "[ ";
    protected const string RndBrktOpn   = "(";
    protected const string RndBrktCls   = ")";
    protected const char RndBrktClsChar   = ')';

    public GraphTrackingBuilder GraphBuilder { get; set; } = null!;

    public virtual CompactLogTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions)
    {
        GraphBuilder = graphTrackingBuilder;
        Options      = styleOptions;

        return this;
    }

    public virtual string Name => nameof(CompactLogTypeFormatting);
    public StyleOptions StyleOptions => (StyleOptions)Options;

    public virtual FormatFlags ResolveContentFormattingFlags<T>(IStringBuilder sb, T input
      , FormatFlags callerFormattingFlags, string? formatString = "", bool isFieldName = false)
    {
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags()) { return callerFormattingFlags; }

        FormatFlags setFlags = callerFormattingFlags;
        setFlags |= (FormatFlags)base.ResolveStringFormattingFlags
            (sb.LastNonWhiteChar(), input, (FormatSwitches)setFlags, formatString ?? "");

        var callerInputType = typeof(T);
        var typeofT         = input?.GetType() ?? callerInputType;
        if (typeofT.IsAnyTypeHoldingChars())
        {
            var notAsStringOrValue = !(callerFormattingFlags.HasAsStringContentFlag()
                                    || callerFormattingFlags.HasAsValueContentFlag());
            setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() && notAsStringOrValue
                ? EnsureFormattedDelimited
                : None;
            setFlags |= setFlags.ShouldDelimit() || callerFormattingFlags.HasAsStringContentFlag()
                ? EncodeAll
                : EncodeInnerContent;
        }
        if (typeofT.IsKeyedCollection())
        {
            var kvpType = typeof(KeyValuePair<,>);
            if (typeofT.IsFrameworkDictionary() || typeofT.IsArrayOfGeneric(kvpType) || typeofT.IsListOfGeneric(kvpType)
                // || typeofT.IsJustReadOnlyListOfGeneric(kvpType) 
             || typeofT.IsSpanOfGeneric(kvpType) || typeofT.IsReadOnlySpanOfGeneric(kvpType)
             || callerInputType.IsJustEnumerableOfGeneric(kvpType) || callerInputType.IsJustEnumeratorOfGeneric(kvpType))
            {
                setFlags |= LogSuppressTypeNames;
            }
        }
        return setFlags;
    }

    public virtual FormatFlags ResolveContentAsValueFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var typeOfT               = typeof(T);
        var isAnyTypeHoldingChars = typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (input == null && (fallbackValue.Length == 0 && !isAnyTypeHoldingChars)) return formatFlags | AsValueContent;
        if (isAnyTypeHoldingChars) return formatFlags | DisableAutoDelimiting | AsValueContent;
        return formatFlags | AsValueContent;
    }

    public virtual FormatFlags ResolveContentAsStringFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var typeOfT                     = typeof(T);
        var isSpanFormattableOrNullable = typeOfT.IsSpanFormattableOrNullable();
        var isAnyTypeHoldingChars       = typeOfT.IsAnyTypeHoldingChars() || typeOfT.IsChar() || typeOfT.IsNullableChar();
        if (isAnyTypeHoldingChars) return formatFlags | DisableAutoDelimiting | AsStringContent;
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable(fallbackValue, formatString);
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable) return formatFlags | DisableAutoDelimiting | AsStringContent;
        return formatFlags | AsStringContent;
    }

    public SkipTypeParts GetNextValueTypePartFlags<T>(ITheOneString tos, T forValue, Type actualType, FormatFlags formatFlags)
    {
        var isLastType = tos.WasThisLastVisitedAsAMoreDerivedType(forValue);
        if (forValue is ISpanFormattable || isLastType) { return SkipTypeParts.TypeStart | SkipTypeParts.TypeName | SkipTypeParts.TypeEnd; }
        return SkipTypeParts.None;
    }

    public SkipTypeParts GetNextValueTypePartFlags(ITheOneString tos, Type actualType, FormatFlags formatFlags)
    {
        if (actualType.IsCharSpan()) { return SkipTypeParts.TypeStart | SkipTypeParts.TypeName | SkipTypeParts.TypeEnd; }
        return SkipTypeParts.None;
    }

    public SkipTypeParts GetNextComplexTypePartFlags<T>(ITheOneString tos, T forValue, Type actualType, FormatFlags formatFlags)
    {
        var isLastType = tos.WasThisLastVisitedAsAMoreDerivedType(forValue);
        var skipParts  = SkipTypeParts.None;
        if (isLastType) { skipParts = SkipTypeParts.TypeStart | SkipTypeParts.TypeName | SkipTypeParts.TypeEnd; }
        skipParts |= formatFlags.HasSuppressOpening() ? SkipTypeParts.TypeStart : SkipTypeParts.None;
        skipParts |= formatFlags.HasSuppressClosing() ? SkipTypeParts.TypeEnd : SkipTypeParts.None;
        skipParts |= formatFlags.HasSuppressTypeNamesFlag() ? SkipTypeParts.TypeName : SkipTypeParts.None;
        return skipParts;
    }

    public SkipTypeParts GetNextComplexTypePartFlags(ITheOneString tos, Type actualType, FormatFlags formatFlags)
    {
        var skipParts = SkipTypeParts.None;
        skipParts |= formatFlags.HasSuppressOpening() ? SkipTypeParts.TypeStart : SkipTypeParts.None;
        skipParts |= formatFlags.HasSuppressClosing() ? SkipTypeParts.TypeEnd : SkipTypeParts.None;
        skipParts |= formatFlags.HasSuppressTypeNamesFlag() ? SkipTypeParts.TypeName : SkipTypeParts.None;
        return skipParts;
    }

    public virtual ContentSeparatorRanges StartContentTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;

        var alternativeName = moldInternal.TypeName;
        var buildingType    = moldInternal.TypeBeingBuilt;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag()
         && !(StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildingType.FullName?.StartsWith(s) ?? false))
           )
        {
            if (alternativeName.IsNotNullOrEmpty())
                sb.Append(alternativeName);
            else
                buildingType.AppendShortNameInCSharpFormat(sb);
            moldInternal.WroteTypeName = true;
        }
        return GraphBuilder.ContentEndToRanges(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishContentTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (moldInternal is { WroteTypeName: true, RemainingGraphDepth: > 0 })
        {
            // space considered content
            GraphBuilder.AppendContent(EqlsSpc);
        }
        return ContentSeparatorRanges.None;
    }

    public virtual ContentSeparatorRanges AppendContentTypeClosing(ITypeMolderDieCast moldInternal)
    {
        GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(moldInternal.Sb, DefaultCallerTypeFlags);
        return GraphBuilder.SnapshotLastAppendSequence(DefaultCallerTypeFlags);
    }

    public virtual ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;

        var alternativeName   = moldInternal.TypeName;
        var buildingType      = moldInternal.TypeBeingBuilt;
        var buildTypeFullName = buildingType.FullName ?? "";

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        
        if (moldInternal.AppendSettings.SkipTypeParts.HasTypeStartFlag()) return GraphBuilder.Complete(formatFlags);
        if (!moldInternal.AppendSettings.SkipTypeParts.HasTypeNameFlag() &&
            (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            (formatFlags.HasAddTypeNameFieldFlag() ||
             !StyleOptions.LogSuppressDisplayTypeNames.Any(s => buildTypeFullName.StartsWith(s)))))
        {
            if (alternativeName.IsNotNullOrEmpty()) { sb.Append(alternativeName); }
            else { buildingType.AppendShortNameInCSharpFormat(sb); }
            sb.Append(Spc);
        }
        return GraphBuilder
               .AppendContent(BrcOpn)
               .AppendPadding(Spc)
               .Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => ContentSeparatorRanges.None;

    public virtual int SizeFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder.GraphEncoder.CalculateEncodedLength(ClnSpc);

    public virtual SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        GraphBuilder
            .AppendSeparator(Cln)
            .AppendPadding(Spc)
            .Complete(formatFlags)
            .SeparatorPaddingRange!.Value;

    public virtual int SizeToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return formatFlags.UseMainFieldSeparator()
            ? GraphBuilder.ParentGraphEncoder.CalculateEncodedLength(StyleOptions.MainItemSeparator)
            : GraphBuilder.ParentGraphEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldSeparator);
    }

    public virtual Range? AddToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldSeparatorFlag()) return null;
        GraphBuilder.AppendSeparator(formatFlags.UseMainFieldSeparator() ? StyleOptions.MainItemSeparator : StyleOptions.AlternateFieldSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual int SizeNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return 0;
        return formatFlags.UseMainFieldPadding()
            ? GraphBuilder.ParentGraphEncoder.CalculateEncodedLength(StyleOptions.MainFieldPadding)
            : GraphBuilder.ParentGraphEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldPadding);
    }

    public virtual ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        GraphBuilder.AppendPadding(formatFlags.UseMainFieldPadding() ? StyleOptions.MainFieldPadding : StyleOptions.AlternateFieldPadding);
        return GraphBuilder.Complete(formatFlags);
    }

    public virtual int SizeFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return SizeToNextFieldSeparator(formatFlags) + SizeNextFieldPadding(formatFlags);
    }

    public virtual ContentSeparatorRanges AddToNextFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddToNextFieldSeparator(formatFlags);
        return AddNextFieldPadding(formatFlags);
    }

    public virtual ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var sb = moldInternal.Sb;

        var previousContentPadSpacing = GraphBuilder.LastContentSeparatorPaddingRanges;

        var lastContentChar = GraphBuilder.RemoveLastSeparatorAndPadding();

        if (moldInternal.AppendSettings.SkipTypeParts.HasTypeEndFlag())
        {
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags, true);
        }
        else
        {
            if (lastContentChar != BrcOpnChar) { GraphBuilder.StartAppendContent(Spc, sb, this, DefaultCallerTypeFlags).AppendContent(BrcCls); }
            else { GraphBuilder.StartAppendContent(BrcCls, sb, this, DefaultCallerTypeFlags); }
        }
        return GraphBuilder.Complete(previousContentPadSpacing.PreviousFormatFlags);
    }

    public IStringBuilder AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (((ReadOnlySpan<char>)formatString).HasFormatStringPadding() || ((ReadOnlySpan<char>)formatString).PrefixSuffixLength() > 0)
        {
            var        formatStringBufferSize  = StyleOptions.NullString.Length.CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(formatString);
            Span<char> justPrefixPaddingSuffix = stackalloc char[formatStringBufferSize];
            justPrefixPaddingSuffix = justPrefixPaddingSuffix.ToPrefixLayoutSuffixOnlyFormatString(formatString);
            Format(StyleOptions.NullString, 0, sb, justPrefixPaddingSuffix
                 , formatSwitches: FormatSwitches.DefaultCallerTypeFlags);
        }
        else { sb.Append(StyleOptions.NullString); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var kvpTypes = keyedCollectionType.GetKeyedCollectionTypes();
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => keyedCollectionType.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => kvpTypes?.Key.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionElementNames.Any(s => kvpTypes?.Value.FullName?.StartsWith(s) ?? false)))
        {
            keyedCollectionType.AppendShortNameInCSharpFormat(sb);
            GraphBuilder.AppendContent(Spc);
        }
        GraphBuilder.AppendContent(BrcOpn);
        AddNextFieldPadding(formatFlags);
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var lastContentChar = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (lastContentChar != BrcOpnChar)
            GraphBuilder.AppendContent(Spc).AppendContent(BrcCls);
        else { GraphBuilder.AppendContent(BrcCls); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue value
      , int retrieveCount
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags) where TMold : TypeMolder
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName).FieldEnd();
        typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName).FieldEnd();

        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TValue : struct, TVRevealBase
        where TVRevealBase : notnull
    {
        typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | IsFieldName).FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { FormatFieldContents(typeMold.Master, value.Value, valueStyler, valueFormatString, valueFlags); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = typeMold.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key, typeMold.Master); }
        typeMold.FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey? key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : struct, TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = typeMold.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key.Value, typeMold.Master); }
        typeMold.FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : TKRevealBase?
        where TValue : struct, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = typeMold.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key, typeMold.Master); }
        typeMold.FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { FormatFieldContents(typeMold.Master, value.Value, valueStyler, valueFormatString, valueFlags); }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType
      , TKey? key
      , TValue? value
      , int retrieveCount
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TKey : struct, TKRevealBase
        where TValue : struct, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var sb = typeMold.Sb;
        if (key == null) { AppendFormattedNull(sb, ""); }
        else { keyStyler(key.Value, typeMold.Master); }
        typeMold.FieldEnd();
        if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
        else { FormatFieldContents(typeMold.Master, value.Value, valueStyler, valueFormatString, valueFlags); }
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int previousItemNumber, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddToNextFieldSeparator(formatFlags);
        AddNextFieldPadding(formatFlags);
        return sb;
    }

    public virtual int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags) => sourceLength;

    public virtual int InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, int refId, int indexToInsertAt, WriteMethodType writeMethod
      , FormatFlags createTypeFlags, int currentEnd = -1, ITypeMolderDieCast? liveMoldInternal = null)
    {
        var sb              = insertBuilder.Sb;
        var preAppendLength = sb.Length;

        var refDigitsCount = refId.NumOfDigits();

        var toRestore  = GraphBuilder;
        GraphBuilder = insertBuilder;
        
        var alreadySupportsMultipleFields = writeMethod.SupportsMultipleFields();
        var needsBracesWrap               = false;
        var prefixInsertSize                    = 0;
        var suffixInsertSize                    = 0;
        
        // first entry spot maybe removed if empty so backtrack to open add one;
        var isEmpty    = indexToInsertAt - SizeNextFieldPadding(createTypeFlags) + 1 == currentEnd;
        if (!alreadySupportsMultipleFields)
        {
            needsBracesWrap = writeMethod == WriteMethodType.RawContent;
            if (needsBracesWrap)
            {
                prefixInsertSize += 1; // Open Brace Only close in done later
                GraphBuilder.IndentLevel++;
                prefixInsertSize += SizeNextFieldPadding(createTypeFlags);
                // insert $id added below
                prefixInsertSize += SizeFieldSeparatorAndPadding(createTypeFlags);
                prefixInsertSize += SizeFormatFieldName("$values".Length, createTypeFlags); 
                prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
            }
            else
            {
                prefixInsertSize += 2;
                isEmpty    =  true;
            }
        }
        else if (isEmpty)
        {
            indexToInsertAt -= SizeNextFieldPadding(createTypeFlags);
            prefixInsertSize      += SizeNextFieldPadding(createTypeFlags);
            GraphBuilder.IndentLevel--;
            // after inserted
            prefixInsertSize      += SizeNextFieldPadding(createTypeFlags);
            GraphBuilder.IndentLevel++;
        }
        else
        {
            // after inserted
            prefixInsertSize += SizeFieldSeparatorAndPadding(createTypeFlags);;
        }
        prefixInsertSize += SizeFormatFieldName("$id".Length, createTypeFlags);
        prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
        prefixInsertSize += refDigitsCount;
        
        insertBuilder.StartInsertAt(indexToInsertAt, prefixInsertSize);
        
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        if (!alreadySupportsMultipleFields)
        {
            if (needsBracesWrap)
            {
                GraphBuilder.AppendContent(BrcOpn);
                AddNextFieldPadding(createTypeFlags);
                
            }
            else { GraphBuilder.AppendContent(RndBrktOpn); }
        }
        else if (isEmpty)
        {
            AddNextFieldPadding(createTypeFlags);
        }
        GraphBuilder.AppendContent("$id");
        AppendFieldValueSeparator();
        Span<char> refSpan = stackalloc char[refDigitsCount];
        if (refId.TryFormat(refSpan, out var charsWritten, ""))
        {
            if (charsWritten != refDigitsCount) { Debugger.Break(); }
            GraphBuilder.AppendContent(refSpan);
        }
        else
        {
            Debugger.Break();
            GraphBuilder.AppendContent(refId.ToString());
        }
        if (!alreadySupportsMultipleFields)
        {
            if (needsBracesWrap)
            {
                AddToNextFieldSeparatorAndPadding(createTypeFlags);
                GraphBuilder.AppendContent("$values");
                AppendFieldValueSeparator();
                GraphBuilder.IndentLevel--;
                suffixInsertSize += SizeNextFieldPadding(createTypeFlags);
                suffixInsertSize += 1; // close Brace
        
                insertBuilder.StartInsertAt(currentEnd + prefixInsertSize, suffixInsertSize);
                AddNextFieldPadding(createTypeFlags);
                GraphBuilder.AppendContent(BrcCls);
            }
            else { GraphBuilder.AppendContent(RndBrktCls);}
        }
        else if(isEmpty)
        {
            GraphBuilder.IndentLevel--;
            AddNextFieldPadding(createTypeFlags);
        }
        else
        {
            AddToNextFieldSeparatorAndPadding(createTypeFlags);
        }
        GraphBuilder = toRestore;
        return sb.Length - preAppendLength;
    }

    public virtual int AppendExistingReferenceId(ITypeMolderDieCast moldInternal, int refId, WriteMethodType writeMethod, FormatFlags createTypeFlags)
    {
        var sb = moldInternal.Sb;

        var preAppendLength = sb.Length;
        
        var alreadySupportsMultipleFields = writeMethod.SupportsMultipleFields(); 
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, createTypeFlags);
        if (!alreadySupportsMultipleFields)
        {
            GraphBuilder.AppendContent(RndBrktOpn);
        }
        AppendFieldName(sb, "$ref");
        AppendFieldValueSeparator();
        FormatFieldContents(sb, refId, "", createTypeFlags);
        
        if (!alreadySupportsMultipleFields) { GraphBuilder.AppendContent(RndBrktCls); }
        else { 
            moldInternal.IsEmpty = false;
            AddToNextFieldSeparatorAndPadding(createTypeFlags); 
        }
        return sb.Length - preAppendLength;
    }

    public virtual int AppendInstanceInfoField(ITypeMolderDieCast moldInternal, string fieldName, ReadOnlySpan<char> description
      , WriteMethodType writeMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasNoRevisitCheck()) return 0; // fieldNames are marked with this and
        var sb = moldInternal.Sb;
        
        var alreadySupportsMultipleFields = writeMethod.SupportsMultipleFields(); 
        var preAppendLength               = sb.Length;
        if (!alreadySupportsMultipleFields)
        {
            if (sb[^1] == RndBrktClsChar)
            {
                sb.Length -= 1;
                AddToNextFieldSeparator(createTypeFlags);
                GraphBuilder.AppendPaddingAndComplete(" ", DefaultCallerTypeFlags);
            }
            else
            {
                GraphBuilder.StartAppendContentAndComplete(RndBrktOpn, sb, DefaultCallerTypeFlags);
            }
        }
        AppendFieldName(sb, fieldName);
        AppendFieldValueSeparator();
        FormatFieldContents(sb, description, 0,"\"{0}\"", formatFlags: createTypeFlags);

        if (!alreadySupportsMultipleFields)
        {
            GraphBuilder.StartAppendContentAndComplete(RndBrktCls, sb, DefaultCallerTypeFlags);
        }
        else { 
            moldInternal.IsEmpty = false;
            AddToNextFieldSeparatorAndPadding(createTypeFlags); 
        }
        return sb.Length - preAppendLength;
    }

    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartAppendContent(fieldName, sb, this, DefaultCallerTypeFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatString.IsNotNullOrEmpty())
            sb.AppendFormat(this, formatString, source);
        else
            sb.Append(source);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        Format(source, sb, formatString ?? "");
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (source != null)
            Format(source, sb, formatString ?? "");
        else
            base.Format(StyleOptions.NullString, 0, sb, formatString);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        FormatFieldContents(sb, source, formatString, formatFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatFieldContents(sb, source, formatString, formatFlags);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb
      , ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, char[] source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source
      , int sourceFrom = 0, string? formatString = null, int maxTransferCount = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        base.Format(source, sourceFrom, sb, formatString ?? "", maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TCloaked, TRevealBase>(ISecretStringOfPower tos, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb = tos.WriteBuffer;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (value == null) { AppendFormattedNull(sb, ""); }
        else { valueRevealer(value, tos); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TBearer>(ISecretStringOfPower tos, TBearer styledObj
      , string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (styledObj == null) { AppendFormattedNull(sb, callerFormatString, callerFormatFlags); }
        else { styledObj.RevealState(tos); }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        sb.AppendFormat(this, formatString ?? "", source, formatFlags: (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
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

        Format(source, sb, formatSpan, formatSwitches: (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
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

        Format(source, sb, formatSpan, formatSwitches: (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        formatString ??= "";

        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
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

        var markInsertStartIndex = sb.Length;
        base.Format(source, sb, formatSpan, (FormatSwitches)formatFlags);
        if (source is Enum)
        {
            var insertLength = sb.Length - markInsertStartIndex;
            var enumName     = source.GetType().Name;

            sb.InsertAt(enumName, markInsertStartIndex);
            sb.InsertAt(".", markInsertStartIndex + enumName.Length);
            Span<char> replaceWithPipeEnumName = stackalloc char[enumName.Length + 1 + 3];
            replaceWithPipeEnumName.OverWriteAt(0, " | ");
            replaceWithPipeEnumName.OverWriteAt(3, enumName);
            replaceWithPipeEnumName.OverWriteAt(enumName.Length + 3, ".");
            sb.Replace(", ", replaceWithPipeEnumName, markInsertStartIndex + enumName.Length + 1, insertLength);
        }
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        formatString ??= "";

        if (source == null)
        {
            AppendFormattedNull(sb, formatString, formatFlags);
            return sb;
        }

        return FormatFieldContents(sb, source.Value, formatString, formatFlags);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var allowEmptyContent = true;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags, allowEmptyContent);
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
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFallbackFieldContents<TAny>(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        formatFlags = ResolveContentFormattingFlags<TAny>(sb, default!, formatFlags, formatString);

        return FormatFieldContents(sb, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var allowEmptyContent = true;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags, allowEmptyContent);
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

        if (formatFlags.ShouldDelimit() && (formatFlags.HasAsCollectionFlag() || StyleOptions.CharBufferWritesAsCharCollection))
            sb.Append(SqBrktOpnChar);
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit() && (formatFlags.HasAsCollectionFlag() || StyleOptions.CharBufferWritesAsCharCollection))
            sb.Append(SqBrktClsChar);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var allowEmptyContent = true;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags, allowEmptyContent);
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
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        var allowEmptyContent = true;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags, allowEmptyContent);
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
        base.Format(source, sourceFrom, sb, formatSpan, maxTransferCount, formatSwitches: (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit()) sb.Append(DblQt);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TCloaked, TRevealBase>(ISecretStringOfPower tos, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb           = tos.WriteBuffer;
        var contentStart = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (value == null) { AppendFormattedNull(sb, ""); }
        else { valueRevealer(value, tos); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TBearer>(ISecretStringOfPower tos, TBearer styledObj,
        string? callerFormatString = null, FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;

        var contentStart = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (styledObj == null) { AppendFormattedNull(sb, ""); }
        else { styledObj.RevealState(tos); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal, Type itemElementType, bool? hasItems, Type collectionType
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (!hasItems.HasValue)
        {
            GraphBuilder.MarkContentEnd();
            return sb;
        }
        var coreElementType = collectionType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? itemElementType;
        if (formatFlags.DoesNotHaveLogSuppressTypeNamesFlag() &&
            !(StyleOptions.LogSuppressDisplayCollectionNames.Any(s => collectionType.FullName?.StartsWith(s) ?? false)
           && StyleOptions.LogSuppressDisplayCollectionNames.Any(s => coreElementType.FullName?.StartsWith(s) ?? false)))
        {
            sb.Append(RndBrktOpn);
            collectionType.AppendShortNameInCSharpFormat(sb);
            sb.Append(RndBrktCls).Append(Spc);
        }
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)formatFlags);
        if (hasItems == true)
            AddCollectionElementPadding(moldInternal, itemElementType, 1, formatFlags);
        else
            GraphBuilder.Complete(formatFlags);
        return sb;
    }

    public override int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }

        GraphBuilder.AppendContent(SqBrktOpn);
        return 2;
    }

    public override int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray()) { return 0; }
        GraphBuilder.ResetCurrent((FormatFlags)formatSwitches);
        GraphBuilder.MarkContentStart(destStartIndex);
        var charsAdded = destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
        GraphBuilder.MarkContentEnd(destStartIndex + charsAdded).Complete((FormatFlags)formatSwitches);
        return charsAdded;
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmt>(IStringBuilder sb, TFmt item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        FormatFieldContents(sb, item, formatString ?? "", formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmtStruct>(IStringBuilder sb, TFmtStruct? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        FormatFieldContents(sb, item.Value, formatString ?? "", formatFlags);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat<TCloaked, TCloakedBase>(ISecretStringOfPower tos
      , TCloaked? item, int retrieveCount, PalantírReveal<TCloakedBase> styler, string? callerFormatString
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull
    {
        var sb = tos.WriteBuffer;
        if (item == null) { return AppendFormattedNull(sb, callerFormatString); }
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        var contentStart = sb.Length;
        styler(item, tos);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        sb.AppendFormat(this, formatString ?? "", item);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        sb.AppendFormat(this, formatString ?? "", item);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextCharSeqFormat<TCharSeq>(IStringBuilder sb, TCharSeq item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        Format(item, 0, sb, formatString ?? "", item.Length);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        Format(item, 0, sb, formatString ?? "", item.Length);
        GraphBuilder.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextStringBearerFormat<TBearer>(ISecretStringOfPower tos, TBearer item, int retrieveCount
      , string? callerFormatString, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;
        if (item == null) { return AppendFormattedNull(sb, callerFormatString); }
        var contentStart = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        item.RevealState(tos);
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        GraphBuilder.MarkContentStart(contentStart);
        GraphBuilder.MarkContentEnd();
        return tos.WriteBuffer;
    }

    public virtual Range? AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return null;
        GraphBuilder.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(formatFlags);
        GraphBuilder.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return GraphBuilder.Complete(formatFlags);
    }

    public ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddCollectionElementSeparator(moldInternal, elementType, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(moldInternal, elementType, nextItemNumber, formatFlags);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.HasNoItemSeparatorFlag()) return 0;
        GraphBuilder.AppendSeparator(formatSwitches.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.CurrentSectionRanges.CurrentSeparatorRange?.Length() ?? 0;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.AppendPadding(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatSwitches.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.MarkSeparatorEnd();
        var charsAdded
            = destSpan.OverWriteAt(atIndex, formatSwitches.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        GraphBuilder.MarkPaddingEnd(atIndex + charsAdded);
        GraphBuilder.Complete(fmtFlgs);
        return charsAdded;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = GraphBuilder.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        GraphBuilder.AppendPadding(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return GraphBuilder.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public virtual IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!totalItemCount.HasValue)
        {
            GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
            if (StyleOptions.NullWritesEmpty)
            {
                CollectionStart(itemElementType, sb, false, (FormatSwitches)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormatSwitches)formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }

            GraphBuilder.MarkContentEnd();

            return sb;
        }

        return CollectionEnd(itemElementType, sb, totalItemCount.Value, (FormatSwitches)formatFlags).ToStringBuilder(sb);
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray()) { return 0; }

        var preClsLen = sb.Length;
        var lastChar  = GraphBuilder.RemoveLastSeparatorAndPadding();
        GraphBuilder.StartNextContentSeparatorPaddingSequence(sb, (FormatFlags)formatSwitches, true);
        if (lastChar != SqBrktOpnChar) { GraphBuilder.AppendContent(Spc); }
        GraphBuilder.AppendContent(SqBrktCls);
        return sb.Length - preClsLen;
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var originalDestIndex = destIndex;
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray()) { return 0; }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;

        var lastChar = GraphBuilder.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
        GraphBuilder.ResetCurrent((FormatFlags)formatSwitches, true);
        GraphBuilder.MarkContentStart(destIndex);

        if (lastChar != SqBrktOpnChar) destIndex += destSpan.OverWriteAt(destIndex, Spc);
        destIndex += destSpan.OverWriteAt(destIndex, SqBrktCls);
        GraphBuilder.MarkContentEnd(destIndex);
        return destIndex - originalDestIndex;
    }

    public override void StateReset()
    {
        GraphBuilder = null!;
        base.StateReset();
    }
}
