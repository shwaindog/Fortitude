// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FieldContentHandlingExtensions;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

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
        set => JsonOptions = value;
    }

    public virtual IStyledTypeFormatting Initialize(ITheOneString theOneString)
    {
        graphBuilder = AlwaysRecycler.Borrow<GraphTrackingBuilder>().Initialize(this, theOneString.WriteBuffer);
        Options      = theOneString.Settings;

        return this;
    }

    protected virtual IStyledTypeFormatting Initialize(GraphTrackingBuilder withGraphBuilder)
    {
        Options      = withGraphBuilder.StyleOptions!;
        graphBuilder = withGraphBuilder.Initialize(this);

        return this;
    }


    public GraphTrackingBuilder? GraphBuilder
    {
        get => graphBuilder ?? AlwaysRecycler.Borrow<GraphTrackingBuilder>();
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


    public IStyledTypeFormatting ContextStartPushToNext(StyleOptions withStyleOptions)
    {
        var next = Clone();
        next.PreviousContext        = this;
        next.AddedContextOnThisCall = true;
        next.StyleOptions           = withStyleOptions;
        return next;
    }

    public IStyledTypeFormatting ContextCompletePopToPrevious()
    {
        var previous = PreviousContext;
        if (previous != null && !ReferenceEquals(this, previous))
        {
            if (previous.GraphBuilder != null && GraphBuilder != null) { previous.GraphBuilder.SetHistory(GraphBuilder); }
            previous.Gb.RemoveCurrentSectionRangesFlags(AsStringContent | EncodeBounds);
            DecrementRefCount();
            return previous;
        }
        return this;
    }

    public override IEncodingTransfer LayoutEncoder
    {
        get
        {
            var formatFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags;
            return formatFlags.HasAsStringContentFlag() || formatFlags.HasEncodeAll()
                ? ContentEncoder
                : ContentEncoder.LayoutEncoder;
        }
        set => ContentEncoder = ContentEncoder.WithAttachedLayoutEncoder(value);
    }

    public virtual FormatFlags ResolveContentFormatFlags<T>(IStringBuilder sb, T input, FormatFlags callerFormattingFlags
      , string formatString = "", bool isFieldName = false)
    {
        if (callerFormattingFlags.HasDisableAddingAutoCallerTypeFlags() || input == null) { return callerFormattingFlags; }

        FormatFlags setFlags = callerFormattingFlags;
        setFlags |= (FormatFlags)ResolveStringFormattingFlags
            (sb.LastNonWhiteChar(), input, (FormatSwitches)setFlags, formatString);
        if (isFieldName) { setFlags |= EnsureFormattedDelimited; }
        if (!callerFormattingFlags.HasContentTreatmentFlags() && typeof(T) == typeof(object))
        {
            var actualType = input.GetType();
            if (!actualType.IsSpanFormattableCached() && !actualType.IsStringBearer() || actualType.IsAnyTypeHoldingChars())
            {
                setFlags |= EnsureFormattedDelimited;
            }
        }

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

    public virtual FormatFlags ResolveContentAsValueFormatFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
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

    public virtual FormatFlags ResolveContentAsStringFormatFlags<T>(T input, ReadOnlySpan<char> fallbackValue, string formatString = ""
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

    public (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags<T>(ITheOneString tos, T forValue, Type actualType
      , WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags) =>
        ResolveMoldWriteAsFormatFlags(tos, actualType, proposedWriteType, visitResult, formatFlags);

    public (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var resolvedFlags     = formatFlags;
        var resolvedWrittenAs = proposedWriteType;

        switch (proposedWriteType)
        {
            case AsRaw:
            case AsContent | AsComplex:
            case AsContent | AsSimple:
                resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType;
                if (visitResult.IsARevisit)
                {
                    resolvedFlags     |= ContentAllowComplexType;
                    resolvedWrittenAs =  (resolvedWrittenAs & ~AsSimple) | AsComplex | AsContent;
                    break;
                }
                if ((formatFlags.HasContentTreatmentFlags() && actualType.IsStringBearerOrNullableCached()))
                {
                    resolvedFlags |= ContentAllowComplexType;
                }

                resolvedWrittenAs =  (resolvedWrittenAs & ~AsComplex) | AsSimple | AsContent;
                resolvedFlags     |= SuppressOpening | SuppressClosing;
                break;
            case AsSimple | WrittenAsFlags.AsCollection:
                if (visitResult.IsARevisit) { resolvedWrittenAs = AsComplex | WrittenAsFlags.AsCollection; }
                break;
            default: resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType; break;
        }
        return (resolvedWrittenAs, resolvedFlags);
    }


    public WrittenAsFlags ResolveMoldCreateWriteAs<T>(ITheOneString tos, T forValue, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags) => proposedWriteType;

    public WrittenAsFlags ResolveMoldCreateWriteAs(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags) => proposedWriteType;

    public virtual ContentSeparatorRanges StartContentTypeOpening(ITypeMolderDieCast mdc
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        StartComplexTypeOpening(mdc, formatFlags);

    public virtual ContentSeparatorRanges FinishContentTypeOpening(ITypeMolderDieCast mdc
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        FinishComplexTypeOpening(mdc, formatFlags);

    public virtual ContentSeparatorRanges AppendContentTypeClosing(ITypeMolderDieCast mdc) =>
        AppendComplexTypeClosing(mdc);

    public virtual ContentSeparatorRanges StartComplexTypeOpening(ITypeMolderDieCast mdc
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.DoesNotHaveSuppressOpening())
        {
            mdc.WroteTypeOpen = true;
            return Gb.StartAppendContentAndComplete(BrcOpn, mdc.Sb, formatFlags);
        }
        return ContentSeparatorRanges.None;
    }

    public virtual ContentSeparatorRanges FinishComplexTypeOpening(ITypeMolderDieCast mdc
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return ContentSeparatorRanges.None;
    }

    public virtual int SizeFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        LayoutEncoder.CalculateEncodedLength(Cln);

    public virtual SeparatorPaddingRanges AppendFieldValueSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return Gb.AppendSeparator(Cln)
                 .SnapshotLastAppendSequence(formatFlags).SeparatorPaddingRange ?? new SeparatorPaddingRanges();
    }

    public virtual int SizeToNextFieldSeparator(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return formatFlags.UseMainFieldSeparator()
            ? LayoutEncoder.CalculateEncodedLength(StyleOptions.MainItemSeparator)
            : LayoutEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldSeparator);
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
            ? LayoutEncoder.CalculateEncodedLength(StyleOptions.MainFieldPadding)
            : LayoutEncoder.CalculateEncodedLength(StyleOptions.AlternateFieldPadding);
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

    public virtual ContentSeparatorRanges AppendComplexTypeClosing(ITypeMolderDieCast mdc)
    {
        var fmtFlags = mdc.CreateMoldFormatFlags;
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        if (fmtFlags.HasSuppressClosing()) return ContentSeparatorRanges.None;

        var sb = mdc.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;

        var lastContentChar = Gb.RemoveLastSeparatorAndPadding();

        Gb.StartNextContentSeparatorPaddingSequence( sb, previousContentPadSpacing.PreviousFormatFlags, true);
        if (lastContentChar != BrcOpnChar)
        {
            AddNextFieldPadding(mdc.CreateMoldFormatFlags);
            Gb.StartNextContentSeparatorPaddingSequence( sb, previousContentPadSpacing.PreviousFormatFlags);
        }
        Gb.StartAppendContent(BrcCls, sb, this, mdc.CreateMoldFormatFlags);
        mdc.WroteTypeClose = true;
        return Gb.Complete(previousContentPadSpacing.PreviousFormatFlags);
    }

    public WrittenAsFlags AppendFormattedNull(IStringBuilder sb, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        formatString ??= "";
        var writeFlags               = AsNull;
        var fieldNameShouldAddDblQts = !formatString.IsDblQtBounded() && !formatFlags.HasDisableAutoDelimiting();
        if (formatFlags.HasIsFieldNameFlag() && fieldNameShouldAddDblQts)
        {
            Gb.AppendDelimiter(DblQt);
            writeFlags |= AsString;
        }
        if (formatString.HasFormatStringPadding() || formatString.PrefixSuffixLength() > 0)
        {
            var        contentStart            = sb.Length;
            var        formatStringBufferSize  = StyleOptions.NullString.Length.CalculatePrefixPaddedAlignedAndSuffixFormatStringLength(formatString);
            Span<char> justPrefixPaddingSuffix = stackalloc char[formatStringBufferSize];
            justPrefixPaddingSuffix = justPrefixPaddingSuffix.ToPrefixLayoutSuffixOnlyFormatString(formatString);
            Format(StyleOptions.NullString, 0, sb, justPrefixPaddingSuffix
                 , formatSwitches: FormatSwitches.DefaultCallerTypeFlags);
            writeFlags |= sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        }
        else { sb.Append(StyleOptions.NullString); }
        if (formatFlags.HasIsFieldNameFlag() && fieldNameShouldAddDblQts) { Gb.AppendDelimiter(DblQt); }
        Gb.MarkContentEnd();
        return writeFlags;
    }

    public virtual (int, int) InsertInstanceReferenceId(GraphTrackingBuilder insertBuilder, int refId, Type actualType, int typeOpenIndex
      , WrittenAsFlags writtenAs, int indexToInsertAt, FormatFlags createTypeFlags, int contentLength = -1
      , ITypeMolderDieCast? liveMoldInternal = null)
    {
        var sb = insertBuilder.Sb;
        var instanceInfoFormatFlags =
            StyleOptions.InstanceMarkingWrapInstanceInfoFieldNamesInQuotes
                ? DefaultCallerTypeFlags
                : DisableFieldNameDelimiting;
        var instanceIdFormatFlags =
            StyleOptions.InstanceMarkingWrapInstanceIdInQuotes
                ? DefaultCallerTypeFlags
                : DisableAutoDelimiting;

        var preAppendLength = sb.Length;

        var refDigitsCount = refId.NumOfDigits();

        var toRestore = Gb;
        Gb = insertBuilder;

        var deltaIndent = 0;

        var prefixInsertSize = 0;
        var actualLength     = contentLength < 0 ? sb.Length - typeOpenIndex : contentLength;

        var alreadySupportsMultipleFields = writtenAs.SupportsMultipleFields();
        // first entry spot maybe removed if empty so backtrack to open add one;
        var firstFieldPad    = SizeNextFieldPadding(createTypeFlags);
        var isEmpty          = contentLength == 2;
        var fronInsertLength = actualLength - (indexToInsertAt - typeOpenIndex);
        int contentNewLines  = sb.SubSequenceOccurenceCount(indexToInsertAt, fronInsertLength, StyleOptions.NewLineStyle);
        if (!alreadySupportsMultipleFields)
        {
            if (liveMoldInternal != null) { liveMoldInternal.CurrentWriteMethod = writtenAs.ToMultiFieldEquivalent(); }
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
        prefixInsertSize += SizeFormatFieldName(3, instanceInfoFormatFlags);
        prefixInsertSize += SizeFieldValueSeparator(createTypeFlags);
        prefixInsertSize += SizeFormatFieldContents(refDigitsCount, instanceIdFormatFlags);

        insertBuilder.StartInsertAt(indexToInsertAt, prefixInsertSize);
        if (!alreadySupportsMultipleFields)
        {
            Gb.AppendContent(BrcOpn);
            AddNextFieldPadding(createTypeFlags);
        }
        else if (isEmpty) { AddNextFieldPadding(createTypeFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags, true);
        Gb.MarkContentStart(indexToInsertAt);
        if (instanceInfoFormatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) Gb.AppendContent(DblQt);
        Gb.AppendContent("$id");
        if (instanceInfoFormatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) Gb.AppendContent(DblQt);
        AppendFieldValueSeparator();
        if (instanceIdFormatFlags.DoesNotHaveDisableAutoDelimiting()) Gb.AppendContent(DblQt);
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
        if (instanceIdFormatFlags.DoesNotHaveDisableAutoDelimiting()) Gb.AppendContent(DblQt);
        if (!alreadySupportsMultipleFields)
        {
            AddToNextFieldSeparatorAndPadding(createTypeFlags);
            AppendInstanceValuesFieldName(typeof(object), createTypeFlags);
            if (contentNewLines > 0)
            {
                deltaIndent++;
                fronInsertLength = sb.IndentSubsequentLines(StyleOptions.NewLineStyle, StyleOptions.Indent
                                                          , indexToInsertAt + prefixInsertSize, fronInsertLength);
            }
            if (contentLength >= 0)
            {
                Gb.IndentLevel--;
                var suffixInsertSize = SizeNextFieldPadding(createTypeFlags);
                suffixInsertSize += 1; // }
                insertBuilder.StartInsertAt(indexToInsertAt + prefixInsertSize + fronInsertLength, suffixInsertSize);
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
        // toRestore.AddHighWaterMark();
        return (sb.Length - preAppendLength, deltaIndent);
    }

    public int AppendInstanceValuesFieldName(Type forType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var preAppendLength = Gb.Sb.Length;
        if (!formatFlags.HasDisableFieldNameDelimitingFlag()) Gb.AppendContent(DblQt);
        Gb.AppendContent("$values");
        if (!formatFlags.HasDisableFieldNameDelimitingFlag()) Gb.AppendContent(DblQt);
        AppendFieldValueSeparator();
        return Gb.Sb.Length - preAppendLength;
    }

    public virtual int AppendExistingReferenceId(ITypeMolderDieCast mdc, int refId, WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasNoRevisitCheck() || createTypeFlags.HasIsFieldNameFlag()) return 0;
        var sb = mdc.Sb;
        var instanceInfoFieldNameFormatFlags =
            StyleOptions.InstanceMarkingWrapInstanceInfoFieldNamesInQuotes
                ? DefaultCallerTypeFlags
                : DisableFieldNameDelimiting;
        var instanceIdFormatFlags =
            StyleOptions.InstanceMarkingWrapInstanceIdInQuotes
                ? DefaultCallerTypeFlags
                : DisableAutoDelimiting;

        var formatFlags = createTypeFlags.HasAsStringContentFlag()
                       && mdc.CurrentWriteMethod.HasAsComplexFlag()
                       && LayoutEncoder.Type != EncodingType.PassThrough
            ? createTypeFlags & ~(AsStringContent | EncodeBounds)
            : createTypeFlags;


        var alreadySupportsMultipleFields = currentWriteMethod.SupportsMultipleFields()
                                         || formatFlags.DoesNotHaveSuppressOpening() && formatFlags.DoesNotHaveSuppressClosing();
        var preAppendLength = sb.Length;
        if (!alreadySupportsMultipleFields)
        {
            mdc.CurrentWriteMethod = currentWriteMethod.ToMultiFieldEquivalent();
            StartComplexTypeOpening(mdc, formatFlags);
            FinishComplexTypeOpening(mdc, formatFlags);
        }
        AppendFieldName(mdc, "$ref", instanceInfoFieldNameFormatFlags);
        mdc.IsEmpty = false;
        AppendFieldValueSeparator();

        var        refDigitsCount = refId.NumOfDigits();
        Span<char> refSpan        = stackalloc char[refDigitsCount];
        if (refId.TryFormat(refSpan, out var charsWritten, ""))
        {
            if (charsWritten != refDigitsCount) { Debugger.Break(); }
            if (instanceIdFormatFlags.DoesNotHaveDisableAutoDelimiting()) LayoutEncoder.AppendTransfer(DblQt, 0, sb);
            LayoutEncoder.AppendTransfer(refSpan, 0, sb, charsWritten);
            if (instanceIdFormatFlags.DoesNotHaveDisableAutoDelimiting()) LayoutEncoder.AppendTransfer(DblQt, 0, sb);
        }
        Gb.AddHighWaterMark();
        AddToNextFieldSeparatorAndPadding(formatFlags);
        return sb.Length - preAppendLength;
    }

    public virtual int AppendInstanceInfoField(ITypeMolderDieCast mdc, string fieldName, ReadOnlySpan<char> description
      , WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasIsFieldNameFlag()) return 0; // fieldNames are marked with this and
        var sb = mdc.Sb;
        var formatter = createTypeFlags.HasAsStringContentFlag()
                     && mdc.CurrentWriteMethod.HasAsComplexFlag()
                     && LayoutEncoder.Type != EncodingType.PassThrough
            ? PreviousContextOrThis
            : this;

        var alreadySupportsMultipleFields = currentWriteMethod.SupportsMultipleFields();
        var preAppendLength               = sb.Length;
        if (!alreadySupportsMultipleFields)
        {
            mdc.CurrentWriteMethod = currentWriteMethod.ToMultiFieldEquivalent();
            StartComplexTypeOpening(mdc, createTypeFlags);
            FinishComplexTypeOpening(mdc, createTypeFlags);
        }
        formatter.AppendFieldName(mdc, fieldName);
        mdc.IsEmpty = false;

        formatter.AppendFieldValueSeparator();
        formatter.FormatFieldContents(mdc, description, 0, "\"{0}\"", formatFlags: createTypeFlags);
        formatter.AddToNextFieldSeparatorAndPadding(createTypeFlags);
        return sb.Length - preAppendLength;
    }

    public virtual WrittenAsFlags AppendFieldName(ITypeMolderDieCast mdc, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.AppendContent(fieldName);
        var writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag())
        {
            writtenAsFlags |= AsString;
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual WrittenAsFlags FormatFieldNameMatch<TAny>(ITypeMolderDieCast mdc, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        sb.AppendFormat(this, formatString ?? "", source, (FormatSwitches)formatFlags);
        var writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            writtenAsFlags |= AsString;
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Format(source, sb, formatString, (FormatSwitches)formatFlags).ToStringBuilder(sb);
        var writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            writtenAsFlags |= AsString;
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Format(source, sb, formatString, (FormatSwitches)formatFlags).ToStringBuilder(sb);
        var writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            writtenAsFlags |= AsString;
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual WrittenAsFlags FormatFieldName<TFmt>(ITypeMolderDieCast mdc, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        base.Format(source, sb, formatString, (FormatSwitches)formatFlags | FormatSwitches.DisableAutoDelimiting);
        var writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            writtenAsFlags |= AsString;
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual WrittenAsFlags FormatFieldName<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mdc.Sb;
        if (!source.HasValue) { return AppendFormattedNull(sb, formatString, formatFlags | IsFieldName); }
        return FormatFieldName(mdc, source.Value, formatString, formatFlags);
    }

    public virtual int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var size                                                          = 0;
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) size = 2 * LayoutEncoder.CalculateEncodedLength(DblQt);
        size += sourceLength;
        return size;
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldName(ITypeMolderDieCast mdc, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag() && !formatString.IsDblQtBounded())
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual AppendSummary FormatFieldName<TCloaked, TRevealBase>(ITypeMolderDieCast mdc, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mdc.Sb;
        var withMoldInherited = callerFormatFlags.MoldSingleGenerationPassFlags() | mdc.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited | NoRevisitCheck | IsFieldName);
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(withMoldInherited | NoRevisitCheck | IsFieldName);

        AppendSummary appendSummary;

        var visitNumber = mdc.MoldGraphVisit.VisitId;
        if (value == null)
        {
            var startAt        = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, "", withMoldInherited);
            return new AppendSummary(typeof(TCloaked), mdc.Master, new Range(startAt, sb.Length), writtenAsFlags, visitNumber);
        }
        else { appendSummary = valueRevealer(value, mdc.Master); }
        var contentStart = appendSummary.StartAt;
        if (sb.Length == contentStart) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, contentStart, value.GetType());
        ProcessAppendedRange(sb, contentStart);
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag()
         || mdc.CreateMoldFormatFlags.HasAsStringContentFlag()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);

        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        var charsInserted = addAsDelimiter
            ? Gb.InsertDelimiter(DblQt, sb, contentStart)
            : Gb.InsertContent(DblQt, sb, contentStart);
        mdc.Master.ShiftRegisteredFromCharOffset(contentStart, 1);
        charsInserted += addAsDelimiter
            ? Gb.AppendDelimiter(DblQt, sb)
            : Gb.AppendContent(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mdc.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mdc.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldName<TBearer>(ITypeMolderDieCast mdc, TBearer styledObj, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb                = mdc.Sb;
        var withMoldInherited = callerFormatFlags | mdc.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited | NoRevisitCheck | IsFieldName);
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(withMoldInherited | NoRevisitCheck | IsFieldName);
        AppendSummary appendSummary;

        var visitNumber = mdc.MoldGraphVisit.VisitId;
        if (styledObj == null)
        {
            var startAt        = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, "");
            return new AppendSummary(typeof(TBearer), mdc.Master, new Range(startAt, sb.Length), writtenAsFlags, visitNumber);
        }
        else { appendSummary = styledObj.RevealState(mdc.Master); }
        var contentStart = appendSummary.StartAt;
        if (sb.Length == contentStart) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, contentStart, styledObj.GetType());
        ProcessAppendedRange(sb, contentStart);
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag()
         || mdc.CreateMoldFormatFlags.HasAsStringContentFlag()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);

        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        var charsInserted = addAsDelimiter
            ? Gb.InsertDelimiter(DblQt, sb, contentStart)
            : Gb.InsertContent(DblQt, sb, contentStart);
        mdc.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += addAsDelimiter
            ? Gb.AppendDelimiter(DblQt, sb)
            : Gb.AppendContent(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mdc.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mdc.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual int SizeFormatFieldContents(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var size                                                 = 0;
        if (formatFlags.DoesNotHaveDisableAutoDelimiting()) size = 2 * LayoutEncoder.CalculateEncodedLength(DblQt);
        size += sourceLength;
        return size;
    }

    public virtual WrittenAsFlags FormatFieldContentsMatch<TAny>(ITypeMolderDieCast mdc, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (source == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        var addAsDelimiter        = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        var contentStart          = sb.Length;
        var fmtStringDblQtBounded = formatString.IsDblQtBounded();
        var shouldDelimit         = formatFlags.ShouldDelimit();
        var fmtSwitches           = (FormatSwitches)formatFlags;
        var isNotAsValue          = !fmtSwitches.HasAsValueContentFlag();
        if (isNotAsValue) { fmtSwitches |= FormatSwitches.AsStringContent; }
        if (shouldDelimit && !fmtStringDblQtBounded && isNotAsValue)
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        sb.AppendFormat(this, formatString ?? "", source, fmtSwitches);
        if (shouldDelimit && !fmtStringDblQtBounded && isNotAsValue)
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        Format(source, sb, formatString, (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        if (source != null) return FormatFieldContents(mdc, source.Value, formatString, formatFlags: formatFlags);

        return AppendFormattedNull(sb, formatString, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldContents<TFmt>(ITypeMolderDieCast mdc, TFmt? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        formatFlags  =   ResolveContentFormatFlags(sb, source, formatFlags);
        formatString ??= "";
        var contentStart = sb.Length;
        base.Format(source, sb, formatString, (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (!source.HasValue) { return AppendFormattedNull(sb, formatString, formatFlags); }
        formatString ??= "";
        var writtenAsFlags = FormatFieldContents(mdc, source.Value, formatString, formatFlags);
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;

        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount
                  , formatSwitches: (FormatSwitches)formatFlags | FormatSwitches.AsStringContent);
        WrittenAsFlags writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
        {
            writtenAsFlags |= AsString;
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual WrittenAsFlags FormatFallbackFieldContents<TAny>(ITypeMolderDieCast mdc, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        formatString ??= "";

        formatFlags = ResolveContentFormatFlags<TAny>(sb, default!, formatFlags, formatString);

        var typeofT = typeof(TAny);

        if (typeofT.IsEnumOrNullable()
         && (JsonOptions.WrapValuesInQuotes
          || ((!source.IsValidEnumIntegerSpan()
            || formatFlags.HasAsStringContentFlag())))) { formatFlags |= EnsureFormattedDelimited; }

        return FormatFieldContents(mdc, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb             = mdc.Sb;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        formatFlags = ResolveContentFormatFlags(sb, source, formatFlags, formatString ?? "");
        var fmtFlags = (FormatSwitches)formatFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (fmtFlags.TreatCharArrayAsString() || (!JsonOptions.CharBufferWritesAsCharCollection && formatFlags.DoesNotHaveAsCollectionFlag()))
        {
            var contentStart = sb.Length;
            if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
            {
                if (addAsDelimiter)
                    Gb.AppendDelimiter(DblQt);
                else
                    Gb.AppendContent(DblQt);
            }
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount, fmtFlags);
            WrittenAsFlags writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
            if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
            {
                writtenAsFlags |= AsString;
                if (addAsDelimiter)
                    Gb.AppendDelimiter(DblQt);
                else
                    Gb.AppendContent(DblQt);
            }
            Gb.MarkContentEnd();
            return writtenAsFlags;
        }
        var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

        var formattedLength = cappedLength + 256;

        Span<char> sourceInSpan = stackalloc char[Math.Min(4096, formattedLength)];

        RecyclingCharArray? largeBuffer = null;

        var asStringHandlingFlags = (fmtFlags & ~FormatSwitches.EncodeBounds);
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
        CollectionStart(charType, sb, cappedLength > 0, fmtFlags);
        Gb.Complete(formatFlags);
        var lastAdded    = 0;
        var previousChar = '\0';
        for (int i = 0; i < cappedLength; i++)
        {
            if (i > 0 && lastAdded > 0)
            {
                if (!formatFlags.TreatCharArrayAsString())
                    AddCollectionElementSeparatorAndPadding(charType, sb, i, fmtFlags);
                else
                    Gb.Complete(formatFlags);
            }

            if (lastAdded > 0 || i == 0) { Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags); }
            var nextChar = sourceInSpan[i];
            lastAdded = lastAdded == 0 && i > 0
                ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, ""
                                         , fmtFlags | FormatSwitches.AsCollection)
                : CollectionNextItemFormat(nextChar, i, sb, "", fmtFlags | FormatSwitches.AsCollection);
            if (lastAdded > 0) { Gb.MarkContentEnd(); }
            previousChar = lastAdded == 0 ? nextChar : '\0';
        }
        Gb.Complete(formatFlags);
        largeBuffer?.DecrementRefCount();
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        CollectionEnd(charType, sb, cappedLength, fmtFlags);
        Gb.MarkContentEnd();
        return WrittenAsFlags.AsCollection;
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb             = mdc.Sb;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        var fmtFlags       = (FormatSwitches)formatFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        if (fmtFlags.TreatCharArrayAsString() || (!JsonOptions.CharBufferWritesAsCharCollection && formatFlags.DoesNotHaveAsCollectionFlag()))
        {
            var contentStart = sb.Length;
            if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
            {
                if (addAsDelimiter)
                    Gb.AppendDelimiter(DblQt);
                else
                    Gb.AppendContent(DblQt);
            }
            base.Format(source, sourceFrom, sb, formatString, maxTransferCount, fmtFlags);
            WrittenAsFlags writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
            if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
            {
                writtenAsFlags |= AsString;
                if (addAsDelimiter)
                    Gb.AppendDelimiter(DblQt);
                else
                    Gb.AppendContent(DblQt);
            }
            Gb.MarkContentEnd();
            return writtenAsFlags;
        }
        var cappedFrom   = Math.Clamp(sourceFrom, 0, sb.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

        var        formattedLength = cappedLength + 256;
        Span<char> sourceInSpan    = stackalloc char[Math.Min(4096, formattedLength)];

        RecyclingCharArray? largeBuffer = null;

        var asStringHandlingFlags = (fmtFlags & ~FormatSwitches.EncodeBounds);
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
        CollectionStart(charType, sb, cappedLength > 0, fmtFlags);
        Gb.Complete(formatFlags);

        var lastAdded    = 0;
        var previousChar = '\0';
        for (int i = 0; i < cappedLength; i++)
        {
            if (i > 0 && lastAdded > 0)
            {
                if (!formatFlags.TreatCharArrayAsString())
                    AddCollectionElementSeparatorAndPadding(charType, sb, i, fmtFlags);
                else
                    Gb.Complete(formatFlags);
            }

            if (lastAdded > 0 || i == 0) { Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags); }
            var nextChar = sourceInSpan[i];
            lastAdded = lastAdded == 0 && i > 0
                ? CollectionNextItemFormat(new Rune(previousChar, nextChar), i, sb, ""
                                         , fmtFlags | FormatSwitches.AsCollection)
                : CollectionNextItemFormat(nextChar, i, sb, "", fmtFlags | FormatSwitches.AsCollection);
            if (lastAdded > 0) { Gb.MarkContentEnd(); }
            previousChar = lastAdded == 0 ? nextChar : '\0';
        }
        Gb.Complete(formatFlags);
        largeBuffer?.DecrementRefCount();
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        CollectionEnd(charType, sb, cappedLength, fmtFlags);
        Gb.MarkContentEnd();
        return WrittenAsFlags.AsCollection;
    }

    public virtual WrittenAsFlags FormatFieldContents(ITypeMolderDieCast mdc, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb             = mdc.Sb;
        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        var contentStart   = sb.Length;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
        {
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        base.Format(source, sourceFrom, sb, formatString, maxTransferCount, (FormatSwitches)formatFlags);
        WrittenAsFlags writtenAsFlags = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        if (formatFlags.ShouldDelimit() && (!formatString.IsDblQtBounded() || formatFlags.HasEncodeBounds()))
        {
            writtenAsFlags |= AsString;
            if (addAsDelimiter)
                Gb.AppendDelimiter(DblQt);
            else
                Gb.AppendContent(DblQt);
        }
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual AppendSummary FormatFieldContents<TCloaked, TRevealBase>(ITypeMolderDieCast mdc, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mdc.Sb;
        var withMoldInherited = callerFormatFlags.MoldSingleGenerationPassFlags() | mdc.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(withMoldInherited);
        var visitNumber = mdc.MoldGraphVisit.VisitId;
        if (value == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, "");
            return new AppendSummary(typeof(TCloaked), mdc.Master, new Range(startAtNull, sb.Length), writtenAsFlags
                                   , visitNumber);
        }
        var appendSummary = valueRevealer(value, mdc.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, contentStart, value.GetType());
        if (!callerFormatFlags.HasAsStringContentFlag()
         || mdc.CreateMoldFormatFlags.HasAsStringContentFlag()
         || withMoldInherited.HasDisableAutoDelimiting()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);

        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        var charsInserted = addAsDelimiter
            ? Gb.InsertDelimiter(DblQt, sb, contentStart)
            : Gb.InsertContent(DblQt, sb, contentStart);
        mdc.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += addAsDelimiter
            ? Gb.AppendDelimiter(DblQt, sb)
            : Gb.AppendContent(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mdc.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mdc.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldContents<TBearer>(ITypeMolderDieCast mdc, TBearer styledObj, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb                = mdc.Sb;
        var withMoldInherited = callerFormatFlags.MoldSingleGenerationPassFlags() | mdc.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(withMoldInherited);

        var visitNumber = mdc.MoldGraphVisit.VisitId;
        if (styledObj == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TBearer), mdc.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }
        var appendSummary = styledObj.RevealState(mdc.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, contentStart, styledObj.GetType());
        if (sb.Length != contentStart) ProcessAppendedRange(sb, contentStart);
        if (!callerFormatFlags.HasAsStringContentFlag()
         || withMoldInherited.HasDisableAutoDelimiting()
         || mdc.CreateMoldFormatFlags.HasAsStringContentFlag()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);

        var addAsDelimiter = !mdc.CreateMoldFormatFlags.HasAsStringContentFlag();
        var charsInserted = addAsDelimiter
            ? Gb.InsertDelimiter(DblQt, sb, contentStart)
            : Gb.InsertContent(DblQt, sb, contentStart);
        mdc.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += addAsDelimiter
            ? Gb.AppendDelimiter(DblQt, sb)
            : Gb.AppendContent(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mdc.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mdc.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).UpdateStringEndRange(charsInserted);
    }


    public virtual ContentSeparatorRanges StartKeyedCollectionOpen(ITypeMolderDieCast mdc, Type keyType, Type valueType
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb                  = mdc.Sb;
        var keyedCollectionType = mdc.TypeBeingBuilt;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList())) { Gb.AppendContent(SqBrktOpn); }
        else { Gb.AppendContent(BrcOpn); }
        AddNextFieldPadding(formatFlags);
        return Gb.Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishKeyedCollectionOpen(ITypeMolderDieCast mdc) => ContentSeparatorRanges.None;

    public virtual ContentSeparatorRanges AppendKeyedCollectionClose(ITypeMolderDieCast mdc, Type keyType, Type valueType
      , int totalItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasSuppressClosing()) return ContentSeparatorRanges.None;
        var sb = mdc.Sb;

        var keyedCollectionType = mdc.TypeBeingBuilt;
        
        var lastNonWhiteSpace = Gb.RemoveLastSeparatorAndPadding();
        if (totalItemCount > 0 && lastNonWhiteSpace != BrcOpnChar && formatFlags.CanAddNewLine())
        {
            AddNextFieldPadding(formatFlags);
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList())) { Gb.AppendContent(SqBrktCls); }
        else { Gb.AppendContent(BrcCls); }
        var range = Gb.Complete(formatFlags);
        Gb.AddHighWaterMark();
        return range;
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
            var sb = typeMold.Sb;
            StartComplexTypeOpening(typeMold, valueFlags);
            FinishComplexTypeOpening(typeMold, valueFlags);
            AppendFieldName(typeMold, "Key");
            sb.FieldEnd(this, valueFlags);
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags);
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(typeMold, "Value");
            sb.FieldEnd(this, valueFlags);
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
            AppendComplexTypeClosing(typeMold);
        }
        else
        {
            typeMold.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName);
            typeMold.FieldEnd();
            typeMold.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        }
        return typeMold;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
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
        if (mdc.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mdc.Sb;
            StartComplexTypeOpening(mdc, valueFlags);
            FinishComplexTypeOpening(mdc, valueFlags);
            AppendFieldName(mdc, "Key");
            sb.FieldEnd(this, valueFlags);
            mdc.AppendMatchFormattedOrNull(key, keyFormatString ?? "");
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mdc, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mdc);
        }
        else
        {
            mdc.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName);
            mdc.FieldEnd();
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
        }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
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
        if (mdc.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mdc.Sb;
            StartComplexTypeOpening(mdc, valueFlags);
            FinishComplexTypeOpening(mdc, valueFlags);
            AppendFieldName(mdc, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldName(mdc, key, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mdc, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mdc);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mdc.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(mdc, key, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            mdc.FieldEnd();
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
        }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
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
        if (mdc.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mdc.Sb;
            StartComplexTypeOpening(mdc, valueFlags);
            FinishComplexTypeOpening(mdc, valueFlags);
            AppendFieldName(mdc, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, key.Value, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mdc, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mdc);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mdc.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(mdc, key.Value, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }

            mdc.FieldEnd();
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value, valueStyler, valueFormatString, valueFlags); }
        }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
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
        if (mdc.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mdc.Sb;
            StartComplexTypeOpening(mdc, valueFlags);
            FinishComplexTypeOpening(mdc, valueFlags);
            AppendFieldName(mdc, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, key, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mdc, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value.Value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mdc);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mdc.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(mdc, key, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            mdc.FieldEnd();
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
        return mdc;
    }

    public ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
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
        if (mdc.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mdc.Sb;
            StartComplexTypeOpening(mdc, valueFlags);
            FinishComplexTypeOpening(mdc, valueFlags);
            AppendFieldName(mdc, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mdc.Sb, keyFormatString, valueFlags); }
            else { mdc.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mdc, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mdc.Sb, valueFormatString, valueFlags); }
            else { valueStyler(value.Value, mdc.Master); }
            AppendComplexTypeClosing(mdc);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mdc.Sb, keyFormatString, valueFlags | IsFieldName); }
            else { mdc.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName); }
            mdc.FieldEnd();
            if (value == null) { AppendFormattedNull(mdc.Sb, valueFormatString, valueFlags); }
            else { FormatFieldContents(mdc, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
        return mdc;
    }

    public virtual ITypeMolderDieCast<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        ITypeMolderDieCast<TMold> mdc
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
        if (mdc.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mdc.Sb;
            StartComplexTypeOpening(mdc, valueFlags);
            FinishComplexTypeOpening(mdc, valueFlags);
            AppendFieldName(mdc, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldName(mdc, key.Value, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mdc, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value.Value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mdc);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mdc.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(mdc, key.Value, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            mdc.FieldEnd();
            if (value == null) { AppendFormattedNull(mdc.Sb, "", valueFlags); }
            else { FormatFieldContents(mdc, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
        return mdc;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , int previousItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.AppendContent(Cma).Complete(formatFlags);
        return sb;
    }

    public virtual ContentSeparatorRanges StartFormatCollectionOpen(ITypeMolderDieCast mdc
      , Type itemElementType, bool? hasItems, Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        ContentSeparatorRanges.None;

    public virtual ContentSeparatorRanges FinishFormatCollectionOpen(ITypeMolderDieCast mdc, Type itemElementType, bool? hasItems
      , Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        mdc.WroteCollectionClose = false;
        if (mdc.SkipBody || mdc.WroteCollectionOpen || hasItems != true) { return ContentSeparatorRanges.None; }

        var  reg                              = mdc.Master.ActiveGraphRegistry;
        var  hasPreviouslyWroteCollectionOpen = false;
        int? firstOpenIndex                   = null;

        ITypeMolderDieCast? initialDc = mdc;

        if (mdc.MoldGraphVisit.HasRegisteredVisit)
        {
            GraphNodeVisit derivedMold = reg[mdc.MoldGraphVisit.VisitId.VisitIndex];
            if (mdc.MoldGraphVisit.IsBaseOfInitial)
            {
                var checkMoldIndex = derivedMold.ParentVisitId.VisitIndex;

                GraphNodeVisit checkMold = reg[checkMoldIndex];
                do
                {
                    derivedMold                      =   checkMold;
                    hasPreviouslyWroteCollectionOpen |=  checkMold.MoldState?.WroteCollectionOpen ?? false;
                    firstOpenIndex                   ??= hasPreviouslyWroteCollectionOpen ? checkMoldIndex : null;
                    checkMoldIndex                   =   reg[checkMoldIndex].ParentVisitId.VisitIndex;
                    checkMold                        =   reg[Math.Max(0, checkMoldIndex)];
                } while (checkMoldIndex >= 0
                      && (checkMold.MoldState?.MoldGraphVisit.IsBaseOfInitial ?? false)
                      && ReferenceEquals(checkMold.VisitedInstance, derivedMold.VisitedInstance));
            }
            initialDc = derivedMold.MoldState;
            if (hasPreviouslyWroteCollectionOpen)
            {
                if (firstOpenIndex != null)
                {
                    var openDc                                      = reg[firstOpenIndex.Value].MoldState;
                    if (openDc != null) openDc.WroteCollectionClose = true;
                }
                mdc.WroteCollectionClose = false;
                return ContentSeparatorRanges.None;
            }
        }
        var sb = mdc.Sb;
        if (hasItems != true) { return ContentSeparatorRanges.None; }
        mdc.WroteCollectionOpen = true;
        var inheritedFmtFlags = mdc.CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | formatFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
        if (mdc is ICollectionMolderDieCast { IsSimple: true } scmdc)
        {
            if (scmdc.SupportsMultipleFields) { AppendInstanceValuesFieldName(mdc.TypeBeingBuilt, inheritedFmtFlags); }
            else if (scmdc.MoldGraphVisit.IsBaseOfInitial)
            {
                if (initialDc != null && initialDc.CurrentWriteMethod.HasAsComplexFlag())
                {
                    AppendInstanceValuesFieldName(mdc.TypeBeingBuilt, inheritedFmtFlags);
                }
            }
        }
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)inheritedFmtFlags);
        return AddCollectionElementPadding(mdc, itemElementType, 1, inheritedFmtFlags);
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems, FormatSwitches formatSwitches
        = FormatSwitches.EncodeInnerContent)
    {
        var inheritedFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags | (FormatFlags)formatSwitches;
        if (inheritedFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
            if ((elementType.IsChar() && (formatSwitches.TreatCharArrayAsString()
                                       || (!JsonOptions.CharBufferWritesAsCharCollection && formatSwitches.DoesNotHaveAsCollectionFlag()))))
            {
                if (formatSwitches.ShouldDelimit())
                {
                    Gb.AppendContent(DblQt); // could be Unicode escaped
                }
                return Gb.Complete(inheritedFmtFlags).Length;
            }
            if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                return Gb.AppendContent(DblQt).Complete(inheritedFmtFlags).Length; // could be Unicode escaped
            }
            Gb.AppendContent(SqBrktOpn);
            return Gb.Complete(inheritedFmtFlags).Length;
        }
        return 0;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var charsAdded = 0;

        var inheritedFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags | (FormatFlags)formatSwitches;
        if (inheritedFmtFlags.DoesNotHaveSuppressOpening() || StyleOptions.Style.IsLog())
        {
            Gb.ResetCurrent(inheritedFmtFlags);
            Gb.MarkContentStart(destStartIndex);
            if ((elementType.IsChar()
              && (formatSwitches.TreatCharArrayAsString()
               || (!JsonOptions.CharBufferWritesAsCharCollection && formatSwitches.DoesNotHaveAsCollectionFlag()))))
            {
                if (formatSwitches.ShouldDelimit())
                {
                    charsAdded += destSpan.OverWriteAt(destStartIndex, DblQt); // could be Unicode escaped
                    Gb.MarkContentEnd(destStartIndex + charsAdded);
                    Gb.Complete(inheritedFmtFlags);
                }
                return charsAdded;
            }
            if (elementType.IsByte() && JsonOptions.ByteArrayWritesBase64String)
            {
                charsAdded += destSpan.OverWriteAt(destStartIndex, DblQt); // could be Unicode escaped
                Gb.MarkContentEnd(destStartIndex + charsAdded);
                Gb.Complete(inheritedFmtFlags);
                return charsAdded;
            }
            charsAdded += destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
            Gb.MarkContentEnd(destStartIndex + charsAdded);
            Gb.Complete(inheritedFmtFlags);
        }
        return charsAdded;
    }

    public WrittenAsFlags CollectionNextItemFormat(ITypeMolderDieCast mdc, bool item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public WrittenAsFlags CollectionNextItemFormat(ITypeMolderDieCast mdc, bool? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mdc.Sb;
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public AppendSummary CollectionNextItemFormat<TFmt>(ITypeMolderDieCast mdc, TFmt item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = item?.GetType() ?? typeof(TFmt);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var typeofT = item.GetType();
        formatString ??= "";
        var contentStart = sb.Length;
        if (formatString.IsNotJsonTypeOpenCloseBounded() && !formatFlags.HasDisableAutoDelimiting() && !formatFlags.HasAsValueContentFlag())
        {
            formatFlags |= JsonOptions.WrapValuesInQuotes || formatFlags.HasAsStringContentFlag() || typeofT.IsDoubleQuoteDelimitedSpanFormattable()
                ? EnsureFormattedDelimited
                : None;
        }
        CollectionNextItemFormat(item, retrieveCount, sb, formatString, (FormatSwitches)formatFlags);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public WrittenAsFlags CollectionNextItemFormat<TFmtStruct>(ITypeMolderDieCast mdc, TFmtStruct? item, int retrieveCount
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mdc.Sb;
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        var typeofT = item.GetType();
        formatString ??= "";
        var contentStart = sb.Length;
        if (formatString.IsNotJsonTypeOpenCloseBounded() && !formatFlags.HasDisableAutoDelimiting() && !formatFlags.HasAsValueContentFlag())
        {
            formatFlags |= JsonOptions.WrapValuesInQuotes || formatFlags.HasAsStringContentFlag() || typeofT.IsDoubleQuoteDelimitedSpanFormattable()
                ? EnsureFormattedDelimited
                : None;
        }
        CollectionNextItemFormat(item, retrieveCount, sb, formatString, (FormatSwitches)formatFlags);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual AppendSummary CollectionNextItemFormat<TCloaked, TCloakedBase>(ITypeMolderDieCast mdc
      , TCloaked? item, int retrieveCount, PalantírReveal<TCloakedBase> styler, string? callerFormatString
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull
    {
        var actualType = item?.GetType() ?? typeof(TCloaked);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, callerFormatString);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }

        var contentStart = sb.Length;
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(callerFormatFlags);
        var stateExtractResult = styler(item, mdc.Master);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return stateExtractResult;
    }

    public virtual AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, string? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var contentStart = sb.Length;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, char[]? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        if (formatFlags.HasAsCollectionFlag())
        {
            var writtenAsCollection = FormatFieldContents(mdc, item, 0, formatString, formatFlags: formatFlags) | WrittenAsFlags.AsCollection;
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsCollection, actualType);
        }
        var contentStart = sb.Length;

        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextCharSeqFormat<TCharSeq>(ITypeMolderDieCast mdc, TCharSeq? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?
    {
        var actualType = item?.GetType() ?? typeof(TCharSeq);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        if (formatFlags.HasAsCollectionFlag())
        {
            var writtenAsCollection = WrittenAsFlags.AsCollection | FormatFieldContents(mdc, item, 0, formatString, formatFlags: formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsCollection, actualType);
        }
        var contentStart = sb.Length;
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextItemFormat(ITypeMolderDieCast mdc, StringBuilder? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var contentStart = sb.Length;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextStringBearerFormat<TBearer>(ITypeMolderDieCast mdc, TBearer item, int retrieveCount
      , string? callerFormatString, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = item?.GetType() ?? typeof(TBearer);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, callerFormatString, callerFormatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var contentStart = sb.Length;
        mdc.Master.SetCallerFormatString(callerFormatString);
        mdc.Master.SetCallerFormatFlags(callerFormatFlags);
        var stateExtractResult = item.RevealState(mdc.Master);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return stateExtractResult;
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

    public virtual IStringBuilder FormatCollectionEnd(ITypeMolderDieCast mdc, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb                   = mdc.Sb;
        var inheritedFmtFlags    = mdc.CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | formatFlags;
        var inheritedFmtSwitches = (FormatSwitches)inheritedFmtFlags;
        if (mdc.SkipBody || mdc.WroteCollectionClose || mdc.SuppressCollectionClose)
        {
            if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(inheritedFmtFlags); }
            Gb.RemoveLastSeparatorAndPadding();
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
            Gb.MarkContentStart(sb.Length - 1);
            Gb.MarkContentEnd(sb.Length);
            return sb;
        }
        mdc.WroteCollectionClose = true;

        Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
        if (!(totalItemCount > 0))
        {
            if (totalItemCount.HasValue || StyleOptions.NullWritesEmpty)
            {
                if (!mdc.WroteCollectionOpen) CollectionStart(itemElementType, sb, true, inheritedFmtSwitches);
                CollectionEnd(itemElementType, sb, 0, inheritedFmtSwitches);
                Gb.Complete(formatFlags);
            }
            else { AppendFormattedNull(sb, formatString, formatFlags); }
            mdc.WroteCollectionOpen = false;
            return sb;
        }
        mdc.WroteCollectionOpen = false;
        CollectionEnd(itemElementType, sb, totalItemCount.Value, inheritedFmtSwitches);
        Gb.MarkContentEnd();
        Gb.Complete(formatFlags);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(sb.Length - 1);
        Gb.MarkContentEnd(sb.Length);
        return sb;
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = FormatSwitches.EncodeInnerContent)
    {
        var preAppendLen = sb.Length;

        var inheritedFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags | (FormatFlags)formatSwitches;
        if (inheritedFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags, true);
            if ((elementType.IsChar()
              && (inheritedFmtFlags.TreatCharArrayAsString()
               || (!JsonOptions.CharBufferWritesAsCharCollection && inheritedFmtFlags.DoesNotHaveAsCollectionFlag()))))
            {
                if (inheritedFmtFlags.ShouldDelimit()) Gb.AppendContent(DblQt);
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
        var inheritedFmtFlags = Gb.CurrentSectionRanges.StartedWithFormatFlags | (FormatFlags)formatSwitches;

        if (inheritedFmtFlags.DoesNotHaveSuppressClosing() || StyleOptions.Style.IsLog())
        {
            Gb.ResetCurrent(inheritedFmtFlags, true);
            if ((elementType.IsChar() && (inheritedFmtFlags.TreatCharArrayAsString()
                                       || (!JsonOptions.CharBufferWritesAsCharCollection && inheritedFmtFlags.DoesNotHaveAsCollectionFlag()))))
            {
                if (inheritedFmtFlags.ShouldDelimit())
                {
                    charsAdded += LayoutEncoder.OverwriteTransfer(DblQt, destSpan, destIndex);
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
                Gb.ResetCurrent(inheritedFmtFlags, true);
                Gb.MarkContentStart(destIndex);
                charsAdded += LayoutEncoder.OverwriteTransfer(BrcCls, destSpan, destIndex);
                Gb.MarkContentEnd(destIndex + charsAdded);
                return charsAdded;
            }
            Gb.RemoveLastSeparatorAndPadding(destSpan, ref destIndex);
            Gb.ResetCurrent(inheritedFmtFlags, true);
            Gb.MarkContentStart(destIndex);
            charsAdded += LayoutEncoder.OverwriteTransfer(SqBrktCls, destSpan, destIndex);
            Gb.MarkContentEnd(destIndex + charsAdded);
        }
        return charsAdded;
    }

    object ICloneable.Clone() => Clone();

    IStyledTypeFormatting IStyledTypeFormatting.Clone() => Clone();

    public override CompactJsonTypeFormatting Clone()
    {
        return AlwaysRecycler.Borrow<CompactJsonTypeFormatting>().CopyFrom(this, CopyMergeFlags.FullReplace);
    }

    public virtual CompactJsonTypeFormatting CopyFrom(CompactJsonTypeFormatting source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (graphBuilder != null)
        {
            graphBuilder.DecrementRefCount();
            graphBuilder = null;
        }
        var nextGb = source.Gb.Clone();
        Initialize(nextGb);

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

    public override string ToString() => $"{{ {GetType().Name}: {InstanceId}, {nameof(Gb)}: {Gb}, " +
                                         $"{nameof(FormatOptions)}: {FormatOptions?.ToString() ?? "null"} }}";
}
