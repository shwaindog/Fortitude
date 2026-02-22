using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public readonly record struct AppendSummary
(
    Type MoldBuildType
  , ITheOneString TheOneString
  , Range AppendRange
  , WrittenAsFlags WrittenAs
  , VisitId VisitNumber
  , Type? ContentType = null)
{
    public static implicit operator AppendSummary(TypeMolder mdc) => mdc.Complete();

    public int StartAt => AppendRange.Start.IsFromEnd
        ? TheOneString.WriteBuffer.Length - AppendRange.Start.Value
        : AppendRange.Start.Value;

    public int EndAt => AppendRange.End.IsFromEnd
        ? TheOneString.WriteBuffer.Length - AppendRange.End.Value
        : AppendRange.End.Value;

    public int Length => EndAt - StartAt;
}

public static class AppendSummaryExtensions
{
    public static AppendSummary EmptyAppendAt(this ITheOneString tos, Type typeBeingBuilt, int indexStart, Type? contentType) =>
        new(typeof(AppendSummary), tos
          , new Range(Index.FromStart(indexStart), Index.FromStart(indexStart))
          , Empty, VisitId.NoVisitRequiredId);

    public static AppendSummary UnregisteredAppend(this ITheOneString tos, Type typeBeingBuilt, int indexStart, int endAt
      , WrittenAsFlags writtenAs, Type? maybeContentType = null) =>
        new(typeBeingBuilt, tos
          , new Range(Index.FromStart(indexStart), Index.FromStart(endAt))
          , writtenAs, VisitId.NoVisitRequiredId, maybeContentType);

    public static AppendSummary AddWrittenAsFlags(this AppendSummary toAlter, WrittenAsFlags toAdd) =>
        toAlter with { WrittenAs = toAlter.WrittenAs | toAdd };

    public static AppendSummary UpdateStringEndRange(this AppendSummary toAlter, int deltaEnd) =>
        toAlter with { AppendRange = new Range(toAlter.AppendRange.Start, Index.FromStart(toAlter.AppendRange.End.Value + deltaEnd)) };
}

public abstract class TypeMolder : ExplicitRecyclableObject, IDisposable
{
    internal int OriginalStartIndex;

    protected void InitializeStyledTypeBuilder(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult visitResult
      , FormatFlags createFormatFlags)
    {
        PortableState.InstanceOrContainer = instanceOrContainer;
        PortableState.TypeBeingBuilt      = typeBeingBuilt;
        PortableState.Master              = master;
        PortableState.TypeVisitedAs       = typeVisitedAs;
        PortableState.TypeName            = typeName;
        PortableState.RemainingGraphDepth = remainingGraphDepth;
        PortableState.CompleteResult      = null;
        PortableState.MoldGraphVisit      = visitResult;
        PortableState.CreateFormatFlags   = createFormatFlags;

        OriginalStartIndex = master.WriteBuffer.Length;
    }

    protected MoldPortableState PortableState { get; set; } = new();

    public bool IsComplete => PortableState.CompleteResult != null;

    public int StartIndex =>
        PortableState.MoldGraphVisit.HasRegisteredVisit
     && PortableState.Master.ActiveGraphRegistry.RegistryId >= PortableState.MoldGraphVisit.VisitId.RegistryId
            ? PortableState.Master.GetLatestVisitBufferStartIndex(PortableState.MoldGraphVisit.VisitId)
            : OriginalStartIndex;

    public int RevisitedInstanceId => PortableState.MoldGraphVisit.InstanceId;
    public VisitResult MoldVisit => PortableState.MoldGraphVisit;

    public abstract void StartTypeOpening(FormatFlags formatFlags);
    public abstract void FinishTypeOpening(FormatFlags formatFlags);

    public bool BuildingInstanceEquals<T>(T check) => PortableState.InstanceOrContainer.Equals(check);

    public Type TypeBeingBuilt => PortableState.TypeBeingBuilt;
    public Type TypeVisitAs => PortableState.TypeVisitedAs;

    public StyleOptions Settings => PortableState.Master.Settings;

    public int InstanceTrackingVisitNumber => PortableState.MoldGraphVisit.VisitId.VisitIndex;

    public WrittenAsFlags WrittenAs
    {
        get => PortableState.WrittenAsFlags;
        protected set => PortableState.WrittenAsFlags = value;
    }

    public FormatFlags CreateFormatFlags => PortableState.CreateFormatFlags;

    public bool ShouldSuppressBody => RevisitedInstanceId > 0 || PortableState.RemainingGraphDepth <= 0;

    public abstract bool IsComplexType { get; }

    public abstract AppendSummary Complete();

    public void Dispose()
    {
        if (!IsComplete) { PortableState.CompleteResult = Complete(); }
    }

    protected override void InheritedStateReset()
    {
        PortableState.TypeBeingBuilt      = null!;
        PortableState.TypeVisitedAs       = null!;
        PortableState.Master              = null!;
        PortableState.TypeName            = null!;
        PortableState.CompleteResult      = null;
        PortableState.CreateFormatFlags   = DefaultCallerTypeFlags;
        PortableState.MoldGraphVisit      = VisitResult.VisitNotChecked;
        PortableState.WrittenAsFlags      = Empty;
        PortableState.RemainingGraphDepth = int.MaxValue;

        if (PortableState.InstanceOrContainer is IRecyclableStructContainer recyclableStructContainer)
        {
            recyclableStructContainer.DecrementRefCount();
        }
        else
        {
            PortableState.InstanceOrContainer = null!;
        }

        OriginalStartIndex = -1;

        MeRecyclable.StateReset();
    }

    protected virtual AppendSummary BuildMoldStringRange(Range typeWriteRange, Type? contentType = null) =>
        new AppendSummary(TypeBeingBuilt, PortableState.Master, typeWriteRange, PortableState.WrittenAsFlags
                        , PortableState.MoldGraphVisit.VisitId, contentType);

    public class MoldPortableState : ITransferState<MoldPortableState>
    {
        public object InstanceOrContainer { get; set; } = null!;
        public Type TypeBeingBuilt { get; set; } = null!;

        public Type TypeVisitedAs { get; set; } = null!;
        public string? TypeName { get; set; }

        public WrittenAsFlags WrittenAsFlags { get; set; }

        public VisitResult MoldGraphVisit { get; set; }

        public FormatFlags CreateFormatFlags { get; set; }

        public int RemainingGraphDepth { get; set; }

        public ISecretStringOfPower Master { get; set; } = null!;

        public AppendSummary? CompleteResult;

        public bool IsComplete => CompleteResult != null;

        public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
        {
            return CopyFrom((MoldPortableState)source, copyMergeFlags);
        }

        public MoldPortableState CopyFrom(MoldPortableState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            InstanceOrContainer = source.InstanceOrContainer;
            TypeBeingBuilt      = source.TypeBeingBuilt;
            TypeVisitedAs       = source.TypeVisitedAs;
            TypeName            = source.TypeName;
            WrittenAsFlags      = source.WrittenAsFlags;
            MoldGraphVisit      = source.MoldGraphVisit;
            CreateFormatFlags   = source.CreateFormatFlags;
            Master              = source.Master;
            CompleteResult      = source.CompleteResult;

            return this;
        }
    }
}

public interface ITypeBuilderComponentSource
{
    IMoldWriteState MoldState { get; }
}

public interface IMigratableTypeBuilderComponentSource
{
    IMigratableMoldWriteState MigratableMoldState { get; }
}

public static class StyledTypeBuilderExtensions
{
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> DynamicSpanFmtContentInvokers           = new();
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> DynamicSpanFmtCollectionElementInvokers = new();

    public static TExt AddGoToNext<TExt>(this IMoldWriteState<TExt> mdc, bool alwaysAddNextSeparator = false)
        where TExt : TypeMolder
    {
        if (mdc.Sf.Gb.HasCommitContent || alwaysAddNextSeparator) { mdc.StyleFormatter.AddToNextFieldSeparatorAndPadding(); }
        return mdc.Mold;
    }

    public static TExt AddGoToNext<TAny, TExt>(this TAny writtenAsFlags, IMoldWriteState<TExt> mdc)
        where TExt : TypeMolder
    {
        if (mdc.Sf.Gb.HasCommitContent) { mdc.StyleFormatter.AddToNextFieldSeparatorAndPadding(); }
        return mdc.Mold;
    }

    public static TExt AddGoToNext<TExt>(this AppendSummary appendSummary, IMoldWriteState<TExt> mdc)
        where TExt : TypeMolder
    {
        if (mdc.Sf.Gb.HasCommitContent || appendSummary.Length > 0) { mdc.StyleFormatter.AddToNextFieldSeparatorAndPadding(); }
        return mdc.Mold;
    }

    public static TExt ToTypeBuilder<TExt, T>(this T _, IMoldWriteState<TExt> typeBuilder)
        where TExt : TypeMolder =>
        typeBuilder.Mold;


    public static IStringBuilder ToStringBuilder<T>(this T _, IStringBuilder sb) => sb;


    public static IMoldWriteState<TExt> ToInternalTypeBuilder<TExt, T>(this T _, IMoldWriteState<TExt> typeBuilder)
        where TExt : TypeMolder =>
        typeBuilder;

    public static IMoldWriteState<TExt> AnyToCompAccess<TExt, T>(this T _, IMoldWriteState<TExt> typeBuilder)
        where TExt : TypeMolder =>
        typeBuilder;

    public static AppendSummary AppendNullableBooleanField<TExt>(this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName
      , bool? value
      , string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var actualType = value?.GetType() ?? typeof(bool?);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, startAt, actualType);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, startAt, actualType);

        mdc.FieldNameJoin(fieldName);
        if (value == null)
        {
            var writtenAsNull = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        WrittenAsFlags writtenAs;
        if (!callContext.HasFormatChange)
            writtenAs = mdc.AppendFormattedOrNull(value.Value, formatString, formatFlags);
        else
        {
            using (callContext) { writtenAs = mdc.AppendFormattedOrNull(value.Value, formatString, formatFlags); }
        }
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public static WrittenAsFlags AppendFormattedOrNull<TExt>(this IMoldWriteState<TExt> mdc, bool value, string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
        return formatFlags.HasIsFieldNameFlag()
            ? mdc.StyleFormatter.FormatFieldName(mdc, value, formatString, formatFlags)
            : mdc.StyleFormatter.FormatFieldContents(mdc, value, formatString, formatFlags);
    }

    public static AppendSummary AppendBooleanField<TExt>(this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, bool value
      , string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var actualType = typeof(bool);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (mdc.HasSkipField(typeof(bool), fieldName, formatFlags)) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, startAt, actualType);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, startAt, actualType);

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendFormatted(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendFormatted(value, formatString, formatFlags); }
    }

    public static AppendSummary AppendFormatted<TExt>(this IMoldWriteState<TExt> mdc, bool value, string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
        var actualType = typeof(bool);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;

        var writtenAs = formatFlags.HasIsFieldNameFlag()
            ? mdc.StyleFormatter.FormatFieldName(mdc, value, formatString, formatFlags)
            : mdc.StyleFormatter.FormatFieldContents(mdc, value, formatString, formatFlags);

        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public static AppendSummary AppendFormattableField<TExt, TFmt>
    (this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, formatFlags); }
    }

    public static AppendSummary AppendFormattedOrNull<TExt, TFmt>
    (this IMoldWriteState<TExt> mdc, TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmt : ISpanFormattable?
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);

        var actualType = value?.GetType() ?? typeof(TFmt);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var writtenAsNull = mdc.StyleFormatter.AppendFormattedNull(mdc.Sb, formatString, formatFlags);
                return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
            }
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, startAt, actualType);
        }
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeSpanFormattableClasses && !typeof(TFmt).IsValueType)
        {
            return mdc.AppendFormattedWithRefenceTracking(value, formatString, formatFlags);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static WrittenAsFlags AppendFormattedNoReferenceTracking<TFmt>
    (this IMoldWriteState mdc, TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        return formatFlags.HasIsFieldNameFlag()
            ? mdc.StyleFormatter.FormatFieldName(mdc, value, formatString, formatFlags)
            : mdc.StyleFormatter.FormatFieldContents(mdc, value, formatString, formatFlags);
    }

    private static AppendSummary AppendFormattedWithRefenceTracking<TFmt>
    (this IMoldWriteState mdc, TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (!formatFlags.HasNoRevisitCheck()
         && mdc.Settings.InstanceTrackingIncludeSpanFormattableClasses
         && value != null
         && !typeof(TFmt).IsValueType)
        {
            var preAppendLength      = mdc.Sb.Length;
            var registeredForRevisit = 
                mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, AsRaw | AsContent, formatFlags);
            var writtenAsTracking    = Empty;
            if (!registeredForRevisit.ShouldSuppressBody || mdc.Settings.InstanceMarkingIncludeSpanFormattableContents)
            {
                if (!formatFlags.HasIsFieldNameFlag())
                {
                    if (registeredForRevisit.ShouldSuppressBody) { mdc.StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags); }
                }
                writtenAsTracking = mdc.AppendFormattedNoReferenceTracking(value, formatString, formatFlags);
            }
            var graphBuilder = mdc.Sf.Gb;
            graphBuilder.Complete(formatFlags);
            var stateExtractResult = registeredForRevisit.Complete();
            graphBuilder.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
            graphBuilder.MarkContentStart(preAppendLength);
            graphBuilder.MarkContentEnd(mdc.Sb.Length);
            return stateExtractResult.AddWrittenAsFlags(writtenAsTracking);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public static AppendSummary DynamicReceiveAppendValue<TFmt>(IMoldWriteState mdc, TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
        var actualType = value.GetType();
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeSpanFormattableClasses && !typeof(TFmt).IsValueType)
        {
            return mdc.AppendFormattedWithRefenceTracking(value, formatString, formatFlags);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public static AppendSummary AppendFormattableField<TExt, TFmtStruct>
    (this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(TFmtStruct?);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        WrittenAsFlags writtenAs;
        if (!callContext.HasFormatChange)
            writtenAs = mdc.AppendNullableFormattedOrNull(value, formatString, formatFlags);
        else
        {
            using (callContext) { writtenAs = mdc.AppendNullableFormattedOrNull(value, formatString, formatFlags); }
        }
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    public static WrittenAsFlags AppendNullableFormattedOrNull<TExt, TFmtStruct>
    (this IMoldWriteState<TExt> mdc, TFmtStruct? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable
    {
        if (value == null) { return mdc.StyleFormatter.AppendFormattedNull(mdc.Sb, formatString, formatFlags); }
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);

        return formatFlags.HasIsFieldNameFlag()
            ? mdc.StyleFormatter.FormatFieldName(mdc, value, formatString, formatFlags)
            : mdc.StyleFormatter.FormatFieldContents(mdc, value, formatString, formatFlags);
    }

    public static AppendSummary RevealCloakedBearerField<TCloaked, TCloakedBase, TExt>(this IMoldWriteState<TExt> mdc
      , ReadOnlySpan<char> fieldName, TCloaked value, PalantírReveal<TCloakedBase> cloakedRevealer, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TExt : TypeMolder
        where TCloakedBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.RevealCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags);
        using (callContext) { return mdc.RevealCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags); }
    }

    public static AppendSummary RevealCloakedBearerOrNull<TCloaked, TRevealBase, TExt>(this IMoldWriteState<TExt> mdc
      , TCloaked? value, PalantírReveal<TRevealBase> styler, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TExt : TypeMolder
        where TRevealBase : notnull
    {
        var actualType   = value?.GetType() ?? typeof(TCloaked);
        var sb           = mdc.Sb;
        var contentStart = sb.Length;
        if (value == null)
        {
            var writtenAsFlags = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return new AppendSummary(actualType, mdc.Master, new Range(contentStart, sb.Length), writtenAsFlags, mdc.MoldGraphVisit.VisitId);
        }

        if (formatFlags.HasIsFieldNameFlag()) return mdc.StyleFormatter.FormatFieldName(mdc, value, styler, formatString, formatFlags);
        return mdc.StyleFormatter.FormatFieldContents(mdc, value, styler, formatString, formatFlags);
    }

    public static AppendSummary RevealNullableCloakedBearerField<TCloakedStruct, TExt>(this IMoldWriteState<TExt> mdc
      , ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> cloakedRevealer
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct where TExt : TypeMolder
    {
        var actualType = typeof(TCloakedStruct?);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.RevealNullableCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags);
        using (callContext) { return mdc.RevealNullableCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags); }
    }

    public static AppendSummary RevealNullableCloakedBearerOrNull<TCloakedStruct, TExt>(this IMoldWriteState<TExt> mdc
      , TCloakedStruct? value, PalantírReveal<TCloakedStruct> styler, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct where TExt : TypeMolder
    {
        var sb           = mdc.Sb;
        var contentStart = sb.Length;
        if (value == null)
        {
            var writtenAsFlags = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return new AppendSummary(typeof(TCloakedStruct), mdc.Master, new Range(contentStart, sb.Length), writtenAsFlags
                                   , mdc.MoldGraphVisit.VisitId);
        }
        if (formatFlags.HasIsFieldNameFlag()) return mdc.StyleFormatter.FormatFieldName(mdc, value.Value, styler, formatString, formatFlags);
        return mdc.StyleFormatter.FormatFieldContents(mdc, value.Value, styler, formatString, formatFlags);
    }

    public static AppendSummary RevealStringBearerField<TExt, TBearer>(this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName
      , TBearer value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.RevealStringBearerOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.RevealStringBearerOrNull(value, formatString, formatFlags); }
    }

    public static AppendSummary RevealStringBearerOrNull<TExt, TBearer>(this IMoldWriteState<TExt> mdc
      , TBearer? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
        where TBearer : IStringBearer?
    {
        var sb           = mdc.Sb;
        var contentStart = sb.Length;
        if (value == null)
        {
            var writtenAsFlags = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return new AppendSummary(typeof(TBearer), mdc.Master, new Range(contentStart, sb.Length), writtenAsFlags, mdc.MoldGraphVisit.VisitId);
        }
        if (formatFlags.HasIsFieldNameFlag()) return mdc.StyleFormatter.FormatBearerFieldName(mdc, value, formatString, formatFlags);
        return mdc.StyleFormatter.FormatBearerFieldContents(mdc, value, formatString, formatFlags);
    }

    public static AppendSummary RevealNullableStringBearerField<TExt, TBearerStruct>(this IMoldWriteState<TExt> mdc
      , ReadOnlySpan<char> fieldName, TBearerStruct? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct?);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.RevealNullableStringBearerOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.RevealNullableStringBearerOrNull(value, formatString, formatFlags); }
    }

    public static AppendSummary RevealNullableStringBearerOrNull<TExt, TBearerStruct>(this IMoldWriteState<TExt> mdc
      , TBearerStruct? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct?);
        var sb         = mdc.Sb;

        var contentStart = sb.Length;
        if (value == null)
        {
            var writtenAsFlags = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return new AppendSummary(typeof(TBearerStruct), mdc.Master, new Range(contentStart, sb.Length), writtenAsFlags, mdc.MoldGraphVisit.VisitId
                                   , actualType);
        }
        if (formatFlags.HasIsFieldNameFlag()) return mdc.StyleFormatter.FormatBearerFieldName(mdc, value.Value, formatString, formatFlags);

        return mdc.StyleFormatter.FormatBearerFieldContents(mdc, value.Value, formatString, formatFlags);
    }

    public static WrittenAsFlags AppendReadOnlySpanField<TExt>
    (this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(typeof(ReadOnlySpan<char>), fieldName, formatFlags))
        {
            return Empty;
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNullOnZeroLength(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNullOnZeroLength(value, formatString, fromIndex, length, formatFlags); }
    }

    public static WrittenAsFlags AppendFormattedOrNullOnZeroLength<TExt>
    (this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var sb         = mdc.Sb;
        var cappedFrom = Math.Clamp(fromIndex, 0, value.Length);
        if (value.Length == 0) { return mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags); }
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, "InputIsCharSpan", formatFlags, formatString);
        if (formatFlags.HasIsFieldNameFlag()) return mdc.StyleFormatter.FormatFieldName(mdc, value, cappedFrom, formatString, length, formatFlags);

        return mdc.StyleFormatter.FormatFieldContents(mdc, value, cappedFrom, formatString, length, formatFlags);
    }

    public static AppendSummary AppendStringField<TExt>
    (this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(typeof(string), fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, typeof(string));
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags); }
    }

    public static AppendSummary AppendFormattedOrNull<TExt>
    (this IMoldWriteState<TExt> mdc, string? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        var actualType = typeof(string);
        if (value == null)
        {
            var writtenAsNull = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, typeof(string));
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeStringInstances)
        {
            return mdc.AppendFormattedWithRefenceTracking(value, formatString, cappedFrom, length, formatFlags);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, cappedFrom, length, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static AppendSummary AppendFormattedWithRefenceTracking
    (this IMoldWriteState mdc, string value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(string);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeStringInstances)
        {
            var preAppendLength      = mdc.Sb.Length;
            var registeredForRevisit = mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, AsRaw | AsContent, formatFlags);
            var writtenAsTracking    = Empty;
            if (!registeredForRevisit.ShouldSuppressBody || mdc.Settings.InstanceMarkingIncludeStringContents)
            {
                if (!formatFlags.HasIsFieldNameFlag())
                {
                    if (registeredForRevisit.ShouldSuppressBody) { mdc.StyleFormatter.AppendInstanceValuesFieldName(actualType, formatFlags); }
                }
                writtenAsTracking = mdc.AppendFormattedNoReferenceTracking(value, formatString, fromIndex, length, formatFlags);
            }
            var graphBuilder = mdc.Sf.Gb;
            graphBuilder.Complete(formatFlags);
            var stateExtractResult = registeredForRevisit.Complete();
            graphBuilder.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
            graphBuilder.MarkContentStart(preAppendLength);
            graphBuilder.MarkContentEnd(mdc.Sb.Length);
            return stateExtractResult.AddWrittenAsFlags(writtenAsTracking);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, fromIndex, length, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static WrittenAsFlags AppendFormattedNoReferenceTracking
    (this IMoldWriteState mdc, string value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        return formatFlags.HasIsFieldNameFlag()
            ? mdc.StyleFormatter.FormatFieldName(mdc, value, cappedFrom, formatString, length, formatFlags)
            : mdc.StyleFormatter.FormatFieldContents(mdc, value, cappedFrom, formatString, length, formatFlags);
    }

    public static AppendSummary AppendCharArrayField<TExt>
    (this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var actualType = typeof(char[]);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags); }
    }

    public static AppendSummary AppendFormattedOrNull<TExt>
    (this IMoldWriteState<TExt> mdc, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        var actualType = typeof(char[]);
        if (value == null)
        {
            var writtenAsNull = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeCharArrayInstances)
        {
            return mdc.AppendFormattedWithRefenceTracking(value, formatString, cappedFrom, length, formatFlags);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, cappedFrom, length, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static AppendSummary AppendFormattedWithRefenceTracking
    (this IMoldWriteState mdc, char[] value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(char[]);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeCharArrayInstances)
        {
            var preAppendLength      = mdc.Sb.Length;
            var registeredForRevisit = mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, AsRaw | AsContent, formatFlags);
            var writtenAsTracking    = Empty;
            if (!registeredForRevisit.ShouldSuppressBody || mdc.Settings.InstanceMarkingIncludeCharArrayContents)
            {
                if (!formatFlags.HasIsFieldNameFlag())
                {
                    if (registeredForRevisit.ShouldSuppressBody) { mdc.StyleFormatter.AppendInstanceValuesFieldName(actualType, formatFlags); }
                }
                writtenAsTracking = mdc.AppendFormattedNoReferenceTracking(value, formatString, fromIndex, length, formatFlags);
            }
            var graphBuilder = mdc.Sf.Gb;
            graphBuilder.Complete(formatFlags);
            var stateExtractResult = registeredForRevisit.Complete();
            graphBuilder.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
            graphBuilder.MarkContentStart(preAppendLength);
            graphBuilder.MarkContentEnd(mdc.Sb.Length);
            return stateExtractResult.AddWrittenAsFlags(writtenAsTracking);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, fromIndex, length, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static WrittenAsFlags AppendFormattedNoReferenceTracking
    (this IMoldWriteState mdc, char[] value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        return formatFlags.HasIsFieldNameFlag()
            ? mdc.StyleFormatter.FormatFieldName(mdc, value, cappedFrom, formatString, length, formatFlags)
            : mdc.StyleFormatter.FormatFieldContents(mdc, value, cappedFrom, formatString, length, formatFlags);
    }

    public static AppendSummary AppendCharSequenceField<TExt, TCharSeq>
    (this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags); }
    }

    public static AppendSummary AppendFormattedOrNull<TExt, TCharSeq>
    (this IMoldWriteState<TExt> mdc, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
        where TCharSeq : ICharSequence?
    {
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        if (value == null)
        {
            var writtenAsNull = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeCharSequenceInstances)
        {
            #pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
            return mdc.AppendFormattedWithRefenceTracking(value, formatString, cappedFrom, length, formatFlags);
            #pragma warning restore CS8631
        }
        #pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, cappedFrom, length, formatFlags);
        #pragma warning restore CS8631
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static AppendSummary AppendFormattedWithRefenceTracking<TCharSeq>
    (this IMoldWriteState mdc, TCharSeq value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value.GetType();
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeCharSequenceInstances)
        {
            var preAppendLength      = mdc.Sb.Length;
            var registeredForRevisit = mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, AsRaw | AsContent, formatFlags);
            var writtenAsTracking    = Empty;
            if (!registeredForRevisit.ShouldSuppressBody || mdc.Settings.InstanceMarkingIncludeCharSequenceContents)
            {
                if (!formatFlags.HasIsFieldNameFlag())
                {
                    if (registeredForRevisit.ShouldSuppressBody) { mdc.StyleFormatter.AppendInstanceValuesFieldName(actualType, formatFlags); }
                }
                writtenAsTracking = mdc.AppendFormattedNoReferenceTracking(value, formatString, fromIndex, length, formatFlags);
            }
            var graphBuilder = mdc.Sf.Gb;
            graphBuilder.Complete(formatFlags);
            var stateExtractResult = registeredForRevisit.Complete();
            graphBuilder.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
            graphBuilder.MarkContentStart(preAppendLength);
            graphBuilder.MarkContentEnd(mdc.Sb.Length);
            return stateExtractResult.AddWrittenAsFlags(writtenAsTracking);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, fromIndex, length, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static WrittenAsFlags AppendFormattedNoReferenceTracking<TCharSeq>
    (this IMoldWriteState mdc, TCharSeq value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        return formatFlags.HasIsFieldNameFlag()
            ? mdc.StyleFormatter.FormatFieldName(mdc, value, cappedFrom, formatString, length, formatFlags)
            : mdc.StyleFormatter.FormatFieldContents(mdc, value, cappedFrom, formatString, length, formatFlags);
    }

    public static AppendSummary AppendStringBuilderField<TExt>
    (this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var actualType = typeof(StringBuilder);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);

        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags); }
    }

    public static AppendSummary AppendFormattedOrNull<TExt>
    (this IMoldWriteState<TExt> mdc, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        var actualType = typeof(StringBuilder);
        if (value == null)
        {
            var writtenAsNull = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAsNull, actualType);
        }
        var cappedFrom = Math.Clamp(fromIndex, 0, value.Length);
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeStringBuilderInstances)
        {
            return mdc.AppendFormattedWithRefenceTracking(value, formatString, cappedFrom, length, formatFlags);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, cappedFrom, length, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static AppendSummary AppendFormattedWithRefenceTracking
    (this IMoldWriteState mdc, StringBuilder value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(StringBuilder);
        var sb         = mdc.Sb;
        var startAt    = sb.Length;
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeStringBuilderInstances)
        {
            var preAppendLength      = mdc.Sb.Length;
            var registeredForRevisit = mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, AsRaw | AsContent, formatFlags);
            var writtenAsTracking    = Empty;
            if (!registeredForRevisit.ShouldSuppressBody || mdc.Settings.InstanceMarkingIncludeStringBuilderContents)
            {
                if (!formatFlags.HasIsFieldNameFlag())
                {
                    if (registeredForRevisit.ShouldSuppressBody) { mdc.StyleFormatter.AppendInstanceValuesFieldName(actualType, formatFlags); }
                }
                writtenAsTracking = mdc.AppendFormattedNoReferenceTracking(value, formatString, fromIndex, length, formatFlags);
            }
            var graphBuilder = mdc.Sf.Gb;
            graphBuilder.Complete(formatFlags);
            var stateExtractResult = registeredForRevisit.Complete();
            graphBuilder.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
            graphBuilder.MarkContentStart(preAppendLength);
            graphBuilder.MarkContentEnd(mdc.Sb.Length);
            return stateExtractResult.AddWrittenAsFlags(writtenAsTracking);
        }
        var writtenAs = mdc.AppendFormattedNoReferenceTracking(value, formatString, fromIndex, length, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, writtenAs, actualType);
    }

    private static WrittenAsFlags AppendFormattedNoReferenceTracking
    (this IMoldWriteState mdc, StringBuilder value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        return formatFlags.HasIsFieldNameFlag()
            ? mdc.StyleFormatter.FormatFieldName(mdc, value, cappedFrom, formatString, length, formatFlags)
            : mdc.StyleFormatter.FormatFieldContents(mdc, value, cappedFrom, formatString, length, formatFlags);
    }

    public static AppendSummary AppendObjectField<TExt>(this IMoldWriteState<TExt> mdc, ReadOnlySpan<char> fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var actualType = value?.GetType() ?? typeof(object);
        if (!mdc.Master.ContinueGivenFormattingFlags(formatFlags) || mdc.HasSkipField(actualType, fieldName, formatFlags))
        {
            return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        }
        mdc.FieldNameJoin(fieldName);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (!callContext.HasFormatChange) return mdc.AppendMatchFormattedOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendMatchFormattedOrNull(value, formatString, formatFlags); }
    }

    public static AppendSummary AppendMatchField<TExt, TAny>(this IMoldWriteState<TExt> mdc
      , ReadOnlySpan<char> fieldName, TAny value, string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType);

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendMatchFormattedOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendMatchFormattedOrNull(value, formatString, formatFlags); }
    }

    public static AppendSummary AppendMatchFormattedOrNull<TValue, TExt>(this IMoldWriteState<TExt> mdc
      , TValue value, string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var sb = mdc.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:           return mdc.AppendFormatted(valueBool, formatString, formatFlags);
                case byte valueByte:           return mdc.AppendFormattedOrNull(valueByte, formatString, formatFlags);
                case sbyte valueSByte:         return mdc.AppendFormattedOrNull(valueSByte, formatString, formatFlags);
                case char valueChar:           return mdc.AppendFormattedOrNull(valueChar, formatString, formatFlags);
                case short valueShort:         return mdc.AppendFormattedOrNull(valueShort, formatString, formatFlags);
                case ushort valueUShort:       return mdc.AppendFormattedOrNull(valueUShort, formatString, formatFlags);
                case Half valueHalfFloat:      return mdc.AppendFormattedOrNull(valueHalfFloat, formatString, formatFlags);
                case int valueInt:             return mdc.AppendFormattedOrNull(valueInt, formatString, formatFlags);
                case uint valueUInt:           return mdc.AppendFormattedOrNull(valueUInt, formatString, formatFlags);
                case nint valueUInt:           return mdc.AppendFormattedOrNull(valueUInt, formatString, formatFlags);
                case float valueFloat:         return mdc.AppendFormattedOrNull(valueFloat, formatString, formatFlags);
                case long valueLong:           return mdc.AppendFormattedOrNull(valueLong, formatString, formatFlags);
                case ulong valueULong:         return mdc.AppendFormattedOrNull(valueULong, formatString, formatFlags);
                case double valueDouble:       return mdc.AppendFormattedOrNull(valueDouble, formatString, formatFlags);
                case decimal valueDecimal:     return mdc.AppendFormattedOrNull(valueDecimal, formatString, formatFlags);
                case Int128 veryLongInt:       return mdc.AppendFormattedOrNull(veryLongInt, formatString, formatFlags);
                case UInt128 veryLongUInt:     return mdc.AppendFormattedOrNull(veryLongUInt, formatString, formatFlags);
                case BigInteger veryLongInt:   return mdc.AppendFormattedOrNull(veryLongInt, formatString, formatFlags);
                case Complex veryLongInt:      return mdc.AppendFormattedOrNull(veryLongInt, formatString, formatFlags);
                case DateTime valueDateTime:   return mdc.AppendFormattedOrNull(valueDateTime, formatString, formatFlags);
                case DateOnly valueDateOnly:   return mdc.AppendFormattedOrNull(valueDateOnly, formatString, formatFlags);
                case TimeSpan valueTimeSpan:   return mdc.AppendFormattedOrNull(valueTimeSpan, formatString, formatFlags);
                case TimeOnly valueTimeOnly:   return mdc.AppendFormattedOrNull(valueTimeOnly, formatString, formatFlags);
                case Rune valueRune:           return mdc.AppendFormattedOrNull(valueRune, formatString, formatFlags);
                case Guid valueGuid:           return mdc.AppendFormattedOrNull(valueGuid, formatString, formatFlags);
                case IPNetwork valueIpNetwork: return mdc.AppendFormattedOrNull(valueIpNetwork, formatString, formatFlags);
                case Version valueVersion:     return mdc.AppendFormattedOrNull(valueVersion, formatString, formatFlags);
                case IPAddress valueIpAddress: return mdc.AppendFormattedOrNull(valueIpAddress, formatString, formatFlags);
                case Uri valueUri:             return mdc.AppendFormattedOrNull(valueUri, formatString, formatFlags);
                case Enum:
                case ISpanFormattable:
                    var actualValueType = value.GetType();
                    var typeOfTValue    = typeof(TValue);
                    var delegateKey     = (typeOfTValue, actualValueType);
                    // ReSharper disable once InconsistentlySynchronizedField
                    if (!DynamicSpanFmtContentInvokers.TryGetValue(delegateKey, out var invoker))
                    {
                        lock (DynamicSpanFmtContentInvokers)
                        {
                            if (!DynamicSpanFmtContentInvokers.TryGetValue(delegateKey, out invoker))
                            {
                                if (typeOfTValue.ImplementsInterface(typeof(ISpanFormattable)))
                                {
                                    invoker = CreateSpanFormattableContentInvoker<TValue>();
                                }
                                else if (value is Enum) { invoker = CreateSpanFormattableContentInvoker<Enum>(); }
                                else { invoker                    = CreateSpanFormattableContentInvoker<ISpanFormattable>(); }
                                DynamicSpanFmtContentInvokers.TryAdd(delegateKey, invoker);
                            }
                        }
                    }
                    if (typeOfTValue.ImplementsInterface(typeof(ISpanFormattable)))
                    {
                        var castInvoker = (SpanFmtStructContentHandler<TValue>)invoker;
                        return castInvoker(mdc, value, formatString, formatFlags);
                    }
                    if (value is Enum valueEnum)
                    {
                        var castInvoker = (SpanFmtStructContentHandler<Enum>)invoker;
                        return castInvoker(mdc, valueEnum, formatString, formatFlags);
                    }
                    else
                    {
                        var castInvoker = (SpanFmtStructContentHandler<ISpanFormattable>)invoker;
                        return castInvoker(mdc, (ISpanFormattable)value, formatString, formatFlags);
                    }

                case char[] valueCharArray:           return mdc.AppendFormattedOrNull(valueCharArray, formatString, formatFlags: formatFlags);
                case string valueString:              return mdc.AppendFormattedOrNull(valueString, formatString, formatFlags: formatFlags);
                case ICharSequence valueCharSequence: return mdc.AppendFormattedOrNull(valueCharSequence, formatString, formatFlags: formatFlags);
                case StringBuilder valueSb:           return mdc.AppendFormattedOrNull(valueSb, formatString, formatFlags: formatFlags);

                case IStringBearer styledToStringObj: return mdc.RevealStringBearerOrNull(styledToStringObj, formatString, formatFlags);
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    mdc.Master.SetCallerFormatFlags(formatFlags);
                    mdc.Master.SetCallerFormatString(formatString);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = mdc.Master.StartKeyedCollectionType(value);
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString, formatFlags);
                        var mapCollectionStateExtractResult = keyedCollectionBuilder.Complete();
                        return mapCollectionStateExtractResult;
                    }
                    var orderedCollectionBuilder = mdc.Master.StartSimpleCollectionType(value);
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString, formatFlags);
                    var collectionStateExtractResult = orderedCollectionBuilder.Complete();
                    return collectionStateExtractResult;

                default:
                    var unknownType = value.GetType();
                    if (formatFlags.HasIsFieldNameFlag())
                    {
                        var startAt        = sb.Length;
                        var actualType     = value.GetType();
                        var fieldWrittenAs = mdc.StyleFormatter.FormatFieldNameMatch(mdc, value, formatString, formatFlags);
                        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, fieldWrittenAs, actualType);
                    }

                    // if (unknownType.IsValueType || mdc.Master.IsCallerSameAsLastRegisteredVisit(value))
                    if (unknownType.IsValueType)
                    {
                        var startAt           = sb.Length;
                        var actualType        = value.GetType();
                        var unknownFmtFlags   = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
                        var contentsWrittenAs = mdc.StyleFormatter.FormatFieldContentsMatch(mdc, value, formatString, unknownFmtFlags);
                        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, sb.Length, contentsWrittenAs, actualType);
                    }

                    var result = mdc.AppendObjectFormatted(value, formatString, formatFlags);
                    return result;
            }
        var nullStartAt   = sb.Length;
        var writtenAsNull = mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, nullStartAt, sb.Length, writtenAsNull, typeof(TValue));
    }

    public static AppendSummary AppendObjectFormatted<TExt>(this IMoldWriteState<TExt> mdc
      , object value, string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var actualType = value.GetType();
        if (mdc.CreateMoldFormatFlags.HasContentTreatmentFlags()) { return mdc.AppendRegisteredObjectFormatted(value, formatString, formatFlags); }
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) { return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, actualType); }
        var resolvedFlags = mdc.Sf.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
        if (!callContext.HasFormatChange) return mdc.AppendRegisteredObjectFormatted(value, formatString, resolvedFlags);
        AppendSummary result;
        using (callContext) { result = mdc.AppendRegisteredObjectFormatted(value, formatString, resolvedFlags); }
        if (callContext.HasFormatChange && result.VisitNumber.VisitIndex >= 0)
        {
            mdc.Master.UpdateVisitEncoders(result.VisitNumber, mdc.Sf.ContentEncoder, mdc.Sf.LayoutEncoder);
        }
        return result;
    }

    private static AppendSummary AppendRegisteredObjectFormatted<TExt>(this IMoldWriteState<TExt> mdc
      , object value, string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var actualType           = value.GetType();
        var preAppendLength      = mdc.Sb.Length;
        var registeredForRevisit = mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, AsRaw | AsObject, formatFlags);
        var writtenAsTracking    = Empty;
        if (!registeredForRevisit.ShouldSuppressBody || mdc.Settings.InstanceMarkingIncludeObjectToStringContents)
        {
            if (!formatFlags.HasIsFieldNameFlag())
            {
                if (registeredForRevisit.ShouldSuppressBody) { mdc.StyleFormatter.AppendInstanceValuesFieldName(actualType, formatFlags); }
            }
            writtenAsTracking = mdc.Sf.FormatFieldContentsMatch(mdc, value, formatString, formatFlags);
        }
        var graphBuilder = mdc.Sf.Gb;
        graphBuilder.Complete(formatFlags);
        var stateExtractResult = registeredForRevisit.Complete();
        graphBuilder.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
        graphBuilder.MarkContentStart(preAppendLength);
        graphBuilder.MarkContentEnd(mdc.Sb.Length);
        return stateExtractResult.AddWrittenAsFlags(writtenAsTracking);
    }

    public static TExt AppendEmptyCollectionOrNull<TExt>(this IMoldWriteState<TExt> mdc, Type elementType, Type collectionType
      , int? matchedItemsCount = null, int? totalCheckedItemsCount = null, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool nullBecomesEmpty = true)
        where TExt : TypeMolder
    {
        var previousWroteName = mdc.WroteOuterTypeName;
        if (totalCheckedItemsCount != null)
        {
            if (collectionType == mdc.TypeBeingBuilt)
            {
                mdc.InnerSameAsOuterType = true;

                formatFlags |= LogSuppressTypeNames | NoRevisitCheck;
            }
            mdc.WroteOuterTypeName = false;
            mdc.Sf.StartSimpleTypeOpening(collectionType, mdc, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
            mdc.Sf.FinishSimpleTypeOpening(collectionType, mdc, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
            mdc.WroteOuterTypeName = previousWroteName;
        }
        previousWroteName      = mdc.WroteInnerTypeName;
        mdc.WroteInnerTypeName = false;
        int? whenNull = nullBecomesEmpty ? 0 : null;
        mdc.StyleFormatter.AppendOpenCollection(mdc, elementType, totalCheckedItemsCount != null ? false : null, formatFlags);
        mdc.StyleFormatter.AppendCloseCollection(mdc, matchedItemsCount, elementType, totalCheckedItemsCount ?? whenNull, formatString, formatFlags);
        mdc.WroteInnerTypeName = previousWroteName;
        return mdc.Mold;
    }

    public static TExt AppendEmptyCollectionOrNullAndGoToNext<TExt>(this IMoldWriteState<TExt> mdc, Type elementType, Type collectionType
      , int? matchedItemsCount = null, int? totalCheckedItemsCount = null, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool nullBecomesEmpty = true)
        where TExt : TypeMolder
    {
        mdc.AppendEmptyCollectionOrNull(elementType, collectionType, matchedItemsCount, totalCheckedItemsCount
                                      , formatString, formatFlags, nullBecomesEmpty);
        return mdc.AddGoToNext();
    }

    public static AppendSummary AppendFormattedCollectionItemMatchOrNull<TValue, TExt>(this IMoldWriteState<TExt> mdc
      , TValue value, int retrieveCount, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var sb = mdc.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool: return mdc.AppendFormattedCollectionItem(valueBool, retrieveCount, formatString, formatFlags);
                case byte valueByte: return mdc.AppendFormattedCollectionItem(valueByte, retrieveCount, formatString, formatFlags);
                case sbyte valueSByte: return mdc.AppendFormattedCollectionItem(valueSByte, retrieveCount, formatString, formatFlags);
                case char valueChar: return mdc.AppendFormattedCollectionItem(valueChar, retrieveCount, formatString, formatFlags);
                case short valueShort: return mdc.AppendFormattedCollectionItem(valueShort, retrieveCount, formatString, formatFlags);
                case ushort valueUShort: return mdc.AppendFormattedCollectionItem(valueUShort, retrieveCount, formatString, formatFlags);
                case Half valueHalfFloat: return mdc.AppendFormattedCollectionItem(valueHalfFloat, retrieveCount, formatString, formatFlags);
                case int valueInt: return mdc.AppendFormattedCollectionItem(valueInt, retrieveCount, formatString, formatFlags);
                case uint valueUInt: return mdc.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString, formatFlags);
                case nint valueUInt: return mdc.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString, formatFlags);
                case float valueFloat: return mdc.AppendFormattedCollectionItem(valueFloat, retrieveCount, formatString, formatFlags);
                case long valueLong: return mdc.AppendFormattedCollectionItem(valueLong, retrieveCount, formatString, formatFlags);
                case ulong valueULong: return mdc.AppendFormattedCollectionItem(valueULong, retrieveCount, formatString, formatFlags);
                case double valueDouble: return mdc.AppendFormattedCollectionItem(valueDouble, retrieveCount, formatString, formatFlags);
                case decimal valueDecimal: return mdc.AppendFormattedCollectionItem(valueDecimal, retrieveCount, formatString, formatFlags);
                case Int128 veryLongInt: return mdc.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags);
                case UInt128 veryLongUInt: return mdc.AppendFormattedCollectionItem(veryLongUInt, retrieveCount, formatString, formatFlags);
                case BigInteger veryLongInt: return mdc.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags);
                case Complex veryLongInt: return mdc.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags);
                case DateTime valueDateTime: return mdc.AppendFormattedCollectionItem(valueDateTime, retrieveCount, formatString, formatFlags);
                case DateOnly valueDateOnly: return mdc.AppendFormattedCollectionItem(valueDateOnly, retrieveCount, formatString, formatFlags);
                case TimeSpan valueTimeSpan: return mdc.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString, formatFlags);
                case TimeOnly valueTimeOnly: return mdc.AppendFormattedCollectionItem(valueTimeOnly, retrieveCount, formatString, formatFlags);
                case Rune valueRune: return mdc.AppendFormattedCollectionItem(valueRune, retrieveCount, formatString, formatFlags);
                case Guid valueGuid: return mdc.AppendFormattedCollectionItem(valueGuid, retrieveCount, formatString, formatFlags);
                case IPNetwork valueIpNetwork: return mdc.AppendFormattedCollectionItem(valueIpNetwork, retrieveCount, formatString, formatFlags);
                case char[] valueCharArray: return mdc.AppendFormattedCollectionItemOrNull(valueCharArray, retrieveCount, formatString, formatFlags);
                case string valueString: return mdc.AppendFormattedCollectionItemOrNull(valueString, retrieveCount, formatString, formatFlags);
                case Version valueVersion: return mdc.AppendFormattedCollectionItem(valueVersion, retrieveCount, formatString, formatFlags);
                case IPAddress valueIpAddress: return mdc.AppendFormattedCollectionItem(valueIpAddress, retrieveCount, formatString, formatFlags);
                case Uri valueUri: return mdc.AppendFormattedCollectionItem(valueUri, retrieveCount, formatString, formatFlags);
                case Enum:
                case ISpanFormattable:
                    var actualValueType = value.GetType();
                    var typeOfTValue    = typeof(TValue);
                    var delegateKey     = (typeOfTValue, actualValueType);
                    // ReSharper disable once InconsistentlySynchronizedField
                    if (!DynamicSpanFmtCollectionElementInvokers.TryGetValue(delegateKey, out var invoker))
                    {
                        lock (DynamicSpanFmtCollectionElementInvokers)
                        {
                            if (!DynamicSpanFmtCollectionElementInvokers.TryGetValue(delegateKey, out invoker))
                            {
                                if (typeOfTValue.ImplementsInterface(typeof(ISpanFormattable)))
                                {
                                    invoker = CreateSpanFormattableCollectionElementInvoker<TValue>();
                                }
                                else if (value is Enum) { invoker = CreateSpanFormattableCollectionElementInvoker<Enum>(); }
                                else { invoker                    = CreateSpanFormattableCollectionElementInvoker<ISpanFormattable>(); }
                                DynamicSpanFmtCollectionElementInvokers.TryAdd(delegateKey, invoker);
                            }
                        }
                    }
                    if (typeOfTValue.ImplementsInterface(typeof(ISpanFormattable)))
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<TValue>)invoker;
                        return castInvoker(mdc, value, retrieveCount, formatString, formatFlags);
                    }
                    if (value is Enum valueEnum)
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<Enum>)invoker;
                        return castInvoker(mdc, valueEnum, retrieveCount, formatString, formatFlags);
                    }
                    else
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<ISpanFormattable>)invoker;
                        return castInvoker(mdc, (ISpanFormattable)value, retrieveCount, formatString, formatFlags);
                    }

                case ICharSequence valueCharSequence:
                    return mdc.AppendFormattedCollectionItemOrNull(valueCharSequence, retrieveCount, formatString, formatFlags);
                case StringBuilder valueSb: return mdc.AppendFormattedCollectionItemOrNull(valueSb, retrieveCount, formatString, formatFlags);

                case IStringBearer styledToStringObj: return mdc.RevealStringBearerOrNull(styledToStringObj, formatString, formatFlags);
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    mdc.Master.SetCallerFormatFlags(formatFlags);
                    mdc.Master.SetCallerFormatString(formatString);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = mdc.Master.StartKeyedCollectionType(value);
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString, formatFlags);
                        var mapCollectionStateExtractResult = keyedCollectionBuilder.Complete();
                        return mapCollectionStateExtractResult;
                    }
                    var orderedCollectionBuilder = mdc.Master.StartSimpleCollectionType(value);
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString, formatFlags);
                    var collectionStateExtractResult = orderedCollectionBuilder.Complete();
                    return collectionStateExtractResult;

                default:
                    var unKnownType = typeof(TValue);
                    if (unKnownType.IsValueType || (!unKnownType.IsAnyTypeHoldingChars()))
                    {
                        formatFlags = mdc.StyleFormatter.ResolveContentFormatFlags(mdc.Sb, value, formatFlags, formatString);
                        bool isSpanFormattableType = unKnownType.IsSpanFormattableCached();
                        if (!formatFlags.HasNoRevisitCheck()
                         && !typeof(TValue).IsValueType
                         && (!isSpanFormattableType
                          || mdc.Settings.InstanceTrackingIncludeSpanFormattableClasses))
                        {
                            var preAppendLength      = mdc.Sb.Length;
                            var registeredForRevisit = mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, AsRaw | AsContent, formatFlags);
                            if (!registeredForRevisit.ShouldSuppressBody
                             || (isSpanFormattableType && mdc.Settings.InstanceMarkingIncludeSpanFormattableContents))
                            {
                                if (registeredForRevisit.ShouldSuppressBody)
                                    mdc.StyleFormatter.AppendInstanceValuesFieldName(typeof(TValue), formatFlags);
                                mdc.StyleFormatter.CollectionNextItem(value, retrieveCount, mdc.Sb, (FormatSwitches)formatFlags);
                            }
                            var gb = mdc.Sf.Gb;
                            gb.Complete(formatFlags);
                            var stateExtractResult = registeredForRevisit.Complete();
                            gb.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
                            gb.MarkContentStart(preAppendLength);
                            gb.MarkContentEnd(mdc.Sb.Length);
                            return stateExtractResult;
                        }
                        var contentStart = sb.Length;
                        mdc.StyleFormatter.CollectionNextItem(value, retrieveCount, mdc.Sb, (FormatSwitches)formatFlags);
                        var itemWrittenAs = sb.WrittenAsFromFirstCharacters(contentStart, mdc.Sf.Gb);
                        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, contentStart, sb.Length, itemWrittenAs, typeof(TValue));
                    }
                    break;
            }
        else
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                var nullStartAt   = sb.Length;
                var writtenAsNull = mdc.StyleFormatter.AppendFormattedNull(mdc.Sb, formatString, formatFlags);
                return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, nullStartAt, sb.Length, writtenAsNull, typeof(TValue));
            }
        }
        return mdc.Master.EmptyAppendAt(mdc.TypeBeingBuilt, mdc.Sb.Length, typeof(TValue));
    }


    public static int FieldNameJoin(this IMoldWriteState mdc, ReadOnlySpan<char> fieldName)
    {
        var preAppendLen = mdc.Sb.Length;
        var sf           = mdc.StyleFormatter;
        // if (sf.AddedContextOnThisCall)
        // {
        //     sf = sf.PreviousContextOrThis;
        // }
        sf.AppendFieldName(mdc, fieldName);
        sf.AppendFieldValueSeparator();
        mdc.IsEmpty = false;
        return mdc.Sb.Length - preAppendLen;
    }


    public static int FieldNameWithDefaultJoin(this IMoldWriteState mdc, ReadOnlySpan<char> fieldName, string onEmptyUseFieldName = "$values")
    {
        var preAppendLen = mdc.Sb.Length;
        var sf           = mdc.StyleFormatter;
        // if (sf.AddedContextOnThisCall)
        // {
        //     sf = sf.PreviousContextOrThis;
        // }
        sf.AppendFieldName(mdc, fieldName);
        sf.AppendFieldValueSeparator();
        mdc.IsEmpty = false;
        return mdc.Sb.Length - preAppendLen;
    }

    public static int FieldEnd<TExt>(this IMoldWriteState<TExt> mdc) where TExt : TypeMolder
    {
        var preAppendLen = mdc.Sb.Length;
        var sf           = mdc.StyleFormatter;
        if (sf.AddedContextOnThisCall) { sf = sf.PreviousContextOrThis; }
        sf.AppendFieldValueSeparator();
        mdc.IsEmpty = false;
        return mdc.Sb.Length - preAppendLen;
    }

    public static void GoToNextCollectionItemStart<TExt>(this IMoldWriteState<TExt> mdc, Type elementType, int elementAt) where TExt : TypeMolder
    {
        mdc.StyleFormatter.AddCollectionElementSeparatorAndPadding(mdc, elementType, elementAt + 1);
    }

    public static AppendSummary AppendFormattedCollectionItem<TExt>
    (this IMoldWriteState<TExt> mdc, bool value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var startAt    = mdc.Sb.Length;
        var actualType = typeof(bool);
        var writtenAs  = mdc.StyleFormatter.CollectionNextItemFormat(mdc, value, retrieveCount, formatString, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, mdc.Sb.Length, writtenAs, actualType);
    }

    public static AppendSummary AppendFormattedCollectionItem<TExt>
    (this IMoldWriteState<TExt> mdc, bool? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var startAt    = mdc.Sb.Length;
        var actualType = typeof(bool?);
        var writtenAs  = mdc.StyleFormatter.CollectionNextItemFormat(mdc, value, retrieveCount, formatString, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, mdc.Sb.Length, writtenAs, actualType);
    }

    public static AppendSummary AppendFormattedCollectionItem<TExt, TFmt>
    (this IMoldWriteState<TExt> mdc, TFmt value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder where TFmt : ISpanFormattable?
    {
        return mdc.StyleFormatter.CollectionNextItemFormat(mdc, value, retrieveCount, formatString, formatFlags);
    }

    public static AppendSummary AppendFormattedCollectionItem<TExt, TFmtStruct>
    (this IMoldWriteState<TExt> mdc, TFmtStruct? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable
    {
        var startAt    = mdc.Sb.Length;
        var actualType = typeof(TFmtStruct?);
        var writtenAs  = mdc.StyleFormatter.CollectionNextItemFormat(mdc, value, retrieveCount, formatString, formatFlags);
        return mdc.Master.UnregisteredAppend(mdc.TypeBeingBuilt, startAt, mdc.Sb.Length, writtenAs, actualType);
    }

    public static AppendSummary DynamicReceiveAppendFormattedCollectionItem<TFmt>(IMoldWriteState mdc, TFmt? value
      , int retrieveCount, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc, value, retrieveCount, formatString, formatFlags);

    public static AppendSummary AppendFormattedCollectionItemOrNull<TExt>
    (this IMoldWriteState<TExt> mdc, string? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc, value, retrieveCount, formatString, formatFlags);

    public static AppendSummary AppendFormattedCollectionItemOrNull<TExt>
    (this IMoldWriteState<TExt> mdc, char[]? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc, value, retrieveCount, formatString, formatFlags);

    public static AppendSummary AppendFormattedCollectionItemOrNull<TExt>
    (this IMoldWriteState<TExt> mdc, ICharSequence? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextCharSeqFormat(mdc, value, retrieveCount, formatString, formatFlags);

    public static AppendSummary AppendFormattedCollectionItemOrNull<TExt>
    (this IMoldWriteState<TExt> mdc, StringBuilder? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc, value, retrieveCount, formatString, formatFlags);

    private delegate AppendSummary SpanFmtStructContentHandler<in TFmt>(IMoldWriteState mdc, TFmt fmt
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags);

    // Invokes DynamicReceiveAppendValue without boxing to ISpanFormattable if the type receive already supports ISpanFormattable
    private static SpanFmtStructContentHandler<T> CreateSpanFormattableContentInvoker<T>()
    {
        var genTypeDefMeth = typeof(StyledTypeBuilderExtensions)
                             .GetMethods().First(mi => mi.Name.Contains(nameof(DynamicReceiveAppendValue)));

        var generified = genTypeDefMeth.MakeGenericMethod(typeof(T));

        return BuildContentInvoker<T>(generified);
    }

    // Invokes DynamicReceiveAppendValue without boxing to ISpanFormattable if the type receive already supports ISpanFormattable
    private static SpanFmtStructContentHandler<T> BuildContentInvoker<T>(MethodInfo methodInfo)
    {
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicStructAppend", typeof(AppendSummary),
                 [typeof(IMoldWriteState), typeof(T), typeof(string), typeof(FormatFlags)]
               , typeof(StyledTypeBuilderExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(SpanFmtStructContentHandler<T>));
        return (SpanFmtStructContentHandler<T>)methodInvoker;
    }

    private delegate AppendSummary SpanFmtStructCollectionElementHandler<in TFmt>(IMoldWriteState mdc
      , TFmt fmt, int retrievalCount, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags);

    // Invokes DynamicReceiveAppendFormattedCollectionItem without boxing to ISpanFormattable if the type receive already supports ISpanFormattable
    private static SpanFmtStructCollectionElementHandler<T> CreateSpanFormattableCollectionElementInvoker<T>()
    {
        var genTypeDefMeth = typeof(StyledTypeBuilderExtensions)
                             .GetMethods().First(mi => mi.Name.Contains(nameof(DynamicReceiveAppendFormattedCollectionItem)));

        var generified = genTypeDefMeth.MakeGenericMethod(typeof(T));

        return BuildCollectionInvoker<T>(generified);
    }

    // Invokes DynamicReceiveAppendFormattedCollectionItem without boxing to ISpanFormattable if the type receive already supports ISpanFormattable
    private static SpanFmtStructCollectionElementHandler<TFmt> BuildCollectionInvoker<TFmt>(MethodInfo methodInfo)
    {
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicStructAppend", typeof(AppendSummary),
                 [typeof(IMoldWriteState), typeof(TFmt), typeof(int), typeof(string), typeof(FormatFlags)]
               , typeof(StyledTypeBuilderExtensions).Module, true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(SpanFmtStructCollectionElementHandler<TFmt>));
        return (SpanFmtStructCollectionElementHandler<TFmt>)methodInvoker;
    }
}
