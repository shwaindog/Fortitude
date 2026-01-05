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
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public record struct StateExtractStringRange(string TypeName, ITheOneString TypeTheOneString, Range AppendRange)
{
    public static implicit operator StateExtractStringRange(TypeMolder stb) => stb.Complete();


    public static readonly StateExtractStringRange EmptyAppend =
        new("Empty", null!, new Range(Index.FromStart(0), Index.FromStart(0)));
}

public abstract class TypeMolder : ExplicitRecyclableObject, IDisposable
{
    protected int StartIndex;

    protected void InitializeStyledTypeBuilder(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags )
    {
        PortableState ??= new();
        
        PortableState.TypeBeingBuilt      = typeBeingBuilt;
        PortableState.Master              = master;
        PortableState.TypeName            = typeName;
        PortableState.RemainingGraphDepth = remainingGraphDepth;
        PortableState.TypeFormatting      = typeFormatting;
        PortableState.AppenderSettings    = typeSettings;
        PortableState.CompleteResult      = null;
        PortableState.ExistingRefId       = existingRefId;
        PortableState.CreateFormatFlags   = createFormatFlags;

        StartIndex = master.WriteBuffer.Length;
    }

    protected StyleTypeBuilderPortableState PortableState { get; set; }

    public bool IsComplete => PortableState.CompleteResult != null;

    public int ExistingRefId => PortableState.ExistingRefId;

    public abstract void Start();

    public Type TypeBeingBuilt => PortableState.TypeBeingBuilt;

    public StyleOptions Settings => PortableState.Master.Settings;

    public string? TypeName => PortableState.TypeName;

    public abstract bool IsComplexType { get; }

    public abstract StateExtractStringRange Complete();

    public void Dispose()
    {
        if (!IsComplete) { PortableState.CompleteResult = Complete(); }
    }

    protected override void InheritedStateReset()
    {
        PortableState.Master           = null!;
        PortableState.TypeName         = null!;
        PortableState.AppenderSettings = default;
        PortableState.CompleteResult   = null;
        PortableState.ExistingRefId    = 0;

        StartIndex = -1;

        MeRecyclable.StateReset();
    }

    public class StyleTypeBuilderPortableState
    {
        public MoldDieCastSettings AppenderSettings;

        public Type TypeBeingBuilt { get; set; } = null!;
        public string? TypeName { get; set; }

        public IStyledTypeFormatting TypeFormatting { get; set; } = null!;
        public int ExistingRefId { get; set; }
        
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

    public static TExt AddGoToNext<TExt>(this ITypeMolderDieCast<TExt> stb)
        where TExt : TypeMolder
    {
        if (stb.StyleFormatter.GraphBuilder.HasCommitContent)
        {
            stb.StyleFormatter.AddNextFieldSeparatorAndPadding().ToTypeBuilder(stb);
        }
        return stb.StyleTypeBuilder;
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

    public static ITypeMolderDieCast<TExt> AppendNullableBooleanField<TExt>(this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName
      , bool? value
      , string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder
    {
        if (stb.SkipField<bool?>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            var sb = stb.Sb;
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
            return stb;
        }
        if (!callContext.HasFormatChange) return stb.AppendFormattedOrNull(value, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.AppendFormattedOrNull(value, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, bool? value, string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false) where TExt : TypeMolder
    {
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags, formatString, isKeyName);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, formatString, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, formatString, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendBooleanField<TExt>(this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, bool value
      , string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder
    {
        if (stb.SkipField<bool>(typeof(bool),  fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendFormatted(value, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.AppendFormatted(value, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormatted<TExt>(this ITypeMolderDieCast<TExt> stb, bool value, string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false) where TExt : TypeMolder
    {
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags, formatString, isKeyName);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, formatString, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, formatString, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattableField<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder where TFmt : ISpanFormattable?
    {
        if (stb.SkipField<TFmt>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendFormatted(value, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.AppendFormatted(value, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormatted<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> stb, TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder where TFmt : ISpanFormattable?
    {
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags, formatString, isKeyName);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, formatString, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, formatString, formatFlags);
        return stb;
    }

    public static IStringBuilder DynamicReceiveAppendValue<TFmt>(IStringBuilder sb, IStyledTypeFormatting stf, TFmt value, string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false) where TFmt : ISpanFormattable
    {
        if (isKeyName)
            stf.FormatFieldName(sb, value, formatString, formatFlags);
        else
            stf.FormatFieldContents(sb, value, formatString, formatFlags);
        return sb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattableField<TExt, TFmtStruct>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipField<TFmtStruct?>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendNullableFormattedOrNull(value, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.AppendNullableFormattedOrNull(value, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendNullableFormattedOrNull<TExt, TFmtStruct>
    (this ITypeMolderDieCast<TExt> stb, TFmtStruct? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable
    {
        if (value == null)
        {
            var sb = stb.Sb;
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
            return stb;
        }
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags, formatString, isKeyName);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, formatString, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, formatString, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> RevealCloakedBearerField<TCloaked, TCloakedBase, TExt>(this ITypeMolderDieCast<TExt> stb
      , ReadOnlySpan<char> fieldName, TCloaked? value, PalantírReveal<TCloakedBase> cloakedRevealer, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TCloaked : TCloakedBase 
        where TExt : TypeMolder
        where TCloakedBase : notnull
    {
        if (stb.SkipField<TCloaked>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.RevealCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.RevealCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> RevealCloakedBearerOrNull<TCloaked, TRevealBase, TExt>(this ITypeMolderDieCast<TExt> stb
      , TCloaked? value, PalantírReveal<TRevealBase> styler, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isKeyName = false) 
        where TCloaked : TRevealBase? 
        where TExt : TypeMolder
        where TRevealBase : notnull
    {
        var sb = stb.Sb;
        if (value != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb.Master, value, styler, formatString, formatFlags);
            else
                stb.StyleFormatter.FormatFieldContents(stb.Master, value, styler, formatString, formatFlags);

            if (!stb.Settings.DisableCircularRefCheck && !typeof(TCloaked).IsValueType) { stb.Master.EnsureRegisteredVisited(value); }
        }
        else { 
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
        }
        return stb;
    }

    public static ITypeMolderDieCast<TExt> RevealNullableCloakedBearerField<TCloakedStruct, TExt>(this ITypeMolderDieCast<TExt> stb
      , ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> cloakedRevealer
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TCloakedStruct : struct where TExt : TypeMolder
    {
        if (stb.SkipField<TCloakedStruct?>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.RevealNullableCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.RevealNullableCloakedBearerOrNull(value, cloakedRevealer, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> RevealNullableCloakedBearerOrNull<TCloakedStruct, TExt>(this ITypeMolderDieCast<TExt> stb
      , TCloakedStruct? value, PalantírReveal<TCloakedStruct> styler, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TCloakedStruct : struct where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb.Master, value.Value, styler, formatString, formatFlags);
            else
                stb.StyleFormatter.FormatFieldContents(stb.Master, value.Value, styler, formatString, formatFlags);

            if (!stb.Settings.DisableCircularRefCheck && !typeof(TCloakedStruct).IsValueType) { stb.Master.EnsureRegisteredVisited(value); }
        }
        else { 
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
        }
        return stb;
    }

    public static ITypeMolderDieCast<TExt> RevealStringBearerField<TExt, TBearer>(this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName
      , TBearer value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder where TBearer : IStringBearer?
    {
        if (stb.SkipField<TBearer>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.RevealStringBearerOrNull(value, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.RevealStringBearerOrNull(value, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> RevealStringBearerOrNull<TExt, TBearer>(this ITypeMolderDieCast<TExt> stb
      , TBearer? value, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false) 
        where TExt : TypeMolder
        where TBearer : IStringBearer?
    {
        var sb = stb.Sb;
        if (value != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb.Master, value, formatString, formatFlags);
            else
                stb.StyleFormatter.FormatFieldContents(stb.Master, value, formatString, formatFlags);
        }
        else { 
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
        }
        return stb;
    }

    public static ITypeMolderDieCast<TExt> RevealNullableStringBearerField<TExt, TBearerStruct>(this ITypeMolderDieCast<TExt> stb
      , ReadOnlySpan<char> fieldName, TBearerStruct? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isKeyName = false)
        where TExt : TypeMolder where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipField<TBearerStruct?>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.RevealNullableStringBearerOrNull(value, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.RevealNullableStringBearerOrNull(value, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> RevealNullableStringBearerOrNull<TExt, TBearerStruct>(this ITypeMolderDieCast<TExt> stb
      , TBearerStruct? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder where TBearerStruct : struct, IStringBearer
    {
        var sb = stb.Sb;

        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Master, value.Value, formatString, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Master, value.Value, formatString, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendReadOnlySpanField<TExt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isKeyName = false) where TExt : TypeMolder
    {
        if (stb.SkipField<ReadOnlyMemory<char>>(typeof(ReadOnlyMemory<char>), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange)
            return stb.AppendFormattedOrNullOnZeroLength(value, formatString, fromIndex, length, formatFlags, isKeyName);
        using (callContext) { return stb.AppendFormattedOrNullOnZeroLength(value, formatString, fromIndex, length, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNullOnZeroLength<TExt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isKeyName = false) where TExt : TypeMolder
    {
        var sb         = stb.Sb;
        var cappedFrom = Math.Clamp(fromIndex, 0, value.Length);
        if (value.Length == 0)
        {
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
            return stb;
        }
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, "InputIsCharSpan", formatFlags, formatString, isKeyName);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendStringField<TExt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isKeyName = false) where TExt : TypeMolder
    {
        if (stb.SkipField<string>(typeof(string), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags, isKeyName);
        using (callContext) { return stb.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, string? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false) 
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
            return stb;
        }
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, "InputIsCharSpan", formatFlags, formatString, isKeyName);
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendCharArrayField<TExt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , int fromIndex = 0, int length = int.MaxValue, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isKeyName = false) where TExt : TypeMolder
    {
        if (stb.SkipField<char[]>(typeof(char[]), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags, isKeyName);
        using (callContext) { return stb.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
            return stb;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value
                                                                     , formatFlags, formatString, isKeyName);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendCharSequenceField<TExt, TCharSeq>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder
        where TCharSeq : ICharSequence?
    {
        if (stb.SkipField<TCharSeq>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags, isKeyName);
        using (callContext) { return stb.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt, TCharSeq>
    (this ITypeMolderDieCast<TExt> stb, TCharSeq? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder
        where TCharSeq : ICharSequence?
    {
        var sb = stb.Sb;
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
            return stb;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value
                                                                     , formatFlags, formatString, isKeyName);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendStringBuilderField<TExt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder
    {
        if (stb.SkipField<StringBuilder>(typeof(StringBuilder), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags, isKeyName);
        using (callContext) { return stb.AppendFormattedOrNull(value, formatString, fromIndex, length, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = "", int fromIndex = 0, int length = int.MaxValue
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
            return stb;
        }
        var cappedFrom = Math.Clamp(fromIndex, 0, value.Length);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value
                                                                     , formatFlags, formatString, isKeyName);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length, formatFlags);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendObjectField<TExt>(this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString = ""
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false) where TExt : TypeMolder
    {
        if (stb.SkipField<object>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendMatchFormattedOrNull(value, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.AppendMatchFormattedOrNull(value, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendMatchField<TExt, TAny>(this ITypeMolderDieCast<TExt> stb
      , ReadOnlySpan<char> fieldName, TAny value, string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isKeyName = false) where TExt : TypeMolder
    {
        if (stb.SkipField<TAny>(value?.GetType(), fieldName, formatFlags)) return stb;
        var callContext = stb.Master.ResolveContextForCallerFlags(formatFlags);
        if (callContext.ShouldSkip) return stb;

        stb.FieldNameJoin(fieldName);
        if (!callContext.HasFormatChange) return stb.AppendMatchFormattedOrNull(value, formatString, formatFlags, isKeyName);
        using (callContext) { return stb.AppendMatchFormattedOrNull(value, formatString, formatFlags, isKeyName); }
    }

    public static ITypeMolderDieCast<TExt> AppendMatchFormattedOrNull<TValue, TExt>(this ITypeMolderDieCast<TExt> stb
      , TValue value, string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags, bool isKeyName = false) where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:           stb.AppendFormatted(valueBool, formatString, formatFlags, isKeyName); break;
                case byte valueByte:           stb.AppendFormatted(valueByte, formatString, formatFlags, isKeyName); break;
                case sbyte valueSByte:         stb.AppendFormatted(valueSByte, formatString, formatFlags, isKeyName); break;
                case char valueChar:           stb.AppendFormatted(valueChar, formatString, formatFlags, isKeyName); break;
                case short valueShort:         stb.AppendFormatted(valueShort, formatString, formatFlags, isKeyName); break;
                case ushort valueUShort:       stb.AppendFormatted(valueUShort, formatString, formatFlags, isKeyName); break;
                case Half valueHalfFloat:      stb.AppendFormatted(valueHalfFloat, formatString, formatFlags, isKeyName); break;
                case int valueInt:             stb.AppendFormatted(valueInt, formatString, formatFlags, isKeyName); break;
                case uint valueUInt:           stb.AppendFormatted(valueUInt, formatString, formatFlags, isKeyName); break;
                case nint valueUInt:           stb.AppendFormatted(valueUInt, formatString, formatFlags, isKeyName); break;
                case float valueFloat:         stb.AppendFormatted(valueFloat, formatString, formatFlags, isKeyName); break;
                case long valueLong:           stb.AppendFormatted(valueLong, formatString, formatFlags, isKeyName); break;
                case ulong valueULong:         stb.AppendFormatted(valueULong, formatString, formatFlags, isKeyName); break;
                case double valueDouble:       stb.AppendFormatted(valueDouble, formatString, formatFlags, isKeyName); break;
                case decimal valueDecimal:     stb.AppendFormatted(valueDecimal, formatString, formatFlags, isKeyName); break;
                case Int128 veryLongInt:       stb.AppendFormatted(veryLongInt, formatString, formatFlags, isKeyName); break;
                case UInt128 veryLongUInt:     stb.AppendFormatted(veryLongUInt, formatString, formatFlags, isKeyName); break;
                case BigInteger veryLongInt:   stb.AppendFormatted(veryLongInt, formatString, formatFlags, isKeyName); break;
                case Complex veryLongInt:      stb.AppendFormatted(veryLongInt, formatString, formatFlags, isKeyName); break;
                case DateTime valueDateTime:   stb.AppendFormatted(valueDateTime, formatString, formatFlags, isKeyName); break;
                case DateOnly valueDateOnly:   stb.AppendFormatted(valueDateOnly, formatString, formatFlags, isKeyName); break;
                case TimeSpan valueTimeSpan:   stb.AppendFormatted(valueTimeSpan, formatString, formatFlags, isKeyName); break;
                case TimeOnly valueTimeOnly:   stb.AppendFormatted(valueTimeOnly, formatString, formatFlags, isKeyName); break;
                case Rune valueRune:           stb.AppendFormatted(valueRune, formatString, formatFlags, isKeyName); break;
                case Guid valueGuid:           stb.AppendFormatted(valueGuid, formatString, formatFlags, isKeyName); break;
                case IPNetwork valueIpNetwork: stb.AppendFormatted(valueIpNetwork, formatString, formatFlags, isKeyName); break;
                case char[] valueCharArray:
                    stb.AppendFormattedOrNull(valueCharArray, formatString, formatFlags: formatFlags, isKeyName: isKeyName);
                    break;
                case string valueString: stb.AppendFormattedOrNull(valueString, formatString, formatFlags: formatFlags, isKeyName: isKeyName); break;
                case Version valueVersion: stb.AppendFormatted(valueVersion, formatString, formatFlags, isKeyName); break;
                case IPAddress valueIpAddress: stb.AppendFormatted(valueIpAddress, formatString, formatFlags, isKeyName); break;
                case Uri valueUri: stb.AppendFormatted(valueUri, formatString, formatFlags, isKeyName); break;
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
                        castInvoker(stb.Sb, stb.StyleFormatter, value, formatString, formatFlags, isKeyName);
                    }
                    else if (value is Enum valueEnum)
                    {
                        var castInvoker = (SpanFmtStructContentHandler<Enum>)invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, valueEnum, formatString, formatFlags, isKeyName);
                    }
                    else
                    {
                        var castInvoker = (SpanFmtStructContentHandler<ISpanFormattable>)invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, (ISpanFormattable)value, formatString, formatFlags, isKeyName);
                    }
                    break;

                case ICharSequence valueCharSequence:
                    stb.AppendFormattedOrNull(valueCharSequence, formatString, formatFlags: formatFlags, isKeyName: isKeyName);
                    break;
                case StringBuilder valueSb: stb.AppendFormattedOrNull(valueSb, formatString, formatFlags: formatFlags, isKeyName: isKeyName); break;

                case IStringBearer styledToStringObj: stb.RevealStringBearerOrNull(styledToStringObj, formatString, formatFlags, isKeyName); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    stb.Master.SetCallerFormatFlags(formatFlags);
                    stb.Master.SetCallerFormatString(formatString);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.Master.StartKeyedCollectionType(value);
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString, formatFlags);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = stb.Master.StartSimpleCollectionType(value);
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString, formatFlags);
                    orderedCollectionBuilder.Complete();
                    break;

                default:
                    var unknownType = value.GetType();
                    if (isKeyName)
                        stb.StyleFormatter.FormatFieldNameMatch(stb.Sb, value, formatString, formatFlags);
                    else
                    {
                        if (unknownType.IsValueType || stb.Master.IsLastVisitedObject(value))
                            stb.StyleFormatter.FormatFieldContentsMatch(stb.Sb, value, formatString, formatFlags);
                        else
                            stb.Master.RegisterVisitedInstanceAndConvert(value, isKeyName, formatString, formatFlags);
                    }
                    break;
            }
        else
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
        return stb;
    }

    public static IStringBuilder AppendFormattedCollectionItemMatchOrNull<TValue, TExt>(this ITypeMolderDieCast<TExt> stb
      , TValue value, int retrieveCount, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool isKeyName = false) where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool: stb.AppendFormattedCollectionItem(valueBool, retrieveCount, formatString, formatFlags); break;
                case byte valueByte: stb.AppendFormattedCollectionItem(valueByte, retrieveCount, formatString, formatFlags); break;
                case sbyte valueSByte: stb.AppendFormattedCollectionItem(valueSByte, retrieveCount, formatString, formatFlags); break;
                case char valueChar: stb.AppendFormattedCollectionItem(valueChar, retrieveCount, formatString, formatFlags); break;
                case short valueShort: stb.AppendFormattedCollectionItem(valueShort, retrieveCount, formatString, formatFlags); break;
                case ushort valueUShort: stb.AppendFormattedCollectionItem(valueUShort, retrieveCount, formatString, formatFlags); break;
                case Half valueHalfFloat: stb.AppendFormattedCollectionItem(valueHalfFloat, retrieveCount, formatString, formatFlags); break;
                case int valueInt: stb.AppendFormattedCollectionItem(valueInt, retrieveCount, formatString, formatFlags); break;
                case uint valueUInt: stb.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString, formatFlags); break;
                case nint valueUInt: stb.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString, formatFlags); break;
                case float valueFloat: stb.AppendFormattedCollectionItem(valueFloat, retrieveCount, formatString, formatFlags); break;
                case long valueLong: stb.AppendFormattedCollectionItem(valueLong, retrieveCount, formatString, formatFlags); break;
                case ulong valueULong: stb.AppendFormattedCollectionItem(valueULong, retrieveCount, formatString, formatFlags); break;
                case double valueDouble: stb.AppendFormattedCollectionItem(valueDouble, retrieveCount, formatString, formatFlags); break;
                case decimal valueDecimal: stb.AppendFormattedCollectionItem(valueDecimal, retrieveCount, formatString, formatFlags); break;
                case Int128 veryLongInt: stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags); break;
                case UInt128 veryLongUInt: stb.AppendFormattedCollectionItem(veryLongUInt, retrieveCount, formatString, formatFlags); break;
                case BigInteger veryLongInt: stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags); break;
                case Complex veryLongInt: stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString, formatFlags); break;
                case DateTime valueDateTime: stb.AppendFormattedCollectionItem(valueDateTime, retrieveCount, formatString, formatFlags); break;
                case DateOnly valueDateOnly: stb.AppendFormattedCollectionItem(valueDateOnly, retrieveCount, formatString, formatFlags); break;
                case TimeSpan valueTimeSpan: stb.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString, formatFlags); break;
                case TimeOnly valueTimeOnly: stb.AppendFormattedCollectionItem(valueTimeOnly, retrieveCount, formatString, formatFlags); break;
                case Rune valueRune: stb.AppendFormattedCollectionItem(valueRune, retrieveCount, formatString, formatFlags); break;
                case Guid valueGuid: stb.AppendFormattedCollectionItem(valueGuid, retrieveCount, formatString, formatFlags); break;
                case IPNetwork valueIpNetwork: stb.AppendFormattedCollectionItem(valueIpNetwork, retrieveCount, formatString, formatFlags); break;
                case char[] valueCharArray: stb.AppendFormattedCollectionItemOrNull(valueCharArray, retrieveCount, formatString, formatFlags); break;
                case string valueString: stb.AppendFormattedCollectionItemOrNull(valueString, retrieveCount, formatString, formatFlags); break;
                case Version valueVersion: stb.AppendFormattedCollectionItem(valueVersion, retrieveCount, formatString, formatFlags); break;
                case IPAddress valueIpAddress: stb.AppendFormattedCollectionItem(valueIpAddress, retrieveCount, formatString, formatFlags); break;
                case Uri valueUri: stb.AppendFormattedCollectionItem(valueUri, retrieveCount, formatString, formatFlags); break;
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
                        castInvoker(stb.Sb, stb.StyleFormatter, value, retrieveCount, formatString, formatFlags);
                    }
                    else if (value is Enum valueEnum)
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<Enum>)invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, valueEnum, retrieveCount, formatString, formatFlags);
                    }
                    else
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<ISpanFormattable>)invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, (ISpanFormattable)value, retrieveCount, formatString, formatFlags);
                    }
                    break;

                case ICharSequence valueCharSequence: stb.AppendFormattedCollectionItemOrNull(valueCharSequence, retrieveCount, formatString, formatFlags); break;
                case StringBuilder valueSb:           stb.AppendFormattedCollectionItemOrNull(valueSb, retrieveCount, formatString, formatFlags); break;

                case IStringBearer styledToStringObj: stb.RevealStringBearerOrNull(styledToStringObj, formatString, formatFlags, isKeyName); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    stb.Master.SetCallerFormatFlags(formatFlags);
                    stb.Master.SetCallerFormatString(formatString);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.Master.StartKeyedCollectionType(value);
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString, formatFlags);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = stb.Master.StartSimpleCollectionType(value);
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString, formatFlags);
                    orderedCollectionBuilder.Complete();
                    break;

                default:
                    var unKnownType = typeof(TValue);
                    if (unKnownType.IsValueType || (!unKnownType.IsAnyTypeHoldingChars() || stb.Master.RegisterVisitedCheckCanContinue(value)))
                    {
                        stb.StyleFormatter.CollectionNextItem(value, retrieveCount, stb.Sb, (FormatSwitches)formatFlags);
                    }
                    break;
            }
        else
            stb.StyleFormatter.AppendFormattedNull(sb, formatString, formatFlags, isKeyName);
        return sb;
    }


    public static ITypeMolderDieCast<TExt> FieldNameJoin<TExt>(this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AppendFieldName(stb.Sb, fieldName);
        stb.StyleFormatter.AppendFieldValueSeparator(stb);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> FieldEnd<TExt>(this ITypeMolderDieCast<TExt> stb) where TExt : TypeMolder
    {
        stb.StyleFormatter.AppendFieldValueSeparator(stb);
        return stb;
    }

    public static void GoToNextCollectionItemStart<TExt>(this ITypeMolderDieCast<TExt> stb, Type elementType, int elementAt) where TExt : TypeMolder
    {
        stb.StyleFormatter.AddCollectionElementSeparatorAndPadding(stb, elementType, elementAt + 1);
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt>
    (this ITypeMolderDieCast<TExt> stb, bool value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt>
    (this ITypeMolderDieCast<TExt> stb, bool? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> stb, TFmt value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder where TFmt : ISpanFormattable? =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt, TFmtStruct>
    (this ITypeMolderDieCast<TExt> stb, TFmtStruct? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(stb);

    public static IStringBuilder DynamicReceiveAppendFormattedCollectionItem<TFmt>(IStringBuilder sb, IStyledTypeFormatting stf, TFmt? value
      , int retrieveCount, string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        stf.CollectionNextItemFormat(sb, value, retrieveCount, formatString, formatFlags);
        return sb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, string? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString, formatFlags)
           .AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, char[]? value, int retrieveCount, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString, formatFlags).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, ICharSequence? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextCharSeqFormat(stb.Sb, value, retrieveCount, formatString, formatFlags).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, StringBuilder? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString, formatFlags).AnyToCompAccess(stb);

    public static void StartDictionary<TExt>(this ITypeMolderDieCast<TExt> stb
      , Type dictionaryType, Type keyType, Type valueType, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AppendKeyedCollectionStart(stb.Sb, dictionaryType, keyType, valueType,  formatFlags);
    }

    public static void EndDictionary<TExt>(this ITypeMolderDieCast<TExt> stb)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AppendTypeClosing(stb);
    }

    private delegate IStringBuilder SpanFmtStructContentHandler<in TFmt>(IStringBuilder sb, IStyledTypeFormatting stf, TFmt fmt
      , string formatString = "", FormatFlags formatFlags = DefaultCallerTypeFlags, bool isFieldName = false);

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
                ($"{methodInfo.Name}_DynamicStructAppend", typeof(IStringBuilder),
                 [typeof(IStringBuilder), typeof(IStyledTypeFormatting), typeof(T), typeof(string), typeof(FormatFlags), typeof(bool)]
               , typeof(StyledTypeBuilderExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(SpanFmtStructContentHandler<T>));
        return (SpanFmtStructContentHandler<T>)methodInvoker;
    }

    private delegate IStringBuilder SpanFmtStructCollectionElementHandler<in TFmt>(IStringBuilder sb, IStyledTypeFormatting stf
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
                ($"{methodInfo.Name}_DynamicStructAppend", typeof(IStringBuilder),
                 [ typeof(IStringBuilder), typeof(IStyledTypeFormatting), typeof(TFmt), typeof(int), typeof(string), typeof(FormatFlags) ]
               , typeof(StyledTypeBuilderExtensions).Module, true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(SpanFmtStructCollectionElementHandler<TFmt>));
        return (SpanFmtStructCollectionElementHandler<TFmt>)methodInvoker;
    }
}
