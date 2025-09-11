using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.Options;
using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public record struct StyledTypeBuildResult(string TypeName, StyledTypeStringAppender TypeStringAppender, Range AppendRange)
{
    public static implicit operator StyledTypeBuildResult(StyledTypeBuilder stb) => stb.Complete();

    public static implicit operator StyledTypeStringAppender(StyledTypeBuildResult stbr) => stbr.TypeStringAppender;

    public static readonly StyledTypeBuildResult EmptyAppend =
        new("Empty", null!, new Range(Index.FromStart(0), Index.FromStart(0)));
}

public abstract class StyledTypeBuilder : ExplicitRecyclableObject, IDisposable
{
    protected readonly StyleTypeBuilderPortableState PortableState = new();

    protected int StartIndex;

    protected void InitializeStyledTypeBuilder(
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings
      , string typeName
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        PortableState.TypeBeingBuilt   = typeBeingBuilt;
        PortableState.OwningAppender   = owningAppender;
        PortableState.TypeName         = typeName;
        PortableState.TypeFormatting    = typeFormatting;
        PortableState.AppenderSettings = typeSettings;
        PortableState.CompleteResult   = null;
        PortableState.ExistingRefId    = existingRefId;

        StartIndex = owningAppender.WriteBuffer.Length;
    }

    public bool IsComplete => PortableState.CompleteResult != null;

    public int ExistingRefId => PortableState.ExistingRefId;

    public abstract void Start();

    public Type TypeBeingBuilt => PortableState.TypeBeingBuilt;

    public StyleOptions Settings => PortableState.OwningAppender.Settings;
    
    public string TypeName => PortableState.TypeName;

    public abstract StyledTypeBuildResult Complete();

    public StyleOptions StyleSettings => PortableState.OwningAppender.Settings;

    public void Dispose()
    {
        if (!IsComplete)
        {
            PortableState.CompleteResult = Complete();
        }
    }

    protected override void InheritedStateReset()
    {
        PortableState.OwningAppender   = null!;
        PortableState.TypeName         = null!;
        PortableState.AppenderSettings = default;
        PortableState.CompleteResult   = null;
        PortableState.ExistingRefId    = 0;

        StartIndex = -1;

        MeRecyclable.StateReset();
    }

    public class StyleTypeBuilderPortableState
    {
        public TypeAppendSettings AppenderSettings;

        public Type TypeBeingBuilt { get; set; } = null!;
        public string TypeName { get; set; } = null!;

        public IStyledTypeFormatting TypeFormatting { get; set; } = null!;
        public int ExistingRefId { get; set; }

        public IStyleTypeAppenderBuilderAccess OwningAppender { get; set; } = null!;

        public StyledTypeBuildResult? CompleteResult;

        public bool IsComplete => CompleteResult != null;
    }
}

public interface ITypeBuilderComponentSource
{
    IStyleTypeBuilderComponentAccess ComponentAccess { get; }
}

public interface ITypeBuilderComponentSource<out T> : ITypeBuilderComponentSource where T : StyledTypeBuilder
{
    IStyleTypeBuilderComponentAccess<T> CompAccess { get; }
}

public static class StyledTypeBuilderExtensions
{
    internal const string Null = "null";
    internal const string NoFormattingFormatString = "{0}";

    public static IStringBuilder AddIndents(this IStringBuilder sb, char indentChar, int settingsIndentSize, int indentLevel)
    {
        sb.Append(indentChar, indentLevel*settingsIndentSize);
        return sb;
    }

    public static TExt Next<TExt>(this IStringBuilder sb, IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder =>
        stb.StyleTypeBuilder;

    public static TExt AddGoToNext<TExt>(this IStringBuilder sb, IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty())
        {
            sb.Append(",\n");
            stb.IncrementIndent();
        }
        else
        {
            sb.Append(stb.Style.IsCompact() ? "," : ", ");
        }
        return stb.StyleTypeBuilder;
    }

    public static TExt AddGoToNext<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty())
        {
            stb.Sb.Append(",\n");
            stb.IncrementIndent();
        }
        else
        {
            stb.Sb.Append(stb.Style.IsCompact() ? "," : ", ");
        }
        return stb.StyleTypeBuilder;
    }

    public static IStringBuilder AppendOrNull<T>(this IStringBuilder sb, T? value, bool inQuotes = false) where T : ISpanFormattable =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, bool? value)
        where TExt : StyledTypeBuilder => value != null ? stb.Sb.Append(value) : stb.Sb.Append(Null);

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, bool value)
        where TExt : StyledTypeBuilder => stb.Sb.Append(value);

    public static IStringBuilder Append<T>(this IStringBuilder sb, T value, bool inQuotes = false) where T : struct, ISpanFormattable =>
        sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder AppendFormattedCollectionItem<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder where T : ISpanFormattable =>
         stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToSb(stb.Sb);

    public static IStringBuilder AppendFormattedCollectionItem<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder where T : struct, ISpanFormattable =>
         stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToSb(stb.Sb);

    public static IStringBuilder AppendFormattedCollectionItemOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, string? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder =>
        value != null 
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToSb(stb.Sb)
            : stb.Sb.Append(stb.Settings.NullStyle);

    public static IStringBuilder AppendFormattedCollectionItemOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, ICharSequence? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder =>
        value != null 
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToSb(stb.Sb)
            : stb.Sb.Append(stb.Settings.NullStyle);

    public static IStringBuilder AppendFormattedCollectionItemOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, StringBuilder? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder =>
        value != null 
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToSb(stb.Sb)
            : stb.Sb.Append(stb.Settings.NullStyle);

    public static IStringBuilder AppendCollectionItem<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T value, int retrieveCount) where TExt : StyledTypeBuilder =>
        stb.StyleFormatter.CollectionNextItem(value, retrieveCount, stb.Sb).AnyToSb(stb.Sb);

    public static IStringBuilder AppendCollectionItemOrNull<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T? value, int retrieveCount) where TExt : StyledTypeBuilder =>
        value != null 
            ? stb.StyleFormatter.CollectionNextItem(value, retrieveCount, stb.Sb).AnyToSb(stb.Sb)
            : stb.Sb.Append(stb.Settings.NullStyle);

    public static IStringBuilder                         AnyToSb<T>(this T _, IStringBuilder sbToReturn)                    => sbToReturn;
    
    public static TExt AnyToTypeBuilder<TExt, T>(this T _, IStyleTypeBuilderComponentAccess<TExt> typeBuilder)
        where TExt : StyledTypeBuilder  => typeBuilder.StyleTypeBuilder;

    public static IStringBuilder AppendFormatted<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder  where T : ISpanFormattable
    {
        var sb        = stb.Sb;
        stb.StyleFormatter.Format(value, sb, formatString);
        return sb;
    }

    public static IStringBuilder AppendValue<TExt, T>(this IStyleTypeBuilderComponentAccess<TExt> stb, T value)
        where TExt : StyledTypeBuilder  where T : ISpanFormattable
    {
        var sb        = stb.Sb;
        stb.StyleFormatter.Format(value, sb, NoFormattingFormatString);
        return sb;
    }

    public static IStringBuilder AppendNullableFormattedOrNull<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TExt : StyledTypeBuilder where T : struct, ISpanFormattable
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        stb.StyleFormatter.Format(value.Value, sb, formatString ?? NoFormattingFormatString);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt, T>(this IStyleTypeBuilderComponentAccess<TExt> stb, T? value)
        where TExt : StyledTypeBuilder where T : struct, ISpanFormattable
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        stb.StyleFormatter.Format(value.Value, sb, NoFormattingFormatString);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNullOnZeroLength<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value.Length == 0)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        stb.StyleFormatter.Format(value, cappedFrom, sb, formatString ?? NoFormattingFormatString, len);
        return sb;
    }

    public static IStringBuilder Qt(this IStringBuilder sb, bool writeQuote) => writeQuote ? sb.Append("\"") : sb;

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string? value)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        stb.StyleFormatter.Format(value, 0, sb, NoFormattingFormatString);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        stb.StyleFormatter.Format(value, cappedFrom, sb, formatString ?? NoFormattingFormatString, len);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, ReadOnlySpan<char> value)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        stb.StyleFormatter.Format(value, 0, sb, NoFormattingFormatString);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        stb.StyleFormatter.Format(value, cappedFrom, sb, formatString ?? NoFormattingFormatString, len);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, ICharSequence? value)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        stb.StyleFormatter.Format(value, 0, sb, NoFormattingFormatString);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, ICharSequence? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        stb.StyleFormatter.Format(value, cappedFrom, sb, formatString ?? NoFormattingFormatString, len);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, StringBuilder? value)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        stb.StyleFormatter.Format(value, 0, sb, NoFormattingFormatString);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len = cappedTo - cappedFrom;
        stb.StyleFormatter.Format(value, cappedFrom, sb, formatString ?? NoFormattingFormatString, len);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, char[]? value)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        stb.StyleFormatter.Format(value, 0, sb, NoFormattingFormatString);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, int fromIndex = 0, int length = int.MaxValue)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value == null)
        {
            return sb.Append(stb.Settings.NullStyle);
        }
        var cappedFrom = Math.Max(0, Math.Min(value.Length, fromIndex));
        var cappedTo   = Math.Min(length, (value.Length - cappedFrom));
        var len        = cappedTo - cappedFrom;
        stb.StyleFormatter.Format(value, cappedFrom, sb, formatString ?? NoFormattingFormatString, len);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder
    {
        if (value != null)
            stb.Sb.AppendFormat(stb.StyleFormatter, formatString, value);
        else
            stb.Sb.Append(Null);
        return stb.Sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , IStyledToStringObject? value) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value != null)
        {
            value.ToString(stb.OwningAppender);
        }
        else
        {
            sb.Append(Null);
        }
        return sb;
    }

    public static IStringBuilder AppendOrNull<TToStyle, TStylerType, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TToStyle? value
      , CustomTypeStyler<TStylerType> styledToStringAction) where TToStyle : TStylerType where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value != null)
        {
            styledToStringAction(value, stb.OwningAppender);
        }
        else
        {
            sb.Append(Null);
        }
        return stb.Sb;
    }

    public static IStringBuilder AppendFormattedCollectionItemMatchOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , TValue value, int retrieveCount, string formatString) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
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
                case IPAddress valueIntPtr:  stb.AppendFormattedCollectionItem(valueIntPtr,retrieveCount, formatString); break;
                case Uri valueUri:           stb.AppendFormattedCollectionItem(valueUri, retrieveCount, formatString); break;

                case IFrozenString valueFrozenString:   stb.AppendFormattedCollectionItemOrNull(valueFrozenString, retrieveCount, formatString); break;
                case IStringBuilder valueStringBuilder: stb.AppendFormattedCollectionItemOrNull(valueStringBuilder, retrieveCount, formatString); break;
                case StringBuilder valueSb:             stb.AppendFormattedCollectionItemOrNull(valueSb, retrieveCount, formatString); break;

                case IStyledToStringObject styledToStringObj: stb.AppendOrNull(styledToStringObj); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.OwningAppender.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = stb.OwningAppender.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString);
                    orderedCollectionBuilder.Complete();
                    break;

                default: stb.AppendCollectionItem(value, retrieveCount); break;
            }
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendMatchFormattedOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , TValue value, string formatString) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:         stb.AppendOrNull(valueBool); break;
                case byte valueByte:         stb.AppendFormatted(valueByte, formatString); break;
                case sbyte valueSByte:       stb.AppendFormatted(valueSByte, formatString); break;
                case char valueChar:         stb.AppendFormatted(valueChar, formatString); break;
                case short valueShort:       stb.AppendFormatted(valueShort, formatString); break;
                case ushort valueUShort:     stb.AppendFormatted(valueUShort, formatString); break;
                case Half valueHalfFloat:    stb.AppendFormatted(valueHalfFloat, formatString); break;
                case int valueInt:           stb.AppendFormatted(valueInt, formatString); break;
                case uint valueUInt:         stb.AppendFormatted(valueUInt, formatString); break;
                case nint valueUInt:         stb.AppendFormatted(valueUInt, formatString); break;
                case float valueFloat:       stb.AppendFormatted(valueFloat, formatString); break;
                case long valueLong:         stb.AppendFormatted(valueLong, formatString); break;
                case ulong valueULong:       stb.AppendFormatted(valueULong, formatString); break;
                case double valueDouble:     stb.AppendFormatted(valueDouble, formatString); break;
                case decimal valueDecimal:   stb.AppendFormatted(valueDecimal, formatString); break;
                case Int128 veryLongInt:     stb.AppendFormatted(veryLongInt, formatString); break;
                case UInt128 veryLongUInt:   stb.AppendFormatted(veryLongUInt, formatString); break;
                case BigInteger veryLongInt: stb.AppendFormatted(veryLongInt, formatString); break;
                case Complex veryLongInt:    stb.AppendFormatted(veryLongInt, formatString); break;
                case DateTime valueDateTime: stb.AppendFormatted(valueDateTime, formatString); break;
                case DateOnly valueDateOnly: stb.AppendFormatted(valueDateOnly, formatString); break;
                case TimeSpan valueTimeSpan: stb.AppendFormatted(valueTimeSpan, formatString); break;
                case TimeOnly valueTimeSpan: stb.AppendFormatted(valueTimeSpan, formatString); break;
                case Rune valueTimeSpan:     stb.AppendFormatted(valueTimeSpan, formatString); break;
                case Guid valueGuid:         stb.AppendFormatted(valueGuid, formatString); break;
                case IPNetwork valueIntPtr:  stb.AppendFormatted(valueIntPtr, formatString); break;
                case char[] valueCharArray:  stb.AppendFormattedOrNull(valueCharArray, formatString); break;
                case string valueString:     stb.AppendFormattedOrNull(valueString, formatString); break;
                case Enum valueEnum:         stb.AppendFormatted(valueEnum, formatString); break;
                case Version valueGuid:      stb.AppendFormatted(valueGuid, formatString); break;
                case IPAddress valueIntPtr:  stb.AppendFormatted(valueIntPtr, formatString); break;
                case Uri valueUri:           stb.AppendFormatted(valueUri, formatString); break;

                case ICharSequence valueCharSequence:   stb.AppendFormattedOrNull(valueCharSequence, formatString); break;
                case StringBuilder valueSb:             stb.AppendFormattedOrNull(valueSb, formatString); break;

                case IStyledToStringObject styledToStringObj: stb.AppendOrNull(styledToStringObj); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.OwningAppender.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = stb.OwningAppender.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString);
                    orderedCollectionBuilder.Complete();
                    break;

                default: sb.AppendFormat(stb.StyleFormatter, formatString, value); break;
            }
        else
            sb.Append(Null);
        return sb;
    }


    public static TExt AppendCollectionItemMatchOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TValue value, int retrieveCount)
        where TExt : StyledTypeBuilder
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

                case IStyledToStringObject styledToStringObject: stb.AppendOrNull(styledToStringObject); break;
                case IEnumerator:
                case IEnumerable:
                    var type = value.GetType();
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.OwningAppender.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll(keyedCollectionBuilder, value);
                        keyedCollectionBuilder.Complete();
                        break;
                    }

                    var orderedCollectionBuilder = stb.OwningAppender.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll(orderedCollectionBuilder, value);
                    orderedCollectionBuilder.Complete();
                    break;

                default: stb.AppendCollectionItem(value, retrieveCount); break;
            }
        else
            stb.Sb.Append(Null);
        return stb.StyleTypeBuilder;
    }

    public static IStringBuilder AppendMatchOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TValue value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb = stb.Sb;
        sb.AppendMatchOrNull(value, stb, inQuotes);
        return sb;
    }

    public static TExt AppendMatchOrNull<TValue, TExt>(this IStringBuilder sb, TValue value
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        if (value != null)
            switch (value)
            {
                case bool valueBool:         stb.AppendOrNull(valueBool); break;
                case byte valueByte:         stb.AppendValue(valueByte); break;
                case sbyte valueSByte:       stb.AppendValue(valueSByte); break;
                case char valueChar:         stb.AppendValue(valueChar); break;
                case short valueShort:       stb.AppendValue(valueShort); break;
                case ushort valueUShort:     stb.AppendValue(valueUShort); break;
                case Half valueHalfFloat:    stb.AppendValue(valueHalfFloat); break;
                case int valueInt:           stb.AppendValue(valueInt); break;
                case uint valueUInt:         stb.AppendValue(valueUInt); break;
                case nint valueUInt:         stb.AppendValue(valueUInt); break;
                case float valueFloat:       stb.AppendValue(valueFloat); break;
                case long valueLong:         stb.AppendValue(valueLong); break;
                case ulong valueULong:       stb.AppendValue(valueULong); break;
                case double valueDouble:     stb.AppendValue(valueDouble); break;
                case decimal valueDecimal:   stb.AppendValue(valueDecimal); break;
                case Int128 veryLongInt:     stb.AppendValue(veryLongInt); break;
                case UInt128 veryLongUInt:   stb.AppendValue(veryLongUInt); break;
                case BigInteger veryLongInt: stb.AppendValue(veryLongInt); break;
                case Complex veryLongInt:    stb.AppendValue(veryLongInt); break;
                case DateTime valueDateTime: stb.AppendValue(valueDateTime); break;
                case DateOnly valueDateOnly: stb.AppendValue(valueDateOnly); break;
                case TimeSpan valueTimeSpan: stb.AppendValue(valueTimeSpan); break;
                case TimeOnly valueTimeSpan: stb.AppendValue(valueTimeSpan); break;
                case Rune valueTimeSpan:     stb.AppendValue(valueTimeSpan); break;
                case Guid valueGuid:         stb.AppendValue(valueGuid); break;
                case IPNetwork valueIntPtr:  stb.AppendValue(valueIntPtr); break;
                case char[] valueCharArray:  stb.AppendOrNull(valueCharArray); break;
                case string valueString:     stb.AppendOrNull(valueString); break;
                case Enum valueEnum:         stb.AppendFormatted(valueEnum, NoFormattingFormatString); break;
                case Version valueGuid:      stb.AppendFormatted(valueGuid, NoFormattingFormatString); break;
                case IPAddress valueIntPtr:  stb.AppendFormatted(valueIntPtr, NoFormattingFormatString); break;
                case Uri valueUri:           stb.AppendFormatted(valueUri, NoFormattingFormatString); break;

                case ICharSequence charSequence:  stb.AppendOrNull(charSequence); break;
                case StringBuilder stringBuilder:  stb.AppendOrNull(stringBuilder); break;

                case IStyledToStringObject styledToStringObject: stb.AppendOrNull(styledToStringObject); break;
                case IEnumerator:
                case IEnumerable:
                    var type = value.GetType();
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.OwningAppender.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll(keyedCollectionBuilder, value);
                        keyedCollectionBuilder.Complete();
                        break;
                    }

                    var orderedCollectionBuilder = stb.OwningAppender.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll(orderedCollectionBuilder, value);
                    orderedCollectionBuilder.Complete();
                    break;

                default: sb.AppendFormat(stb.StyleFormatter, NoFormattingFormatString, value); break;
            }
        else
            sb.Append(Null);
        return stb.StyleTypeBuilder;
    }

    public static void StartCollection<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, Type elementType, bool hasElements)
        where TExt : StyledTypeBuilder
    {
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, hasElements);
    }

    public static IStyleTypeBuilderComponentAccess<TExt> FieldNameJoin<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string fieldName)
        where TExt : StyledTypeBuilder
    {
        stb.StyleFormatter.AppendFieldName(stb, fieldName);
        stb.StyleFormatter.AppendFieldValueSeparator(stb, fieldName); // field name added in case it variaries in the future 
        return stb;
    }

    public static IStyleTypeBuilderComponentAccess<TExt> FieldNameJoin<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string fieldName
      , IStyleTypeBuilderComponentAccess<TExt> toReturn)
        where TExt : StyledTypeBuilder
    {
        stb.StyleFormatter.AppendFieldName(stb, fieldName);
        stb.StyleFormatter.AppendFieldValueSeparator(stb, fieldName); // field name added in case it variaries in the future 
        return toReturn;
    }

    public static IStringBuilder FieldEnd(this IStringBuilder sb, IStyleTypeBuilderComponentAccess stb) =>
        sb.Append(stb.Style.IsCompact() ? ":" : ": ");

    public static void GoToNextCollectionItemStart<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, Type elementType, int elementAt)
        where TExt : StyledTypeBuilder
    {
        stb.StyleFormatter.AddCollectionElementSeparator(stb, elementType,  elementAt + 1);
    }

    public static IStringBuilder ItemNext(this IStringBuilder sb, IStyleTypeBuilderComponentAccess stb) =>
        sb.Append(stb.Style.IsCompact() ? "," : ", ");


    public static void EndCollection<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, Type elementType, int numberOfElements)
        where TExt : StyledTypeBuilder
    {
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, numberOfElements);
    }

    public static IStringBuilder RemoveLastWhiteSpacedCommaIfFound<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        if (stb.Sb[^1] == ',')
        {
            stb.Sb.Length -= 1;
            return stb.Sb;
        }
        if (stb.Sb[^2] == ',' && stb.Sb[^1] == ' ')
        {
            stb.Sb.Length -= 2;
            if (stb.Style.IsPretty()) stb.Sb.Append(" ");
            return stb.Sb;
        }
        for (var i = stb.Sb.Length - 1; i > 0 && stb.Sb[i] is ' ' or '\r' or '\n' or ','; i--)
            if (stb.Sb[i] == ',')
            {
                stb.Sb.Remove(i, 1);
                break;
            }
        return stb.Sb;
    }


    public static void StartDictionary<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        stb.Sb.Append("{");
        if (stb.Style.IsPretty())
        {
            stb.IncrementIndent();
            stb.Sb.Append(stb.Settings.NewLineStyle).AddIndents(stb.Settings.IndentChar, stb.Settings.IndentSize, stb.IndentLevel);
        }
    }

    public static void EndDictionary<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        stb.RemoveLastWhiteSpacedCommaIfFound();
        stb.Sb.Append("}");
    }
}
