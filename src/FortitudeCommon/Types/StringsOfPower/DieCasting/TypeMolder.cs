using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public record struct StateExtractStringRange(string TypeName, ITheOneString TypeTheOneString, Range AppendRange)
{
    public static implicit operator StateExtractStringRange(TypeMolder stb) => stb.Complete();


    public static readonly StateExtractStringRange EmptyAppend =
        new("Empty", null!, new Range(Index.FromStart(0), Index.FromStart(0)));
}

public abstract class TypeMolder : ExplicitRecyclableObject, IDisposable
{
    protected readonly StyleTypeBuilderPortableState PortableState = new();

    protected int StartIndex;

    protected void InitializeStyledTypeBuilder(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        PortableState.TypeBeingBuilt      = typeBeingBuilt;
        PortableState.Master      = master;
        PortableState.TypeName            = typeName;
        PortableState.RemainingGraphDepth = remainingGraphDepth;
        PortableState.TypeFormatting      = typeFormatting;
        PortableState.AppenderSettings    = typeSettings;
        PortableState.CompleteResult      = null;
        PortableState.ExistingRefId       = existingRefId;

        StartIndex = master.WriteBuffer.Length;
    }

    public bool IsComplete => PortableState.CompleteResult != null;

    public int ExistingRefId => PortableState.ExistingRefId;

    public abstract void Start();

    public Type TypeBeingBuilt => PortableState.TypeBeingBuilt;

    public StyleOptions Settings => PortableState.Master.Settings;

    public string TypeName => PortableState.TypeName;

    public abstract bool IsComplexType { get; }

    public abstract StateExtractStringRange Complete();

    public StyleOptions StyleSettings => PortableState.Master.Settings;

    public void Dispose()
    {
        if (!IsComplete)
        {
            PortableState.CompleteResult = Complete();
        }
    }

    protected override void InheritedStateReset()
    {
        PortableState.Master   = null!;
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
        public string TypeName { get; set; } = null!;

        public IStyledTypeFormatting TypeFormatting { get; set; } = null!;
        public int ExistingRefId { get; set; }

        public int RemainingGraphDepth { get; set; }

        public ISecretStringOfPower Master { get; set; } = null!;

        public StateExtractStringRange? CompleteResult;

        public bool IsComplete => CompleteResult != null;
    }
}

public interface ITypeBuilderComponentSource
{
    ITypeMolderDieCast ComponentAccess { get; }
}

public interface ITypeBuilderComponentSource<out T> : ITypeBuilderComponentSource where T : TypeMolder
{
    ITypeMolderDieCast<T> CompAccess { get; }
}

public static class StyledTypeBuilderExtensions
{
    internal const string Null                     = "null";
    internal const string NoFormattingFormatString = "{0}";
    
    private static readonly ConcurrentDictionary<(Type,Type), Delegate> DynamicSpanFmtContentInvokers           = new();
    private static readonly ConcurrentDictionary<(Type,Type), Delegate> DynamicSpanFmtCollectionElementInvokers = new();

    public static TExt AddGoToNext<TExt>(this ITypeMolderDieCast<TExt> stb)
        where TExt : TypeMolder
    {
        return stb.StyleFormatter.AddNextFieldSeparator(stb.Sb).ToTypeBuilder(stb);
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

    public static IStringBuilder Qt(this IStringBuilder sb, bool writeQuote) => writeQuote ? sb.Append("\"") : sb;

    public static ITypeMolderDieCast<TExt> Qt<TExt>(this ITypeMolderDieCast<TExt> stb, bool writeQuote)
        where TExt : TypeMolder =>
        writeQuote ? stb.Sb.Append("\"").AnyToCompAccess(stb) : stb;

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, bool? value, string? formatString
      , bool isKeyName = false) where TExt : TypeMolder 
    {
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, formatString);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, formatString);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormatted<TExt>(this ITypeMolderDieCast<TExt> stb, bool value, string? formatString
      , bool isKeyName = false) where TExt : TypeMolder 
    {
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, formatString);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, formatString);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormatted<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> stb, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool isKeyName = false)
        where TExt : TypeMolder where TFmt : ISpanFormattable
    {
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, formatString);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, formatString);
        return stb;
    }

    public static IStringBuilder DynamicReceiveAppendValue<TFmt>(IStringBuilder sb, IStyledTypeFormatting stf, TFmt value, string? formatString, bool isKeyName = false)
        where TFmt : ISpanFormattable
    {
        if (isKeyName)
            stf.FormatFieldName(sb, value, formatString);
        else
            stf.FormatFieldContents(sb, value, formatString);
        return sb;
    }

    public static ITypeMolderDieCast<TExt> AppendValue<TExt, TFmt>(this ITypeMolderDieCast<TExt> stb, TFmt? value
      , bool isKeyName = false)
        where TExt : TypeMolder where TFmt : ISpanFormattable
    {
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendNullableFormattedOrNull<TExt, TStructFmt>
    (this ITypeMolderDieCast<TExt> stb, TStructFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, bool isKeyName = false)
        where TExt : TypeMolder where TStructFmt : struct, ISpanFormattable
    {
        if (value == null)
        {
            var sb = stb.Sb;
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, formatString);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, formatString);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt, T>(this ITypeMolderDieCast<TExt> stb, T? value
      , bool isKeyName = false)
        where TExt : TypeMolder where T : struct, ISpanFormattable
    {
        if (value == null)
        {
            var sb = stb.Sb;
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNullOnZeroLength<TExt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb         = stb.Sb;
        var cappedFrom = Math.Clamp(fromIndex, 0,  value.Length);
        var cappedLength   = Math.Clamp(length, 0, value.Length - cappedFrom);
        if (cappedLength == 0)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, string? value
      , bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> value
      , bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, len);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, len);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, ICharSequence? value
      , bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, ICharSequence? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString , length);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, StringBuilder? value
      , bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        var cappedFrom = Math.Clamp(fromIndex, 0, value.Length);
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, char[]? value
      , bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value == null)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb.Sb, value, cappedFrom, formatString, length);
        else
            stb.StyleFormatter.FormatFieldContents(stb.Sb, value, cappedFrom, formatString, length);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, bool isKeyName = false)
        where TExt : TypeMolder
    {
        if (value != null)
            if (isKeyName)
                stb.StyleFormatter.FormatFieldNameMatch(stb.Sb, value, formatString);
            else
            {
                var unknownType = value.GetType();
                if (unknownType.IsValueType)
                    stb.StyleFormatter.FormatFieldContentsMatch(stb.Sb, value, formatString);
                else
                    stb.Master.RegisterVisitedInstanceAndConvert(value, isKeyName, formatString);
            }
        else
            stb.Sb.Append(Null);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendRevealBearerOrNull<TExt>(this ITypeMolderDieCast<TExt> stb
      , IStringBearer? value, bool isKeyName = false) where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb.Master, value);
            else
                stb.StyleFormatter.FormatFieldContents(stb.Master, value);
        }
        else
        {
            sb.Append(Null);
        }
        return stb;
    }
    
    public static ITypeMolderDieCast<TExt> AppendRevealBearerOrNull<TExt, TBearer>(this ITypeMolderDieCast<TExt> stb
      , TBearer? value, bool isKeyName = false) where TExt : TypeMolder where TBearer : struct, IStringBearer
    {
        var sb = stb.Sb;
        if (value != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb.Master, value);
            else
                stb.StyleFormatter.FormatFieldContents(stb.Master, value);
        }
        else
        {
            sb.Append(Null);
        }
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TToStyle, TStylerType, TExt>(this ITypeMolderDieCast<TExt> stb
      , TToStyle? toStyle, PalantírReveal<TStylerType> styler, bool isKeyName = false) where TToStyle : TStylerType where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (toStyle != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb.Master, toStyle, styler);
            else
                stb.StyleFormatter.FormatFieldContents(stb.Master, toStyle, styler);

            if (!stb.Settings.DisableCircularRefCheck && !typeof(TToStyle).IsValueType)
            {
                stb.Master.EnsureRegisteredVisited(toStyle);
            }
        }
        else
        {
            sb.Append(Null);
        }
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TToStyle, TStylerType, TExt>(this ITypeMolderDieCast<TExt> stb
      , TToStyle? toStyle, PalantírReveal<TStylerType> styler, bool isKeyName = false) 
        where TToStyle : struct, TStylerType where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (toStyle != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb.Master, toStyle.Value, styler);
            else
                stb.StyleFormatter.FormatFieldContents(stb.Master, toStyle.Value, styler);

            if (!stb.Settings.DisableCircularRefCheck && !typeof(TToStyle).IsValueType)
            {
                stb.Master.EnsureRegisteredVisited(toStyle);
            }
        }
        else
        {
            sb.Append(Null);
        }
        return stb;
    }

    public static IStringBuilder AppendFormattedCollectionItemMatchOrNull<TValue, TExt>(this ITypeMolderDieCast<TExt> stb
      , TValue value, int retrieveCount, string formatString = "", bool isKeyName = false) where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:           stb.AppendFormattedCollectionItem(valueBool, retrieveCount, formatString); break;
                case byte valueByte:           stb.AppendFormattedCollectionItem(valueByte, retrieveCount, formatString); break;
                case sbyte valueSByte:         stb.AppendFormattedCollectionItem(valueSByte, retrieveCount, formatString); break;
                case char valueChar:           stb.AppendFormattedCollectionItem(valueChar, retrieveCount, formatString); break;
                case short valueShort:         stb.AppendFormattedCollectionItem(valueShort, retrieveCount, formatString); break;
                case ushort valueUShort:       stb.AppendFormattedCollectionItem(valueUShort, retrieveCount, formatString); break;
                case Half valueHalfFloat:      stb.AppendFormattedCollectionItem(valueHalfFloat, retrieveCount, formatString); break;
                case int valueInt:             stb.AppendFormattedCollectionItem(valueInt, retrieveCount, formatString); break;
                case uint valueUInt:           stb.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString); break;
                case nint valueUInt:           stb.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString); break;
                case float valueFloat:         stb.AppendFormattedCollectionItem(valueFloat, retrieveCount, formatString); break;
                case long valueLong:           stb.AppendFormattedCollectionItem(valueLong, retrieveCount, formatString); break;
                case ulong valueULong:         stb.AppendFormattedCollectionItem(valueULong, retrieveCount, formatString); break;
                case double valueDouble:       stb.AppendFormattedCollectionItem(valueDouble, retrieveCount, formatString); break;
                case decimal valueDecimal:     stb.AppendFormattedCollectionItem(valueDecimal, retrieveCount, formatString); break;
                case Int128 veryLongInt:       stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case UInt128 veryLongUInt:     stb.AppendFormattedCollectionItem(veryLongUInt, retrieveCount, formatString); break;
                case BigInteger veryLongInt:   stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case Complex veryLongInt:      stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case DateTime valueDateTime:   stb.AppendFormattedCollectionItem(valueDateTime, retrieveCount, formatString); break;
                case DateOnly valueDateOnly:   stb.AppendFormattedCollectionItem(valueDateOnly, retrieveCount, formatString); break;
                case TimeSpan valueTimeSpan:   stb.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString); break;
                case TimeOnly valueTimeOnly:   stb.AppendFormattedCollectionItem(valueTimeOnly, retrieveCount, formatString); break;
                case Rune valueRune:           stb.AppendFormattedCollectionItem(valueRune, retrieveCount, formatString); break;
                case Guid valueGuid:           stb.AppendFormattedCollectionItem(valueGuid, retrieveCount, formatString); break;
                case IPNetwork valueIpNetwork: stb.AppendFormattedCollectionItem(valueIpNetwork, retrieveCount, formatString); break;
                case char[] valueCharArray:    stb.AppendFormattedCollectionItemOrNull(valueCharArray, retrieveCount, formatString); break;
                case string valueString:       stb.AppendFormattedCollectionItemOrNull(valueString, retrieveCount,  formatString); break;
                case Version valueVersion:     stb.AppendFormattedCollectionItem(valueVersion, retrieveCount, formatString); break;
                case IPAddress valueIpAddress: stb.AppendFormattedCollectionItem(valueIpAddress, retrieveCount, formatString); break;
                case Uri valueUri:             stb.AppendFormattedCollectionItem(valueUri, retrieveCount, formatString); break;
                case Enum:
                case ISpanFormattable:
                    var actualValueType = value.GetType();
                    var typeOfTValue    = typeof(TValue);
                    var delegateKey     = (typeOfTValue, actualValueType);
                    // ReSharper disable once InconsistentlySynchronizedField
                    if(!DynamicSpanFmtCollectionElementInvokers.TryGetValue(delegateKey, out var invoker))
                    {
                        lock (DynamicSpanFmtCollectionElementInvokers)
                        {
                            if (!DynamicSpanFmtCollectionElementInvokers.TryGetValue(delegateKey, out invoker))
                            {
                                if (typeOfTValue.ImplementsInterface(typeof(ISpanFormattable)))
                                {
                                    invoker = CreateSpanFormattableCollectionElementInvoker<TValue>();
                                }
                                else if (value is Enum)
                                {
                                    invoker = CreateSpanFormattableCollectionElementInvoker<Enum>();
                                }
                                else 
                                {
                                    invoker = CreateSpanFormattableCollectionElementInvoker<ISpanFormattable>();
                                }
                                DynamicSpanFmtCollectionElementInvokers.TryAdd(delegateKey, invoker);
                            }
                        }
                    }
                    if (typeOfTValue.ImplementsInterface(typeof(ISpanFormattable)))
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<TValue>) invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, value, retrieveCount, formatString);
                    }
                    else if (value is Enum valueEnum)
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<Enum>) invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, valueEnum, retrieveCount, formatString);
                    }
                    else
                    {
                        var castInvoker = (SpanFmtStructCollectionElementHandler<ISpanFormattable>)invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, (ISpanFormattable)value, retrieveCount, formatString);
                    }
                    break;

                case ICharSequence valueCharSequence:   stb.AppendFormattedCollectionItemOrNull(valueCharSequence, retrieveCount, formatString); break;
                case StringBuilder valueSb:             stb.AppendFormattedCollectionItemOrNull(valueSb, retrieveCount, formatString); break;

                case IStringBearer styledToStringObj: stb.AppendRevealBearerOrNull(styledToStringObj); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.Master.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = stb.Master.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString);
                    orderedCollectionBuilder.Complete();
                    break;

                default: 
                    var unKnownType = typeof(TValue);
                    if (unKnownType.IsValueType || (!unKnownType.IsAnyTypeHoldingChars() || stb.Master.RegisterVisitedCheckCanContinue(value)))
                    {
                        stb.StyleFormatter.CollectionNextItem(value, retrieveCount, stb.Sb);
                    }
                    break;
            }
        else
            sb.Append(Null);
        return sb;
    }

    public static ITypeMolderDieCast<TExt> AppendMatchFormattedOrNull<TValue, TExt>(this ITypeMolderDieCast<TExt> stb
      , TValue value, string formatString, bool isKeyName = false) where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:           stb.AppendFormatted(valueBool, formatString, isKeyName); break;
                case byte valueByte:           stb.AppendFormatted(valueByte, formatString, isKeyName); break;
                case sbyte valueSByte:         stb.AppendFormatted(valueSByte, formatString, isKeyName); break;
                case char valueChar:           stb.AppendFormatted(valueChar, formatString, isKeyName); break;
                case short valueShort:         stb.AppendFormatted(valueShort, formatString, isKeyName); break;
                case ushort valueUShort:       stb.AppendFormatted(valueUShort, formatString, isKeyName); break;
                case Half valueHalfFloat:      stb.AppendFormatted(valueHalfFloat, formatString, isKeyName); break;
                case int valueInt:             stb.AppendFormatted(valueInt, formatString, isKeyName); break;
                case uint valueUInt:           stb.AppendFormatted(valueUInt, formatString, isKeyName); break;
                case nint valueUInt:           stb.AppendFormatted(valueUInt, formatString, isKeyName); break;
                case float valueFloat:         stb.AppendFormatted(valueFloat, formatString, isKeyName); break;
                case long valueLong:           stb.AppendFormatted(valueLong, formatString, isKeyName); break;
                case ulong valueULong:         stb.AppendFormatted(valueULong, formatString, isKeyName); break;
                case double valueDouble:       stb.AppendFormatted(valueDouble, formatString, isKeyName); break;
                case decimal valueDecimal:     stb.AppendFormatted(valueDecimal, formatString, isKeyName); break;
                case Int128 veryLongInt:       stb.AppendFormatted(veryLongInt, formatString, isKeyName); break;
                case UInt128 veryLongUInt:     stb.AppendFormatted(veryLongUInt, formatString, isKeyName); break;
                case BigInteger veryLongInt:   stb.AppendFormatted(veryLongInt, formatString, isKeyName); break;
                case Complex veryLongInt:      stb.AppendFormatted(veryLongInt, formatString, isKeyName); break;
                case DateTime valueDateTime:   stb.AppendFormatted(valueDateTime, formatString, isKeyName); break;
                case DateOnly valueDateOnly:   stb.AppendFormatted(valueDateOnly, formatString, isKeyName); break;
                case TimeSpan valueTimeSpan:   stb.AppendFormatted(valueTimeSpan, formatString, isKeyName); break;
                case TimeOnly valueTimeOnly:   stb.AppendFormatted(valueTimeOnly, formatString, isKeyName); break;
                case Rune valueRune:           stb.AppendFormatted(valueRune, formatString, isKeyName); break;
                case Guid valueGuid:           stb.AppendFormatted(valueGuid, formatString, isKeyName); break;
                case IPNetwork valueIpNetwork: stb.AppendFormatted(valueIpNetwork, formatString, isKeyName); break;
                case char[] valueCharArray:    stb.AppendFormattedOrNull(valueCharArray, formatString, isKeyName: isKeyName); break;
                case string valueString:       stb.AppendFormattedOrNull(valueString, formatString, isKeyName: isKeyName); break;
                case Version valueVersion:     stb.AppendFormatted(valueVersion, formatString, isKeyName); break;
                case IPAddress valueIpAddress: stb.AppendFormatted(valueIpAddress, formatString, isKeyName); break;
                case Uri valueUri:             stb.AppendFormatted(valueUri, formatString, isKeyName); break;
                case Enum:
                case ISpanFormattable:
                    var actualValueType = value.GetType();
                    var typeOfTValue    = typeof(TValue);
                    var delegateKey     = (typeOfTValue, actualValueType);
                    // ReSharper disable once InconsistentlySynchronizedField
                    if(!DynamicSpanFmtContentInvokers.TryGetValue(delegateKey, out var invoker))
                    {
                        lock (DynamicSpanFmtContentInvokers)
                        {
                            if (!DynamicSpanFmtContentInvokers.TryGetValue(delegateKey, out invoker))
                            {
                                if (typeOfTValue.ImplementsInterface(typeof(ISpanFormattable)))
                                {
                                    invoker = CreateSpanFormattableContentInvoker<TValue>();
                                }
                                else if (value is Enum)
                                {
                                    invoker = CreateSpanFormattableContentInvoker<Enum>();
                                }
                                else 
                                {
                                    invoker = CreateSpanFormattableContentInvoker<ISpanFormattable>();
                                }
                                DynamicSpanFmtContentInvokers.TryAdd(delegateKey, invoker);
                            }
                        }
                    }
                    if (typeOfTValue.ImplementsInterface(typeof(ISpanFormattable)))
                    {
                        var castInvoker = (SpanFmtStructContentHandler<TValue>) invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, value, formatString, isKeyName);
                    }
                    else if (value is Enum valueEnum)
                    {
                        var castInvoker = (SpanFmtStructContentHandler<Enum>) invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, valueEnum, formatString, isKeyName);
                    }
                    else
                    {
                        var castInvoker = (SpanFmtStructContentHandler<ISpanFormattable>)invoker;
                        castInvoker(stb.Sb, stb.StyleFormatter, (ISpanFormattable)value, formatString, isKeyName);
                    }
                    break;

                case ICharSequence valueCharSequence: stb.AppendFormattedOrNull(valueCharSequence, formatString, isKeyName: isKeyName); break;
                case StringBuilder valueSb:           stb.AppendFormattedOrNull(valueSb, formatString, isKeyName: isKeyName); break;

                case IStringBearer styledToStringObj: stb.AppendRevealBearerOrNull(styledToStringObj, isKeyName); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.Master.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = stb.Master.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString);
                    orderedCollectionBuilder.Complete();
                    break;

                default:
                    var unknownType = value.GetType();
                    if (isKeyName)
                        stb.StyleFormatter.FormatFieldNameMatch(stb.Sb, value, formatString);
                    else
                    {
                        if (unknownType.IsValueType)
                            stb.StyleFormatter.FormatFieldContentsMatch(stb.Sb, value, formatString);
                        else
                            stb.Master.RegisterVisitedInstanceAndConvert(value, isKeyName, formatString);
                    }
                    break;
            }
        else
            sb.Append(Null);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> FieldNameJoin<TExt>(this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> fieldName)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AppendFieldName(stb.Sb, fieldName);
        stb.StyleFormatter.AppendFieldValueSeparator(stb.Sb);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> FieldEnd<TExt>(this ITypeMolderDieCast<TExt> stb)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AppendFieldValueSeparator(stb.Sb);
        return stb;
    }
    
    public static void GoToNextCollectionItemStart<TExt>(this ITypeMolderDieCast<TExt> stb, Type elementType, int elementAt)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AddCollectionElementSeparator(stb.Sb, elementType, elementAt + 1);
    }

    public static void EndCollection<TExt>(this ITypeMolderDieCast<TExt> stb, Type elementType, int numberOfElements)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, numberOfElements);
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt>
    (this ITypeMolderDieCast<TExt> stb, bool value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder  =>
        stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt>
    (this ITypeMolderDieCast<TExt> stb, bool? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder  =>
        stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> stb, TFmt? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder where TFmt : ISpanFormattable =>
        stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt, TFmtStruct>
    (this ITypeMolderDieCast<TExt> stb, TFmtStruct? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder where TFmtStruct : struct, ISpanFormattable =>
        stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToCompAccess(stb);
    
    public static IStringBuilder DynamicReceiveAppendFormattedCollectionItem<TFmt>(IStringBuilder sb, IStyledTypeFormatting stf, TFmt? value
      , int retrieveCount, string? formatString)
        where TFmt : ISpanFormattable
    {
        stf.CollectionNextItemFormat(value, retrieveCount, sb, formatString ?? "");
        return sb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, string? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, char[]? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, ICharSequence? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, StringBuilder? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder =>
        stb.StyleFormatter.CollectionNextItemFormat(stb.Sb, value, retrieveCount, formatString).AnyToCompAccess(stb);

    public static void StartDictionary<TExt, TDict>(this ITypeMolderDieCast<TExt> stb, TDict keyValueInstances)
        where TExt : TypeMolder where TDict : notnull
    {
        stb.StyleFormatter.AppendComplexTypeOpening(stb.Sb, keyValueInstances.GetType());
    }

    public static void EndDictionary<TExt>(this ITypeMolderDieCast<TExt> stb)
        where TExt : TypeMolder
    {
        stb.Sb.RemoveLastWhiteSpacedCommaIfFound();
        stb.StyleFormatter.AppendTypeClosing(stb.Sb);
    }
    
    private delegate IStringBuilder SpanFmtStructContentHandler<in TFmt>(IStringBuilder sb, IStyledTypeFormatting stf, TFmt fmt, string? formatString
      , bool isFieldName);
    
    // Invokes DynamicReceiveAppendValue without boxing to ISpanFormattable if the type receive already supports ISpanFormattable
    private static SpanFmtStructContentHandler<T> CreateSpanFormattableContentInvoker<T>() 
    {
        var genTypeDefMeth = typeof(StyledTypeBuilderExtensions)
            .GetMethods().First(mi => mi.Name.Contains("DynamicReceiveAppendValue"));
        
        var generified = genTypeDefMeth.MakeGenericMethod(typeof(T));
        
        return BuildContentInvoker<T>(generified);
    }
    
    // Invokes DynamicReceiveAppendValue without boxing to ISpanFormattable if the type receive already supports ISpanFormattable
    private static SpanFmtStructContentHandler<T> BuildContentInvoker<T>(MethodInfo methodInfo) 
    {
        
        var helperMethod =
            new DynamicMethod($"{methodInfo.Name}_DynamicStructAppend", typeof(IStringBuilder),
                              [typeof(IStringBuilder), typeof(IStyledTypeFormatting), typeof(T), typeof(string), typeof(bool)]
                            , typeof(StyledTypeBuilderExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(SpanFmtStructContentHandler<T>));
        return (SpanFmtStructContentHandler<T>)methodInvoker;
    }
    
    private delegate IStringBuilder SpanFmtStructCollectionElementHandler<in TFmt>(IStringBuilder sb, IStyledTypeFormatting stf
      , TFmt fmt, int retrievalCount, string? formatString);
    
    // Invokes DynamicReceiveAppendFormattedCollectionItem without boxing to ISpanFormattable if the type receive already supports ISpanFormattable
    private static SpanFmtStructCollectionElementHandler<T> CreateSpanFormattableCollectionElementInvoker<T>()
    {
        var genTypeDefMeth = typeof(StyledTypeBuilderExtensions)
                             .GetMethods().First(mi => mi.Name.Contains("DynamicReceiveAppendFormattedCollectionItem"));
        
        var generified = genTypeDefMeth.MakeGenericMethod(typeof(T));
        
        return BuildCollectionInvoker<T>( generified);
    }
    
    // Invokes DynamicReceiveAppendFormattedCollectionItem without boxing to ISpanFormattable if the type receive already supports ISpanFormattable
    private static SpanFmtStructCollectionElementHandler<TFmt> BuildCollectionInvoker<TFmt>(MethodInfo methodInfo)
    {
        var helperMethod =
            new DynamicMethod($"{methodInfo.Name}_DynamicStructAppend", typeof(IStringBuilder),
                              [typeof(IStringBuilder), typeof(IStyledTypeFormatting), typeof(TFmt), typeof(int), typeof(string)]
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
