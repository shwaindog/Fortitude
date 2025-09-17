using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
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

    public static TExt AddGoToNext<TExt>(this ITypeMolderDieCast<TExt> stb)
        where TExt : TypeMolder
    {
        return stb.StyleFormatter.AddNextFieldSeparator(stb).ToTypeBuilder(stb);
    }


    public static TExt ToTypeBuilder<TExt, T>(this T _, ITypeMolderDieCast<TExt> typeBuilder)
        where TExt : TypeMolder =>
        typeBuilder.StyleTypeBuilder;


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

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, bool? value
      , bool isKeyName = false)
        where TExt : TypeMolder =>
        value != null ? stb.Sb.Append(value).AnyToCompAccess(stb) : stb.Sb.Append(Null).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt>(this ITypeMolderDieCast<TExt> stb, bool value, bool isKeyName = false)
        where TExt : TypeMolder =>
        stb.Sb.Append(value).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormatted<TExt, TFmt>
    (this ITypeMolderDieCast<TExt> stb, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool isKeyName = false)
        where TExt : TypeMolder where TFmt : ISpanFormattable
    {
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb, value, formatString);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value, formatString);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendValue<TExt, TFmt>(this ITypeMolderDieCast<TExt> stb, TFmt value
      , bool isKeyName = false)
        where TExt : TypeMolder where TFmt : ISpanFormattable
    {
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value);
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
            stb.StyleFormatter.FormatFieldName(stb, value, formatString);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value, formatString);
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
            stb.StyleFormatter.FormatFieldName(stb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNullOnZeroLength<TExt>
    (this ITypeMolderDieCast<TExt> stb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue, bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value.Length == 0)
        {
            sb.Append(stb.Settings.NullStyle);
            return stb;
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb, value, cappedFrom, formatString, len);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value, cappedFrom, formatString, len);
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
            stb.StyleFormatter.FormatFieldName(stb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value);
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
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb, value, cappedFrom, formatString, len);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value, cappedFrom, formatString, len);
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
            stb.StyleFormatter.FormatFieldName(stb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value);
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
            stb.StyleFormatter.FormatFieldName(stb, value, cappedFrom, formatString, len);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value, cappedFrom, formatString, len);
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
            stb.StyleFormatter.FormatFieldName(stb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value);
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
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb, value, cappedFrom, formatString, len);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value, cappedFrom, formatString, len);
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
            stb.StyleFormatter.FormatFieldName(stb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value);
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
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb, value, cappedFrom, formatString, len);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value, cappedFrom, formatString, len);
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
            stb.StyleFormatter.FormatFieldName(stb, value);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value);
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
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        if (isKeyName)
            stb.StyleFormatter.FormatFieldName(stb, value, cappedFrom, formatString, len);
        else
            stb.StyleFormatter.FormatFieldContents(stb, value, cappedFrom, formatString, len);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool isKeyName = false)
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

    public static ITypeMolderDieCast<TExt> AppendOrNull<TExt>(this ITypeMolderDieCast<TExt> stb
      , IStringBearer? value, bool isKeyName = false) where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb, value);
            else
                stb.StyleFormatter.FormatFieldContents(stb, value);
        }
        else
        {
            sb.Append(Null);
        }
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendOrNull<TToStyle, TStylerType, TExt>(this ITypeMolderDieCast<TExt> stb
      , TToStyle? toStyle
      , StringBearerRevealState<TStylerType> styler, bool isKeyName = false) where TToStyle : TStylerType where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (toStyle != null)
        {
            if (isKeyName)
                stb.StyleFormatter.FormatFieldName(stb, toStyle, styler);
            else
                stb.StyleFormatter.FormatFieldContents(stb, toStyle, styler);

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
      , TValue value, int retrieveCount, string formatString, bool isKeyName = false) where TExt : TypeMolder
    {
        var sb = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:         stb.AppendCollectionItem(valueBool, retrieveCount); break;
                case byte valueByte:         stb.AppendFormattedCollectionItem(valueByte, retrieveCount, formatString); break;
                case sbyte valueSByte:       stb.AppendFormattedCollectionItem(valueSByte, retrieveCount, formatString); break;
                case char valueChar:         stb.AppendFormattedCollectionItem(valueChar, retrieveCount, formatString); break;
                case short valueShort:       stb.AppendFormattedCollectionItem(valueShort, retrieveCount, formatString); break;
                case ushort valueUShort:     stb.AppendFormattedCollectionItem(valueUShort, retrieveCount, formatString); break;
                case Half valueHalfFloat:    stb.AppendFormattedCollectionItem(valueHalfFloat, retrieveCount, formatString); break;
                case int valueInt:           stb.AppendFormattedCollectionItem(valueInt, retrieveCount, formatString); break;
                case uint valueUInt:         stb.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString); break;
                case nint valueUInt:         stb.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString); break;
                case float valueFloat:       stb.AppendFormattedCollectionItem(valueFloat, retrieveCount, formatString); break;
                case long valueLong:         stb.AppendFormattedCollectionItem(valueLong, retrieveCount, formatString); break;
                case ulong valueULong:       stb.AppendFormattedCollectionItem(valueULong, retrieveCount, formatString); break;
                case double valueDouble:     stb.AppendFormattedCollectionItem(valueDouble, retrieveCount, formatString); break;
                case decimal valueDecimal:   stb.AppendFormattedCollectionItem(valueDecimal, retrieveCount, formatString); break;
                case Int128 veryLongInt:     stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case UInt128 veryLongUInt:   stb.AppendFormattedCollectionItem(veryLongUInt, retrieveCount, formatString); break;
                case BigInteger veryLongInt: stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case Complex veryLongInt:    stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case DateTime valueDateTime: stb.AppendFormattedCollectionItem(valueDateTime, retrieveCount, formatString); break;
                case DateOnly valueDateOnly: stb.AppendFormattedCollectionItem(valueDateOnly, retrieveCount, formatString); break;
                case TimeSpan valueTimeSpan: stb.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString); break;
                case TimeOnly valueTimeSpan: stb.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString); break;
                case Rune valueTimeSpan:     stb.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString); break;
                case Guid valueGuid:         stb.AppendFormattedCollectionItem(valueGuid, retrieveCount, formatString); break;
                case IPNetwork valueIntPtr:  stb.AppendFormattedCollectionItem(valueIntPtr, retrieveCount, formatString); break;
                case char[] valueCharArray:  stb.AppendCollectionItem(valueCharArray, retrieveCount); break;
                case string valueString:     stb.AppendFormattedCollectionItemOrNull(valueString, retrieveCount, formatString); break;
                case Enum valueEnum:         stb.AppendFormattedCollectionItem(valueEnum, retrieveCount, formatString); break;
                case Version valueGuid:      stb.AppendFormattedCollectionItem(valueGuid, retrieveCount, formatString); break;
                case IPAddress valueIntPtr:  stb.AppendFormattedCollectionItem(valueIntPtr, retrieveCount, formatString); break;
                case Uri valueUri:           stb.AppendFormattedCollectionItem(valueUri, retrieveCount, formatString); break;

                case IFrozenString valueFrozenString:   stb.AppendFormattedCollectionItemOrNull(valueFrozenString, retrieveCount, formatString); break;
                case IStringBuilder valueStringBuilder: stb.AppendFormattedCollectionItemOrNull(valueStringBuilder, retrieveCount, formatString); break;
                case StringBuilder valueSb:             stb.AppendFormattedCollectionItemOrNull(valueSb, retrieveCount, formatString); break;

                case IStringBearer styledToStringObj: stb.AppendOrNull(styledToStringObj); break;
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

                default: stb.AppendCollectionItem(value, retrieveCount); break;
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
                case bool valueBool:         stb.AppendOrNull(valueBool, isKeyName); break;
                case byte valueByte:         stb.AppendFormatted(valueByte, formatString, isKeyName); break;
                case sbyte valueSByte:       stb.AppendFormatted(valueSByte, formatString, isKeyName); break;
                case char valueChar:         stb.AppendFormatted(valueChar, formatString, isKeyName); break;
                case short valueShort:       stb.AppendFormatted(valueShort, formatString, isKeyName); break;
                case ushort valueUShort:     stb.AppendFormatted(valueUShort, formatString, isKeyName); break;
                case Half valueHalfFloat:    stb.AppendFormatted(valueHalfFloat, formatString, isKeyName); break;
                case int valueInt:           stb.AppendFormatted(valueInt, formatString, isKeyName); break;
                case uint valueUInt:         stb.AppendFormatted(valueUInt, formatString, isKeyName); break;
                case nint valueUInt:         stb.AppendFormatted(valueUInt, formatString, isKeyName); break;
                case float valueFloat:       stb.AppendFormatted(valueFloat, formatString, isKeyName); break;
                case long valueLong:         stb.AppendFormatted(valueLong, formatString, isKeyName); break;
                case ulong valueULong:       stb.AppendFormatted(valueULong, formatString, isKeyName); break;
                case double valueDouble:     stb.AppendFormatted(valueDouble, formatString, isKeyName); break;
                case decimal valueDecimal:   stb.AppendFormatted(valueDecimal, formatString, isKeyName); break;
                case Int128 veryLongInt:     stb.AppendFormatted(veryLongInt, formatString, isKeyName); break;
                case UInt128 veryLongUInt:   stb.AppendFormatted(veryLongUInt, formatString, isKeyName); break;
                case BigInteger veryLongInt: stb.AppendFormatted(veryLongInt, formatString, isKeyName); break;
                case Complex veryLongInt:    stb.AppendFormatted(veryLongInt, formatString, isKeyName); break;
                case DateTime valueDateTime: stb.AppendFormatted(valueDateTime, formatString, isKeyName); break;
                case DateOnly valueDateOnly: stb.AppendFormatted(valueDateOnly, formatString, isKeyName); break;
                case TimeSpan valueTimeSpan: stb.AppendFormatted(valueTimeSpan, formatString, isKeyName); break;
                case TimeOnly valueTimeSpan: stb.AppendFormatted(valueTimeSpan, formatString, isKeyName); break;
                case Rune valueTimeSpan:     stb.AppendFormatted(valueTimeSpan, formatString, isKeyName); break;
                case Guid valueGuid:         stb.AppendFormatted(valueGuid, formatString, isKeyName); break;
                case IPNetwork valueIntPtr:  stb.AppendFormatted(valueIntPtr, formatString, isKeyName); break;
                case char[] valueCharArray:  stb.AppendFormattedOrNull(valueCharArray, formatString, isKeyName); break;
                case string valueString:     stb.AppendFormattedOrNull(valueString, formatString, isKeyName); break;
                case Enum valueEnum:         stb.AppendFormatted(valueEnum, formatString, isKeyName); break;
                case Version valueGuid:      stb.AppendFormatted(valueGuid, formatString, isKeyName); break;
                case IPAddress valueIntPtr:  stb.AppendFormatted(valueIntPtr, formatString, isKeyName); break;
                case Uri valueUri:           stb.AppendFormatted(valueUri, formatString, isKeyName); break;

                case ICharSequence valueCharSequence: stb.AppendFormattedOrNull(valueCharSequence, formatString); break;
                case StringBuilder valueSb:           stb.AppendFormattedOrNull(valueSb, formatString); break;

                case IStringBearer styledToStringObj: stb.AppendOrNull(styledToStringObj, isKeyName); break;
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


    public static ITypeMolderDieCast<TExt> AppendCollectionItemMatchOrNull<TValue, TExt>(
        this ITypeMolderDieCast<TExt> stb, TValue value, int retrieveCount)
        where TExt : TypeMolder
    {
        if (value != null)
            switch (value)
            {
                case bool valueBool:         stb.AppendCollectionItem(valueBool, retrieveCount); break;
                case byte valueByte:         stb.AppendCollectionItem(valueByte, retrieveCount); break;
                case sbyte valueSByte:       stb.AppendCollectionItem(valueSByte, retrieveCount); break;
                case char valueChar:         stb.AppendCollectionItem(valueChar, retrieveCount); break;
                case short valueShort:       stb.AppendCollectionItem(valueShort, retrieveCount); break;
                case ushort valueUShort:     stb.AppendCollectionItem(valueUShort, retrieveCount); break;
                case Half valueHalfFloat:    stb.AppendCollectionItem(valueHalfFloat, retrieveCount); break;
                case int valueInt:           stb.AppendCollectionItem(valueInt, retrieveCount); break;
                case uint valueUInt:         stb.AppendCollectionItem(valueUInt, retrieveCount); break;
                case nint valueUInt:         stb.AppendCollectionItem(valueUInt, retrieveCount); break;
                case float valueFloat:       stb.AppendCollectionItem(valueFloat, retrieveCount); break;
                case long valueLong:         stb.AppendCollectionItem(valueLong, retrieveCount); break;
                case ulong valueULong:       stb.AppendCollectionItem(valueULong, retrieveCount); break;
                case double valueDouble:     stb.AppendCollectionItem(valueDouble, retrieveCount); break;
                case decimal valueDecimal:   stb.AppendCollectionItem(valueDecimal, retrieveCount); break;
                case Int128 veryLongInt:     stb.AppendCollectionItem(veryLongInt, retrieveCount); break;
                case UInt128 veryLongUInt:   stb.AppendCollectionItem(veryLongUInt, retrieveCount); break;
                case BigInteger veryLongInt: stb.AppendCollectionItem(veryLongInt, retrieveCount); break;
                case Complex veryLongInt:    stb.AppendCollectionItem(veryLongInt, retrieveCount); break;
                case DateTime valueDateTime: stb.AppendCollectionItem(valueDateTime, retrieveCount); break;
                case DateOnly valueDateOnly: stb.AppendCollectionItem(valueDateOnly, retrieveCount); break;
                case TimeSpan valueTimeSpan: stb.AppendCollectionItem(valueTimeSpan, retrieveCount); break;
                case TimeOnly valueTimeSpan: stb.AppendCollectionItem(valueTimeSpan, retrieveCount); break;
                case Rune valueTimeSpan:     stb.AppendCollectionItem(valueTimeSpan, retrieveCount); break;
                case Guid valueGuid:         stb.AppendCollectionItem(valueGuid, retrieveCount); break;
                case IPNetwork valueIntPtr:  stb.AppendCollectionItem(valueIntPtr, retrieveCount); break;
                case char[] valueCharArray:  stb.AppendCollectionItem(valueCharArray, retrieveCount); break;
                case string valueString:     stb.AppendCollectionItem(valueString, retrieveCount); break;
                case Enum valueEnum:         stb.AppendCollectionItem(valueEnum, retrieveCount); break;
                case Version valueGuid:      stb.AppendCollectionItem(valueGuid, retrieveCount); break;
                case IPAddress valueIntPtr:  stb.AppendCollectionItem(valueIntPtr, retrieveCount); break;
                case Uri valueUri:           stb.AppendCollectionItem(valueUri, retrieveCount); break;

                case IFrozenString valueFrozenString:  stb.AppendCollectionItem(valueFrozenString, retrieveCount); break;
                case IStringBuilder valueFrozenString: stb.AppendCollectionItem(valueFrozenString, retrieveCount); break;
                case StringBuilder valueFrozenString:  stb.AppendCollectionItem(valueFrozenString, retrieveCount); break;

                case IStringBearer styledToStringObject: stb.AppendOrNull(styledToStringObject); break;
                case IEnumerator:
                case IEnumerable:
                    var type = value.GetType();
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.Master.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll(keyedCollectionBuilder, value);
                        keyedCollectionBuilder.Complete();
                        break;
                    }

                    var orderedCollectionBuilder = stb.Master.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll(orderedCollectionBuilder, value);
                    orderedCollectionBuilder.Complete();
                    break;

                default: stb.AppendCollectionItem(value, retrieveCount); break;
            }
        else
            stb.Sb.Append(Null);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendMatchOrNull<TValue, TExt>(this ITypeMolderDieCast<TExt> stb, TValue value
      , bool isKeyName = false)
        where TExt : TypeMolder
    {
        var sb = stb.Sb;
        return sb.AppendMatchOrNull(value, stb, isKeyName);
    }

    public static ITypeMolderDieCast<TExt> AppendMatchOrNull<TValue, TExt>(this IStringBuilder sb, TValue value
      , ITypeMolderDieCast<TExt> stb, bool isKeyName = false)
        where TExt : TypeMolder
    {
        if (value != null)
            switch (value)
            {
                case bool valueBool:         stb.AppendOrNull(valueBool, isKeyName); break;
                case byte valueByte:         stb.AppendValue(valueByte, isKeyName); break;
                case sbyte valueSByte:       stb.AppendValue(valueSByte, isKeyName); break;
                case char valueChar:         stb.AppendValue(valueChar, isKeyName); break;
                case short valueShort:       stb.AppendValue(valueShort, isKeyName); break;
                case ushort valueUShort:     stb.AppendValue(valueUShort, isKeyName); break;
                case Half valueHalfFloat:    stb.AppendValue(valueHalfFloat, isKeyName); break;
                case int valueInt:           stb.AppendValue(valueInt, isKeyName); break;
                case uint valueUInt:         stb.AppendValue(valueUInt, isKeyName); break;
                case nint valueUInt:         stb.AppendValue(valueUInt, isKeyName); break;
                case float valueFloat:       stb.AppendValue(valueFloat, isKeyName); break;
                case long valueLong:         stb.AppendValue(valueLong, isKeyName); break;
                case ulong valueULong:       stb.AppendValue(valueULong, isKeyName); break;
                case double valueDouble:     stb.AppendValue(valueDouble, isKeyName); break;
                case decimal valueDecimal:   stb.AppendValue(valueDecimal, isKeyName); break;
                case Int128 veryLongInt:     stb.AppendValue(veryLongInt, isKeyName); break;
                case UInt128 veryLongUInt:   stb.AppendValue(veryLongUInt, isKeyName); break;
                case BigInteger veryLongInt: stb.AppendValue(veryLongInt, isKeyName); break;
                case Complex veryLongInt:    stb.AppendValue(veryLongInt, isKeyName); break;
                case DateTime valueDateTime: stb.AppendValue(valueDateTime, isKeyName); break;
                case DateOnly valueDateOnly: stb.AppendValue(valueDateOnly, isKeyName); break;
                case TimeSpan valueTimeSpan: stb.AppendValue(valueTimeSpan, isKeyName); break;
                case TimeOnly valueTimeSpan: stb.AppendValue(valueTimeSpan, isKeyName); break;
                case Rune valueTimeSpan:     stb.AppendValue(valueTimeSpan, isKeyName); break;
                case Guid valueGuid:         stb.AppendValue(valueGuid, isKeyName); break;
                case IPNetwork valueIntPtr:  stb.AppendValue(valueIntPtr, isKeyName); break;
                case char[] valueCharArray:  stb.AppendOrNull(valueCharArray, isKeyName); break;
                case string valueString:     stb.AppendOrNull(valueString, isKeyName); break;
                case Enum valueEnum:         stb.AppendValue(valueEnum, isKeyName); break;
                case Version valueGuid:      stb.AppendValue(valueGuid, isKeyName); break;
                case IPAddress valueIntPtr:  stb.AppendValue(valueIntPtr, isKeyName); break;
                case Uri valueUri:           stb.AppendValue(valueUri, isKeyName); break;

                case ICharSequence charSequence:  stb.AppendOrNull(charSequence, isKeyName); break;
                case StringBuilder stringBuilder: stb.AppendOrNull(stringBuilder, isKeyName); break;

                case IStringBearer styledToStringObject: stb.AppendOrNull(styledToStringObject, isKeyName); break;
                case IEnumerator:
                case IEnumerable:
                    var type = value.GetType();
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.Master.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll(keyedCollectionBuilder, value);
                        keyedCollectionBuilder.Complete();
                        break;
                    }

                    var orderedCollectionBuilder = stb.Master.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll(orderedCollectionBuilder, value);
                    orderedCollectionBuilder.Complete();
                    break;

                default:
                    var unknownType = value.GetType();
                    if (isKeyName)
                        stb.StyleFormatter.FormatFieldNameMatch(stb.Sb, value);
                    else
                    {
                        if (unknownType.IsValueType)
                            stb.StyleFormatter.FormatFieldContentsMatch(stb.Sb, value);
                        else
                            stb.Master.RegisterVisitedInstanceAndConvert(value, isKeyName);
                    }
                    break;
            }
        else
            sb.Append(Null);
        return stb;
    }

    public static void StartCollection<TExt, T>(this ITypeMolderDieCast<TExt> stb, Type elementType, bool hasElements, T collectionInstance)
        where TExt : TypeMolder where T : notnull
    {
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, hasElements, collectionInstance.GetType());
    }

    public static ITypeMolderDieCast<TExt> FieldNameJoin<TExt>(this ITypeMolderDieCast<TExt> stb, string fieldName)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AppendFieldName(stb, fieldName);
        stb.StyleFormatter.AppendFieldValueSeparator(stb);
        return stb;
    }

    public static ITypeMolderDieCast<TExt> FieldEnd<TExt>(this ITypeMolderDieCast<TExt> stb)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AppendFieldValueSeparator(stb);
        return stb;
    }

    public static void GoToNextCollectionItemStart<TExt>(this ITypeMolderDieCast<TExt> stb, Type elementType, int elementAt)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.AddCollectionElementSeparator(stb, elementType, elementAt + 1);
    }

    public static void EndCollection<TExt>(this ITypeMolderDieCast<TExt> stb, Type elementType, int numberOfElements)
        where TExt : TypeMolder
    {
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, numberOfElements);
    }

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt, T>
    (this ITypeMolderDieCast<TExt> stb, T value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder where T : ISpanFormattable =>
        stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItem<TExt, T>
    (this ITypeMolderDieCast<TExt> stb, T? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder where T : struct, ISpanFormattable =>
        stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, string? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder =>
        value != null
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToCompAccess(stb)
            : stb.Sb.Append(stb.Settings.NullStyle).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, ICharSequence? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder =>
        value != null
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToCompAccess(stb)
            : stb.Sb.Append(stb.Settings.NullStyle).AnyToCompAccess(stb);

    public static ITypeMolderDieCast<TExt> AppendFormattedCollectionItemOrNull<TExt>
    (this ITypeMolderDieCast<TExt> stb, StringBuilder? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : TypeMolder
    {
        return value != null
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToCompAccess(stb)
            : stb.Sb.Append(stb.Settings.NullStyle).AnyToCompAccess(stb);
    }

    public static ITypeMolderDieCast<TExt> AppendCollectionItem<TExt, T>
        (this ITypeMolderDieCast<TExt> stb, T value, int retrieveCount) where TExt : TypeMolder
    {
        if (typeof(T).IsValueType || value == null || stb.Master.RegisterVisitedCheckCanContinue(value))
        {
            return stb.StyleFormatter.CollectionNextItem(value, retrieveCount, stb.Sb).AnyToCompAccess(stb);
        }
        return stb;
    }

    public static ITypeMolderDieCast<TExt> AppendCollectionItemOrNull<TExt, T>
        (this ITypeMolderDieCast<TExt> stb, T? value, int retrieveCount) where TExt : TypeMolder
    {
        if (typeof(T).IsValueType || value == null || stb.Master.RegisterVisitedCheckCanContinue(value))
        {
            return stb.StyleFormatter.CollectionNextItem(value, retrieveCount, stb.Sb).AnyToCompAccess(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AnyToCompAccess(stb);
    }

    public static void StartDictionary<TExt, TDict>(this ITypeMolderDieCast<TExt> stb, TDict keyValueInstances)
        where TExt : TypeMolder where TDict : notnull
    {
        stb.StyleFormatter.AppendComplexTypeOpening(stb, keyValueInstances.GetType());
    }

    public static void EndDictionary<TExt>(this ITypeMolderDieCast<TExt> stb)
        where TExt : TypeMolder
    {
        stb.RemoveLastWhiteSpacedCommaIfFound();
        stb.StyleFormatter.AppendTypeClosing(stb);
    }
}
