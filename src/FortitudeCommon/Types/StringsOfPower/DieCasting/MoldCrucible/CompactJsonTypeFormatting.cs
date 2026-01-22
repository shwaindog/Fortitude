// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FieldContentHandlingExtensions;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class CompactJsonTypeFormatting : JsonFormatter, IStyledTypeFormatting, ICloneable<CompactJsonTypeFormatting>
  , ITransferState<CompactJsonTypeFormatting>
{
    private GraphTrackingBuilder? graphBuilder;

    protected const string Cln = ":";
    public virtual string Name => nameof(CompactJsonTypeFormatting);

    public StyleOptions StyleOptions
    {
        get
        {
            if (FormatOptions is not StringsOfPower.Options.StyleOptions)
            {
                Console.Out.WriteLine($"Error got {FormatOptions?.ToString() ?? "null"}");
            }
            return (StyleOptions)JsonOptions;
        }
        set
        {
            // Console.Out.WriteLine($"Setting {ToString()}  StyleOptions: {value?.ToString() ?? "null"}");
            JsonOptions = value!;
        }
    }

    public virtual IStyledTypeFormatting Initialize(GraphTrackingBuilder graphTrackingBuilder, StyleOptions styleOptions, IStringBuilder sb)
    {
        Gb      = graphTrackingBuilder;
        Options = styleOptions;
        graphTrackingBuilder.Initialize(styleOptions, sb);

        return this;
    }


    public GraphTrackingBuilder? GraphBuilder
    {
        get => graphBuilder ?? Recycler.Borrow<GraphTrackingBuilder>();
        set
        {
            if (ReferenceEquals(graphBuilder, value)) return;
            graphBuilder?.DecrementRefCount();
            value?.IncrementRefCount();
            graphBuilder = value;
        }
    }


    public GraphTrackingBuilder Gb
    {
        get => graphBuilder ?? throw new ArgumentException("Never expect this to be called and not set!");
        set => GraphBuilder = value;
    }

    public IStyledTypeFormatting? PreviousContext { get; set; }

    public IStyledTypeFormatting PreviousContextOrThis => PreviousContext ?? this;

    public bool AddedContextOnThisCall { get; set; }


    public IStyledTypeFormatting ContextStartPushToNext()
    {
        var next = Clone();
        next.PreviousContext        = this;
        next.AddedContextOnThisCall = true;
        return next;
    }

    public IStyledTypeFormatting ContextCompletePopToPrevious()
    {
        var previous = PreviousContext;
        if (previous != null)
        {
            if (previous.GraphBuilder != null && GraphBuilder != null) { previous.GraphBuilder.SetHistory(GraphBuilder); }
            DecrementRefCount();
            return previous;
        }
        return this;
    }

    public override IRecycler Recycler
    {
        get => base.Recycler ?? DataStructures.MemoryPools.Recycler.ThreadStaticRecycler;
        #pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        #pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        set => base.Recycler = value;
        #pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        #pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    }

    public override IEncodingTransfer LayoutEncoder
    {
        get
        {
            var formatFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
            return formatFlags.HasAsStringContentFlag() || formatFlags.HasEncodeAll()
                ? ContentEncoder
                : Gb.ParentGraphEncoder;
        }
        set => Gb.GraphEncoder = value;
    }

    public virtual FormatFlags ResolveContentFormattingFlags<T>(IStringBuilder sb, T input, FormatFlags callerFormattingFlags
      , string formatString = "", bool isFieldName = false)
    {
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags() || input == null) { return callerFormattingFlags; }

        FormatFlags setFlags = callerFormattingFlags;
        setFlags |= (FormatFlags)ResolveStringFormattingFlags
            (sb.LastNonWhiteChar(), input, (FormatSwitches)setFlags, formatString);
        if (isFieldName) { setFlags |= EnsureFormattedDelimited; }

        return setFlags;
    }

    public override FormatSwitches ResolveStringFormattingFlags<T>(
        char lastNonWhiteSpace, T input, FormatSwitches callerFormattingFlags
      , string formatString = "")
    {
        if (callerFormattingFlags.HasNoAutoAddCallerTypeFlag() || input == null) { return callerFormattingFlags; }

        FormatSwitches setFlags = callerFormattingFlags;

        setFlags |= base.ResolveStringFormattingFlags(lastNonWhiteSpace, input, callerFormattingFlags, formatString);

        var typeofT = typeof(T);
        if (typeofT.IsAnyTypeHoldingCharsCached())
        {
            var notAsStringOrValue = !(callerFormattingFlags.HasAsStringContentFlag()
                                    || callerFormattingFlags.HasAsValueContentFlag());
            setFlags |= !callerFormattingFlags.HasDisableAutoDelimiting() && notAsStringOrValue
                ? FormatSwitches.EnsureFormattedDelimited
                : FormatSwitchesExtensions.None;
            // setFlags |= setFlags.ShouldDelimit() && callerFormattingFlags.DoesNotHaveAsValueContentFlag() ? EncodeAll : EncodeInnerContent;
            setFlags |= setFlags.ShouldDelimit()
                ? (callerFormattingFlags.HasAsStringContentFlag()
                    ? FormatSwitches.EncodeAll
                    : FormatSwitches.EncodeInnerContent)
                : (callerFormattingFlags.HasReformatMultiLineFlag()
                    ? FormatSwitchesExtensions.None
                    : FormatSwitches.EncodeInnerContent);
        }
        return setFlags;
    }

    public virtual FormatFlags ResolveContentAsValueFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var typeOfT       = typeof(T);
        var modifiedFlags = formatFlags;
        if (formatFlags.DoesNotHaveReformatMultiLineFlag()) { modifiedFlags |= EncodeInnerContent; }
        var isSpanFormattableOrNullable = typeOfT.IsSpanFormattableOrNullableCached();
        // if (input == null && fallbackValue.Length > 0) return DefaultCallerTypeFlags;
        var isAnyTypeHoldingChars = typeOfT.IsAnyTypeHoldingCharsCached();
        if (isAnyTypeHoldingChars)
        {
            if (!(typeOfT.IsChar() || typeOfT.IsRune() || typeOfT.IsNullableChar() || typeOfT.IsNullableRune()))
                return modifiedFlags | DisableAutoDelimiting | AsValueContent;
        }
        var isDoubleQuoteDelimitedSpanFormattable = input.IsDoubleQuoteDelimitedSpanFormattable(fallbackValue, formatString);
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable) return modifiedFlags | EnsureFormattedDelimited | AsValueContent;
        return modifiedFlags | AsValueContent;
    }

    public virtual FormatFlags ResolveContentAsStringFormattingFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var typeOfT       = typeof(T);
        var modifiedFlags = formatFlags;
        if (formatFlags.DoesNotHaveReformatMultiLineFlag()) { modifiedFlags |= EncodeInnerContent; }
        var isSpanFormattableOrNullable = typeOfT.IsSpanFormattableOrNullableCached();
        var isAnyTypeHoldingChars       = typeOfT.IsAnyTypeHoldingCharsCached();
        if (isAnyTypeHoldingChars) return modifiedFlags | DisableAutoDelimiting | AsStringContent;
        var isJsonStringExemptType = typeOfT.IsJsonStringExemptTypeCached();
        var isDoubleQuoteDelimitedSpanFormattable
            = input.IsDoubleQuoteDelimitedSpanFormattable(fallbackValue, formatString) || !isJsonStringExemptType;
        if (isSpanFormattableOrNullable && isDoubleQuoteDelimitedSpanFormattable) return modifiedFlags | DisableAutoDelimiting | AsStringContent;
        return modifiedFlags | AsStringContent;
    }

    public FormatFlags GetFormatterContentHandlingFlags<T>(ITheOneString tos, T forValue, Type actualType, WriteMethodType proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags) =>
        GetFormatterContentHandlingFlags(tos, actualType, proposedWriteType, visitResult, formatFlags);

    public FormatFlags GetFormatterContentHandlingFlags(ITheOneString tos, Type actualType, WriteMethodType proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var resolvedFlags = formatFlags;

        switch (proposedWriteType)
        {
            case WriteMethodType.MoldSimpleContentType:
            case WriteMethodType.MoldComplexContentType:
                resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType;
                if (visitResult.HasExistingInstanceId ||
                    (actualType.IsStringBearerOrNullableCached()
                  && formatFlags.HasContentTreatmentFlags()
                  && formatFlags.DoesNotHaveIsFieldNameFlag()))
                {
                    resolvedFlags |= ContentAllowComplexType;
                    break;
                }
                resolvedFlags |= SuppressOpening | SuppressClosing;
                break;
            default: resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType; break;
        }
        return resolvedFlags;
    }

    public virtual ContentSeparatorRanges StartContentTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        StartComplexTypeOpening(moldInternal, formatFlags);

    public virtual ContentSeparatorRanges FinishContentTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        FinishComplexTypeOpening(moldInternal, formatFlags);

    public virtual ContentSeparatorRanges AppendContentTypeClosing(ITypeMolderDieCast moldInternal) =>
        AppendComplexTypeClosing(moldInternal);

    public virtual ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        formatFlags.DoesNotHaveSuppressOpening()
            ? Gb.StartAppendContentAndComplete(BrcOpn, moldInternal.Sb, formatFlags)
            : ContentSeparatorRanges.None;

    public virtual ContentSeparatorRanges FinishComplexTypeOpening(ITypeMolderDieCast moldInternal
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => ContentSeparatorRanges.None;

    public virtual int SizeFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        Gb.GraphEncoder.CalculateEncodedLength(Cln);

    public virtual SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        Gb.AppendSeparator(Cln)
          .SnapshotLastAppendSequence(formatFlags).SeparatorPaddingRange!.Value;

    public virtual int SizeToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return formatFlags.UseMainFieldSeparator()
            ? Gb.ParentGraphEncoder.CalculateEncodedLength(StyleOptions.MainItemSeparator)
            : Gb.ParentGraphEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldSeparator);
    }

    public virtual Range? AddToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldSeparatorFlag()) return null;
        Gb.AppendSeparator(formatFlags.UseMainFieldSeparator() ? StyleOptions.MainItemSeparator : StyleOptions.AlternateFieldSeparator);
        return Gb.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual int SizeNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return 0;
        return formatFlags.UseMainFieldPadding()
            ? Gb.ParentGraphEncoder.CalculateEncodedLength(StyleOptions.MainFieldPadding)
            : Gb.ParentGraphEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldPadding);
    }

    public virtual ContentSeparatorRanges AddNextFieldPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoFieldPaddingFlag()) return Gb.Complete(formatFlags);
        Gb.AppendPadding(formatFlags.UseMainFieldPadding() ? StyleOptions.MainFieldPadding : StyleOptions.AlternateFieldPadding);
        return Gb.Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges AddToNextFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddToNextFieldSeparator(formatFlags);
        return AddNextFieldPadding(formatFlags);
    }

    public int SizeFieldSeparatorAndPadding(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return SizeToNextFieldSeparator(formatFlags) + SizeNextFieldPadding(formatFlags);
    }

    public virtual ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast moldInternal)
    {
        var fmtFlags = moldInternal.CreateMoldFormatFlags;
        if (fmtFlags.HasSuppressClosing()) return ContentSeparatorRanges.None;

        var sb = moldInternal.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;

        var lastContentChar = Gb.RemoveLastSeparatorAndPadding();

        if (moldInternal.CreateMoldFormatFlags.HasSuppressClosing())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags, true);
        }
        else
        {
            if (lastContentChar != BrcOpnChar) { Gb.StartAppendContentAndComplete(BrcCls, sb, previousContentPadSpacing.PreviousFormatFlags); }
            else { Gb.StartAppendContent(BrcCls, sb, this, DefaultCallerTypeFlags); }
        }
        return Gb.Complete(previousContentPadSpacing.PreviousFormatFlags);
    }

    public IStringBuilder AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        formatString ??= "";
        var fieldNameShouldAddDbltQts = !formatString.IsDblQtBounded() && !formatFlags.HasDisableAutoDelimiting();
        if (formatFlags.HasIsFieldNameFlag() && fieldNameShouldAddDbltQts) { Gb.AppendDelimiter(DblQt); }
        if (formatString.HasFormatStringPadding() || formatString.PrefixSuffixLength() > 0)
        {
            var        formatStringBufferSize  = StyleOptions.NullString.Length.CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(formatString);
            Span<char> justPrefixPaddingSuffix = stackalloc char[formatStringBufferSize];
            justPrefixPaddingSuffix = justPrefixPaddingSuffix.ToPrefixLayoutSuffixOnlyFormatString(formatString);
            Format(StyleOptions.NullString, 0, sb, justPrefixPaddingSuffix
                 , formatSwitches: FormatSwitches.DefaultCallerTypeFlags);
        }
        else { sb.Append(StyleOptions.NullString); }
        if (formatFlags.HasIsFieldNameFlag() && fieldNameShouldAddDbltQts) { Gb.AppendDelimiter(DblQt); }
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual int InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, Type actualType, int refId, int indexToInsertAt
      , WriteMethodType writeMethod, FormatFlags createTypeFlags, int currentEnd = -1, ITypeMolderDieCast? liveMoldInternal = null)
    {
        if (createTypeFlags.HasNoRevisitCheck()) return 0;

        var sb              = insertBuilder.Sb;
        var preAppendLength = sb.Length;

        var refDigitsCount = refId.NumOfDigits();

        var toRestore = Gb;
        Gb = insertBuilder;

        var prefixInsertSize = 0;

        var alreadySupportsMultipleFields = writeMethod.SupportsMultipleFields();
        // first entry spot maybe removed if empty so backtrack to open add one;
        var firstFieldPad = SizeNextFieldPadding(createTypeFlags);
        var isEmpty       = indexToInsertAt - firstFieldPad + 1 == currentEnd;
        if (!alreadySupportsMultipleFields)
        {
            if (liveMoldInternal != null) { liveMoldInternal.CurrentWriteMethod = writeMethod.ToMultiFieldEquivalent(); }
            isEmpty          =  true;
            prefixInsertSize += 1; // {
            Gb.IndentLevel++;
            prefixInsertSize += SizeNextFieldPadding(createTypeFlags);
            // after inserted $id
            prefixInsertSize += SizeFieldSeparatorAndPadding(createTypeFlags);
            prefixInsertSize += SizeFormatFieldName("$values".Length, createTypeFlags);
            prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
        }
        else if (isEmpty)
        {
            indexToInsertAt  -= firstFieldPad;
            prefixInsertSize += firstFieldPad;
            Gb.IndentLevel--;
            // after inserted
            prefixInsertSize += SizeNextFieldPadding(createTypeFlags);
            Gb.IndentLevel++;
        }
        else
        {
            // after inserted
            prefixInsertSize += SizeFieldSeparatorAndPadding(createTypeFlags);
        }
        prefixInsertSize += SizeFormatFieldName(3, createTypeFlags);
        prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
        prefixInsertSize += refDigitsCount + (!createTypeFlags.HasDisableAutoDelimiting() ? 2 : 0); // bound by DblQt

        insertBuilder.StartInsertAt(indexToInsertAt, prefixInsertSize);
        if (!alreadySupportsMultipleFields)
        {
            Gb.AppendContent(BrcOpn);
            AddNextFieldPadding(createTypeFlags);
        }
        else if (isEmpty) { AddNextFieldPadding(createTypeFlags); }
        if (!createTypeFlags.HasDisableFieldNameDelimitingFlag()) Gb.AppendDelimiter(DblQt);
        Gb.AppendContent("$id");
        if (!createTypeFlags.HasDisableFieldNameDelimitingFlag()) Gb.AppendDelimiter(DblQt);
        AppendFieldValueSeparator();
        if (!createTypeFlags.HasDisableAutoDelimiting()) Gb.AppendDelimiter(DblQt);
        Span<char> refSpan = stackalloc char[refDigitsCount];
        if (refId.TryFormat(refSpan, out var charsWritten, ""))
        {
            if (charsWritten != refDigitsCount) { Debugger.Break(); }
            Gb.AppendContent(refSpan);
        }
        else
        {
            Debugger.Break();
            Gb.AppendContent(refId.ToString());
        }
        if (!createTypeFlags.HasDisableAutoDelimiting()) Gb.AppendDelimiter(DblQt);
        if (!alreadySupportsMultipleFields)
        {
            AddToNextFieldSeparatorAndPadding(createTypeFlags);
            AppendInstanceValuesFieldName(typeof(object), createTypeFlags);
            if (currentEnd >= 0)
            {
                Gb.IndentLevel--;
                var suffixInsertSize = SizeNextFieldPadding(createTypeFlags);
                suffixInsertSize += 1; // }
                insertBuilder.StartInsertAt(currentEnd + prefixInsertSize, suffixInsertSize);
                AddNextFieldPadding(createTypeFlags);
                Gb.AppendContent(BrcCls);
            }
        }
        else if (isEmpty)
        {
            Gb.IndentLevel--;
            AddNextFieldPadding(createTypeFlags);
        }
        else { AddToNextFieldSeparatorAndPadding(createTypeFlags); }
        Gb = toRestore;
        return sb.Length - preAppendLength;
    }

    public int AppendInstanceValuesFieldName(Type forType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var preAppendLength = Gb.Sb.Length;
        if (!formatFlags.HasDisableFieldNameDelimitingFlag()) Gb.AppendDelimiter(DblQt);
        Gb.AppendContent("$values");
        if (!formatFlags.HasDisableFieldNameDelimitingFlag()) Gb.AppendDelimiter(DblQt);
        AppendFieldValueSeparator();
        return Gb.Sb.Length - preAppendLength;
    }

    public virtual int AppendExistingReferenceId(ITypeMolderDieCast moldInternal, int refId, WriteMethodType writeMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasNoRevisitCheck() || createTypeFlags.HasIsFieldNameFlag()) return 0;
        var sb = moldInternal.Sb;

        var alreadySupportsMultipleFields = writeMethod.SupportsMultipleFields()
                                         || createTypeFlags.DoesNotHaveSuppressOpening() && createTypeFlags.DoesNotHaveSuppressClosing();
        var preAppendLength = sb.Length;
        if (!alreadySupportsMultipleFields)
        {
            moldInternal.CurrentWriteMethod = writeMethod.ToMultiFieldEquivalent();
            StartComplexTypeOpening(moldInternal, createTypeFlags);
            FinishComplexTypeOpening(moldInternal, createTypeFlags);
        }
        AppendFieldName(sb, "$ref");
        moldInternal.IsEmpty = false;
        AppendFieldValueSeparator();
        FormatFieldContents(sb, refId, "\"{0}\"", createTypeFlags);
        AddToNextFieldSeparatorAndPadding(createTypeFlags);
        return sb.Length - preAppendLength;
    }

    public virtual int AppendInstanceInfoField(ITypeMolderDieCast moldInternal, string fieldName, ReadOnlySpan<char> description
      , WriteMethodType writeMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasIsFieldNameFlag()) return 0; // fieldNames are marked with this and
        var sb = moldInternal.Sb;

        var alreadySupportsMultipleFields = writeMethod.SupportsMultipleFields();
        var preAppendLength               = sb.Length;
        if (!alreadySupportsMultipleFields)
        {
            moldInternal.CurrentWriteMethod = writeMethod.ToMultiFieldEquivalent();
            StartComplexTypeOpening(moldInternal, createTypeFlags);
            FinishComplexTypeOpening(moldInternal, createTypeFlags);
        }
        AppendFieldName(sb, fieldName);
        moldInternal.IsEmpty = false;
        AppendFieldValueSeparator();
        FormatFieldContents(sb, description, 0, "\"{0}\"", formatFlags: createTypeFlags);
        AddToNextFieldSeparatorAndPadding(createTypeFlags);
        return sb.Length - preAppendLength;
    }

    public virtual IStringBuilder AppendFieldName(IStringBuilder sb, ReadOnlySpan<char> fieldName, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) Gb.AppendContent(DblQt);
        Gb.AppendContent(fieldName);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) Gb.AppendContent(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldNameMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        sb.AppendFormat(this, formatString ?? "", source, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Format(source, sb, formatString, (FormatSwitches)formatFlags).ToStringBuilder(sb);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Format(source, sb, formatString, (FormatSwitches)formatFlags).ToStringBuilder(sb);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmt>(IStringBuilder sb, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        base.Format(source, sb, formatString, (FormatSwitches)formatFlags | FormatSwitches.DisableAutoDelimiting);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (!source.HasValue) { return AppendFormattedNull(sb, formatString, formatFlags | IsFieldName); }
        return FormatFieldName(sb, source.Value, formatString, formatFlags);
    }

    public virtual int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var size                                                          = 0;
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) size = 2 * Gb.GraphEncoder.CalculateEncodedLength(DblQt);
        size += sourceLength;
        return size;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded()) Gb.AppendDelimiter(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TCloaked, TRevealBase>(ISecretStringOfPower tos, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = tos.WriteBuffer;
        var withMoldInherited = callerFormatFlags | tos.CurrentTypeBuilder?.CreateFormatFlags ?? DefaultCallerTypeFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited | NoRevisitCheck | IsFieldName);
        var preAppendLen = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(withMoldInherited | NoRevisitCheck | IsFieldName);
        if (value == null) { AppendFormattedNull(sb, "", withMoldInherited); }
        else { valueRevealer(value, tos); }
        if (sb.Length == preAppendLen) return tos.WriteBuffer;
        ProcessAppendedRange(sb, preAppendLen);
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag() || sb[preAppendLen] == DblQtChar)
        {
            Gb.MarkContentEnd();
            return sb;
        }
        Gb.GraphEncoder.InsertTransfer(DblQt, sb, preAppendLen);
        Gb.GraphEncoder.AppendTransfer(DblQt, sb);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldName<TBearer>(ISecretStringOfPower tos, TBearer styledObj, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb                = tos.WriteBuffer;
        // var withMoldInherited = callerFormatFlags | tos.CurrentTypeBuilder?.CreateFormatFlags ?? DefaultCallerTypeFlags;
        var withMoldInherited = callerFormatFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited | NoRevisitCheck | IsFieldName);
        var preAppendLen = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(withMoldInherited | NoRevisitCheck | IsFieldName);
        if (styledObj == null) { AppendFormattedNull(sb, ""); }
        else { styledObj.RevealState(tos); }
        if (sb.Length == preAppendLen) return sb;
        ProcessAppendedRange(sb, preAppendLen);
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag() || sb[preAppendLen] == DblQtChar)
        {
            Gb.MarkContentEnd();
            return sb;
        }
        Gb.GraphEncoder.InsertTransfer(DblQt, sb, preAppendLen);
        Gb.GraphEncoder.AppendTransfer(DblQt, sb);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContentsMatch<TAny>(IStringBuilder sb, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (source == null)
        {
            Gb.MarkContentEnd();
            return sb;
        }
        string rawValue;
        if (source is JsonNode jsonNode) { rawValue = jsonNode.ToJsonString(); }
        else { rawValue                             = source.ToString() ?? ""; }
        if (formatFlags.ShouldDelimit()) Gb.AppendDelimiter(DblQt);
        sb.AppendFormat(this, formatString ?? "{0}", rawValue, (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit()) Gb.AppendDelimiter(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        Format(source, sb, formatString, (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (source != null)
            FormatFieldContents(sb, source.Value, formatString, formatFlags: formatFlags);
        else
            sb.Append(StyleOptions.NullString);
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmt>(IStringBuilder sb, TFmt? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        formatFlags  =   ResolveContentFormattingFlags(sb, source, formatFlags);
        formatString ??= "";
        // var previousParentEncoder = Gb.ParentGraphEncoder;
        // if (formatFlags.HasAsStringContentFlag())
        // {
        //     Gb.ParentGraphEncoder = Gb.GraphEncoder;
        // }
        base.Format(source, sb, formatString, (FormatSwitches)formatFlags);
        // if (formatFlags.HasAsStringContentFlag())
        // {
        //     Gb.ParentGraphEncoder = previousParentEncoder;
        // }
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TFmtStruct>(IStringBuilder sb, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (!source.HasValue) { return AppendFormattedNull(sb, formatString, formatFlags); }
        formatString ??= "";
        FormatFieldContents(sb, source.Value, formatString, formatFlags);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
        {
            if (formatFlags.HasAsStringContentFlag())
                Gb.AppendContent(DblQt);
            else
                Gb.AppendDelimiter(DblQt);
        }
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount
                  , formatSwitches: (FormatSwitches)formatFlags | FormatSwitches.AsStringContent);
        if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
        {
            if (formatFlags.HasAsStringContentFlag())
                Gb.AppendContent(DblQt);
            else
                Gb.AppendDelimiter(DblQt);
        }
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFallbackFieldContents<TAny>(IStringBuilder sb, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatString ??= "";

        formatFlags = ResolveContentFormattingFlags<TAny>(sb, default!, formatFlags, formatString);

        var typeofT = typeof(TAny);

        if (typeofT.IsEnumOrNullable()
         && (JsonOptions.WrapValuesInQuotes
          || ((!source.IsValidEnumIntegerSpan()
            || formatFlags.HasAsStringContentFlag())))) { formatFlags |= EnsureFormattedDelimited; }

        return FormatFieldContents(sb, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatFlags = ResolveContentFormattingFlags(sb, source, formatFlags, formatString ?? "");
        var fmtHndlingFlags = (FormatSwitches)formatFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (fmtHndlingFlags.TreatCharArrayAsString() || (!JsonOptions.CharBufferWritesAsCharCollection && formatFlags.DoesNotHaveAsCollectionFlag()))
        {
            if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
            {
                if (formatFlags.HasAsStringContentFlag())
                    Gb.AppendContent(DblQt);
                else
                    Gb.AppendDelimiter(DblQt);
            }
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount, fmtHndlingFlags);
            if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
            {
                if (formatFlags.HasAsStringContentFlag())
                    Gb.AppendContent(DblQt);
                else
                    Gb.AppendDelimiter(DblQt);
            }
        }
        else
        {
            var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
            var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

            var formattedLength = cappedLength + 256;

            Span<char> sourceInSpan = stackalloc char[Math.Min(4096, formattedLength)];

            RecyclingCharArray? largeBuffer = null;

            var asStringHandlingFlags = (fmtHndlingFlags & ~FormatSwitches.EncodeBounds);
            if (formattedLength < 4096)
            {
                cappedLength =
                    ICustomStringFormatter
                        .DefaultBufferFormatter
                        .Format(source, cappedFrom, sourceInSpan, formatString, 0, cappedLength, asStringHandlingFlags);
            }
            else
            {
                largeBuffer  = (formattedLength).SourceRecyclingCharArray();
                sourceInSpan = largeBuffer.RemainingAsSpan();
                cappedLength =
                    ICustomStringFormatter
                        .DefaultBufferFormatter
                        .Format(source, cappedFrom, sourceInSpan, formatString, 0, cappedLength, asStringHandlingFlags);
            }

            var charType = typeof(char);
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            CollectionStart(charType, sb, cappedLength > 0, fmtHndlingFlags);
            Gb.Complete(formatFlags);
            var lastAdded    = 0;
            var previousChar = '\0';
            for (int i = 0; i < cappedLength; i++)
            {
                if (i > 0 && lastAdded > 0)
                {
                    if (!formatFlags.TreatCharArrayAsString())
                        AddCollectionElementSeparatorAndPadding(charType, sb, i, fmtHndlingFlags);
                    else
                        Gb.Complete(formatFlags);
                }

                if (lastAdded > 0 || i == 0) { Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags); }
                var nextChar = sourceInSpan[i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, ""
                                             , fmtHndlingFlags | FormatSwitches.AsCollection)
                    : CollectionNextItemFormat(nextChar, i, sb, "", fmtHndlingFlags | FormatSwitches.AsCollection);
                if (lastAdded > 0) { Gb.MarkContentEnd(); }
                previousChar = lastAdded == 0 ? nextChar : '\0';
            }
            Gb.Complete(formatFlags);
            largeBuffer?.DecrementRefCount();
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            CollectionEnd(charType, sb, cappedLength, fmtHndlingFlags);
        }
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var fmtHndlingFlags = (FormatSwitches)formatFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        if (fmtHndlingFlags.TreatCharArrayAsString() || (!JsonOptions.CharBufferWritesAsCharCollection && formatFlags.DoesNotHaveAsCollectionFlag()))
        {
            if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
            {
                if (formatFlags.HasAsStringContentFlag())
                    Gb.AppendContent(DblQt);
                else
                    Gb.AppendDelimiter(DblQt);
            }
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount, fmtHndlingFlags);
            if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
            {
                if (formatFlags.HasAsStringContentFlag())
                    Gb.AppendContent(DblQt);
                else
                    Gb.AppendDelimiter(DblQt);
            }
        }
        else
        {
            var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
            var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

            var        formattedLength = cappedLength + 256;
            Span<char> sourceInSpan    = stackalloc char[Math.Min(4096, formattedLength)];

            RecyclingCharArray? largeBuffer = null;

            var asStringHandlingFlags = (fmtHndlingFlags & ~FormatSwitches.EncodeBounds);
            if (formattedLength < 4096)
            {
                cappedLength =
                    ICustomStringFormatter.DefaultBufferFormatter
                                          .Format(source, cappedFrom, sourceInSpan, formatString
                                                , 0, cappedLength, asStringHandlingFlags);
            }
            else
            {
                largeBuffer  = (formattedLength).SourceRecyclingCharArray();
                sourceInSpan = largeBuffer.RemainingAsSpan();
                cappedLength =
                    ICustomStringFormatter.DefaultBufferFormatter
                                          .Format(source, cappedFrom, sourceInSpan, formatString
                                                , 0, cappedLength, asStringHandlingFlags);
            }

            var charType = typeof(char);
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            CollectionStart(charType, sb, cappedLength > 0, fmtHndlingFlags);
            Gb.Complete(formatFlags);

            var lastAdded    = 0;
            var previousChar = '\0';
            for (int i = 0; i < cappedLength; i++)
            {
                if (i > 0 && lastAdded > 0)
                {
                    if (!formatFlags.TreatCharArrayAsString())
                        AddCollectionElementSeparatorAndPadding(charType, sb, i, fmtHndlingFlags);
                    else
                        Gb.Complete(formatFlags);
                }

                if (lastAdded > 0 || i == 0) { Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags); }
                var nextChar = sourceInSpan[i];
                lastAdded = lastAdded == 0 && i > 0
                    ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, ""
                                             , fmtHndlingFlags | FormatSwitches.AsCollection)
                    : CollectionNextItemFormat(nextChar, i, sb, "", fmtHndlingFlags | FormatSwitches.AsCollection);
                if (lastAdded > 0) { Gb.MarkContentEnd(); }
                previousChar = lastAdded == 0 ? nextChar : '\0';
            }
            Gb.Complete(formatFlags);
            largeBuffer?.DecrementRefCount();
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            CollectionEnd(charType, sb, cappedLength, fmtHndlingFlags);
        }
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents(IStringBuilder sb, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
        {
            if (formatFlags.HasAsStringContentFlag())
                Gb.AppendContent(DblQt);
            else
                Gb.AppendDelimiter(DblQt);
        }
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
        {
            if (formatFlags.HasAsStringContentFlag())
                Gb.AppendContent(DblQt);
            else
                Gb.AppendDelimiter(DblQt);
        }
        Gb.MarkContentEnd();
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
        // if (sb.Length != contentStart) ProcessAppendedRange(sb, contentStart);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder FormatFieldContents<TBearer>(ISecretStringOfPower tos, TBearer styledObj, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb           = tos.WriteBuffer;
        var contentStart = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        if (styledObj == null) { AppendFormattedNull(sb, ""); }
        else { styledObj.RevealState(tos); }
        if (sb.Length != contentStart) ProcessAppendedRange(sb, contentStart);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return sb;
    }


    public virtual IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList())) { Gb.AppendContent(SqBrktOpn); }
        else { Gb.AppendContent(BrcOpn); }
        Gb.Complete(formatFlags);
        return sb;
    }

    public virtual IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , int totalItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.RemoveLastSeparatorAndPadding();
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        if (!formatFlags.HasSuppressClosing())
        {
            if (StyleOptions.WriteKeyValuePairsAsCollection
             && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
                 keyedCollectionType.IsReadOnlyList())) { Gb.AppendContent(SqBrktCls); }
            else { Gb.AppendContent(BrcCls); }
        }
        Gb.MarkContentEnd();
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
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            StartComplexTypeOpening(typeMold, valueFlags);
            FinishComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this, valueFlags);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags);
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this, valueFlags);
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
            AppendComplexTypeClosing(typeMold);
        }
        else
        {
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName)
                    .FieldEnd();
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        }
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
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            StartComplexTypeOpening(typeMold, valueFlags);
            FinishComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this, valueFlags);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "");
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(typeMold);
        }
        else
        {
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName).FieldEnd();
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
        }
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
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            StartComplexTypeOpening(typeMold, valueFlags);
            FinishComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldName(typeMold.Master, key, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(typeMold);
        }
        else
        {
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(typeMold.Master, key, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            typeMold.FieldEnd();
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
        }
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
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            StartComplexTypeOpening(typeMold, valueFlags);
            FinishComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, key.Value, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(typeMold);
        }
        else
        {
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(typeMold.Master, key.Value, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }

            typeMold.FieldEnd();
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value, valueStyler, valueFormatString, valueFlags); }
        }
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
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            StartComplexTypeOpening(typeMold, valueFlags);
            FinishComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, key, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value.Value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(typeMold);
        }
        else
        {
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(typeMold.Master, key, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            typeMold.FieldEnd();
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
        return typeMold;
    }

    public ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> typeMold
      , Type keyedCollectionType, TKey key
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
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            StartComplexTypeOpening(typeMold, valueFlags);
            FinishComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(typeMold.Sb, keyFormatString, valueFlags); }
            else { typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(typeMold.Sb, valueFormatString, valueFlags); }
            else { valueStyler(value.Value, typeMold.Master); }
            AppendComplexTypeClosing(typeMold);
        }
        else
        {
            if (key == null) { AppendFormattedNull(typeMold.Sb, keyFormatString, valueFlags | IsFieldName); }
            else { typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName); }
            typeMold.FieldEnd();
            if (value == null) { AppendFormattedNull(typeMold.Sb, valueFormatString, valueFlags); }
            else { FormatFieldContents(typeMold.Master, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
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
        if (typeMold.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            StartComplexTypeOpening(typeMold, valueFlags);
            FinishComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold.Sb, "Key").FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldName(typeMold.Master, key.Value, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(typeMold.Sb, "Value").FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value.Value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(typeMold);
        }
        else
        {
            if (key == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(typeMold.Master, key.Value, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            typeMold.FieldEnd();
            if (value == null) { AppendFormattedNull(typeMold.Sb, "", valueFlags); }
            else { FormatFieldContents(typeMold.Master, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
        return typeMold;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , int previousItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.AppendContent(Cma).Complete(formatFlags);
        return sb;
    }

    public virtual IStringBuilder FormatCollectionStart(ITypeMolderDieCast moldInternal
      , Type itemElementType, bool? hasItems, Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!hasItems.HasValue) return sb;
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)formatFlags);
        AddCollectionElementPadding(moldInternal, itemElementType, 1, formatFlags);
        return sb;
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems, FormatSwitches formatSwitches
        = FormatSwitches.EncodeInnerContent)
    {
        var currFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (currFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, currFmtFlags);
            var contentStart = sb.Length;
            if ((elementType.IsChar()
              && (formatSwitches.TreatCharArrayAsString() || (!JsonOptions.CharBufferWritesAsCharCollection
                                                           && formatSwitches.DoesNotHaveAsCollectionFlag()))))
            {
                if (formatSwitches.ShouldDelimit())
                {
                    Gb.AppendContent(DblQt).Complete((FormatFlags)formatSwitches); // could be unicode escaped
                }
                return sb.Length - contentStart;
            }
            else if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                Gb.AppendContent(DblQt).Complete((FormatFlags)formatSwitches); // could be unicode escaped
                return sb.Length - contentStart;
            }
            Gb.AppendContent(SqBrktOpn);
            return sb.Length - contentStart;
        }
        return 0;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var charsAdded = 0;

        var currFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (currFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            Gb.ResetCurrent((FormatFlags)formatSwitches);
            Gb.MarkContentStart(destStartIndex);
            if ((elementType.IsChar()
              && (formatSwitches.TreatCharArrayAsString()
               || (!JsonOptions.CharBufferWritesAsCharCollection && formatSwitches.DoesNotHaveAsCollectionFlag()))))
            {
                if (formatSwitches.ShouldDelimit())
                {
                    charsAdded += destSpan.OverWriteAt(destStartIndex, DblQt); // could be unicode escaped
                    Gb.Complete((FormatFlags)formatSwitches);
                }
                return charsAdded;
            }
            if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded += destSpan.OverWriteAt(destStartIndex, DblQt); // could be unicode escaped
                Gb.Complete((FormatFlags)formatSwitches);
                return charsAdded;
            }
            charsAdded += destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
            Gb.MarkContentEnd(destStartIndex + charsAdded);
        }
        return charsAdded;
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat(IStringBuilder sb, bool? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmt>(IStringBuilder sb, TFmt item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        var typeofT = item.GetType();
        formatString ??= "";
        if (formatString.IsNotJsonTypeOpenCloseBounded() && !formatFlags.HasDisableAutoDelimiting() && !formatFlags.HasAsValueContentFlag())
        {
            formatFlags |= JsonOptions.WrapValuesInQuotes || formatFlags.HasAsStringContentFlag() || typeofT.IsDoubleQuoteDelimitedSpanFormattable()
                ? EnsureFormattedDelimited
                : None;
        }
        var preAppendLen = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString, (FormatSwitches)formatFlags);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(preAppendLen);
        Gb.MarkContentEnd();
        return sb;
    }

    public IStringBuilder CollectionNextItemFormat<TFmtStruct>(IStringBuilder sb, TFmtStruct? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        var typeofT = item.GetType();
        formatString ??= "";
        if (formatString.IsNotJsonTypeOpenCloseBounded() && !formatFlags.HasDisableAutoDelimiting() && !formatFlags.HasAsValueContentFlag())
        {
            formatFlags |= JsonOptions.WrapValuesInQuotes || formatFlags.HasAsStringContentFlag() || typeofT.IsDoubleQuoteDelimitedSpanFormattable()
                ? EnsureFormattedDelimited
                : None;
        }
        var preAppendLen = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString, (FormatSwitches)formatFlags);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(preAppendLen);
        Gb.MarkContentEnd();
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

        var contentStart = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        styler(item, tos);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, string? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, char[]? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        if (formatFlags.HasAsCollectionFlag())
        {
            FormatFieldContents(sb, item, 0, formatString, formatFlags: formatFlags);
            return sb;
        }
        var preAppendLen = sb.Length;
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(preAppendLen);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextCharSeqFormat<TCharSeq>(IStringBuilder sb, TCharSeq? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        if (formatFlags.HasAsCollectionFlag())
        {
            FormatFieldContents(sb, item, 0, formatString, formatFlags: formatFlags);
            return sb;
        }
        var preAppendLen = sb.Length;
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(preAppendLen);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextItemFormat(IStringBuilder sb, StringBuilder? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder CollectionNextStringBearerFormat<TBearer>(ISecretStringOfPower tos, TBearer item, int retrieveCount
      , string? callerFormatString, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var sb = tos.WriteBuffer;
        if (item == null) { return AppendFormattedNull(sb, ""); }
        var contentStart = sb.Length;
        tos.SetCallerFormatString(callerFormatString);
        tos.SetCallerFormatFlags(callerFormatFlags);
        item.RevealState(tos);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return sb;
    }

    public virtual IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if ((elementType == typeof(char) && (JsonOptions.CharBufferWritesAsCharCollection || formatFlags.HasAsStringContentFlag()))
         || (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String))
        {
            Gb.Complete(formatFlags);
            return sb;
        }

        Gb.AppendSeparator
            (formatFlags.UseMainItemSeparator()
                 ? Options.MainItemSeparator
                 : Options.AlternateItemSeparator);
        return sb;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.HasNoItemSeparatorFlag()) return 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) { return 0; }
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return 0; }
        var preAppendLen = sb.Length;
        Gb.AppendSeparator
            (formatSwitches.UseMainItemSeparator()
                 ? Options.MainItemSeparator
                 : Options.AlternateItemSeparator);
        return sb.Length - preAppendLen;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        if (formatSwitches.HasNoItemSeparatorFlag()) return 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) { return 0; }
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return 0; }

        Gb.MarkContentEnd();
        var addedChars =
            destSpan.OverWriteAt
                (atIndex,
                 formatSwitches.UseMainItemSeparator()
                     ? Options.MainItemSeparator
                     : Options.AlternateItemSeparator);
        Gb.MarkSeparatorEnd();
        return addedChars;
    }

    public virtual Range? AddCollectionElementSeparator(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return null;
        if (elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) { return null; }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return null; }
        Gb.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return Gb.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddCollectionElementPadding(ITypeMolderDieCast moldInternal, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(formatFlags);
        if (elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) { return Gb.Complete(formatFlags); }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return Gb.Complete(formatFlags); }
        Gb.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return Gb.Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(ITypeMolderDieCast moldInternal, Type elementType
      , int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddCollectionElementSeparator(moldInternal, elementType, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(moldInternal, elementType, nextItemNumber, formatFlags);
    }

    public override int AddCollectionElementPadding(Type collectionElementType, IStringBuilder sb, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        Gb.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public override int AddCollectionElementPadding(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber
      , FormatSwitches formatFlags = FormatSwitches.EncodeInnerContent)
    {
        var fmtFlgs = Gb.CurrentSectionRanges.StartedWithFormatFlags;
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
            return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
        Gb.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return Gb.Complete(fmtFlgs).SeparatorPaddingRange?.PaddingRange?.Length() ?? 0;
    }

    public virtual IStringBuilder FormatCollectionEnd(ITypeMolderDieCast moldInternal, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = moldInternal.Sb;
        if (!totalItemCount.HasValue)
        {
            if (StyleOptions.NullWritesEmpty)
            {
                Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
                CollectionStart(itemElementType, sb, false, (FormatSwitches)formatFlags);
                CollectionEnd(itemElementType, sb, 0, (FormatSwitches)formatFlags);
                Gb.Complete(formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }
            return sb;
        }

        CollectionEnd(itemElementType, sb, totalItemCount.Value, (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb;
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var preAppendLen = sb.Length;

        var prevFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags;
        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, (FormatFlags)formatSwitches, true);
            if ((elementType.IsChar()
              && (formatSwitches.TreatCharArrayAsString()
               || (!JsonOptions.CharBufferWritesAsCharCollection && formatSwitches.DoesNotHaveAsCollectionFlag()))))
            {
                if (formatSwitches.ShouldDelimit()) Gb.AppendContent(DblQt);
            }
            else if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                CompleteBase64Sequence(sb);
                Gb.AppendContent(DblQt);
            }
            else if (elementType == typeof(KeyValuePair<string, JsonNode>))
            {
                Gb.RemoveLastSeparatorAndPadding();
                Gb.AppendContent(BrcCls);
            }
            else
            {
                Gb.RemoveLastSeparatorAndPadding();
                Gb.AppendContent(SqBrktCls);
            }
        }

        return sb.Length - preAppendLen;
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var charsAdded = 0;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        var prevFmtFlags = Gb.LastContentSeparatorPaddingRanges.PreviousFormatFlags;

        if (prevFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            Gb.ResetCurrent((FormatFlags)formatSwitches, true);
            if ((elementType.IsChar() && (formatSwitches.TreatCharArrayAsString()
                                       || (!JsonOptions.CharBufferWritesAsCharCollection && formatSwitches.DoesNotHaveAsCollectionFlag()))))
            {
                if (formatSwitches.ShouldDelimit())
                {
                    charsAdded += Gb.GraphEncoder.OverwriteTransfer(DblQt, destSpan, destIndex);
                    Gb.MarkContentEnd(destIndex + charsAdded);
                    return charsAdded;
                }
                return 0;
            }
            if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded += CompleteBase64Sequence(destSpan, destIndex);
                Gb.AppendContent(DblQt);
                Gb.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            if (elementType == typeof(KeyValuePair<string, JsonNode>))
            {
                Gb.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
                Gb.ResetCurrent((FormatFlags)formatSwitches, true);
                Gb.MarkContentStart(destIndex);
                charsAdded += Gb.GraphEncoder.OverwriteTransfer(BrcCls, destSpan, destIndex);
                Gb.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            Gb.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
            Gb.ResetCurrent((FormatFlags)formatSwitches, true);
            Gb.MarkContentStart(destIndex);
            charsAdded += Gb.GraphEncoder.OverwriteTransfer(SqBrktCls, destSpan, destIndex);
            Gb.MarkContentEnd(destIndex + charsAdded);
        }
        return charsAdded;
    }

    object ICloneable.Clone() => Clone();

    public virtual CompactJsonTypeFormatting Clone()
    {
        return Recycler.Borrow<CompactJsonTypeFormatting>().CopyFrom(this, CopyMergeFlags.FullReplace);
    }

    public virtual ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        return CopyFrom((CompactJsonTypeFormatting)source, copyMergeFlags);
    }

    public virtual CompactJsonTypeFormatting CopyFrom(CompactJsonTypeFormatting source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (graphBuilder != null)
        {
            graphBuilder.DecrementRefCount();
            graphBuilder = null;
        }
        var nextGb = source.Gb.Clone();
        Initialize(nextGb, source.StyleOptions, source.Gb.Sb);

        return this;
    }

    public override void StateReset()
    {
        Console.Out.WriteLine($"Reset {ToString()}");
        Gb              = null!;
        PreviousContext = null;
        Options         = null!;
        base.StateReset();
    }

    public override string ToString() => $"{{ {GetType().Name}: {InstanceId}, {nameof(Gb)}: {Gb?.ToString() ?? "null"}, " +
                                         $"{nameof(FormatOptions)}: {FormatOptions?.ToString() ?? "null"} }}";
}
