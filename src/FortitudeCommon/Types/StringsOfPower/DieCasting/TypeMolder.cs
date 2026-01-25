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
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public record struct StateExtractStringRange(string TypeName, ITheOneString TypeTheOneString, Range AppendRange)
{
    public static implicit operator StateExtractStringRange(TypeMolder mdc) => mdc.Complete();


    public static readonly StateExtractStringRange EmptyAppend =
        new("Empty", null!, new Range(Index.FromStart(0), Index.FromStart(0)));
}

public abstract class TypeMolder : ExplicitRecyclableObject, IDisposable
{
    protected int StartIndex;

    protected void InitializeStyledTypeBuilder(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , string? typeName
      , int remainingGraphDepth
      , VisitResult visitResult
      , FormatFlags createFormatFlags)
    {
        PortableState.InstanceOrContainer = instanceOrContainer;
        PortableState.TypeBeingBuilt      = typeBeingBuilt;
        PortableState.Master              = master;
        PortableState.TypeName            = typeName;
        PortableState.RemainingGraphDepth = remainingGraphDepth;
        PortableState.CompleteResult      = null;
        PortableState.MoldGraphVisit      = visitResult;
        PortableState.CreateFormatFlags   = createFormatFlags;

        StartIndex = master.WriteBuffer.Length;
    }

    protected MoldPortableState PortableState { get; set; } = new();

    public bool IsComplete => PortableState.CompleteResult != null;

    public int RevisitedInstanceId => PortableState.MoldGraphVisit.InstanceId;
    public VisitResult MoldVisit => PortableState.MoldGraphVisit;

    public abstract void StartTypeOpening();
    public abstract void FinishTypeOpening();

    public bool BuildingInstanceEquals<T>(T check) => PortableState.InstanceOrContainer.Equals(check);

    public Type TypeBeingBuilt => PortableState.TypeBeingBuilt;

    public StyleOptions Settings => PortableState.Master.Settings;

    public string? TypeName => PortableState.TypeName;

    public FormatFlags CreateFormatFlags => PortableState.CreateFormatFlags;

    public bool ShouldSuppressBody => RevisitedInstanceId > 0 || PortableState.RemainingGraphDepth <= 0;

    public abstract bool IsComplexType { get; }

    public abstract StateExtractStringRange Complete();

    public void Dispose()
    {
        if (!IsComplete) { PortableState.CompleteResult = Complete(); }
    }

    protected override void InheritedStateReset()
    {
        PortableState.Master            = null!;
        PortableState.TypeName          = null!;
        PortableState.CompleteResult    = null;
        PortableState.CreateFormatFlags = DefaultCallerTypeFlags;
        PortableState.MoldGraphVisit    = VisitResult.Empty;
        if (PortableState.InstanceOrContainer is IRecyclableStructContainer recyclableStructContainer)
        {
            recyclableStructContainer.DecrementRefCount();
        }

        StartIndex = -1;

        MeRecyclable.StateReset();
    }

    public class MoldPortableState
    {
        public object InstanceOrContainer { get; set; } = null!;
        public Type TypeBeingBuilt { get; set; } = null!;
        public string? TypeName { get; set; }

        public VisitResult MoldGraphVisit { get; set; } = null!;

        public FormatFlags CreateFormatFlags { get; set; }

        public int RemainingGraphDepth { get; set; }

        public ISecretStringOfPower Master { get; set; } = null!;

        public StateExtractStringRange? CompleteResult;

        public bool IsComplete => CompleteResult != null;
    }
}

public interface ITypeBuilderComponentSource
{
    ITypeMolderDieCast MoldState { get; }
}

public interface IMigratableTypeBuilderComponentSource
{
    IMigratableTypeMolderDieCast MigratableMoldState { get; }
}

public interface ITypeBuilderComponentSource<out T> : ITypeBuilderComponentSource where T : TypeMolder
{
    ITypeMolderDieCast<T> KnownTypeMoldState { get; }
}

public static class StyledTypeBuilderExtensions
{
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> DynamicSpanFmtContentInvokers           = new();
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> DynamicSpanFmtCollectionElementInvokers = new();
    
    private static ReadOnlySpan<char> EmptyRs => ReadOnlySpan<char>.Empty;

    public static TExt AddGoToNext<TExt>(this ITypeMolderDieCast<TExt> mdc)
        where TExt : TypeMolder
    {
        if (mdc.Sf.Gb.HasCommitContent) { mdc.StyleFormatter.AddToNextFieldSeparatorAndPadding(); }
        return mdc.StyleTypeBuilder;
    }

    public static TExt ToTypeBuilder<TExt, T>(this T _, ITypeMolderDieCast<TExt> typeBuilder)
        where TExt : TypeMolder =>
        typeBuilder.StyleTypeBuilder;


    public static IStringBuilder ToStringBuilder<T>(this T _, IStringBuilder sb) => sb;


    public static ITypeMolderDieCast<TExt> ToInternalTypeBuilder<TExt, T>(this T _, ITypeMolderDieCast<TExt> typeBuilder)
        where TExt : TypeMolder =>
        typeBuilder;

    public static ITypeMolderDieCast<TExt> AnyToCompAccess<TExt, T>(this T _, ITypeMolderDieCast<TExt> typeBuilder)
        where TExt : TypeMolder =>
        typeBuilder;

    public static ITypeMolderDieCast<TExt> AppendNullableBooleanField<TExt>(this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName
      , bool? value
      , string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var actualType = value?.GetType() ?? typeof(bool?);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (value == null)
        {
            var sb = mdc.Sb;
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc;
        }
        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>(this ITypeMolderDieCast<TExt> mdc, bool? value, string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, formatString, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, formatString, formatFlags);
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> AppendBooleanField<TExt>(this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, bool value
      , string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        if (mdc.HasSkipField(typeof(bool), fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendFormatted(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendFormatted(value, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormatted<TExt>(this ITypeMolderDieCast<TExt> mdc, bool value, string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, formatString, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, formatString, formatFlags);
        return mdc;
    }
    
    public static ITypeMolderDieCast<TExt> AppendFormattableField<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;
    
        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> mdc, TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmt : ISpanFormattable?
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
        
        if (value == null)
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                mdc.StyleFormatter.AppendFormattedNull(mdc.Sb, formatString, formatFlags);
            }
            return mdc;
        }
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeSpanFormattableClasses && !typeof(TFmt).IsValueType)
        {
            mdc.AppendFormattedWithRefenceTracking(value, formatString, formatFlags);
            return mdc;
        }
        mdc.AppendFormattedNoReferenceTracking(value, formatString, formatFlags);
        return mdc;
    }

    public static int AppendFormattedNoReferenceTracking<TFmt>
    (this ITypeMolderDieCast mdc, TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var preAppendLength = mdc.Sb.Length;
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, formatString, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, formatString, formatFlags);
        return mdc.Sb.Length - preAppendLength;
    }

    public static int AppendFormattedWithRefenceTracking<TFmt>
    (this ITypeMolderDieCast mdc, TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeSpanFormattableClasses && value != null && !typeof(TFmt).IsValueType)
        {
            var preAppendLength      = mdc.Sb.Length;
            var registeredForRevisit = mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, formatFlags);
            if (!registeredForRevisit.ShouldSuppressBody || mdc.Settings.InstanceMarkingIncludeSpanFormattableContents)
            {
                if (!formatFlags.HasIsFieldNameFlag())
                {
                    if (registeredForRevisit.ShouldSuppressBody)
                    {
                        mdc.StyleFormatter.AppendInstanceValuesFieldName(typeof(TFmt), formatFlags);
                    }
                }
                mdc.AppendFormattedNoReferenceTracking(value, formatString, formatFlags);
            }
            var graphBuilder = mdc.Sf.Gb;
            graphBuilder.Complete(formatFlags);
            registeredForRevisit.Complete();
            graphBuilder.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
            graphBuilder.MarkContentStart(preAppendLength);
            graphBuilder.MarkContentEnd(mdc.Sb.Length);
            return mdc.Sb.Length - preAppendLength;
        }
        return mdc.AppendFormattedNoReferenceTracking(value, formatString, formatFlags);
    }

    public static int DynamicReceiveAppendValue<TFmt>(ITypeMolderDieCast mdc, TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
        if (!formatFlags.HasNoRevisitCheck() && mdc.Settings.InstanceTrackingIncludeSpanFormattableClasses && !typeof(TFmt).IsValueType)
        {
            return mdc.AppendFormattedWithRefenceTracking(value, formatString, formatFlags);
        }
        return mdc.AppendFormattedNoReferenceTracking(value, formatString, formatFlags);
    }

    public static ITypeMolderDieCast<TExt> AppendFormattableField<TExt, TFmtStruct>
    (this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmtStruct?);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendNullableFormattedOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendNullableFormattedOrNull(value, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendNullableFormattedOrNull<TExt, TFmtStruct>
    (this ITypeMolderDieCast<TExt> mdc, TFmtStruct? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable
    {
        if (value == null)
        {
            var sb = mdc.Sb;
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc;
        }
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, formatString, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, formatString, formatFlags);
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> RevealCloakedBearerField<TCloaked, TCloakedBase, TExt>(this ITypeMolderDieCast<TExt> mdc
      , ReadOnlySpan<char> fieldName, TCloaked value, PalantírReveal<TCloakedBase> cloakedRevealer, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase?
        where TExt : TypeMolder
        where TCloakedBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.RevealCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags);
        using (callContext) { return mdc.RevealCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> RevealCloakedBearerOrNull<TCloaked, TRevealBase, TExt>(this ITypeMolderDieCast<TExt> mdc
      , TCloaked? value, PalantírReveal<TRevealBase> styler, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TExt : TypeMolder
        where TRevealBase : notnull
    {
        var sb = mdc.Sb;
        if (value != null)
        {
            if (formatFlags.HasIsFieldNameFlag())
                mdc.StyleFormatter.FormatFieldName(mdc.Master, value, styler, formatString, formatFlags);
            else
                mdc.StyleFormatter.FormatFieldContents(mdc.Master, value, styler, formatString, formatFlags);

            // if (!mdc.Settings.DisableCircularRefCheck && !typeof(TCloaked).IsValueType)
            // {
            //     mdc.Master.EnsureRegisteredVisited(value, formatFlags);
            // }
        }
        else { mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags); }
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> RevealNullableCloakedBearerField<TCloakedStruct, TExt>(this ITypeMolderDieCast<TExt> mdc
      , ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> cloakedRevealer
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct where TExt : TypeMolder
    {
        var actualType = value?.GetType() ?? typeof(TCloakedStruct?);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.RevealNullableCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags);
        using (callContext) { return mdc.RevealNullableCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> RevealNullableCloakedBearerOrNull<TCloakedStruct, TExt>(this ITypeMolderDieCast<TExt> mdc
      , TCloakedStruct? value, PalantírReveal<TCloakedStruct> styler, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct where TExt : TypeMolder
    {
        var sb = mdc.Sb;
        if (value != null)
        {
            if (formatFlags.HasIsFieldNameFlag())
                mdc.StyleFormatter.FormatFieldName(mdc.Master, value.Value, styler, formatString, formatFlags);
            else
                mdc.StyleFormatter.FormatFieldContents(mdc.Master, value.Value, styler, formatString, formatFlags);

            // if (!mdc.Settings.DisableCircularRefCheck && !typeof(TCloakedStruct).IsValueType)
            // {
            //     mdc.Master.EnsureRegisteredVisited(value, formatFlags);
            // }
        }
        else { mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags); }
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> RevealStringBearerField<TExt, TBearer>(this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName
      , TBearer value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.RevealStringBearerOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.RevealStringBearerOrNull(value, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> RevealStringBearerOrNull<TExt, TBearer>(this ITypeMolderDieCast<TExt> mdc
      , TBearer? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
        where TBearer : IStringBearer?
    {
        var sb = mdc.Sb;
        if (value != null)
        {
            if (formatFlags.HasIsFieldNameFlag())
                mdc.StyleFormatter.FormatFieldName(mdc.Master, value, formatString, formatFlags);
            else
                mdc.StyleFormatter.FormatFieldContents(mdc.Master, value, formatString, formatFlags);
        }
        else { mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags); }
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> RevealNullableStringBearerField<TExt, TBearerStruct>(this ITypeMolderDieCast<TExt> mdc
      , ReadOnlySpan<char> fieldName, TBearerStruct? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct?);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.RevealNullableStringBearerOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.RevealNullableStringBearerOrNull(value, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> RevealNullableStringBearerOrNull<TExt, TBearerStruct>(this ITypeMolderDieCast<TExt> mdc
      , TBearerStruct? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TBearerStruct : struct, IStringBearer
    {
        var sb = mdc.Sb;

        if (value == null)
        {
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc;
        }
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Master, value.Value, formatString, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Master, value.Value, formatString, formatFlags);
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> AppendReadOnlySpanField<TExt>
    (this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        if (mdc.HasSkipField(typeof(ReadOnlySpan<char>), fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNullOnZeroLength(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNullOnZeroLength(value, formatString, fromIndex, length, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNullOnZeroLength<TExt>
    (this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var sb         = mdc.Sb;
        var cappedFrom = Math.Clamp(fromIndex, 0, value.Length);
        if (value.Length == 0)
        {
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc;
        }
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, "InputIsCharSpan", formatFlags, formatString);
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> AppendStringField<TExt>
    (this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        if (mdc.HasSkipField(typeof(string), fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> mdc, string? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var sb = mdc.Sb;
        if (value == null)
        {
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc;
        }
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, "InputIsCharSpan", formatFlags, formatString);
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> AppendCharArrayField<TExt>
    (this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        if (mdc.HasSkipField(typeof(char[]), fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> mdc, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var sb = mdc.Sb;
        if (value == null)
        {
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> AppendCharSequenceField<TExt, TCharSeq>
    (this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt, TCharSeq>
    (this ITypeMolderDieCast<TExt> mdc, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
        where TCharSeq : ICharSequence?
    {
        var sb = mdc.Sb;
        if (value == null)
        {
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> AppendStringBuilderField<TExt>
    (this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        if (mdc.HasSkipField(typeof(StringBuilder), fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags);
        using (callContext) { return mdc.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> mdc, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var sb = mdc.Sb;
        if (value == null)
        {
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
            return mdc;
        }
        var cappedFrom = Math.Clamp(fromIndex, 0, value.Length);
        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
        if (formatFlags.HasIsFieldNameFlag())
            mdc.StyleFormatter.FormatFieldName(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            mdc.StyleFormatter.FormatFieldContents(mdc.Sb, value, cappedFrom, formatString, length, formatFlags);
        return mdc;
    }

    public static ITypeMolderDieCast<TExt> AppendObjectField<TExt>(this ITypeMolderDieCast<TExt> mdc, ReadOnlySpan<char> fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var actualType = value?.GetType() ?? typeof(object);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendMatchFormattedOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendMatchFormattedOrNull(value, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendMatchField<TExt, TAny>(this ITypeMolderDieCast<TExt> mdc
      , ReadOnlySpan<char> fieldName, TAny value, string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        var actualType = value?.GetType() ?? typeof(TAny);
        if (mdc.HasSkipField(actualType, fieldName, formatFlags)) return mdc;
        var callContext = mdc.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return mdc;

        mdc.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return mdc.AppendMatchFormattedOrNull(value, formatString, formatFlags);
        using (callContext) { return mdc.AppendMatchFormattedOrNull(value, formatString, formatFlags); }
    }

    public static ITypeMolderDieCast<TExt> AppendMatchFormattedOrNull<TValue, TExt>(this ITypeMolderDieCast<TExt> mdc
      , TValue value, string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var sb = mdc.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:           mdc.AppendFormatted(valueBool, formatString, formatFlags); break;
                case byte valueByte:           mdc.AppendFormattedOrNull(valueByte, formatString, formatFlags); break;
                case sbyte valueSByte:         mdc.AppendFormattedOrNull(valueSByte, formatString, formatFlags); break;
                case char valueChar:           mdc.AppendFormattedOrNull(valueChar, formatString, formatFlags); break;
                case short valueShort:         mdc.AppendFormattedOrNull(valueShort, formatString, formatFlags); break;
                case ushort valueUShort:       mdc.AppendFormattedOrNull(valueUShort, formatString, formatFlags); break;
                case Half valueHalfFloat:      mdc.AppendFormattedOrNull(valueHalfFloat, formatString, formatFlags); break;
                case int valueInt:             mdc.AppendFormattedOrNull(valueInt, formatString, formatFlags); break;
                case uint valueUInt:           mdc.AppendFormattedOrNull(valueUInt, formatString, formatFlags); break;
                case nint valueUInt:           mdc.AppendFormattedOrNull(valueUInt, formatString, formatFlags); break;
                case float valueFloat:         mdc.AppendFormattedOrNull(valueFloat, formatString, formatFlags); break;
                case long valueLong:           mdc.AppendFormattedOrNull(valueLong, formatString, formatFlags); break;
                case ulong valueULong:         mdc.AppendFormattedOrNull(valueULong, formatString, formatFlags); break;
                case double valueDouble:       mdc.AppendFormattedOrNull(valueDouble, formatString, formatFlags); break;
                case decimal valueDecimal:     mdc.AppendFormattedOrNull(valueDecimal, formatString, formatFlags); break;
                case Int128 veryLongInt:       mdc.AppendFormattedOrNull(veryLongInt, formatString, formatFlags); break;
                case UInt128 veryLongUInt:     mdc.AppendFormattedOrNull(veryLongUInt, formatString, formatFlags); break;
                case BigInteger veryLongInt:   mdc.AppendFormattedOrNull(veryLongInt, formatString, formatFlags); break;
                case Complex veryLongInt:      mdc.AppendFormattedOrNull(veryLongInt, formatString, formatFlags); break;
                case DateTime valueDateTime:   mdc.AppendFormattedOrNull(valueDateTime, formatString, formatFlags); break;
                case DateOnly valueDateOnly:   mdc.AppendFormattedOrNull(valueDateOnly, formatString, formatFlags); break;
                case TimeSpan valueTimeSpan:   mdc.AppendFormattedOrNull(valueTimeSpan, formatString, formatFlags); break;
                case TimeOnly valueTimeOnly:   mdc.AppendFormattedOrNull(valueTimeOnly, formatString, formatFlags); break;
                case Rune valueRune:           mdc.AppendFormattedOrNull(valueRune, formatString, formatFlags); break;
                case Guid valueGuid:           mdc.AppendFormattedOrNull(valueGuid, formatString, formatFlags); break;
                case IPNetwork valueIpNetwork: mdc.AppendFormattedOrNull(valueIpNetwork, formatString, formatFlags); break;
                case Version valueVersion:     mdc.AppendFormattedOrNull(valueVersion, formatString, formatFlags); break;
                case IPAddress valueIpAddress: mdc.AppendFormattedOrNull(valueIpAddress, formatString, formatFlags); break;
                case Uri valueUri:             mdc.AppendFormattedOrNull(valueUri, formatString, formatFlags); break;
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
                        castInvoker(mdc, value, formatString, formatFlags);
                    }
                    else if (value is Enum valueEnum)
                    {
                        var castInvoker = (SpanFmtStructContentHandler<Enum>)invoker;
                        castInvoker(mdc, valueEnum, formatString, formatFlags);
                    }
                    else
                    {
                        var castInvoker = (SpanFmtStructContentHandler<ISpanFormattable>)invoker;
                        castInvoker(mdc, (ISpanFormattable)value, formatString, formatFlags);
                    }
                    break;

                case char[] valueCharArray:           mdc.AppendFormattedOrNull(valueCharArray, formatString, formatFlags: formatFlags); break;
                case string valueString:              mdc.AppendFormattedOrNull(valueString, formatString, formatFlags: formatFlags); break;
                case ICharSequence valueCharSequence: mdc.AppendFormattedOrNull(valueCharSequence, formatString, formatFlags: formatFlags); break;
                case StringBuilder valueSb:           mdc.AppendFormattedOrNull(valueSb, formatString, formatFlags: formatFlags); break;

                case IStringBearer styledToStringObj: mdc.RevealStringBearerOrNull(styledToStringObj, formatString, formatFlags); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    mdc.Master.SetCallerFormatFlags(formatFlags);
                    mdc.Master.SetCallerFormatString(formatString);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = mdc.Master.StartKeyedCollectionType(value);
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString, formatFlags);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = mdc.Master.StartSimpleCollectionType(value);
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString, formatFlags);
                    orderedCollectionBuilder.Complete();
                    break;

                default:
                    var unknownType = value.GetType();
                    if (formatFlags.HasIsFieldNameFlag())
                        mdc.StyleFormatter.FormatFieldNameMatch(mdc.Sb, value, formatString, formatFlags);
                    else
                    {
                        if (unknownType.IsValueType || mdc.Master.IsCallerSameInstanceAndMoreDerived(value))
                            mdc.StyleFormatter.FormatFieldContentsMatch(mdc.Sb, value, formatString, formatFlags);
                        else
                            mdc.Master.RegisterVisitedInstanceAndConvert(value, formatString, formatFlags);
                    }
                    break;
            }
        else
            mdc.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags);
        return mdc;
    }

    public static IStringBuilder AppendFormattedCollectionItemMatchOrNull<TValue, TExt>(this ITypeMolderDieCast<TExt> mdc
      , TValue value, int retrieveCount, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder
    {
        var sb = mdc.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool: mdc.AppendFormattedCollectionItem(valueBool, retrieveCount, formatString, formatFlags); break;
                case byte valueByte: mdc.AppendFormattedCollectionItem(valueByte, retrieveCount, formatString, formatFlags); break;
                case sbyte valueSByte: mdc.AppendFormattedCollectionItem(valueSByte, retrieveCount, formatString, formatFlags); break;
                case char valueChar: mdc.AppendFormattedCollectionItem(valueChar, retrieveCount, formatString, formatFlags); break;
                case short valueShort: mdc.AppendFormattedCollectionItem(valueShort, retrieveCount, formatString, formatFlags); break;
                case ushort valueUShort: mdc.AppendFormattedCollectionItem(valueUShort, retrieveCount, formatString, formatFlags); break;
                case Half valueHalfFloat: mdc.AppendFormattedCollectionItem(valueHalfFloat, retrieveCount, formatString, formatFlags); break;
                case int valueInt: mdc.AppendFormattedCollectionItem(valueInt, retrieveCount, formatString, formatFlags); break;
                case uint valueUInt: mdc.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString, formatFlags); break;
                case nint valueUInt: mdc.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString, formatFlags); break;
                case float valueFloat: mdc.AppendFormattedCollectionItem(valueFloat, retrieveCount, formatString, formatFlags); break;
                case long valueLong: mdc.AppendFormattedCollectionItem(valueLong, retrieveCount, formatString, formatFlags); break;
                case ulong valueULong: mdc.AppendFormattedCollectionItem(valueULong, retrieveCount, formatString, formatFlags); break;
                case double valueDouble: mdc.AppendFormattedCollectionItem(valueDouble, retrieveCount, formatString, formatFlags); break;
                case decimal valueDecimal: mdc.AppendFormattedCollectionItem(valueDecimal, retrieveCount, formatString, formatFlags); break;
                case Int128 veryLongInt: mdc.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags); break;
                case UInt128 veryLongUInt: mdc.AppendFormattedCollectionItem(veryLongUInt, retrieveCount, formatString, formatFlags); break;
                case BigInteger veryLongInt: mdc.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags); break;
                case Complex veryLongInt: mdc.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags); break;
                case DateTime valueDateTime: mdc.AppendFormattedCollectionItem(valueDateTime, retrieveCount, formatString, formatFlags); break;
                case DateOnly valueDateOnly: mdc.AppendFormattedCollectionItem(valueDateOnly, retrieveCount, formatString, formatFlags); break;
                case TimeSpan valueTimeSpan: mdc.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString, formatFlags); break;
                case TimeOnly valueTimeOnly: mdc.AppendFormattedCollectionItem(valueTimeOnly, retrieveCount, formatString, formatFlags); break;
                case Rune valueRune: mdc.AppendFormattedCollectionItem(valueRune, retrieveCount, formatString, formatFlags); break;
                case Guid valueGuid: mdc.AppendFormattedCollectionItem(valueGuid, retrieveCount, formatString, formatFlags); break;
                case IPNetwork valueIpNetwork: mdc.AppendFormattedCollectionItem(valueIpNetwork, retrieveCount, formatString, formatFlags); break;
                case char[] valueCharArray: mdc.AppendFormattedCollectionItemOrNull(valueCharArray, retrieveCount, formatString, formatFlags); break;
                case string valueString: mdc.AppendFormattedCollectionItemOrNull(valueString, retrieveCount, formatString, formatFlags); break;
                case Version valueVersion: mdc.AppendFormattedCollectionItem(valueVersion, retrieveCount, formatString, formatFlags); break;
                case IPAddress valueIpAddress: mdc.AppendFormattedCollectionItem(valueIpAddress, retrieveCount, formatString, formatFlags); break;
                case Uri valueUri: mdc.AppendFormattedCollectionItem(valueUri, retrieveCount, formatString, formatFlags); break;
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
                        castInvoker(mdc, value, retrieveCount, formatString, formatFlags);
                    }
                    else if (value is Enum valueEnum)
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<Enum>)invoker;
                        castInvoker(mdc, valueEnum, retrieveCount, formatString, formatFlags);
                    }
                    else
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<ISpanFormattable>)invoker;
                        castInvoker(mdc, (ISpanFormattable)value, retrieveCount, formatString, formatFlags);
                    }
                    break;

                case ICharSequence valueCharSequence:
                    mdc.AppendFormattedCollectionItemOrNull(valueCharSequence, retrieveCount, formatString, formatFlags);
                    break;
                case StringBuilder valueSb: mdc.AppendFormattedCollectionItemOrNull(valueSb, retrieveCount, formatString, formatFlags); break;

                case IStringBearer styledToStringObj: mdc.RevealStringBearerOrNull(styledToStringObj, formatString, formatFlags); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    mdc.Master.SetCallerFormatFlags(formatFlags);
                    mdc.Master.SetCallerFormatString(formatString);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = mdc.Master.StartKeyedCollectionType(value);
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString, formatFlags);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = mdc.Master.StartSimpleCollectionType(value);
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString, formatFlags);
                    orderedCollectionBuilder.Complete();
                    break;

                default:
                    var unKnownType = typeof(TValue);
                    if (unKnownType.IsValueType || (!unKnownType.IsAnyTypeHoldingChars()))
                    {
                        formatFlags = mdc.StyleFormatter.ResolveContentFormattingFlags(mdc.Sb, value, formatFlags, formatString);
                        bool isSpanFormattableType = unKnownType.IsSpanFormattableCached();
                        if (!formatFlags.HasNoRevisitCheck()
                         && !typeof(TValue).IsValueType
                         && (!isSpanFormattableType
                          || mdc.Settings.InstanceTrackingIncludeSpanFormattableClasses))
                        {
                            var preAppendLength      = mdc.Sb.Length;
                            var registeredForRevisit = mdc.Master.EnsureRegisteredClassIsReferenceTracked(value, formatFlags);
                            if (!registeredForRevisit.ShouldSuppressBody 
                             || (isSpanFormattableType && mdc.Settings.InstanceMarkingIncludeSpanFormattableContents))
                            {
                                if (registeredForRevisit.ShouldSuppressBody)
                                    mdc.StyleFormatter.AppendInstanceValuesFieldName(typeof(TValue), formatFlags);
                                mdc.StyleFormatter.CollectionNextItem(value, retrieveCount, mdc.Sb, (FormatSwitches)formatFlags);
                            }
                            var gb = mdc.Sf.Gb;
                            gb.Complete(formatFlags);
                            registeredForRevisit.Complete();
                            gb.StartNextContentSeparatorPaddingSequence(mdc.Sb, formatFlags, true);
                            gb.MarkContentStart(preAppendLength);
                            gb.MarkContentEnd(mdc.Sb.Length);
                            return sb;
                        }
                        mdc.StyleFormatter.CollectionNextItem(value, retrieveCount, mdc.Sb, (FormatSwitches)formatFlags);
                        return sb;
                    }
                    break;
            }
        else
        {
            if (!formatFlags.HasNullBecomesEmptyFlag())
            {
                mdc.StyleFormatter.AppendFormattedNull(mdc.Sb, formatString, formatFlags);
            }
        }
        return sb;
    }


    public static int FieldNameJoin(this ITypeMolderDieCast mdc, ReadOnlySpan<char> fieldName)
    {
        var preAppendLen = mdc.Sb.Length;
        var sf           = mdc.StyleFormatter;
        // if (sf.AddedContextOnThisCall)
        // {
        //     sf = sf.PreviousContextOrThis;
        // }
        sf.AppendFieldName(mdc.Sb, fieldName);
        sf.AppendFieldValueSeparator();
        mdc.IsEmpty = false;
        return mdc.Sb.Length - preAppendLen;
    }


    public static int FieldNameWithDefaultJoin(this ITypeMolderDieCast mdc, ReadOnlySpan<char> fieldName, string onEmptyUseFieldName = "$values")
    {
        var preAppendLen = mdc.Sb.Length;
        var sf           = mdc.StyleFormatter;
        // if (sf.AddedContextOnThisCall)
        // {
        //     sf = sf.PreviousContextOrThis;
        // }
        sf.AppendFieldName(mdc.Sb, fieldName);
        sf.AppendFieldValueSeparator();
        mdc.IsEmpty = false;
        return mdc.Sb.Length - preAppendLen;
    }

    public static int FieldEnd<TExt>(this ITypeMolderDieCast<TExt> mdc) where TExt : TypeMolder
    {
        var preAppendLen = mdc.Sb.Length;
        var sf           = mdc.StyleFormatter;
        if (sf.AddedContextOnThisCall)
        {
            sf = sf.PreviousContextOrThis;
        }
        sf.AppendFieldValueSeparator();
        mdc.IsEmpty = false;
        return mdc.Sb.Length - preAppendLen;
    }

    public static void GoToNextCollectionItemStart<TExt>(this ITypeMolderDieCast<TExt> mdc, Type elementType, int elementAt) where TExt : TypeMolder
    {
        mdc.StyleFormatter.AddCollectionElementSeparatorAndPadding(mdc, elementType, elementAt + 1);
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt>
    (this ITypeMolderDieCast<TExt> mdc, bool value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(mdc);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt>
    (this ITypeMolderDieCast<TExt> mdc, bool? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(mdc);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> mdc, TFmt value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder where TFmt : ISpanFormattable? =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(mdc);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt, TFmtStruct>
    (this ITypeMolderDieCast<TExt> mdc, TFmtStruct? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(mdc);

    public static int DynamicReceiveAppendFormattedCollectionItem<TFmt>(ITypeMolderDieCast mdc, TFmt? value
      , int retrieveCount, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        var preAppendLength = mdc.Sb.Length;
        mdc.StyleFormatter.CollectionNextItemFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags);
        return mdc.Sb.Length - preAppendLength;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> mdc, string? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(mdc);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> mdc, char[]? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags).AnyToCompAccess(mdc);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> mdc, ICharSequence? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextCharSeqFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags).AnyToCompAccess(mdc);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> mdc, StringBuilder? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder =>
        mdc.StyleFormatter.CollectionNextItemFormat(mdc.Sb, value, retrieveCount, formatString, formatFlags).AnyToCompAccess(mdc);

    private delegate int SpanFmtStructContentHandler<in TFmt>(ITypeMolderDieCast mdc, TFmt fmt
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
                ($"{methodInfo.Name}_DynamicStructAppend", typeof(int),
                 [typeof(ITypeMolderDieCast), typeof(T), typeof(string), typeof(FormatFlags)]
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

    private delegate int SpanFmtStructCollectionElementHandler<in TFmt>(ITypeMolderDieCast mdc
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
                ($"{methodInfo.Name}_DynamicStructAppend", typeof(int),
                 [typeof(ITypeMolderDieCast), typeof(TFmt), typeof(int), typeof(string), typeof(FormatFlags)]
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
