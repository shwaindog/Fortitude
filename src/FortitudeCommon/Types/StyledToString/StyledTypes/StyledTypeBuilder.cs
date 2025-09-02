using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
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
    protected StyleTypeBuilderPortableState PortableState = new();

    protected int StartIndex;

    protected void InitializeStyledTypeBuilder(IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName
      , int existingRefId)
    {
        PortableState.OwningAppender   = owningAppender;
        PortableState.TypeName         = typeName;
        PortableState.AppenderSettings = typeSettings;
        PortableState.CompleteResult   = null;
        PortableState.ExistingRefId    = existingRefId;

        StartIndex = owningAppender.WriteBuffer.Length;
    }

    public bool IsComplete => PortableState.CompleteResult != null;

    public int ExistingRefId => PortableState.ExistingRefId;

    public abstract void Start();

    public string TypeName => PortableState.TypeName;

    public abstract StyledTypeBuildResult Complete();

    public StringBuildingStyle Style => PortableState.OwningAppender.Style;

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

        public string TypeName { get; set; } = null!;

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

public interface ITypeBuilderComponentSource<T> : ITypeBuilderComponentSource where T : StyledTypeBuilder
{
    IStyleTypeBuilderComponentAccess<T> CompAccess { get; }
}

public static class StyledTypeBuilderExtensions
{
    internal const string Null = "null";

    public static IStringBuilder AddIndents(this IStringBuilder sb, string indentString, int indentLevel)
    {
        for (var i = 0; i < indentLevel; i++) sb.Append(indentString);
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

    public static IStringBuilder AppendOrNull(this IStringBuilder sb, string? value, bool inQuotes = false) =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendOrNull<T>(this IStringBuilder sb, T? value, bool inQuotes = false) where T : ISpanFormattable =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendOrNull<T>(this IStringBuilder sb, T? value, bool inQuotes = false) where T : struct, ISpanFormattable =>
        value != null ? sb.Qt(inQuotes).Append(value.Value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendOrNull(this IStringBuilder sb, bool? value, bool inQuotes = false) =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder Append(this IStringBuilder sb, bool value, bool inQuotes = false) => sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder Append<T>(this IStringBuilder sb, T value, bool inQuotes = false) where T : struct, ISpanFormattable =>
        sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder Append(this IStringBuilder sb, string value, bool inQuotes = false) => sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder Append<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string value, bool inQuotes = false)
        where TExt : StyledTypeBuilder =>
        stb.Sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder AppendFormattedOrNull<T>
    (this IStringBuilder sb, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false) where T : ISpanFormattable =>
        value != null ? sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendFormattedOrNull<T>
    (this IStringBuilder sb, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false) where T : struct, ISpanFormattable =>
        value != null ? sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendFormatted<T>
    (this IStringBuilder sb, T value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false) where T : struct, ISpanFormattable
    {
        return sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes);
    }

    public static IStringBuilder AppendSpanFormattableClass<T>
    (this IStringBuilder sb, T value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false) where T : class, ISpanFormattable
    {
        return sb.Qt(inQuotes).AppendSpanFormattable(formatString, value).Qt(inQuotes);
    }

    public static IStringBuilder AppendFormatted
    (this IStringBuilder sb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
    {
        return sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes);
    }

    public static IStringBuilder Qt(this IStringBuilder sb, bool writeQuote) => writeQuote ? sb.Append("\"") : sb;

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stsa, string? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stsa.Sb;
        var addQuotes = inQuotes ?? stsa.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stsa, ICharSequence? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stsa.Sb;
        var addQuotes = inQuotes ?? stsa.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, ICharSequence? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
        {
            if (addQuotes) sb.Append("\"");
            sb.Qt(addQuotes).AppendFormat(formatString, value).Qt(addQuotes);
            if (addQuotes) sb.Append("\"");
        }
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , IStringBuilder? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).AppendFormat(formatString, value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , StringBuilder? value, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).AppendFormat(formatString, value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendObjectOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, object? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , char[]? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var isJson    = stb.Style.IsJson();
        var addQuotes = inQuotes ?? false;
        if (value != null)
        {
            if (!isJson)
            {
                if(addQuotes) sb.Qt(addQuotes);
                sb.Append(value);
                if(addQuotes) sb.Qt(addQuotes);
            }
            else
            {
                var hasAdded = false;
                for (int i = 0; i < value.Length; i++)
                {
                    if (hasAdded) sb.ItemNext(stb);
                    sb.Qt(true).Append(value[i]).Qt(true);
                }
            }
        }
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).AppendFormat(formatString, value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
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

    public static IStringBuilder Append<T, TBase, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, T value
      , CustomTypeStyler<TBase> styledToStringAction, bool? inQuotes = null)
        where T : class, TBase where TBase : class where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (addQuotes) sb.Append("\"");
        styledToStringAction(value, stb.OwningAppender);
        if (addQuotes) sb.Append("\"");
        return stb.Sb;
    }

    public static IStringBuilder AppendOrNull<TFmt, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TFmt? value
      , bool? inQuotes = null) where TFmt : ISpanFormattable where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? false;
        if (value != null)
        {
            var isJson    = stb.Style.IsJson();
            if (!isJson)
            {
                if(addQuotes) sb.Qt(addQuotes);
                sb.Append(value);
                if(addQuotes) sb.Qt(addQuotes);
            }
            else
            {
                var typeOfTFmt = typeof(TFmt);
                var isNumeric  = typeOfTFmt.IsNumericType();
                var isChar     = typeof(char) == typeOfTFmt;
                addQuotes |= !isNumeric || isChar;
                sb.Qt(addQuotes).Append(value).Qt(addQuotes);
            }
        }
        else
        {
            sb.Append(Null);
        }
        return stb.Sb;
    }

    public static IStringBuilder AppendOrNull<TFmtStruct, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TFmtStruct? value
      , bool? inQuotes = null) where TFmtStruct : struct, ISpanFormattable where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? false;
        if (value != null)
        {
            var isJson    = stb.Style.IsJson();
            if (!isJson)
            {
                if(addQuotes) sb.Qt(addQuotes);
                sb.Append(value);
                if(addQuotes) sb.Qt(addQuotes);
            }
            else
            {
                var typeOfTFmt = typeof(TFmtStruct);
                var isNumeric  = typeOfTFmt.IsNumericType();
                var isChar     = typeof(char) == typeOfTFmt;
                addQuotes |= !isNumeric || isChar;
                sb.Qt(addQuotes).Append(value).Qt(addQuotes);
            }
        }
        else
        {
            sb.Append(Null);
        }
        return stb.Sb;
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

    public static IStringBuilder AppendFormattedOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , TValue value, string formatString, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        var sb        = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:         sb.Append(valueBool, addQuotes); break;
                case byte valueByte:         sb.AppendFormattedOrNull(valueByte, formatString, addQuotes); break;
                case sbyte valueSByte:       sb.AppendFormattedOrNull(valueSByte, formatString, addQuotes); break;
                case char valueChar:         sb.AppendFormattedOrNull(valueChar, formatString, addQuotes); break;
                case short valueShort:       sb.AppendFormattedOrNull(valueShort, formatString, addQuotes); break;
                case ushort valueUShort:     sb.AppendFormattedOrNull(valueUShort, formatString, addQuotes); break;
                case Half valueHalfFloat:    sb.AppendFormattedOrNull(valueHalfFloat, formatString, addQuotes); break;
                case int valueInt:           sb.AppendFormattedOrNull(valueInt, formatString, addQuotes); break;
                case uint valueUInt:         sb.AppendFormattedOrNull(valueUInt, formatString, addQuotes); break;
                case nint valueUInt:         sb.AppendFormattedOrNull(valueUInt, formatString, addQuotes); break;
                case float valueFloat:       sb.AppendFormattedOrNull(valueFloat, formatString, addQuotes); break;
                case long valueLong:         sb.AppendFormattedOrNull(valueLong, formatString, addQuotes); break;
                case ulong valueULong:       sb.AppendFormattedOrNull(valueULong, formatString, addQuotes); break;
                case double valueDouble:     sb.AppendFormattedOrNull(valueDouble, formatString, addQuotes); break;
                case decimal valueDecimal:   sb.AppendFormattedOrNull(valueDecimal, formatString, addQuotes); break;
                case Int128 veryLongInt:     sb.AppendFormattedOrNull(veryLongInt, formatString, addQuotes); break;
                case UInt128 veryLongUInt:   sb.AppendFormattedOrNull(veryLongUInt, formatString, addQuotes); break;
                case BigInteger veryLongInt: sb.AppendFormattedOrNull(veryLongInt, formatString, addQuotes); break;
                case Complex veryLongInt:    sb.AppendFormattedOrNull(veryLongInt, formatString, addQuotes); break;
                case DateTime valueDateTime: sb.AppendFormattedOrNull(valueDateTime, formatString, addQuotes); break;
                case DateOnly valueDateOnly: sb.AppendFormattedOrNull(valueDateOnly, formatString, addQuotes); break;
                case TimeSpan valueTimeSpan: sb.AppendFormattedOrNull(valueTimeSpan, formatString, addQuotes); break;
                case TimeOnly valueTimeSpan: sb.AppendFormattedOrNull(valueTimeSpan, formatString, addQuotes); break;
                case Rune valueTimeSpan:     sb.AppendFormattedOrNull(valueTimeSpan, formatString, addQuotes); break;
                case Guid valueGuid:         sb.AppendFormattedOrNull(valueGuid, formatString, addQuotes); break;
                case IPNetwork valueIntPtr:  sb.AppendFormattedOrNull(valueIntPtr, formatString, addQuotes); break;
                case char[] valueCharArray:  stb.AppendOrNull(valueCharArray, addQuotes); break;
                case string valueString:     sb.AppendFormat(formatString, valueString); break;
                case Version valueGuid:      sb.AppendSpanFormattableClass(valueGuid, formatString, addQuotes); break;
                case IPAddress valueIntPtr:  sb.AppendSpanFormattableClass(valueIntPtr, formatString, addQuotes); break;
                case Uri valueUri:           sb.AppendSpanFormattableClass(valueUri, formatString, addQuotes); break;

                case IFrozenString valueFrozenString:   stb.AppendFormattedOrNull(valueFrozenString, formatString, addQuotes); break;
                case IStringBuilder valueStringBuilder: stb.AppendFormattedOrNull(valueStringBuilder, formatString, addQuotes); break;
                case StringBuilder valueSb:             stb.AppendFormattedOrNull(valueSb, formatString, addQuotes); break;

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

                default: sb.Qt(addQuotes).Append(value).Qt(addQuotes); break;
            }
        else
            sb.Append(Null);
        return sb;
    }


    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, IStyledToStringObject? value
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        inQuotes |= stb.Style.IsJson();
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
        {
            if (addQuotes) sb.Append("\"");
            value.ToString(stb.OwningAppender);
            if (addQuotes) sb.Append("\"");
        }
        else
        {
            sb.Append(Null);
        }
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, ICharSequence? value
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, string? value, int startIndex, int length, string? formatString
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value, startIndex, length).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, char[]? value, int startIndex, int length, string? formatString
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value, startIndex, length, formatString).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, ICharSequence? value, int startIndex, int length
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value, startIndex, length).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, StringBuilder? value, int startIndex, int length
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value, startIndex, length).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static IStringBuilder AppendNullOrValue<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TValue value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb = stb.Sb;
        sb.AddNullOrValue(value, stb, inQuotes);
        return sb;
    }

    public static TExt AddNullOrValue<TValue, TExt>(this IStringBuilder sb, TValue value
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            switch (value)
            {
                case bool valueBool:         sb.Append(valueBool, addQuotes); break;
                case byte valueByte:         sb.Append(valueByte, addQuotes); break;
                case sbyte valueSByte:       sb.Append(valueSByte, addQuotes); break;
                case char valueChar:         sb.Append(valueChar, addQuotes); break;
                case short valueShort:       sb.Append(valueShort, addQuotes); break;
                case ushort valueUShort:     sb.Append(valueUShort, addQuotes); break;
                case Half valueHalfFloat:    sb.Append(valueHalfFloat, addQuotes); break;
                case int valueInt:           sb.Append(valueInt, addQuotes); break;
                case uint valueUInt:         sb.Append(valueUInt, addQuotes); break;
                case nint valueUInt:         sb.Append(valueUInt, addQuotes); break;
                case float valueFloat:       sb.Append(valueFloat, addQuotes); break;
                case long valueLong:         sb.Append(valueLong, addQuotes); break;
                case ulong valueULong:       sb.Append(valueULong, addQuotes); break;
                case double valueDouble:     sb.Append(valueDouble, addQuotes); break;
                case decimal valueDecimal:   sb.Append(valueDecimal, addQuotes); break;
                case Int128 veryLongInt:     sb.Append(veryLongInt, addQuotes); break;
                case UInt128 veryLongUInt:   sb.Append(veryLongUInt, addQuotes); break;
                case BigInteger veryLongInt: sb.Append(veryLongInt, addQuotes); break;
                case Complex veryLongInt:    sb.Append(veryLongInt, addQuotes); break;
                case DateTime valueDateTime: sb.Append(valueDateTime, addQuotes); break;
                case DateOnly valueDateOnly: sb.Append(valueDateOnly, addQuotes); break;
                case TimeSpan valueTimeSpan: sb.Append(valueTimeSpan, addQuotes); break;
                case TimeOnly valueTimeSpan: sb.Append(valueTimeSpan, addQuotes); break;
                case Rune valueTimeSpan:     sb.Append(valueTimeSpan, addQuotes); break;
                case Guid valueGuid:         sb.Append(valueGuid, addQuotes); break;
                case IPNetwork valueIntPtr:  sb.Append(valueIntPtr, addQuotes); break;
                case char[] valueCharArray:  stb.AppendOrNull(valueCharArray, addQuotes); break;
                case string valueString:     stb.AppendOrNull(valueString, addQuotes); break;
                case Version valueGuid:      sb.AppendSpanFormattableClass(valueGuid, "{0}", addQuotes); break;
                case IPAddress valueIntPtr:  sb.AppendSpanFormattableClass(valueIntPtr, "{0}", addQuotes); break;
                case Uri valueUri:           sb.AppendSpanFormattableClass(valueUri, "{0}", addQuotes); break;

                case IFrozenString valueFrozenString:  stb.AppendOrNull(valueFrozenString, addQuotes); break;
                case IStringBuilder valueFrozenString: stb.AppendOrNull(valueFrozenString, addQuotes); break;
                case StringBuilder valueFrozenString:  stb.AppendOrNull(valueFrozenString, addQuotes); break;

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
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll(orderedCollectionBuilder, value!);
                    orderedCollectionBuilder.Complete();
                    break;

                default: sb.Qt(addQuotes).Append(value).Qt(addQuotes); break;
            }
        else
            sb.Append(Null);
        return stb.StyleTypeBuilder;
    }

    public static void StartCollection<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        stb.Sb.Append("[");
        if (stb.Style.IsPretty())
        {
            stb.IncrementIndent();
            stb.Sb.Append(stb.OwningAppender.NewLineStyle).AddIndents(stb.OwningAppender.Indent, stb.IndentLevel);
        }
    }

    public static IStringBuilder FieldNameJoin<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string fieldName)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty()) stb.Sb.Append(stb.OwningAppender.Indent);

        if (stb.Style.IsJson())
            stb.Sb.Append("\"").Append(fieldName).Append("\"").FieldEnd(stb);
        else
            stb.Sb.Append(fieldName).FieldEnd(stb);

        return stb.Sb;
    }

    public static IStyleTypeBuilderComponentAccess<TExt> FieldNameJoin<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string fieldName
      , IStyleTypeBuilderComponentAccess<TExt> toReturn)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty()) stb.Sb.Append(stb.OwningAppender.Indent);

        if (stb.Style.IsJson())
            stb.Sb.Append("\"").Append(fieldName).Append("\"").FieldEnd(stb);
        else
            stb.Sb.Append(fieldName).FieldEnd(stb);

        return toReturn;
    }

    public static IStringBuilder FieldEnd(this IStringBuilder sb, IStyleTypeBuilderComponentAccess stb) =>
        sb.Append(stb.Style.IsCompact() ? ":" : ": ");

    public static void GoToNextCollectionItemStart<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty())
            stb.Sb.ItemNext(stb).Append(stb.OwningAppender.NewLineStyle).AddIndents(stb.OwningAppender.Indent, stb.IndentLevel);
        else
            stb.Sb.ItemNext(stb);
    }

    public static IStringBuilder ItemNext(this IStringBuilder sb, IStyleTypeBuilderComponentAccess stb) =>
        sb.Append(stb.Style.IsCompact() ? "," : ", ");


    public static void EndCollection<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        stb.RemoveLastWhiteSpacedCommaIfFound();
        stb.Sb.Append("]");
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
            stb.Sb.Append(stb.OwningAppender.NewLineStyle).AddIndents(stb.OwningAppender.Indent, stb.IndentLevel);
        }
    }

    public static void EndDictionary<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        stb.RemoveLastWhiteSpacedCommaIfFound();
        stb.Sb.Append("}");
    }
}
