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

    public override FormatSwitches ResolveStringFormattingFlags<T>(char lastNonWhiteSpace, T input, FormatSwitches callerFormattingFlags
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
      , WrittenAsFlags proposedWriteType, VisitResult visitResult, FormatFlags formatFlags) =>
        ResolveMoldWriteAsFormatFlags(tos, actualType, proposedWriteType, visitResult, formatFlags);

    public (WrittenAsFlags, FormatFlags) ResolveMoldWriteAsFormatFlags(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var resolvedFlags     = formatFlags;
        var resolvedWrittenAs = proposedWriteType;

        switch (proposedWriteType)
        {
            case AsObject | AsRaw:
                resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType;
                if (visitResult.IsARevisit)
                {
                    resolvedFlags     |= ContentAllowComplexType;
                    resolvedWrittenAs =  (resolvedWrittenAs & ~AsSimple) | AsComplex | AsRaw | AsObject;
                    break;
                }

                resolvedWrittenAs =  (resolvedWrittenAs & ~AsComplex) | AsSimple | AsRaw  | AsObject;
                break;
            case AsContent | AsRaw:
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
            case AsRaw | WrittenAsFlags.AsCollection:
                resolvedFlags     |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType;
                if (visitResult.IsARevisit) { resolvedWrittenAs = AsRaw | AsComplex | WrittenAsFlags.AsCollection; }
                else resolvedWrittenAs = AsRaw | AsSimple | WrittenAsFlags.AsCollection;
                break;
            case AsComplex | WrittenAsFlags.AsCollection:
            case AsSimple | WrittenAsFlags.AsCollection:
                if (visitResult.IsARevisit) { resolvedWrittenAs = AsComplex | WrittenAsFlags.AsCollection; }
                else resolvedWrittenAs = AsSimple | WrittenAsFlags.AsCollection;
                break;
            default: resolvedFlags |= formatFlags | ContentAllowText | ContentAllowText | ContentAllowAnyValueType | ContentAllowComplexType; break;
        }
        return (resolvedWrittenAs, resolvedFlags);
    }


    public WrittenAsFlags ResolveMoldCreateWriteAs<T>(ITheOneString tos, T forValue, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags) => proposedWriteType;

    public WrittenAsFlags ResolveMoldCreateWriteAs(ITheOneString tos, Type actualType, WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags) => proposedWriteType;

    public virtual ContentSeparatorRanges StartSimpleTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openingAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => ContentSeparatorRanges.None;
        // StartComplexTypeOpening( mws.InstanceOrContainer, mws, mws.CurrentWriteMethod, formatFlags);

    public virtual ContentSeparatorRanges FinishSimpleTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openingAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => ContentSeparatorRanges.None;
        // FinishComplexTypeOpening(mws.InstanceOrContainer, mws, mws.CurrentWriteMethod, formatFlags);

    public virtual ContentSeparatorRanges AppendSimpleTypeClosing<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openingAs)
    {
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        Gb.RemoveLastSeparatorAndPadding();
        return ContentSeparatorRanges.None;
    }
    // AppendComplexTypeClosing(mws.InstanceOrContainer, mws, openingAs);

    public virtual ContentSeparatorRanges StartComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openingAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.DoesNotHaveSuppressOpening())
        {
            mws.WroteOuterTypeOpen = true;
            return Gb.StartAppendContentAndComplete(BrcOpn, mws.Sb, formatFlags);
        }
        return ContentSeparatorRanges.None;
    }

    public virtual ContentSeparatorRanges FinishComplexTypeOpening<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags openingAs
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        return ContentSeparatorRanges.None;
    }

    public virtual ContentSeparatorRanges AppendComplexTypeClosing<T>(T instanceToOpen, IMoldWriteState mws, WrittenAsFlags closingAs)
    {
        var fmtFlags = mws.CreateMoldFormatFlags;
        if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(Gb.CurrentSectionRanges.StartedWithFormatFlags); }
        if (fmtFlags.HasSuppressClosing()) return ContentSeparatorRanges.None;

        var sb = mws.Sb;

        var previousContentPadSpacing = Gb.LastContentSeparatorPaddingRanges;

        var lastContentChar = Gb.RemoveLastSeparatorAndPadding();

        Gb.StartNextContentSeparatorPaddingSequence( sb, previousContentPadSpacing.PreviousFormatFlags, true);
        if (lastContentChar != BrcOpnChar)
        {
            AddNextFieldPadding(mws.CreateMoldFormatFlags);
            Gb.StartNextContentSeparatorPaddingSequence( sb, previousContentPadSpacing.PreviousFormatFlags);
        }
        Gb.StartAppendContent(BrcCls, sb, this, mws.CreateMoldFormatFlags);
        mws.WroteOuterTypeClose = true;
        return Gb.Complete(previousContentPadSpacing.PreviousFormatFlags);
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
      , IMoldWriteState? liveMoldInternal = null)
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

        int contentNewLines = sb.SubSequenceOccurrenceCount(indexToInsertAt, fronInsertLength, StyleOptions.NewLineStyle);
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
                Span<char> indent = stackalloc char[StyleOptions.IndentSize];
                indent.OverWriteRepatAt(0, StyleOptions.IndentChar, StyleOptions.IndentSize);
                fronInsertLength = sb.IndentSubsequentLines(StyleOptions.NewLineStyle, indent
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

    public virtual int AppendExistingReferenceId(IMoldWriteState mws, int refId, WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasNoRevisitCheck() || createTypeFlags.HasIsFieldNameFlag()) return 0;
        var sb = mws.Sb;
        var instanceInfoFieldNameFormatFlags =
            StyleOptions.InstanceMarkingWrapInstanceInfoFieldNamesInQuotes
                ? DefaultCallerTypeFlags
                : DisableFieldNameDelimiting;
        var instanceIdFormatFlags =
            StyleOptions.InstanceMarkingWrapInstanceIdInQuotes
                ? DefaultCallerTypeFlags
                : DisableAutoDelimiting;

        var formatFlags = createTypeFlags.HasAsStringContentFlag()
                       && mws.CurrentWriteMethod.HasAsComplexFlag()
                       && LayoutEncoder.Type != EncodingType.PassThrough
            ? createTypeFlags & ~(AsStringContent | EncodeBounds)
            : createTypeFlags;


        var alreadySupportsMultipleFields = currentWriteMethod.SupportsMultipleFields()
                                         || formatFlags.DoesNotHaveSuppressOpening() && formatFlags.DoesNotHaveSuppressClosing();
        var preAppendLength = sb.Length;
        if (!alreadySupportsMultipleFields)
        {
            mws.CurrentWriteMethod = currentWriteMethod.ToMultiFieldEquivalent();
            StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, formatFlags);
        }
        AppendFieldName(mws, "$ref", instanceInfoFieldNameFormatFlags);
        mws.IsEmpty = false;
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

    public virtual int AppendInstanceInfoField(IMoldWriteState mws, string fieldName, ReadOnlySpan<char> description
      , WrittenAsFlags currentWriteMethod, FormatFlags createTypeFlags)
    {
        if (createTypeFlags.HasIsFieldNameFlag()) return 0; // fieldNames are marked with this and
        var sb = mws.Sb;
        var formatter = createTypeFlags.HasAsStringContentFlag()
                     && mws.CurrentWriteMethod.HasAsComplexFlag()
                     && LayoutEncoder.Type != EncodingType.PassThrough
            ? PreviousContextOrThis
            : this;

        var alreadySupportsMultipleFields = currentWriteMethod.SupportsMultipleFields();
        var preAppendLength               = sb.Length;
        if (!alreadySupportsMultipleFields)
        {
            mws.CurrentWriteMethod = currentWriteMethod.ToMultiFieldEquivalent();
            StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, createTypeFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, createTypeFlags);
        }
        formatter.AppendFieldName(mws, fieldName);
        mws.IsEmpty = false;

        formatter.AppendFieldValueSeparator();
        formatter.FormatFieldContents(mws, description, 0, "\"{0}\"", formatFlags: createTypeFlags);
        formatter.AddToNextFieldSeparatorAndPadding(createTypeFlags);
        return sb.Length - preAppendLength;
    }

    public virtual WrittenAsFlags AppendFieldName(IMoldWriteState mws, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldNameMatch<TAny>(IMoldWriteState mws, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldName<TFmt>(IMoldWriteState mws, TFmt source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldName<TFmtStruct>(IMoldWriteState mws, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mws.Sb;
        if (!source.HasValue) { return AppendFormattedNull(sb, formatString, formatFlags | IsFieldName); }
        return FormatFieldName(mws, source.Value, formatString, formatFlags);
    }

    public virtual int SizeFormatFieldName(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var size                                                          = 0;
        if (formatFlags.DoesNotHaveDisableFieldNameDelimitingFlag()) size = 2 * LayoutEncoder.CalculateEncodedLength(DblQt);
        size += sourceLength;
        return size;
    }

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, ReadOnlySpan<char> source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldName(IMoldWriteState mws, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart   = sb.Length;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual AppendSummary FormatFieldName<TCloaked, TRevealBase>(IMoldWriteState mws, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mws.Sb;
        var withMoldInherited = callerFormatFlags.MoldSingleGenerationPassFlags() | mws.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited | NoRevisitCheck | IsFieldName);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited | NoRevisitCheck | IsFieldName);

        AppendSummary appendSummary;

        var visitNumber = mws.MoldGraphVisit.VisitId;
        if (value == null)
        {
            var startAt        = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, "", withMoldInherited);
            return new AppendSummary(typeof(TCloaked), mws.Master, new Range(startAt, sb.Length), writtenAsFlags, visitNumber);
        }
        else { appendSummary = valueRevealer(value, mws.Master); }
        var contentStart = appendSummary.StartAt;
        if (sb.Length == contentStart) return mws.Master.EmptyAppendAt(mws.TypeBeingBuilt, contentStart, value.GetType());
        ProcessAppendedRange(sb, contentStart);
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag()
         || mws.CreateMoldFormatFlags.HasAsStringContentFlag()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);

        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
        var charsInserted = addAsDelimiter
            ? Gb.InsertDelimiter(DblQt, sb, contentStart)
            : Gb.InsertContent(DblQt, sb, contentStart);
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, 1);
        charsInserted += addAsDelimiter
            ? Gb.AppendDelimiter(DblQt, sb)
            : Gb.AppendContent(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).ShiftStringEndRangeBy(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldName<TBearer>(IMoldWriteState mws, TBearer styledObj, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb                = mws.Sb;
        var withMoldInherited = callerFormatFlags | mws.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        Gb.StartNextContentSeparatorPaddingSequence(sb, withMoldInherited | NoRevisitCheck | IsFieldName);
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited | NoRevisitCheck | IsFieldName);
        AppendSummary appendSummary;

        var visitNumber = mws.MoldGraphVisit.VisitId;
        if (styledObj == null)
        {
            var startAt        = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, "");
            return new AppendSummary(typeof(TBearer), mws.Master, new Range(startAt, sb.Length), writtenAsFlags, visitNumber);
        }
        else { appendSummary = styledObj.RevealState(mws.Master); }
        var contentStart = appendSummary.StartAt;
        if (sb.Length == contentStart) return mws.Master.EmptyAppendAt(mws.TypeBeingBuilt, contentStart, styledObj.GetType());
        ProcessAppendedRange(sb, contentStart);
        if (withMoldInherited.HasDisableFieldNameDelimitingFlag()
         || mws.CreateMoldFormatFlags.HasAsStringContentFlag()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);

        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
        var charsInserted = addAsDelimiter
            ? Gb.InsertDelimiter(DblQt, sb, contentStart)
            : Gb.InsertContent(DblQt, sb, contentStart);
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += addAsDelimiter
            ? Gb.AppendDelimiter(DblQt, sb)
            : Gb.AppendContent(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).ShiftStringEndRangeBy(charsInserted);
    }

    public virtual int SizeFormatFieldContents(int sourceLength, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var size                                                 = 0;
        if (formatFlags.DoesNotHaveDisableAutoDelimiting()) size = 2 * LayoutEncoder.CalculateEncodedLength(DblQt);
        size += sourceLength;
        return size;
    }

    public virtual WrittenAsFlags FormatFieldContentsMatch<TAny>(IMoldWriteState mws, TAny source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (source == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        var addAsDelimiter        = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, bool source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        Format(source, sb, formatString, (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, bool? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        if (source != null) return FormatFieldContents(mws, source.Value, formatString, formatFlags: formatFlags);

        return AppendFormattedNull(sb, formatString, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldContents<TFmt>(IMoldWriteState mws, TFmt? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        formatFlags  =   ResolveContentFormatFlags(sb, source, formatFlags);
        formatString ??= "";
        var contentStart = sb.Length;
        base.Format(source, sb, formatString, (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public virtual WrittenAsFlags FormatFieldContents<TFmtStruct>(IMoldWriteState mws, TFmtStruct? source, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (!source.HasValue) { return AppendFormattedNull(sb, formatString, formatFlags); }
        formatString ??= "";
        var writtenAsFlags = FormatFieldContents(mws, source.Value, formatString, formatFlags);
        Gb.MarkContentEnd();
        return writtenAsFlags;
    }

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;

        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFallbackFieldContents<TAny>(IMoldWriteState mws, ReadOnlySpan<char> source, int sourceFrom = 0
      , string? formatString = null, int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        formatString ??= "";

        formatFlags = ResolveContentFormatFlags<TAny>(sb, default!, formatFlags, formatString);

        var typeofT = typeof(TAny);

        if (typeofT.IsEnumOrNullable()
         && (JsonOptions.WrapValuesInQuotes
          || ((!source.IsValidEnumIntegerSpan()
            || formatFlags.HasAsStringContentFlag())))) { formatFlags |= EnsureFormattedDelimited; }

        return FormatFieldContents(mws, source, sourceFrom, formatString, maxTransferCount, formatFlags);
    }

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, char[] source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb             = mws.Sb;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, ICharSequence source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb             = mws.Sb;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual WrittenAsFlags FormatFieldContents(IMoldWriteState mws, StringBuilder source, int sourceFrom = 0, string? formatString = null
      , int maxTransferCount = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb             = mws.Sb;
        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
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

    public virtual AppendSummary FormatFieldContents<TCloaked, TRevealBase>(IMoldWriteState mws, TCloaked value
      , PalantírReveal<TRevealBase> valueRevealer, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var sb                = mws.Sb;
        var withMoldInherited = callerFormatFlags.MoldSingleGenerationPassFlags() | mws.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited);
        var visitNumber = mws.MoldGraphVisit.VisitId;
        if (value == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, "");
            return new AppendSummary(typeof(TCloaked), mws.Master, new Range(startAtNull, sb.Length), writtenAsFlags
                                   , visitNumber);
        }
        var appendSummary = valueRevealer(value, mws.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mws.Master.EmptyAppendAt(mws.TypeBeingBuilt, contentStart, value.GetType());
        if (!callerFormatFlags.HasAsStringContentFlag()
         || mws.CreateMoldFormatFlags.HasAsStringContentFlag()
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

        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
        var charsInserted = addAsDelimiter
            ? Gb.InsertDelimiter(DblQt, sb, contentStart)
            : Gb.InsertContent(DblQt, sb, contentStart);
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += addAsDelimiter
            ? Gb.AppendDelimiter(DblQt, sb)
            : Gb.AppendContent(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).ShiftStringEndRangeBy(charsInserted);
    }

    public virtual AppendSummary FormatBearerFieldContents<TBearer>(IMoldWriteState mws, TBearer styledObj, string? callerFormatString = null
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?
    {
        var sb                = mws.Sb;
        var withMoldInherited = callerFormatFlags.MoldSingleGenerationPassFlags() | mws.CreateMoldFormatFlags.MoldMultiGenerationPassFlags();
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(withMoldInherited);

        var visitNumber = mws.MoldGraphVisit.VisitId;
        if (styledObj == null)
        {
            var startAtNull    = sb.Length;
            var writtenAsFlags = AppendFormattedNull(sb, callerFormatString);
            return new AppendSummary(typeof(TBearer), mws.Master, new Range(startAtNull, sb.Length), writtenAsFlags, visitNumber);
        }
        var appendSummary = styledObj.RevealState(mws.Master);
        var contentStart  = appendSummary.StartAt;
        if (sb.Length == contentStart) return mws.Master.EmptyAppendAt(mws.TypeBeingBuilt, contentStart, styledObj.GetType());
        if (sb.Length != contentStart) ProcessAppendedRange(sb, contentStart);
        if (!callerFormatFlags.HasAsStringContentFlag()
         || withMoldInherited.HasDisableAutoDelimiting()
         || mws.CreateMoldFormatFlags.HasAsStringContentFlag()
         || sb.WrittenAsFromFirstCharacters(contentStart, Gb) == AsString)
        {
            Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
            Gb.MarkContentStart(contentStart);
            Gb.MarkContentEnd();
            return appendSummary;
        }
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);

        var addAsDelimiter = !mws.CreateMoldFormatFlags.HasAsStringContentFlag();
        var charsInserted = addAsDelimiter
            ? Gb.InsertDelimiter(DblQt, sb, contentStart)
            : Gb.InsertContent(DblQt, sb, contentStart);
        mws.Master.ShiftRegisteredFromCharOffset(contentStart, charsInserted);
        charsInserted += addAsDelimiter
            ? Gb.AppendDelimiter(DblQt, sb)
            : Gb.AppendContent(DblQt, sb);
        Gb.MarkContentEnd();
        if (!mws.Settings.InstanceTrackingAllAsStringHaveLocalTracking && appendSummary.VisitNumber.VisitIndex >= 0)
        {
            mws.Master.UpdateVisitLength(appendSummary.VisitNumber, charsInserted);
        }
        return appendSummary.AddWrittenAsFlags(AsString).ShiftStringEndRangeBy(charsInserted);
    }


    public virtual ContentSeparatorRanges StartKeyedCollectionOpen(IMoldWriteState mws, Type keyType, Type valueType
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb                  = mws.Sb;
        var keyedCollectionType = mws.TypeBeingBuilt;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (StyleOptions.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList())) { Gb.AppendContent(SqBrktOpn); }
        else { Gb.AppendContent(BrcOpn); }
        AddNextFieldPadding(formatFlags);
        return Gb.Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges FinishKeyedCollectionOpen(IMoldWriteState mws) => ContentSeparatorRanges.None;

    public virtual ContentSeparatorRanges AppendKeyedCollectionClose(IMoldWriteState mws, Type keyType, Type valueType
      , int totalItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasSuppressClosing()) return ContentSeparatorRanges.None;
        var sb = mws.Sb;

        var keyedCollectionType = mws.TypeBeingBuilt;
        
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

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue>(
        IMoldWriteState<TMold> mws
      , Type keyedCollectionType
      , TKey key
      , TValue value
      , int retrieveCount
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags valueFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
    {
        if (mws.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mws.Sb;
            StartComplexTypeOpening(mws.InstanceOrType,  mws, mws.CurrentWriteMethod, valueFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            AppendFieldName(mws, "Key");
            sb.FieldEnd(this, valueFlags);
            mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags);
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mws, "Value");
            sb.FieldEnd(this, valueFlags);
            mws.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
            AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        }
        else
        {
            mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName);
            mws.FieldEnd();
            mws.AppendMatchFormattedOrNull(value, valueFormatString ?? "", valueFlags);
        }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        if (mws.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mws.Sb;
            StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            AppendFieldName(mws, "Key");
            sb.FieldEnd(this, valueFlags);
            mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "");
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mws, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        }
        else
        {
            mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName);
            mws.FieldEnd();
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
        }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        if (mws.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mws.Sb;
            StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            AppendFieldName(mws, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldName(mws, key, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mws, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mws.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(mws, key, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            mws.FieldEnd();
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
        }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        if (mws.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mws.Sb;
            StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            AppendFieldName(mws, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, key.Value, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mws, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mws.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(mws, key.Value, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }

            mws.FieldEnd();
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value, valueStyler, valueFormatString, valueFlags); }
        }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        if (mws.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mws.Sb;
            StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            AppendFieldName(mws, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, key, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mws, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value.Value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mws.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(mws, key, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            mws.FieldEnd();
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
        return mws;
    }

    public IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        if (mws.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mws.Sb;
            StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            AppendFieldName(mws, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mws.Sb, keyFormatString, valueFlags); }
            else { mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mws, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mws.Sb, valueFormatString, valueFlags); }
            else { valueStyler(value.Value, mws.Master); }
            AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mws.Sb, keyFormatString, valueFlags | IsFieldName); }
            else { mws.AppendMatchFormattedOrNull(key, keyFormatString ?? "", valueFlags | NoRevisitCheck | IsFieldName); }
            mws.FieldEnd();
            if (value == null) { AppendFormattedNull(mws.Sb, valueFormatString, valueFlags); }
            else { FormatFieldContents(mws, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
        return mws;
    }

    public virtual IMoldWriteState<TMold> AppendKeyValuePair<TMold, TKey, TValue, TKRevealBase, TVRevealBase>(
        IMoldWriteState<TMold> mws
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
        if (mws.Settings.WriteKeyValuePairsAsCollection
         && (keyedCollectionType.IsNotReadOnlyDictionaryType() || keyedCollectionType.IsArray() ||
             keyedCollectionType.IsReadOnlyList()))
        {
            var sb = mws.Sb;
            StartComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            FinishComplexTypeOpening(mws.InstanceOrType, mws, mws.CurrentWriteMethod, valueFlags);
            AppendFieldName(mws, "Key");
            sb.FieldEnd(this, valueFlags);
            if (key == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldName(mws, key.Value, keyStyler, callerFormatFlags: valueFlags); }
            AddToNextFieldSeparator(valueFlags);
            AppendFieldName(mws, "Value");
            sb.FieldEnd(this, valueFlags);
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value.Value, valueStyler, valueFormatString, valueFlags); }
            AppendComplexTypeClosing(mws.InstanceOrType, mws, mws.CurrentWriteMethod);
        }
        else
        {
            if (key == null) { AppendFormattedNull(mws.Sb, "", valueFlags | IsFieldName); }
            else { FormatFieldName(mws, key.Value, keyStyler, callerFormatFlags: valueFlags | NoRevisitCheck | IsFieldName); }
            mws.FieldEnd();
            if (value == null) { AppendFormattedNull(mws.Sb, "", valueFlags); }
            else { FormatFieldContents(mws, value.Value, valueStyler, valueFormatString, valueFlags); }
        }
        return mws;
    }

    public virtual IStringBuilder AppendKeyedCollectionNextItem(IStringBuilder sb, Type keyedCollectionType, Type keyType, Type valueType
      , int previousItemCount, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        Gb.AppendContent(Cma).Complete(formatFlags);
        return sb;
    }

    public virtual ContentSeparatorRanges AppendCollectionTypeOpen(IMoldWriteState mws
      , Type itemElementType, bool? hasItems, Type collectionType, FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        ContentSeparatorRanges.None;

    public virtual ContentSeparatorRanges AppendOpenCollection(IMoldWriteState mws, Type itemElementType, bool? hasItems
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        mws.WroteInnerTypeClose = false;
        if (mws.SkipBody || mws.WroteInnerTypeOpen || hasItems != true) { return ContentSeparatorRanges.None; }

        var  reg                              = mws.Master.ActiveGraphRegistry;
        var  hasPreviouslyWroteCollectionOpen = false;
        int? firstOpenIndex                   = null;

        IMoldWriteState? initialDc = mws;

        if (mws.MoldGraphVisit.HasRegisteredVisit)
        {
            GraphNodeVisit derivedMold = reg[mws.MoldGraphVisit.VisitId.VisitIndex];
            if (mws.MoldGraphVisit.IsBaseOfInitial)
            {
                var checkMoldIndex = derivedMold.ParentVisitId.VisitIndex;

                GraphNodeVisit checkMold = reg[checkMoldIndex];
                do
                {
                    derivedMold                      =   checkMold;
                    hasPreviouslyWroteCollectionOpen |=  checkMold.MoldState?.WroteInnerTypeOpen ?? false;
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
                    if (openDc != null) openDc.WroteInnerTypeClose = true;
                }
                mws.WroteInnerTypeClose = false;
                return ContentSeparatorRanges.None;
            }
        }
        var sb = mws.Sb;
        if (hasItems != true) { return ContentSeparatorRanges.None; }
        mws.WroteInnerTypeOpen = true;
        var inheritedFmtFlags = mws.CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | formatFlags;
        Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
        if (mws is ICollectionMoldWriteState { IsSimple: true } scmdc)
        {
            if (scmdc.SupportsMultipleFields) { AppendInstanceValuesFieldName(mws.TypeBeingBuilt, inheritedFmtFlags); }
            else if (scmdc.MoldGraphVisit.IsBaseOfInitial)
            {
                if (initialDc != null && initialDc.CurrentWriteMethod.HasAsComplexFlag())
                {
                    AppendInstanceValuesFieldName(mws.TypeBeingBuilt, inheritedFmtFlags);
                }
            }
        }
        CollectionStart(itemElementType, sb, hasItems.Value, (FormatSwitches)inheritedFmtFlags);
        return AddCollectionElementPadding(mws, itemElementType, 1, inheritedFmtFlags);
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

    public virtual int AppendCloseCollection(IMoldWriteState mws, int? resultsFoundCount, Type itemElementType
      , int? totalItemCount, string? formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb                   = mws.Sb;
        var inheritedFmtFlags    = mws.CreateMoldFormatFlags.MoldSingleGenerationPassFlags() | formatFlags;
        var inheritedFmtSwitches = (FormatSwitches)inheritedFmtFlags;
        var preAppendAt          = sb.Length;
        if (mws.SkipBody || mws.WroteInnerTypeClose)
        {
            if (Gb.CurrentSectionRanges.HasNonZeroLengthContent) { Gb.SnapshotLastAppendSequence(inheritedFmtFlags); }
            Gb.RemoveLastSeparatorAndPadding();
            return sb.Length - preAppendAt;
        }
        mws.WroteInnerTypeClose = true;
        Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
        if (!(totalItemCount > 0))
        {
            
            if (totalItemCount.HasValue || StyleOptions.NullWritesEmpty)
            {
                if (!mws.WroteInnerTypeOpen) CollectionStart(itemElementType, sb, true, inheritedFmtSwitches);
                CollectionEnd(itemElementType, sb, 0, inheritedFmtSwitches);
                Gb.Complete(formatFlags);
            }
            else
            {
                AppendFormattedNull(sb, formatString, formatFlags);
            }
            mws.WroteInnerTypeOpen = false;
            Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
            Gb.MarkContentStart(preAppendAt);
            Gb.MarkContentEnd();
            return sb.Length - preAppendAt;
            
        }
        mws.WroteInnerTypeOpen = false;
        CollectionEnd(itemElementType, sb, totalItemCount.Value, inheritedFmtSwitches);
        return sb.Length - preAppendAt;
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
                Gb.StartNextContentSeparatorPaddingSequence(sb, inheritedFmtFlags);
                Gb.MarkContentStart(preAppendLen);
                Gb.AppendContent(DblQt);
                Gb.MarkContentEnd();
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

    public WrittenAsFlags CollectionNextItemFormat(IMoldWriteState mws, bool item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public WrittenAsFlags CollectionNextItemFormat(IMoldWriteState mws, bool? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var sb = mws.Sb;
        if (item == null) { return AppendFormattedNull(sb, formatString, formatFlags); }
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        var contentStart = sb.Length;
        CollectionNextItemFormat(item, retrieveCount, sb, formatString ?? "", (FormatSwitches)formatFlags);
        Gb.MarkContentEnd();
        return sb.WrittenAsFromFirstCharacters(contentStart, Gb);
    }

    public AppendSummary CollectionNextItemFormat<TFmt>(IMoldWriteState mws, TFmt item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = item?.GetType() ?? typeof(TFmt);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
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
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public WrittenAsFlags CollectionNextItemFormat<TFmtStruct>(IMoldWriteState mws, TFmtStruct? item, int retrieveCount
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var sb = mws.Sb;
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

    public virtual AppendSummary CollectionNextItemFormat<TCloaked, TCloakedBase>(IMoldWriteState mws
      , TCloaked? item, int retrieveCount, PalantírReveal<TCloakedBase> styler, string? callerFormatString
      , FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TCloakedBase : notnull
    {
        var actualType = item?.GetType() ?? typeof(TCloaked);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, callerFormatString);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }

        var contentStart = sb.Length;
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(callerFormatFlags);
        var stateExtractResult = styler(item, mws.Master);
        Gb.StartNextContentSeparatorPaddingSequence(sb, DefaultCallerTypeFlags);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        return stateExtractResult;
    }

    public virtual AppendSummary CollectionNextItemFormat(IMoldWriteState mws, string? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var contentStart = sb.Length;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextItemFormat(IMoldWriteState mws, char[]? item, int retrieveCount, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        if (formatFlags.HasAsCollectionFlag())
        {
            var writtenAsCollection = FormatFieldContents(mws, item, 0, formatString, formatFlags: formatFlags) | WrittenAsFlags.AsCollection;
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsCollection, actualType);
        }
        var contentStart = sb.Length;

        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextCharSeqFormat<TCharSeq>(IMoldWriteState mws, TCharSeq? item, int retrieveCount
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?
    {
        var actualType = item?.GetType() ?? typeof(TCharSeq);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        if (formatFlags.HasAsCollectionFlag())
        {
            var writtenAsCollection = WrittenAsFlags.AsCollection | FormatFieldContents(mws, item, 0, formatString, formatFlags: formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsCollection, actualType);
        }
        var contentStart = sb.Length;
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags, true);
        Gb.MarkContentStart(contentStart);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextItemFormat(IMoldWriteState mws, StringBuilder? item
      , int retrieveCount, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, formatString, formatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var contentStart = sb.Length;
        Gb.StartNextContentSeparatorPaddingSequence(sb, formatFlags);
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Format(item, 0, sb, formatString ?? "");
        if (formatFlags.DoesNotHaveAsValueContentFlag()) sb.Append(DblQt);
        Gb.MarkContentEnd();
        var writtenAs = sb.WrittenAsFromFirstCharacters(contentStart, Gb);
        return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public virtual AppendSummary CollectionNextStringBearerFormat<TBearer>(IMoldWriteState mws, TBearer item, int retrieveCount
      , string? callerFormatString, FormatFlags callerFormatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = item?.GetType() ?? typeof(TBearer);
        var sb         = mws.Sb;
        var startAt    = sb.Length;
        if (item == null)
        {
            var writtenAsNull = AppendFormattedNull(sb, callerFormatString, callerFormatFlags);
            return mws.Master.UnregisteredAppend(mws.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var contentStart = sb.Length;
        mws.Master.SetCallerFormatString(callerFormatString);
        mws.Master.SetCallerFormatFlags(callerFormatFlags);
        var stateExtractResult = item.RevealState(mws.Master);
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

    public virtual Range? AddCollectionElementSeparator(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemSeparatorFlag()) return null;
        if (elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) { return null; }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return null; }
        Gb.AppendSeparator(formatFlags.UseMainItemSeparator() ? Options.MainItemSeparator : Options.AlternateItemSeparator);
        return Gb.CurrentSectionRanges.CurrentSeparatorRange;
    }

    public virtual ContentSeparatorRanges AddCollectionElementPadding(IMoldWriteState mws, Type elementType, int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (formatFlags.HasNoItemPaddingFlag()) return Gb.Complete(formatFlags);
        if (elementType == typeof(char) && JsonOptions.CharBufferWritesAsCharCollection) { return Gb.Complete(formatFlags); }
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) { return Gb.Complete(formatFlags); }
        Gb.AppendPadding(formatFlags.UseMainItemPadding() ? Options.MainItemPadding : Options.AlternateItemPadding);
        return Gb.Complete(formatFlags);
    }

    public virtual ContentSeparatorRanges AddCollectionElementSeparatorAndPadding(IMoldWriteState mws, Type elementType
      , int nextItemNumber
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddCollectionElementSeparator(mws, elementType, nextItemNumber, formatFlags);
        return AddCollectionElementPadding(mws, elementType, nextItemNumber, formatFlags);
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
        Gb              = null!;
        PreviousContext = null;
        Options         = null!;
        base.StateReset();
    }

    public override string ToString() => $"{{ {GetType().Name}: {InstanceId}, {nameof(Gb)}: {Gb}, " +
                                         $"{nameof(FormatOptions)}: {FormatOptions?.ToString() ?? "null"} }}";
}
